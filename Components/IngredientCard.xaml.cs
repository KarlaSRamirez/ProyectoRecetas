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
        }
    }
}
