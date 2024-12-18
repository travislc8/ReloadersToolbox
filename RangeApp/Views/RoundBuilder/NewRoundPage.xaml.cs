namespace RangeApp.Views;

public partial class NewRoundPage : ContentPage
{
    public NewRoundPage()
    {
        InitializeComponent();
        VM = new ViewModel.NewRoundPageViewModel();
        BindingContext = VM;
    }
    public NewRoundPage(ViewModel.NewRoundPageViewModel vm)
    {
        InitializeComponent();
        VM = vm;
        BindingContext = VM;
    }

    private ViewModel.NewRoundPageViewModel VM;

}
