using Inverter.Data;
using Inverter.GenerateInverter.Views;
using Inverter.Helpers;
using Inverter.Sets;

namespace Inverter;

public partial class MainPage : ContentPage
{
    FileManager _fm;
    public MainPage()
    {
        InitializeComponent();
        _fm = new FileManager();
        BindingContext = this;
        try
        {
            FontSize = int.Parse(_fm.GetConfig(MyEnums.configName.FontSize));
            Config.FontSize = FontSize;
            Config.PspicePath = _fm.GetConfig(MyEnums.configName.PspicePath);
        }
        catch (Exception ex)
        {
            _fm.SaveLog(ex.ToString());
        }
    }

    private async void CreateNewInverter(object sender, EventArgs e)
    {

        await Shell.Current.GoToAsync($"{nameof(InverterV)}");
    }
    private async void Configuration(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(ReadySetsV)}");
    }

    private async void PspiceLocation_Clicked(object sender, EventArgs e)
    {
        try
        {
            string result = string.Empty;
            result = _fm.GetConfig(MyEnums.configName.PspicePath);
            result = await DisplayPromptAsync("Konfiguracja", "Podaj ścieżkę do Pspice", "OK", "cancel", result);


            if (!string.IsNullOrWhiteSpace(result))
            {
                if (!result.Contains(".exe"))
                {
                    result += ".exe";
                }
                bool isComplited = await _fm.CreateConfig(result, MyEnums.configName.PspicePath);
                if (isComplited)
                {
                    Config.PspicePath = result;
                    await DisplayAlert("Configuracja", "Zapisano ścieżkę" + Environment.NewLine + result, "OK");
                }
                else
                    await DisplayAlert("Configuracja", "Nie udało się zapisać konfiguracji", "OK");
            }
        }
        catch (Exception ex)
        {
            _fm.SaveLog(ex.ToString());
        }
    }
    private int _fontSize = 14;
    public int FontSize
    {
        get => _fontSize;
        set
        {
            _fontSize = value;
            OnPropertyChanged(nameof(FontSize));
        }
    }

    private async void FontSize_Clicked(object sender, EventArgs e)
    {
        try
        {
            string result = string.Empty;
            result = _fm.GetConfig(MyEnums.configName.FontSize);
            try
            {
                FontSize = int.Parse(result);
            }
            catch (Exception ex)
            {
                _fm.SaveLog(ex.ToString());
            }
            result = await DisplayPromptAsync("Konfiguracja", "Rozmiar czcionki", "OK", "cancel", result);
            if (!string.IsNullOrWhiteSpace(result))
            {
                bool isComplited = await _fm.CreateConfig(result, MyEnums.configName.FontSize);
                if (isComplited)
                {
                    await DisplayAlert("Configuracja", "Zapisano Rozmiar czcionki" + Environment.NewLine + result, "OK");
                    try
                    {
                        FontSize = int.Parse(result);
                        Config.FontSize = FontSize;
                    }
                    catch
                    { }
                }
                else

                    await DisplayAlert("Configuracja", "Nie udało się zapisać konfiguracji", "OK");
            }

        }
        catch (Exception ex)
        {
            _fm.SaveLog(ex.ToString());
        }
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        // Application.Current.OpenWindow(new Window
        // {
        //     Page = new MainPage()
        // });
    }

    private void svMain_SizeChanged(object sender, EventArgs e)
    {
        svMain.MaximumHeightRequest = this.Height;
        svMain.MaximumWidthRequest = this.Width;
    }
}

