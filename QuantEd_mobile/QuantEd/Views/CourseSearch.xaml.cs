using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Controls.Internals;
using Newtonsoft.Json;
//using Windows.Media.Protection.PlayReady;
using Core.Models;
using WebAPI.Response.Models;
using Nito.AsyncEx;
using System;
using Core.Enums;

namespace QuantEd.Views;

public partial class CourseSearch : ContentPage
{
    List<Course> courses;
    public static bool isMine = false;

    public CourseSearch()
    {
		InitializeComponent();
        isMine = false;
        AsyncContext.Run(() => FillCards());
        Button acc = (Button)FindByName("account");
        Button mycourses = (Button)FindByName("myCourses");
        Border bord = (Border)FindByName("myCoursBord");
        if (!MainPage.isAuthorized)
        {
            acc.Text = "Log In";
            myCourses.IsEnabled = false;
            myCourses.IsVisible = false;
            bord.IsVisible = false;
        }
        Picker pick = (Picker)FindByName("topicPicker");
        var enumValues = Enum.GetValues(typeof(CourseTopic)).Cast<CourseTopic>().ToList();
        pick.ItemsSource = enumValues;
        Picker picklevel = (Picker)FindByName("levelPicker");
        var enumlevels = Enum.GetValues(typeof(CourseDifficulty)).Cast<CourseDifficulty>().ToList();
        picklevel.ItemsSource = enumlevels;
        Slider slider = (Slider)FindByName("priceSlider");
        int maxPr = GetMaxPrice();
        slider.Maximum = maxPr;
        Label max = (Label)FindByName("max");
        max.Text = maxPr.ToString();
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
            CourseStack.Children.Add(card);
        }
        ScrollView scrollView = (ScrollView)FindByName("cardList");
        scrollView.Content = CourseStack;
       
    }
    void ToMyCourses(System.Object sender, System.EventArgs e)
    {
        isMine = true;
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

    int GetMaxPrice()
    {
        List<int> price = new List<int>();
        int max = 0;
        for(int i = 0; i < courses.Count; i++)
        {
            if (courses[i].Price > max)
            {
                max = courses[i].Price;
            }
        }
        return max;
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

    void Sort()
    {
        List<Course> list = new List<Course>();
        list = courses;
        Picker picker = (Picker)FindByName("topicPicker");
        string topic = null;
        if(picker.SelectedItem != null)
        {
            topic = picker.SelectedItem.ToString();
        }
        picker = (Picker)FindByName("levelPicker");
        string level = null;
        if (picker.SelectedItem != null)
        {
            level = picker.SelectedItem.ToString();
        }
        for(int i = 0; i< list.Count; i++)
        {
            if(topic != null)
            {
                
                if (list[i].Topic.ToString() != topic) {
                list.RemoveAt(i);
                    i--;
                }
            }
            if (level != null)
            {
                if (list[i].Difficulty.ToString() != level)
                {
                    list.RemoveAt(i);
                }
            }
        }
        FillCardsFromList(list);
    }

    async void FillCardsFromList(List<Course> list)
    {
        Grid grid = (Grid)FindByName("Courses");
        StackLayout CourseStack = new StackLayout { Margin = new Thickness(0, 10, 0, 40) };
        for (int i = 0; i < list.Count; i++)
        {
            Grid card = MakeCourseCard(list[i]);
            card.AutomationId = list[i].Id;
            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += ToCourseDescription;
            card.GestureRecognizers.Add(tapGesture);
            CourseStack.Add(card);
        }
        ScrollView scrollView = (ScrollView)FindByName("cardList");
        scrollView.Content = null;
        scrollView.Content = CourseStack;
        
    }

    private void button_Sort(object sender, EventArgs e)
    {
        Sort();
    }
}
