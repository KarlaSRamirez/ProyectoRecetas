namespace ProyectoRecetas.Views;
using MySqlConnector;

public partial class SignPage : ContentPage
{
	public SignPage()
	{
		InitializeComponent();
	}

	private async void OnCreateButton(object sender, EventArgs e)
	{
		var name = Name.Text;
		var password = Password.Text;
		var passwordTwice = PasswordTwice.Text;

        if (password == passwordTwice)
        {
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

                // Creacion de Usuario
                string query = "INSERT INTO Usuarios (Nombre, Contraseña) VALUES (@name, @password)";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@password", password);

                await command.ExecuteNonQueryAsync();

                await DisplayAlert("Éxito", "Se ha creado el usuario correctamente.", "OK");
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al conectar: {ex.Message}");
                // Mostrar el mensaje de error en la interfaz de usuario, por ejemplo:
                await DisplayAlert("Error", $"No se pudo conectar a la base de datos: {ex.Message}", "OK");
            }
        } else
        {
            await DisplayAlert("Error", "La contraseña no coincide.", "OK");
        }
    }
}