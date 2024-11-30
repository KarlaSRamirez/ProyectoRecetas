using MySqlConnector;

namespace ProyectoRecetas.Views;

public partial class AddNewPage : ContentPage
{
	public AddNewPage()
	{
		InitializeComponent();
	}

	private async void OnRecipeAdded(object sender, EventArgs e)
	{
        var name = NombreReceta.Text;
        var instructions = Instrucciones.Text;
        var ingredients = Ingredientes.Text;

        // Crear la cadena de conexión
        var builder = DatabaseConfig.GetConnectionStringBuilder();

        try
        {
            // Crear la conexión y abrirla de manera asincrónica
            using var connection = new MySqlConnection(builder.ConnectionString);
            await connection.OpenAsync();

            // Consulta para verificar las credenciales del usuario
            string query = "INSERT INTO recetas (Nombre, Instrucciones, Ingredientes) VALUES (@name, @instructions, @ingredients)";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@instructions", instructions);
            command.Parameters.AddWithValue("@ingredients", ingredients);
            await command.ExecuteNonQueryAsync();

            await DisplayAlert("Éxito", "Se ha agregado una nueva receta.", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al conectar: {ex.Message}");
            // Mostrar el mensaje de error en la interfaz de usuario, por ejemplo:
            await DisplayAlert("Error", $"No se pudo conectar a la base de datos: {ex.Message}", "OK");
        }
    }
    private void OnEditorTextChanged1(object sender, TextChangedEventArgs e)
    {
        var editor = sender as Editor;

        if (editor.Text.Length > 255)
        {
            // Truncar el texto al límite máximo permitido
            editor.Text = editor.Text.Substring(0, 255);
        }
    }

    private void OnEditorTextChanged2(object sender, TextChangedEventArgs e)
    {
        var editor = sender as Editor;

        if (editor.Text.Length > 255)
        {
            // Truncar el texto al límite máximo permitido
            editor.Text = editor.Text.Substring(0, 255);
        }
    }
}