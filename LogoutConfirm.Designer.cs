namespace DatabaseExperimentSOFSEC1Project
{
    partial class LogoutConfirm
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
            this.AreYouSureText = new System.Windows.Forms.Label();
            this.CancelConfirmForm = new System.Windows.Forms.Button();
            this.LogOutConfirmForm = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // AreYouSureText
            // 
            this.AreYouSureText.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AreYouSureText.Location = new System.Drawing.Point(-1, 9);
            this.AreYouSureText.Name = "AreYouSureText";
            this.AreYouSureText.Size = new System.Drawing.Size(385, 49);
            this.AreYouSureText.TabIndex = 0;
            this.AreYouSureText.Text = "Are you sure you wish to log out?";
            this.AreYouSureText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CancelConfirmForm
            // 
            this.CancelConfirmForm.BackColor = System.Drawing.Color.Gray;
            this.CancelConfirmForm.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CancelConfirmForm.ForeColor = System.Drawing.Color.Black;
            this.CancelConfirmForm.Location = new System.Drawing.Point(201, 61);
            this.CancelConfirmForm.Name = "CancelConfirmForm";
            this.CancelConfirmForm.Size = new System.Drawing.Size(110, 34);
            this.CancelConfirmForm.TabIndex = 52;
            this.CancelConfirmForm.Text = "Cancel";
            this.CancelConfirmForm.UseVisualStyleBackColor = false;
            this.CancelConfirmForm.Click += new System.EventHandler(this.CancelConfirmForm_Click);
            // 
            // LogOutConfirmForm
            // 
            this.LogOutConfirmForm.BackColor = System.Drawing.Color.Red;
            this.LogOutConfirmForm.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LogOutConfirmForm.ForeColor = System.Drawing.Color.Black;
            this.LogOutConfirmForm.Location = new System.Drawing.Point(75, 61);
            this.LogOutConfirmForm.Name = "LogOutConfirmForm";
            this.LogOutConfirmForm.Size = new System.Drawing.Size(110, 34);
            this.LogOutConfirmForm.TabIndex = 53;
            this.LogOutConfirmForm.Text = "Log Out";
            this.LogOutConfirmForm.UseVisualStyleBackColor = false;
            this.LogOutConfirmForm.Click += new System.EventHandler(this.LogOutConfirmForm_Click);
            // 
            // LogoutConfirm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 111);
            this.Controls.Add(this.LogOutConfirmForm);
            this.Controls.Add(this.CancelConfirmForm);
            this.Controls.Add(this.AreYouSureText);
            this.Name = "LogoutConfirm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Confirm Log Out";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label AreYouSureText;
        private System.Windows.Forms.Button CancelConfirmForm;
        private System.Windows.Forms.Button LogOutConfirmForm;
    }
}