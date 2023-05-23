using Core.Models;
using Newtonsoft.Json;
using Nito.AsyncEx;
using WebAPI.Response.Models;

namespace QuantEd.Views;

public partial class MyCourses : ContentPage
{
    List<Course> courses;
    public MyCourses()
	{
		InitializeComponent();
        
        AsyncContext.Run(() => FillCards());
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
        //MainPage_LogIn.GoToRoot<MainPage_LogIn>(this.Navigation);
       Navigation.PushModalAsync(new MainPage_LogIn());
    }

    async void FillCards()
    {
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
        var coursesList = await GetMyCourses();
        for (int i = 0; i < coursesList.Count; i++)
        {
            Grid card = CourseSearch.MakeCourseCard(coursesList[i]);
            card.AutomationId = coursesList[i].Id;
            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += ToCourseDescription;
            card.GestureRecognizers.Add(tapGesture);
            pairList.Add(card);
        }
        scroll.Content = pairList;
    }
    async Task<List<Course>> GetMyCourses()
    {
        var httpResponse = await MainPage._client.GetAsync($"{MainPage.BaseAddress}/api/courses/my");
        var responseData = JsonConvert.DeserializeObject<Response<List<Course>>>(await httpResponse.Content.ReadAsStringAsync());
        var list = responseData.Content;
        courses = list;
        return list;
    }
    void ToCourseDescription(System.Object sender, System.EventArgs e)
    {
        Grid grid = (Grid)sender;
        string id = grid.AutomationId;
        Course course = new Course();
        for (int i = 0; i < courses.Count; i++)
        {
            if (id == courses[i].Id)
            {
                course = courses[i];
                break;
            }
        }
        var courseDescrPage = new CourseDescr(course);
        Navigation.PushModalAsync(courseDescrPage);
    }
}
