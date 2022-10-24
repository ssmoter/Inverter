using Inverter.Data;
using Inverter.Models;

namespace Inverter.Display.Views;

public partial class DisplayV : ContentPage
{
	private FileManager _FileManager;
	public DisplayV(ResponseModel responseModel)
	{
		_FileManager = new();
		responseModel = _FileManager.OpenFile().Result.Mapping(responseModel);


		InitializeComponent();
		labelTest.Text = responseModel.FileDataPath;
	}
}