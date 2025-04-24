using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Tekla.Structures;
using Tekla.Structures.Drawing;
using Tekla.Structures.Drawing.UI;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using Tekla.Structures.Model.Operations;
using Object = Tekla.Structures.Model.Object;
using Operation = Tekla.Structures.Analysis.Operations.Operation;

// ReSharper disable LocalizableElement

namespace Drawing.CenterView;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public abstract partial class QuickCenterClass
{
    //private static readonly Model Model = new Model();
    private static readonly DrawingHandler DrawingHandler = new DrawingHandler();

    public static void EntryPoint()
    {
        var drawingSelector = DrawingHandler.GetDrawingSelector();
        var selectedSize = drawingSelector.GetSelected().GetSize();

        if (selectedSize <= 0)
        {
            const string boxTitle = "Center All Drawings?";
            const string boxQuestion = "No drawings are currently selected.\n\n" +
                                       "Proceeding will affect all erection drawings.";
            var boxResult = MessageBox.Show(boxQuestion,
                boxTitle,
                MessageBoxButtons.OKCancel,
                0,
                0,
                MessageBoxOptions.DefaultDesktopOnly);

            switch (boxResult)
            {
                case DialogResult.OK:
                    GotoCenterAll();
                    break;
                case DialogResult.Cancel:
                    Tekla.Structures.Model.Operations.Operation.DisplayPrompt("Aborting.");
                    return;
            }
        }
        else
        {
            const string boxTitle = "Center All Drawings?";
            const string boxQuestion = "Should ONLY the selected erection drawings be centered?\n\n" +
                                       "  Yes = Center only selected\n" +
                                       "  No = Center all";
            var boxResult = MessageBox.Show(boxQuestion,
                boxTitle,
                MessageBoxButtons.YesNoCancel,
                0,
                0,
                MessageBoxOptions.DefaultDesktopOnly);

            switch (boxResult)
            {
                case DialogResult.Yes:
                    CenterSelectedDrawings(drawingSelector.GetSelected());
                    break;
                case DialogResult.No:
                    GotoCenterAll();
                    break;
                case DialogResult.Cancel:
                    Tekla.Structures.Model.Operations.Operation.DisplayPrompt("Aborting.");
                    return;
                case DialogResult.None:
                case DialogResult.OK:
                case DialogResult.Abort:
                case DialogResult.Retry:
                case DialogResult.Ignore:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            //var testArray = selectedList.ToArray(typeof(GADrawing));

            // foreach (GADrawing dwg in testArray)
            // {
            //     Console.WriteLine(dwg.Name.ToString());
            // }

            //CenterSelectedDrawings(selectedList);
        }

        Tekla.Structures.Model.Operations.Operation.DisplayPrompt("Done.");
    }

    private static void GotoCenterAll()
    {
        Tekla.Structures.Model.Operations.Operation.DisplayPrompt("Centering Drawings...");
        var allDrawings = DrawingHandler.GetDrawings();
        var allGADrawings = new ArrayList();
        while (allDrawings.MoveNext())
            if (allDrawings.Current is GADrawing)
                allGADrawings.Add(allDrawings.Current);
        CenterAllDrawings(allGADrawings);
    }

    private static void CenterSelectedDrawings(DrawingEnumerator selectedGADrawings)
    {
        var reportStringBuilder = new StringBuilder();
        Tuple<Tekla.Structures.Drawing.Drawing, string> s = null;
        while (selectedGADrawings.MoveNext())
        {
            var memberCount = 0;
            var views = selectedGADrawings.Current.GetSheet().GetViews();
            var filteredDrawings = new ArrayList();

            while (views.MoveNext())
            {
                views.Current.GetStringUserProperties(out Dictionary<string, string> viewTypes);
                var type = PluginForm.GetViewTypeEnum(viewTypes);
                if (type is not PluginForm.ViewType.None) memberCount++;
                //Console.WriteLine(PluginForm.GetViewTypeEnum(viewTypes).ToString());
            }

            if (memberCount != 1) continue;
            filteredDrawings.Add(selectedGADrawings.Current);
            var filteredDrawingsArray = filteredDrawings.ToArray(typeof(GADrawing));

            foreach (GADrawing drawing in filteredDrawingsArray)
            {
                DrawingHandler.SetActiveDrawing(drawing);
                var allViews = DrawingHandler.GetActiveDrawing().GetSheet().GetAllViews();
                while (allViews.MoveNext())
                {
                    allViews.Current.GetStringUserProperties(out Dictionary<string, string> viewType);
                    var currentView = (ViewBase)allViews.Current;
                    try
                    {
                        var reportString = CenterView(currentView, (int)PluginForm.GetViewTypeEnum(viewType), out s);
                        reportStringBuilder.AppendLine(reportString);
                        s.Item1.Title3 = s.Item2.ToString();
                        s.Item1.Modify();
                    }
                    catch (Exception e) when (e is KeyNotFoundException)
                    {
                        Console.WriteLine(@"Invalid View: " + currentView.ToString());
                    }
                }


                DrawingHandler.CloseActiveDrawing();
                if (s != null)
                {
                    s.Item1.Title3 = s.Item2.ToString();
                    s.Item1.Modify();
                }
            }
            //Console.WriteLine("Candidate for centering drawing");

            //Console.WriteLine(views.Current);
        }

        GenerateAndDisplayReport("Centered_Report", reportStringBuilder.ToString());
        selectedGADrawings.Reset();
        while (selectedGADrawings.MoveNext())
        {
            selectedGADrawings.Current.Title3 = "";
            selectedGADrawings.Current.Modify();
        }
    }

    private static void CenterAllDrawings(ArrayList drawings)
    {
        var reportStringBuilder = new StringBuilder();
        Tuple<Tekla.Structures.Drawing.Drawing, string> s;
        foreach (var gaDwg in drawings)
        {
            var dwg = (Tekla.Structures.Drawing.Drawing)gaDwg;
            var memberCount = 0;
            var views = dwg.GetSheet().GetViews();
            var filteredDrawings = new ArrayList();

            while (views.MoveNext())
            {
                views.Current.GetStringUserProperties(out Dictionary<string, string> viewTypes);
                var type = PluginForm.GetViewTypeEnum(viewTypes);
                if (type is not PluginForm.ViewType.None) memberCount++;
                //Console.WriteLine(PluginForm.GetViewTypeEnum(viewTypes).ToString());
            }

            if (memberCount != 1) continue;
            filteredDrawings.Add(gaDwg);
            var filteredDrawingsArray = filteredDrawings.ToArray(typeof(GADrawing));

            foreach (GADrawing drawing in filteredDrawingsArray)
            {
                DrawingHandler.SetActiveDrawing(drawing);
                var allViews = DrawingHandler.GetActiveDrawing().GetSheet().GetAllViews();
                while (allViews.MoveNext())
                {
                    allViews.Current.GetStringUserProperties(out Dictionary<string, string> viewType);
                    var currentView = (ViewBase)allViews.Current;
                    try
                    {
                        var reportString = CenterView(currentView, (int)PluginForm.GetViewTypeEnum(viewType), out s);
                        reportStringBuilder.AppendLine(reportString);
                        s.Item1.Title3 = s.Item2.ToString();
                        s.Item1.Modify();
                    }
                    catch (Exception e) when (e is KeyNotFoundException)
                    {
                        Console.WriteLine(@"Invalid View: " + currentView.ToString());
                    }
                }
            }

            // views.Reset();
            // while (views.MoveNext())
            // {
            //     views.Current.GetStringUserProperties(out Dictionary<string, string> viewTypes);
            //     var type = PluginForm.GetViewTypeEnum(viewTypes);
            //     ViewBase? viewsCurrent = (ViewBase)views.Current;
            //     CenterView(ref viewsCurrent, (int)type);
            // }
            //Console.WriteLine("Candidate for centering drawing");
        }

        DrawingHandler.CloseActiveDrawing();
        GenerateAndDisplayReport("Centered_Report", reportStringBuilder.ToString());
        foreach (GADrawing drawing in drawings)
        {
            drawing.Title3 = "";
            drawing.Modify();
        }
    }

    private static string CenterView(ViewBase view, int viewType, out Tuple<Tekla.Structures.Drawing.Drawing, string> s)
    {
        var sheet = view.GetDrawing().GetSheet();
        double sheetHeightOffset = 0;
        switch (viewType)
        {
            case 1:
                sheetHeightOffset = 25.4; // 1"
                break;
            case >= 2 and <= 24:
                sheetHeightOffset = 22.225; // 7/8"
                break;
            default: break;
        }

        Console.WriteLine($"Sheet origin: {sheet.Origin.ToString()}");
        sheet.Origin.Y = sheetHeightOffset;
        //sheet.Modify();
        var originalOriginX = view.Origin.X;
        var originalOriginY = view.Origin.Y;
        view.Origin = sheet.Origin;
        //view.Modify();
        Console.WriteLine($"Sheet origin: {sheet.Origin.ToString()}");

        var viewCenterPoint = view.GetAxisAlignedBoundingBox().GetCenterPoint();

        var sheetHeight = sheet.Height / 2;
        var sheetWidth = (sheet.Width - 33.274) / 2;
        var xOffset = sheetWidth - viewCenterPoint.X;
        var yOffset = sheetHeight - viewCenterPoint.Y;

        Console.WriteLine($"View Center:  {viewCenterPoint.ToString()}");
        Console.WriteLine(
            $"Sheet Height: {sheetHeight.ToString(CultureInfo.InvariantCulture)}\nSheet Width: {sheetWidth.ToString(CultureInfo.InvariantCulture)}");
        Console.WriteLine($"View Origin: {view.Origin.ToString()}");
        Console.WriteLine($"x offset: {xOffset}, y offset: {yOffset}");

        if (Math.Abs(originalOriginX - xOffset) < 0.0001 &&
            Math.Abs(originalOriginY - yOffset - sheetHeightOffset) < 0.0001)
        {
            s = new Tuple<Tekla.Structures.Drawing.Drawing, string>(view.GetDrawing(), "NC");
            return $@"Nothing To Do. {view.GetDrawing().Name} => {(PluginForm.ViewType)viewType}";
        }
        else if (Math.Abs(view.ExtremaCenter.X - sheetWidth) > 0.0001 ||
                 Math.Abs(view.ExtremaCenter.Y - sheetHeight) > 0.0001)
        {
            view.Origin.X += xOffset;
            view.Origin.Y += yOffset;
            view.Modify();
            s = new Tuple<Tekla.Structures.Drawing.Drawing, string>(view.GetDrawing(), "C");
            DrawingHandler.GetActiveDrawing().CommitChanges("Center View");
            DrawingHandler.SaveActiveDrawing();
            Console.WriteLine(view.Origin.ToString());
            return $"Centering {view.GetDrawing().Name} => {(PluginForm.ViewType)viewType}";
        }

        s = new Tuple<Tekla.Structures.Drawing.Drawing, string>(view.GetDrawing(), "X");
        return $"Something Went Wrong At {view.GetDrawing().Name} => " + (PluginForm.ViewType)viewType;
    }
}