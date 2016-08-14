using SavedContentsManager.utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace SavedContentsManager
{
    /// <summary>
    /// 메인 폼 / 상세 탭 페이지 코드
    /// </summary>
    partial class SavedContentsManager
    {
        /// <summary>
        /// listDetail 의 데이터소스
        /// </summary>
        private DetailDirectory detailDir;

        private string fullPath;

        /// <summary>
        /// 상세페이지 초기화
        /// </summary>
        private void listDetail_Init()
        {
            if (txtTitle.Text.Length == 0)
                return;

            listDetail.Columns.Clear();
            ColumnHeader colHeader = new ColumnHeader
            {
                Text = "No.",
                Width = 5 * 15
            };
            listDetail.Columns.Add(colHeader);
            colHeader = new ColumnHeader
            {
                Text = "Title",
                Width = (txtTitle.Text.Length + 5) * 15
            };
            listDetail.Columns.Add(colHeader);
            colHeader = new ColumnHeader
            {
                Text = "Date",
                Width = 10 * 15
            };
            listDetail.Columns.Add(colHeader);
            colHeader = new ColumnHeader
            {
                Text = "Rename",
                Width = (txtTitle.Text.Length + 5) * 15
            };
            listDetail.Columns.Add(colHeader);

            fullPath = Configure.LastSelectedContentsFolder + Path.DirectorySeparatorChar + txtTitle.Text;
            detailDir_Init();

            listDetail_Refresh();
            if (listDetail.Items.Count > 0)
                listDetail.SelectedIndices.Add(0);
        }

        /// <summary>
        /// 리스트 초기화
        /// </summary>
        public void detailDir_Init()
        {
            // 리스트 초기화
            if (txtTitle.Text.Length == 0)
                return;

            detailDir = new DetailDirectory(fullPath);
        }


        /// <summary>
        /// Open 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenDetail_Click(object sender, EventArgs e)
        {
            if (txtTitle.Text.Length > 0)
            {
                System.Diagnostics.Process.Start("explorer.exe", "\"" + fullPath + "\"");
            }
        }

        /// <summary>
        /// 업 버튼 클릭했을 때
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUp_Click(object sender, EventArgs e)
        {
            if (detailDir == null)
                return;

            if (listDetail.SelectedIndices.Count == 0)
                return;

            int idx = listDetail.SelectedIndices[0];
            if (idx <= 0)
                return;

            detailDir.moveUp(idx);

            listDetail_Refresh();
            idx--;
            listDetail.SelectedIndices.Add(idx);
        }

        /// <summary>
        /// 다운 버튼 클릭했을 때
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDown_Click(object sender, EventArgs e)
        {
            if (detailDir == null)
                return;

            if (listDetail.SelectedIndices.Count == 0)
                return;

            int idx = listDetail.SelectedIndices[0];
            if ((idx + 1) >= listDetail.Items.Count)
                return;

            ListViewItem row = listDetail.SelectedItems[0];
            Console.WriteLine("idx(" + idx + ")" + row.Text + "// [" + row.Name + "]");

            detailDir.moveDown(idx);

            listDetail_Refresh();
            idx++;
            listDetail.SelectedIndices.Add(idx);
        }


        /// <summary>
        /// 리스트에서 항목 더블클릭했을 때 액션
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listDetail_DoubleClick(object sender, EventArgs e)
        {
            if (detailDir == null)
                return;

            if (listDetail.SelectedItems.Count == 0)
                return;

            ListViewItem row = listDetail.SelectedItems[0];
            string selectedName = row.Name;
            Console.WriteLine("Full name [" + row.Name + "]");
            string fullPathName = fullPath + Path.DirectorySeparatorChar + selectedName;

            FileInfo[] files = new DirectoryInfo(fullPathName).GetFiles();
            Array.Sort(files, (x, y) =>
            {
                FileInfo xx = x as FileInfo;
                FileInfo yy = y as FileInfo;

                return xx.Name.CompareTo(yy.Name);
            });

            if (files.Length <= 0)
                return;

            string startName = fullPathName + Path.DirectorySeparatorChar + files[0].Name;
            Console.WriteLine("Start [" + startName + "]");

            System.Diagnostics.Process.Start("explorer.exe", "\"" + startName + "\"");
        }

        /// <summary>
        /// 키보드 액션
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listDetail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                Console.WriteLine("[Backspace]");

                tabControl1.SelectedIndex = 0;
                dataGridTitles.Focus();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                listDetail_DoubleClick(sender, e);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                btnCancel_Click(sender, e);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Up && e.Control)
            {
                // Control-up
                btnUp_Click(sender, e);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Down && e.Control)
            {
                // Control-up
                btnDown_Click(sender, e);
                e.Handled = true;
            }


        }


        /// <summary>
        /// 리매핑 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemapping_Click(object sender, EventArgs e)
        {
            if (detailDir == null)
                return;

            detailDir.remapName();
            listDetail_Refresh();
        }

        /// <summary>
        /// 변경 내역을 적용
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (detailDir == null)
                return;

            if (detailDir.renameCount() <= 0)
            {
                MessageBox.Show("변경 내역이 없습니다.");
                return;
            }

            DialogResult result = MessageBox.Show("변경 내역을 적용하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
            if (result != DialogResult.Yes)
                return;

            try
            {
                detailDir.renameAll();
            }
            catch (Exception ee)
            {
                MessageBox.Show("변경 중 오류가 발생했습니다. " + ee.ToString());
            }

            detailDir_Init();
            listDetail_Refresh();
        }

        /// <summary>
        /// 변경내역 취소
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            detailDir_Init();
            listDetail_Refresh();
        }


        /// <summary>
        /// 리스트 리프레쉬 처리
        /// </summary>
        private void listDetail_Refresh()
        {
            List<NameStructure> dir1 = detailDir.Directories;

            listDetail.BeginUpdate();
            listDetail.Items.Clear();
            foreach (NameStructure dir in dir1)
            {
                ListViewItem item = new ListViewItem(dir.Prefix);
                item.Name = dir.FullName; // 원본 파일명 저장

                item.SubItems.Add(dir.Name);
                item.SubItems.Add(dir.LastTime);
                item.SubItems.Add(dir.TargetFullName);

                listDetail.Items.Add(item);
            }
            listDetail.EndUpdate();
        }

    }
}
