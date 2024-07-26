namespace RangeApp.Views;

public partial class HomePage : ContentPage
{

	public HomePage()
	{
		InitializeComponent();
	}

    private void RoundBuilderClicked(object sender, EventArgs e)
    {
		Shell.Current.GoToAsync("NewRoundPage");
    }
}

