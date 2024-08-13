using CommunityToolkit.Mvvm.Messaging;
using RangeApp.Models;

namespace RangeApp.Views;

public partial class FirearmListPage : ContentPage
{
	public FirearmListPage(ViewModel.FirearmListPageViewModel vm)
	{
		InitializeComponent();
		VM = vm;
		BindingContext = VM;
		WeakReferenceMessenger.Default.Register<Models.SendItemMessage>(this, (r, m) =>
            {
				VM.UpdateList();
            });  
	}
	public FirearmListPage()
	{
		InitializeComponent();
		VM = new ViewModel.FirearmListPageViewModel();
		BindingContext = VM;
		WeakReferenceMessenger.Default.Register<Models.SendItemMessage>(this, (r, m) =>
            {
				VM.UpdateList();
            });  
	}
	private ViewModel.FirearmListPageViewModel VM;

	async void AddNew(object sender, EventArgs e)
	{
		var vm = new ViewModel.NewFirearmPageViewModel();
        await Navigation.PushAsync(new Views.NewFirearmPage(vm));
	}
	async void EditSelected(object sender, EventArgs e)
	{
		var vm = new ViewModel.NewFirearmPageViewModel();
        await Navigation.PushAsync(new Views.NewFirearmPage(vm));
        WeakReferenceMessenger.Default.Send(new SendItemMessage(VM.SelectedName));	
	}
	void SearchInputChanged(object sender, EventArgs e)
	{
		VM.RefineSearch();
	}

}