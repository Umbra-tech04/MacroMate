namespace MacroMate;

public partial class MainPage : ContentPage
{
    private double _tdee;

    public MainPage()
    {
        InitializeComponent();
    }

    private void OnCalculateClicked(object sender, EventArgs e)
    {
        if (!double.TryParse(AgeEntry.Text, out double age) ||
            !double.TryParse(HeightEntry.Text, out double height) ||
            !double.TryParse(WeightEntry.Text, out double weight) ||
            GenderPicker.SelectedIndex == -1 ||
            ActivityPicker.SelectedIndex == -1)
        {
            DisplayAlert("Error", "Please fill in all fields!", "OK");
            return;
        }

        double bmr;

        if (GenderPicker.SelectedIndex == 0)
        {
            bmr = (10 * weight) + (6.25 * height) - (5 * age) + 5;
        }
        else
        {
            bmr = (10 * weight) + (6.25 * height) - (5 * age) - 161;
        }

        double[] multipliers = { 1.2, 1.375, 1.55, 1.725, 1.9 };
        _tdee = bmr * multipliers[ActivityPicker.SelectedIndex];

        BmrLabel.Text = $"BMR: {bmr:F0} kcal";
        TdeeLabel.Text = $"TDEE: {_tdee:F0} kcal";
        BmrRow.IsVisible = true;
        TdeeRow.IsVisible = true;
        GoalLabel.IsVisible = true;
        GoalPicker.IsVisible = true;
        AggressivenessLabel.IsVisible = true;
        AggressivenessSlider.IsVisible = true;
        AggressivenessDescription.IsVisible = true;
        ResultBtn.IsVisible = true;
    }

    private async void OnBmrInfoTapped(object sender, EventArgs e)
    {
        await DisplayAlert("BMR", "Basal Metabolic Rate - the calories your body burns at complete rest. Breathing, heartbeat, digestion.", "OK");
    }

    private async void OnTdeeInfoTapped(object sender, EventArgs e)
    {
        await DisplayAlert("TDEE", "Total Daily Energy Expenditure - your BMR multiplied by your activity level. This is how many calories you burn per day in total.", "OK");
    }

    private void OnSliderChanged(object sender, ValueChangedEventArgs e)
    {
        int level = (int)Math.Round(AggressivenessSlider.Value);

        switch (level)
        {
            case 1:
                AggressivenessLabel.Text = "Aggressiveness: Mild";
                AggressivenessDescription.Text = "Small deficit/surplus. Slower results, easier to maintain.";
                break;
            case 2:
                AggressivenessLabel.Text = "Aggressiveness: Moderate";
                AggressivenessDescription.Text = "Balanced approach. Recommended for most people.";
                break;
            case 3:
                AggressivenessLabel.Text = "Aggressiveness: Aggressive";
                AggressivenessDescription.Text = "Large deficit/surplus. Faster results but harder to sustain.";
                break;
        }
    }

    private void OnResultClicked(object sender, EventArgs e)
    {
        int goal = GoalPicker.SelectedIndex;
        int level = (int)Math.Round(AggressivenessSlider.Value);

        if (goal == -1)
        {
            DisplayAlert("Error", "Please select a goal!", "OK");
            return;
        }

        double[] cutDeficits = { -200, -400, -600 };
        double[] bulkSurpluses = { 150, 300, 500 };
        double[] recompDeficits = { -100, -200, -300 };

        double target = goal switch
        {
            0 => _tdee + cutDeficits[level - 1],
            1 => _tdee + recompDeficits[level - 1],
            2 => _tdee,
            3 => _tdee + bulkSurpluses[level - 1],
            _ => _tdee
        };

        string goalName = GoalPicker.Items[goal];
        ResultLabel.Text = $"{goalName}: {target:F0} kcal/day";
        ResultLabel.IsVisible = true;
    }
}