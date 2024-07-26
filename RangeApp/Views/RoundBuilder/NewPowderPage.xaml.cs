namespace RangeApp.Views;

public partial class NewPowderPage : ContentPage
{
	public NewPowderPage()
	{
		InitializeComponent();
        VM = new ViewModel.NewPowderPageViewModel();
        BindingContext = VM;
        TextColorPass = Title.TextColor;
        TextColorFail = Colors.Red;
	}
    private readonly ViewModel.NewPowderPageViewModel VM;
    private Color TextColorPass;
    private Color TextColorFail;


    private void TypeTextChanged(object sender, TextChangedEventArgs e)
    {
        if(VM.CheckType())
        {
            TypeEntry.TextColor = TextColorPass;
        }
        else
        {
            TypeEntry.TextColor = TextColorFail;
        }
    }

    private void MfgTextChanged(object sender, TextChangedEventArgs e)
    {
        //TODO
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
}