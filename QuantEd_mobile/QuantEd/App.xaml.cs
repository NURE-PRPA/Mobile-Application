using QuantEd.Views;
using Microsoft.Maui.Controls;

namespace QuantEd;

public partial class App : Application
{

	public MainPage_LogIn mainPage_registered;

	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
		mainPage_registered = new MainPage_LogIn();

    }

}   

