using Core.Models;
using Microsoft.Maui.Controls.Shapes;
using Newtonsoft.Json;
using Nito.AsyncEx;
using WebAPI.Response.Models;

namespace QuantEd.Views;

public partial class Test : ContentPage
{
    Core.Models.Test test;
	public Test(string testId)
	{
		InitializeComponent();
        AsyncContext.Run(() => GetTest(testId));
        StackLayout view = (StackLayout)FindByName("questions");
        for(int i = 0; i < test.Questions.Count; i++)
        {
            Border q = MakeQuestion(test.Questions[i], i+1);
            view.Children.Add(q);
        }
	}

    Border MakeQuestion(Question question, int num)
    {
        if(question == null) { return null; }
            var border = new Border
            {
                Stroke = Color.FromHex("#3D6D79"),
                StrokeThickness = 1.5,
                BackgroundColor = Colors.White,
                Margin = new Thickness(20, 10, 20, 5),
                Padding = 5,
                StrokeShape = new RoundRectangle
                {
                    CornerRadius = 5
                }
            };
        StackLayout stack = new StackLayout();
        var text = new Label
        {
            TextColor = Color.FromHex("#3D6D79"),
            Text = num + ". " + question.Text,
                Margin = new Thickness(10, 10, 10, 10),
                FontFamily = "Jost",
                FontAttributes = FontAttributes.Bold,
                FontSize = 15
            };
            stack.Children.Add(text);
            if(question.Type == Core.Enums.QuestionType.Single)
        {
            StackLayout answ = new StackLayout();
            for(int i =0;i < question.Answers.Count; i++) {
                RadioButton answer = new RadioButton
                {
                    Content = question.Answers[i].Text,
                    GroupName = $"{num}",
                    TextColor = Color.FromHex("#3D6D79"),
                    FontFamily = "Jost",
                    FontSize = 15//,
                    //Margin = new Thickness { Left = 0, Right = 0, Bottom = 0}
                };
                answ.Children.Add(answer);
            }
            stack.Children.Add(answ);
        }else if (question.Type == Core.Enums.QuestionType.Multiple)
        {
            for (int i = 0; i < question.Answers.Count; i++)
            {
                CheckBox answer = new CheckBox();
                Label answCont = new Label
                {
                    Text = question.Answers[i].Text,
                    TextColor = Color.FromHex("#3D6D79"),
                    FontFamily = "Jost",
                    FontSize = 15,
                    
                };
                StackLayout checkStack = new StackLayout {
                    Orientation= StackOrientation.Horizontal,
                    Children = { answer, answCont },
                    Spacing = 4 
                };
                stack.Children.Add(checkStack);
            }
        }


        border.Content = stack;

          
        return border;
    }

    

    protected override bool OnBackButtonPressed()
    {
        DisplayConfirmationMessage();
        return true;
    }

    private async void DisplayConfirmationMessage()
    {
        bool userWantsToLeave = await DisplayAlert("Confirmation", "Do you want to leave test? Your answers won't have been saved", "Leave", "Continue");

        if (userWantsToLeave)
        {
            await Navigation.PopAsync(); // Pop the current page from the navigation stack
        }
    }

    async void GetTest(string id)
    {
        var content1 = JsonConvert.SerializeObject(id);
        var httpResponse = await MainPage._client.GetAsync($"{MainPage.BaseAddress}/api/tests/{id}");
        var responseData = JsonConvert.DeserializeObject<Response<Core.Models.Test>>(await httpResponse.Content.ReadAsStringAsync());
        var list = responseData.Content;
        this.test = list;
    }
}