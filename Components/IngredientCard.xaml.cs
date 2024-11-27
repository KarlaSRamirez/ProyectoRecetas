using MySqlConnector;
using static ProyectoRecetas.Views.IngredientPage;

namespace ProyectoRecetas.Components
{
    public partial class IngredientCard : ContentView
    {
        // Propiedades que ser�n enlazadas en el XAML
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
                "Confirmaci�n",
                $"�Deseas eliminar el ingrediente {Name}?",
                "S�",
                "No"
            );

            if (!confirm)
                return;

            try
            {
                // Cadena de conexi�n a la base de datos
                var builder = new MySqlConnectionStringBuilder
                {
                    Server = "192.168.247.1", // Cambia a tu direcci�n IP o servidor
                    UserID = "root",
                    Password = "11julio2002", // Cambia por tu contrase�a
                    Database = "RecetasBD"
                };

                using var connection = new MySqlConnection(builder.ConnectionString);
                await connection.OpenAsync();

                // Consulta SQL para eliminar el ingrediente
                string deleteQuery = "DELETE FROM ingredientes WHERE Nombre = @Nombre";

                using var command = new MySqlCommand(deleteQuery, connection);
                command.Parameters.AddWithValue("@Nombre", Name);

                int rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected > 0)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "�xito",
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
                        $"No se encontr� el ingrediente {Name} en la base de datos.",
                        "OK"
                    );
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    $"Ocurri� un error al eliminar el ingrediente: {ex.Message}",
                    "OK"
                );
            }
        }
    }
}
