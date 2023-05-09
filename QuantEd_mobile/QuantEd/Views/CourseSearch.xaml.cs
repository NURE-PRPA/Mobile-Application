using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Controls.Internals;

namespace QuantEd.Views;

public partial class CourseSearch : ContentPage
{
	public CourseSearch()
	{
		InitializeComponent();
		Grid grid = (Grid)FindByName("Courses");
        StackLayout CourseStack = new StackLayout { Margin = new Thickness(0, 10, 0, 0) };
        for (int i = 0; i< 2; i++)
        {
            Grid card = MakeCourseCard();
            var tapGesture = new ClickGestureRecognizer();
            tapGesture.Clicked += ToCourseDescription;
            card.GestureRecognizers.Add(tapGesture);
            CourseStack.Add(card);
        }
        ScrollView scrollView = new ScrollView { Margin = new Thickness(0, 20, 0, 0), Content = CourseStack};
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
        Navigation.PushModalAsync(new CourseDescr());
    }

    //Parameters is Course
    public static Grid MakeCourseCard()
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
            Margin = new Thickness(10, 10, 10, 0),
           
        };

        var CourseName = new Label
        {
            Text = "C# for everyone",
            TextColor = Color.FromHex("#3D6D79"),
            FontAttributes = FontAttributes.Bold,
            FontFamily = "Jost",
            FontSize = 18
        };

        var ByOrganization = new Label
        {
            Text = "by NURE",
            Margin = new Thickness(0, 4, 0, 0),
            TextColor = Color.FromHex("#8C8D8D"),
            FontAttributes = FontAttributes.Bold,
            FontSize = 15,
            FontFamily = "Jost"
        };

        var CourseTopic = new Label
        {
            Text = "Programming",
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
            Spacing = 13
        };

        var CourseModules = new Label
        {
            Text = "12 modules",
            TextColor = Color.FromHex("#3D6D79"),
            FontFamily = "Jost"
        };

        var CoursePrice = new Label
        {
            Text = "20$",
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
