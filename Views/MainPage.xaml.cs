namespace ProyectoRecetas.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private void OnStartButtonClicked(object sender, EventArgs e)
    {
        DisplayAlert("¡Bienvenido!", "Esperamos que disfrutes de nuestras recetas.", "OK");
    }

    private async void OnNavigateButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RecetasPage());
    }
}
