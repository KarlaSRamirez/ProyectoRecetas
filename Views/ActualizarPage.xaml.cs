using MySqlConnector;

namespace ProyectoRecetas.Views;

public partial class ActualizarPage : ContentPage
{
    private string _name;
    private string _description;
    private string _quantity;
    private string _unit;
    private string _expiration;

	public ActualizarPage(string name, string description, int quantity, string unit, DateTime expiration)
	{
		InitializeComponent();
        _name = name;
        _description = description;
        _quantity = quantity.ToString();
        _unit = unit;
        _expiration = expiration.ToString("yyyy-MM-dd");

        Nombre.Text = _name;
        Cantidad.Text = _quantity;
        Descripcion.Text = _description;
        Unidad_de_Medida.Text = _unit;
        Caducidad.Text = _expiration;
	}

    private async void ActualizarButton(object sender, EventArgs e)
    {
        var name = Nombre.Text;
        var description = Descripcion.Text;
        var quantity = Cantidad.Text;
        var unit = Unidad_de_Medida.Text;
        var expiration = Caducidad.Text;

        // Crear la cadena de conexión
        var builder = new MySqlConnectionStringBuilder
        {
            // IP Joska 247.1
            // IP Sugey 0.249
            Server = "192.168.247.1",     // Dirección del servidor de la base de datos
            UserID = "root",          // Usuario
            Password = "11julio2002",  // Contraseña del usuario
            Database = "RecetasBD"    // Nombre de la base de datos
        };

        try
        {
            // Crear la conexión y abrirla de manera asincrónica
            using var connection = new MySqlConnection(builder.ConnectionString);
            await connection.OpenAsync();

            // Consulta para verificar las credenciales del usuario
            string query = "UPDATE Ingredientes SET Nombre = @name, Descripcion = @description, Cantidad = @quantity, Unidad_de_Medida = @unit, Caducidad = @expiration WHERE ID_Ingrediente = (SELECT ID_Ingrediente FROM Ingredientes WHERE Nombre = @_name AND Descripcion = @_description AND Cantidad = @_quantity AND Unidad_de_Medida = @_unit AND Caducidad = @_expiration)";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@description", description);
            command.Parameters.AddWithValue("@quantity", quantity);
            command.Parameters.AddWithValue("@unit", unit);
            command.Parameters.AddWithValue("@expiration", expiration);
            command.Parameters.AddWithValue("@_name", _name);
            command.Parameters.AddWithValue("@_description", _description);
            command.Parameters.AddWithValue("@_quantity", _quantity);
            command.Parameters.AddWithValue("@_unit", _unit);
            command.Parameters.AddWithValue("@_expiration", _expiration);
            await command.ExecuteNonQueryAsync();

            await DisplayAlert("Éxito", "Se ha actualizado el ingrediente.", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al conectar: {ex.Message}");
            // Mostrar el mensaje de error en la interfaz de usuario, por ejemplo:
            await DisplayAlert("Error", $"No se pudo conectar a la base de datos: {ex.Message}", "OK");
        }
    }

    private void OnEditorTextChanged(object sender, TextChangedEventArgs e)
    {
        var editor = sender as Editor;

        if (editor.Text.Length > 100)
        {
            // Truncar el texto al límite máximo permitido
            editor.Text = editor.Text.Substring(0, 100);
        }
    }
}