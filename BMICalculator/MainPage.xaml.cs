namespace BMICalculator;

public partial class MainPage : ContentPage
{
    private int _age;
    private double _weightKg;
    private double _heightCm = 50;
    private bool _isFemale;
    private bool _weightInLbs;
    private bool _heightInFeet;

    public MainPage()
    {
        InitializeComponent();
        WeightUnitPicker.SelectedIndex = 0;
        HeightUnitPicker.SelectedIndex = 0;
    }

    private void OnAgeTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (int.TryParse(e.NewTextValue, out int age))
        {
            _age = age;
        }
    }

    private void OnWeightTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (double.TryParse(e.NewTextValue, out double value))
        {
            _weightKg = _weightInLbs ? value * 0.453592 : value;
        }
    }

    private void OnHeightChanged(object? sender, ValueChangedEventArgs e)
    {
        _heightCm = _heightInFeet ? e.NewValue * 2.54 : e.NewValue;
        HeightLabel.Text = _heightCm.ToString("F1");
    }

    private void OnGenderToggle(object? sender, EventArgs e)
    {
        _isFemale = !_isFemale;

        if (_isFemale)
        {
            GenderToggleButton.Text = "Female";
            GenderToggleButton.BackgroundColor = Colors.HotPink;
        }
        else
        {
            GenderToggleButton.Text = "Male";
            GenderToggleButton.BackgroundColor = Color.FromArgb("#3B4CCA");
        }
    }

    private void OnWeightUnitChanged(object? sender, EventArgs e)
    {
        _weightInLbs = WeightUnitPicker.SelectedIndex == 1;
        WeightUnitLabel.Text = _weightInLbs ? "Weight (LB)" : "Weight (KG)";
    }

    private void OnHeightUnitChanged(object? sender, EventArgs e)
    {
        _heightInFeet = HeightUnitPicker.SelectedIndex == 1;
        HeightUnitLabel.Text = _heightInFeet ? "Height (ft/in)" : "Height (CM)";
    }

    private async void OnCalculateClicked(object? sender, EventArgs e)
    {
        if (_age <= 0)
        {
            await DisplayAlertAsync("Missing Info", "Enter your age.", "OK");
            return;
        }

        if (_weightKg <= 0)
        {
            await DisplayAlertAsync("Missing Info", "Enter your weight.", "OK");
            return;
        }

        double heightMeters = _heightCm / 100;
        double bmi = _weightKg / (heightMeters * heightMeters);

        BmiWhole.Text = bmi.ToString("F1");

        CategoryLabel.Text =
            bmi < 18.5 ? "UNDERWEIGHT" :
            bmi < 25 ? "NORMAL BMI" :
            bmi < 30 ? "OVERWEIGHT" :
            "OBESE";

        InputPage.IsVisible = false;
        ResultsPage.IsVisible = true;
    }

    private void OnRecalculateClicked(object? sender, EventArgs e)
    {
        ResultsPage.IsVisible = false;
        InputPage.IsVisible = true;
    }
}