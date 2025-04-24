using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Tekla.Structures.Drawing;
using Tekla.Structures.Drawing.UI;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using Tekla.Structures.Model.Operations;

// ReSharper disable LocalizableElement

namespace Drawing.CenterView;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public abstract class QuickCenterClass
{
    //private static readonly Model Model = new Model();
    private static readonly DrawingHandler DrawingHandler = new DrawingHandler();

    public static void EntryPoint()
    {
        var drawingSelector = DrawingHandler.GetDrawingSelector();
        var selectedSize = drawingSelector.GetSelected().GetSize();

        if (selectedSize <= 0)
        {
            var allDrawings = DrawingHandler.GetDrawings();
            var allGADrawings = new ArrayList();
            while (allDrawings.MoveNext())
                if (allDrawings.Current is GADrawing)
                    allGADrawings.Add(allDrawings.Current);

            CenterAllDrawings(allGADrawings);
        }
        else
        {
            Console.WriteLine("Selected size is " + selectedSize);
            var selectedList = new ArrayList();
            if (selectedList == null) throw new ArgumentNullException(nameof(selectedList));
            var selectedDrawings = drawingSelector.GetSelected();

            while (selectedDrawings.MoveNext())
            {
                var drawing = selectedDrawings.Current as GADrawing;
                //Console.WriteLine(drawing.Name.ToString());
                selectedList.Add(drawing);
                //selectedList.Add(drawingSelector.GetSelected().Current);
            }

            //var testArray = selectedList.ToArray(typeof(GADrawing));

            // foreach (GADrawing dwg in testArray)
            // {
            //     Console.WriteLine(dwg.Name.ToString());
            // }

            //CenterSelectedDrawings(selectedList);

            CenterSelectedDrawings(drawingSelector.GetSelected());
        }
    }

    private static void CenterSelectedDrawings(DrawingEnumerator selectedGADrawings)
    {
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
                        CenterView(currentView, (int)PluginForm.GetViewTypeEnum(viewType));
                    }
                    catch (Exception e) when (e is KeyNotFoundException)
                    {
                        Console.WriteLine(@"Invalid View: " + currentView.ToString());
                    }
                }

                DrawingHandler.CloseActiveDrawing();
            }
            //Console.WriteLine("Candidate for centering drawing");

            //Console.WriteLine(views.Current);
        }
    }

    private static void CenterAllDrawings(ArrayList drawings)
    {
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
                        CenterView(currentView, (int)PluginForm.GetViewTypeEnum(viewType));
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
    }

    private static void CenterView(ViewBase view, int viewType)
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
        view.Origin = sheet.Origin;
        //view.Modify();
        Console.WriteLine($"Sheet origin: {sheet.Origin.ToString()}");

        var viewCenterPoint = view.GetAxisAlignedBoundingBox().GetCenterPoint();

        var sheetHeight = sheet.Height / 2;
        var sheetWidth = (sheet.Width - 33.274) / 2;
        var xOffset = sheetWidth - viewCenterPoint.X;
        var yOffset = sheetHeight - viewCenterPoint.Y;

        Console.WriteLine($"View Center:  {viewCenterPoint.ToString()}");
        Console.WriteLine($"Sheet Height: {sheetHeight.ToString(CultureInfo.InvariantCulture)}\nSheet Width: {sheetWidth.ToString(CultureInfo.InvariantCulture)}");
        Console.WriteLine($"View Origin: {view.Origin.ToString()}");
        Console.WriteLine($"x offset: {xOffset}, y offset: {yOffset}");

        switch (Math.Abs(view.ExtremaCenter.X - sheetWidth))
        {
            case > 0.0001 when Math.Abs(view.ExtremaCenter.Y - sheetHeight) > 0.0001:
                //InfoBox.OnInfo(infoBox, $"Centering {(ViewType)viewType}");
                view.Origin.X += xOffset;
                view.Origin.Y += yOffset;
                view.Modify();
                DrawingHandler.GetActiveDrawing().CommitChanges("Center View");
                DrawingHandler.SaveActiveDrawing();
                Console.WriteLine(view.Origin.ToString());
                break;
            default:
                //InfoBox.OnInfo(infoBox, @"Nothing To Do.");
                break;
        }
    }
}

/*
public static void CenterView(ref ViewBase? view, int viewType)
{
    //DrawingHandler.SetActiveDrawing(view.GetDrawing());
    var sheet = view?.GetDrawing().GetSheet();
    double sheetHeightOffset = 0;
    switch (viewType)
    {
        case 1:
            sheetHeightOffset = 25.4; // 1"
            break;
        case >=2 and <= 24:
            sheetHeightOffset = 22.225; // 7/8"
            break;
        default: break;
    }

    // Console.WriteLine($"View Origin: {view.Origin.ToString()}");
    sheet.Origin.Y = sheetHeightOffset;
    sheet.Modify();
    view.Origin = sheet.Origin;
    view.Modify();
    //Console.WriteLine($"Sheet origin: {sheet.Origin.ToString()}");
    // Console.WriteLine($"Sheet origin: {sheet.Origin.ToString()}");
    //Console.WriteLine($"View Origin: {view.Origin.ToString()}");

    //var cent = new Point(view.Origin.X + (view.GetView().Width)/2, view.Origin.Y + (view.GetView().Height)/2, 0.0);
    var viewCenterPoint = view.GetAxisAlignedBoundingBox().GetCenterPoint();

    //Console.WriteLine($"view height: {view.GetView().Height}, view width: {view.GetView().Width}");

    var sheetHeight = (sheet.Height)/2;
    var sheetWidth = (sheet.Width - 33.274)/2;
    var xOffset =  sheetWidth - viewCenterPoint.X;
    var yOffset = sheetHeight - viewCenterPoint.Y;

    // Console.WriteLine($"View Center:  {viewCenterPoint.ToString()}");
    //  Console.WriteLine($"Sheet Height: {sheetHeight.ToString()}\nSheet Width: {sheetWidth.ToString()}");
    //  Console.WriteLine($"View Origin: {view.Origin.ToString()}");
    // Console.WriteLine($"xoffset: {xOffset}, yoffset: {yOffset}");


    switch (Math.Abs(viewCenterPoint.X - sheetWidth))
    {
        case > 0.0001 when Math.Abs(viewCenterPoint.Y - sheetHeight) > 0.0001:
            view.Origin.X += xOffset;
            Console.WriteLine(view.Origin.X);
            view.Origin.Y += yOffset;
            Console.WriteLine(view.Origin.Y);
            view.Modify();
            Operation.DisplayPrompt("Centered view in: " + view.GetDrawing().Name);
            sheet.GetDrawing().CommitChanges("Center View: " + view.GetDrawing().Name);
            sheet.GetDrawing().Modify();
            //Console.WriteLine($"View Origin Final: {view.Origin.ToString()}");
            //DrawingHandler.GetActiveDrawing().CommitChanges("Center View");
            break;
        default:
            Operation.DisplayPrompt(@"Nothing To Do. => " + view.GetDrawing().Name);
            break;
    }
}
*/
//}