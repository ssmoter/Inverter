using Inverter.Data;
using Inverter.GenerateInverter.Views;
using Inverter.Helpers;
using Inverter.Sets;
using Microsoft.Maui.ApplicationModel.DataTransfer;
using Microsoft.Maui.Controls.Shapes;

namespace Inverter;

public partial class MainPage : ContentPage
{
    FileManager _fm;
    Animation _easterEggAnim;
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

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
#if ANDROID
        var status = PermissionStatus.Unknown;

        status = await Permissions.CheckStatusAsync<Permissions.NetworkState>();

        if (status == PermissionStatus.Granted)
        {
            return;
        }
        if (Permissions.ShouldShowRationale<Permissions.NetworkState>())
        {
            await Shell.Current.DisplayAlert("Pozwolenie", "NetworkState", "Tak");
        }
#endif
    }

    private void svMain_SizeChanged(object sender, EventArgs e)
    {
        svMain.MaximumHeightRequest = this.Height;
        svMain.MaximumWidthRequest = this.Width;


        if (iEasterEgg.AnimationIsRunning("Move"))
        {
            iEasterEgg.AbortAnimation("Move");
        }

        double opacity = 0;
        bool directionOpacity = true;
        _easterEggAnim = new Animation((e) =>
        {
            if (directionOpacity)
            {
                opacity += 0.01;
            }
            else
            {
                opacity -= 0.01;
            }
            if (opacity >= 1)
            {
                directionOpacity = false;
            }
            if (opacity <= 0)
            {
                directionOpacity = true;
            }

            iEasterEgg.TranslationX = e;
            iEasterEgg.Opacity = opacity;

        }, this.Width, 0);
        iEasterEgg.Animate("Move", _easterEggAnim, 16, 10000, Easing.Linear, null, () => true);

    }

}
