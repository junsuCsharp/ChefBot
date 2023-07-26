using Common;
using devi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Forms
{
    public partial class FormInfo : Form
    {
        public FormInfo()
        {
            InitializeComponent();

            devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
     $"{System.Reflection.MethodBase.GetCurrentMethod().Name}");
        }

        List<CheckBox> chkMode = new List<CheckBox>();

        public SuperVisorChangeEventHandler SuperVisorChangeEvent;
        public delegate void SuperVisorChangeEventHandler(bool superVisior);

        List<RadioButton> radioButtonsGripperModel = new List<RadioButton>();

        private void FormInfo_Load(object sender, EventArgs e)
        {
            Robot_Init_TreeNode();
            Robot_Init_DataGridView(treeViewInfo, ref dataGridViewInfo);
         
            treeViewInfo.Font = new Font(Fonts.FontLibrary.Families[0], 12);
            textBoxAdminPassWord.Font = new Font(Fonts.FontLibrary.Families[0], 12);
            lbPassWordMsg.Text = "비밀번호를 입력하여 주세요!";

            chkMode.Add(checkBox1);
            chkMode.Add(checkBox2);

            radioButtonsGripperModel.Add(radioButton3);
            radioButtonsGripperModel.Add(radioButton4);
            radioButtonsGripperModel.Add(radioButton5);

            foreach (CheckBox chk in chkMode)
            {
                chk.Visible = false;
            }

            foreach (RadioButton chk in radioButtonsGripperModel)
            {
                chk.Visible = false;
            }

            groupBoxAdmin.Visible =false;

        }
        void Robot_Init_TreeNode()
        {
            TreeNode svrNode = new TreeNode("ChefBot");

            TreeNode opration = new TreeNode("OperatingSystem");
            opration.Nodes.Add("Platform");
            opration.Nodes.Add("ServicePack");
            opration.Nodes.Add("Version");
            opration.Nodes.Add("VersionString");
            opration.Nodes.Add("CLR Version");

            TreeNode robot = new TreeNode("Doosan Robot");
            robot.Nodes.Add("Model");
            robot.Nodes.Add("Version");
            

            TreeNode gripper = new TreeNode("Gripper");
            gripper.Nodes.Add("Model");
            gripper.Nodes.Add("Version");  
        

            TreeNode motor = new TreeNode("X-Axis Motor");
            motor.Nodes.Add("Model");
            motor.Nodes.Add("Version");

            TreeNode gpio1 = new TreeNode("GPIO (1)");
            gpio1.Nodes.Add("Model");
            gpio1.Nodes.Add("Version");

            TreeNode gpio2 = new TreeNode("GPIO (2)");
            gpio2.Nodes.Add("Model");
            gpio2.Nodes.Add("Version");

            TreeNode gpio3 = new TreeNode("GPIO (3)");
            gpio3.Nodes.Add("Model");
            gpio3.Nodes.Add("Version");

            TreeNode gpio4 = new TreeNode("GPIO (4)");
            gpio4.Nodes.Add("Model");
            gpio4.Nodes.Add("Version");


            svrNode.Nodes.Add(opration);
            svrNode.Nodes.Add(robot);
            svrNode.Nodes.Add(gripper);
            svrNode.Nodes.Add(motor);
            svrNode.Nodes.Add(gpio1);
            svrNode.Nodes.Add(gpio2);
            svrNode.Nodes.Add(gpio3);
            svrNode.Nodes.Add(gpio4);

            treeViewInfo.Nodes.Add(svrNode);
            treeViewInfo.ExpandAll();

            //Robot_Init_DataGridView(svrNode, ref dataGridViewInfo);
        }

        void Robot_Init_DataGridView(TreeNode treeNode, ref DataGridView dgv)
        {
            //Console.WriteLine($"DEBUG ::: { treeNode.GetNodeCount(true)} / { treeNode.GetNodeCount(false)}");

            dgv.ReadOnly = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // row 로 선택하기
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None; //row size 막기
            dgv.AllowUserToResizeRows = false; //row size 막기
            dgv.AllowUserToResizeColumns = false; //column size 막기
            dgv.MultiSelect = false;
            dgv.RowHeadersVisible = true;
            dgv.RowHeadersWidth = 15;
            dgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;


            dgv.Rows.Clear();
            dgv.Columns.Clear();

            dgv.ColumnCount = 2;
            dgv.RowCount = treeNode.GetNodeCount(true);
            dgv.ColumnHeadersVisible = true;

            dgv.Columns[0].HeaderText = $"이름";
            dgv.Columns[0].Width = 150;
            dgv.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dgv.Columns[1].HeaderText = $"내용";
            for (int ix = 0; ix < dgv.ColumnCount; ix++)
            {
                if (ix >= 1)
                {
                    //dgv.Columns[ix].HeaderText = $"{ix:00}";
                    dgv.Columns[ix].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgv.Columns[ix].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                dgv.Columns[ix].Resizable = DataGridViewTriState.False;
                dgv.Columns[ix].SortMode = DataGridViewColumnSortMode.NotSortable;

            }
            List<TreeNode> treeNodes = GetAllNodes(treeNode);
            string strNodeHeader = null;
            for (int ux = 0; ux < dgv.RowCount; ux++)
            {
                dgv.Rows[ux].Cells[0].Value = treeNodes[ux].Text;
                if (ux == 0)
                {
                    strNodeHeader = treeNodes[ux].Text;
                    dgv.Rows[ux].HeaderCell.Style.BackColor = Color.DarkGray;
                    dgv.Rows[ux].Cells[0].Style.BackColor = Color.Gray;
                }

                if (treeNodes[ux].Parent != null)
                {
                    if (strNodeHeader.Contains(treeNodes[ux].Parent.Text))
                    {
                        dgv.Rows[ux].HeaderCell.Style.BackColor = Color.DarkGray;
                        dgv.Rows[ux].Cells[0].Style.BackColor = Color.AliceBlue;
                    }
                    else
                    {
                        //dgv.Rows[ux].Cells[0].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }
                //Console.WriteLine($"DEBUG ::: {ux} / {treeNodes[ux].Checked} / {treeNodes[ux].Index} / {treeNodes[ux].Text} / {treeNodes[ux].Name} / {treeNodes[ux].Parent}");
            }


            dgv.AllowUserToAddRows = false;
            //Console.WriteLine($"DEBUG :::");
        }

        void Robot_Init_DataGridView(TreeView treeView, ref DataGridView dgv)
        {
            //Console.WriteLine($"DEBUG ::: { treeNode.GetNodeCount(true)} / { treeNode.GetNodeCount(false)}");

            dgv.ReadOnly = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // row 로 선택하기
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None; //row size 막기
            dgv.AllowUserToResizeRows = false; //row size 막기
           
            dgv.MultiSelect = false;
            dgv.RowHeadersVisible = true;
            dgv.RowHeadersWidth = 15;
            dgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgv.AllowUserToAddRows = false;

            dgv.Rows.Clear();
            dgv.Columns.Clear();

            dgv.ColumnCount = 2;
            dgv.RowCount = treeView.GetNodeCount(true);
            dgv.ColumnHeadersVisible = true;

            dgv.Columns[0].HeaderText = $"이름";
            dgv.Columns[0].Width = 150;
            dgv.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dgv.ColumnHeadersHeight = 50;
            dgv.AllowUserToResizeColumns = false; //column size 막기

            dgv.Columns[1].HeaderText = $"내용";
            for (int ix = 0; ix < dgv.ColumnCount; ix++)
            {
                if (ix >= 1)
                {
                    //dgv.Columns[ix].HeaderText = $"{ix:00}";
                    dgv.Columns[ix].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgv.Columns[ix].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                }

                dgv.Columns[ix].Resizable = DataGridViewTriState.False;
                dgv.Columns[ix].SortMode = DataGridViewColumnSortMode.NotSortable;

            }
            List<TreeNode> treeNodes = GetAllNodes(treeView);
            string strNodeHeader = null;
            for (int ux = 0; ux < dgv.RowCount; ux++)
            {
                dgv.Rows[ux].Cells[0].Value = treeNodes[ux].Text;
                if (ux == 0)
                {
                    strNodeHeader = treeNodes[ux].Text;
                    dgv.Rows[ux].HeaderCell.Style.BackColor = Color.DarkGray;
                    dgv.Rows[ux].Cells[0].Style.BackColor = Color.Gray;
                }

                if (treeNodes[ux].Parent != null)
                {
                    if (strNodeHeader.Contains(treeNodes[ux].Parent.Text))
                    {
                        dgv.Rows[ux].HeaderCell.Style.BackColor = Color.DarkGray;
                        dgv.Rows[ux].Cells[0].Style.BackColor = Color.AliceBlue;
                    }
                    else
                    {
                        //dgv.Rows[ux].Cells[0].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }
                //Console.WriteLine($"DEBUG ::: {ux} / {treeNodes[ux].Checked} / {treeNodes[ux].Index} / {treeNodes[ux].Text} / {treeNodes[ux].Name} / {treeNodes[ux].Parent}");
            }

            //Operating System
            OperatingSystem os = System.Environment.OSVersion;
            //if(System.Environment.Is64BitOperatingSystem)
            //Console.WriteLine("Platform :" + os.Platform);
            //Console.WriteLine("ServicePack :" + os.ServicePack);
            //Console.WriteLine("Version :" + os.Version);
            //Console.WriteLine("VersionString :" + os.VersionString);
            //Console.WriteLine("CLR Version :" + System.Environment.OSVersion);

            dgv.Rows[2].Cells[1].Value = os.Platform;
            dgv.Rows[3].Cells[1].Value = os.ServicePack;
            dgv.Rows[4].Cells[1].Value = os.Version;
            dgv.Rows[5].Cells[1].Value = os.VersionString;
            dgv.Rows[6].Cells[1].Value = System.Environment.OSVersion;

            dgv.Rows[8].Cells[1].Value = "A0509";
            dgv.Rows[9].Cells[1].Value = "2.16.0";

            //dgv.Rows[8].Cells[1].Value = "E0509";
            //dgv.Rows[9].Cells[1].Value = "2.16.2";

            dgv.Rows[11].Cells[1].Value = "PGI-140-80";
            dgv.Rows[12].Cells[1].Value = "PGI-140-80 A 320072B";

            dgv.Rows[14].Cells[1].Value = "Ezi-MOTIONLINK2 Plus-E";
            dgv.Rows[15].Cells[1].Value = "V06.03.031.13";

            dgv.Rows[17].Cells[1].Value = "Ezi-IO Ethernet";
            dgv.Rows[18].Cells[1].Value = "V06.01.010.10";

            dgv.Rows[20].Cells[1].Value = "Ezi-IO Ethernet";
            dgv.Rows[21].Cells[1].Value = "V06.01.010.10";

            dgv.Rows[23].Cells[1].Value = "Ezi-IO Ethernet";
            dgv.Rows[24].Cells[1].Value = "V06.01.010.10";

            dgv.Rows[26].Cells[1].Value = "Ezi-IO Ethernet";
            dgv.Rows[27].Cells[1].Value = "V06.01.010.10";

            
            for (int row = 0; row < dgv.Rows.Count; row++)
            {
                dgv.Rows[row].Height = 29;
                for (int col = 0; col < dgv.Columns.Count; col++)
                {
                    dgv.Rows[row].Cells[col].Style.Font = new Font(Fonts.FontLibrary.Families[0], 12);
                }
            }
            

          
            dgv.ClearSelection();
            //Console.WriteLine($"DEBUG :::");
        }

        // 트리뷰에서 노드 하나 선택시에 모든 자식 노드를 가져오는 함수
        public static List<TreeNode> GetAllNodes(TreeNode node)
        {
            List<TreeNode> result = new List<TreeNode>();
            result.Add(node);
            foreach (TreeNode child in node.Nodes)
            {
                result.AddRange(GetAllNodes(child));
            }
            return result;

        }

        // 트리뷰의 전체 노드
        public static List<TreeNode> GetAllNodes(TreeView treeView)
        {
            List<TreeNode> result = new List<TreeNode>();
            foreach (TreeNode child in treeView.Nodes)
            {
                result.AddRange(GetAllNodes(child));
            }
            return result;
        }

        public void TEST()
        {
            Robot_Init_DataGridView(treeViewInfo, ref dataGridViewInfo);

            //dataGridViewInfo.AllowUserToResizeColumns = true; //column size 막기
            //dataGridViewInfo.ColumnHeadersHeight = 60;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton btn = (RadioButton)sender;
            groupBoxAdmin.Visible = btn.Checked;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            passwordKeying();
        }

        private void textBoxAdminPassWord_Click(object sender, EventArgs e)
        {
            FormKeyboard keyboard = new FormKeyboard(null);
            if(keyboard.ShowDialog() == DialogResult.OK)
            {
                textBoxAdminPassWord.Text = keyboard.UserInputChar;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton btn = (RadioButton)sender;
            if (btn.Checked == true)
            {
                Define.IsAdministrator = false;
                Define.IsSupervisor = false;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;

            Define.IsCobotDebugMove = chk.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;

            Define.IsXaxisDebugMove = chk.Checked;
        }

        /// <summary>
        /// 2023.05.23 ::: 그리퍼 모델 선택
        /// 짐머, 디에치, 온로봇
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            for(int idx =0; idx < radioButtonsGripperModel.Count; idx++) 
            {
                if (radioButtonsGripperModel[idx].Checked == true)
                {
                    Cores.Core_Object.GetObj_File.iGripperModel = idx;

                    Define.eGripper = (EGRIPS)idx;

                    Cores.Core_Object.ObjectFile_Save_Extern();
                    break;
                }
            }
        }

        private void textBoxAdminPassWord_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode != Keys.Enter)
            { return; }

            passwordKeying();


        }

        void passwordKeying()
        {
            if (textBoxAdminPassWord.Text.ToLower() == "admin")
            {
                lbPassWordMsg.Text = "관리자 모드로 변경 하였습니다.";

                //foreach (CheckBox chk in chkMode)
                //{
                //    chk.Visible = true;
                //}

                Define.IsAdministrator = true;
            }
            else if (textBoxAdminPassWord.Text.ToLower() == "jdadmin")
            {
                lbPassWordMsg.Text = "관리자 모드로 변경 하였습니다.";

                foreach (CheckBox chk in chkMode)
                {
                    chk.Visible = true;
                }

                foreach (RadioButton chk in radioButtonsGripperModel)
                {
                    chk.Visible = true;
                }

                Define.IsAdministrator = true;
                Define.IsSupervisor = true;
            }
            else
            {
                lbPassWordMsg.Text = "비밀번호를 다시 입력 하여 주세요!";
            }
        }


        //ip info

        //doosan robot info

        //program info

        //gripper info

        //default info, get info




    }
}

