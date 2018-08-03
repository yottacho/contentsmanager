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
    public partial class MoveForm : Form
    {
        const string TABLE_NAME = "SCM_MAPPING";

        //const string CACHE_FILE_NAME = "scm_mapping.xml";
        //private SerializableDictionary<string, string> nameMappingCache;

        private MoveForm()
        {
            InitializeComponent();
            /*
            typeof(ListView).InvokeMember("DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null, listSource, new object[] { true });
            typeof(ListView).InvokeMember("DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null, listTarget, new object[] { true });
            typeof(ListView).InvokeMember("DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null, listTargetDetail, new object[] { true });
            */

            // db check
            // Open database
            using (SQLiteConnection conn = new SQLiteConnection(DataBaseUtils.getConnectionString()))
            {
                conn.Open();

                if (!DataBaseUtils.checkTable(conn, TABLE_NAME))
                {
                    using (IDbCommand cmd = conn.CreateCommand())
                        _newDatabase(cmd);
                }
            }
        }

        public MoveForm(string targetPath) : this()
        {
            textTarget.Text = targetPath;
            //nameMappingCache = new SerializableDictionary<string, string>();

            /*
            try
            {
                nameMappingCache.Load(textTarget.Text, CACHE_FILE_NAME);

                nameMappingCache["?"] = "$";
                nameMappingCache.Save(textTarget.Text, CACHE_FILE_NAME);
            }
            catch (Exception e)
            {
                MessageBox.Show("매핑 파일에 접근할 수 없습니다.\n" + e.Message);
            }
            */

            comboSourceFolder.Items.Clear();
            comboSourceFolder.Text = Configure.LastSelectedSourceFolder;

            list_TitleInit(listSource);
            list_TitleInit(listSourceDetail);
            list_TitleInit(listTarget);

            list_TitleDetailInit(listTargetDetail);
            list_TitleDetailInit(listTargetTodo);

            listTarget_Init();

            if (comboSourceFolder.Text.Length > 0)
                listSource_Init();
        }

        /// <summary>
        /// 리스트 타이틀 생성
        /// </summary>
        /// <param name="list"></param>
        private void list_TitleInit(ListView list)
        {
            list.BeginUpdate();
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
            list.EndUpdate();
        }

        /// <summary>
        /// 리스트 타이틀(세부) 생성
        /// </summary>
        /// <param name="list"></param>
        private void list_TitleDetailInit(ListView list)
        {
            list.BeginUpdate();
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
            list.EndUpdate();
        }

        private void _newDatabase(IDbCommand cmd)
        {
            string sql = "CREATE TABLE " + TABLE_NAME + " (" +
                "SOURCE_NAME varchar(200), " +
                "TARGET_NAME varchar(200), " +
                "DELETE_YN varchar(1), " +
                "PRIMARY KEY (SOURCE_NAME)" +
                ")";

            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }

        private string[] _selectDatabase(string sourceName)
        {
            using (SQLiteConnection conn = new SQLiteConnection(DataBaseUtils.getConnectionString()))
            {
                conn.Open();

                return _selectDatabase(conn, sourceName);
            }
        }

        private string[] _selectDatabase(SQLiteConnection conn, string sourceName)
        {
            string[] targetInfo = new string[2];
            SQLiteCommand selectCommand = conn.CreateCommand();
            selectCommand.CommandText = "SELECT TARGET_NAME, DELETE_YN FROM " + TABLE_NAME + " WHERE SOURCE_NAME = @SOURCE_NAME";
            selectCommand.Parameters.Add("@SOURCE_NAME", DbType.String, 200);
            selectCommand.Prepare();

            selectCommand.Parameters["@SOURCE_NAME"].Value = sourceName;

            using (SQLiteDataReader r = selectCommand.ExecuteReader())
            {
                if (r.Read())
                {
                    targetInfo[0] = (string)r["TARGET_NAME"];
                    targetInfo[1] = (string)r["DELETE_YN"];
                    return targetInfo;
                }
            }

            return null;
        }

        /// <summary>
        /// 소스폴더 리스트 데이터 구성
        /// </summary>
        private void listSource_Init()
        {

            DirectoryInfo[] dirList;
            try
            {
                dirList = new DirectoryInfo(comboSourceFolder.Text).GetDirectories();
            }
            catch (Exception e)
            {
                MessageBox.Show("소스 디렉터리에 접근할 수 없습니다.\n" + e.Message);
                return;
            }

            Array.Sort(dirList, (x, y) =>
            {
                DirectoryInfo xx = x as DirectoryInfo;
                DirectoryInfo yy = y as DirectoryInfo;

                //return xx.Name.CompareTo(yy.Name);
                // 최근등록일 역순
                return yy.LastWriteTime.CompareTo(xx.LastWriteTime);
            });

            listSource.BeginUpdate();
            listSource.Items.Clear();

            using (SQLiteConnection conn = new SQLiteConnection(DataBaseUtils.getConnectionString()))
            {
                conn.Open();

                foreach (DirectoryInfo dir in dirList)
                {
                    ListViewItem item = new ListViewItem(dir.Name);
                    item.Name = dir.FullName; // 원본 파일명 저장
                    item.SubItems.Add(dir.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss"));

                    string[] mapInfo = _selectDatabase(conn, dir.Name);
                    string targetName = dir.Name;
                    if (mapInfo != null)
                    {
                        // 삭제된 항목
                        if ("Y".Equals(mapInfo[1]))
                            item.BackColor = Color.FromArgb(0xff, 0xc0, 0xc0);
                        else
                            targetName = mapInfo[0];
                    }

                    // target 과 매핑되는지 확인
                    for (int i = 0; i < listTarget.Items.Count; i++)
                    {
                        if (targetName.Equals(listTarget.Items[i].Text))
                        {
                            item.BackColor = Color.FromArgb(0xe0, 0xfc, 0xe4);
                            break;
                        }
                    }

                    /*
                    currentTrLog.BackColor = Color.FromArgb(0xe0, 0xfc, 0xe4);  // d 연녹색
                    currentTrLog.BackColor = Color.FromArgb(0xb6, 0xda, 0xf0);  // i 청색
                    currentTrLog.BackColor = Color.FromArgb(0xec, 0xec, 0x90);  // w 노랑
                    currentTrLog.BackColor = Color.FromArgb(0xff, 0xc0, 0xc0);  // e 빨강
                    currentTrLog.BackColor = Color.FromArgb(0xe0, 0xe0, 0xe0);  // x 회색
                     */

                    listSource.Items.Add(item);
                }
            }

            listSource.EndUpdate();
            textCount.Text = string.Format("{0:N0}", listSource.Items.Count);
        }

        /// <summary>
        /// 선택한 소스폴더에 대해 상세내용 리스트
        /// </summary>
        private void listSourceDetail_Init(string path)
        {
            DirectoryInfo[] dirList;
            try
            {
                dirList = new DirectoryInfo(path).GetDirectories();
            }
            catch (DirectoryNotFoundException)
            {
                listSourceDetail.Items.Clear();
                return;
            }

            Array.Sort(dirList, (x, y) =>
            {
                DirectoryInfo xx = x as DirectoryInfo;
                DirectoryInfo yy = y as DirectoryInfo;

                return xx.Name.CompareTo(yy.Name);
            });

            listSourceDetail.BeginUpdate();
            listSourceDetail.Items.Clear();

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
            //textTargetSearch.Text = "";

            DirectoryToDataTable contentsDirectory = new DirectoryToDataTable(textTarget.Text);

            /*
            DirectoryInfo[] dirList;
            try
            {
                dirList = new DirectoryInfo(textTarget.Text).GetDirectories("*" + search + "*");
            }
            catch (Exception e)
            {
                MessageBox.Show("타겟 디렉터리에 접근할 수 없습니다.\n" + e.Message);
                return;
            }

            Array.Sort(dirList, (x, y) =>
            {
                DirectoryInfo xx = x as DirectoryInfo;
                DirectoryInfo yy = y as DirectoryInfo;

                return xx.Name.CompareTo(yy.Name);
            });
            */

            listTarget.BeginUpdate();
            listTarget.Items.Clear();

            if (search != null && search.Length > 0)
                contentsDirectory.DirectoryInfoView.RowFilter = "[TITLE_NAME] LIKE '%" + search + "%'";

            foreach (DataRowView dir in contentsDirectory.DirectoryInfoView)
            {
                /*
                ListViewItem item = new ListViewItem(dir.Name);
                item.Name = dir.FullName; // 원본 파일명 저장
                item.SubItems.Add(dir.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss"));
                */

                ListViewItem item = new ListViewItem((string)dir["TITLE_NAME"]);
                item.Name = Path.Combine(textTarget.Text, (string)dir["TITLE_NAME"]);
                item.SubItems.Add((string)dir["LAST_UPDATED"]);
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
            DetailDirectory detail;
            try
            {
                detail = new DetailDirectory(path);
            }
            catch (DirectoryNotFoundException)
            {
                listTargetDetail.Items.Clear();
                return;
            }

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

            listTargetDetail.EndUpdate();

            listTargetDetail.Columns[1].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            if (listTargetDetail.Columns[1].Width < (16 * 15))
                listTargetDetail.Columns[1].Width = 16 * 15;

            if (listTargetDetail.Items.Count > 0)
                listTargetDetail.TopItem = listTargetDetail.Items[listTargetDetail.Items.Count - 1];
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

            listTargetTodo.Columns[1].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            if (listTargetTodo.Columns[1].Width < (16 * 15))
                listTargetTodo.Columns[1].Width = 16 * 15;

        }

        /// <summary>
        /// 소스폴더 찾기버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSourceBrowse_Click(object sender, EventArgs e)
        {
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
                System.Diagnostics.Process.Start("explorer.exe", "\"" + comboSourceFolder.Text + "\"");
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

            System.Diagnostics.Process.Start("explorer.exe", "\"" + listSource.SelectedItems[0].Name + "\"");
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
                System.Diagnostics.Process.Start("explorer.exe", "\"" + textTarget.Text + "\"");
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

            System.Diagnostics.Process.Start("explorer.exe", "\"" + listTarget.SelectedItems[0].Name + "\"");
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
                //e.Handled = true;
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

            textTargetName.ReadOnly = false;
            textTargetName.Text = "";

            if (listSource.SelectedItems.Count > 0)
                textTargetName.Text = listSource.SelectedItems[0].Text;

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

            textTargetName.ReadOnly = false;
            textTargetName.Text = "";
            textTargetSearch.Text = "";
            listTarget_Init();
            listTargetDetail.Items.Clear();

            listSourceDetail_Init(dirPathName);

            // 기존에 정의해 둔 이름이 있는지 체크
            /*
            if (nameMappingCache.ContainsKey(dirName))
            {
                // 기존에 정의한 이름이 있으면 이 이름을 우선 사용
                dirName = nameMappingCache[dirName];
            }
            */
            string[] mappingInfo = _selectDatabase(dirName);
            if (mappingInfo != null && !"Y".Equals(mappingInfo[1]))
                dirName = mappingInfo[0];

            // 타겟 중에서 같은 이름이 있는지 찾아서 선택
            if (listTarget.Items.Count > 0)
            {
                ListViewItem foundItem = listTarget.FindItemWithText(dirName, false, 0, false);
                if (foundItem != null)
                {
                    foundItem.Selected = true;
                    listTarget.TopItem = foundItem;

                    textTargetName.Text = foundItem.Text;
                    textTargetName.ReadOnly = true;
                }
                else
                {
                    bool f = false;
                    int nmIdx = dirName.LastIndexOf(" ");
                    if (nmIdx > 0)
                    {
                        dirName = dirName.Substring(0, nmIdx);
                        foundItem = listTarget.FindItemWithText(dirName, false, 0, false);
                        if (foundItem != null)
                        {
                            //Console.WriteLine("일부 일치하는 이름 발견");
                            foundItem.Selected = true;
                            listTarget.TopItem = foundItem;

                            textTargetName.Text = foundItem.Text;
                            textTargetName.ReadOnly = true;

                            f = true;
                        }
                    }

                    if (!f)
                    {
                        // 일치하는 이름이 없을 경우 선택 클리어
                        //Console.WriteLine("일치하는 이름이 없음 (새 항목)");
                        listTarget.SelectedIndices.Clear();
                        if (listTarget.Items.Count > 0)
                            listTarget.TopItem = listTarget.Items[0];

                        textTargetName.Text = listSource.SelectedItems[0].Text;
                        listTargetTodo_Init();
                    }
                }
            }
            // 타겟이 하나도 없을 경우...
            else
            {
                // 일치하는 이름이 없을 경우 선택 클리어
                //Console.WriteLine("일치하는 이름이 없음 (새 항목)");
                listTarget.SelectedIndices.Clear();
                if (listTarget.Items.Count > 0)
                    listTarget.TopItem = listTarget.Items[0];

                textTargetName.Text = listSource.SelectedItems[0].Text;
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
        private ListViewItem processDir = null;
        private ListViewItem[] progressTarget = null;
        private bool workerRunning = false;
        private int itemIndex = 0;

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

            if (listSource.SelectedItems.Count < 1)
            {
                MessageBox.Show("소스가 선택되지 않았습니다.");
                return;
            }

            if (listTargetTodo.Items.Count < 1)
            {
                // target 이름 생성
                ListViewItem srcItem = listSource.SelectedItems[0];

                // 최종번호 가져와서 최종번호 이후로 처리
                int startIndex = 0;
                foreach (ListViewItem targetItem in listTargetDetail.Items)
                {
                    int i;
                    if (targetItem.Text.Length > 0 && int.TryParse(targetItem.Text, out i))
                    {
                        startIndex = i;
                    }
                }
                startIndex++;
                string no = string.Format("{0:D3}", startIndex);

                // srcItem.Name : full path
                // srcItem.Text : directory name
                ListViewItem item = new ListViewItem(no);
                item.Name = srcItem.Name; // move from
                item.SubItems.Add(srcItem.Text);
                item.SubItems.Add(srcItem.SubItems[1]);

                string targetTitleName = item.SubItems[0].Text + " " + item.SubItems[1].Text;
                if (MessageBox.Show("처리할 대상이 없습니다. 생성해서 이동할까요?\n" + targetTitleName, "선택", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }

                // source 폴더를 sub folder에 밀어넣음
                listTargetTodo.BeginUpdate();

                //MessageBox.Show("Name:" + srcItem.Name + "\nText:" + srcItem.Text);

                listTargetTodo.Items.Add(item);

                listTargetTodo.EndUpdate();
            }

            if (listTarget.SelectedItems.Count < 1)
            {
                if (textTargetName.Text.Length == 0)
                {
                    MessageBox.Show("폴더명이 없습니다.");
                    return;
                }

                // 현재 선택한 소스 폴더명으로 새 폴더 생성
                //targetDirName = textTarget.Text + Path.DirectorySeparatorChar + listSource.SelectedItems[0].Text;
                // 지정한 이름으로 생성할 수 있도록 함
                targetDirName = textTarget.Text + Path.DirectorySeparatorChar + textTargetName.Text;
                //Console.WriteLine("New target dir : " + targetDirName);

                // 이름이 불일치하면 매핑캐시에 저장
                if (!listSource.SelectedItems[0].Text.Equals(textTargetName.Text))
                {
                    //Console.WriteLine("Cache Set " + listSource.SelectedItems[0].Text + " = " + textTargetName.Text);
                    // 소스와 타겟 이름이 다르면 매핑캐시에 저장
                    using (SQLiteConnection conn = new SQLiteConnection(DataBaseUtils.getConnectionString()))
                    {
                        conn.Open();
                        string[] info = _selectDatabase(conn, listSource.SelectedItems[0].Text);
                        if (info == null)
                        {
                            SQLiteCommand insertCommand = conn.CreateCommand();
                            insertCommand.CommandText = "INSERT INTO " + TABLE_NAME +
                                " (SOURCE_NAME, TARGET_NAME, DELETE_YN) VALUES" +
                                " (@SOURCE_NAME, @TARGET_NAME, @DELETE_YN)";
                            insertCommand.Parameters.Add("@SOURCE_NAME", DbType.String, 200);
                            insertCommand.Parameters.Add("@TARGET_NAME", DbType.String, 200);
                            insertCommand.Parameters.Add("@DELETE_YN", DbType.String, 1);
                            insertCommand.Prepare();

                            insertCommand.Parameters["@SOURCE_NAME"].Value = listSource.SelectedItems[0].Text;
                            insertCommand.Parameters["@TARGET_NAME"].Value = textTargetName.Text;
                            insertCommand.Parameters["@DELETE_YN"].Value = "N";

                            insertCommand.ExecuteNonQuery();
                        }
                        else
                        {
                            SQLiteCommand updateCommand = conn.CreateCommand();
                            updateCommand.CommandText = "UPDATE " + TABLE_NAME +
                                " SET DELETE_YN = 'N' WHERE SOURCE_NAME = @SOURCE_NAME AND TARGET_NAME = @TARGET_NAME";
                            updateCommand.Parameters.Add("@SOURCE_NAME", DbType.String, 200);
                            updateCommand.Parameters.Add("@TARGET_NAME", DbType.String, 200);
                            updateCommand.Prepare();

                            updateCommand.Parameters["@SOURCE_NAME"].Value = listSource.SelectedItems[0].Text;
                            updateCommand.Parameters["@TARGET_NAME"].Value = textTargetName.Text;

                            updateCommand.ExecuteNonQuery();

                        }
                    }

                    //nameMappingCache[listSource.SelectedItems[0].Text] = textTargetName.Text;
                    // 저장
                    //nameMappingCache.Save(textTarget.Text, CACHE_FILE_NAME);
                }

                // targetDirName 생성
                try
                {
                    Directory.CreateDirectory(targetDirName);
                }
                catch (Exception ee)
                {
                    MessageBox.Show("디렉터리를 생성할 수 없습니다.\n" + ee.Message);
                    return;
                }
            }
            else
            {
                targetDirName = listTarget.SelectedItems[0].Name;

                if (!listSource.SelectedItems[0].Text.Equals(listTarget.SelectedItems[0].Text))
                {
                    //Console.WriteLine("Cache Set " + listSource.SelectedItems[0].Text + " = " + listTarget.SelectedItems[0].Text);
                    // 소스와 타겟 이름이 다르면 매핑캐시에 저장
                    //nameMappingCache[listSource.SelectedItems[0].Text] = listTarget.SelectedItems[0].Text;
                    using (SQLiteConnection conn = new SQLiteConnection(DataBaseUtils.getConnectionString()))
                    {
                        conn.Open();
                        string[] info = _selectDatabase(conn, listSource.SelectedItems[0].Text);
                        if (info == null)
                        {
                            SQLiteCommand insertCommand = conn.CreateCommand();
                            insertCommand.CommandText = "INSERT INTO " + TABLE_NAME +
                                " (SOURCE_NAME, TARGET_NAME, DELETE_YN) VALUES" +
                                " (@SOURCE_NAME, @TARGET_NAME, @DELETE_YN)";
                            insertCommand.Parameters.Add("@SOURCE_NAME", DbType.String, 200);
                            insertCommand.Parameters.Add("@TARGET_NAME", DbType.String, 200);
                            insertCommand.Parameters.Add("@DELETE_YN", DbType.String, 1);
                            insertCommand.Prepare();

                            insertCommand.Parameters["@SOURCE_NAME"].Value = listSource.SelectedItems[0].Text;
                            insertCommand.Parameters["@TARGET_NAME"].Value = listTarget.SelectedItems[0].Text;
                            insertCommand.Parameters["@DELETE_YN"].Value = "N";

                            insertCommand.ExecuteNonQuery();
                        }
                        else
                        {
                            SQLiteCommand updateCommand = conn.CreateCommand();
                            updateCommand.CommandText = "UPDATE " + TABLE_NAME +
                                " SET DELETE_YN = 'N' WHERE SOURCE_NAME = @SOURCE_NAME AND TARGET_NAME = @TARGET_NAME";
                            updateCommand.Parameters.Add("@SOURCE_NAME", DbType.String, 200);
                            updateCommand.Parameters.Add("@TARGET_NAME", DbType.String, 200);
                            updateCommand.Prepare();

                            updateCommand.Parameters["@SOURCE_NAME"].Value = listSource.SelectedItems[0].Text;
                            updateCommand.Parameters["@TARGET_NAME"].Value = textTargetName.Text;

                            updateCommand.ExecuteNonQuery();

                        }

                    }
                }
                /*
                else
                {
                    // 소스와 타겟이 동일하면 소스를 매핑캐시에서 삭제
                    //if (nameMappingCache.ContainsKey(listSource.SelectedItems[0].Text))
                    //{
                        //Console.WriteLine("Cache Remove " + listSource.SelectedItems[0].Text);
                        //nameMappingCache.Remove(listSource.SelectedItems[0].Text);
                    //}
                }
                */
                //nameMappingCache.Save(textTarget.Text, CACHE_FILE_NAME);
            }

            //Console.WriteLine("btnProcessAll_Click Target dir : " + targetDirName);

            // UI 컨트롤에 저장한 항목을 다른 메모리로 복제(다른 쓰레드에서 UI 컨트롤 억세스를 차단)
            processDir = listSource.SelectedItems[0];
            progressTarget = new ListViewItem[listTargetTodo.Items.Count];
            listTargetTodo.Items.CopyTo(progressTarget, 0);
            itemIndex = listSource.SelectedIndices[0];   // 현재 선택한 항목 위치 저장

            btnProcessAll.Enabled = false;
            // ---------------------
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += allMoveProcess;
            bw.ProgressChanged += MoveProgressChanged;
            bw.RunWorkerCompleted += MoveAllWorkerCompleted;

            bw.WorkerReportsProgress = true;
            bw.RunWorkerAsync();
            // ---------------------

            textTargetSearch.Text = "";
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
            //Console.WriteLine("Count: " + progressTarget.Length);
            int procIdx = 0;
            int procTot = 0;

            progressText = "Init ...";
            worker.ReportProgress(0);
            foreach (ListViewItem item in progressTarget)
            {
                procTot += new DirectoryInfo(item.Name).GetFiles().Length;
            }

            foreach (ListViewItem item in progressTarget)
            {
                directoryCount++;
                string targetName = targetDirName + Path.DirectorySeparatorChar + item.Text + " " + item.SubItems[1].Text;
                //Console.WriteLine("Move [" + item.Name + "] to [" + targetName + "]");

                try
                {
                    Directory.CreateDirectory(targetName);

                    DirectoryInfo srcDir = new DirectoryInfo(item.Name);
                    DateTime srcTime = srcDir.LastWriteTime;

                    FileInfo[] files = srcDir.GetFiles();
                    foreach (FileInfo f in files)
                    {
                        procIdx++;
                        //Console.WriteLine("Move ... " + f.FullName);

                        progressText = "[" + directoryCount + "/" + progressTarget.Length + "] " +
                            item.SubItems[1].Text + "\\" +
                            f.Name + " (" + procIdx + "/" + procTot + ")";

                        int p = (int)((double)procIdx / (double)procTot * 100.0);
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

                    // 소스 디렉터리 삭제
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
            txtStatus.Text = processDir.Text + " 이동 완료";
            progressBar1.Value = 0;
            workerRunning = false;
            progressTarget = null;

            btnProcessAll.Enabled = true;

            // moveall 이후 껍데기만 남은 소스 디렉터리 삭제
            //Console.WriteLine("Delete source: " + processDir.Name);
            try
            {
                if (Directory.Exists(processDir.Name))
                    Directory.Delete(processDir.Name);
            }
            catch (Exception ee)
            {
                MessageBox.Show("디렉터리를 삭제할 수 없습니다. [" + processDir.Name + "]\n사유: " + ee.Message);
                txtStatus.Text += "(폴더 삭제 실패)";
            }

            int selectedItem = -1;
            if (listSource.SelectedIndices.Count > 0)
            {
                // 현재 목록에서 선택되어 있는 항목
                selectedItem = listSource.SelectedIndices[0];

                // 현재 선택된 항목이 이동한 항목과 같거나 크면 선택된 항목을 1개 줄인다.
                if (selectedItem >= itemIndex)
                    selectedItem--;

                // 인덱스가 0보다 작을 수는 없으므로 보정
                if (selectedItem < 0)
                    selectedItem = 0;
            }

            processDir = null;

            // 소스폴더 리프레시
            listSource.Items.Clear();
            listSource_Init();
            listSourceDetail.Items.Clear();

            // 타겟폴더 리프레시
            listTarget.Items.Clear();
            listTarget_Init();
            listTargetDetail.Items.Clear();
            listTargetTodo.Items.Clear();

            // 선택항목을 복원
            if (selectedItem >= 0 && selectedItem < listSource.Items.Count)
            {
                listSource.SelectedIndices.Add(selectedItem);
                listSource_SelectedIndexChanged(sender, e);
            }

            _listSource_SetTopItem();
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

            int selectedItem = -1;
            if (listSource.SelectedIndices.Count > 0)
            {
                // 현재 목록에서 선택되어 있는 항목
                selectedItem = listSource.SelectedIndices[0];
                selectedItem--;

                // 인덱스가 0보다 작을 수는 없으므로 보정
                if (selectedItem < 0)
                    selectedItem = 0;
            }

            // 삭제
            DialogResult result = MessageBox.Show("\"" + srcItem.Text + "\"를 삭제하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
            if (result != DialogResult.Yes)
                return;

            //Console.WriteLine("Delete [" + srcItem.Name + "]");

            // 매핑캐시에 정의된 이름이 있으면 삭제
            /*
            if (nameMappingCache.ContainsKey(listSource.SelectedItems[0].Text))
            {
                nameMappingCache.Remove(listSource.SelectedItems[0].Text);
            }
            */
            string[] mappingInfo = _selectDatabase(srcItem.Text);
            if (mappingInfo != null)
            {
                // UPDATE
                using (SQLiteConnection conn = new SQLiteConnection(DataBaseUtils.getConnectionString()))
                {
                    conn.Open();
                    SQLiteCommand updateCommand = conn.CreateCommand();
                    updateCommand.CommandText = "UPDATE " + TABLE_NAME +
                        " SET TARGET_NAME = @TARGET_NAME, DELETE_YN = @DELETE_YN" +
                        " WHERE SOURCE_NAME = @SOURCE_NAME";
                    updateCommand.Parameters.Add("@TARGET_NAME", DbType.String, 200);
                    updateCommand.Parameters.Add("@DELETE_YN", DbType.String, 1);
                    updateCommand.Parameters.Add("@SOURCE_NAME", DbType.String, 200);
                    updateCommand.Prepare();

                    updateCommand.Parameters["@TARGET_NAME"].Value = srcItem.Text;
                    updateCommand.Parameters["@DELETE_YN"].Value = "Y";
                    updateCommand.Parameters["@SOURCE_NAME"].Value = srcItem.Text;

                    updateCommand.ExecuteNonQuery();
                }
            }
            else
            {
                // INSERT
                using (SQLiteConnection conn = new SQLiteConnection(DataBaseUtils.getConnectionString()))
                {
                    conn.Open();
                    SQLiteCommand insertCommand = conn.CreateCommand();
                    insertCommand.CommandText = "INSERT INTO " + TABLE_NAME +
                        " (SOURCE_NAME, TARGET_NAME, DELETE_YN) VALUES" +
                        " (@SOURCE_NAME, @TARGET_NAME, @DELETE_YN)";
                    insertCommand.Parameters.Add("@SOURCE_NAME", DbType.String, 200);
                    insertCommand.Parameters.Add("@TARGET_NAME", DbType.String, 200);
                    insertCommand.Parameters.Add("@DELETE_YN", DbType.String, 1);
                    insertCommand.Prepare();

                    insertCommand.Parameters["@SOURCE_NAME"].Value = srcItem.Text;
                    insertCommand.Parameters["@TARGET_NAME"].Value = srcItem.Text;
                    insertCommand.Parameters["@DELETE_YN"].Value = "Y";

                    insertCommand.ExecuteNonQuery();
                }
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

            textTargetSearch.Text = "";
            listTarget_Init();

            // 소스폴더 리프레시
            listSource.Items.Clear();
            listSource_Init();
            listSourceDetail.Items.Clear();
            listTargetTodo.Items.Clear();

            // 선택항목을 복원
            if (selectedItem >= 0 && selectedItem < listSource.Items.Count)
            {
                listSource.SelectedIndices.Add(selectedItem);
                listSource_SelectedIndexChanged(sender, e);
            }

            // 항목 스크롤
            _listSource_SetTopItem();
        }

        /// <summary>
        /// 작업 후 최상위 항목 Set
        /// </summary>
        private void _listSource_SetTopItem()
        {
            // 선택한 항목이 있으면 선택한 항목이 대략 가운데 오도록 한다. 5개 위에 있는 항목 선택
            if (listSource.SelectedIndices.Count > 0)
            {
                int idx = listSource.SelectedIndices[0] - 5;
                if (idx < 0)
                {
                    idx = 0;
                }
                listSource.TopItem = listSource.Items[idx];
            }
            // TODO 선택한 항목이 없는 경우
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
            //Console.WriteLine("Full name [" + selectedName + "]");

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
            //Console.WriteLine("Start [" + startName + "]");

            System.Diagnostics.Process.Start("explorer.exe", "\"" + startName + "\"");
        }

        private void listSource_DoubleClick(object sender, EventArgs e)
        {
            if (listSource.SelectedItems.Count == 0)
                return;

            string selectedName = listSource.SelectedItems[0].Name;
            //Console.WriteLine("Full name [" + selectedName + "]");

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
            //Console.WriteLine("Start [" + startName + "]");

            System.Diagnostics.Process.Start("explorer.exe", "\"" + startName + "\"");
        }


        private void btnUp_Click(object sender, EventArgs e)
        {
            if (listSourceDetail.SelectedIndices.Count == 0)
                return;

            int idx = listSourceDetail.SelectedIndices[0];
            if (idx <= 0)
                return;

            ListViewItem item = listSourceDetail.SelectedItems[0];
            listSourceDetail.Items.RemoveAt(idx);
            idx--;
            listSourceDetail.Items.Insert(idx, item);
            listSourceDetail.SelectedIndices.Add(idx);
            listSourceDetail.TopItem = listSourceDetail.SelectedItems[0];
            listSourceDetail.SelectedItems[0].Focused = true;

            listTargetTodo_Init();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (listSourceDetail.SelectedIndices.Count == 0)
                return;

            int idx = listSourceDetail.SelectedIndices[0];
            if ((idx + 1) >= listSourceDetail.Items.Count)
                return;

            ListViewItem item = listSourceDetail.SelectedItems[0];
            listSourceDetail.Items.RemoveAt(idx);
            idx++;
            listSourceDetail.Items.Insert(idx, item);
            listSourceDetail.SelectedIndices.Add(idx);
            listSourceDetail.TopItem = listSourceDetail.SelectedItems[0];
            listSourceDetail.SelectedItems[0].Focused = true;

            listTargetTodo_Init();
        }

        private void listTargetDetail_DoubleClick(object sender, EventArgs e)
        {
            if (listTarget.SelectedItems.Count == 0)
                return;

            string selectedName = listTarget.SelectedItems[0].Name + Path.DirectorySeparatorChar + listTargetDetail.SelectedItems[0].Name;
            //Console.WriteLine("Full name [" + selectedName + "]");

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
            //Console.WriteLine("Start [" + startName + "]");

            System.Diagnostics.Process.Start("explorer.exe", "\"" + startName + "\"");
        }

        /// <summary>
        /// 리스트소스 선택항목 위로
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelUp_Click(object sender, EventArgs e)
        {
            if (listSource.SelectedItems.Count == 0)
                return;

            int selectedIndex = listSource.SelectedIndices[0] - 1;
            if (selectedIndex < 0)
                return;

            listSource.SelectedIndices.Remove(0);
            listSource.SelectedIndices.Add(selectedIndex);

            _listSource_SetTopItem();
        }

        /// <summary>
        /// 리스트소스 선택항목 아래로
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelDown_Click(object sender, EventArgs e)
        {
            if (listSource.SelectedItems.Count == 0)
                return;

            int selectedIndex = listSource.SelectedIndices[0] + 1;
            if (selectedIndex >= listSource.Items.Count)
                return;

            listSource.SelectedIndices.Remove(0);
            listSource.SelectedIndices.Add(selectedIndex);

            _listSource_SetTopItem();
        }

        /// <summary>
        /// 폼 키보드 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyCode == Keys.Up)
                {
                    btnSelUp_Click(sender, null);
                    e.Handled = true;
                    e.SuppressKeyPress = false;
                }
                if (e.KeyCode == Keys.Down)
                {
                    btnSelDown_Click(sender, null);
                    e.Handled = true;
                    e.SuppressKeyPress = false;
                }
                if (e.KeyCode == Keys.D)
                {
                    btnDelete_Click(sender, null);
                    e.Handled = true;
                    e.SuppressKeyPress = false;
                }
                if (e.KeyCode == Keys.A)
                {
                    btnProcessAll_Click(sender, null);
                    e.Handled = true;
                    e.SuppressKeyPress = false;
                }
                if ((e.KeyCode == Keys.R))
                {
                    btnRefreshSource_Click(sender, null);
                    e.Handled = true;
                    e.SuppressKeyPress = false;
                }
            }
        }

        private void listSourceContextMenu_Opening(object sender, CancelEventArgs e)
        {
            // 오른쪽 버튼 클릭했을 때 폴더명을 단어단위로 쪼개서 검색
            if (listSource.SelectedItems.Count == 0)
            {
                e.Cancel = true;
                return;
            }

            string titleName = listSource.SelectedItems[0].Text;
            Debug.WriteLine("[" + titleName + "]");

            listSourceContextMenu.Items.Clear();

            string[] words = titleName.Split(' ');
            foreach (string word in words)
            {
                ToolStripItem stripItem = new ToolStripMenuItem(word);
                stripItem.Tag = word;
                stripItem.Click += (s2, e2) =>
                {
                    ToolStripItem itm = s2 as ToolStripItem;
                    string tag = (string)itm.Tag;

                    Debug.WriteLine("Tag=>" + tag);
                    listTarget_Init(tag);
                };
                listSourceContextMenu.Items.Add(stripItem);
            }

        }
    }
}
