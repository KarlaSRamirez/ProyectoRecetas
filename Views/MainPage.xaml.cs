namespace ProyectoRecetas.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }
    private async void OnNavigateButtonClicked(object sender, EventArgs e)
    {
        // L�gica para navegar a la siguiente p�gina
        await Navigation.PushAsync(new IngredientPage());
    }
}
