using System.ComponentModel;

namespace Drawing.CenterView;

partial class CustomMessageBox
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

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
        this.messageLabel = new System.Windows.Forms.Label();
        this.button1 = new System.Windows.Forms.Button();
        this.button2 = new System.Windows.Forms.Button();
        this.button3 = new System.Windows.Forms.Button();
        this.button4 = new System.Windows.Forms.Button();
        this.SuspendLayout();
        // 
        // messageLabel
        // 
        this.messageLabel.Location = new System.Drawing.Point(9, 5);
        this.messageLabel.MaximumSize = new System.Drawing.Size(366, 82);
        this.messageLabel.MinimumSize = new System.Drawing.Size(366, 82);
        this.messageLabel.Name = "messageLabel";
        this.messageLabel.Size = new System.Drawing.Size(366, 82);
        this.messageLabel.TabIndex = 0;
        this.messageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // button1
        // 
        this.button1.Location = new System.Drawing.Point(12, 105);
        this.button1.Name = "button1";
        this.button1.Size = new System.Drawing.Size(75, 23);
        this.button1.TabIndex = 1;
        this.button1.Text = "button1";
        this.button1.UseVisualStyleBackColor = true;
        // 
        // button2
        // 
        this.button2.Location = new System.Drawing.Point(93, 105);
        this.button2.Name = "button2";
        this.button2.Size = new System.Drawing.Size(75, 23);
        this.button2.TabIndex = 2;
        this.button2.Text = "button2";
        this.button2.UseVisualStyleBackColor = true;
        // 
        // button3
        // 
        this.button3.Location = new System.Drawing.Point(174, 105);
        this.button3.Name = "button3";
        this.button3.Size = new System.Drawing.Size(75, 23);
        this.button3.TabIndex = 3;
        this.button3.Text = "button3";
        this.button3.UseVisualStyleBackColor = true;
        // 
        // button4
        // 
        this.button4.Location = new System.Drawing.Point(255, 105);
        this.button4.Name = "button4";
        this.button4.Size = new System.Drawing.Size(75, 23);
        this.button4.TabIndex = 4;
        this.button4.Text = "button4";
        this.button4.UseVisualStyleBackColor = true;
        // 
        // CustomMessageBox
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(387, 140);
        this.Controls.Add(this.button4);
        this.Controls.Add(this.button3);
        this.Controls.Add(this.button2);
        this.Controls.Add(this.button1);
        this.Controls.Add(this.messageLabel);
        this.MaximumSize = new System.Drawing.Size(403, 179);
        this.MinimumSize = new System.Drawing.Size(403, 179);
        this.Name = "CustomMessageBox";
        this.Text = "CustomMessageBox";
        this.ResumeLayout(false);
    }

    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.Button button3;
    private System.Windows.Forms.Button button4;

    private System.Windows.Forms.Label messageLabel;

    #endregion
}