namespace RangeApp.Views;

public partial class HomePage : ContentPage
{
    private readonly ViewModel.HomePageViewModel VM;
    public HomePage()
    {
        InitializeComponent();
        VM = new ViewModel.HomePageViewModel();
        BindingContext = VM;
        if (Preferences.Get("SessionActive", 0) != 0)
        {
            ContinueSessionButton.IsVisible = true;
            NewSessionButton.IsVisible = false;
        }
        else
        {
            ContinueSessionButton.IsVisible = false;
            NewSessionButton.IsVisible = true;
        }

    }
}

