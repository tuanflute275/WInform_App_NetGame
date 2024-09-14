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
    public partial class FrmMainAdmin : Form
    {
        public Account account = null;
        SQLDataContext db = new SQLDataContext();
        public FrmMainAdmin()
        {
            InitializeComponent();
            FetchingAccount();
            FetchingComputer();
            FetchingFoodAndDrink();
            FetchingLog();
            FetchingOrder();
            LoadDateTimePickerBill();
            nmPriceFD.DecimalPlaces = 2;
            nmPriceFD.ThousandsSeparator = true;
            cboComputerNameLog.DataSource = db.Computers;
            cboComputerNameLog.DisplayMember = "Name";
            cboComputerNameLog.ValueMember = "ComputerId";
            cboSearchLog.DataSource = db.Computers;
            cboSearchLog.DisplayMember = "Name";
            cboSearchLog.ValueMember = "ComputerId";
        }

        #region accountTab
        public void FetchingAccount()
        {
            cboRoleAccount.SelectedItem = "User";
            cboStatusAccount.SelectedItem = "Enable";
            var accs = from acc in db.Accounts
                       select new
                       {
                           Id = acc.Id,
                           Username = acc.UserName,
                           Password = acc.Password,
                           Price = acc.Price,
                           Status = acc.Status == 1 ? "Enable" : "Disabled",
                           Role = acc.Type == 1 ? "Admin" : "User"
                       };

            var accList = accs.ToList();

            var formattedAccList = accList.Select(f => new
            {
                Id = f.Id,
                Username = f.Username,
                Password = f.Password,
                Price = Convert.ToDouble(f.Price).ToString("N0") + " " + "VND",
                Status = f.Status,
                Role = f.Role,
            });

            dgvAccounts.DataSource = formattedAccList.ToList();
        }

        public void ShowDetailAccount()
        {
            if (dgvAccounts.CurrentRow != null)
            {
                var currentRow = dgvAccounts.CurrentRow;
                var id = currentRow.Cells[0].Value;
                var acc = db.Accounts.Where(x => x.Id.ToString() == id.ToString()).FirstOrDefault();
                txtIdAccount.Text = acc.Id.ToString();
                txtUsernameAccount.Text = acc.UserName.ToString();
                txtPasswordAccount.Text = acc.Password.ToString();
                nmPriceAccount.Value = (decimal)acc.Price;
                cboStatusAccount.SelectedItem = acc.Status == 1 ? "Enable" : "Disabled";
                cboRoleAccount.SelectedItem = acc.Type == 1 ? "Admin" : "User";
            }
        }

        private void ClearFormsAccount()
        {
            txtIdAccount.Text = txtUsernameAccount.Text = txtSearchAccount.Text = txtPasswordAccount.Text = string.Empty;
            cboRoleAccount.SelectedItem = "User";
            cboStatusAccount.SelectedItem = "Enable";
            nmPriceAccount.Value = 0;
        }

        private void dgvAccounts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ShowDetailAccount();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtSearchAccount.Text.Equals(""))
            {
                MessageBox.Show("Search field is required", "System Notify", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSearchAccount.Select();
                return;
            }
            else
            {
                dgvAccounts.DataSource = null;
                dgvAccounts.DataSource = null;
                var accs = from acc in db.Accounts
                           where acc.UserName.Contains(txtSearchAccount.Text)
                           select new
                           {
                               Id = acc.Id,
                               Username = acc.UserName,
                               Password = acc.Password,
                               Role = acc.Type == 1 ? "Admin" : "User"
                           };
                dgvAccounts.DataSource = accs.ToList();
            }
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Do You Wanna Delete This Account ?", "System Notify", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
                    var acc = db.Accounts.Where(x => x.Id.ToString() == txtIdAccount.Text).FirstOrDefault();
                    if (acc != null)
                    {
                        if (account.Equals(acc))
                        {
                            MessageBox.Show("Cannot Delete Current Account !", "System Notify", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        db.Accounts.DeleteOnSubmit(acc);
                        db.SubmitChanges();
                        MessageBox.Show("Delete Success !", "System Notify", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        FetchingAccount();
                        ClearFormsAccount();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "System Error");
                }

            }
        }

        private void btnUpdateAccount_Click(object sender, EventArgs e)
        {
            int currentRowIndex = dgvAccounts.CurrentCell?.RowIndex ?? -1;
            var acc = db.Accounts.Where(x => x.Id.ToString() == txtIdAccount.Text).FirstOrDefault();

            if (acc != null)
            {
                if (txtUsernameAccount.Text.Equals(""))
                {
                    MessageBox.Show("Username is required", "System Notify", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUsernameAccount.Select();
                    return;
                }
                if (txtUsernameAccount.Text.Length < 5)
                {
                    MessageBox.Show("Username at least 5 characters", "System Notify", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUsernameAccount.Select();
                    return;
                }

                if (txtPasswordAccount.Text.Equals(""))
                {
                    MessageBox.Show("Password is required", "System Notify", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPasswordAccount.Select();
                    return;
                }
                if (txtPasswordAccount.Text.Length < 8)
                {
                    MessageBox.Show("Password at least 8 characters", "System Notify", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPasswordAccount.Select();
                    return;
                }

                acc.UserName = txtUsernameAccount.Text;
                acc.Password = txtPasswordAccount.Text;
                acc.Price = Convert.ToDouble(nmPriceAccount.Value);
                acc.Type = cboRoleAccount.Text.Equals("Admin") ? 1 : 0;
                acc.Status = cboStatusAccount.Text.Equals("Enable") ? 1 : 0;

                db.SubmitChanges();
                MessageBox.Show("Update Data Success !", "System Notify", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFormsAccount();
                FetchingAccount();
                if (currentRowIndex >= 0 && currentRowIndex < dgvAccounts.Rows.Count)
                {
                    dgvAccounts.CurrentCell = dgvAccounts.Rows[currentRowIndex].Cells[0];
                }
            }
        }

        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            if (txtUsernameAccount.Text.Equals(""))
            {
                MessageBox.Show("Username is required", "System Notify", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsernameAccount.Select();
                return;
            }
            if (txtUsernameAccount.Text.Length < 5)
            {
                MessageBox.Show("Username at least 5 characters", "System Notify", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsernameAccount.Select();
                return;
            }

            if (txtPasswordAccount.Text.Equals(""))
            {
                MessageBox.Show("Password is required", "System Notify", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPasswordAccount.Select();
                return;
            }
            if (txtPasswordAccount.Text.Length < 8)
            {
                MessageBox.Show("Password at least 8 characters", "System Notify", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPasswordAccount.Select();
                return;
            }

            var UsAdd = db.Accounts.Where(x => x.UserName.ToString() == txtUsernameAccount.Text).FirstOrDefault();
            if (UsAdd != null || UsAdd.Equals(""))
            {
                MessageBox.Show("Name already exists", "System Notify", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var acc = new Account();
            acc.UserName = txtUsernameAccount.Text;
            acc.Password = txtPasswordAccount.Text;
            acc.Price = Convert.ToDouble(nmPriceAccount.Value);
            acc.Type = cboRoleAccount.Text.Equals("Admin") ? 1 : 0;
            acc.Status = cboStatusAccount.Text.Equals("Enable") ? 1 : 0;

            db.Accounts.InsertOnSubmit(acc);
            db.SubmitChanges();
            MessageBox.Show("Insert Data Success !", "System Notify", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearFormsAccount();
            FetchingAccount();
        }

        private void btnRefreshAccount_Click(object sender, EventArgs e)
        {
            FetchingAccount();
            ClearFormsAccount();
        }

        private void btnShutdown_Click(object sender, EventArgs e)
        {
            int currentRowIndex = dgvAccounts.CurrentCell?.RowIndex ?? -1;
            var acc = db.Accounts.Where(x => x.Id.ToString() == txtIdAccount.Text).FirstOrDefault();
            var result = MessageBox.Show("Do You Wanna Shutdown This Account ?", "System Notify", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                if(acc.Id == 1 || acc.UserName.Equals("Admin") || acc.Type == 1)
                {
                    MessageBox.Show("You Can Not Delete Account Admin ?", "System Notify", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    acc.UserName = txtUsernameAccount.Text;
                    acc.Password = txtPasswordAccount.Text;
                    acc.Price = Convert.ToDouble(nmPriceAccount.Value);
                    acc.Type = cboRoleAccount.Text.Equals("Admin") ? 1 : 0;
                    acc.Status = 0;

                    db.SubmitChanges();
                    MessageBox.Show("Success !", "System Notify", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFormsAccount();
                    FetchingAccount();
                    // giữ vị trí sửa hiện tại 
                    if (currentRowIndex >= 0 && currentRowIndex < dgvAccounts.Rows.Count)
                    {
                        dgvAccounts.CurrentCell = dgvAccounts.Rows[currentRowIndex].Cells[0];
                    }
                }
            }
        }
        
        private void btnEnable_Click(object sender, EventArgs e)
        {
            int currentRowIndex = dgvAccounts.CurrentCell?.RowIndex ?? -1;
            var acc = db.Accounts.Where(x => x.Id.ToString() == txtIdAccount.Text).FirstOrDefault();
            var result = MessageBox.Show("Do You Wanna Enable This Account ?", "System Notify", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                acc.UserName = txtUsernameAccount.Text;
                acc.Password = txtPasswordAccount.Text;
                acc.Price = Convert.ToDouble(nmPriceAccount.Value);
                acc.Type = cboRoleAccount.Text.Equals("Admin") ? 1 : 0;
                acc.Status = 1;

                db.SubmitChanges();
                MessageBox.Show("Success !", "System Notify", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFormsAccount();
                FetchingAccount();
                // giữ vị trí sửa hiện tại 
                if (currentRowIndex >= 0 && currentRowIndex < dgvAccounts.Rows.Count)
                {
                    dgvAccounts.CurrentCell = dgvAccounts.Rows[currentRowIndex].Cells[0];
                }
            }
        }
        
        private void btnChangePassAccount_Click(object sender, EventArgs e)
        {
            if (dgvAccounts.CurrentRow != null)
            {
                var currentRow = dgvAccounts.CurrentRow;
                var id = currentRow.Cells[0].Value;
                FrmChangePass frmChangePass = new FrmChangePass();
                frmChangePass.accId = (int)id;
                frmChangePass.ShowDialog();
            }
        }
        
        private void btnDeposit_Click(object sender, EventArgs e)
        {
            int currentRowIndex = dgvAccounts.CurrentCell?.RowIndex ?? -1;
            if (dgvAccounts.CurrentRow != null)
            {
                var currentRow = dgvAccounts.CurrentRow;
                var id = currentRow.Cells[0].Value;
                FrmDeposit frmDeposit = new FrmDeposit();
                frmDeposit.accId = (int)id;
                frmDeposit.ShowDialog();
            }
            FetchingAccount();
            if (currentRowIndex >= 0 && currentRowIndex < dgvAccounts.Rows.Count)
            {
                dgvAccounts.CurrentCell = dgvAccounts.Rows[currentRowIndex].Cells[0];
            }
        }

        #endregion


        #region computerTab
        public void FetchingComputer()
        {
            cboStatusCategory.SelectedItem = "Trống";
            var computer = from com in db.Computers
                           select new
                           {
                               Id = com.ComputerID,
                               Name = com.Name,
                               Status = com.Status ? "Đang sử dụng" : "Trống"
                           };
            dgvComputer.DataSource = computer.ToList();
        }
        private void btnSearchComputer_Click(object sender, EventArgs e)
        {
            if (txtSearchComputer.Text.Equals(""))
            {
                MessageBox.Show("Search field is required", "System Notify", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSearchComputer.Select();
                return;
            }
            else
            {
                dgvComputer.DataSource = null;
                var computers = from com in db.Computers
                                where com.Name.Contains(txtSearchComputer.Text)
                                select new
                                {
                                    Id = com.ComputerID,
                                    Name = com.Name,
                                    Status = com.Status ? "Đang sử dụng" : "Trống"
                                };
                dgvComputer.DataSource = computers.ToList();
            }
        }
        private void dgvComputer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ShowDetailCompuetr();
        }
        public void ShowDetailCompuetr()
        {
            if (dgvComputer.CurrentRow != null)
            {
                var currentRow = dgvComputer.CurrentRow;
                var id = currentRow.Cells[0].Value;
                var computer = db.Computers.Where(x => x.ComputerID.ToString() == id.ToString()).FirstOrDefault();
                txtIdComputer.Text = computer.ComputerID.ToString();
                txtNameComputer.Text = computer.Name.ToString();
                cboStatusCategory.SelectedItem = computer.Status ? "Đang sử dụng" : "Trống";
            }
        }
        private void ClearFormsComputer()
        {
            txtIdComputer.Text = txtNameComputer.Text = txtSearchComputer.Text = string.Empty;
            cboStatusCategory.SelectedItem = "Trống";
        }
        private void btnDeleteComputer_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Do You Wanna Delete This Account ?", "System Notify", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
                    var com = db.Computers.Where(x => x.ComputerID.ToString() == txtIdComputer.Text).FirstOrDefault();
                    if (com != null)
                    {
                        db.Computers.DeleteOnSubmit(com);
                        db.SubmitChanges();
                        MessageBox.Show("Delete Success !", "System Notify", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        FetchingComputer();
                        ClearFormsComputer();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "System Error");
                }

            }
        }
        private void btnUpdateComputer_Click(object sender, EventArgs e)
        {
            int currentRowIndex = dgvComputer.CurrentCell?.RowIndex ?? -1;
            var com = db.Computers.Where(x => x.ComputerID.ToString() == txtIdComputer.Text).FirstOrDefault();

            if (com != null)
            {
                if (txtNameComputer.Text.Equals(""))
                {
                    MessageBox.Show("Name is required", "System Notify", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNameComputer.Select();
                    return;
                }
                com.Name = txtNameComputer.Text;
                com.Status = cboStatusCategory.Text.Equals("Trống") ? false : true;
                db.SubmitChanges();
                MessageBox.Show("Update Data Success !", "System Notify", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFormsComputer();
                FetchingComputer();
                // giữ vị trí sửa hiện tại 
                if (currentRowIndex >= 0 && currentRowIndex < dgvComputer.Rows.Count)
                {
                    dgvComputer.CurrentCell = dgvComputer.Rows[currentRowIndex].Cells[0];
                }
            }
        }
        private void btnAddComputer_Click(object sender, EventArgs e)
        {
            if (txtNameComputer.Text.Equals(""))
            {
                MessageBox.Show("Name is required", "System Notify", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNameComputer.Select();
                return;
            }
            var comAdd = db.Computers.Where(x => x.Name.ToString() == txtNameComputer.Text).FirstOrDefault();
            if (comAdd != null || comAdd.Equals("")) 
            {
                MessageBox.Show("Name Computer already exists", "System Notify", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var com = new Computer();
            com.Name = txtNameComputer.Text;
            com.Status = cboStatusCategory.Text.Equals("Trống") ? false : true;
            db.Computers.InsertOnSubmit(com);
            db.SubmitChanges();
            MessageBox.Show("Insert Data Success !", "System Notify", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearFormsComputer();
            FetchingComputer();
        }
        private void btnRefreshComputer_Click(object sender, EventArgs e)
        {
            FetchingComputer();
            ClearFormsComputer();
        }

        #endregion

        #region Food and drinks
        public void FetchingFoodAndDrink()
        {
            var food = from fd in db.DrinkSnacks
                       select new
                       {
                           Id = fd.DrinkSnackID,
                           Name = fd.Name,
                           Price = fd.Price
                       };

            var foodList = food.ToList();

            var formattedFoodList = foodList.Select(f => new
            {
                Id = f.Id,
                Name = f.Name,
                Price = f.Price.ToString("N0") + " " + "VND"
            });

            dgvFoodAndDrink.DataSource = formattedFoodList.ToList();
        }
        private void ClearFormFoodAndDrink()
        {
            txtIdFD.Text = txtNameFD.Text = txtSearchFD.Text = string.Empty;
            nmPriceFD.Value = 0;
        }
        public void ShowDetailFoodAndDrink()
        {
            if (dgvFoodAndDrink.CurrentRow != null)
            {
                var currentRow = dgvFoodAndDrink.CurrentRow;
                var id = currentRow.Cells[0].Value;
                var fd = db.DrinkSnacks.Where(x => x.DrinkSnackID.ToString() == id.ToString()).FirstOrDefault();
                txtIdFD.Text = fd.DrinkSnackID.ToString();
                txtNameFD.Text = fd.Name.ToString();
                nmPriceFD.Value = fd.Price;
            }
        }
        private void dgvFoodAndDrink_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ShowDetailFoodAndDrink();
        }
        private void btnRefreshFD_Click(object sender, EventArgs e)
        {
            FetchingFoodAndDrink();
            ClearFormFoodAndDrink();
        }
        private void btnSearchFD_Click(object sender, EventArgs e)
        {
            if (txtSearchFD.Text.Equals(""))
            {
                MessageBox.Show("Search field is required", "System Notify", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSearchFD.Select();
                return;
            }
            else
            {
                dgvFoodAndDrink.DataSource = null;
                var foods = from fd in db.DrinkSnacks
                                where fd.Name.Contains(txtSearchFD.Text)
                            select new
                            {
                                Id = fd.DrinkSnackID,
                                Name = fd.Name,
                                Price = fd.Price,
                            };
                dgvFoodAndDrink.DataSource = foods.ToList();
            }
        }
        private void btnDeleteFD_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Do You Wanna Delete This Account ?", "System Notify", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
                    var fd = db.DrinkSnacks.Where(x => x.DrinkSnackID.ToString() == txtIdFD.Text).FirstOrDefault();
                    if (fd != null)
                    {
                        db.DrinkSnacks.DeleteOnSubmit(fd);
                        db.SubmitChanges();
                        MessageBox.Show("Delete Success !", "System Notify", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        FetchingFoodAndDrink();
                        ClearFormFoodAndDrink();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "System Error");
                }

            }
        }

        private void btnUpdateFD_Click(object sender, EventArgs e)
        {
            int currentRowIndex = dgvFoodAndDrink.CurrentCell?.RowIndex ?? -1;
            var fd = db.DrinkSnacks.Where(x => x.DrinkSnackID.ToString() == txtIdFD.Text).FirstOrDefault();

            if (fd != null)
            {
                if (txtNameFD.Text.Equals(""))
                {
                    MessageBox.Show("Name is required", "System Notify", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNameFD.Select();
                    return;
                }
                fd.Name = txtNameFD.Text;
                fd.Price = nmPriceFD.Value;
                db.SubmitChanges();
                MessageBox.Show("Update Data Success !", "System Notify", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFormFoodAndDrink();
                FetchingFoodAndDrink();
                // giữ vị trí sửa hiện tại 
                if (currentRowIndex >= 0 && currentRowIndex < dgvFoodAndDrink.Rows.Count)
                {
                    dgvFoodAndDrink.CurrentCell = dgvFoodAndDrink.Rows[currentRowIndex].Cells[0];
                }
            }
        }

        private void btnAddFD_Click(object sender, EventArgs e)
        {
            if (txtNameFD.Text.Equals(""))
            {
                MessageBox.Show("Name is required", "System Notify", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNameFD.Select();
                return;
            }
            var fdAdd = db.DrinkSnacks.Where(x => x.Name.ToString() == txtNameFD.Text).FirstOrDefault();
            if (fdAdd != null || fdAdd.Equals(""))
            {
                MessageBox.Show("Name already exists", "System Notify", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var fd = new DrinkSnack();
            fd.Name = txtNameFD.Text;
            fd.Price = nmPriceFD.Value;
            db.DrinkSnacks.InsertOnSubmit(fd);
            db.SubmitChanges();
            MessageBox.Show("Insert Data Success !", "System Notify", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearFormFoodAndDrink();
            FetchingFoodAndDrink();
        }

        #endregion

        #region logUser
        public static string ConvertNumberToWords(double number)
        {
            if (number <= 0) return "Không Đồng";
            string[] units = { "", "Ngàn", "Triệu", "Tỷ" };
            string[] digits = { "", "Một", "Hai", "Ba", "Bốn", "Năm", "Sáu", "Bảy", "Tám", "Chín" };
            string result = "";
            int unitIndex = 0;

            while (number > 0)
            {
                int part = (int)(number % 1000);
                number /= 1000;
                string partText = "";

                if (part > 99)
                {
                    partText += digits[part / 100] + " Trăm ";
                    part %= 100;
                }
                if (part > 9)
                {
                    if (part < 20)
                    {
                        partText += "Mười " + digits[part % 10];
                        part = 0;
                    }
                    else
                    {
                        partText += digits[part / 10] + " Mươi ";
                        part %= 10;
                    }
                }
                if (part > 0)
                {
                    partText += digits[part];
                }

                if (partText != "")
                {
                    result = partText + units[unitIndex] + " " + result;
                }

                unitIndex++;
            }

            return result.Trim() + " Đồng";
        }

        public void FetchingLog()
        {
            var logs = from u in db.UsageLogs
                       join c in db.Computers on u.ComputerID equals c.ComputerID
                       join a in db.Accounts on u.UserID equals a.Id
                       orderby u.EndTime descending
                       select new
                       {
                           Id = u.UsageLogID,
                           ComputerName = c.Name,
                           StartDate = u.StartTime,
                           EndDate = u.EndTime,
                           Duration = (DateTime)u.EndTime - u.StartTime,
                           TotalAmount = u.TotalAmount,
                           UserPrice = a.Price,
                           UserName = a.UserName
                       };

            var logList = logs.ToList();
            const double pricePerMinute = 166.67;
            var formattedFoodList = logList.Select(f =>
            {
                var totalMinutes = (f.Duration.Days * 24 * 60) + (f.Duration.Hours * 60) + f.Duration.Minutes;
                var totalCost = Math.Round(totalMinutes * pricePerMinute);
                // Tính số tiền còn lại, và nếu nhỏ hơn 0 thì gán bằng 0
                var remainingAmount = Math.Max(0, (double)f.UserPrice - totalCost);

                return new
                {
                    Id = f.Id,
                    UserName = f.UserName,
                    Money = Convert.ToDouble(f.UserPrice).ToString("N0") + " VND",
                    ComputerName = f.ComputerName,
                    StartDate = f.StartDate.ToString("dd/MM/yyyy HH:mm:ss"),
                    EndDate = f.EndDate?.ToString("dd/MM/yyyy HH:mm:ss"),
                    DurationInMinutes = $"{totalMinutes} Minutes",
                    Duration = $"{f.Duration.Days * 24 + f.Duration.Hours} hours {f.Duration.Minutes} minutes",
                    TotalAmount = $"{totalCost.ToString("N0")} VND",
                    RemainingAmount = $"{remainingAmount.ToString("N0")} VND",
                    RemainingAmountInWords = ConvertNumberToWords(remainingAmount)
                };
            });

            dgvLog.DataSource = formattedFoodList.ToList();


            dgvLog.DataSource = formattedFoodList.ToList();
        }

        public void ShowDetailLog()
        {
            if (dgvLog.CurrentRow != null)
            {
                var currentRow = dgvLog.CurrentRow;
                var id = currentRow.Cells[0].Value;
                var log = db.UsageLogs.Where(x => x.UsageLogID.ToString() == id.ToString()).FirstOrDefault();
                txtIdLog.Text = log.UsageLogID.ToString();
                cboComputerNameLog.SelectedValue = log.ComputerID; 
                nmAmountLog.Value = log.TotalAmount;
                dtpkFormDateLog.Value = log.StartTime;
                dtpkToDateLog.Value = (DateTime)log.EndTime;
            }
        }

        private void dgvLog_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ShowDetailLog();
        }
        private void btnRefreshLog_Click(object sender, EventArgs e)
        {
            FetchingLog();
        }
        private void btnSearchLog_Click(object sender, EventArgs e)
        {
            dgvLog.DataSource = null;
            var logs = from u in db.UsageLogs
                        join c in db.Computers on u.ComputerID equals c.ComputerID
                        where u.ComputerID.ToString().Contains(cboSearchLog.SelectedValue.ToString())
                       select new
                        {
                            Id = u.UsageLogID,
                            ComputerName = c.Name,
                            StartDate = u.StartTime,
                            EndDate = u.EndTime,
                            TotalAmount = u.TotalAmount,
                        };

            var logList = logs.ToList();

            var formattedFoodList = logList.Select(f => new
            {
                Id = f.Id,
                ComputerName = f.ComputerName,
                StartDate = f.StartDate.ToString("dd/MM/yyyy HH:mm:ss"),
                EndDate = f.EndDate?.ToString("dd/MM/yyyy HH:mm:ss"),
                TotalAmount = f.TotalAmount.ToString("N0") + " " + "VND"
            });

            dgvLog.DataSource = formattedFoodList.ToList();
            
        }

        private void btnAddLog_Click(object sender, EventArgs e)
        {
            var log = new UsageLog();
            log.UserID = account.Id;
            log.ComputerID = (int)cboComputerNameLog.SelectedValue;
            log.StartTime = dtpkFormDateLog.Value;
            log.EndTime = dtpkToDateLog.Value;
            log.TotalAmount = nmAmountLog.Value;
            db.UsageLogs.InsertOnSubmit(log);
            db.SubmitChanges();
            MessageBox.Show("Insert Data Success !", "System Notify", MessageBoxButtons.OK, MessageBoxIcon.Information);
            FetchingLog();
        }

        private void btnUpdateLog_Click(object sender, EventArgs e)
        {
            int currentRowIndex = dgvLog.CurrentCell?.RowIndex ?? -1;
            var log = db.UsageLogs.Where(x => x.UsageLogID.ToString() == txtIdLog.Text).FirstOrDefault();

            if (log != null)
            {
                log.UserID = account.Id;
                log.ComputerID = (int)cboComputerNameLog.SelectedValue;
                log.StartTime = dtpkFormDateLog.Value;
                log.EndTime = dtpkToDateLog.Value;
                log.TotalAmount = nmAmountLog.Value;
                db.SubmitChanges();
                MessageBox.Show("Update Data Success !", "System Notify", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FetchingLog();
                // giữ vị trí sửa hiện tại 
                if (currentRowIndex >= 0 && currentRowIndex < dgvLog.Rows.Count)
                {
                    dgvLog.CurrentCell = dgvLog.Rows[currentRowIndex].Cells[0];
                }
            }
        }

        private void btnDeleteLog_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Do You Wanna Delete This Account ?", "System Notify", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
                    var log = db.UsageLogs.Where(x => x.UsageLogID.ToString() == txtIdLog.Text).FirstOrDefault();
                    if (log != null)
                    {
                        db.UsageLogs.DeleteOnSubmit(log);
                        db.SubmitChanges();
                        MessageBox.Show("Delete Success !", "System Notify", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        FetchingLog();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "System Error");
                }

            }
        }
        #endregion

        #region Order 
        public void FetchingOrder()
        {
            var orders = from o in db.Orders
                         join ul in db.UsageLogs on o.UsageLogID equals ul.UsageLogID
                         join c in db.Computers on ul.ComputerID equals c.ComputerID
                         join d in db.DrinkSnacks on o.DrinkSnackID equals d.DrinkSnackID
                       select new
                       {
                           Id = o.OrderID,
                           ComputerName = c.Name,
                           FoodName = d.Name,
                           Price = d.Price,
                           Quantity = o.Quantity,
                           Amount = d.Price * o.Quantity,
                           StartDate = ul.StartTime,
                           EndDate = ul.EndTime,
                       };
            var orderList = orders.ToList();
            var totalAmount = orderList.Sum(f => f.Amount);
            CultureInfo cultureInfo = new CultureInfo("vi-VN");
            txtTotalAmountRevenue.Text = totalAmount.ToString("c", cultureInfo);
            var formattedOrderList = orderList.Select(f => new
            {
                Id = f.Id,
                ComputerName = f.ComputerName,
                FoodAndDrinkName = f.FoodName,
                Price = Convert.ToDouble(f.Price).ToString("N0") + " " + "VND",
                Quantity = f.Quantity,
                Amount = Convert.ToDouble(f.Amount).ToString("N0") + " " + "VND",
                StartDate = f.StartDate.ToString("dd/MM/yyyy HH:mm:ss"),
                EndDate = f.EndDate?.ToString("dd/MM/yyyy HH:mm:ss"),
            });

            dgvRevenue.DataSource = formattedOrderList.ToList();
        }

        void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpkFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpkToDate.Value = dtpkFromDate.Value.AddMonths(1).AddDays(-1);
        }

        private void btnRevenue_Click(object sender, EventArgs e)
        {
            DateTime formDate = dtpkFromDate.Value.Date;
            DateTime toDate = dtpkToDate.Value.Date;

            var orders = from o in db.Orders
                         join ul in db.UsageLogs on o.UsageLogID equals ul.UsageLogID
                         join c in db.Computers on ul.ComputerID equals c.ComputerID
                         join d in db.DrinkSnacks on o.DrinkSnackID equals d.DrinkSnackID
                         where ul.EndTime >= formDate && ul.EndTime <= toDate
                         select new
                         {
                             Id = o.OrderID,
                             ComputerName = c.Name,
                             FoodName = d.Name,
                             Price = d.Price,
                             Quantity = o.Quantity,
                             Amount = d.Price * o.Quantity,
                             StartDate = ul.StartTime,
                             EndDate = ul.EndTime,
                         };

            var orderList = orders.ToList();
            var totalAmount = orderList.Sum(f => f.Amount);
            CultureInfo cultureInfo = new CultureInfo("vi-VN");
            txtTotalAmountRevenue.Text = totalAmount.ToString("c", cultureInfo);
            var formattedOrderList = orderList.Select(f => new
            {
                Id = f.Id,
                ComputerName = f.ComputerName,
                FoodName = f.FoodName,
                PriceFood = Convert.ToDouble(f.Price).ToString("N0") + " " + "VND",
                Quantity = f.Quantity,
                AmountOfFoodMoney = Convert.ToDouble(f.Amount).ToString("N0") + " " + "VND",
                StartDate = f.StartDate.ToString("dd/MM/yyyy HH:mm:ss"),
                EndDate = f.EndDate?.ToString("dd/MM/yyyy HH:mm:ss"),
            });

            dgvRevenue.DataSource = formattedOrderList.ToList();


        }
        private void btnRefreshOrder_Click(object sender, EventArgs e)
        {
            FetchingOrder();
        }


        #endregion

    }
}
