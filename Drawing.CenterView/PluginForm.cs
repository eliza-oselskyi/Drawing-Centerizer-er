/*
 *
 Drawing Centerizer-er: Mainly centers Tekla drawings, specifically NBG's flavor.
      Copyright (C) 2025  Eliza Oselskyi

      This program is free software: you can redistribute it and/or modify
      it under the terms of the GNU Lesser General Public License as published by
      the Free Software Foundation, either version 3 of the License, or
      (at your option) any later version.

      This program is distributed in the hope that it will be useful,
      but WITHOUT ANY WARRANTY; without even the implied warranty of
      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
      GNU Lesser General Public License for more details.

      You should have received a copy of the GNU Lesser General Public License
      along with this program.  If not, see <https://www.gnu.org/licenses/>.
 *
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Svg;
using System.Windows.Forms;
using Drawing.CenterView.Properties;
using Tekla.Structures;
using Tekla.Structures.Catalogs;
using Tekla.Structures.Dialog;
using Tekla.Structures.Dialog.UIControls;
using Tekla.Structures.Drawing;
using Tekla.Structures.Drawing.UI;
using Tekla.Structures.DrawingInternal;
using Tekla.Structures.Model;
using Tekla.Structures.Model.Operations;
using Tekla.Structures.ModelInternal;
using Color = RenderData.Color;
using ModelObjectSelector = Tekla.Structures.Model.UI.ModelObjectSelector;
using Polygon = Tekla.Structures.Model.Polygon;
using Size = System.Drawing.Size;
using TSG = Tekla.Structures.Geometry3d;
using TSD = Tekla.Structures.Drawing;

// ReSharper disable LocalizableElement

namespace Drawing.CenterView
{
    public partial class PluginForm : ApplicationFormBase
    {
        private readonly Model _myModel;
        private readonly DrawingHandler _drawingHandler;

#pragma warning disable CS8618, CS9264
        public PluginForm() // Main entry point for the form.
#pragma warning restore CS8618, CS9264
        {
            InitializeComponent();
            base.InitializeForm(); // Required for TeklaAPI

            // Set up the private, read-only fields. Can do only in here (the constructor method).
            _myModel = new Model();
            _drawingHandler = new DrawingHandler();
        }

        private void ShiftViewRight(int amount)
        {
            var view = GetValidViewInActiveDrawing();
            if (view == null) return;
            view.Origin.X += amount;
            view.Modify();
            view.GetStringUserProperties(out Dictionary<string, string> viewType);
            GetViewTypeEnum(viewType);
            InfoBox.OnInfo(infoBox, $"Shifting Right => {(ViewType)GetViewTypeEnum(viewType)}");
            _drawingHandler.GetActiveDrawing().CommitChanges("Shift View Right");
        }

        private void ShiftViewUp(int amount)
        {
            var view = GetValidViewInActiveDrawing();
            if (view == null) return;
            view.Origin.Y += amount;
            view.Modify();
            view.GetStringUserProperties(out Dictionary<string, string> viewType);
            GetViewTypeEnum(viewType);
            InfoBox.OnInfo(infoBox, $"{(ViewType)GetViewTypeEnum(viewType)}\nShifting Up =^");
            _drawingHandler.GetActiveDrawing().CommitChanges("Shift View Up");
        }

        private void ShiftViewDown(int amount)
        {
            var view = GetValidViewInActiveDrawing();
            if (view == null) return;
            view.Origin.Y -= amount;
            view.Modify();
            view.GetStringUserProperties(out Dictionary<string, string> viewType);
            GetViewTypeEnum(viewType);
            InfoBox.OnInfo(infoBox, $"Shifting Down=v\n{(ViewType)GetViewTypeEnum(viewType)}");
            _drawingHandler.GetActiveDrawing().CommitChanges("Shift View Down");
        }

        private void ShiftViewLeft(int amount)
        {
            var view = GetValidViewInActiveDrawing();
            if (view == null) return;
            view.Origin.X -= amount;
            view.Modify();
            view.GetStringUserProperties(out Dictionary<string, string> viewType);
            GetViewTypeEnum(viewType);
            InfoBox.OnInfo(infoBox, $"{(ViewType)GetViewTypeEnum(viewType)} <= Shifting Left");
            _drawingHandler.GetActiveDrawing().CommitChanges("Shift View Left");
        }

        private ViewBase? GetValidViewInActiveDrawing()
        {
            var allViews = _drawingHandler.GetActiveDrawing().GetSheet().GetAllViews();
            var memberCount = 0;
            while (allViews.MoveNext())
            {
                allViews.Current.GetStringUserProperties(out Dictionary<string, string> viewTypes);
                var type = PluginForm.GetViewTypeEnum(viewTypes);
                if (type is not PluginForm.ViewType.None) memberCount++;
                //Console.WriteLine(PluginForm.GetViewTypeEnum(viewTypes).ToString());
            }

            if (memberCount != 1) return null;
            allViews.Reset();
            while (allViews.MoveNext())
            {
                allViews.Current.GetStringUserProperties(out Dictionary<string, string> viewType);
                var currentView = (ViewBase)allViews.Current;
                try
                {
                    return currentView;
                }
                catch (Exception e) when (e is KeyNotFoundException)
                {
                    Console.WriteLine(@"Invalid View: " + currentView.ToString());
                }
            }

            return null;
        }

        private void CenterViewsInDrawing()
        {
            var allViews = _drawingHandler.GetActiveDrawing().GetSheet().GetAllViews();

            while (allViews.MoveNext())
            {
                allViews.Current.GetStringUserProperties(out Dictionary<string, string> viewType);
                var currentView = (ViewBase)allViews.Current;
                try
                {
                    CenterView(currentView, (int)GetViewTypeEnum(viewType));
                }
                catch (Exception e) when (e is KeyNotFoundException)
                {
                    InfoBox.OnInfo(infoBox, @"Invalid View: " + currentView.ToString());
                }
            }
        }

        private void CenterView(ViewBase view, int viewType)
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
            var originalOriginX = view.Origin.X;
            var originalOriginY = view.Origin.Y;
            view.Origin = sheet.Origin;
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
                InfoBox.OnInfo(infoBox, @"Nothing To Do.");
            }
            else if (Math.Abs(view.ExtremaCenter.X - sheetWidth) > 0.0001 ||
                     Math.Abs(view.ExtremaCenter.Y - sheetHeight) > 0.0001)
            {
                InfoBox.OnInfo(infoBox, $"Centering {(ViewType)viewType}");
                view.Origin.X += xOffset;
                view.Origin.Y += yOffset;

                view.Modify();
                _drawingHandler.GetActiveDrawing().CommitChanges("Center View");
                Console.WriteLine(view.Origin.ToString());
            }
        }

        public enum ViewType
        {
            None,
            CoverSheet,
            BuildingSheet1,
            BuildingSheet2,
            PlaneIdentificationPlan,
            AnchorRodPlan,
            BasePlateDetails,
            Reactions,
            ShakeoutPlan,
            RoofFramedOpeningPlan,
            CraneBeamPlan,
            CrossSection,
            PortalCrossSection,
            RoofFramingPlan,
            RoofFramingPlanSecondary,
            RoofFramingPlanOpenings,
            RoofFramingPlanPurlinBracing,
            EndwallFraming,
            EndwallPartitionFraming,
            SidewallFraming,
            SidewallPartitionFraming,
            WallSheeting,
            PartitionWallSheeting,
            RoofSheeting,
            WallLiner,
            PartitionWallLiner,
            RoofLiner,
            RoofPanelClipLayout,
            MezzaninePlanFramingOnly,
            MezzaninePlanJoistsOnly,
            MezzaninePlanDecking
        }

        public static ViewType GetViewTypeEnum(Dictionary<string, string> viewType)
        {
            return viewType["ViewType"] switch
            {
                "Cover Sheet" => ViewType.CoverSheet,
                "Building Sheet 1" => ViewType.BuildingSheet1,
                "Building Sheet 2" => ViewType.BuildingSheet2,
                "Plane Identification Plan" => ViewType.PlaneIdentificationPlan,
                "Anchor Rod Plan" => ViewType.AnchorRodPlan,
                "Base Plate Details" => ViewType.BasePlateDetails,
                "Reactions" => ViewType.Reactions,
                "Shakeout Plan" => ViewType.ShakeoutPlan,
                "Roof Framed Opening Plan" => ViewType.RoofFramedOpeningPlan,
                "Crane Beam Plan" => ViewType.CraneBeamPlan,
                "Cross Section" => ViewType.CrossSection,
                "Portal Cross Section" => ViewType.PortalCrossSection,
                "Roof Framing Plan" => ViewType.RoofFramingPlan,
                "Roof Framing Plan - Secondary" => ViewType.RoofFramingPlanSecondary,
                "Roof Framing Plan - Openings" => ViewType.RoofFramingPlanOpenings,
                "Roof Framing Plan - Purlin Bracing" => ViewType.RoofFramingPlanPurlinBracing,
                "Endwall Framing" => ViewType.EndwallFraming,
                "Endwall Partition Framing" => ViewType.EndwallPartitionFraming,
                "Sidewall Framing" => ViewType.SidewallFraming,
                "Sidewall Partition Framing" => ViewType.SidewallPartitionFraming,
                "Wall Sheeting" => ViewType.WallSheeting,
                "Partition Wall Sheeting" => ViewType.PartitionWallSheeting,
                "Roof Sheeting" => ViewType.RoofSheeting,
                "Wall Liner" => ViewType.WallLiner,
                "Partition Wall Liner" => ViewType.PartitionWallLiner,
                "Roof Liner" => ViewType.RoofLiner,
                "Roof Panel Clip Layout" => ViewType.RoofPanelClipLayout,
                "Mezzanine Plan - Framing Only" => ViewType.MezzaninePlanFramingOnly,
                "Mezzanine Plan - Joists Only" => ViewType.MezzaninePlanJoistsOnly,
                "Mezzanine Plan - Decking" => ViewType.MezzaninePlanDecking,
                _ => ViewType.None
            };
        }

        // NOTE: Overloads default Windows Forms OnLoad() method.
        // I'm setting up the environment for the application, here.
        // NOTE: for sake debugging purposes, the property PluginForm.ShowInTaskbar is set to TRUE.
        // Tekla style guidelines states to have this set to FALSE in deployment.
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                base.OnLoad(e);
                UI();
                InfoBox.ToDefault(infoBox);
                // Set some default values
                infoBox.AutoSize = true;
                infoBox.MinimumSize = new Size(249, 0);
                infoBox.MaximumSize = new Size(249, 0);

                InfoBox.OnInfo(infoBox,
                    _myModel.GetConnectionStatus() ? "Connection succeeded" : "Connection failed");

                if (!_myModel.GetConnectionStatus())
                    // Disable buttons if no connection found
                    selectedObjectsButton.Enabled = false;

                if (onTopCheckBox.Checked) this.TopMost = true;
            }
            catch (Exception)
            {
                // InfoBox.OnError(infoBox, e);
                InfoBox.OnError(infoBox, new Exception("Connection failed. Do you have a model open?" +
                                                       "\n Currently, there is no way to reestablish connection. " +
                                                       "You must restart this application to do so."));
            }

            _events.DrawingEditorClosed += ExitApplication;
            _events.Register();
        }
    }
}