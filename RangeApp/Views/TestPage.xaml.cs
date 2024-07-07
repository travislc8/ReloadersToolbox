using RangeApp.Models;
namespace RangeApp.Views;

public partial class  TestPage : ContentPage
{
	public TestPage()
	{
		InitializeComponent();
		BindingContext = new RangeApp.ViewModel.TestPageViewModel();
    }


}