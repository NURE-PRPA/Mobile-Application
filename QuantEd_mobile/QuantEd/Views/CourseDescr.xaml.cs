using Core.Models;

namespace QuantEd.Views;

public partial class CourseDescr : ContentPage
{
	public CourseDescr(Course course)
	{
		InitializeComponent();
        if(course == null)
        {
            Navigation.PopAsync();
        }
        else
        {
            Label lb = (Label)FindByName("name");
            lb.Text = course.Name;
            lb = (Label)FindByName("organization");
            lb.Text = course.Lecturer.Organization.Name;
            lb = (Label)FindByName("topic");
            lb.Text = course.Topic.ToString();
            lb = (Label)FindByName("modules");
            lb.Text = course.Modules.Count.ToString()+ " modules";
            lb = (Label)FindByName("price");
            lb.Text = course.Price + " $";
            lb = (Label)FindByName("description");
            lb.Text = course.Description;
            TableSection table = (TableSection)FindByName("modulesList");
            for(int i = 0; i< course.Modules.Count; i++)
            {
                TextCell cell = new TextCell();
                cell.Text = course.Modules[i].Name;
                cell.Detail = course.Modules[i].Description;
                table.Add(cell);
            }
        }
        Button enroll = (Button)FindByName("enroll");
        if (!MainPage.isAuthorized)
        {
            enroll.IsEnabled = false;
            enroll.IsVisible = false;
        }
    }

    void ToCourses(System.Object sender, System.EventArgs e)
    {
        Navigation.PushModalAsync(new CourseSearch());
    }

    void ViewAccount(System.Object sender, System.EventArgs e)
    {
        Navigation.PushModalAsync(new AccountView());
    }
}
