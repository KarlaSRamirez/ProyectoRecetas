, using MySqlConnector;

namespace ProyectoRecetas.Views;

public partial class ActualizarPage : ContentPage
{
    private readonly string _name;
    private readonly string _description;
    private readonly string _quantity;
    private readonly string _unit;
    private readonly string _expiration;

	public ActualizarPage(string name, string description, int quantity, string unit, DateTime expiration)
	{
		InitializeComponent();
        LoadIngredients();
        LoadUnits();
        _name = name;
        _description = description;
        _quantity = quantity.ToString();
        _unit = unit;
        _expiration = expiration.ToString("yyyy-MM-dd");

        IngredientPicker.SelectedItem = _name;
        Cantidad.Text = _quantity;
        Descripcion.Text = _description;
        UnitPicker.SelectedItem = _unit;
        datePicker.Date = expiration;
	}

    private void OnIngredientSelected(object sender, EventArgs e)
    {
        var selectedIngredient = IngredientPicker.SelectedItem as string;
    }

    private void LoadUnits()
    {
        var units = new List<string>
        {
            "Gramos","Kilogramos","Litros","Mililitros","Piezas","Galon"
        };
        UnitPicker.ItemsSource = units;
    }

    private void LoadIngredients()
    {
        // Aqu� puedes obtener los ingredientes de una base de datos o una lista predeterminada
        var ingredients = new List<string>
            {
                "Aceite de coco","Aceite de oliva","Aceite de s�samo","Aceite vegetal","Ajo","Albahaca seca","Almendras","An�s estrellado","Arroz","Ar�ndanos","At�n enlatado","Avellanas","Avena","Az�car","Az�car glas","Az�car mascabado","Berenjena","Betabel","Bicarbonato de sodio","Br�coli","Cacao amargo","Cacao en polvo","Calabac�n","Calamares","Caldo","Camarones","Canela","Cardamomo","Carne de res","Carne molida","Cebolla","Cereza","Champi�ones","Chayote","Chiles habaneros","Chiles jalape�os","Chiles poblanos","Chocolate","Chocolate con leche","Chocolate oscuro","Chorizo","Cilantro","Ciruela","Clavo de olor","Coliflor","Comino","Crema","Crema de coco","Curry en polvo","Cusc�s","C�rcuma","Durazno","Elote","Espinacas","Esp�rragos","Estrag�n","Extracto de vainilla","Fideos de arroz","Frambuesa","Fresa","Frijoles negros","Garbanzos","Gelatina sin sabor","Harina","Harina de ma�z","Huevos","Jam�n","Jarabe de agave","Jarabe de ma�z","Jengibre fresco","K�tchup","Laurel","Leche","Leche condensada","Leche de almendras","Leche de avena","Leche de soya","Leche evaporada","Lechuga","Lentejas","Levadura seca","Lima","Lim�n","Maicena","Mango","Manteca de cerdo","Mantequilla","Manzana","Man�","Masa para tortillas","Mayonesa","Mejillones","Mel�n","Mermelada","Miel","Moras","Mostaza","Muslos de pollo","Naranja","Nopal","Nueces","Nuez moscada","Or�gano","Pan","Pan rallado","Papas","Papaya","Paprika","Pasta","Pechuga de pollo","Pepino","Pera","Perejil","Pescado","Pescado blanco","Piernas de pollo","Pimienta negra","Pimientos","Pi�a","Pl�tano","Pollo","Polvo para hornear","Queso","Queso crema","Queso feta","Queso manchego","Queso parmesano","Quinoa","Romero","R�banos","Sal","Salchichas","Salm�n","Salsa de soya","Salsa inglesa","Sand�a","Semillas de ch�a","Semillas de girasol","Semillas de lino","Setas","S�mola","Tocino","Tomates","Tomillo","Tortillas","Uvas","Vinagre","Vinagre bals�mico","Vinagre de arroz","Vinagre de vino tinto","Yogur","Zanahorias"
            };

        // Asignar los ingredientes al Picker
        IngredientPicker.ItemsSource = ingredients;
    }

    private void OnUnitSelected(object sender, EventArgs e)
    {
        var selectedUnit = UnitPicker.SelectedItem as string;
    }

    private void OnCantidadTextChanged(object sender, TextChangedEventArgs e)
    {
        var entry = sender as Entry;

        // Verificar si el texto ingresado es un n�mero v�lido
        if (!string.IsNullOrEmpty(entry.Text) && !IsNumeric(entry.Text))
        {
            // Si el texto no es num�rico, lo revertimos al valor anterior
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

    private async void ActualizarButton(object sender, EventArgs e)
    {
        var name = IngredientPicker.SelectedItem as string;
        var description = Descripcion.Text;
        var quantity = Cantidad.Text;
        var unit = UnitPicker.SelectedItem as string;
        var expiration = datePicker.Date.ToString("yyyy-MM-dd");

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

            await DisplayAlert("�xito", "Se ha actualizado el ingrediente.", "OK");
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
            // Truncar el texto al l�mite m�ximo permitido
            editor.Text = editor.Text.Substring(0, 100);
        }
    }
}