using MySqlConnector;
using System.Text.RegularExpressions;
namespace ProyectoRecetas.Views;

public partial class RealizarRecetaPage : ContentPage
{
    private string _ingredients;
    private int _id;

    public RealizarRecetaPage(int id, string name, string instruction, string ingredients)
    {
        InitializeComponent();

        _ingredients = ingredients;
        _id = id;

        NombreReceta.Text = name;
        InstruccionesReceta.Text = instruction;
        IngredientesReceta.Text = ingredients;
    }

    private async void HechoButton(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
