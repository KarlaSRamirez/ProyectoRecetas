namespace ProyectoRecetas.Views;
using MySqlConnector;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnNavigateButtonClicked(object sender, EventArgs e)
    {
        var username = ((StackLayout)((Button)sender).Parent).Children.OfType<Entry>().FirstOrDefault(x => x.Placeholder == "Username")?.Text;
        var password = ((StackLayout)((Button)sender).Parent).Children.OfType<Entry>().FirstOrDefault(x => x.Placeholder == "Password")?.Text;

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
            string query = "SELECT COUNT(*) FROM Usuarios WHERE Nombre = @username AND Contraseña = @password";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);

            var result = Convert.ToInt32(await command.ExecuteScalarAsync());

            if (result > 0)
            {
                // Si las credenciales son correctas, navegar a la página de recetas
                Console.WriteLine("Usuario autenticado exitosamente.");
                // Aquí puedes navegar a otra página, por ejemplo:
                await Navigation.PushAsync(new RecetasPage());
            }
            else
            {
                // Si el usuario no existe o las credenciales son incorrectas
                Console.WriteLine("Usuario o contraseña incorrectos.");
                await DisplayAlert("Error", "Usuario o contraseña incorrectos. Intente de nuevo.", "OK");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al conectar: {ex.Message}");
            // Mostrar el mensaje de error en la interfaz de usuario, por ejemplo:
            await DisplayAlert("Error", $"No se pudo conectar a la base de datos: {ex.Message}", "OK");
        }
    }
}
