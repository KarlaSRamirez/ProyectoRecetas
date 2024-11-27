namespace ProyectoRecetas.Views;
using MySqlConnector;
using ProyectoRecetas.Components;

public partial class IngredientPage : ContentPage
{
    public IngredientPage()
    {
        InitializeComponent();
        LoadIngredients();
    }

    // Definición de la clase Ingredient
    public class Ingredient
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
        public DateTime Expiration { get; set; }
    }

    private async void LoadIngredients()
    {
        var ingredientsList = new List<Ingredient>();

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

            // Consulta para obtener todos los ingredientes
            string query = "SELECT Nombre, Descripcion, Cantidad, Unidad_de_Medida, Caducidad FROM ingredientes";
            using var command = new MySqlCommand(query, connection);

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
