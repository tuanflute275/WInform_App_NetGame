using BigProject.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BigProject.Forms
{
    public partial class FrmCustomer : Form
    {
        public Account account = null;
        SQLDataContext db = new SQLDataContext();
        private int elapsedTimeInSeconds;
        private string time;
        private double payable;
        public FrmCustomer()
        {
            InitializeComponent();
            timer1 = new System.Windows.Forms.Timer();
            timer1.Interval = 1000;
            timer1.Tick += new EventHandler(Timer1_Tick);
            elapsedTimeInSeconds = 0; 
            timer1.Start();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            // tính thơi gian sử dụng
            elapsedTimeInSeconds++;
            int hours = elapsedTimeInSeconds / 3600;
            int minutes = (elapsedTimeInSeconds % 3600) / 60;
            int seconds = elapsedTimeInSeconds % 60;
            lblTimeElapsed.Text = $"Time elapsed: {hours:D2}:{minutes:D2}:{seconds:D2}";
            time = $"{hours:D2}:{minutes:D2}:{seconds:D2}";

            // từ thời gian sử dụng tính số tiền phải trả
            // Tính theo giá 1 giờ là 10.000 vnd
            // Mỗi 1 phút là 167.67 vnd
            payable = Math.Round(Convert.ToDouble(minutes * 166.67));

            // tiến hành trừ tiền trong tài khoản
            var accountUpdate = db.Accounts.Where(x => x.Id.ToString() == account.Id.ToString()).FirstOrDefault();
            if(accountUpdate.Price > 0)
            {
                accountUpdate.Price = Math.Round(Convert.ToDouble(accountUpdate.Price - payable));
                db.SubmitChanges();
            }

        }
        private void FrmCustomer_Load(object sender, EventArgs e)
        {
            // insert time login into table userLog
        }
        private void FrmCustomer_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Stop();
            // insert time logout into table userLog

        }

        private void personalInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmChangePass frmChangePass = new FrmChangePass();
            frmChangePass.accId = account.Id;
            frmChangePass.ShowDialog();
        }

        private void accountDetailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmAccountDetail frmAccountDetail = new FrmAccountDetail();
            frmAccountDetail.account = account;
            frmAccountDetail.timeDuration = time;
            frmAccountDetail.payable = payable;
            frmAccountDetail.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
