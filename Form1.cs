using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ChangeBin
{
    public partial class Form1 : Form
    {
        const string rootPath = @"C:\COSMOS\";
        const int MENU = 1;
        const int WEB = 2;

        public Form1()
        {
            InitializeComponent();
        }

        /*
         method : startBtn_Click
         
             */
        private void startBtn_Click(object sender, EventArgs e)
        {
            if (!isEmptyList(menuNames) && !isEmptyList(webNames))
            {
                foreach (string menuName in menuNames.CheckedItems)
                {
                    copyFolder(menuName);
                }
                
            }
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string menuPath = rootPath;
            string webPath = Path.Combine(rootPath, "MKD");

            readFolderNames(MENU, menuPath);
            readFolderNames(WEB, webPath);

        }

        /*
         * [ Method Name ] readFolderNames
         * [ Param_1 ] CheckedListBox list
         * [ Param_2 ] string path
         * 
         */
        private void readFolderNames(int type, string path)
        {
            CheckedListBox list = (type == 1) ? menuNames : webNames;

            DirectoryInfo Info = new DirectoryInfo(path);

            if (Info.Exists)
            {
                DirectoryInfo[] CInfo = Info.GetDirectories();
                foreach (DirectoryInfo info in CInfo)
                {
                    if (folderTypeCheck(info.Name, type))
                    {
                        list.Items.Add(info.Name);
                    }
                }
            }
            else
            {
                MessageBox.Show(path + "에 해당하는 파일이 없습니다.");
            }
        }

        /*
         * [ Method Name ] isEmptyList
         * [ Param_1 ] CheckedListBox list
         * [ Return ] Booelan isEmpty
         * 
         */
        private Boolean isEmptyList(CheckedListBox list)
        {
            Boolean isEmpty = false;

            string listName = (list == menuNames) ? label1.Text : label2.Text;
            if (list.CheckedItems.Count < 1)
            {
                MessageBox.Show(listName + "を選択してください");
            }
            return isEmpty;
        }

        /*
         * [ Method Name ] copyFolder
         * [ Param_1 ] string menuName :　작업할 메뉴
         */
        private void copyFolder(string menuName)
        {
            //기본 접근 root ( 변경할 메뉴 )
            string mainPath = Path.Combine(rootPath, menuName);

            //web 하위의 folder 가져오기 
            DirectoryInfo info = new DirectoryInfo(Path.Combine(mainPath, "web"));
            DirectoryInfo[] folders = info.GetDirectories();

            Console.WriteLine(folders[0]);

            foreach (DirectoryInfo folder in folders) 
            {
                //bin copy
                string[] files = Directory.GetFiles(Path.Combine(mainPath, "web", folder.Name, "bin"));

                foreach (string webName in webNames.CheckedItems)
                {
                    //string pastePath = mainPath + @"\" + webName + @"\" + folder.Name + @"\bin";
                    string pastePath = Path.Combine(mainPath, webName, folder.Name, "bin");
                    //bin 디렉토리 없으면 만들기
                    if (!Directory.Exists(pastePath))
                    {
                        Directory.CreateDirectory(pastePath);
                    }

                    foreach (string file in files)
                    {
                        string path = Path.GetFileName(file);
                        string dest = Path.Combine(pastePath, path);
                        File.Copy(file, dest, true);
                    }
                }
            }

            MessageBox.Show("처리끝");
        }

        private void rootPathSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.StartPosition = FormStartPosition.Manual;
            form2.Location = new Point(10, 10);

            form2.ShowDialog();
            
        }

        /*
         * input check
         * 
         */
        private Boolean folderTypeCheck(string fName, int type) 
        {
            fName = fName.Trim();

            if (type == 1)
            {
                if (fName.Length != 3 || fName[0] != 'M')
                {
                    return false;
                }

            }
            else
            {
                if (!fName.Contains("web") || fName == "web")
                {
                    return false;
                }
            }

            return true;
        }
    }
}
