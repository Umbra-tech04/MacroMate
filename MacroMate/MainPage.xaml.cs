namespace MacroMate;

public partial class MainPage : ContentPage
{
    private double _bmr;
    private double _dailyBase;
    private double _adjustment;
    private double _weight;

    public MainPage()
    {
        InitializeComponent();
    }

    private void OnCalculateClicked(object sender, EventArgs e)
    {
        if (!double.TryParse(AgeEntry.Text, out double age) ||
            !double.TryParse(HeightEntry.Text, out double height) ||
            !double.TryParse(WeightEntry.Text, out _weight) ||
            GenderPicker.SelectedIndex == -1)
        {
            DisplayAlert("Error", "Please fill in all fields!", "OK");
            return;
        }

        if (GenderPicker.SelectedIndex == 0)
            _bmr = (10 * _weight) + (6.25 * height) - (5 * age) + 5;
        else
            _bmr = (10 * _weight) + (6.25 * height) - (5 * age) - 161;

        _dailyBase = _bmr * 1.2;

        BmrLabel.Text = $"BMR: {_bmr:F0} kcal";
        BaseLabel.Text = $"Daily Base: {_dailyBase:F0} kcal";
        BmrRow.IsVisible = true;
        BaseRow.IsVisible = true;
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

    private async void OnBaseInfoTapped(object sender, EventArgs e)
    {
        await DisplayAlert("Daily Base", "Your BMR × 1.2 — calories burned on a rest day with minimal movement (walking, daily tasks). Exercise is added on top by the tracker.", "OK");
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

        double[][] adjustments =
        {
            new double[] { -200, -400, -600 }, // Cut
            new double[] { -100, -200, -300 }, // Recomp
            new double[] {    0,    0,    0 }, // Maintain
            new double[] {  150,  300,  500 }, // Bulk
        };

        _adjustment = adjustments[goal][level - 1];
        double restDay = _dailyBase + _adjustment;

        // Protein alap goal szerint
        double[] proteinBase = { 2.2, 2.2, 1.8, 1.8 }; // Cut, Recomp, Maintain, Bulk
        double[] aggressivenessModifier = { -0.2, 0, 0.2 }; // Mild, Moderate, Aggressive

        double proteinPerKg = proteinBase[goal] + aggressivenessModifier[level - 1];
        double protein = Math.Round(proteinPerKg * _weight);
        double minProtein = Math.Round(1.6 * _weight);

        // Fat - 25% a kalóriából, minimum 0.5g/kg
        double fat = Math.Round(Math.Max((restDay * 0.25) / 9, 0.5 * _weight));

        // Carbs - maradék
        double proteinKcal = protein * 4;
        double fatKcal = fat * 9;
        double carbs = Math.Round(Math.Max((restDay - proteinKcal - fatKcal) / 4, 0));

        ResultLabel.Text =
            $"🛌 Rest day: {restDay:F0} kcal\n" +
            $"🏋️ Training day: {restDay:F0} kcal + workout (tracked daily)\n" +
            $"{(_adjustment < 0 ? "📉 Deficit" : _adjustment > 0 ? "📈 Surplus" : "⚖️ Maintain")}: {_adjustment:F0} kcal/day\n\n" +
            $"🥩 Protein: {protein:F0}g (min. {minProtein:F0}g)\n" +
            $"🧈 Fat: {fat:F0}g\n" +
            $"🌾 Carbs: {carbs:F0}g";

        ResultLabel.IsVisible = true;
    }
}