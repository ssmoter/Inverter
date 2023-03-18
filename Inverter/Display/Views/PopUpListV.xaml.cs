using CommunityToolkit.Maui.Views;
using Inverter.Display.ViewsModel;

namespace Inverter.Display.Views;

public partial class PopUpListV : Popup {
    public PopUpListV(PopUpListVM vm) {
        InitializeComponent();
        this.BindingContext = vm;
    }
    public PopUpListV() {

    }
}