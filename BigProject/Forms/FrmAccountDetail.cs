using BigProject.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BigProject.Forms
{
    public partial class FrmAccountDetail : Form
    {
        public Account account = null;
        public string timeDuration;
        public double payable = 0;
        SQLDataContext db = new SQLDataContext();
        public FrmAccountDetail()
        {
            InitializeComponent();
        }

        private void FrmAccountDetail_Load(object sender, EventArgs e)
        {
            txtUserName.Text = account.UserName;
            txtStatus.Text = account.Status == 1 ? "Enable" : "Disable";
            CultureInfo cultureInfo = new CultureInfo("vi-VN");
            txtMoney.Text = Convert.ToDouble(account.Price).ToString("c", cultureInfo);
            txtTime.Text = timeDuration;
            txtPayable.Text = Convert.ToDouble(payable).ToString("c", cultureInfo);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
