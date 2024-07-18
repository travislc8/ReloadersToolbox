using Android.Provider;
using CommunityToolkit.Mvvm.Messaging;
using RangeApp.Models;
namespace RangeApp.Views;

public partial class NewGroupPage : ContentPage
{
	public NewGroupPage(ViewModel.NewGroupPageViewModel vm)
	{
		InitializeComponent();
		VM = vm;
		BindingContext = vm;
		UnitPicker.SelectedIndex = 0;
	}
	public NewGroupPage()
	{
		InitializeComponent();
		VM = new ViewModel.NewGroupPageViewModel();
		BindingContext = VM;
		UnitPicker.SelectedIndex = 0;
	}
	private ViewModel.NewGroupPageViewModel VM { get; set; }
    public List<string> Unit { get; set; } = new List<string> { "FPS", "MPS" };

    private void UnitPickerIndexChanged(object sender, EventArgs e)
    {
		VM.UnitChanged(UnitPicker.Title);
    }

    private void OnShotSelected(object sender, SelectedItemChangedEventArgs e)
    {
		VM.ShotSelected(e.SelectedItemIndex);
    }

    private void EditShotClicked(object sender, EventArgs e)
    {
		if (VM.EditShot())
		{
			UpdateShotButton.IsVisible = true;
			CancelUpdateShotButton.IsVisible = true;
			AddShotButton.IsVisible = false;
		}
    }
	private void UpdateShotClicked(object sender, EventArgs e)
	{
		VM.UpdateShot();
		UpdateShotButton.IsVisible = false;
		CancelUpdateShotButton.IsVisible = false;
		AddShotButton.IsVisible = true;
	}
	private void CancelUpdateShotClicked(object sender, EventArgs e)
	{

		VM.CancelUpdateShot();
		UpdateShotButton.IsVisible = false;
		CancelUpdateShotButton.IsVisible = false;
		AddShotButton.IsVisible = true;
	}
}

