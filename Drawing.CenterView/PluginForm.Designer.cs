using Tekla.Structures.Drawing;
using Tekla.Structures;
using System.Windows.Forms;
using System.Drawing;

namespace Drawing.CenterView
{
    partial class PluginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PluginForm));
            this.watermark = new System.Windows.Forms.Label();
            this.onTopCheckBox = new System.Windows.Forms.CheckBox();
            this.header = new System.Windows.Forms.Label();
            this.infoBox = new System.Windows.Forms.Label();
            this.selectedObjectsButton = new System.Windows.Forms.Button();
            this.infoPanel = new System.Windows.Forms.Panel();
            this.centerViewButton = new System.Windows.Forms.Button();
            this.scrambleViewsButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.paperImage = new System.Windows.Forms.PictureBox();
            this.refreshButton = new System.Windows.Forms.Button();
            this.rightChevronImage = new System.Windows.Forms.PictureBox();
            this.bottomChevronImage = new System.Windows.Forms.PictureBox();
            this.topChevronImage = new System.Windows.Forms.PictureBox();
            this.leftChevronImage = new System.Windows.Forms.PictureBox();
            this.topArrowImage = new System.Windows.Forms.PictureBox();
            this.rightArrowImage = new System.Windows.Forms.PictureBox();
            this.bottomArrowImage = new System.Windows.Forms.PictureBox();
            this.leftArrowImage = new System.Windows.Forms.PictureBox();
            this.centerImage = new System.Windows.Forms.PictureBox();
            this.infoPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.paperImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rightChevronImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bottomChevronImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.topChevronImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.leftChevronImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.topArrowImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rightArrowImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bottomArrowImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.leftArrowImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.centerImage)).BeginInit();
            this.SuspendLayout();
            // 
            // watermark
            // 
            this.structuresExtender.SetAttributeName(this.watermark, null);
            this.structuresExtender.SetAttributeTypeName(this.watermark, null);
            this.watermark.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.structuresExtender.SetBindPropertyName(this.watermark, null);
            this.watermark.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.watermark.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.watermark.Location = new System.Drawing.Point(646, 388);
            this.watermark.Name = "watermark";
            this.watermark.Size = new System.Drawing.Size(142, 53);
            this.watermark.TabIndex = 15;
            this.watermark.Text = "Written By: Eliza Oselskyi";
            // 
            // onTopCheckBox
            // 
            this.structuresExtender.SetAttributeName(this.onTopCheckBox, null);
            this.structuresExtender.SetAttributeTypeName(this.onTopCheckBox, null);
            this.onTopCheckBox.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.structuresExtender.SetBindPropertyName(this.onTopCheckBox, null);
            this.onTopCheckBox.Checked = true;
            this.onTopCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.onTopCheckBox.FlatAppearance.BorderSize = 3;
            this.onTopCheckBox.ForeColor = System.Drawing.Color.Black;
            this.onTopCheckBox.Location = new System.Drawing.Point(684, 11);
            this.onTopCheckBox.Name = "onTopCheckBox";
            this.onTopCheckBox.Padding = new System.Windows.Forms.Padding(3);
            this.onTopCheckBox.Size = new System.Drawing.Size(104, 24);
            this.onTopCheckBox.TabIndex = 21;
            this.onTopCheckBox.Text = "Always On Top";
            this.onTopCheckBox.UseVisualStyleBackColor = false;
            this.onTopCheckBox.CheckedChanged += new System.EventHandler(this.onTopCheckBox_CheckedChanged);
            // 
            // header
            // 
            this.structuresExtender.SetAttributeName(this.header, null);
            this.structuresExtender.SetAttributeTypeName(this.header, null);
            this.structuresExtender.SetBindPropertyName(this.header, null);
            this.header.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.header.Location = new System.Drawing.Point(307, 12);
            this.header.Name = "header";
            this.header.Size = new System.Drawing.Size(182, 23);
            this.header.TabIndex = 25;
            this.header.Text = "Drawing Goodizer";
            // 
            // infoBox
            // 
            this.structuresExtender.SetAttributeName(this.infoBox, null);
            this.structuresExtender.SetAttributeTypeName(this.infoBox, null);
            this.infoBox.AutoSize = true;
            this.infoBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(57)))), ((int)(((byte)(56)))));
            this.structuresExtender.SetBindPropertyName(this.infoBox, null);
            this.infoBox.Font = new System.Drawing.Font("Monospac821 BT", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.infoBox.Location = new System.Drawing.Point(0, 0);
            this.infoBox.Name = "infoBox";
            this.infoBox.Size = new System.Drawing.Size(0, 15);
            this.infoBox.TabIndex = 26;
            // 
            // selectedObjectsButton
            // 
            this.selectedObjectsButton.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.structuresExtender.SetAttributeName(this.selectedObjectsButton, null);
            this.structuresExtender.SetAttributeTypeName(this.selectedObjectsButton, null);
            this.structuresExtender.SetBindPropertyName(this.selectedObjectsButton, null);
            this.selectedObjectsButton.ForeColor = System.Drawing.Color.Black;
            this.selectedObjectsButton.Location = new System.Drawing.Point(636, 259);
            this.selectedObjectsButton.Name = "selectedObjectsButton";
            this.selectedObjectsButton.Size = new System.Drawing.Size(118, 23);
            this.selectedObjectsButton.TabIndex = 29;
            this.selectedObjectsButton.Text = "Get Selected Objects";
            this.selectedObjectsButton.UseVisualStyleBackColor = true;
            this.selectedObjectsButton.Click += new System.EventHandler(this.selectedObjectsButton_Click);
            // 
            // infoPanel
            // 
            this.structuresExtender.SetAttributeName(this.infoPanel, null);
            this.structuresExtender.SetAttributeTypeName(this.infoPanel, null);
            this.infoPanel.AutoScroll = true;
            this.infoPanel.BackColor = System.Drawing.Color.Transparent;
            this.structuresExtender.SetBindPropertyName(this.infoPanel, null);
            this.infoPanel.Controls.Add(this.infoBox);
            this.infoPanel.Location = new System.Drawing.Point(12, 388);
            this.infoPanel.MaximumSize = new System.Drawing.Size(585, 192);
            this.infoPanel.MinimumSize = new System.Drawing.Size(585, 0);
            this.infoPanel.Name = "infoPanel";
            this.infoPanel.Size = new System.Drawing.Size(585, 53);
            this.infoPanel.TabIndex = 30;
            // 
            // centerViewButton
            // 
            this.structuresExtender.SetAttributeName(this.centerViewButton, null);
            this.structuresExtender.SetAttributeTypeName(this.centerViewButton, null);
            this.structuresExtender.SetBindPropertyName(this.centerViewButton, null);
            this.centerViewButton.ForeColor = System.Drawing.Color.Black;
            this.centerViewButton.Location = new System.Drawing.Point(636, 230);
            this.centerViewButton.Name = "centerViewButton";
            this.centerViewButton.Size = new System.Drawing.Size(118, 23);
            this.centerViewButton.TabIndex = 31;
            this.centerViewButton.Text = "Center View";
            this.centerViewButton.UseVisualStyleBackColor = true;
            this.centerViewButton.Click += new System.EventHandler(this.centerViewButton_Click);
            // 
            // scrambleViewsButton
            // 
            this.structuresExtender.SetAttributeName(this.scrambleViewsButton, null);
            this.structuresExtender.SetAttributeTypeName(this.scrambleViewsButton, null);
            this.structuresExtender.SetBindPropertyName(this.scrambleViewsButton, null);
            this.scrambleViewsButton.ForeColor = System.Drawing.Color.Black;
            this.scrambleViewsButton.Location = new System.Drawing.Point(636, 288);
            this.scrambleViewsButton.Name = "scrambleViewsButton";
            this.scrambleViewsButton.Size = new System.Drawing.Size(118, 23);
            this.scrambleViewsButton.TabIndex = 32;
            this.scrambleViewsButton.Text = "Scramble Views";
            this.scrambleViewsButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.structuresExtender.SetAttributeName(this.label1, null);
            this.structuresExtender.SetAttributeTypeName(this.label1, null);
            this.structuresExtender.SetBindPropertyName(this.label1, null);
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(339, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 23);
            this.label1.TabIndex = 33;
            this.label1.Text = "Test Build";
            // 
            // paperImage
            // 
            this.paperImage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.structuresExtender.SetAttributeName(this.paperImage, null);
            this.structuresExtender.SetAttributeTypeName(this.paperImage, null);
            this.paperImage.BackColor = System.Drawing.Color.Transparent;
            this.structuresExtender.SetBindPropertyName(this.paperImage, null);
            this.paperImage.Location = new System.Drawing.Point(339, 158);
            this.paperImage.Name = "paperImage";
            this.paperImage.Size = new System.Drawing.Size(125, 95);
            this.paperImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.paperImage.TabIndex = 34;
            this.paperImage.TabStop = false;
            // 
            // refreshButton
            // 
            this.structuresExtender.SetAttributeName(this.refreshButton, null);
            this.structuresExtender.SetAttributeTypeName(this.refreshButton, null);
            this.structuresExtender.SetBindPropertyName(this.refreshButton, null);
            this.refreshButton.ForeColor = System.Drawing.Color.Black;
            this.refreshButton.Location = new System.Drawing.Point(48, 50);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(62, 27);
            this.refreshButton.TabIndex = 35;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // rightChevronImage
            // 
            this.structuresExtender.SetAttributeName(this.rightChevronImage, null);
            this.structuresExtender.SetAttributeTypeName(this.rightChevronImage, null);
            this.structuresExtender.SetBindPropertyName(this.rightChevronImage, null);
            this.rightChevronImage.Location = new System.Drawing.Point(470, 196);
            this.rightChevronImage.Name = "rightChevronImage";
            this.rightChevronImage.Size = new System.Drawing.Size(25, 25);
            this.rightChevronImage.TabIndex = 36;
            this.rightChevronImage.TabStop = false;
            this.rightChevronImage.MouseLeave += new System.EventHandler(this.iconImage_MouseLeave);
            this.rightChevronImage.MouseHover += new System.EventHandler(this.iconImage_MouseHover);
            // 
            // bottomChevronImage
            // 
            this.structuresExtender.SetAttributeName(this.bottomChevronImage, null);
            this.structuresExtender.SetAttributeTypeName(this.bottomChevronImage, null);
            this.structuresExtender.SetBindPropertyName(this.bottomChevronImage, null);
            this.bottomChevronImage.Location = new System.Drawing.Point(390, 257);
            this.bottomChevronImage.Name = "bottomChevronImage";
            this.bottomChevronImage.Size = new System.Drawing.Size(25, 25);
            this.bottomChevronImage.TabIndex = 37;
            this.bottomChevronImage.TabStop = false;
            this.bottomChevronImage.MouseLeave += new System.EventHandler(this.iconImage_MouseLeave);
            this.bottomChevronImage.MouseHover += new System.EventHandler(this.iconImage_MouseHover);
            // 
            // topChevronImage
            // 
            this.structuresExtender.SetAttributeName(this.topChevronImage, null);
            this.structuresExtender.SetAttributeTypeName(this.topChevronImage, null);
            this.structuresExtender.SetBindPropertyName(this.topChevronImage, null);
            this.topChevronImage.Location = new System.Drawing.Point(390, 127);
            this.topChevronImage.Name = "topChevronImage";
            this.topChevronImage.Size = new System.Drawing.Size(25, 25);
            this.topChevronImage.TabIndex = 38;
            this.topChevronImage.TabStop = false;
            this.topChevronImage.MouseLeave += new System.EventHandler(this.iconImage_MouseLeave);
            this.topChevronImage.MouseHover += new System.EventHandler(this.iconImage_MouseHover);
            // 
            // leftChevronImage
            // 
            this.structuresExtender.SetAttributeName(this.leftChevronImage, null);
            this.structuresExtender.SetAttributeTypeName(this.leftChevronImage, null);
            this.structuresExtender.SetBindPropertyName(this.leftChevronImage, null);
            this.leftChevronImage.Location = new System.Drawing.Point(308, 196);
            this.leftChevronImage.Name = "leftChevronImage";
            this.leftChevronImage.Size = new System.Drawing.Size(25, 25);
            this.leftChevronImage.TabIndex = 39;
            this.leftChevronImage.TabStop = false;
            this.leftChevronImage.MouseLeave += new System.EventHandler(this.iconImage_MouseLeave);
            this.leftChevronImage.MouseHover += new System.EventHandler(this.iconImage_MouseHover);
            // 
            // topArrowImage
            // 
            this.structuresExtender.SetAttributeName(this.topArrowImage, null);
            this.structuresExtender.SetAttributeTypeName(this.topArrowImage, null);
            this.structuresExtender.SetBindPropertyName(this.topArrowImage, null);
            this.topArrowImage.Location = new System.Drawing.Point(390, 96);
            this.topArrowImage.Name = "topArrowImage";
            this.topArrowImage.Size = new System.Drawing.Size(25, 25);
            this.topArrowImage.TabIndex = 40;
            this.topArrowImage.TabStop = false;
            this.topArrowImage.MouseLeave += new System.EventHandler(this.iconImage_MouseLeave);
            this.topArrowImage.MouseHover += new System.EventHandler(this.iconImage_MouseHover);
            // 
            // rightArrowImage
            // 
            this.structuresExtender.SetAttributeName(this.rightArrowImage, null);
            this.structuresExtender.SetAttributeTypeName(this.rightArrowImage, null);
            this.structuresExtender.SetBindPropertyName(this.rightArrowImage, null);
            this.rightArrowImage.Location = new System.Drawing.Point(501, 196);
            this.rightArrowImage.Name = "rightArrowImage";
            this.rightArrowImage.Size = new System.Drawing.Size(25, 25);
            this.rightArrowImage.TabIndex = 41;
            this.rightArrowImage.TabStop = false;
            this.rightArrowImage.MouseLeave += new System.EventHandler(this.iconImage_MouseLeave);
            this.rightArrowImage.MouseHover += new System.EventHandler(this.iconImage_MouseHover);
            // 
            // bottomArrowImage
            // 
            this.structuresExtender.SetAttributeName(this.bottomArrowImage, null);
            this.structuresExtender.SetAttributeTypeName(this.bottomArrowImage, null);
            this.structuresExtender.SetBindPropertyName(this.bottomArrowImage, null);
            this.bottomArrowImage.Location = new System.Drawing.Point(390, 288);
            this.bottomArrowImage.Name = "bottomArrowImage";
            this.bottomArrowImage.Size = new System.Drawing.Size(25, 25);
            this.bottomArrowImage.TabIndex = 42;
            this.bottomArrowImage.TabStop = false;
            this.bottomArrowImage.MouseLeave += new System.EventHandler(this.iconImage_MouseLeave);
            this.bottomArrowImage.MouseHover += new System.EventHandler(this.iconImage_MouseHover);
            // 
            // leftArrowImage
            // 
            this.structuresExtender.SetAttributeName(this.leftArrowImage, null);
            this.structuresExtender.SetAttributeTypeName(this.leftArrowImage, null);
            this.structuresExtender.SetBindPropertyName(this.leftArrowImage, null);
            this.leftArrowImage.Location = new System.Drawing.Point(277, 196);
            this.leftArrowImage.Name = "leftArrowImage";
            this.leftArrowImage.Size = new System.Drawing.Size(25, 25);
            this.leftArrowImage.TabIndex = 43;
            this.leftArrowImage.TabStop = false;
            this.leftArrowImage.MouseLeave += new System.EventHandler(this.iconImage_MouseLeave);
            this.leftArrowImage.MouseHover += new System.EventHandler(this.iconImage_MouseHover);
            // 
            // centerImage
            // 
            this.structuresExtender.SetAttributeName(this.centerImage, null);
            this.structuresExtender.SetAttributeTypeName(this.centerImage, null);
            this.centerImage.BackColor = System.Drawing.Color.Transparent;
            this.structuresExtender.SetBindPropertyName(this.centerImage, null);
            this.centerImage.Location = new System.Drawing.Point(390, 196);
            this.centerImage.Name = "centerImage";
            this.centerImage.Size = new System.Drawing.Size(25, 25);
            this.centerImage.TabIndex = 44;
            this.centerImage.TabStop = false;
            this.centerImage.Click += new System.EventHandler(this.centerImage_Click);
            this.centerImage.MouseLeave += new System.EventHandler(this.iconImage_MouseLeave);
            this.centerImage.MouseHover += new System.EventHandler(this.iconImage_MouseHover);
            // 
            // PluginForm
            // 
            this.structuresExtender.SetAttributeName(this, null);
            this.structuresExtender.SetAttributeTypeName(this, null);
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(57)))), ((int)(((byte)(56)))));
            this.structuresExtender.SetBindPropertyName(this, null);
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.centerImage);
            this.Controls.Add(this.watermark);
            this.Controls.Add(this.leftArrowImage);
            this.Controls.Add(this.bottomArrowImage);
            this.Controls.Add(this.rightArrowImage);
            this.Controls.Add(this.topArrowImage);
            this.Controls.Add(this.leftChevronImage);
            this.Controls.Add(this.topChevronImage);
            this.Controls.Add(this.bottomChevronImage);
            this.Controls.Add(this.rightChevronImage);
            this.Controls.Add(this.refreshButton);
            this.Controls.Add(this.paperImage);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.scrambleViewsButton);
            this.Controls.Add(this.centerViewButton);
            this.Controls.Add(this.infoPanel);
            this.Controls.Add(this.selectedObjectsButton);
            this.Controls.Add(this.header);
            this.Controls.Add(this.onTopCheckBox);
            this.ForeColor = System.Drawing.Color.Transparent;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(15, 15);
            this.MaximumSize = new System.Drawing.Size(816, 489);
            this.MinimumSize = new System.Drawing.Size(816, 489);
            this.Name = "PluginForm";
            this.ShowInTaskbar = true;
            this.Text = "Drawing Goodizer";
            this.infoPanel.ResumeLayout(false);
            this.infoPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.paperImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rightChevronImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bottomChevronImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.topChevronImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.leftChevronImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.topArrowImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rightArrowImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bottomArrowImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.leftArrowImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.centerImage)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.PictureBox centerImage;

        private System.Windows.Forms.PictureBox bottomArrowImage;
        private System.Windows.Forms.PictureBox rightChevronImage;
        private System.Windows.Forms.PictureBox bottomChevronImage;
        private System.Windows.Forms.PictureBox topChevronImage;
        private System.Windows.Forms.PictureBox leftChevronImage;
        private System.Windows.Forms.PictureBox topArrowImage;
        private System.Windows.Forms.PictureBox rightArrowImage;
        private System.Windows.Forms.PictureBox leftArrowImage;

        private System.Windows.Forms.Button refreshButton;

        private System.Windows.Forms.PictureBox paperImage;

        private System.Windows.Forms.Label label1;

        private System.Windows.Forms.Button scrambleViewsButton;

        private System.Windows.Forms.Button centerViewButton;

        private System.Windows.Forms.Panel infoPanel;

        private System.Windows.Forms.Button selectedObjectsButton;

        private System.Windows.Forms.Label infoBox;

        private System.Windows.Forms.Label header;

        private System.Windows.Forms.CheckBox onTopCheckBox;

        private System.Windows.Forms.Label watermark;

        #endregion
    }
}