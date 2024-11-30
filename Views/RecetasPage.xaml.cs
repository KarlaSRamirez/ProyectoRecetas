using MySqlConnector;
using ProyectoRecetas.Components;

namespace ProyectoRecetas.Views

{
    public partial class RecetasPage : ContentPage
    {
        public RecetasPage()
        {
            InitializeComponent();
            LoadRecipes();
        }

        public class Recipe
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Instruction { get; set; }
            public string Ingredients { get; set; }
        }

        private async void LoadRecipes()
        {
            var recipesList = new List<Recipe>();

            // Crear la cadena de conexión
            var builder = new MySqlConnectionStringBuilder
            {
                Server = "192.168.247.1",     // Dirección del servidor de la base de datos
                UserID = "root",              // Usuario
                Password = "11julio2002",     // Contraseña
                Database = "RecetasBD"        // Nombre de la base de datos
            };

            try
            {
                using var connection = new MySqlConnection(builder.ConnectionString);
                await connection.OpenAsync();
                // Consulta para obtener todos las recetas
                string query = "SELECT * FROM Recetas ORDER BY RAND() LIMIT 10";
                using var command = new MySqlCommand(query, connection);

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    // Crear una nueva receta y agregarlo a la lista
                    var recipe = new Recipe
                    {
                        Id = reader.GetInt32("ID_Receta"),
                        Name = reader.GetString("Nombre"),
                        Instruction = reader.GetString("Instrucciones"),
                        Ingredients = reader.GetString("Ingredientes")
                    };

                    recipesList.Add(recipe);
                }

                // Ahora agregamos las tarjetas de ingredientes al StackLayout
                foreach (var recipe in recipesList)
                {
                    RecipeCard ingredientCard = new RecipeCard
                    (
                        recipe.Id,
                        recipe.Name,
                        recipe.Instruction,
                        recipe.Ingredients
                    );

                    // Agregar la tarjeta al StackLayout dinámicamente
                    RecetasContainer.Children.Add(ingredientCard);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al conectar: {ex.Message}");
            }
        }
        private async void OnNavigateButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new IngredientPage());
        }

        private async void OnNavigateButtonClicked1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HistoryPage());
        }
        private async void OnAddButton(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddNewPage());
        }
    }
}
