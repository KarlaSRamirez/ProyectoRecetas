namespace ProyectoRecetas.Views

{
    public partial class RecetasPage : ContentPage
    {
        public RecetasPage()
        {
            InitializeComponent();
        }
        private async void OnNavigateButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new IngredientPage());
        }

        private async void OnNavigateButtonClicked1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HistoryPage());
        }
    }
}
