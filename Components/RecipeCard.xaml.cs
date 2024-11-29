namespace ProyectoRecetas.Components;

public partial class RecipeCard : ContentView
{
	public RecipeCard(string name, string instruction, string ingredients)
	{
		InitializeComponent();

		RecipeTitle.Text = name;
		RecipeIngredients.Text = ingredients;
	}

	private async void CookButton(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new RealizarRecetaPage());
	}
}