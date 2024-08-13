using CommunityToolkit.Mvvm.Messaging;
using RangeApp.Models;

namespace RangeApp.Views;

public partial class LocationListPage : ContentPage
{
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
	private ViewModel.LocationListPageViewModel VM;

	async void AddNew(object sender, EventArgs e)
	{
		var vm = new ViewModel.NewLocationPageViewModel();
        await Navigation.PushAsync(new Views.NewLocationPage(vm));
	}
	void SearchInputChanged(object sender, EventArgs e)
	{
		VM.RefineSearch();
	}

}
