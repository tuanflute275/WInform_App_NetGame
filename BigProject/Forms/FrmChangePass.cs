using BigProject.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BigProject.Forms
{
    public partial class FrmChangePass : Form
    {
        public int accId = 0;
        SQLDataContext db = new SQLDataContext();
        public FrmChangePass()
        {
            InitializeComponent();
        }

        private void FrmChangePass_Load(object sender, EventArgs e)
        {
            var acc = db.Accounts.Where(x => x.Id.ToString() == accId.ToString()).FirstOrDefault();
            txtUserNameChangePass.Text = acc.UserName;
        }

        private void btnSavePass_Click(object sender, EventArgs e)
        {
            var acc = db.Accounts.Where(x => x.Id.ToString() == accId.ToString()).FirstOrDefault();

            if (acc != null)
            {
                if (txtOldPassword.Text.Equals(""))
                {
                    MessageBox.Show("Old Password is required", "System Notify", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtOldPassword.Select();
                    return;
                }
                if (txtNewPass.Text.Equals(""))
                {
                    MessageBox.Show("New Password is required", "System Notify", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNewPass.Select();
                    return;
                }
                if (txtConfirmNewPass.Text.Equals(""))
                {
                    MessageBox.Show("Confirm Password is required", "System Notify", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtConfirmNewPass.Select();
                    return;
                }
                if (!txtConfirmNewPass.Text.Equals(txtNewPass.Text))
                {
                    MessageBox.Show("Confirm Password does not match", "System Notify", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtConfirmNewPass.Select();
                    return;
                }

                acc.Password = txtNewPass.Text;
               
                db.SubmitChanges();
                MessageBox.Show("Update Data Success !", "System Notify", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
               
            }
        }
    }
}
