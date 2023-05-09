namespace QuantEd.Views;

public partial class MainPage_LogIn : ContentPage
{
	public MainPage_LogIn()
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


    

    public static void GoToRoot(INavigation navigation)
    {
        int pageCount = navigation.ModalStack.Count;
        Page page1 = navigation.ModalStack[0];
       
        for (int i = pageCount - 1; i >= 0; i--)
        {
            if (navigation.ModalStack[i] is MainPage_LogIn page)
            {
                page1 = navigation.ModalStack[i];
            }
            
        }
        navigation.PopToRootAsync();
        navigation.PushModalAsync(page1);
    }

}
