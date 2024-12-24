namespace RangeApp.Views;

public partial class SessionPage : ContentPage
{
    ViewModel.SessionPageViewModel VM;
    public SessionPage(ViewModel.SessionPageViewModel vm)
    {
        InitializeComponent();
        VM = vm;
        BindingContext = vm;
    }
    public SessionPage()
    {
        InitializeComponent();
        VM = new ViewModel.SessionPageViewModel();
        BindingContext = VM;
    }

}
