namespace RangeApp.Views;

public partial class NewRoundPage : ContentPage
{
	public NewRoundPage()
	{
		VM = new ViewModel.NewRoundPageViewModel();
		BindingContext = VM;
		InitializeComponent();
	}
	public NewRoundPage(ViewModel.NewRoundPageViewModel vm)
	{
		InitializeComponent();
        VM = vm;
		BindingContext = VM;
	}

	private ViewModel.NewRoundPageViewModel VM;

    private void PowderNameTextChanged(object sender, TextChangedEventArgs e)
    {
       VM.PowderNameTextChanged();
    }

    private void PowderSelected(object sender, SelectedItemChangedEventArgs e)
    {
		VM.PowderSelected(e.SelectedItemIndex);
    }

    private void BulletNameTextChanged(object sender, TextChangedEventArgs e)
    {
        VM.BulletNameTextChanged();
    }

    private void QueueCheckBoxChanged(object sender, CheckedChangedEventArgs e)
    {
		VM.QueueCheckBoxChecked(e.Value);
    }

    private void BulletByCaliberCheckBoxChanged(object sender, CheckedChangedEventArgs e)
    {
		VM.BulletByCaliberCheckBoxChanged(e.Value);
    }

    private void BulletSelected(object sender, SelectedItemChangedEventArgs e)
    {
        VM.BulletSelected(e.SelectedItemIndex);
    }

    private void FirearmNameTextChanged(object sender, TextChangedEventArgs e)
    {
        VM.FirearmNameTextChanged();
    }

    private void FirearmSelected(object sender, SelectedItemChangedEventArgs e)
    {
        VM.FirearmSelected(e.SelectedItemIndex);
    }
}