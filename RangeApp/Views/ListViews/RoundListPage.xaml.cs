namespace RangeApp.Views;

public partial class RoundListPage : ContentPage
{
    private readonly ViewModel.RoundListPageViewModel VM;
    public RoundListPage()
    {
        InitializeComponent();
        VM = new ViewModel.RoundListPageViewModel();
        BindingContext = VM;
    }
    public RoundListPage(ViewModel.RoundListPageViewModel vm)
    {
        InitializeComponent();
        VM = vm;
        BindingContext = VM;
    }

}
