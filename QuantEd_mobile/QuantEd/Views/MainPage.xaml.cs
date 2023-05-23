namespace QuantEd.Views;

using Core.Models;
using Newtonsoft.Json;
using Nito.AsyncEx;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using WebAPI.Response.Models;

public partial class MainPage : ContentPage
{
    public static HttpClient _client = new HttpClient();
    public static string BaseAddress =
      DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5233" : "http://localhost:5233";
    public static bool isAuthorized = false;
    public MainPage()
	{
		InitializeComponent();
    }

	private void Register(object sender, EventArgs e)
	{
		 Navigation.PushModalAsync(new Registration());
	}


    private void GoToLogIn(object sender, EventArgs e)
    {
        Navigation.PushModalAsync(new LogIn());
    }

    private void ViewCoursesLogOut(object sender, EventArgs e)
    {
        MainPage.isAuthorized = false;
        Navigation.PushModalAsync(new CourseSearch());
    }
}


