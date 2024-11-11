namespace ProyectoRecetas.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }
    private async void OnNavigateButtonClicked(object sender, EventArgs e)
    {
        // Lógica para navegar a la siguiente página
        await Navigation.PushAsync(new IngredientPage());
    }
}
