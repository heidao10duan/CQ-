namespace CQCMWin
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.txtDb = new System.Windows.Forms.TextBox();
            this.lblDb = new System.Windows.Forms.Label();
            this.lblSite = new System.Windows.Forms.Label();
            this.txtSite = new System.Windows.Forms.TextBox();
            this.btnDb = new System.Windows.Forms.Button();
            this.btnSite = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.richTxt = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // txtDb
            // 
            this.txtDb.Location = new System.Drawing.Point(206, 59);
            this.txtDb.Name = "txtDb";
            this.txtDb.Size = new System.Drawing.Size(394, 28);
            this.txtDb.TabIndex = 0;
            // 
            // lblDb
            // 
            this.lblDb.AutoSize = true;
            this.lblDb.Location = new System.Drawing.Point(57, 62);
            this.lblDb.Name = "lblDb";
            this.lblDb.Size = new System.Drawing.Size(143, 18);
            this.lblDb.TabIndex = 1;
            this.lblDb.Text = "database input:";
            // 
            // lblSite
            // 
            this.lblSite.AutoSize = true;
            this.lblSite.Location = new System.Drawing.Point(57, 148);
            this.lblSite.Name = "lblSite";
            this.lblSite.Size = new System.Drawing.Size(89, 18);
            this.lblSite.TabIndex = 3;
            this.lblSite.Text = "扩容站点:";
            // 
            // txtSite
            // 
            this.txtSite.Location = new System.Drawing.Point(206, 138);
            this.txtSite.Name = "txtSite";
            this.txtSite.Size = new System.Drawing.Size(394, 28);
            this.txtSite.TabIndex = 2;
            // 
            // btnDb
            // 
            this.btnDb.Location = new System.Drawing.Point(645, 59);
            this.btnDb.Name = "btnDb";
            this.btnDb.Size = new System.Drawing.Size(97, 28);
            this.btnDb.TabIndex = 6;
            this.btnDb.Text = "...";
            this.btnDb.UseVisualStyleBackColor = true;
            this.btnDb.Click += new System.EventHandler(this.btnDb_Click);
            // 
            // btnSite
            // 
            this.btnSite.Location = new System.Drawing.Point(645, 148);
            this.btnSite.Name = "btnSite";
            this.btnSite.Size = new System.Drawing.Size(97, 28);
            this.btnSite.TabIndex = 8;
            this.btnSite.Text = "...";
            this.btnSite.UseVisualStyleBackColor = true;
            this.btnSite.Click += new System.EventHandler(this.btnSite_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(192, 573);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(86, 36);
            this.btnOk.TabIndex = 9;
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(489, 573);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(89, 36);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // richTxt
            // 
            this.richTxt.Location = new System.Drawing.Point(60, 232);
            this.richTxt.Name = "richTxt";
            this.richTxt.Size = new System.Drawing.Size(682, 296);
            this.richTxt.TabIndex = 11;
            this.richTxt.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(821, 680);
            this.Controls.Add(this.richTxt);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnSite);
            this.Controls.Add(this.btnDb);
            this.Controls.Add(this.lblSite);
            this.Controls.Add(this.txtSite);
            this.Controls.Add(this.lblDb);
            this.Controls.Add(this.txtDb);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "重庆扩容";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtDb;
        private System.Windows.Forms.Label lblDb;
        private System.Windows.Forms.Label lblSite;
        private System.Windows.Forms.TextBox txtSite;
        private System.Windows.Forms.Button btnDb;
        private System.Windows.Forms.Button btnSite;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.RichTextBox richTxt;
    }
}

