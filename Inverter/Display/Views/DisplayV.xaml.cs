using Inverter.Data;
using Inverter.Data.Draw;
using Inverter.Display.ViewsModel;
using Inverter.Helpers;
using Inverter.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Inverter.Display.Views;

public partial class DisplayV : ContentPage
{ 
    public DisplayV(DisplayVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private void Initialization()
    {
        _axis = (Axis)_graphicsDraw.Drawable;
    }
    private GraphicsView _graphicsDraw;
    private Axis _axis;







}