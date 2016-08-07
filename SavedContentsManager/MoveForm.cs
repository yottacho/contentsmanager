﻿using SavedContentsManager.utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SavedContentsManager
{
    public partial class MoveForm : Form
    {
        const string CACHE_FILE_NAME = "scm_mapping.xml";
        private SerializableDictionary<string, string> nameMappingCache;

        private MoveForm()
        {
            InitializeComponent();
        }

        public MoveForm(string targetPath) : this()
        {
            textTarget.Text = targetPath;
            nameMappingCache = new SerializableDictionary<string, string>();
            nameMappingCache.Load(textTarget.Text, CACHE_FILE_NAME);

            nameMappingCache["?"] = "$";
            nameMappingCache.Save(textTarget.Text, CACHE_FILE_NAME);


            comboSourceFolder.Items.Clear();
            comboSourceFolder.Text = Configure.LastSelectedSourceFolder;

            list_TitleInit(listSource);
            list_TitleInit(listSourceDetail);
            list_TitleInit(listTarget);

            list_TitleDetailInit(listTargetDetail);
            list_TitleDetailInit(listTargetTodo);

            if (comboSourceFolder.Text.Length > 0)
                listSource_Init();

            listTarget_Init();
        }

        /// <summary>
        /// 리스트 타이틀 생성
        /// </summary>
        /// <param name="list"></param>
        private void list_TitleInit(ListView list)
        {
            list.Columns.Clear();
            ColumnHeader colHeader = new ColumnHeader
            {
                Text = "Title",
                Width = 18 * 15
            };
            list.Columns.Add(colHeader);
            colHeader = new ColumnHeader
            {
                Text = "Date",
                Width = 8 * 15
            };
            list.Columns.Add(colHeader);
        }

        /// <summary>
        /// 리스트 타이틀(세부) 생성
        /// </summary>
        /// <param name="list"></param>
        private void list_TitleDetailInit(ListView list)
        {
            list.Columns.Clear();
            ColumnHeader colHeader = new ColumnHeader
            {
                Text = "No",
                Width = 4 * 10
            };
            list.Columns.Add(colHeader);
            colHeader = new ColumnHeader
            {
                Text = "Title",
                Width = 16 * 15
            };
            list.Columns.Add(colHeader);
            colHeader = new ColumnHeader
            {
                Text = "Date",
                Width = 8 * 15
            };
            list.Columns.Add(colHeader);
        }

        /// <summary>
        /// 소스폴더 리스트 데이터 구성
        /// </summary>
        private void listSource_Init()
        {
            listSource.BeginUpdate();
            listSource.Items.Clear();

            DirectoryInfo[] dirList = new DirectoryInfo(comboSourceFolder.Text).GetDirectories();
            Array.Sort(dirList, (x, y) =>
            {
                DirectoryInfo xx = x as DirectoryInfo;
                DirectoryInfo yy = y as DirectoryInfo;

                //return xx.Name.CompareTo(yy.Name);
                // 최근등록일 역순
                return yy.LastWriteTime.CompareTo(xx.LastWriteTime);
            });

            foreach (DirectoryInfo dir in dirList)
            {
                ListViewItem item = new ListViewItem(dir.Name);
                item.Name = dir.FullName; // 원본 파일명 저장
                item.SubItems.Add(dir.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss"));

                listSource.Items.Add(item);
            }

            listSource.EndUpdate();
        }

        /// <summary>
        /// 선택한 소스폴더에 대해 상세내용 리스트
        /// </summary>
        private void listSourceDetail_Init(string path)
        {
            listSourceDetail.BeginUpdate();
            listSourceDetail.Items.Clear();

            DirectoryInfo[] dirList = new DirectoryInfo(path).GetDirectories();
            Array.Sort(dirList, (x, y) =>
            {
                DirectoryInfo xx = x as DirectoryInfo;
                DirectoryInfo yy = y as DirectoryInfo;

                return xx.Name.CompareTo(yy.Name);
            });

            foreach (DirectoryInfo dir in dirList)
            {
                ListViewItem item = new ListViewItem(dir.Name);

                NameStructure detail = new NameStructure(dir.Name);
                if (detail.isPrefix)
                    item = new ListViewItem(detail.Name);

                item.Name = dir.FullName; // 원본 파일명 저장
                item.SubItems.Add(dir.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss"));

                listSourceDetail.Items.Add(item);
            }

            listSourceDetail.EndUpdate();
        }

        /// <summary>
        /// 타겟폴더 리스트 데이터 구성
        /// </summary>
        private void listTarget_Init(string search = "")
        {
            listTarget.BeginUpdate();
            listTarget.Items.Clear();

            DirectoryInfo[] dirList = new DirectoryInfo(textTarget.Text).GetDirectories("*" + search + "*");
            Array.Sort(dirList, (x, y) =>
            {
                DirectoryInfo xx = x as DirectoryInfo;
                DirectoryInfo yy = y as DirectoryInfo;

                return xx.Name.CompareTo(yy.Name);
            });

            foreach (DirectoryInfo dir in dirList)
            {
                ListViewItem item = new ListViewItem(dir.Name);
                item.Name = dir.FullName; // 원본 파일명 저장
                item.SubItems.Add(dir.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss"));

                listTarget.Items.Add(item);
            }

            listTarget.EndUpdate();
        }

        /// <summary>
        /// 타겟폴더 선택항목에 대한 상세정보 리스트 데이터 구성
        /// </summary>
        /// <param name="path"></param>
        private void listTargetDetail_Init(string path)
        {
            DetailDirectory detail = new DetailDirectory(path);

            listTargetDetail.BeginUpdate();
            listTargetDetail.Items.Clear();

            foreach (NameStructure dir in detail.Directories)
            {
                ListViewItem item = new ListViewItem(dir.Prefix);
                item.Name = dir.FullName; // 원본 파일명 저장
                item.SubItems.Add(dir.Name);
                item.SubItems.Add(dir.LastTime);

                listTargetDetail.Items.Add(item);
            }

            if (listTargetDetail.Items.Count > 0)
                listTargetDetail.TopItem = listTargetDetail.Items[listTargetDetail.Items.Count - 1];

            listTargetDetail.EndUpdate();
        }

        /// <summary>
        /// 소스폴더 선택되면 타겟Todo 리스트 생성
        /// </summary>
        private void listTargetTodo_Init()
        {
            // listSourceDetail 이 있어야 함
            if (listSourceDetail.Items.Count < 1)
                return;

            int startIndex = 0;

            // 최종번호 가져와서 최종번호 이후로 처리
            foreach (ListViewItem targetItem in listTargetDetail.Items)
            {
                int i;
                if (targetItem.Text.Length > 0 && int.TryParse(targetItem.Text, out i))
                {
                    startIndex = i;
                }
            }

            listTargetTodo.BeginUpdate();
            listTargetTodo.Items.Clear();
            foreach (ListViewItem srcItem in listSourceDetail.Items)
            {
                startIndex++;
                // srcItem.Name : full path
                // srcItem.Text : directory name

                string no = string.Format("{0:D3}", startIndex);
                ListViewItem item = new ListViewItem(no);
                item.Name = srcItem.Name; // move from
                item.SubItems.Add(srcItem.Text);
                item.SubItems.Add(srcItem.SubItems[1]);

                listTargetTodo.Items.Add(item);
            }
            listTargetTodo.EndUpdate();
        }

        /// <summary>
        /// 소스폴더 찾기버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSourceBrowse_Click(object sender, EventArgs e)
        {
            //Console.WriteLine("Text => " + comboContentsFolder.Text);
            if (comboSourceFolder.Text.Length > 0)
            {
                folderBrowserDialog1.SelectedPath = comboSourceFolder.Text;
            }

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                comboSourceFolder.Text = folderBrowserDialog1.SelectedPath;
                Configure.LastSelectedSourceFolder = folderBrowserDialog1.SelectedPath;
            }
        }

        /// <summary>
        /// 소스폴더 Open버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSourceOpen_Click(object sender, EventArgs e)
        {
            if (comboSourceFolder.Text.Length > 0)
            {
                System.Diagnostics.Process.Start("explorer.exe", comboSourceFolder.Text);
            }
        }

        /// <summary>
        /// 소스폴더 선택항목 Open 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSourceSelOpen_Click(object sender, EventArgs e)
        {
            if (listSource.SelectedItems.Count < 1)
                return;

            System.Diagnostics.Process.Start("explorer.exe", listSource.SelectedItems[0].Name);
        }

        /// <summary>
        /// 소스폴더 리프레시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefreshSource_Click(object sender, EventArgs e)
        {
            listSource.Items.Clear();
            listSourceDetail.Items.Clear();
            listTargetTodo.Items.Clear();
            listSource_Init();
        }


        /// <summary>
        /// 타겟폴더 Open버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTargetOpen_Click(object sender, EventArgs e)
        {
            if (textTarget.Text.Length > 0)
            {
                System.Diagnostics.Process.Start("explorer.exe", textTarget.Text);
            }
        }

        /// <summary>
        /// 타겟폴더 선택항목 Open 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTargetSelOpen_Click(object sender, EventArgs e)
        {
            if (listTarget.SelectedItems.Count < 1)
                return;

            System.Diagnostics.Process.Start("explorer.exe", listTarget.SelectedItems[0].Name);
        }

        /// <summary>
        /// 검색키워드 입력
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textTargetSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                listTarget_Init(textTargetSearch.Text);
                e.Handled = true;
            }
            else if (e.KeyChar == (char)Keys.Escape)
            {
                textTargetSearch.Text = "";
                listTarget_Init(textTargetSearch.Text);
                e.Handled = true;
            }
        }

        /// <summary>
        /// 타겟리스트 선택항목 삭제(완전신규)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNew_Click(object sender, EventArgs e)
        {
            listTarget.SelectedIndices.Clear();
            if (listTarget.Items.Count > 0)
                listTarget.TopItem = listTarget.Items[0];
            listTargetTodo_Init();
        }

        /// <summary>
        /// 소스리스트 선택항목 변경 시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            listSourceDetail.Items.Clear();
            listTargetTodo.Items.Clear();

            if (listSource.SelectedItems.Count < 1)
                return;

            string dirName = listSource.SelectedItems[0].Text;
            string dirPathName = listSource.SelectedItems[0].Name;

            Console.WriteLine("Source [" + dirName + "][" + dirPathName + "]");

            listSourceDetail_Init(dirPathName);

            // 기존에 정의해 둔 이름이 있는지 체크
            var cachedName = nameMappingCache.Where(x => x.Key.Equals(dirName)).SingleOrDefault();
            if (cachedName.Key != null)
            {
                // 기존에 정의한 이름이 있으면 이 이름을 우선 사용
                Console.WriteLine("Target name set [" + cachedName.Value + "]");
                dirName = cachedName.Value;
            }

            // 타겟 중에서 같은 이름이 있는지 찾아서 선택
            ListViewItem foundItem = listTarget.FindItemWithText(dirName, false, 0, false);
            if (foundItem != null)
            {
                Console.WriteLine("일치하는 이름 발견");
                foundItem.Selected = true;
                listTarget.TopItem = foundItem;
            }
            else
            {
                // 일치하는 이름이 없을 경우 선택 클리어
                listTarget.SelectedIndices.Clear();
                if (listTarget.Items.Count > 0)
                    listTarget.TopItem = listTarget.Items[0];
                listTargetTodo_Init();
            }
        }

        /// <summary>
        /// 타겟리스트 선택항목 변경 시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listTarget_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listTarget.SelectedItems.Count < 1)
            {
                listTargetDetail.Items.Clear();
                return;
            }

            listTargetDetail_Init(listTarget.SelectedItems[0].Name);

            listTargetTodo_Init();
        }

        /// <summary>
        /// 이동할 타겟 디렉터리(full path)
        /// </summary>
        private string targetDirName = "";
        private string progressText = "";
        private bool workerRunning = false;

        /// <summary>
        /// 이동처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnProcessAll_Click(object sender, EventArgs e)
        {
            if (workerRunning)
            {
                MessageBox.Show("작업이 진행중입니다. 작업이 완료된 후 다시 시도하세요.");
                return;
            }

            if (listTargetTodo.Items.Count < 1)
            {
                MessageBox.Show("처리할 대상이 없습니다.");
                return;
            }

            if (listSource.SelectedItems.Count < 1)
            {
                MessageBox.Show("소스가 선택되지 않았습니다.");
                return;
            }

            if (listTarget.SelectedItems.Count < 1)
            {
                // 현재 선택한 소스 폴더명으로 새 폴더 생성
                targetDirName = textTarget.Text + Path.DirectorySeparatorChar + listSource.SelectedItems[0].Text;
                Console.WriteLine("New target dir : " + targetDirName);
            }
            else
            {
                targetDirName = listTarget.SelectedItems[0].Name;

                // targetDirName 생성
                Directory.CreateDirectory(targetDirName);

                if (!listSource.SelectedItems[0].Text.Equals(listTarget.SelectedItems[0].Text))
                {
                    // 소스와 타겟 이름이 다르면 매핑캐시에 저장
                    nameMappingCache[listSource.SelectedItems[0].Text] = listTarget.SelectedItems[0].Text;
                }
                else
                {
                    // 소스와 타겟이 동일하면 매핑캐시에서 삭제
                    var cachedName = nameMappingCache.Where(x => x.Key.Equals(listSource.SelectedItems[0].Text)).SingleOrDefault();
                    if (cachedName.Key != null)
                    {
                        nameMappingCache.Remove(nameMappingCache[listSource.SelectedItems[0].Text]);
                    }
                }
                nameMappingCache.Save(textTarget.Text, CACHE_FILE_NAME);
            }

            Console.WriteLine("btnProcessAll_Click Target dir : " + targetDirName);

            // ---------------------
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += allMoveProcess;
            bw.ProgressChanged += MoveProgressChanged;
            bw.RunWorkerCompleted += MoveAllWorkerCompleted;

            bw.WorkerReportsProgress = true;
            bw.RunWorkerAsync();
            // ---------------------
        }

        /// <summary>
        /// 이동 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void allMoveProcess(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            workerRunning = true;

            int directoryCount = 0;
            foreach (ListViewItem item in listTargetTodo.Items)
            {
                directoryCount++;
                string targetName = targetDirName + Path.DirectorySeparatorChar + item.Text + " " + item.SubItems[1].Text;
                Console.WriteLine("Move [" + item.Name + "] to [" + targetName + "]");

                try
                {
                    Directory.CreateDirectory(targetName);

                    DirectoryInfo srcDir = new DirectoryInfo(item.Name);
                    DateTime srcTime = srcDir.LastWriteTime;

                    FileInfo[] files = srcDir.GetFiles();
                    int procIdx = 0;
                    foreach (FileInfo f in files)
                    {
                        procIdx++;
                        Console.WriteLine("Move ... " + f.FullName);

                        progressText = "[" + directoryCount + "/" + listTargetTodo.Items.Count + "] " +
                            item.SubItems[1].Text + "\\" +
                            f.Name + " (" + procIdx + "/" + files.Length + ")";

                        int m = (int)(((double)directoryCount / (double)listTargetTodo.Items.Count) * 100.0) / 10;
                        int s = (int)((double)procIdx / (double)files.Length * 100.0) / 10;

                        int p = (m * 10) + s;
                        if (p < 0)
                            p = 0;
                        if (p > 100)
                            p = 100;

                        worker.ReportProgress(p);

                        f.MoveTo(targetName + Path.DirectorySeparatorChar + f.Name);
                    }

                    // 디렉터리 생성시각 일치
                    DirectoryInfo tDir = new DirectoryInfo(targetName);
                    tDir.LastWriteTime = srcDir.LastWriteTime;

                    srcDir.Delete();
                }
                catch (Exception ee)
                {
                    MessageBox.Show("이동 실패했습니다.\n[" +
                        item.Name + " to " + targetName + "]\n사유: " +
                        ee.Message);
                    return;
                }
            }
        }

        /// <summary>
        /// 프로그래스 업데이트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //e.ProgressPercentage
            txtStatus.Text = progressText;
            progressBar1.Value = e.ProgressPercentage;
        }

        /// <summary>
        /// 워커 실행이 완료되었을 때
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveAllWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            txtStatus.Text = listSource.SelectedItems[0].Text + " 이동 완료";
            progressBar1.Value = 0;
            workerRunning = false;

            // 소스폴더 삭제
            Console.WriteLine("Delete source: " + listSource.SelectedItems[0].Name);
            try
            {
                Directory.Delete(listSource.SelectedItems[0].Name);
            }
            catch (Exception ee)
            {
                MessageBox.Show("디렉터리를 삭제할 수 없습니다. [" + listSource.SelectedItems[0].Name + "]\n사유: " + ee.Message);
                txtStatus.Text += "(폴더 삭제 실패)";
            }

            // 소스폴더 리프레시
            listSource.Items.Clear();
            listSource_Init();
            listSourceDetail.Items.Clear();

            // 타겟폴더 리프레시
            listTarget.Items.Clear();
            listTarget_Init();
            listTargetDetail.Items.Clear();
            listTargetTodo.Items.Clear();
        }


        /// <summary>
        /// 소스 폴더 삭제
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listSource.SelectedItems.Count < 1)
            {
                MessageBox.Show("소스가 선택되지 않았습니다.");
                return;
            }

            ListViewItem srcItem = listSource.SelectedItems[0];

            // 삭제
            DialogResult result = MessageBox.Show("\"" + srcItem.Text + "\"를 삭제하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
            if (result != DialogResult.Yes)
                return;

            Console.WriteLine("Delete [" + srcItem.Name + "]");

            // 매핑캐시에 정의된 이름이 있으면 삭제
            var cachedName = nameMappingCache.Where(x => x.Key.Equals(listSource.SelectedItems[0].Text)).SingleOrDefault();
            if (cachedName.Key != null)
            {
                nameMappingCache.Remove(nameMappingCache[listSource.SelectedItems[0].Text]);
            }

            // 폴더 삭제
            DirectoryInfo dir = new DirectoryInfo(srcItem.Name);
            try
            {
                dir.Delete(true);
            }
            catch (Exception ee)
            {
                MessageBox.Show("디렉터리를 삭제할 수 없습니다. [" + srcItem.Name + "]\n사유: " + ee.Message);
            }

            // 소스폴더 리프레시
            listSource.Items.Clear();
            listSource_Init();
            listSourceDetail.Items.Clear();
            listTargetTodo.Items.Clear();
        }

        /// <summary>
        /// 항목 더블클릭했을 때 액션
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listSourceDetail_DoubleClick(object sender, EventArgs e)
        {
            if (listSourceDetail.SelectedItems.Count == 0)
                return;

            string selectedName = listSourceDetail.SelectedItems[0].Name;
            Console.WriteLine("Full name [" + selectedName + "]");

            FileInfo[] files = new DirectoryInfo(selectedName).GetFiles();
            Array.Sort(files, (x, y) =>
            {
                FileInfo xx = x as FileInfo;
                FileInfo yy = y as FileInfo;

                return xx.Name.CompareTo(yy.Name);
            });

            if (files.Length <= 0)
                return;

            string startName = selectedName + Path.DirectorySeparatorChar + files[0].Name;
            Console.WriteLine("Start [" + startName + "]");

            System.Diagnostics.Process.Start("explorer.exe", startName);
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("개발 중...");
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            MessageBox.Show("개발 중...");
        }

    }
}