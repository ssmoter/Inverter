﻿using Inverter.Data;
using Inverter.GenerateInverter.Views;
using Inverter.Helpers;
using Inverter.Sets;
using System.Diagnostics;

namespace Inverter;

public partial class MainPage : ContentPage
{
    FileManager _fm;
    Animation _easterEggAnim;
    public MainPage()
    {
        InitializeComponent();
        _fm = new FileManager();
        FontSize = Config.FontSize;
        BindingContext = this;

        try
        {
            FontSize = int.Parse(_fm.GetConfig(MyEnums.configName.FontSize));
            Config.FontSize = FontSize;
        }
        catch (Exception ex)
        {
            _fm.SaveLog(ex.ToString());
        }
        try
        {
            Config.PspicePath = _fm.GetConfig(MyEnums.configName.PspicePath);
        }
        catch (Exception ex)
        {
            _fm.SaveLog(ex.ToString());
        }
    }


    private async void CreateNewInverter(object sender, EventArgs e)
    {
        Abort();
        await Shell.Current.GoToAsync($"{nameof(InverterV)}");
    }
    private async void Configuration(object sender, EventArgs e)
    {
        Abort();
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
                    await DisplayAlert("Konfiguracja", "Zapisano ścieżkę" + Environment.NewLine + result, "OK");
                }
                else
                    await DisplayAlert("Konfiguracja", "Nie udało się zapisać konfiguracji", "OK");
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
                    await DisplayAlert("Konfiguracja", "Zapisano Rozmiar czcionki" + Environment.NewLine + result, "OK");
                    try
                    {
                        FontSize = int.Parse(result);
                        Config.FontSize = FontSize;
                    }
                    catch
                    { }
                }
                else

                    await DisplayAlert("Konfiguracja", "Nie udało się zapisać konfiguracji", "OK");
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
        StartAnimation();
    }
    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        StartAnimation();
    }
    private void StartAnimation()
    {
        Abort();
        _easterEggAnim = new Animation((e) =>
        {
            iEasterEgg.TranslationX = e;
        }, this.Width, 0);
        iEasterEgg.Animate("MoveX", _easterEggAnim, 16, 10000, Easing.Linear, null, () => true);

        _easterEggAnim = new Animation((e) =>
        {
            iEasterEgg.TranslationY = e;
        }, 0, -this.Height);
        iEasterEgg.Animate("MoveY", _easterEggAnim, 16, 10000, Easing.Linear, null, () => true);
    }
    private void Abort()
    {
        if (iEasterEgg.AnimationIsRunning("MoveX"))
        {
            iEasterEgg.AbortAnimation("MoveX");
        }
        if (iEasterEgg.AnimationIsRunning("MoveY"))
        {
            iEasterEgg.AbortAnimation("MoveY");
        }
    }

    private void Documents_Clicked(object sender, EventArgs e)
    {
        try
        {
            using (var procces = new Process())
            {
                procces.StartInfo.UseShellExecute = true;
                procces.StartInfo.FileName = "https://docs.google.com/document/d/1d8R1OJ626Q3paDIDXYS-lMutd68nYdz1LkT9ipsI6EM/edit?usp=sharing";
                procces.Start();
            }
        }
        catch (Exception ex)
        {
            _fm.SaveLog(ex.ToString());
        }
    }
    private void GitHub_Clicked(object sender, EventArgs e)
    {
        try
        {
            using (var procces = new Process())
            {
                procces.StartInfo.UseShellExecute = true;
                procces.StartInfo.FileName = "https://github.com/ssmoter/Inverter";
                procces.Start();
            }
        }
        catch (Exception ex)
        {
            _fm.SaveLog(ex.ToString());
        }
    }

    private void PointerGestureRecognizer_PointerEntered(object sender, PointerEventArgs e)
    {
        vslInfo.IsVisible = true;
    }

    private async void PointerGestureRecognizer_PointerExited(object sender, PointerEventArgs e)
    {
        int n = 1;

        var t = Task.Run(() =>
         {
             for (int i = 0; i < 1; i++)
             {
                 Thread.Sleep(1000);
                 n++;
             }
         });

        await t;

        if (n >= 1)
        {
            vslInfo.IsVisible = false;
        }
    }

}
