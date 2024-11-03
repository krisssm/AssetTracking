namespace AssetTracking
{
    partial class Form1
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
            this.btnFloorplan = new System.Windows.Forms.Button();
            this.textSearch = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblValRSSI = new System.Windows.Forms.Label();
            this.lblRSSI = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnFloorplan
            // 
            this.btnFloorplan.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnFloorplan.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnFloorplan.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnFloorplan.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFloorplan.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnFloorplan.Location = new System.Drawing.Point(58, 68);
            this.btnFloorplan.Name = "btnFloorplan";
            this.btnFloorplan.Size = new System.Drawing.Size(135, 50);
            this.btnFloorplan.TabIndex = 0;
            this.btnFloorplan.Text = "Floor plan";
            this.btnFloorplan.UseVisualStyleBackColor = false;
            // 
            // textSearch
            // 
            this.textSearch.Location = new System.Drawing.Point(41, 195);
            this.textSearch.Name = "textSearch";
            this.textSearch.Size = new System.Drawing.Size(168, 20);
            this.textSearch.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(68, 167);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "Enter asset";
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSearch.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnSearch.Location = new System.Drawing.Point(58, 221);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(135, 50);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblValRSSI
            // 
            this.lblValRSSI.AutoSize = true;
            this.lblValRSSI.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValRSSI.Location = new System.Drawing.Point(21, 295);
            this.lblValRSSI.Name = "lblValRSSI";
            this.lblValRSSI.Size = new System.Drawing.Size(140, 25);
            this.lblValRSSI.TabIndex = 5;
            this.lblValRSSI.Text = "Value of RSSI:";
            // 
            // lblRSSI
            // 
            this.lblRSSI.AutoSize = true;
            this.lblRSSI.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRSSI.Location = new System.Drawing.Point(170, 295);
            this.lblRSSI.Name = "lblRSSI";
            this.lblRSSI.Size = new System.Drawing.Size(42, 25);
            this.lblRSSI.TabIndex = 6;
            this.lblRSSI.Text = "-53";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(1012, 750);
            this.Controls.Add(this.lblRSSI);
            this.Controls.Add(this.lblValRSSI);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textSearch);
            this.Controls.Add(this.btnFloorplan);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnFloorplan;
        private System.Windows.Forms.TextBox textSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblValRSSI;
        private System.Windows.Forms.Label lblRSSI;
    }
}

