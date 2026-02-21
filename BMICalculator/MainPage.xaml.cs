namespace BMICalculator;
public partial class MainPage : ContentPage
{
    private int _age = 0;
    private double _weightKg = 0;
    private double _heightCm = 50;
    private bool _isFemale = false;
    private bool _weightInLbs = false;
    private bool _heightInFeet = false;
    public MainPage()
    {
        InitializeComponent();
        WeightUnitPicker.SelectedIndex = 0;
        HeightUnitPicker.SelectedIndex = 0;
        UpdateLabels();
    }
    private void OnAgeIncrement(object? sender, EventArgs e)
    {
        _age++;
        UpdateLabels();
    }
    private void OnAgeDecrement(object? sender, EventArgs e)
    {
        if (_age > 0) { _age--; UpdateLabels(); }
    }
    private void OnWeightIncrement(object? sender, EventArgs e)
    {
        _weightKg += _weightInLbs ? 0.453592 : 1;
        UpdateLabels();
    }
    private void OnWeightDecrement(object? sender, EventArgs e)
    {
        double step = _weightInLbs ? 0.453592 : 1;
        if (_weightKg - step >= 0) { _weightKg -= step; UpdateLabels(); }
    }
    private void OnHeightChanged(object? sender, ValueChangedEventArgs e)
    {
        _heightCm = _heightInFeet ? e.NewValue * 2.54 : e.NewValue;
        UpdateLabels();
    }
    private void OnGenderToggle(object? sender, EventArgs e)
    {
        _isFemale = !_isFemale;

        if (_isFemale)
        {
            GenderToggleButton.BackgroundColor = Color.FromArgb("#cc40cc");
            MaleLabel.TextColor = Color.FromArgb("#9090b0");
            MaleLabel.FontAttributes = FontAttributes.None;
            FemaleLabel.TextColor = Color.FromArgb("#1a1a6e");
            FemaleLabel.FontAttributes = FontAttributes.Bold;
            GenderValueLabel.Text = "Female";
            GenderValueLabel.TextColor = Color.FromArgb("#cc40cc");
        }
        else
        {
            GenderToggleButton.BackgroundColor = Color.FromArgb("#d0d0f0");
            MaleLabel.TextColor = Color.FromArgb("#1a1a6e");
            MaleLabel.FontAttributes = FontAttributes.Bold;
            FemaleLabel.TextColor = Color.FromArgb("#9090b0");
            FemaleLabel.FontAttributes = FontAttributes.None;
            GenderValueLabel.Text = "Male";
            GenderValueLabel.TextColor = Color.FromArgb("#6060e0");
        }
    }

    private void OnWeightUnitChanged(object? sender, EventArgs e)
    {
        _weightInLbs = WeightUnitPicker.SelectedIndex == 1;
        WeightUnitLabel.Text = _weightInLbs ? "Weight (LB)" : "Weight (KG)";
        UpdateLabels();
    }
    private void OnHeightUnitChanged(object? sender, EventArgs e)
    {
        _heightInFeet = HeightUnitPicker.SelectedIndex == 1;
        HeightUnitLabel.Text = _heightInFeet ? "Height (ft / in)" : "Height (CM)";

        if (_heightInFeet)
        {
            HeightSlider.Minimum = 20;
            HeightSlider.Maximum = 120;
            HeightSlider.Value = _heightCm / 2.54;
        }
        else
        {
            HeightSlider.Minimum = 50;
            HeightSlider.Maximum = 300;
            HeightSlider.Value = _heightCm;
        }

        UpdateLabels();
    }
    private void UpdateLabels()
    {
        AgeLabel.Text = _age.ToString();

        if (_weightInLbs)
        {
            double lbs = _weightKg * 2.20462;
            WeightLabel.Text = ((int)lbs).ToString();
        }
        else
        {
            WeightLabel.Text = ((int)_weightKg).ToString();
        }

        if (_heightInFeet)
        {
            double totalInches = _heightCm / 2.54;
            int feet = (int)(totalInches / 12);
            int inches = (int)(totalInches % 12);
            HeightLabel.Text = $"{feet}'{inches}\"";
        }
        else
        {
            HeightLabel.Text = _heightCm.ToString("F1");
        }
    }
    private void OnCalculateClicked(object? sender, EventArgs e)
    {
        if (_age <= 0)
        {
            DisplayAlert("Missing Info", "Please set your Age using the + button.", "OK");
            return;
        }
        if (_weightKg <= 0)
        {
            DisplayAlert("Missing Info", "Please set your Weight using the + button.", "OK");
            return;
        }

        double heightMeters = _heightCm / 100.0;
        double bmi = _weightKg / (heightMeters * heightMeters);

        string whole = ((int)bmi).ToString();
        string dec = "." + (bmi % 1).ToString("F2").Substring(2);

        BmiWhole.Text = whole;
        BmiDecimal.Text = dec;

        Color resultColor;
        string category;

        if (bmi < 18.5)
        {
            category = "UNDERWEIGHT";
            resultColor = Color.FromArgb("#3b82f6");
        }
        else if (bmi < 25)
        {
            category = "NORMAL BMI";
            resultColor = Color.FromArgb("#4040cc");
        }
        else if (bmi < 30)
        {
            category = "OVERWEIGHT";
            resultColor = Color.FromArgb("#f59e0b");
        }
        else
        {
            category = "OBESE";
            resultColor = Color.FromArgb("#ef4444");
        }

        CategoryLabel.Text = category;
        CategoryLabel.TextColor = resultColor;
        BmiWhole.TextColor = resultColor;
        BmiDecimal.TextColor = resultColor;

        InputPage.IsVisible = false;
        ResultsPage.IsVisible = true;
    }

    private void OnRecalculateClicked(object? sender, EventArgs e)
    {
        ResultsPage.IsVisible = false;
        InputPage.IsVisible = true;
    }
}
