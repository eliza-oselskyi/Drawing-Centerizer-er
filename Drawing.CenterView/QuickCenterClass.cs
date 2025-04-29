/*
 *
 *  Drawing Centerizer-er: Mainly centers Tekla drawings, specifically NBG's flavor.
 *       Copyright (C) 2025  Eliza Oselskyi
 *
 *       This program is free software: you can redistribute it and/or modify
 *       it under the terms of the GNU Lesser General Public License as published by
 *       the Free Software Foundation, either version 3 of the License, or
 *       (at your option) any later version.
 *
 *       This program is distributed in the hope that it will be useful,
 *       but WITHOUT ANY WARRANTY; without even the implied warranty of
 *       MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *       GNU Lesser General Public License for more details.
 *
 *       You should have received a copy of the GNU Lesser General Public License
 *       along with this program.  If not, see <https://www.gnu.org/licenses/>.
 *
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Tekla.Structures;
using Tekla.Structures.Drawing;
using Tekla.Structures.Drawing.UI;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using TSMO = Tekla.Structures.Model.Operations;
using Object = Tekla.Structures.Model.Object;
using Operation = Tekla.Structures.Analysis.Operations.Operation;
using View = Tekla.Structures.Drawing.View;

// ReSharper disable LocalizableElement

namespace Drawing.CenterView;

[SuppressMessage("ReSharper", "InconsistentNaming")]
abstract partial class QuickCenterClass
{
    //private static readonly Model Model = new Model();
    private static readonly DrawingHandler DrawingHandler = new DrawingHandler();

    public static void EntryPoint()
    {
        var drawingSelector = DrawingHandler.GetDrawingSelector();
        var selectedSize = drawingSelector.GetSelected().GetSize();
        var stopWatch = new Stopwatch();

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
                    stopWatch.Start();
                    _CenterAllDriver();
                    break;
                case DialogResult.Cancel:
                    Tekla.Structures.Model.Operations.Operation.DisplayPrompt("Aborting.");
                    return;
                case DialogResult.None:
                case DialogResult.Abort:
                case DialogResult.Retry:
                case DialogResult.Ignore:
                case DialogResult.Yes:
                case DialogResult.No:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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
                    stopWatch.Start();
                    CenterSelectedGADrawingsDriver(drawingSelector.GetSelected());
                    break;
                case DialogResult.No:
                    stopWatch.Start();
                    _CenterAllDriver();
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
        }

        //Tekla.Structures.Model.Operations.Operation.DisplayPrompt("Done.");
        stopWatch.Stop();
        Tekla.Structures.Model.Operations.Operation.DisplayPrompt(
            $@"Drawings centered. Time elapsed = {stopWatch.Elapsed.ToString(@"mm\:ss\:mss")}");
    }

    private static bool _CenterAllDriver()
    {
        Tekla.Structures.Model.Operations.Operation.DisplayPrompt("Centering Drawings...");
        var allDrawings = DrawingHandler.GetDrawings();
        var allGADrawings = new ArrayList();
        while (allDrawings.MoveNext())
            if (allDrawings.Current is GADrawing)
                allGADrawings.Add(allDrawings.Current);
        CenterAllDriver(allGADrawings);
        return allGADrawings.Count != 1 ? true : false;
    }


    private static void RenameDrawingTitle3FromTuple(Tuple<Tekla.Structures.Drawing.Drawing, string> drawingTuple)
    {
        drawingTuple.Item1.Title3 = drawingTuple.Item2.ToString();
        drawingTuple.Item1.Modify();
    }

    private static void CleanUp(Tekla.Structures.Drawing.Drawing drawing)
    {
        if (drawing.Title3.Equals("X")) return;
        drawing.Title3 = "";
        drawing.Modify();
    }

    private static void FinalizeDrawing(Tuple<Tekla.Structures.Drawing.Drawing, string> s)
    {
        DrawingHandler.GetActiveDrawing().CommitChanges("Center View");
        DrawingHandler.SaveActiveDrawing();
        DrawingHandler.CloseActiveDrawing(true);
        if (s.Item1.Title3.Equals("X")) return;
        s.Item1.Title3 = s.Item2.ToString();
        s.Item1.Modify();
    }

    /// <summary>
    /// Method <c>IsValidDrawingForCenter</c> checks if there is exactly one view that has a valid property from ViewType enum.
    /// </summary>
    /// <param name="drawing"></param>
    /// <returns></returns>
    public static bool IsValidDrawingForCenter(Tekla.Structures.Drawing.Drawing drawing)
    {
        var memberCount = 0;
        var views = drawing.GetSheet().GetViews();

        while (views.MoveNext())
        {
            views.Current.GetStringUserProperties(out Dictionary<string, string> viewTypes);
            var type = PluginForm.GetViewTypeEnum(viewTypes);
            if (type is not PluginForm.ViewType.None) memberCount++;
        }

        return (memberCount == 1); // valid if memberCount is 1
    }

    private static bool CenterSelectedGADrawingsDriver(DrawingEnumerator selectedGADrawings)
    {
        Tekla.Structures.Model.Operations.Operation.DisplayPrompt("Centering Drawings...");
        var reportStringBuilder = new StringBuilder();
        var counter = 1; // keeps track of each iteration
        var total = selectedGADrawings.GetSize();

        while (selectedGADrawings.MoveNext())
        {
            if (!IsValidDrawingForCenter(selectedGADrawings.Current))
                continue; // check if dwg is a candidate for centering

            var filteredDrawing = selectedGADrawings.Current as GADrawing ?? new GADrawing();

            DrawingHandler.SetActiveDrawing(filteredDrawing, false);
            var allViews = DrawingHandler.GetActiveDrawing().GetSheet().GetAllViews() ??
                           throw new ArgumentNullException(
                               nameof(selectedGADrawings));
            var drawingTuple =
                new Tuple<Tekla.Structures.Drawing.Drawing, string>(
                    new GADrawing(), string.Empty);

            while (allViews.MoveNext())
            {
                var currentView = (ViewBase)allViews.Current;
                currentView.GetStringUserProperties(out Dictionary<string, string> viewType);
                try
                {
                    if (!currentView.GetDrawing().Title3.Equals("X"))
                    {
                        viewType.TryGetValue("ViewType", out string vt);
                        if (vt != null)
                        {
                            var reportString = CenterView(currentView as ViewBase,
                                (int)PluginForm.GetViewTypeEnum(viewType),
                                out drawingTuple);
                            reportStringBuilder.AppendLine(reportString);
                            TSMO.Operation.DisplayPrompt($@"({counter}/{total}) " + reportString);
                            RenameDrawingTitle3FromTuple(drawingTuple);
                            counter++;
                        }
                    }
                    else
                    {
                        TSMO.Operation.DisplayPrompt(
                            $@"({counter}/{total}) Skipping {currentView.GetDrawing().Name}.");
                        counter++;
                    }
                }
                catch (Exception e) when (e is KeyNotFoundException)
                {
                    TSMO.Operation.DisplayPrompt(@"Invalid View: " +
                                                 currentView.ToString());
                }
            }

            FinalizeDrawing(drawingTuple);
        }

        GenerateAndDisplayReport("Centered_Report", reportStringBuilder.ToString());
        selectedGADrawings.Reset();
        while (selectedGADrawings.MoveNext())
        {
            CleanUp(selectedGADrawings.Current);
        }

        return true;
    }

    private static void CenterAllDriver(ArrayList drawings)
    {
        var reportStringBuilder = new StringBuilder();
        var counter = 1;
        var total = drawings.Count; //TODO: Figure out how to remove not relevant drawings from this count.

        foreach (var gaDwg in drawings)
        {
            var drawingTuple =
                new Tuple<Tekla.Structures.Drawing.Drawing, string>(
                    new GADrawing(), string.Empty);
            var dwg = (Tekla.Structures.Drawing.Drawing)gaDwg;

            if (!IsValidDrawingForCenter(gaDwg as Tekla.Structures.Drawing.Drawing ?? new GADrawing())) continue;
            var filteredDrawing = gaDwg as GADrawing;

            DrawingHandler.SetActiveDrawing(filteredDrawing, false);
            var allViews = DrawingHandler.GetActiveDrawing().GetSheet().GetAllViews();
            while (allViews.MoveNext())
            {
                var currentView = (ViewBase)allViews.Current;
                var viewTypeDict = GetViewTypeDict(currentView);
                var viewTypeEnum = PluginForm.GetViewTypeEnum(viewTypeDict);
                try
                {
                    switch (currentView.GetDrawing().Title3)
                    {
                        case "X":
                            TSMO.Operation.DisplayPrompt(
                                $@"({counter}/{total}) Skipping {currentView.GetDrawing().Name}.");
                            break;
                        default:
                            if (viewTypeEnum != PluginForm.ViewType.None)
                            {
                                var reportString = CenterView(currentView, (int)viewTypeEnum,
                                    out drawingTuple);
                                reportStringBuilder.AppendLine(reportString);
                                TSMO.Operation.DisplayPrompt($@"({counter}/{total}) " + reportString);
                                counter++;
                                RenameDrawingTitle3FromTuple(drawingTuple);
                            }

                            break;
                    }
                }
                catch (Exception e) when (e is KeyNotFoundException)
                {
                    TSMO.Operation.DisplayPrompt(@"Invalid View: " +
                                                 currentView.ToString());
                }

                FinalizeDrawing(drawingTuple);
            }
        }

        GenerateAndDisplayReport("Centered_Report", reportStringBuilder.ToString());
        foreach (GADrawing drawing in drawings)
        {
            CleanUp(drawing);
        }
    }

    private static Dictionary<string, string> GetViewTypeDict(ViewBase view)
    {
        view.GetStringUserProperties(new List<string>() { "ViewType" }, out var viewType);
        return viewType;
    }

    public static string CenterView(ViewBase view, int viewType, out Tuple<Tekla.Structures.Drawing.Drawing, string> s)
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
            default: Tekla.Structures.Model.Operations.Operation.DisplayPrompt(viewType.ToString()); break;
        }

        sheet.Origin.Y = sheetHeightOffset;
        var originalOriginX = view.Origin.X;
        var originalOriginY = view.Origin.Y;
        view.Origin = sheet.Origin;

        var viewCenterPoint = view.GetAxisAlignedBoundingBox().GetCenterPoint();

        var sheetHeight = sheet.Height / 2;
        var sheetWidth = (sheet.Width - 33.274) / 2;
        var xOffset = sheetWidth - viewCenterPoint.X;
        var yOffset = sheetHeight - viewCenterPoint.Y;

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

            return $"Centering {view.GetDrawing().Name} => {(PluginForm.ViewType)viewType}";
        }

        s = new Tuple<Tekla.Structures.Drawing.Drawing, string>(view.GetDrawing(), "X");
        return $"Something Went Wrong At {view.GetDrawing().Name} => " + (PluginForm.ViewType)viewType;
    }
}