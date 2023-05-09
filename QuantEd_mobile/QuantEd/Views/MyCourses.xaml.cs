namespace QuantEd.Views;

public partial class MyCourses : ContentPage
{
	public MyCourses()
	{
		InitializeComponent();
        var label = new Label
        {
            Text = "My Courses",
            TextColor = Color.FromHex("#3D6D79"),
            FontAttributes = FontAttributes.Bold,
            FontFamily = "Jost",
            HorizontalTextAlignment = TextAlignment.Center,
            FontSize = 24,
            Margin = new Thickness(0, 10, 0, 0)
        };
        ScrollView scroll = (ScrollView)FindByName("CoursesList");
        StackLayout pairList = new StackLayout();
        pairList.Add(label);
        for(int i = 0;i<4; i++)
        {
            Grid card = CourseSearch.MakeCourseCard();
            card = CourseSearch.MakeCourseCard();
            pairList.Add(card);
        }
        scroll.Content = pairList;

	}

    void ViewAccount(System.Object sender, System.EventArgs e)
    {
        Navigation.PushModalAsync(new AccountView());
    }

    void ToCourses(System.Object sender, System.EventArgs e)
    {
        Navigation.PushModalAsync(new CourseSearch());
    }

    void ToMainLogged(System.Object sender, System.EventArgs e)
    {
        MainPage_LogIn.GoToRoot(this.Navigation);
       // Navigation.PopModalAsync();
    }
}
