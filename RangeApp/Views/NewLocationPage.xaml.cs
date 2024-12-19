namespace RangeApp.Views;

public partial class NewLocationPage : ContentPage
{
    private ViewModel.NewLocationPageViewModel VM;
    public NewLocationPage(ViewModel.NewLocationPageViewModel vm)
    {
        InitializeComponent();
        VM = vm;
        BindingContext = vm;
    }
    public NewLocationPage()
    {
        InitializeComponent();
        var vm = new ViewModel.NewLocationPageViewModel();
        VM = vm;
        BindingContext = vm;
    }


}
