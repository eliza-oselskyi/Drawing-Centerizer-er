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
                    CenterSelectedDriver(drawingSelector.GetSelected());
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

        Tekla.Structures.Model.Operations.Operation.DisplayPrompt("Done.");
        stopWatch.Stop();
        Tekla.Structures.Model.Operations.Operation.DisplayPrompt(
            $@"Drawings centered. Time elapsed = {stopWatch.Elapsed.ToString(@"mm\:ss\:mss")}");
    }

    private static void _CenterAllDriver()
    {
        Tekla.Structures.Model.Operations.Operation.DisplayPrompt("Centering Drawings...");
        var allDrawings = DrawingHandler.GetDrawings();
        var allGADrawings = new ArrayList();
        while (allDrawings.MoveNext())
            if (allDrawings.Current is GADrawing)
                allGADrawings.Add(allDrawings.Current);
        CenterAllDriver(allGADrawings);
    }

    private static void CenterSelectedDriver(DrawingEnumerator selectedGADrawings)
    {
        Tekla.Structures.Model.Operations.Operation.DisplayPrompt("Centering Drawings...");
        var reportStringBuilder = new StringBuilder();
        var counter = 1;
        var total = selectedGADrawings.GetSize();
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
            }

            if (memberCount != 1) continue;
            filteredDrawings.Add(selectedGADrawings.Current);
            var filteredDrawingsArray = filteredDrawings.ToArray(typeof(GADrawing));

            foreach (GADrawing drawing in filteredDrawingsArray)
            {
                DrawingHandler.SetActiveDrawing(drawing, false);
                var allViews = DrawingHandler.GetActiveDrawing().GetSheet().GetAllViews() ?? throw new ArgumentNullException("DrawingHandler.GetActiveDrawing().GetSheet().GetAllViews()");
                Tuple<Tekla.Structures.Drawing.Drawing, string> s = new Tuple<Tekla.Structures.Drawing.Drawing, string>(
                    new GADrawing(), string.Empty);
                while (allViews.MoveNext())
                {
                    allViews.Current.GetStringUserProperties(out Dictionary<string, string> viewType);
                    var currentView = (ViewBase)allViews.Current;
                    try
                    {
                        if (!currentView.GetDrawing().Title3.Equals("X"))
                        {
                            viewType.TryGetValue("ViewType", out string vt);
                            if (vt != null)
                            {
                                var reportString = CenterView(currentView as ViewBase, (int)PluginForm.GetViewTypeEnum(viewType),
                                    out s);
                                reportStringBuilder.AppendLine(reportString);
                                TSMO.Operation.DisplayPrompt($@"({counter}/{total}) " + reportString);
                                counter++;
                                s.Item1.Title3 = s.Item2.ToString();
                                s.Item1.Modify();
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

                DrawingHandler.CloseActiveDrawing(true);
                if (s.Item1.Title3.Equals("X")) continue;
                s.Item1.Title3 = s.Item2.ToString();
                s.Item1.Modify();
            }
        }

        GenerateAndDisplayReport("Centered_Report", reportStringBuilder.ToString());
        selectedGADrawings.Reset();
        while (selectedGADrawings.MoveNext())
        {
            if (selectedGADrawings.Current.Title3.Equals("X")) continue;
            selectedGADrawings.Current.Title3 = "";
            selectedGADrawings.Current.Modify();
        }
    }

    private static void CenterAllDriver(ArrayList drawings)
    {
        var reportStringBuilder = new StringBuilder();
        var counter = 1;
        var total = drawings.Count; //TODO: Figure out how to remove not relevant drawings from this count.

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
            }

            if (memberCount != 1) continue;
            filteredDrawings.Add(gaDwg);
            var filteredDrawingsArray = filteredDrawings.ToArray(typeof(GADrawing));

            foreach (GADrawing drawing in filteredDrawingsArray)
            {
                DrawingHandler.SetActiveDrawing(drawing, false);
                var allViews = DrawingHandler.GetActiveDrawing().GetSheet().GetAllViews();
                while (allViews.MoveNext())
                {
                    allViews.Current.GetStringUserProperties(out Dictionary<string, string> viewType);
                    var currentView = (ViewBase)allViews.Current;
                    try
                    {
                        if (!currentView.GetDrawing().Title3.Equals("X"))
                        {
                            viewType.TryGetValue("ViewType", out string vt);
                            if (vt != null){
                                var reportString = CenterView(currentView, (int)PluginForm.GetViewTypeEnum(viewType),
                                    out var s);
                                reportStringBuilder.AppendLine(reportString);
                                TSMO.Operation.DisplayPrompt($@"({counter}/{total}) " + reportString);
                                counter++;
                                s.Item1.Title3 = s.Item2.ToString();
                                s.Item1.Modify();
                            }
                        }
                        else
                        {
                            Tekla.Structures.Model.Operations.Operation.DisplayPrompt(
                                $@"({counter}/{total}) Skipping {currentView.GetDrawing().Name}.");
                        }
                    }
                    catch (Exception e) when (e is KeyNotFoundException)
                    {
                        Tekla.Structures.Model.Operations.Operation.DisplayPrompt(@"Invalid View: " +
                            currentView.ToString());
                    }
                }
            }
        }

        DrawingHandler.CloseActiveDrawing(true);
        GenerateAndDisplayReport("Centered_Report", reportStringBuilder.ToString());
        foreach (GADrawing drawing in drawings)
        {
            if (drawing.Title3.Equals("X")) continue;
            drawing.Title3 = "";
            drawing.Modify();
        }
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
            DrawingHandler.GetActiveDrawing().CommitChanges("Center View");
            DrawingHandler.SaveActiveDrawing();

            return $"Centering {view.GetDrawing().Name} => {(PluginForm.ViewType)viewType}";
        }

        s = new Tuple<Tekla.Structures.Drawing.Drawing, string>(view.GetDrawing(), "X");
        return $"Something Went Wrong At {view.GetDrawing().Name} => " + (PluginForm.ViewType)viewType;
    }
}