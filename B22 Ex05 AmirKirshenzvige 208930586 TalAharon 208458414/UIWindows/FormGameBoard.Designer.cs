
namespace Checkers.UI
{
    partial class FormGameBoard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGameBoard));
            this.panelLegend = new System.Windows.Forms.Panel();
            this.label1Messages = new System.Windows.Forms.Label();
            this.label2Messages = new System.Windows.Forms.Label();
            this.panelBoard = new System.Windows.Forms.Panel();
            this.panelLegend.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelLegend
            // 
            this.panelLegend.Controls.Add(this.label1Messages);
            this.panelLegend.Controls.Add(this.label2Messages);
            this.panelLegend.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelLegend.Location = new System.Drawing.Point(0, 812);
            this.panelLegend.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panelLegend.Name = "panelLegend";
            this.panelLegend.Size = new System.Drawing.Size(781, 100);
            this.panelLegend.TabIndex = 1;
            // 
            // label1Messages
            // 
            this.label1Messages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1Messages.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label1Messages.Font = new System.Drawing.Font("Segoe Print", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1Messages.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.label1Messages.Location = new System.Drawing.Point(0, 2);
            this.label1Messages.Name = "label1Messages";
            this.label1Messages.Size = new System.Drawing.Size(781, 49);
            this.label1Messages.TabIndex = 1;
            this.label1Messages.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1Messages.Parent = this.panelLegend;
            // 
            // label2Messages
            // 
            this.label2Messages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2Messages.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label2Messages.Font = new System.Drawing.Font("Segoe Print", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2Messages.ForeColor = System.Drawing.Color.DimGray;
            this.label2Messages.Location = new System.Drawing.Point(0, 51);
            this.label2Messages.Name = "label2Messages";
            this.label2Messages.Size = new System.Drawing.Size(781, 49);
            this.label2Messages.TabIndex = 0;
            this.label2Messages.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1Messages.Parent = this.panelLegend;
            // 
            // panelBoard
            // 
            this.panelBoard.AutoSize = true;
            this.panelBoard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBoard.Location = new System.Drawing.Point(0, 0);
            this.panelBoard.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelBoard.Name = "panelBoard";
            this.panelBoard.Size = new System.Drawing.Size(781, 812);
            this.panelBoard.TabIndex = 2;
            // 
            // FormGameBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(781, 912);
            this.Controls.Add(this.panelBoard);
            this.Controls.Add(this.panelLegend);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "FormGameBoard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Checkers Game";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormGameBoard_FormClosing);
            this.panelLegend.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panelBoard;
        private System.Windows.Forms.Panel panelLegend;
        private System.Windows.Forms.Label label2Messages;
        private System.Windows.Forms.Label label1Messages;
    }
}