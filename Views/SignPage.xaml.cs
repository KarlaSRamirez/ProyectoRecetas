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
            var builder = DatabaseConfig.GetConnectionStringBuilder();

            try
            {
                // Crear la conexi�n y abrirla de manera asincr�nica
                using var connection = new MySqlConnection(builder.ConnectionString);
                await connection.OpenAsync();

                // Creacion de Usuario
                string query = "INSERT INTO Usuarios (Nombre, Contrase�a) VALUES (@name, @password)";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@password", password);

                await command.ExecuteNonQueryAsync();

                await DisplayAlert("�xito", "Se ha creado el usuario correctamente.", "OK");
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
            await DisplayAlert("Error", "La contrase�a no coincide.", "OK");
        }
    }
}