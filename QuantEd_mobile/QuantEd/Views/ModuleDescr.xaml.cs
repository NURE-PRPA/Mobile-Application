using Core.Models;

namespace QuantEd.Views;

public partial class ModuleDescr : ContentPage
{
	CourseModule module;
	public ModuleDescr(CourseModule module)
	{
		InitializeComponent();
		this.module = module;
		FillPage();
	}


	void FillPage()
	{
		Label label = (Label)FindByName("name");
		label.Text = module.Name;
		label = (Label)FindByName("description");
		label.Text = module.Description;
        label = (Label)FindByName("questNum");
		if (module.Test != null)
		{
			label.Text = module.Test.Questions.Count.ToString() + " questions";
		}

    }
    private void testStart_Clicked(object sender, EventArgs e)
    {
		Navigation.PushAsync(new Test(module.Test.Id));
    }
}