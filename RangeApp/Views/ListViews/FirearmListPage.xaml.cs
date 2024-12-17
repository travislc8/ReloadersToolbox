namespace RangeApp.Views;

public partial class FirearmListPage : ContentPage
{
	private readonly ViewModel.FirearmListPageViewModel VM; /// Instance of View model

	public FirearmListPage(ViewModel.FirearmListPageViewModel vm)
	{
		InitializeComponent();
		VM = vm;
		BindingContext = VM;
	}

	public FirearmListPage()
	{
		InitializeComponent();
		VM = new ViewModel.FirearmListPageViewModel();
		BindingContext = VM;
	}


}
