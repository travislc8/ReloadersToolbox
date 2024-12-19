namespace RangeApp.Views;

public partial class LocationListPage : ContentPage
{
    private ViewModel.LocationListPageViewModel VM;
    public LocationListPage(ViewModel.LocationListPageViewModel vm)
    {
        InitializeComponent();
        VM = vm;
        BindingContext = VM;
    }
    public LocationListPage()
    {
        InitializeComponent();
        VM = new ViewModel.LocationListPageViewModel();
        BindingContext = VM;
    }

}
