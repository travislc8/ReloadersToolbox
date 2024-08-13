namespace RangeApp.Views;

public partial class SessionListPage : ContentPage
{
	public SessionListPage()
	{
		InitializeComponent();
		var vm = new ViewModel.SessionListPageViewModel();
		VM = vm;
		BindingContext = VM;
		
	}
	ViewModel.SessionListPageViewModel VM;
}