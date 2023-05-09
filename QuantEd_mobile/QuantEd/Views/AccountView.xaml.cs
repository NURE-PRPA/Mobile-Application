namespace QuantEd.Views;

public partial class AccountView : ContentPage
{
	public AccountView()
	{
		InitializeComponent();
	}

    void SeePassword(System.Object sender, System.EventArgs e)
    {

    }

    void Button_Clicked(System.Object sender, System.EventArgs e)
    {
        Navigation.PushModalAsync(new CourseSearch());
    }

    void ToMainPageLogged(System.Object sender, System.EventArgs e)
    {
        MainPage_LogIn.GoToRoot(Navigation);
        //Navigation.PopModalAsync();
    }
}
