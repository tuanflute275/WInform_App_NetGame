using BigProject.Data;
using BigProject.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BigProject
{
    public partial class FrmLogin : Form
    {
        SQLDataContext db = new SQLDataContext();
        public FrmLogin()
        {
            InitializeComponent();
            btnLogin.Cursor = Cursors.Hand;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            #region Validating Form
            if (string.IsNullOrEmpty(TxbUserName.Text))
            {
                MessageBox.Show("Username is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TxbUserName.Select();
                return;
            }
            if (TxbUserName.Text.Length < 5)
            {
                MessageBox.Show("Username at least 5 characters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TxbUserName.Select();
                return;
            }

            if (string.IsNullOrEmpty(txbPassword.Text))
            {
                MessageBox.Show("Password is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txbPassword.Select();
                return;
            }
           

            #endregion

            var account = db.Accounts.Where(x => x.UserName == TxbUserName.Text && x.Password == txbPassword.Text).FirstOrDefault();
            if (account != null)
            {
                if(account.Status == 0)
                {
                    MessageBox.Show("Account has been locked !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if(account.Price <= 0)
                {
                    MessageBox.Show("Account has no money !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (account.Type == 1)
                    {
                        FrmMainAdmin frmMainAdmin = new FrmMainAdmin();
                        frmMainAdmin.account = account;
                        frmMainAdmin.ShowDialog();

                    }
                    if (account.Type == 0)
                    {
                        FrmCustomer frmCustomer = new FrmCustomer();
                        frmCustomer.account = account;
                        frmCustomer.Show();
                    }
                }
            }
            else
            {
                MessageBox.Show("Incorrect username or password !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void fLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Do You Want To Close This App ?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != System.Windows.Forms.DialogResult.OK)
            {
                e.Cancel = true;
            }
        }
    }
}
