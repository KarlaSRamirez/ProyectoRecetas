namespace ProyectoRecetas.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }
    private async void OnNavigateButtonClicked(object sender, EventArgs e)
    {
        // Agrega la nueva p�gina a la pila
        await Navigation.PushAsync(new RecetasPage());
    }
}
