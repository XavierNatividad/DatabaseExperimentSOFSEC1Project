namespace DatabaseExperimentSOFSEC1Project
{
    partial class SessionTimeout
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
            this.SessionTimeoutMessage = new System.Windows.Forms.Label();
            this.CancelConfirmForm = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SessionTimeoutMessage
            // 
            this.SessionTimeoutMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SessionTimeoutMessage.Location = new System.Drawing.Point(1, 9);
            this.SessionTimeoutMessage.Name = "SessionTimeoutMessage";
            this.SessionTimeoutMessage.Size = new System.Drawing.Size(384, 49);
            this.SessionTimeoutMessage.TabIndex = 1;
            this.SessionTimeoutMessage.Text = "You have been automatically logged out after 10 minutes of inactivity.";
            this.SessionTimeoutMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CancelConfirmForm
            // 
            this.CancelConfirmForm.BackColor = System.Drawing.Color.Gray;
            this.CancelConfirmForm.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CancelConfirmForm.ForeColor = System.Drawing.Color.Black;
            this.CancelConfirmForm.Location = new System.Drawing.Point(137, 61);
            this.CancelConfirmForm.Name = "CancelConfirmForm";
            this.CancelConfirmForm.Size = new System.Drawing.Size(110, 34);
            this.CancelConfirmForm.TabIndex = 53;
            this.CancelConfirmForm.Text = "Close";
            this.CancelConfirmForm.UseVisualStyleBackColor = false;
            this.CancelConfirmForm.Click += new System.EventHandler(this.CancelConfirmForm_Click);
            // 
            // SessionTimeout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 111);
            this.Controls.Add(this.CancelConfirmForm);
            this.Controls.Add(this.SessionTimeoutMessage);
            this.Name = "SessionTimeout";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Session Timeout";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SessionTimeout_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label SessionTimeoutMessage;
        private System.Windows.Forms.Button CancelConfirmForm;
    }
}