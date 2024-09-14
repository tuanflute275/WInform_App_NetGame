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
    public partial class FrmDeposit : Form
    {
        public int accId = 0;
        SQLDataContext db = new SQLDataContext();
        public FrmDeposit()
        {
            InitializeComponent();
        }

        private void FrmDeposit_Load(object sender, EventArgs e)
        {
            var acc = db.Accounts.Where(x => x.Id.ToString() == accId.ToString()).FirstOrDefault();
            txtUserNameDeposit.Text = acc.UserName;
        }

        private void btnOkFormChangePass_Click(object sender, EventArgs e)
        {
            var acc = db.Accounts.Where(x => x.Id.ToString() == accId.ToString()).FirstOrDefault();

            if (acc != null)
            {
                if (txtMoney.Text.Equals(""))
                {
                    MessageBox.Show("Money is required", "System Notify", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMoney.Select();
                    return;
                }
                if (string.IsNullOrEmpty(txtMoney.Text))
                {
                    MessageBox.Show("Money is required", "System Notify", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMoney.Select();
                    return;
                }
                else if (!decimal.TryParse(txtMoney.Text, out _))  // Kiểm tra xem chuỗi có phải là số không
                {
                    MessageBox.Show("Money must be a valid number", "System Notify", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMoney.Select();
                    return;
                }
                acc.Price += Convert.ToDouble(txtMoney.Text);
                db.SubmitChanges();
                MessageBox.Show("Deposit Money Success !", "System Notify", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }
    }
}
