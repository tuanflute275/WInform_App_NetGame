namespace BigProject.Forms
{
    partial class FrmDeposit
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.txtUserNameDeposit = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnOkFormChangePass = new System.Windows.Forms.Button();
            this.btnCloseFormChangePass = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtMoney = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Controls.Add(this.btnOkFormChangePass);
            this.panel1.Controls.Add(this.btnCloseFormChangePass);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(465, 259);
            this.panel1.TabIndex = 2;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.txtUserNameDeposit);
            this.panel5.Controls.Add(this.label4);
            this.panel5.Location = new System.Drawing.Point(7, 18);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(443, 49);
            this.panel5.TabIndex = 2;
            // 
            // txtUserNameDeposit
            // 
            this.txtUserNameDeposit.Location = new System.Drawing.Point(184, 17);
            this.txtUserNameDeposit.Name = "txtUserNameDeposit";
            this.txtUserNameDeposit.ReadOnly = true;
            this.txtUserNameDeposit.Size = new System.Drawing.Size(256, 22);
            this.txtUserNameDeposit.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 24);
            this.label4.TabIndex = 0;
            this.label4.Text = "UserName";
            // 
            // btnOkFormChangePass
            // 
            this.btnOkFormChangePass.Location = new System.Drawing.Point(256, 180);
            this.btnOkFormChangePass.Name = "btnOkFormChangePass";
            this.btnOkFormChangePass.Size = new System.Drawing.Size(98, 32);
            this.btnOkFormChangePass.TabIndex = 2;
            this.btnOkFormChangePass.Text = "Deposit";
            this.btnOkFormChangePass.UseVisualStyleBackColor = true;
            this.btnOkFormChangePass.Click += new System.EventHandler(this.btnOkFormChangePass_Click);
            // 
            // btnCloseFormChangePass
            // 
            this.btnCloseFormChangePass.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCloseFormChangePass.Location = new System.Drawing.Point(360, 180);
            this.btnCloseFormChangePass.Name = "btnCloseFormChangePass";
            this.btnCloseFormChangePass.Size = new System.Drawing.Size(90, 32);
            this.btnCloseFormChangePass.TabIndex = 3;
            this.btnCloseFormChangePass.Text = "Close";
            this.btnCloseFormChangePass.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txtMoney);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Location = new System.Drawing.Point(7, 92);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(443, 49);
            this.panel2.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Money";
            // 
            // txtMoney
            // 
            this.txtMoney.Location = new System.Drawing.Point(184, 17);
            this.txtMoney.Name = "txtMoney";
            this.txtMoney.Size = new System.Drawing.Size(256, 22);
            this.txtMoney.TabIndex = 1;
            // 
            // FrmDeposit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(486, 280);
            this.Controls.Add(this.panel1);
            this.Name = "FrmDeposit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Deposit money into account";
            this.Load += new System.EventHandler(this.FrmDeposit_Load);
            this.panel1.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.TextBox txtUserNameDeposit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnOkFormChangePass;
        private System.Windows.Forms.Button btnCloseFormChangePass;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMoney;
    }
}