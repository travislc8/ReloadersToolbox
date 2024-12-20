namespace RangeApp.Views;

public partial class ModifyNavigationPage : ContentPage
{

    ViewModel.ModifyDataPageViewModel VM;
	public ModifyNavigationPage()
	{
		InitializeComponent();
        VM = new ViewModel.ModifyDataPageViewModel();
        BindingContext = VM;
	}

}
