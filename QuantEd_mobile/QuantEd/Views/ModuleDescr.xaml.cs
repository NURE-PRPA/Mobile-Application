using Core.Models;
using Newtonsoft.Json;
using Nito.AsyncEx;
using WebAPI.Response.Models;

namespace QuantEd.Views;

public partial class ModuleDescr : ContentPage
{
	CourseModule module;
	public static UserAttempt attempt;
	public static Core.Models.Test test;
	int max = 0;
	bool was = false;
	public ModuleDescr(CourseModule module)
	{
		InitializeComponent();
		this.module = module;
        FillPage();
		attempt = new UserAttempt();
		if (module.Test != null)
		{
			AsyncContext.Run(() => GetTest(module.Test.Id));
			//test = module.Test;
			GetMark();
			if(module.Test.UserAttempts != null)
			{
				test.UserAttempts = module.Test.UserAttempts;
				attempt = test.UserAttempts[0];

            }

            Label label = (Label)FindByName("markLabel");

            if (attempt == null)
            {
                label.Text = $"0/{max}";
            }
            else if (test.Questions.Count == 0)
            {
                label.Text = "0/0";
            }
            else
            {
                label.Text = $"{attempt.Mark}/{max}";
            }
        }
		else
		{
            if (module.Test == null)
            {
                Label label2 = (Label)FindByName("markLabel");
                label2.Text = "0/0";
            }
        }
    }


	void FillPage()
	{
		Label label = (Label)FindByName("name");
		label.Text = module.Name;
		label = (Label)FindByName("description");
		label.Text = module.Description;
        label = (Label)FindByName("questNum");
		Button btn = (Button)FindByName("testStart");
		if (module.Test != null)
		{
			label.Text = module.Test.Questions.Count.ToString() + " questions";
            btn.IsEnabled = true;
            btn.IsVisible = true;
        }
		else
		{
            label.Text = "0 questions";
			btn.IsEnabled = false;
			btn.IsVisible = false;
            Label label2 = (Label)FindByName("markLabel");
			label2.Text = "0/0";
        }

    }
    private void testStart_Clicked(object sender, EventArgs e)
    {
		Navigation.PushAsync(new Test(test));
    }

	 void GetMark()
	 {
		
		int count = 0;
		for (int i = 0; i < test.Questions.Count; i++)
		{
			for (int j = 0; j < test.Questions[i].Answers.Count; j++)
			{
				if (test.Questions[i].Answers[j].IsCorrect)
				{
					count++;
				}
			}
		}
		max = count;
	}
	async void GetUserAttempt()
	{
		var httpResponse = await MainPage._client.GetAsync($"{MainPage.BaseAddress}/api/attempts/{module.Test.Id}");
		var responseData = JsonConvert.DeserializeObject<Response<UserAttempt>>(await httpResponse.Content.ReadAsStringAsync());
		if (responseData.Messages[0] != null)
		{
			var list = responseData.Content;
			attempt = list;
		}
	}

    async void GetTest(string id)
    {
        var content1 = JsonConvert.SerializeObject(id);
        var httpResponse = await MainPage._client.GetAsync($"{MainPage.BaseAddress}/api/tests/{id}");
        var responseData = JsonConvert.DeserializeObject<Response<Core.Models.Test>>(await httpResponse.Content.ReadAsStringAsync());
        var list = responseData.Content;
        test = list;
    }

    private void ContentPage_NavigatingFrom(object sender, NavigatingFromEventArgs e)
    {

    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Label label = (Label)FindByName("markLabel");

        if (attempt == null)
        {
            label.Text = $"0/{max}";
        }
		else if( test == null || test.Questions.Count == 0)
		{
            label.Text = "0/0";
        }
        else if(test.Questions.Count >= 1)
        {
            label.Text = $"{ModuleDescr.test.UserAttempts[0].Mark}/{max}";
        }
    }
}