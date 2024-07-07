using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Windows.Input;
using RangeApp.Models;

namespace RangeApp.ViewModel;

public partial class SessionOptionsViewModel : ObservableObject
{
	[ObservableProperty]
	private List<Firearm>? firearmsInSession;
	public SessionOptionsViewModel()
	{
		FirearmsInSession = [];
	}

}
