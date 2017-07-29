namespace SavedContentsManager
{
    partial class MoveForm
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
            this.btnSourceOpen = new System.Windows.Forms.Button();
            this.btnSourceBrowse = new System.Windows.Forms.Button();
            this.comboSourceFolder = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnTargetOpen = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.listSource = new System.Windows.Forms.ListView();
            this.listSourceDetail = new System.Windows.Forms.ListView();
            this.listTarget = new System.Windows.Forms.ListView();
            this.listTargetDetail = new System.Windows.Forms.ListView();
            this.listTargetTodo = new System.Windows.Forms.ListView();
            this.textTargetSearch = new System.Windows.Forms.TextBox();
            this.btnProcessAll = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textTarget = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnSourceSelOpen = new System.Windows.Forms.Button();
            this.btnTargetSelOpen = new System.Windows.Forms.Button();
            this.btnRefreshSource = new System.Windows.Forms.Button();
            this.textTargetName = new System.Windows.Forms.TextBox();
            this.btnSelUp = new System.Windows.Forms.Button();
            this.btnSelDown = new System.Windows.Forms.Button();
            this.labelDesc1 = new System.Windows.Forms.Label();
            this.labelDesc2 = new System.Windows.Forms.Label();
            this.labelDesc3 = new System.Windows.Forms.Label();
            this.labelDesc4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnSourceOpen
            // 
            this.btnSourceOpen.Location = new System.Drawing.Point(286, 13);
            this.btnSourceOpen.Name = "btnSourceOpen";
            this.btnSourceOpen.Size = new System.Drawing.Size(75, 23);
            this.btnSourceOpen.TabIndex = 2;
            this.btnSourceOpen.Text = "Open";
            this.btnSourceOpen.UseVisualStyleBackColor = true;
            this.btnSourceOpen.Click += new System.EventHandler(this.btnSourceOpen_Click);
            // 
            // btnSourceBrowse
            // 
            this.btnSourceBrowse.Location = new System.Drawing.Point(367, 13);
            this.btnSourceBrowse.Name = "btnSourceBrowse";
            this.btnSourceBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnSourceBrowse.TabIndex = 3;
            this.btnSourceBrowse.Text = "Browse...";
            this.btnSourceBrowse.UseVisualStyleBackColor = true;
            this.btnSourceBrowse.Click += new System.EventHandler(this.btnSourceBrowse_Click);
            // 
            // comboSourceFolder
            // 
            this.comboSourceFolder.FormattingEnabled = true;
            this.comboSourceFolder.Location = new System.Drawing.Point(63, 15);
            this.comboSourceFolder.Name = "comboSourceFolder";
            this.comboSourceFolder.Size = new System.Drawing.Size(217, 20);
            this.comboSourceFolder.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Source";
            // 
            // btnTargetOpen
            // 
            this.btnTargetOpen.Location = new System.Drawing.Point(802, 13);
            this.btnTargetOpen.Name = "btnTargetOpen";
            this.btnTargetOpen.Size = new System.Drawing.Size(75, 23);
            this.btnTargetOpen.TabIndex = 8;
            this.btnTargetOpen.Text = "Open";
            this.btnTargetOpen.UseVisualStyleBackColor = true;
            this.btnTargetOpen.Click += new System.EventHandler(this.btnTargetOpen_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(528, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "Target";
            // 
            // listSource
            // 
            this.listSource.FullRowSelect = true;
            this.listSource.GridLines = true;
            this.listSource.HideSelection = false;
            this.listSource.Location = new System.Drawing.Point(14, 68);
            this.listSource.MultiSelect = false;
            this.listSource.Name = "listSource";
            this.listSource.Size = new System.Drawing.Size(428, 226);
            this.listSource.TabIndex = 4;
            this.listSource.UseCompatibleStateImageBehavior = false;
            this.listSource.View = System.Windows.Forms.View.Details;
            this.listSource.SelectedIndexChanged += new System.EventHandler(this.listSource_SelectedIndexChanged);
            this.listSource.DoubleClick += new System.EventHandler(this.listSource_DoubleClick);
            // 
            // listSourceDetail
            // 
            this.listSourceDetail.FullRowSelect = true;
            this.listSourceDetail.GridLines = true;
            this.listSourceDetail.HideSelection = false;
            this.listSourceDetail.Location = new System.Drawing.Point(15, 300);
            this.listSourceDetail.MultiSelect = false;
            this.listSourceDetail.Name = "listSourceDetail";
            this.listSourceDetail.Size = new System.Drawing.Size(428, 111);
            this.listSourceDetail.TabIndex = 5;
            this.listSourceDetail.UseCompatibleStateImageBehavior = false;
            this.listSourceDetail.View = System.Windows.Forms.View.Details;
            this.listSourceDetail.DoubleClick += new System.EventHandler(this.listSourceDetail_DoubleClick);
            // 
            // listTarget
            // 
            this.listTarget.FullRowSelect = true;
            this.listTarget.GridLines = true;
            this.listTarget.HideSelection = false;
            this.listTarget.Location = new System.Drawing.Point(530, 68);
            this.listTarget.MultiSelect = false;
            this.listTarget.Name = "listTarget";
            this.listTarget.Size = new System.Drawing.Size(428, 155);
            this.listTarget.TabIndex = 11;
            this.listTarget.UseCompatibleStateImageBehavior = false;
            this.listTarget.View = System.Windows.Forms.View.Details;
            this.listTarget.SelectedIndexChanged += new System.EventHandler(this.listTarget_SelectedIndexChanged);
            // 
            // listTargetDetail
            // 
            this.listTargetDetail.FullRowSelect = true;
            this.listTargetDetail.GridLines = true;
            this.listTargetDetail.HideSelection = false;
            this.listTargetDetail.Location = new System.Drawing.Point(530, 229);
            this.listTargetDetail.MultiSelect = false;
            this.listTargetDetail.Name = "listTargetDetail";
            this.listTargetDetail.Size = new System.Drawing.Size(428, 99);
            this.listTargetDetail.TabIndex = 12;
            this.listTargetDetail.UseCompatibleStateImageBehavior = false;
            this.listTargetDetail.View = System.Windows.Forms.View.Details;
            this.listTargetDetail.DoubleClick += new System.EventHandler(this.listTargetDetail_DoubleClick);
            // 
            // listTargetTodo
            // 
            this.listTargetTodo.FullRowSelect = true;
            this.listTargetTodo.GridLines = true;
            this.listTargetTodo.HideSelection = false;
            this.listTargetTodo.Location = new System.Drawing.Point(530, 334);
            this.listTargetTodo.MultiSelect = false;
            this.listTargetTodo.Name = "listTargetTodo";
            this.listTargetTodo.Size = new System.Drawing.Size(428, 77);
            this.listTargetTodo.TabIndex = 13;
            this.listTargetTodo.UseCompatibleStateImageBehavior = false;
            this.listTargetTodo.View = System.Windows.Forms.View.Details;
            // 
            // textTargetSearch
            // 
            this.textTargetSearch.Location = new System.Drawing.Point(580, 41);
            this.textTargetSearch.Name = "textTargetSearch";
            this.textTargetSearch.Size = new System.Drawing.Size(216, 21);
            this.textTargetSearch.TabIndex = 10;
            this.textTargetSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textTargetSearch_KeyPress);
            // 
            // btnProcessAll
            // 
            this.btnProcessAll.Location = new System.Drawing.Point(449, 300);
            this.btnProcessAll.Name = "btnProcessAll";
            this.btnProcessAll.Size = new System.Drawing.Size(75, 40);
            this.btnProcessAll.TabIndex = 19;
            this.btnProcessAll.Text = "Move All ->";
            this.btnProcessAll.UseVisualStyleBackColor = true;
            this.btnProcessAll.Click += new System.EventHandler(this.btnProcessAll_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(529, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "Search";
            // 
            // textTarget
            // 
            this.textTarget.Location = new System.Drawing.Point(580, 14);
            this.textTarget.Name = "textTarget";
            this.textTarget.ReadOnly = true;
            this.textTarget.Size = new System.Drawing.Size(216, 21);
            this.textTarget.TabIndex = 7;
            // 
            // btnUp
            // 
            this.btnUp.Location = new System.Drawing.Point(140, 417);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(75, 23);
            this.btnUp.TabIndex = 20;
            this.btnUp.Text = "Up";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDown
            // 
            this.btnDown.Location = new System.Drawing.Point(239, 417);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(75, 23);
            this.btnDown.TabIndex = 21;
            this.btnDown.Text = "Down";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(530, 417);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 23);
            this.btnNew.TabIndex = 22;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(367, 417);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 23;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(532, 446);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            this.txtStatus.Size = new System.Drawing.Size(427, 21);
            this.txtStatus.TabIndex = 24;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(15, 446);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(427, 21);
            this.progressBar1.TabIndex = 25;
            // 
            // btnSourceSelOpen
            // 
            this.btnSourceSelOpen.Location = new System.Drawing.Point(367, 41);
            this.btnSourceSelOpen.Name = "btnSourceSelOpen";
            this.btnSourceSelOpen.Size = new System.Drawing.Size(75, 23);
            this.btnSourceSelOpen.TabIndex = 26;
            this.btnSourceSelOpen.Text = "Open Sel";
            this.btnSourceSelOpen.UseVisualStyleBackColor = true;
            this.btnSourceSelOpen.Click += new System.EventHandler(this.btnSourceSelOpen_Click);
            // 
            // btnTargetSelOpen
            // 
            this.btnTargetSelOpen.Location = new System.Drawing.Point(802, 40);
            this.btnTargetSelOpen.Name = "btnTargetSelOpen";
            this.btnTargetSelOpen.Size = new System.Drawing.Size(75, 23);
            this.btnTargetSelOpen.TabIndex = 27;
            this.btnTargetSelOpen.Text = "Open Sel";
            this.btnTargetSelOpen.UseVisualStyleBackColor = true;
            this.btnTargetSelOpen.Click += new System.EventHandler(this.btnTargetSelOpen_Click);
            // 
            // btnRefreshSource
            // 
            this.btnRefreshSource.Location = new System.Drawing.Point(286, 41);
            this.btnRefreshSource.Name = "btnRefreshSource";
            this.btnRefreshSource.Size = new System.Drawing.Size(75, 23);
            this.btnRefreshSource.TabIndex = 28;
            this.btnRefreshSource.Text = "Refresh";
            this.btnRefreshSource.UseVisualStyleBackColor = true;
            this.btnRefreshSource.Click += new System.EventHandler(this.btnRefreshSource_Click);
            // 
            // textTargetName
            // 
            this.textTargetName.Location = new System.Drawing.Point(611, 417);
            this.textTargetName.Name = "textTargetName";
            this.textTargetName.Size = new System.Drawing.Size(347, 21);
            this.textTargetName.TabIndex = 29;
            // 
            // btnSelUp
            // 
            this.btnSelUp.Location = new System.Drawing.Point(451, 146);
            this.btnSelUp.Name = "btnSelUp";
            this.btnSelUp.Size = new System.Drawing.Size(24, 23);
            this.btnSelUp.TabIndex = 30;
            this.btnSelUp.Text = "△";
            this.btnSelUp.UseVisualStyleBackColor = true;
            this.btnSelUp.Click += new System.EventHandler(this.btnSelUp_Click);
            // 
            // btnSelDown
            // 
            this.btnSelDown.Location = new System.Drawing.Point(452, 174);
            this.btnSelDown.Name = "btnSelDown";
            this.btnSelDown.Size = new System.Drawing.Size(24, 23);
            this.btnSelDown.TabIndex = 31;
            this.btnSelDown.Text = "▽";
            this.btnSelDown.UseVisualStyleBackColor = true;
            this.btnSelDown.Click += new System.EventHandler(this.btnSelDown_Click);
            // 
            // labelDesc1
            // 
            this.labelDesc1.AutoSize = true;
            this.labelDesc1.Location = new System.Drawing.Point(485, 151);
            this.labelDesc1.Name = "labelDesc1";
            this.labelDesc1.Size = new System.Drawing.Size(26, 12);
            this.labelDesc1.TabIndex = 32;
            this.labelDesc1.Text = "^Up";
            // 
            // labelDesc2
            // 
            this.labelDesc2.AutoSize = true;
            this.labelDesc2.Location = new System.Drawing.Point(484, 179);
            this.labelDesc2.Name = "labelDesc2";
            this.labelDesc2.Size = new System.Drawing.Size(43, 12);
            this.labelDesc2.TabIndex = 33;
            this.labelDesc2.Text = "^Down";
            // 
            // labelDesc3
            // 
            this.labelDesc3.AutoSize = true;
            this.labelDesc3.Location = new System.Drawing.Point(474, 343);
            this.labelDesc3.Name = "labelDesc3";
            this.labelDesc3.Size = new System.Drawing.Size(19, 12);
            this.labelDesc3.TabIndex = 34;
            this.labelDesc3.Text = "^A";
            // 
            // labelDesc4
            // 
            this.labelDesc4.AutoSize = true;
            this.labelDesc4.Location = new System.Drawing.Point(447, 423);
            this.labelDesc4.Name = "labelDesc4";
            this.labelDesc4.Size = new System.Drawing.Size(19, 12);
            this.labelDesc4.TabIndex = 35;
            this.labelDesc4.Text = "^D";
            // 
            // MoveForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(971, 480);
            this.Controls.Add(this.labelDesc4);
            this.Controls.Add(this.labelDesc3);
            this.Controls.Add(this.labelDesc2);
            this.Controls.Add(this.labelDesc1);
            this.Controls.Add(this.btnSelDown);
            this.Controls.Add(this.btnSelUp);
            this.Controls.Add(this.textTargetName);
            this.Controls.Add(this.btnRefreshSource);
            this.Controls.Add(this.btnTargetSelOpen);
            this.Controls.Add(this.btnSourceSelOpen);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.textTarget);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnProcessAll);
            this.Controls.Add(this.textTargetSearch);
            this.Controls.Add(this.listTargetTodo);
            this.Controls.Add(this.listTargetDetail);
            this.Controls.Add(this.listTarget);
            this.Controls.Add(this.listSourceDetail);
            this.Controls.Add(this.listSource);
            this.Controls.Add(this.btnTargetOpen);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSourceOpen);
            this.Controls.Add(this.btnSourceBrowse);
            this.Controls.Add(this.comboSourceFolder);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "MoveForm";
            this.Text = "Contents Manage";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MoveForm_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSourceOpen;
        private System.Windows.Forms.Button btnSourceBrowse;
        private System.Windows.Forms.ComboBox comboSourceFolder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnTargetOpen;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView listSource;
        private System.Windows.Forms.ListView listSourceDetail;
        private System.Windows.Forms.ListView listTarget;
        private System.Windows.Forms.ListView listTargetDetail;
        private System.Windows.Forms.ListView listTargetTodo;
        private System.Windows.Forms.TextBox textTargetSearch;
        private System.Windows.Forms.Button btnProcessAll;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textTarget;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btnSourceSelOpen;
        private System.Windows.Forms.Button btnTargetSelOpen;
        private System.Windows.Forms.Button btnRefreshSource;
        private System.Windows.Forms.TextBox textTargetName;
        private System.Windows.Forms.Button btnSelUp;
        private System.Windows.Forms.Button btnSelDown;
        private System.Windows.Forms.Label labelDesc1;
        private System.Windows.Forms.Label labelDesc2;
        private System.Windows.Forms.Label labelDesc3;
        private System.Windows.Forms.Label labelDesc4;
    }
}