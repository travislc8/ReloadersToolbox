﻿namespace RangeApp.Views;

public partial class HomePage : ContentPage
{

	public HomePage()
	{
		InitializeComponent();
		if (Preferences.Get("SessionActive", 0) != 0)
		{
			ContinueSessionButton.IsVisible = true;
		}
		else
		{
			ContinueSessionButton.IsVisible = false;
		}

	}

	private void RoundBuilderClicked(object sender, EventArgs e)
	{

		Shell.Current.GoToAsync("NewRoundPage");
	}
	void ContinueSessionClicked(object sender, EventArgs e)
	{

	}
}

