using System;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Tekla.Structures.Model.UI;
using Color = System.Drawing.Color;

namespace Drawing.CenterView;

public partial class PluginForm
{
        /// <summary>
        ///     Method <c>onTopCheckBox_CheckedChanged</c> toggles "always stay on top"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onTopCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = onTopCheckBox.Checked;
        }
        /// <summary>
        ///     Method <c>selectedObjectsButton_Click</c> retrieves all selected objects in model and prints them in the infobox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectedObjectsButton_Click(object sender, EventArgs e)
        {
            InfoBox.ToDefault(infoBox); // restore to defaults

            // Create a selector and enumerator of selected objects. StringBuilder for the final output of the 
            // aggregated objects.
            var modelObjectSelector = new ModelObjectSelector();
            var selectedObjects = modelObjectSelector.GetSelectedObjects();
            var selectedObjectString = new StringBuilder();

            if (selectedObjects.GetSize() > 1000)
            {
                InfoBox.OnError(infoBox, new Exception("Selected object count is too large"));
            }
            else if (selectedObjects.GetSize() > 0)
            {
                while (selectedObjects.MoveNext())
                    selectedObjectString.AppendLine(selectedObjects.Current + @"=> ID: " +
                                                    selectedObjects.Current.Identifier + "\n Up to date? " +
                                                    selectedObjects.Current.IsUpToDate);

                InfoBox.OnInfo(infoBox, selectedObjectString.ToString());
            }
            else
            {
                // If no objects selected, replace text of the button with "no objects selected" for 1 second.
                selectedObjectsButton.Text = @"No Objects Selected";
                Wait(1000);
                selectedObjectsButton.Text = @"Get Selected Objects";
            }
        }
        private void centerViewButton_Click(object sender, EventArgs e)
        {
            CenterViewsInDrawing();
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            UI();
        }

        private void iconImage_MouseHover(object sender, EventArgs e)
        {
            ChangePictureBoxIconColor(sender, Color.White, Color.Aquamarine );
        }
        private void iconImage_MouseLeave(object sender, EventArgs e)
        {
            ChangePictureBoxIconColor(sender, Color.Aquamarine, Color.White);
        }
        private void ChangePictureBoxIconColor(object sender, Color sourceColor,  Color targetColor)
        {
            if (sender is not PictureBox pictureBox) return;
            _svgIcons.TryGetValue(pictureBox.Name.ToString(), out var icon);
            if (icon == null) return;
            icon.Resize(pictureBox.Width, pictureBox.Height);
            icon.ChangeIconColors(sourceColor, targetColor);
            pictureBox.Image = icon.Bmp;
        }
        private void centerImage_Click(object sender, EventArgs e)
        {
            CenterViewsInDrawing();
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