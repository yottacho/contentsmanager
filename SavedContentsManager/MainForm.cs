using SavedContentsManager.utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SavedContentsManager
{
    public partial class SavedContentsManager : Form
    {
        private DirectoryToDataTable contentsDirectory;
        BackgroundWorker bWorker;

        public SavedContentsManager()
        {
            InitializeComponent();
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null, dataGridTitles, new object[] { true });

            // 타이틀에 버전명 추가
            this.Text += " - " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            // DB 백업
            DateTime dateTime = DateTime.Now;
            if (!dateTime.ToString("yyyy-MM-dd").Equals(Configure.LastDate))
            {
                Configure.LastDate = dateTime.ToString("yyyy-MM-dd");

                string dbFile = DataBaseUtils.getDatabaseFilePath();
                File.Copy(dbFile, dbFile + ".bak", true);
            }

            Application.Idle += Initialize;
        }

        /// <summary>
        /// 폼 로드 이후에 실행할 초기화
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Initialize(object sender, EventArgs e)
        {
            Application.Idle -= Initialize;

            comboContentsFolder.Items.Clear();
            comboContentsFolder.Items.AddRange((object[]) Configure.getContentsHistory());
            comboContentsFolder.Text = Configure.LastSelectedContentsFolder;

            tabControl1.SelectedIndex = 0;
            txtSearch.Focus();

            //dataGridTitles_Init();
        }

        private void comboContentsFolder_SelectedIndexChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("선택한 폴더명: " + comboContentsFolder.Text);

            //comboContentsFolder.Text = folderBrowserDialog.SelectedPath;
            Configure.LastSelectedContentsFolder = comboContentsFolder.Text;

            dataGridTitles_Init();
        }

        /// <summary>
        /// 데이터그리드 초기화
        /// </summary>
        private void dataGridTitles_Init()
        {
            if (comboContentsFolder.Text.Length > 0)
            {
                txtSearch.Text = "";

                try
                {
                    // 시간을 끄는 작업
                    contentsDirectory = new DirectoryToDataTable(comboContentsFolder.Text);
                }
                catch (Exception e)
                {
                    MessageBox.Show("디렉터리에 접근할 수 없습니다.\n" + e.Message);
                    Trace.WriteLine(e.StackTrace);
                    contentsDirectory = null;
                    return;
                }

                dataGridTitles.DataSource = contentsDirectory.DirectoryInfoView;
                for (int i = 0; i < dataGridTitles.Columns.Count; i++)
                {
                    dataGridTitles.Columns[i].HeaderText = contentsDirectory.DirectoryInfoView.Table.Columns[dataGridTitles.Columns[i].Name].Caption;
                }

                dataGridTitles.Font = new Font("굴림", 9.0f);

                dataGridTitles.Columns["SUB_FOLDERS"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridTitles.AutoResizeColumns();

                // 캐시 파일이 있으면 변경분만 갱신하고 캐시 파일이 없으면 백그라운드에서 전체를 갱신 처리
                if (contentsDirectory.DirectoryInfoView.Count > 0)
                    btnRefresh_Click(null, null);
                else
                    btnRefreshAll_Click(null, null);
            }
        }

        /// <summary>
        /// 디렉터리 변경 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (comboContentsFolder.Text.Length > 0)
            {
                folderBrowserDialog.SelectedPath = comboContentsFolder.Text;
            }

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                comboContentsFolder.Text = folderBrowserDialog.SelectedPath;
                Configure.LastSelectedContentsFolder = folderBrowserDialog.SelectedPath;

                comboContentsFolder.Items.Clear();
                comboContentsFolder.Items.AddRange(Configure.getContentsHistory());

                dataGridTitles_Init();
            }
        }

        /// <summary>
        /// 콤보박스에 디렉터리 직접 입력
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboContentsFolder_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                Configure.LastSelectedContentsFolder = comboContentsFolder.Text;

                comboContentsFolder.Items.Clear();
                comboContentsFolder.Items.AddRange(Configure.getContentsHistory());

                dataGridTitles_Init();
                e.Handled = true;
            }
        }

        /// <summary>
        /// 설정한 디렉터리 열기 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (comboContentsFolder.Text.Length > 0)
            {
                System.Diagnostics.Process.Start("explorer.exe", "\"" + comboContentsFolder.Text + "\"");
            }
        }

        /// <summary>
        /// 텍스트박스 실시간 검색
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (contentsDirectory != null)
            {
                string srchStr = txtSearch.Text.Replace("'", "''");
                contentsDirectory.DirectoryInfoView.RowFilter = "[TITLE_NAME] LIKE '%" + srchStr + "%'";
            }
        }

        /// <summary>
        /// 텍스트박스 키캡쳐
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                txtSearch.Text = "";
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                // 검색결과 첫번째 항목으로 이동
                dataGridTitles_CellDoubleClick(null, null);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            {
                dataGridTitles.Focus();
                e.Handled = false;
            }
        }

        /// <summary>
        /// 캐시 전체 새로 고침
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefreshAll_Click(object sender, EventArgs e)
        {

            if (bWorker != null && bWorker.IsBusy)
            {
                MessageBox.Show("리프레시 작업이 진행중입니다.");
                return;
            }

            if (contentsDirectory == null)
            {
                dataGridTitles_Init();
            }
            else
            {
                txtSearch.Text = "";
                progressBar1.Value = 0;
                dataGridTitles.DataSource = null;

                if (bWorker == null)
                {
                    bWorker = new BackgroundWorker();

                    bWorker.DoWork += refreshProcess;
                    bWorker.ProgressChanged += updateProgressBar;
                    bWorker.RunWorkerCompleted += refreshProcessCompleted;
                }

                contentsDirectory.reportProgressMethod = bWorker.ReportProgress;
                bWorker.WorkerReportsProgress = true;
                bWorker.RunWorkerAsync();
            }

        }

        public void refreshProcess(object sender, DoWorkEventArgs e)
        {
            contentsDirectory.Refresh();
        }

        public void updateProgressBar(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        public void refreshProcessCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            contentsDirectory.reportProgressMethod = null;
            progressBar1.Value = 100;

            dataGridTitles.DataSource = contentsDirectory.DirectoryInfoView;
            for (int i = 0; i < dataGridTitles.Columns.Count; i++)
            {
                dataGridTitles.Columns[i].HeaderText = contentsDirectory.DirectoryInfoView.Table.Columns[dataGridTitles.Columns[i].HeaderText].Caption;
            }

            dataGridTitles.Columns["SUB_FOLDERS"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dataGridTitles.AutoResizeColumns();
            dataGridTitles.FirstDisplayedScrollingRowIndex = 0;
            dataGridTitles.FirstDisplayedScrollingColumnIndex = 0;
            if (dataGridTitles.Rows.Count > 0)
                dataGridTitles.CurrentCell = dataGridTitles.Rows[0].Cells[0];

        }

        /// <summary>
        /// 변경내역만 다시 로드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (contentsDirectory == null)
            {
                dataGridTitles_Init();
            }
            else
            {
                txtSearch.Text = "";
                dataGridTitles.ClearSelection();

                try
                {
                    contentsDirectory.DifferentCheck();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                dataGridTitles.AutoResizeColumns();
                dataGridTitles.FirstDisplayedScrollingRowIndex = 0;
                dataGridTitles.FirstDisplayedScrollingColumnIndex = 0;
                if (dataGridTitles.Rows.Count > 0)
                    dataGridTitles.CurrentCell = dataGridTitles.Rows[0].Cells[0];

            }
            this.Cursor = this.DefaultCursor;
        }

        /// <summary>
        /// 목록 더블클릭하면 상세보기로 이동
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridTitles_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string title = "";
            if (e != null && e.RowIndex >= 0)
                title = dataGridTitles.Rows[e.RowIndex].Cells["TITLE_NAME"].Value.ToString();

            // 제목 검색 후 엔터키 쳤을 경우... 첫번째 항목
            if (sender == null && e == null)
            {
                if (dataGridTitles.SelectedRows.Count > 0)
                    title = dataGridTitles.SelectedRows[0].Cells["TITLE_NAME"].Value.ToString();
                else if (dataGridTitles.Rows.Count > 0)
                    title = dataGridTitles.Rows[0].Cells["TITLE_NAME"].Value.ToString();
            }

            if (title.Length > 0)
            {
                txtTitle.Text = title;
                // 탭 변경
                tabControl1.SelectedIndex = 1;
            }
        }

        /// <summary>
        /// 그리드에서 Enter 키 눌렀을 때 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridTitles_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string title = dataGridTitles.SelectedRows[0].Cells["TITLE_NAME"].Value.ToString();
                txtTitle.Text = title;

                // 탭 변경
                tabControl1.SelectedIndex = 1;
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                txtSearch.Focus();
                e.Handled = true;
            }
        }

        /// <summary>
        /// 탭 페이지 변경 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            switch (e.TabPageIndex)
            {
                case 0: // 타이틀 페이지
                    // 특별히 하는 건 없음...
                    break;
                case 1: // 세부 페이지
                    listDetail_Init();
                    break;
            }
        }

        /// <summary>
        /// 자료폴더 관리 다이얼로그 생성
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnManage_Click(object sender, EventArgs e)
        {
            new MoveForm(comboContentsFolder.Text).ShowDialog();

            // 변경내역을 적용하기 위해 데이터그리드를 리로드한다
            btnRefresh_Click(sender, e);
        }

    }
}
