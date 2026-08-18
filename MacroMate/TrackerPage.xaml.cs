namespace MacroMate;

public partial class TrackerPage : ContentPage
{
    private DateTime _currentDate = DateTime.Today;

    public TrackerPage()
    {
        InitializeComponent();
        DataLabel.Text = _currentDate.ToString("MMMM dd, yyyy");
    }

    private void MoveLeft_Clicked(object sender, EventArgs e)
    {
        _currentDate = _currentDate.AddDays(-1);
        DataLabel.Text = _currentDate.ToString("MMMM dd, yyyy");
    }

    private void MoveRight_Clicked(object sender, EventArgs e)
    {
        _currentDate = _currentDate.AddDays(1);
        DataLabel.Text = _currentDate.ToString("MMMM dd, yyyy");
    }

    private void MealsBtn_Clicked(object sender, EventArgs e)
    {
        MealsContent.IsVisible = true;
        WorkoutContent.IsVisible = false;
    }

    private void WorkoutBtn_Clicked(object sender, EventArgs e)
    {
        MealsContent.IsVisible = false;
        WorkoutContent.IsVisible = true;
    }

    private async void AddMealBtn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new AddMealPage(OnMealSaved));
    }

    private void OnMealSaved(Meal meal)
    {
        var label = new Label
        {
            Text = $"{meal.type}: {meal.description} | {meal.calories} kcal | P: {meal.protein}g | F: {meal.fat}g | C: {meal.carb}g"
        };
        MealsList.Children.Add(label);
    }
}