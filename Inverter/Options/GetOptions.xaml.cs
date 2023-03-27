using Inverter.Data;
using Inverter.Helpers;

namespace Inverter.Options;

public partial class GetOptions : ContentPage {
    FileManager _fm;


    private int _fontSize;
    public int FontSize {
        get => _fontSize;
        set {
            if (_fontSize != value) {
                _fontSize = value;
                OnPropertyChanged(nameof(FontSize));
            }
        }
    }


    public GetOptions() {
        InitializeComponent();
        _fm = new FileManager();
        FontSize = Config.FontSize;
        Initialize();
        BindingContext = this;
    }
    private async void Initialize() {
        try {

            this.cvLoggi.ItemsSource = await _fm.LoadLog();
            svLoggi.MaximumHeightRequest = this.Height * 0.7;
            svLoggi.MaximumWidthRequest = this.Width * 0.7;
        }
        catch {
        }
    }

    private async void PspiceLocation_Clicked(object sender, EventArgs e) {
        try {
            string result = string.Empty;
            result = _fm.GetConfig(MyEnums.configName.PspicePath);
            result = await DisplayPromptAsync("Konfiguracja", "Podaj ścieżkę do Pspice", "OK", "Anuluj", result);


            if (!string.IsNullOrWhiteSpace(result)) {
                if (!result.Contains(".exe")) {
                    result += ".exe";
                }
                bool isComplited = await _fm.CreateConfig(result, MyEnums.configName.PspicePath);
                if (isComplited) {
                    Config.PspicePath = result;
                    await DisplayAlert("Konfiguracja", "Zapisano ścieżkę" + Environment.NewLine + result, "OK");
                }
                else
                    await DisplayAlert("Konfiguracja", "Nie udało się zapisać konfiguracji", "OK");
            }
        }
        catch (Exception ex) {
            _fm.SaveLog(ex.ToString());
        }
    }
    private async void FontSize_Clicked(object sender, EventArgs e) {
        try {
            string result = string.Empty;
            result = _fm.GetConfig(MyEnums.configName.FontSize);
            try {
                FontSize = int.Parse(result);
            }
            catch (Exception ex) {
                _fm.SaveLog(ex.ToString());
            }
            result = await DisplayPromptAsync("Konfiguracja", "Rozmiar czcionki", "OK", "Anuluj", result);
            if (!string.IsNullOrWhiteSpace(result)) {
                bool isComplited = await _fm.CreateConfig(result, MyEnums.configName.FontSize);
                if (isComplited) {
                    await DisplayAlert("Konfiguracja", "Zapisano Rozmiar czcionki" + Environment.NewLine + result, "OK");
                    try {
                        FontSize = int.Parse(result);
                        Config.FontSize = FontSize;
                    }
                    catch { }
                }
                else

                    await DisplayAlert("Konfiguracja", "Nie udało się zapisać konfiguracji", "OK");
            }

        }
        catch (Exception ex) {
            _fm.SaveLog(ex.ToString());
        }
    }

    private async void GraphSize_Clicked(object sender, EventArgs e) {
        try {
            string result = Config.GraphSize.ToString();

            result = await DisplayPromptAsync("Konfiguracja", "Minimalna wysokość okna z wykresami", "OK", "Anuluj", result);
            if (!string.IsNullOrWhiteSpace(result)) {
                bool isComplited = await _fm.CreateConfig(result, MyEnums.configName.GraphSize);
                Config.GraphSize = int.Parse(result);
                if (isComplited) {
                    await DisplayAlert("Konfiguracja", "Zapisano Wysokość okna" + Environment.NewLine + result, "OK");
                }
                else
                    await DisplayAlert("Konfiguracja", "Nie udało się zapisać konfiguracji", "OK");
            }
        }
        catch (Exception ex) {
            _fm.SaveLog(ex.ToString());
        }
    }

    private void ScrollView_SizeChanged(object sender, EventArgs e) {
        svMain.MaximumHeightRequest = this.Height;
        svMain.MaximumWidthRequest = this.Width;
        svLoggi.MaximumHeightRequest = this.Height * 0.7;
        svLoggi.MaximumWidthRequest = this.Width * 0.7;
    }


}