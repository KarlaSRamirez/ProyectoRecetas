namespace ProyectoRecetas.Views;

public partial class ActualizarPage : ContentPage
{
	public ActualizarPage()
	{
		InitializeComponent();
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