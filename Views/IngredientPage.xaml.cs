namespace ProyectoRecetas.Views;
using MySqlConnector;
using ProyectoRecetas.Components;

public partial class IngredientPage : ContentPage
{
    public IngredientPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Limpia las tarjetas para evitar duplicados
        ingredientCardsStackLayout.Children.Clear();

        // Recarga los ingredientes desde la base de datos
        LoadIngredients();
    }

    // Definición de la clase Ingredient
    public class Ingredient
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
    }

    private async void LoadIngredients()
    {
        var ingredientsList = new List<Ingredient>();

        // Crear la cadena de conexión
        var builder = DatabaseConfig.GetConnectionStringBuilder();

        try
        {
            using var connection = new MySqlConnection(builder.ConnectionString);
            await connection.OpenAsync();
            // Consulta para obtener todos los ingredientes
            string query = "SELECT * FROM Ingredientes WHERE ID_ingrediente IN (SELECT ID_ingrediente FROM usuarios_ingredientes WHERE ID_User = (SELECT ID_User FROM usuarios WHERE Nombre = @nombreUsuario))";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@nombreUsuario", GlobalVariables.UsuarioActual);


            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                // Crear un nuevo ingrediente y agregarlo a la lista
                var ingredient = new Ingredient
                {
                    Name = reader.GetString("Nombre"),
                    Description = reader.GetString("Descripcion"),
                    Quantity = reader.GetInt32("Cantidad"),
                    Unit = reader.GetString("Unidad_de_Medida"),
                    Expiration = reader.GetDateTime("Caducidad")
                };

                ingredientsList.Add(ingredient);
            }

            // Ahora agregamos las tarjetas de ingredientes al StackLayout
            foreach (var ingredient in ingredientsList)
            {
                IngredientCard ingredientCard = new IngredientCard
                {
                    // Establecer el BindingContext para cada IngredientCard
                    BindingContext = ingredient
                };

                // Agregar la tarjeta al StackLayout dinámicamente
                ingredientCardsStackLayout.Children.Add(ingredientCard);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al conectar: {ex.Message}");
        }
    }


    private async void OnNavigateButtonClicked(object sender, EventArgs e)
    {
        // Agrega la nueva página a la pila
        await Navigation.PushAsync(new AddRecipePage());
    }
}
