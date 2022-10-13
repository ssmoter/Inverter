namespace Inverter;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
        var width = (int)MainPage.WidthRequest;
        var height = (int)MainPage.HeightRequest;
    }
}
