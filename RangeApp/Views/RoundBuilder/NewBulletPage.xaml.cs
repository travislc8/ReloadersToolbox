namespace RangeApp.Views;

public partial class NewBulletPage : ContentPage
{
	public NewBulletPage()
	{
		InitializeComponent();
        VM = new ViewModel.NewBulletPageViewModel();
        BindingContext = VM;
        TextColorPass = Title.TextColor;
        TextColorFail = Colors.Red;
	}
    private readonly ViewModel.NewBulletPageViewModel VM;
    private Color TextColorPass;
    private Color TextColorFail;

    private void MfgTextChanged(object sender, TextChangedEventArgs e)
    {
        if(VM.CheckMfg())
        {
            MfgEntry.TextColor = TextColorPass;
        }
        else
        {
            MfgEntry.TextColor = TextColorFail;
        }
    }

    private void NameTextChanged(object sender, TextChangedEventArgs e)
    {
        //TODO 
        if(VM.CheckName())
        {
            NameEntry.TextColor = TextColorPass;
            if (!SaveButton.IsEnabled)
                SaveButton.IsEnabled = true;
        }
        else
        {
            NameEntry.TextColor = TextColorFail;
            if (SaveButton.IsEnabled)
                SaveButton.IsEnabled = true;
        }
    }

    private void CaliberTextChanged(object sender, TextChangedEventArgs e)
    {
        if(VM.CheckCaliber())
        {
            CaliberEntry.TextColor = TextColorPass;
        }
        else
        {
            CaliberEntry.TextColor = TextColorFail;
        }
    }

    private void GrainsTextChanged(object sender, TextChangedEventArgs e)
    {
        if(VM.CheckGrains())
        {
            GrainsEntry.TextColor = TextColorPass;
        }
        else
        {
            GrainsEntry.TextColor = TextColorFail;
        }
    }
}
