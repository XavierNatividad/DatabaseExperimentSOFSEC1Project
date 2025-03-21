using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DatabaseExperimentSOFSEC1Project
{
    public partial class LogoutConfirm : Form
    {
        private Action onLogoutConfirmed;

        public LogoutConfirm(Action logoutCallback)
        {
            InitializeComponent();
            this.onLogoutConfirmed = logoutCallback;
        }

        private void LogOutConfirmForm_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            onLogoutConfirmed?.Invoke();
            this.Close();
        }

        private void CancelConfirmForm_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
