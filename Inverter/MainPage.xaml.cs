﻿using Inverter.GenerateInverter.Views;

namespace Inverter;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private async void CreateNewInverter(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new InverterV());
	}
	private async void Configuration(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new InverterV());
	}


}

