namespace ProyectoRecetas.Views;

public partial class RealizarRecetaPage : ContentPage
{
	public RealizarRecetaPage(string name, string instruction, string ingredients)
	{
		InitializeComponent();
        NombreReceta.Text = name;
        InstruccionesReceta.Text = instruction;
        IngredientesReceta.Text = ingredients;
    }

    private async void HechoButton(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }

}