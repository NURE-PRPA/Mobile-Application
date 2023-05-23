namespace QuantEd.Views;

public partial class Registration : ContentPage
{
	public Registration()
	{
		InitializeComponent();
	}

    void ToMainPage(System.Object sender, System.EventArgs e)
    {
       // MainPage_LogIn.GoToRoot<MainPage>(Navigation);
       Navigation.PushModalAsync(new MainPage());
    }


    //if sign up is successfull
    void ToMainLog(System.Object sender, System.EventArgs e)
    {
       // Navigation.PushModalAsync(new MainPage_LogIn());
       // MainPage.isAuthorized = true;
    }

    private void ToCourses(object sender, EventArgs e)
    {
        Navigation.PushModalAsync(new CourseSearch());
    }

    private void SignUp(object sender, EventArgs e)
    {
        Navigation.PushModalAsync(new Registration());

    }
}
