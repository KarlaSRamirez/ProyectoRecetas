using MySqlConnector;
using ProyectoRecetas.Views;using static ProyectoRecetas.Views.IngredientPage;

namespace ProyectoRecetas.Components
{
    public partial class IngredientCard : ContentView
    {
        // Propiedades que serán enlazadas en el XAML
        public static readonly BindableProperty NameProperty =
            BindableProperty.Create(nameof(Name), typeof(string), typeof(IngredientCard), string.Empty);

        public static readonly BindableProperty DescriptionProperty =
            BindableProperty.Create(nameof(Description), typeof(string), typeof(IngredientCard), string.Empty);

        public static readonly BindableProperty QuantityProperty =
            BindableProperty.Create(nameof(Quantity), typeof(int), typeof(IngredientCard), 0);

        public static readonly BindableProperty UnitProperty =
            BindableProperty.Create(nameof(Unit), typeof(string), typeof(IngredientCard), string.Empty);

        public static readonly BindableProperty ExpirationProperty =
            BindableProperty.Create(nameof(Expiration), typeof(DateTime), typeof(IngredientCard), DateTime.MinValue);

        public string Name
        {
            get => (string)GetValue(NameProperty);
            set => SetValue(NameProperty, value);
        }

        public string Description
        {
            get => (string)GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }

        public int Quantity
        {
            get => (int)GetValue(QuantityProperty);
            set => SetValue(QuantityProperty, value);
        }

        public string Unit
        {
            get => (string)GetValue(UnitProperty);
            set => SetValue(UnitProperty, value);
        }

        public DateTime Expiration
        {
            get => (DateTime)GetValue(ExpirationProperty);
            set => SetValue(ExpirationProperty, value);
        }

        public IngredientCard()
        {
            InitializeComponent();
            BindingContextChanged += OnBindingContextChanged;
        }

        private void OnBindingContextChanged(object sender, EventArgs e)
        {
            if (BindingContext is Ingredient ingredient)
            {
                Name = ingredient.Name;
                Description = ingredient.Description;
                Quantity = ingredient.Quantity;
                Unit = ingredient.Unit;
                Expiration = ingredient.Expiration;
            }
        }

        private async void EliminateIngredient(object sender, EventArgs e)
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Confirmación",
                $"¿Deseas eliminar el ingrediente {Name}?",
                "Sí",
                "No"
            );

            if (!confirm)
                return;

            try
            {
                // Cadena de conexión a la base de datos
                var builder = DatabaseConfig.GetConnectionStringBuilder();

                using var connection = new MySqlConnection(builder.ConnectionString);
                await connection.OpenAsync();

                // Eliminar relacion
                string query1 = "DELETE FROM usuarios_ingredientes WHERE ID_Ingrediente = (SELECT ID_Ingrediente FROM Ingredientes WHERE Nombre = @name AND Descripcion = @description AND Cantidad = @quantity AND Unidad_de_Medida = @unit AND Caducidad = @expiration)";
                using var command1 = new MySqlCommand(query1, connection);
                command1.Parameters.AddWithValue("@name", Name);
                command1.Parameters.AddWithValue("@description", Description);
                command1.Parameters.AddWithValue("@quantity", Quantity);
                command1.Parameters.AddWithValue("@unit", Unit);
                command1.Parameters.AddWithValue("@expiration", Expiration);
                await command1.ExecuteNonQueryAsync();

                // Consulta SQL para eliminar el ingrediente
                string deleteQuery = "DELETE FROM ingredientes WHERE ID_Ingrediente = (SELECT ID_Ingrediente FROM Ingredientes WHERE Nombre = @name AND Descripcion = @description AND Cantidad = @quantity AND Unidad_de_Medida = @unit AND Caducidad = @expiration)"; //Cambiar "name" por el id del ingrediente
                using var command = new MySqlCommand(deleteQuery, connection);
                command.Parameters.AddWithValue("@name", Name);
                command.Parameters.AddWithValue("@description", Description);
                command.Parameters.AddWithValue("@quantity", Quantity);
                command.Parameters.AddWithValue("@unit", Unit);
                command.Parameters.AddWithValue("@expiration", Expiration);

                int rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected > 0)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Éxito",
                        $"El ingrediente {Name} fue eliminado correctamente.",
                        "OK"
                    );

                    // Opcional: Notifica a la vista principal para refrescar la lista
                    var parent = this.Parent as StackLayout;
                    parent?.Children.Remove(this);
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        $"No se encontró el ingrediente {Name} en la base de datos.",
                        "OK"
                    );
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    $"Ocurrió un error al eliminar el ingrediente: {ex.Message}",
                    "OK"
                );
            }
        }

        private async void NavActualizar(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new ActualizarPage(Name, Description, Quantity, Unit, Expiration));
        }
    }
}
