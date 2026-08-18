namespace MacroMate;

public partial class AddMealPage : ContentPage
{
    private Action<Meal> _onSave;

    public AddMealPage(Action<Meal> onSave)
    {
        InitializeComponent();
        _onSave = onSave;
    }

    private async void Savebtn_Clicked(object sender, EventArgs e)
    {
        if (MealTypePicker.SelectedIndex == -1 ||
            string.IsNullOrWhiteSpace(DescriptionEntry.Text) ||
            !double.TryParse(CaloriesEntry.Text, out double calories) ||
            !double.TryParse(ProteinEntry.Text, out double protein) ||
            !double.TryParse(FatEntry.Text, out double fat) ||
            !double.TryParse(CarbsEntry.Text, out double carbs))
        {
            await DisplayAlert("Error", "Please fill in all fields!", "OK");
            return;
        }

        var meal = new Meal(
            MealTypePicker.Items[MealTypePicker.SelectedIndex],
            DescriptionEntry.Text,
            calories,
            protein,
            fat,
            carbs
        );

        _onSave(meal);
        await Navigation.PopModalAsync();
    }

    private async void Cancelbtn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}