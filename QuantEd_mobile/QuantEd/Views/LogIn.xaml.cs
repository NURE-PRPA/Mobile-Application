using Core.Models;
using Newtonsoft.Json;
using System.Text;
using WebAPI.Response.Models;

namespace QuantEd.Views;

public partial class LogIn : ContentPage
{
	public LogIn()
	{
		InitializeComponent();
	}

    void ToMainPage(System.Object sender, System.EventArgs e)
    {
        Navigation.PopModalAsync();
    }

    async void ToMainLog(System.Object sender, System.EventArgs e)
    {
        Entry email = (Entry)FindByName("email");
        Entry password = (Entry)FindByName("password");

        var user = new LoginUser()
        {
            Email = email.Text,
            Password = password.Text,
            UserType = "listener"
        };

        var content1 = JsonConvert.SerializeObject(user);
        StringContent content = new StringContent(content1, Encoding.UTF8, "application/json");
        var response = await MainPage._client.PostAsync($"{MainPage.BaseAddress}/api/auth/login", content);
        var responseData1 = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync());
        if (responseData1.Messages[0] == "Log in success") {
            await Navigation.PushModalAsync(new MainPage_LogIn());
        }
        else
        {
            await DisplayAlert("Error", "Log In failed", "OK");
            ToMainPage(sender, e);
        }
    }
}