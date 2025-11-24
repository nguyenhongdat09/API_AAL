namespace API_Migrate
{
    partial class FrmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            lstToken = new ListView();
            clToken = new ColumnHeader();
            lstID = new ListView();
            TransactionID = new ColumnHeader();
            lblTitle = new Label();
            notifyIcon1 = new NotifyIcon(components);
            SuspendLayout();
            // 
            // lstToken
            // 
            lstToken.Columns.AddRange(new ColumnHeader[] { clToken });
            lstToken.Location = new Point(25, 12);
            lstToken.Name = "lstToken";
            lstToken.Size = new Size(180, 245);
            lstToken.TabIndex = 0;
            lstToken.UseCompatibleStateImageBehavior = false;
            lstToken.View = View.Details;
            // 
            // clToken
            // 
            clToken.Text = "Token";
            // 
            // lstID
            // 
            lstID.Columns.AddRange(new ColumnHeader[] { TransactionID });
            lstID.Location = new Point(314, 12);
            lstID.Name = "lstID";
            lstID.Size = new Size(214, 245);
            lstID.TabIndex = 0;
            lstID.UseCompatibleStateImageBehavior = false;
            lstID.View = View.Details;
            // 
            // TransactionID
            // 
            TransactionID.Text = "TransactionID";
            TransactionID.Width = 200;
            // 
            // lblTitle
            // 
            lblTitle.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblTitle.AutoSize = true;
            lblTitle.BackColor = Color.WhiteSmoke;
            lblTitle.Font = new Font("Segoe UI", 40F, FontStyle.Regular, GraphicsUnit.Point);
            lblTitle.Location = new Point(163, 82);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(188, 72);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "lblTitle";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // notifyIcon1
            // 
            notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
            notifyIcon1.Text = "notifyIcon1";
            notifyIcon1.Visible = true;
            notifyIcon1.MouseDoubleClick += notifyIcon1_MouseDoubleClick;
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(549, 286);
            Controls.Add(lblTitle);
            Controls.Add(lstID);
            Controls.Add(lstToken);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FrmMain";
            Text = "API";
            Load += FrmMain_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListView lstToken;
        private ListView lstID;
        private ColumnHeader TransactionID;
        private ColumnHeader clToken;
        public ColumnHeader columnHeader1;
        private Label lblTitle;
        private NotifyIcon notifyIcon1;
    }
}