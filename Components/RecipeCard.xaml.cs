using ProyectoRecetas.Views;

namespace ProyectoRecetas.Components;

public partial class RecipeCard : ContentView
{

	private string _title;
	private string _instructions;
	private string _ingredients;
	public RecipeCard(string name, string instruction, string ingredients)
	{
		InitializeComponent();

		_title = name;
		_instructions = instruction;
		_ingredients = ingredients;

		RecipeTitle.Text = name;
		RecipeIngredients.Text = ingredients;
	}

	private async void CookButton(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new RealizarRecetaPage(_title, _instructions, _ingredients));
	}
}