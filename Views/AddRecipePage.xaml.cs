namespace ProyectoRecetas.Views;
using MySqlConnector;

public partial class AddRecipePage : ContentPage
{
	public AddRecipePage()
	{
		InitializeComponent();
        LoadIngredients();
        LoadUnits();
	}
    private void LoadIngredients()
    {
        // Aquí puedes obtener los ingredientes de una base de datos o una lista predeterminada
        var ingredients = new List<string>
            {
                "Tomate","Lechuga","Cebolla","Pimiento","Ajo"
            };

        // Asignar los ingredientes al Picker
        IngredientPicker.ItemsSource = ingredients;
    }

    private void OnIngredientSelected(object sender, EventArgs e)
    {
        var selectedIngredient = IngredientPicker.SelectedItem as string;
    }

    private void LoadUnits()
    {
        var units = new List<string>
        {
            "gr","Kg","L","ml","piezas","galon"
        };
        UnitPicker.ItemsSource = units;
    }

    private void OnUnitSelected(object sender, EventArgs e)
    {
        var selectedUnit = UnitPicker.SelectedItem as string;
    }
    private void OnCantidadTextChanged(object sender, TextChangedEventArgs e)
    {
        var entry = sender as Entry;

        // Verificar si el texto ingresado es un número válido
        if (!string.IsNullOrEmpty(entry.Text) && !IsNumeric(entry.Text))
        {
            // Si el texto no es numérico, lo revertimos al valor anterior
            entry.Text = e.OldTextValue;
        }
    }
    private bool IsNumeric(string text)
    {
        return decimal.TryParse(text, out _);
    }
    private void OnDateSelected(object sender, DateChangedEventArgs e)
    {
        // Capturar la fecha seleccionada
        var selectedDate = e.NewDate;
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


    private async void OnIngredientAdded(object sender, EventArgs e)
    {
        var name = IngredientPicker.SelectedItem as string;
        var description = Descripcion.Text;
        var quantity = Cantidad.Text;
        var unit = UnitPicker.SelectedItem as string;
        var expiration = datePicker.Date.ToString("yyyy-MM-dd");

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
            string query = "INSERT INTO ingredientes (Nombre, Descripcion, Cantidad, Unidad_de_Medida, Caducidad) VALUES (@name, @description, @quantity, @unit, @expiration)";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@description", description);
            command.Parameters.AddWithValue("@quantity", quantity);
            command.Parameters.AddWithValue("@unit", unit);
            command.Parameters.AddWithValue("@expiration", expiration);
            await command.ExecuteNonQueryAsync();

            // Hacer relacion
            string query2 = @"INSERT INTO usuarios_ingredientes (ID_User, ID_Ingrediente) VALUES (
                                (SELECT ID_User FROM Usuarios WHERE Nombre = @usuarioNow), 
                                (SELECT ID_Ingrediente FROM Ingredientes WHERE Nombre = @name AND Descripcion = @description AND Cantidad = @quantity AND Unidad_de_Medida = @unit AND Caducidad = @expiration)
                            )";
            using var command2 = new MySqlCommand(query2, connection);
            command2.Parameters.AddWithValue("@usuarioNow", GlobalVariables.UsuarioActual);
            command2.Parameters.AddWithValue("@name", name);
            command2.Parameters.AddWithValue("@description", description);
            command2.Parameters.AddWithValue("@quantity", quantity);
            command2.Parameters.AddWithValue("@unit", unit);
            command2.Parameters.AddWithValue("@expiration", expiration);
            await command2.ExecuteNonQueryAsync();

            await DisplayAlert("Éxito", "Se ha agregado un nuevo ingrediente.", "OK");
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