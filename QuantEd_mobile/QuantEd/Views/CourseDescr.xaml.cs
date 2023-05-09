namespace QuantEd.Views;

public partial class CourseDescr : ContentPage
{
	public CourseDescr()
	{
		InitializeComponent();
	}

    void ToCourses(System.Object sender, System.EventArgs e)
    {
        Navigation.PushModalAsync(new CourseSearch());
    }

    void ViewAccount(System.Object sender, System.EventArgs e)
    {
        Navigation.PushModalAsync(new AccountView());
    }
}
