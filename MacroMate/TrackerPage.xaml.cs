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
        var card = new Border
        {
            Stroke = Colors.Gray,
            StrokeThickness = 1,
            StrokeShape = new Microsoft.Maui.Controls.Shapes.Rectangle(),
            BackgroundColor = Color.FromArgb("#1a1a1a"),
            Padding = new Thickness(12, 8),
            Margin = new Thickness(0, 0, 0, 8),
            Content = new VerticalStackLayout
            {
                Spacing = 4,
                Children =
            {
                new Label { Text = meal.type, FontSize = 11, TextColor = Colors.Gray },
                new Label { Text = meal.description, FontSize = 14, TextColor = Colors.White, FontAttributes = FontAttributes.Bold },
                new Label { Text = $"{meal.calories} kcal  |  P: {meal.protein}g  F: {meal.fat}g  C: {meal.carb}g", FontSize = 12, TextColor = Colors.Gray }
            }
            }
        };

        MealsList.Children.Add(card);
    }
}