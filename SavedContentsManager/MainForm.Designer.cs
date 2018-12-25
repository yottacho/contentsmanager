namespace SavedContentsManager
{
    partial class SavedContentsManager
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.comboContentsFolder = new System.Windows.Forms.ComboBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageFolder = new System.Windows.Forms.TabPage();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnRefreshAll = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.dataGridTitles = new System.Windows.Forms.DataGridView();
            this.tabPageDetail = new System.Windows.Forms.TabPage();
            this.btnOpenDetail = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnRemapping = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.listDetail = new System.Windows.Forms.ListView();
            this.btnManage = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPageFolder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridTitles)).BeginInit();
            this.tabPageDetail.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Stored Folder";
            // 
            // comboContentsFolder
            // 
            this.comboContentsFolder.FormattingEnabled = true;
            this.comboContentsFolder.Location = new System.Drawing.Point(98, 16);
            this.comboContentsFolder.Name = "comboContentsFolder";
            this.comboContentsFolder.Size = new System.Drawing.Size(334, 20);
            this.comboContentsFolder.TabIndex = 2;
            this.comboContentsFolder.SelectedIndexChanged += new System.EventHandler(this.comboContentsFolder_SelectedIndexChanged);
            this.comboContentsFolder.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboContentsFolder_KeyPress);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(519, 14);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 4;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(438, 14);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(75, 23);
            this.btnOpen.TabIndex = 3;
            this.btnOpen.Text = "Open";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageFolder);
            this.tabControl1.Controls.Add(this.tabPageDetail);
            this.tabControl1.Location = new System.Drawing.Point(12, 42);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(801, 483);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl1_Selected);
            // 
            // tabPageFolder
            // 
            this.tabPageFolder.Controls.Add(this.progressBar1);
            this.tabPageFolder.Controls.Add(this.btnRefresh);
            this.tabPageFolder.Controls.Add(this.btnRefreshAll);
            this.tabPageFolder.Controls.Add(this.label2);
            this.tabPageFolder.Controls.Add(this.txtSearch);
            this.tabPageFolder.Controls.Add(this.dataGridTitles);
            this.tabPageFolder.Location = new System.Drawing.Point(4, 22);
            this.tabPageFolder.Name = "tabPageFolder";
            this.tabPageFolder.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFolder.Size = new System.Drawing.Size(793, 457);
            this.tabPageFolder.TabIndex = 0;
            this.tabPageFolder.Text = "Titles";
            this.tabPageFolder.UseVisualStyleBackColor = true;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(523, 10);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(100, 16);
            this.progressBar1.TabIndex = 6;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(629, 6);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnRefreshAll
            // 
            this.btnRefreshAll.Location = new System.Drawing.Point(710, 6);
            this.btnRefreshAll.Name = "btnRefreshAll";
            this.btnRefreshAll.Size = new System.Drawing.Size(75, 23);
            this.btnRefreshAll.TabIndex = 5;
            this.btnRefreshAll.Text = "Refresh All";
            this.btnRefreshAll.UseVisualStyleBackColor = true;
            this.btnRefreshAll.Click += new System.EventHandler(this.btnRefreshAll_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "Title Search";
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(82, 6);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(167, 21);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // dataGridTitles
            // 
            this.dataGridTitles.AllowUserToAddRows = false;
            this.dataGridTitles.AllowUserToDeleteRows = false;
            this.dataGridTitles.AllowUserToOrderColumns = true;
            this.dataGridTitles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridTitles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridTitles.Location = new System.Drawing.Point(3, 33);
            this.dataGridTitles.MultiSelect = false;
            this.dataGridTitles.Name = "dataGridTitles";
            this.dataGridTitles.ReadOnly = true;
            this.dataGridTitles.RowTemplate.Height = 23;
            this.dataGridTitles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridTitles.Size = new System.Drawing.Size(782, 418);
            this.dataGridTitles.TabIndex = 3;
            this.dataGridTitles.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridTitles_CellDoubleClick);
            this.dataGridTitles.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridTitles_KeyDown);
            // 
            // tabPageDetail
            // 
            this.tabPageDetail.Controls.Add(this.btnOpenDetail);
            this.tabPageDetail.Controls.Add(this.btnCancel);
            this.tabPageDetail.Controls.Add(this.btnSave);
            this.tabPageDetail.Controls.Add(this.btnRemapping);
            this.tabPageDetail.Controls.Add(this.btnDown);
            this.tabPageDetail.Controls.Add(this.btnUp);
            this.tabPageDetail.Controls.Add(this.label3);
            this.tabPageDetail.Controls.Add(this.txtTitle);
            this.tabPageDetail.Controls.Add(this.listDetail);
            this.tabPageDetail.Location = new System.Drawing.Point(4, 22);
            this.tabPageDetail.Name = "tabPageDetail";
            this.tabPageDetail.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDetail.Size = new System.Drawing.Size(793, 457);
            this.tabPageDetail.TabIndex = 1;
            this.tabPageDetail.Text = "Details";
            this.tabPageDetail.UseVisualStyleBackColor = true;
            // 
            // btnOpenDetail
            // 
            this.btnOpenDetail.Location = new System.Drawing.Point(422, 13);
            this.btnOpenDetail.Name = "btnOpenDetail";
            this.btnOpenDetail.Size = new System.Drawing.Size(75, 23);
            this.btnOpenDetail.TabIndex = 8;
            this.btnOpenDetail.Text = "Open";
            this.btnOpenDetail.UseVisualStyleBackColor = true;
            this.btnOpenDetail.Click += new System.EventHandler(this.btnOpenDetail_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(712, 428);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(596, 428);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnRemapping
            // 
            this.btnRemapping.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemapping.Location = new System.Drawing.Point(515, 428);
            this.btnRemapping.Name = "btnRemapping";
            this.btnRemapping.Size = new System.Drawing.Size(75, 23);
            this.btnRemapping.TabIndex = 5;
            this.btnRemapping.Text = "Remap";
            this.btnRemapping.UseVisualStyleBackColor = true;
            this.btnRemapping.Click += new System.EventHandler(this.btnRemapping_Click);
            // 
            // btnDown
            // 
            this.btnDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDown.Location = new System.Drawing.Point(408, 428);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(75, 23);
            this.btnDown.TabIndex = 4;
            this.btnDown.Text = "Down";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnUp
            // 
            this.btnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUp.Location = new System.Drawing.Point(312, 428);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(75, 23);
            this.btnUp.TabIndex = 3;
            this.btnUp.Text = "Up";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "Title";
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(41, 15);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.ReadOnly = true;
            this.txtTitle.Size = new System.Drawing.Size(375, 21);
            this.txtTitle.TabIndex = 1;
            // 
            // listDetail
            // 
            this.listDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listDetail.FullRowSelect = true;
            this.listDetail.GridLines = true;
            this.listDetail.HideSelection = false;
            this.listDetail.Location = new System.Drawing.Point(6, 42);
            this.listDetail.MultiSelect = false;
            this.listDetail.Name = "listDetail";
            this.listDetail.Size = new System.Drawing.Size(781, 380);
            this.listDetail.TabIndex = 0;
            this.listDetail.UseCompatibleStateImageBehavior = false;
            this.listDetail.View = System.Windows.Forms.View.Details;
            this.listDetail.DoubleClick += new System.EventHandler(this.listDetail_DoubleClick);
            this.listDetail.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listDetail_KeyDown);
            // 
            // btnManage
            // 
            this.btnManage.Location = new System.Drawing.Point(716, 14);
            this.btnManage.Name = "btnManage";
            this.btnManage.Size = new System.Drawing.Size(85, 23);
            this.btnManage.TabIndex = 5;
            this.btnManage.Text = "Merge Tool";
            this.btnManage.UseVisualStyleBackColor = true;
            this.btnManage.Click += new System.EventHandler(this.btnManage_Click);
            // 
            // SavedContentsManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(825, 537);
            this.Controls.Add(this.btnManage);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.comboContentsFolder);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.Name = "SavedContentsManager";
            this.Text = "Saved Contents Manager";
            this.tabControl1.ResumeLayout(false);
            this.tabPageFolder.ResumeLayout(false);
            this.tabPageFolder.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridTitles)).EndInit();
            this.tabPageDetail.ResumeLayout(false);
            this.tabPageDetail.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboContentsFolder;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageFolder;
        private System.Windows.Forms.TabPage tabPageDetail;
        private System.Windows.Forms.DataGridView dataGridTitles;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.ListView listDetail;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Button btnRefreshAll;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnRemapping;
        private System.Windows.Forms.Button btnOpenDetail;
        private System.Windows.Forms.Button btnManage;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}

