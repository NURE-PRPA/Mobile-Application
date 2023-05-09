namespace QuantEd.Views;

public partial class MainPage : ContentPage
{
	

	public MainPage()
	{
		InitializeComponent();
	}

	private void Register(object sender, EventArgs e)
	{
		 Navigation.PushModalAsync(new Registration());
	}
}


