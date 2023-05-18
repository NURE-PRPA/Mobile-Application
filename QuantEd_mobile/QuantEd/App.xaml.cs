using QuantEd.Views;
using Microsoft.Maui.Controls;

using Core.Models;
using Newtonsoft.Json;
using System.Text;
using WebAPI.Response.Models;
using Nito.AsyncEx;


namespace QuantEd;

public partial class App : Application
{
   // public static HttpClient _client = new HttpClient();
    public MainPage_LogIn mainPage_registered;

    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
        
        mainPage_registered = new MainPage_LogIn();
        //AsyncContext.Run(() => MainAsync());
        
    }

   
}

