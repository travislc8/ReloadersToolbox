using CommunityToolkit.Mvvm.Input;

namespace RangeApp.Views;

public partial class RoundListPage : ContentPage
{
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
	private readonly ViewModel.RoundListPageViewModel VM;

}
