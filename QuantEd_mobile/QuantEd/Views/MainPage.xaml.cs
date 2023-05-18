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
   
    public MainPage()
	{
		InitializeComponent();

      //  AsyncContext.Run(() => MainAsync());
    }

	private void Register(object sender, EventArgs e)
	{
		 Navigation.PushModalAsync(new Registration());
	}

    public static async void MainAsync()
    {

        var hostEntry = Dns.GetHostEntry("localhost");
        var ipAddress = hostEntry.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);

        var user = new LoginUser()
        {
            Email = "anastasiia.lulakova@nure.ua",
            Password = "1234",
            UserType = "listener"
        };

        var content1 = JsonConvert.SerializeObject(user);
        StringContent content = new StringContent(content1, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{BaseAddress}/api/auth/login", content);
        var responseData1 = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync());




        var httpResponse = await _client.GetAsync($"{BaseAddress}/api/courses/all");
    
     var responseData = JsonConvert.DeserializeObject<Response<List<Course>>>(await httpResponse.Content.ReadAsStringAsync());
     var courses = responseData.Content;

        //var httpResponse = await _Client.GetAsync(url);

        //var responseData = JsonConvert.DeserializeObject<Response<List<Course>>>(await httpResponse.Content.ReadAsStringAsync());
        //var courses = responseData.Content;
        //User_List.ItemsSource = userCollection;
    }

    private void GoToLogIn(object sender, EventArgs e)
    {
        Navigation.PushModalAsync(new LogIn());
    }
}


