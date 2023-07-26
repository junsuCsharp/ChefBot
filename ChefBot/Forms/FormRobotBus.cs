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
    public partial class FormRobotBus : Form
    {
        public FormRobotBus()
        {
            InitializeComponent();
        }


        public EventHandler RobotSenderEvent;
        public delegate void EventHandler(object sender, EventArgs e);

        private void FormRobotBus_Load(object sender, EventArgs e)
        {
            Robot_Init_TreeNode();
            
            if (devi.Define.IsSupervisor)
            {
                this.dataGridViewRobotInfo.Size = new Size(1640, 716);
            }
            else
            {
                this.dataGridViewRobotInfo.Size = new Size(1640, 863);
            }

            this.dataGridViewRobotInfo.Size = new Size(1640, 863);
        }

        #region TreeView, DataGridView
        void Robot_Init_TreeNode()
        {
            TreeNode svrNode = new TreeNode("두산 협동 로봇");

            TreeNode jointSpace = new TreeNode("조인트 공간");
            jointSpace.Nodes.Add("현재 각도");
            jointSpace.Nodes.Add("현재 속도");

            TreeNode taskSpace_base = new TreeNode("태스크 공간(베이스)");
            taskSpace_base.Nodes.Add("현재 좌표");
            taskSpace_base.Nodes.Add("현재 속도");
            taskSpace_base.Nodes.Add("현재 TCP속도");

            TreeNode taskSpace_world = new TreeNode("태스크 공간(월드)");
            taskSpace_world.Nodes.Add("현재 좌표");
            taskSpace_world.Nodes.Add("현재 속도");
            taskSpace_world.Nodes.Add("월드/베이스 관계");

            TreeNode taskSpace_user = new TreeNode("태스크 공간(사용자)");
            //TreeNode taskSpace_user = new TreeNode("툴 옵셋");
            taskSpace_user.Nodes.Add("툴 옵셋");
            taskSpace_user.Nodes.Add("현재 속도");
            taskSpace_user.Nodes.Add("사용자 좌표게 ID");
            taskSpace_user.Nodes.Add("부모 좌표계");

            TreeNode force_torque = new TreeNode("힘/토크");
            force_torque.Nodes.Add("조인트 토크(센서)");
            force_torque.Nodes.Add("모터 토크");
            force_torque.Nodes.Add("힘토크 센서");
            force_torque.Nodes.Add("가속도 센서");
            force_torque.Nodes.Add("조이트 토크(중력/모델)");
            force_torque.Nodes.Add("조인트 외력 토크");
            force_torque.Nodes.Add("태스크 외력(베이스)");
            force_torque.Nodes.Add("태스크 외력(월드)");
            force_torque.Nodes.Add("태스크 외력(사용자)");

            TreeNode control_info = new TreeNode("제어 정보");
            control_info.Nodes.Add("운용 속도 모드");
            control_info.Nodes.Add("제어 상태");
            control_info.Nodes.Add("툴 무게 설정");
            control_info.Nodes.Add("TCP설정");
            control_info.Nodes.Add("충돌 민감도");
            control_info.Nodes.Add("특이점");

            TreeNode gpio = new TreeNode("IO");
            gpio.Nodes.Add("플렌지 DI");
            gpio.Nodes.Add("플렌지 DO");
            gpio.Nodes.Add("디지털 입력");
            gpio.Nodes.Add("디지털 출력");

            TreeNode etc = new TreeNode("기타");
            etc.Nodes.Add("모터 전류");
            etc.Nodes.Add("인버터 온도");
            etc.Nodes.Add("제어 모드");
            etc.Nodes.Add("제어 공간");
            etc.Nodes.Add("DRCF 상태");
            etc.Nodes.Add("DRCL 상태");
            etc.Nodes.Add("브레이크 상태");
            etc.Nodes.Add("로봇암 버튼 상태");
            etc.Nodes.Add("스위치 상태");

            svrNode.Nodes.Add(jointSpace);
            svrNode.Nodes.Add(taskSpace_base);
            svrNode.Nodes.Add(taskSpace_world);
            svrNode.Nodes.Add(taskSpace_user);
            svrNode.Nodes.Add(force_torque);
            svrNode.Nodes.Add(control_info);
            svrNode.Nodes.Add(gpio);
            svrNode.Nodes.Add(etc);

            treeViewRobotMonitoring.Nodes.Add(svrNode);
            treeViewRobotMonitoring.ExpandAll();

            Robot_Init_DataGridView(svrNode, ref dataGridViewRobotInfo);
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

            dgv.ColumnCount = 17;
            dgv.RowCount = treeNode.GetNodeCount(true);
            dgv.ColumnHeadersVisible = true;

            dgv.Columns[0].HeaderText = $"이름";
            dgv.Columns[0].Width = 150;
            dgv.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            for (int ix = 0; ix < dgv.ColumnCount; ix++)
            {
                if (ix >= 1)
                {
                    dgv.Columns[ix].HeaderText = $"{ix:00}";
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

        //데이터그리드뷰에 새로운 데이터를 넣고 리프레쉬 하는 작업
        //private BindingSource _bsPreview = new BindingSource();
        //dgvPreView.DataSource = _bsPreview;
        //dgvPreView.DataSource = typeof(DataTable);
        //dgvPreView.DataSource = dt;
        //dgvPreView.Refresh();
        //_bsPreview.DataSource = typeof(DataTable);
        //_bsPreview.DataSource = dt;
        //dgvPreView.Refresh();

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

     

        /// <summary>
        /// 2023.02.09
        /// UI ::: dataGridViewRobotInfo
        /// </summary>
        public void Robot_Update(Externs.Robot_Modbus_Table.Data accessData)
        {
            switch (accessData.Address)
            {
                case 0://Digital Input
                    for (int i = 0; i < 16; i++)
                    {
                        dataGridViewRobotInfo.Rows[37].Cells[i + 1].Value = (accessData.iData >> i) & 1;
                    }
                    break;

                case 1://Digital Output
                    for (int i = 0; i < 16; i++)
                    {
                        dataGridViewRobotInfo.Rows[38].Cells[i + 1].Value = (accessData.iData >> i) & 1;
                    }
                    break;

                case 4://analog input
                    break;

                case 5://analog input type
                    break;

                case 6://analog input
                    break;

                case 7://analog input type
                    break;

                case 16://analog output
                    break;

                case 17://analog output type
                    break;

                case 18://analog output
                    break;

                case 19://analog output type
                    break;

                case 21://Tool Input
                    for (int i = 0; i < 6; i++)
                    {
                        dataGridViewRobotInfo.Rows[35].Cells[i + 1].Value = (accessData.iData >> i) & 1;
                    }
                    break;

                case 22://Tool Output
                    for (int i = 0; i < 6; i++)
                    {
                        dataGridViewRobotInfo.Rows[36].Cells[i + 1].Value = (accessData.iData >> i) & 1;
                    }
                    break;

                case 256://major version
                    break;

                case 257://minor version
                    break;

                case 258://patch version
                    break;

                case 259://robot state
                    //Console.WriteLine($"{DateTime.Now} >>> {System.Reflection.MethodBase.GetCurrentMethod().Name} ::: {(Externs.Robot_Modbus_Table.RobotState_Ver_1_0)accessData.iData}");
                    break;

                case 260://servo on robot
                    break;

                case 261://emc stopped
                    break;

                case 262://safety stopped
                    break;

                case 263://direct tech button pressed
                    break;

                case 264://power button pressed
                    break;

                case 270://Joint Position + Col Index
                    dataGridViewRobotInfo.Rows[2].Cells[1].Value = accessData.strData;
                    break;

                case 271://Joint Position + Col Index
                    dataGridViewRobotInfo.Rows[2].Cells[2].Value = accessData.strData;
                    break;

                case 272://Joint Position + Col Index
                    dataGridViewRobotInfo.Rows[2].Cells[3].Value = accessData.strData;
                    break;

                case 273://Joint Position + Col Index
                    dataGridViewRobotInfo.Rows[2].Cells[4].Value = accessData.strData;
                    break;

                case 274://Joint Position + Col Index
                    dataGridViewRobotInfo.Rows[2].Cells[5].Value = accessData.strData;
                    break;

                case 275://Joint Position + Col Index
                    dataGridViewRobotInfo.Rows[2].Cells[6].Value = accessData.strData;
                    break;

                case 280://Joint Velocity + Col Index
                    dataGridViewRobotInfo.Rows[3].Cells[1].Value = accessData.strData;
                    break;

                case 281://Joint Velocity + Col Index
                    dataGridViewRobotInfo.Rows[3].Cells[2].Value = accessData.strData;
                    break;

                case 282://Joint Velocity + Col Index
                    dataGridViewRobotInfo.Rows[3].Cells[3].Value = accessData.strData;
                    break;

                case 283://Joint Velocity + Col Index
                    dataGridViewRobotInfo.Rows[3].Cells[4].Value = accessData.strData;
                    break;

                case 284://Joint Velocity + Col Index
                    dataGridViewRobotInfo.Rows[3].Cells[5].Value = accessData.strData;
                    break;

                case 285://Joint Velocity + Col Index
                    dataGridViewRobotInfo.Rows[3].Cells[6].Value = accessData.strData;
                    break;

                case 290://Joint Motor Current + Col Index
                    dataGridViewRobotInfo.Rows[40].Cells[1].Value = accessData.strData;
                    break;

                case 291://Joint Motor Current + Col Index
                    dataGridViewRobotInfo.Rows[40].Cells[2].Value = accessData.strData;
                    break;

                case 292://Joint Motor Current + Col Index
                    dataGridViewRobotInfo.Rows[40].Cells[3].Value = accessData.strData;
                    break;

                case 293://Joint Motor Current + Col Index
                    dataGridViewRobotInfo.Rows[40].Cells[4].Value = accessData.strData;
                    break;

                case 294://Joint Motor Current + Col Index
                    dataGridViewRobotInfo.Rows[40].Cells[5].Value = accessData.strData;
                    break;

                case 295://Joint Motor Current + Col Index
                    dataGridViewRobotInfo.Rows[40].Cells[6].Value = accessData.strData;
                    break;

                case 300://Joint Motor Temperature + Col Index
                    dataGridViewRobotInfo.Rows[41].Cells[1].Value = accessData.strData;
                    break;

                case 301://Joint Motor Temperature + Col Index
                    dataGridViewRobotInfo.Rows[41].Cells[2].Value = accessData.strData;
                    break;

                case 302://Joint Motor Temperature + Col Index
                    dataGridViewRobotInfo.Rows[41].Cells[3].Value = accessData.strData;
                    break;

                case 303://Joint Motor Temperature + Col Index
                    dataGridViewRobotInfo.Rows[41].Cells[4].Value = accessData.strData;
                    break;

                case 304://Joint Motor Temperature + Col Index
                    dataGridViewRobotInfo.Rows[41].Cells[5].Value = accessData.strData;
                    break;

                case 305://Joint Motor Temperature + Col Index
                    dataGridViewRobotInfo.Rows[41].Cells[6].Value = accessData.strData;
                    break;

                case 310://Joint Torque + Col Index
                    dataGridViewRobotInfo.Rows[18].Cells[1].Value = accessData.strData;
                    break;

                case 311://Joint Torque + Col Index
                    dataGridViewRobotInfo.Rows[18].Cells[2].Value = accessData.strData;
                    break;

                case 312://Joint Torque + Col Index
                    dataGridViewRobotInfo.Rows[18].Cells[3].Value = accessData.strData;
                    break;

                case 313://Joint Torque + Col Index
                    dataGridViewRobotInfo.Rows[18].Cells[4].Value = accessData.strData;
                    break;

                case 314://Joint Torque + Col Index
                    dataGridViewRobotInfo.Rows[18].Cells[5].Value = accessData.strData;
                    break;

                case 315://Joint Torque + Col Index
                    dataGridViewRobotInfo.Rows[18].Cells[6].Value = accessData.strData;
                    break;

                case 400://Task Position + Col Index
                    dataGridViewRobotInfo.Rows[5].Cells[1].Value = accessData.strData;
                    break;

                case 401://Task Position + Col Index
                    dataGridViewRobotInfo.Rows[5].Cells[2].Value = accessData.strData;
                    break;

                case 402://Task Position + Col Index
                    dataGridViewRobotInfo.Rows[5].Cells[3].Value = accessData.strData;
                    break;

                case 403://Task Position + Col Index
                    dataGridViewRobotInfo.Rows[5].Cells[4].Value = accessData.strData;
                    break;

                case 404://Task Position + Col Index
                    dataGridViewRobotInfo.Rows[5].Cells[5].Value = accessData.strData;
                    break;

                case 405://Task Position + Col Index
                    dataGridViewRobotInfo.Rows[5].Cells[6].Value = accessData.strData;
                    break;

                case 410://Task Velocity + Col Index
                    dataGridViewRobotInfo.Rows[6].Cells[1].Value = accessData.strData;
                    break;

                case 411://Task Velocity + Col Index
                    dataGridViewRobotInfo.Rows[6].Cells[2].Value = accessData.strData;
                    break;

                case 412://Task Velocity + Col Index
                    dataGridViewRobotInfo.Rows[6].Cells[3].Value = accessData.strData;
                    break;

                case 413://Task Velocity + Col Index
                    dataGridViewRobotInfo.Rows[6].Cells[4].Value = accessData.strData;
                    break;

                case 414://Task Velocity + Col Index
                    dataGridViewRobotInfo.Rows[6].Cells[5].Value = accessData.strData;
                    break;

                case 415://Task Velocity + Col Index
                    dataGridViewRobotInfo.Rows[6].Cells[6].Value = accessData.strData;
                    break;

                case 430://Task External Force + Col Index
                    dataGridViewRobotInfo.Rows[24].Cells[1].Value = accessData.strData;
                    dataGridViewRobotInfo.Rows[25].Cells[1].Value = accessData.strData;
                    dataGridViewRobotInfo.Rows[26].Cells[1].Value = accessData.strData;
                    break;

                case 431://Task External Force + Col Index
                    dataGridViewRobotInfo.Rows[24].Cells[2].Value = accessData.strData;
                    dataGridViewRobotInfo.Rows[25].Cells[2].Value = accessData.strData;
                    dataGridViewRobotInfo.Rows[26].Cells[2].Value = accessData.strData;
                    break;

                case 432://Task External Force + Col Index
                    dataGridViewRobotInfo.Rows[24].Cells[3].Value = accessData.strData;
                    dataGridViewRobotInfo.Rows[25].Cells[3].Value = accessData.strData;
                    dataGridViewRobotInfo.Rows[26].Cells[3].Value = accessData.strData;
                    break;

                case 433://Task External Force + Col Index
                    dataGridViewRobotInfo.Rows[24].Cells[4].Value = accessData.strData;
                    dataGridViewRobotInfo.Rows[25].Cells[4].Value = accessData.strData;
                    dataGridViewRobotInfo.Rows[26].Cells[4].Value = accessData.strData;
                    break;

                case 434://Task External Force + Col Index
                    dataGridViewRobotInfo.Rows[24].Cells[5].Value = accessData.strData;
                    dataGridViewRobotInfo.Rows[25].Cells[5].Value = accessData.strData;
                    dataGridViewRobotInfo.Rows[26].Cells[5].Value = accessData.strData;
                    break;

                case 435://Task External Force + Col Index
                    dataGridViewRobotInfo.Rows[24].Cells[6].Value = accessData.strData;
                    dataGridViewRobotInfo.Rows[25].Cells[6].Value = accessData.strData;
                    dataGridViewRobotInfo.Rows[26].Cells[6].Value = accessData.strData;
                    break;

                case 420://Tool offset + Col Index
                    dataGridViewRobotInfo.Rows[13].Cells[1].Value = accessData.strData;
                    break;

                case 421://Tool offset + Col Index
                    dataGridViewRobotInfo.Rows[13].Cells[2].Value = accessData.strData;
                    break;

                case 422://Tool offset + Col Index
                    dataGridViewRobotInfo.Rows[13].Cells[3].Value = accessData.strData;
                    break;

                case 423://Tool offset + Col Index
                    dataGridViewRobotInfo.Rows[13].Cells[4].Value = accessData.strData;
                    break;

                case 424://Tool offset + Col Index
                    dataGridViewRobotInfo.Rows[13].Cells[5].Value = accessData.strData;
                    break;

                case 425://Tool offset + Col Index
                    dataGridViewRobotInfo.Rows[13].Cells[6].Value = accessData.strData;
                    break;


                default:
                    break;
            }
        }


        #endregion

        private void buttonRegsitorSet_Click(object sender, EventArgs e)
        {
            RobotSenderEvent(sender, e);
        }

        private void gpioOutput(object sender, EventArgs e)
        {
            RobotSenderEvent(sender, e);
        }

        private void toolOutput(object sender, EventArgs e)
        {
            RobotSenderEvent(sender, e);
        }

        private void buttonTestSet_Click(object sender, EventArgs e)
        {
            //RobotSenderEvent(sender, e);
        }
    }
}
