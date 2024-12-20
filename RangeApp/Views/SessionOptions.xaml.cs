using CommunityToolkit.Mvvm.Messaging;
using RangeApp.Models;
namespace RangeApp.Views;

public partial class SessionOptions : ContentPage
{

    private ViewModel.SessionOptionsViewModel VM;
    public SessionOptions()
    {
        InitializeComponent();
        VM = new ViewModel.SessionOptionsViewModel();
        BindingContext = VM;
    }
    public SessionOptions(ViewModel.SessionOptionsViewModel vm)
    {
        InitializeComponent();
        VM = vm;
        BindingContext = vm;
    }
}
