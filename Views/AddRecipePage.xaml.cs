namespace ProyectoRecetas.Views;
using MySqlConnector;

public partial class AddRecipePage : ContentPage
{
	public AddRecipePage()
	{
		InitializeComponent();
	}

    private void OnEditorTextChanged(object sender, TextChangedEventArgs e)
    {
        var editor = sender as Editor;

        if (editor.Text.Length > 100)
        {
            // Truncar el texto al l�mite m�ximo permitido
            editor.Text = editor.Text.Substring(0, 100);
        }
    }


    private async void OnIngredientAdded(object sender, EventArgs e)
    {
        var name = Nombre.Text;
        var description = Descripcion.Text;
        var quantity = Cantidad.Text;
        var unit = Unidad_de_Medida.Text;
        var expiration = Caducidad.Text;

        // Crear la cadena de conexi�n
        var builder = new MySqlConnectionStringBuilder
        {
            // IP Joska 247.1
            // IP Sugey 0.249
            Server = "192.168.247.1",     // Direcci�n del servidor de la base de datos
            UserID = "root",          // Usuario
            Password = "11julio2002",  // Contrase�a del usuario
            Database = "RecetasBD"    // Nombre de la base de datos
        };

        try
        {
            // Crear la conexi�n y abrirla de manera asincr�nica
            using var connection = new MySqlConnection(builder.ConnectionString);
            await connection.OpenAsync();

            // Consulta para verificar las credenciales del usuario
            string query = "INSERT INTO ingredientes (Nombre, Descripcion, Cantidad, Unidad_de_Medida, Caducidad) VALUES (@name, @description, @quantity, @unit, @expiration)";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@description", description);
            command.Parameters.AddWithValue("@quantity", quantity);
            command.Parameters.AddWithValue("@unit", unit);
            command.Parameters.AddWithValue("@expiration", expiration);

            await command.ExecuteNonQueryAsync();

            await DisplayAlert("�xito", "Se ha agregado un nuevo ingrediente.", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al conectar: {ex.Message}");
            // Mostrar el mensaje de error en la interfaz de usuario, por ejemplo:
            await DisplayAlert("Error", $"No se pudo conectar a la base de datos: {ex.Message}", "OK");
        }
    }
}