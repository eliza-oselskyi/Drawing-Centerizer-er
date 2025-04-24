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

        public PluginForm() // Main entry point for the form.
        {
            InitializeComponent();
            //base.InitializeForm(); // Required for TeklaAPI

            // Set up the private, read-only fields. Can do only in here (the constructor method).
            _myModel = new Model();
            _drawingHandler = new DrawingHandler();
        }


        private void GetViewsToCenter()
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
            Console.WriteLine(
                $"Sheet Height: {sheetHeight.ToString(CultureInfo.InvariantCulture)}\nSheet Width: {sheetWidth.ToString(CultureInfo.InvariantCulture)}");
            Console.WriteLine($"View Origin: {view.Origin.ToString()}");
            Console.WriteLine($"x offset: {xOffset}, y offset: {yOffset}");

            switch (Math.Abs(view.ExtremaCenter.X - sheetWidth))
            {
                case > 0.0001 when Math.Abs(view.ExtremaCenter.Y - sheetHeight) > 0.0001:
                    InfoBox.OnInfo(infoBox, $"Centering {(ViewType)viewType}");
                    view.Origin.X += xOffset;
                    view.Origin.Y += yOffset;

                    view.Modify();
                    _drawingHandler.GetActiveDrawing().CommitChanges("Center View");
                    Console.WriteLine(view.Origin.ToString());
                    break;
                default:
                    InfoBox.OnInfo(infoBox, @"Nothing To Do.");
                    break;
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
                infoBox.MinimumSize = new Size(585, 0);
                infoBox.MaximumSize = new Size(585, 0);

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
        }


        // Create a timer, instead of using sleep, to not lock up the UI. (Sleep pauses the entire thread for a given amount of milliseconds)
        private static void Wait(int milliseconds)
        {
            if (milliseconds == 0 | milliseconds < 0) return;

            var timer = new Timer();
            timer.Interval = milliseconds;
            timer.Enabled = true;
            timer.Start();
            timer.Tick += (_, _) => // this is the event. All it does is stop the timer after 1 second.
            {
                timer.Enabled = false; // Stops the while loop
                timer.Stop();
            };
            while (timer.Enabled) Application.DoEvents(); // Goes to the event
        }
    }
}