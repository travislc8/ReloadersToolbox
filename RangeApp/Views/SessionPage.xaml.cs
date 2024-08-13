using CommunityToolkit.Mvvm.Messaging;
using RangeApp.Models;
namespace RangeApp.Views;

public partial class SessionPage : ContentPage
{
	public SessionPage(ViewModel.SessionPageViewModel vm)
	{
		InitializeComponent();
		VM = vm;
		BindingContext = vm;
        FirearmsInSessionCheckBox.IsChecked = true;
	}
	public SessionPage()
	{
		InitializeComponent();
		VM = new ViewModel.SessionPageViewModel();
		BindingContext = VM;
        FirearmsInSessionCheckBox.IsChecked = true;
	}
	private ViewModel.SessionPageViewModel VM { get; set; }

    private void OnGroupItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        VM.SetSelectedGroup(e.SelectedItemIndex);
    }
	private void ChangeFirearmButtonClicked(object sender, EventArgs e)
	{
		if (FirearmChange.IsVisible)
			FirearmChange.IsVisible = false;
		else
            FirearmChange.IsVisible = true;
    }
    private void FirearmChangeSave(object sender, EventArgs e)
	{
		VM.ChangeFirearm();
		FirearmChange.IsVisible = false;
	}
	private void FirearmChangeCancel(object sender, EventArgs e)
	{
		FirearmChange.IsVisible = false;
	}

    private void OnFirearmSelected(object sender, SelectedItemChangedEventArgs e)
    {
        VM.SetCurrentFirearm(e.SelectedItemIndex);
    }

    private void OnRoundSelected(object sender, SelectedItemChangedEventArgs e)
    {
        VM.SetCurrentRound(e.SelectedItemIndex);
        RoundChange.IsVisible = false;
    }

    private void RoundChangeSave(object sender, EventArgs e)
    {
		VM.ChangeRound();
		RoundChange.IsVisible = false;
    }

    private void RoundChangeCancel(object sender, EventArgs e)
    {
		RoundChange.IsVisible = false;
    }

    private void ChangeRoundButtonClicked(object sender, EventArgs e)
    {
		if (RoundChange.IsVisible) 
			RoundChange.IsVisible = false;
		else
            RoundChange.IsVisible = true;
    }

    async private void FirearmChangeCreateNew(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("NewFirearmPage");
    }

    async private void RoundChangeCreateNew(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("NewRoundPage");
    }

    private void EditGroupButtonClicked(object sender, EventArgs e)
    {
		VM.EditGroup();
    }

    private void DeleteGroupButtonClicked(object sender, EventArgs e)
    {
        VM.DeleteGroup();
    }

    private void SearchFirearmTextChanged(object sender, TextChangedEventArgs e)
    {
        VM.FirearmSearchEntryTextChanged();
    }

    private void OnFirearmInSessionCheckBoxChanged(object sender, CheckedChangedEventArgs e)
    {
        VM.FirearmsInSessionCheckBoxChanged(FirearmsInSessionCheckBox.IsChecked);
    }

    private void RoundsSearchEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        VM.RoundSearchEntryTextChanged();
    }

    private void RoundsInTestQueueChanged(object sender, CheckedChangedEventArgs e)
    {
        VM.RoundsInTestQueueChanged();
    }
}