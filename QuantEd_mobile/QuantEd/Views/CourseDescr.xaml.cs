using Core.Models;
using Newtonsoft.Json;
using Nito.AsyncEx;
using WebAPI.Response.Models;

namespace QuantEd.Views;

public partial class CourseDescr : ContentPage
{
    Course detailedCourse;
    public CourseDescr(Course course)
	{
        detailedCourse = new Course();
        AsyncContext.Run(() => GetCourse(course.Id));
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
            bool my = CourseSearch.isMine;
            for(int i = 0; i< course.Modules.Count; i++)
            {
                TextCell cell = new TextCell();
                cell.Text = course.Modules[i].Name;
                cell.Detail = course.Modules[i].Description;
                cell.AutomationId = course.Modules[i].Id;
                cell.Tapped += ToModuleDescr;
                if (!my)
                {
                    cell.IsEnabled = false;
                }
                table.Add(cell);
                
            }
        }
       
    }

    private void ToModuleDescr(object sender, EventArgs e)
    {
        TextCell cell = (TextCell)sender;
        string modId = cell.AutomationId.ToString();
        foreach(CourseModule mod in detailedCourse.Modules) {
            if(mod.Id == modId)
            {
                Navigation.PushModalAsync(new ModuleDescr(mod));
            }
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

    async void GetCourse(string id)
    {
        var content1 = JsonConvert.SerializeObject(id);
        var httpResponse = await MainPage._client.GetAsync($"{MainPage.BaseAddress}/api/courses/{id}");
        var responseData = JsonConvert.DeserializeObject<Response<Course>>(await httpResponse.Content.ReadAsStringAsync());
        var list = responseData.Content;
        detailedCourse = list;
    }
    
}
