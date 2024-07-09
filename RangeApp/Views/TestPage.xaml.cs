using CommunityToolkit.Mvvm.Messaging;
using RangeApp.Models;
namespace RangeApp.Views;

public partial class TestPage : ContentPage
{
	public TestPage()
	{
		InitializeComponent();
		BindingContext = new RangeApp.ViewModel.TestPageViewModel();
		WeakReferenceMessenger.Default.Register<SendItemMessage>(this, (r, m) =>
            {
				TestLabel.Text = m.Value.ToString();
            });  
	}


    private async void GoToAddNewFirearmPage(object sender, EventArgs e)
    {
		var vm = new ViewModel.NewFirearmPageViewModel();
		await Navigation.PushAsync(new Views.NewFirearmPage(vm));

    }
}