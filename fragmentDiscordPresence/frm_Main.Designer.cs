namespace fragmentDiscordPresence
{
    partial class frm_Main
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
            this.components = new System.ComponentModel.Container();
            this.tmr_PCSX2Check = new System.Windows.Forms.Timer(this.components);
            this.tmr_GameData = new System.Windows.Forms.Timer(this.components);
            this.lbl_PCSX2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tmr_PCSX2Check
            // 
            this.tmr_PCSX2Check.Enabled = true;
            this.tmr_PCSX2Check.Tick += new System.EventHandler(this.tmr_PCSX2Check_Tick);
            // 
            // tmr_GameData
            // 
            this.tmr_GameData.Enabled = true;
            this.tmr_GameData.Interval = 3000;
            this.tmr_GameData.Tick += new System.EventHandler(this.tmr_GameData_Tick);
            // 
            // lbl_PCSX2
            // 
            this.lbl_PCSX2.AutoSize = true;
            this.lbl_PCSX2.Location = new System.Drawing.Point(13, 13);
            this.lbl_PCSX2.Name = "lbl_PCSX2";
            this.lbl_PCSX2.Size = new System.Drawing.Size(35, 13);
            this.lbl_PCSX2.TabIndex = 0;
            this.lbl_PCSX2.Text = "label1";
            // 
            // frm_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(221, 35);
            this.Controls.Add(this.lbl_PCSX2);
            this.MaximumSize = new System.Drawing.Size(237, 74);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(237, 74);
            this.Name = "frm_Main";
            this.Text = "Fragment";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frm_Main_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer tmr_PCSX2Check;
        private System.Windows.Forms.Timer tmr_GameData;
        private System.Windows.Forms.Label lbl_PCSX2;
    }
}

