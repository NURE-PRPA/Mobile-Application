namespace QuantEd.Views;

public partial class Registration : ContentPage
{
	public Registration()
	{
		InitializeComponent();
	}

    void ToMainPage(System.Object sender, System.EventArgs e)
    {
		Navigation.PopModalAsync();
    }

    void ToMainLog(System.Object sender, System.EventArgs e)
    {
        Navigation.PushModalAsync(new MainPage_LogIn());
    }
}
