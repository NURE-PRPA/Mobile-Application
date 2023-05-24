using Core.Models;
using Microsoft.Maui.Controls.Shapes;
using Newtonsoft.Json;
using Nito.AsyncEx;
using System.Text;
using WebAPI.Response.Models;

namespace QuantEd.Views;

public partial class Test : ContentPage
{
    Core.Models.Test test;
    List<int> results;
	public Test(Core.Models.Test test)
	{
		InitializeComponent();
        //AsyncContext.Run(() => GetTest(test.Id));
        this.test = test;
        StackLayout view = (StackLayout)FindByName("questions");
        for(int i = 0; i < test.Questions.Count; i++)
        {
            Border q = MakeQuestion(test.Questions[i], i);
            view.Children.Add(q);
        }
        results = new List<int>();
        for(int i = 0; i < test.Questions.Count; i++)
        {
            results.Add(0);
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
            Text = num+1 + ". " + question.Text,
                Margin = new Thickness(10, 10, 10, 10),
                FontFamily = "Jost",
                FontAttributes = FontAttributes.Bold,
                FontSize = 15
            };
            stack.Children.Add(text);
        string QId = num.ToString();
        string AnswId = "";
        if (question.Type == Core.Enums.QuestionType.Single)
        {
            StackLayout answ = new StackLayout();
            answ.AutomationId = QId;
            
            for (int i =0;i < question.Answers.Count; i++) {
                AnswId = i.ToString();
                RadioButton answer = new RadioButton
                {
                    Content = question.Answers[i].Text,
                    GroupName = $"{num}",
                    TextColor = Color.FromHex("#3D6D79"),
                    FontFamily = "Jost",
                    FontSize = 15
                };
                answer.CheckedChanged += CheckAnswer;
                answer.AutomationId = AnswId;
                answ.Children.Add(answer);
            }
            stack.Children.Add(answ);
        }
        else if (question.Type == Core.Enums.QuestionType.Multiple)
        {
            StackLayout answ = new StackLayout();
            answ.AutomationId = QId;

            for (int i = 0; i < question.Answers.Count; i++)
            {
                AnswId = i.ToString();
                CheckBox answer = new CheckBox { AutomationId = AnswId };
                answer.CheckedChanged += CheckMultAnswer;
                Label answCont = new Label
                {
                    Text = question.Answers[i].Text,
                    TextColor = Color.FromHex("#3D6D79"),
                    FontFamily = "Jost",
                    FontSize = 15,
                    
                };
                StackLayout checkStack = new StackLayout {
                    Orientation = StackOrientation.Horizontal,
                    Children = { answer, answCont },
                    Spacing = 4
                    
                };
                checkStack.AutomationId = QId;
                answ.Children.Add(checkStack);
            }
            stack.Children.Add(answ);
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


    int CheckTest()
    {
        StackLayout stack = (StackLayout)FindByName("questions");
        int mark = 0;
        for(int i = 0; i < test.Questions.Count; i++)
        {
            if (test.Questions[i].Type == Core.Enums.QuestionType.Single)
            {
                StackLayout answ = (StackLayout)stack.FindByName(i.ToString());
                for(int j = 0; j < test.Questions[i].Answers.Count; j++)
                {
                    RadioButton check = (RadioButton)answ.FindByName(j.ToString());
                    if (test.Questions[i].Answers[j].IsCorrect && check.IsChecked)
                    {
                        mark++;
                        break;
                    }
                }
            }else if(test.Questions[i].Type == Core.Enums.QuestionType.Multiple)
            {
                int num = 0;
                int correct = 0;
                StackLayout stack2 = (StackLayout)stack.FindByName(i.ToString());
                for(int j = 0; j < test.Questions[i].Answers.Count; j++)
                {
                    CheckBox check =(CheckBox)stack2.FindByName(j.ToString());
                    if (test.Questions[i].Answers[j].IsCorrect)
                    {
                        correct++;
                        if (check.IsChecked)
                        {
                            num++;
                        }
                    }
                }
                mark = num / correct;

            }
        }
        return mark;
    }

    private void testEnd_Clicked(object sender, EventArgs e)
    {
        float mark = CountMark();
        TestResult res = new TestResult();
        res.TestId = test.Id;
        res.Mark = ((byte)mark);
        AddUserAttempt(res);
       
    }

    async void AddUserAttempt(TestResult res)
    {
        var content1 = JsonConvert.SerializeObject(res);
        StringContent content = new StringContent(content1, Encoding.UTF8, "application/json");
        var response = await MainPage._client.PostAsync($"{MainPage.BaseAddress}/api/attempts/add", content);
        var responseData1 = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync());
        if (responseData1.Messages[0]== "Attempt added successfully")
        {
            ModuleDescr.test.UserAttempts[0].Mark = res.Mark;
            await DisplayAlert("Test", "Your answer was saved", "Ok");
             await Navigation.PopAsync();
        }
    }


    private void CheckAnswer(object sender, EventArgs e)
    {
        RadioButton btn = (RadioButton)sender;
        string id = btn.AutomationId;
        int AnswId = int.Parse(id);
        StackLayout parentElement = btn.Parent as StackLayout;
        id = parentElement.AutomationId;
        int QId = int.Parse(id);
        if (btn.IsChecked == true)
        {
            if (test.Questions[QId].Answers[AnswId].IsCorrect)
            {
                results[QId] = 1;
            }
            else
            {
                results[QId] = 0;
            }
        }
       
    }

    private void CheckMultAnswer(object sender, EventArgs e)
    {
        CheckBox btn = (CheckBox)sender;
        string id = btn.AutomationId;
        int AnswId = int.Parse(id);
        StackLayout parentElement = btn.Parent as StackLayout;
        id = parentElement.AutomationId;
        int QId = int.Parse(id);
        if (btn.IsChecked == true)
        {
            if (test.Questions[QId].Answers[AnswId].IsCorrect)
            {
                results[QId]++;
            }
        }
        else
        {
            if (test.Questions[QId].Answers[AnswId].IsCorrect)
            {
                results[QId]--;
            }
        }
    }

    int CountMark()
    {
        int count = 0;
        for(int i = 0; i < test.Questions.Count; i++)
        {
            if (test.Questions[i].Type == Core.Enums.QuestionType.Single)
            {
                if (results[i] == 1)
                {
                    count += 1;
                }
            }else if(test.Questions[i].Type == Core.Enums.QuestionType.Multiple)
            {
                //int max = 0;
                //for(int j = 0; j < test.Questions[i].Answers.Count; j++)
                //{
                //    if (test.Questions[i].Answers[j].IsCorrect)
                //    {
                //        max++;
                //    }
                //}

                //count = count + results[i] / max;
                count += results[i];
            }
        }
        return count;
    }
}