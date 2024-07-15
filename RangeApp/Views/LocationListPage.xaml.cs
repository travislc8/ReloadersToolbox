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
		WeakReferenceMessenger.Default.Register<Models.SendItemMessage>(this, (r, m) =>
            {
				VM.UpdateList();
            });  
	}
	public LocationListPage()
	{
		InitializeComponent();
		VM = new ViewModel.LocationListPageViewModel();
		BindingContext = VM;
		WeakReferenceMessenger.Default.Register<Models.SendItemMessage>(this, (r, m) =>
            {
				VM.UpdateList();
            });  
	}
	private ViewModel.LocationListPageViewModel VM;

	async void AddNew(object sender, EventArgs e)
	{
		var vm = new ViewModel.NewLocationPageViewModel();
        await Navigation.PushAsync(new Views.NewLocationPage(vm));
	}
	async void EditSelected(object sender, EventArgs e)
	{
		var vm = new ViewModel.NewLocationPageViewModel();
        await Navigation.PushAsync(new Views.NewLocationPage(vm));
        WeakReferenceMessenger.Default.Send(new SendItemMessage(VM.SelectedName));	
	}
	void SearchInputChanged(object sender, EventArgs e)
	{
		VM.RefineSearch();
	}

}
