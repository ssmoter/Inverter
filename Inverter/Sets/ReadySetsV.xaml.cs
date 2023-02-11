using Inverter.Data;
using Inverter.Display.Views;
using Inverter.Helpers;
using Inverter.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace Inverter.Sets;

public partial class ReadySetsV : ContentPage
{
    private FileManager _fm;


    public ReadySetsV()
    {
        _fm = new FileManager();
        SetInverterData();

        FontSize = Config.FontSize;
        BindingContext = this;
        InitializeComponent();

        sbFind.MinimumWidthRequest = FontSize * 6;
        sbFind.MaximumWidthRequest = FontSize * 10;
    }

    private void SetInverterData()
    {
        try
        {
            var list = _fm.GetFilesName();
            _InverterDatas = new ObservableCollection<SaveData>();
            for (int i = 0; i < list.Count; i++)
            {
                _InverterDatas.Add(new SaveData()
                {
                    Name = list[i].Replace(_fm.path, "").Remove(0, 1).Replace(".txt", ""),
                    Path = list[i],
                });
            }
            OnPropertyChanged(nameof(InverterDatas));
        }
        catch (Exception ex)
        {
            _fm.SaveLog(ex.ToString());
        }
    }

    private ObservableCollection<SaveData> _InverterDatas;
    public ObservableCollection<SaveData> InverterDatas
    {
        get { return _InverterDatas; }
        set
        {
            _InverterDatas = value;
            OnPropertyChanged(nameof(InverterDatas));
        }
    }

    private SaveData _SelectedData;
    public SaveData SelectedData
    {
        get { return _SelectedData; }
        set
        {
            _SelectedData = value;
            OnPropertyChanged(nameof(SelectedData));
        }
    }


    private int _fontSize = 20;
    public int FontSize
    {
        get => _fontSize;
        set
        {
            _fontSize = value;
            OnPropertyChanged(nameof(FontSize));
        }
    }
    private void vslMain_SizeChanged(object sender, EventArgs e)
    {
        vslMain.MaximumWidthRequest = this.Width;
        vslMain.MaximumHeightRequest = this.Height;

        cvInverterDatas.MaximumWidthRequest = this.Width * 0.8;
        cvInverterDatas.MaximumHeightRequest = this.Height * 0.6;

        hslButtons.MaximumWidthRequest = this.Width * 0.8;
        hslButtons.MaximumHeightRequest = this.Height * 0.6;
    }

    private async void bDelete_Clicked(object sender, EventArgs e)
    {
        try
        {

            if (SelectedData != null)
            {
                if (await DisplayAlert("Usuwanie", $"Czy na pewno chcesz usunąć plik {SelectedData.Name}", "Tak", "Nie"))
                {
                    bool result = _fm.DeleteFile(SelectedData.Path);
                    if (result)
                    {
                        await DisplayAlert("Usuwanie", "Plik został usunięty", "OK");
                        _InverterDatas.Remove(SelectedData);
                        SelectedData = new();
                    }
                    else
                        await DisplayAlert("Configuracja", "Nie udało się usunać pliku", "OK");
                }

            }
            else
            {
                await DisplayAlert("Configuracja", "Nie wybrano pliku", "OK");
            }
        }
        catch (Exception ex)
        {
            _fm.SaveLog(ex.ToString());
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void bLoad_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (SelectedData != null)
            {
                if (await DisplayAlert("Wczytywanie", $"Czy na pewno chcesz wczytać plik {SelectedData.Name}", "Tak", "Nie"))
                {
                    var json = await _fm.LoadDataPath(SelectedData.Path);

                    ResponseModel response = new ResponseModel(SelectedData.Name);
                    response = JsonConvert.DeserializeObject<ResponseModel>(json);
                    response.IsReady = true;

                    await Shell.Current.GoToAsync($"../{nameof(DisplayV)}?",
                        new Dictionary<string, object>
                        {
                            [nameof(ResponseModel)] = response,
                        });
                }
            }
            else
            {
                await DisplayAlert("Configuracja", "Nie wybrano pliku", "OK");
            }
        }
        catch (Exception ex)
        {
            _fm.SaveLog(ex.ToString());
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
    public class SaveData
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }

    private void sbFind_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        try
        {
            if (sbFind != null)
            {
                if (!string.IsNullOrEmpty(sbFind.Text))
                    cvInverterDatas.ItemsSource = InverterDatas.Where(x => x.Name.ToUpper().Contains(sbFind.Text.ToUpper()));
                else
                    cvInverterDatas.ItemsSource = InverterDatas;
            }
        }
        catch (Exception ex)
        {
            _fm.SaveLog(ex.ToString());
        }

    }

}