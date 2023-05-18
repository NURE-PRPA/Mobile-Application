using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Controls.Internals;
using Newtonsoft.Json;
//using Windows.Media.Protection.PlayReady;
using Core.Models;
using WebAPI.Response.Models;
using Nito.AsyncEx;

namespace QuantEd.Views;

public partial class CourseSearch : ContentPage
{
    List<Course> courses;
    public CourseSearch()
    {
		InitializeComponent();
        AsyncContext.Run(() => FillCards());
        Button acc = (Button)FindByName("account");
        Button mycourses = (Button)FindByName("myCourses");
        if (!MainPage.isAuthorized)
        {
            acc.Text = "Log In";
            myCourses.IsEnabled = false;
            myCourses.IsVisible = false;
        }

    }

    async void FillCards()
    {
        var coursesList = await GetCourses();
        Grid grid = (Grid)FindByName("Courses");
        StackLayout CourseStack = new StackLayout { Margin = new Thickness(0, 10, 0, 40) };
        for (int i = 0; i < coursesList.Count; i++)
        {
            Grid card = MakeCourseCard(coursesList[i]);
            card.AutomationId = coursesList[i].Id;
            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += ToCourseDescription;
            card.GestureRecognizers.Add(tapGesture);
            CourseStack.Add(card);
        }
        ScrollView scrollView = new ScrollView { Margin = new Thickness(0, 10, 0, 40), Content = CourseStack };
        grid.Add(scrollView, 0, 3);
    }
    void ToMyCourses(System.Object sender, System.EventArgs e)
    {
        Navigation.PushModalAsync(new MyCourses());
    }

    void ViewAccount(System.Object sender, System.EventArgs e)
    {
		Navigation.PushModalAsync(new AccountView());
    }

    void ToMainPageLogged(System.Object sender, System.EventArgs e)
    {
        MainPage_LogIn.GoToRoot(Navigation);
       
    }

    void ToCourseDescription(System.Object sender, System.EventArgs e)
    {
        Grid grid = (Grid)sender;
        string id = grid.AutomationId;
        Course course = new Course();
        for(int i = 0; i < courses.Count; i++)
        {
            if(id == courses[i].Id)
            {
                course = courses[i];
                break;
            }
        }
        var courseDescrPage = new CourseDescr(course);
        Navigation.PushModalAsync(courseDescrPage);
    }

   async Task<List<Course>> GetCourses()
    {
        var httpResponse = await MainPage._client.GetAsync($"{MainPage.BaseAddress}/api/courses/all");
        var responseData = JsonConvert.DeserializeObject<Response<List<Course>>>(await httpResponse.Content.ReadAsStringAsync());
        var list = responseData.Content;
        courses = list;
        return list;
    }

    public static Grid MakeCourseCard(Course course)
    {
        var CardGrid = new Grid
        {
            RowDefinitions =
    {
        new RowDefinition { Height = new GridLength(2, GridUnitType.Star) },
        new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
    },
            ColumnDefinitions =
    {
        new ColumnDefinition { Width = new GridLength(2.5, GridUnitType.Star) },
        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
    },
            Margin = new Thickness(35, 30, 35, 15),
            WidthRequest = 300,
            HeightRequest = 240
        };

        var ImageBorder = new Border
        {
            Stroke = Color.FromHex("#3D6D79"),
            StrokeThickness = 1.5,
            StrokeShape = new RoundRectangle { CornerRadius = 5 }
        };

        var CourseImage = new Image
        {
            Source = "astronomy.png",
            Aspect = Aspect.AspectFill,
        };
        
        ImageBorder.Content = CourseImage;
        CardGrid.Add(ImageBorder, 0 ,0);
        CardGrid.SetColumnSpan(ImageBorder, 3);
        

        var CourseBackCol = new BoxView
        {
            BackgroundColor = Color.FromHex("#F5F7F4")
        };

        CardGrid.Add(CourseBackCol, 0, 1);
        CardGrid.SetColumnSpan(CourseBackCol, 3);

        var stackLayoutLeft = new StackLayout
        {
            Margin = new Thickness(10, 5, 10, 0),
           
        };

        var CourseName = new Label
        {
            Text = course.Name,
            TextColor = Color.FromHex("#3D6D79"),
            FontAttributes = FontAttributes.Bold,
            FontFamily = "Jost",
            FontSize = 18,
            StyleId = "name"
        };
     
        var ByOrganization = new Label
        {
            Text = "by " + course.Lecturer.Organization.Name,
            Margin = new Thickness(0, 4, 0, 0),
            TextColor = Color.FromHex("#8C8D8D"),
            FontAttributes = FontAttributes.Bold,
            FontSize = 15,
            FontFamily = "Jost"
        };

        var CourseTopic = new Label
        {
            Text = course.Topic.ToString(),
            Margin = new Thickness(0, 4, 0, 0),
            TextColor = Color.FromHex("#3D6D79"),
            FontSize = 15,
            FontFamily = "Jost"
        };

        stackLayoutLeft.Children.Add(CourseName);
        stackLayoutLeft.Children.Add(ByOrganization);
        stackLayoutLeft.Children.Add(CourseTopic);

        CardGrid.Add(stackLayoutLeft, 0, 1);

        var stackLayoutRight = new StackLayout
        {
            Margin = new Thickness(0, 10, 0, 10),
            Spacing = 10
        };

        var CourseModules = new Label
        {
            Text = course.Modules.Count+ " modules",
            TextColor = Color.FromHex("#3D6D79"),
            FontFamily = "Jost"
        };

        var CoursePrice = new Label
        {
            Text = course.Price.ToString()+ "$",
            TextColor = Color.FromHex("#3D6D79"),
            Margin = new Thickness(0, 10, 10, 0),
            FontAttributes = FontAttributes.Bold,
            FontSize = 25,
            HorizontalTextAlignment = TextAlignment.End,
            VerticalTextAlignment = TextAlignment.End,
            FontFamily = "Jost"
        };

        stackLayoutRight.Children.Add(CourseModules);
        stackLayoutRight.Children.Add(CoursePrice);
        CardGrid.Add(stackLayoutRight, 1, 1);
        
        return CardGrid;
    }

    
}
