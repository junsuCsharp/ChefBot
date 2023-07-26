using devJace.Files;
using MetroFramework.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Cores;
using System.Windows.Media;
using static Cores.Core_StepModule;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Threading;
using System.Windows.Interop;
using System.Web.UI.WebControls;
using devi;

namespace Forms
{
    public partial class FormXaxis : Form
    {
        public FormXaxis()
        {
            InitializeComponent();
        }

        public FormXaxis(Project_Main.FormMain mainForm)
        {
            InitializeComponent();

            main = mainForm;
        }
        private Project_Main.FormMain main;

        public List<Control> lstFastechControls = new List<Control>();
        public List<LBSoft.IndustrialCtrls.Leds.LBLed> leds = new List<LBSoft.IndustrialCtrls.Leds.LBLed>();
        private DateTime dtUpdate = DateTime.Now;
        private TimeSpan tsUpdate = new TimeSpan(0, 0, 0, 0, 500);
        const int nFasBdNumber = 0;

        int iXaxisSpeed = 50;
        int iXaxisAccDec = 150;
        double dCommandPos = 0;
        double dCmdPlusMinusPos = 0;
        double dOffsetPos = 0;


        const int iXaxisAccDecMax = 750;
        const int iXaxisAccDecMin = 200;

        //2023.03.20 ::: 수동화면이 아닐 경우 모드버스 메뉴얼 동작 초기화
        public int iMyPageNumber = 0;

        List<System.Windows.Forms.RadioButton> lstMovePos = new List<System.Windows.Forms.RadioButton>();
        List<System.Windows.Forms.Button> lstRobotMove = new List<System.Windows.Forms.Button>();

        //코봇 메뉴얼 동작 확인 용
        public bool IsCobotManual = false;
        int nCobotManualNumber = -1;

        Stopwatch swManualErrorTime = new Stopwatch();
        TimeSpan tsManualOperTime;
        DateTime dtManualOperTime;

        bool IsCobotAutoTest = false;

        //2023.04.12 ::: 소리 재생 전달 
        public RobotSoundEventHandler RobotSoundEventEvent;
        public delegate void RobotSoundEventHandler(int index);


        private void FormXaxis_Load(object sender, EventArgs e)
        {

            //string pos_file_Path = $"{Application.StartupPath}\\whdmd_xos.xml";
            //if (Xml.Load(pos_file_Path, ref _Pos_File))
            //{
            //
            //}
            //else
            //{
            //    _Pos_File.iMaxPosition = 6;
            //    for (int i = 0; i < _Pos_File.iMaxPosition; i++)
            //    {
            //        _Pos_File.lstPositions.Add(0);
            //        _Pos_File.lstRealPositions.Add(0);
            //    }
            //    Xml.Save(pos_file_Path, _Pos_File);
            //}


            //2023.03.13 ::: 디버그용 표시
            bool IsVisible = false;
            checkBox1.Visible = IsVisible;
            textBox1.Visible = IsVisible;
            button2.Visible = IsVisible;
            button4.Visible = IsVisible;
            labelRobotVersion.Text = null;

            button1.Visible = Define.IsSupervisor;
            button3.Visible = Define.IsSupervisor;
            button5.Visible = Define.IsSupervisor;


            //button3.Visible = IsVisible;//얼라인계산
            //button1.Visible = IsVisible;//적용

            #region MyRegion
            //Fastech
            lstFastechControls.Add(buttonServoOn);
            lstFastechControls.Add(buttonServoOff);
            lstFastechControls.Add(buttonAlarmReset);
            lstFastechControls.Add(buttonStop);
            lstFastechControls.Add(buttonJonAbs);
            lstFastechControls.Add(buttonJonMinus);
            lstFastechControls.Add(buttonJogMinus);
            lstFastechControls.Add(buttonJogPlus);
            lstFastechControls.Add(buttonJonPlus);
            lstFastechControls.Add(buttonOrg);

            //LBSoft.IndustrialCtrls.Leds.LBLed
            leds.Add(lbLedAlarm);
            leds.Add(lbLedHwPlusLimit);
            leds.Add(lbLedHwMinusLimit);
            leds.Add(lbLedSwPlusLimit);
            leds.Add(lbLedSwMinusLimit);
            leds.Add(lbLedEmgStop);
            leds.Add(lbLedSlowStop);
            leds.Add(lbLedOrgReturning);
            leds.Add(lbLedInposition);
            leds.Add(lbLedServoOn);
            leds.Add(lbLedAlarmReset);
            leds.Add(lbLedPTStopped);
            leds.Add(lbLedOriginSensor);
            leds.Add(lbLedZPulse);
            leds.Add(lbLedOrgRetOk);
            leds.Add(lbLedMotionDIR);
            leds.Add(lbLedMotioning);
            leds.Add(lbLedMotionPause);
            leds.Add(lbLedMotionAccel);
            leds.Add(lbLedMotionDecel);
            leds.Add(lbLedMotionConst);


            lstMovePos.Add(radioButton1);
            lstMovePos.Add(radioButton2);
            lstMovePos.Add(radioButton3);
            lstMovePos.Add(radioButton4);
            lstMovePos.Add(radioButton5);
            lstMovePos.Add(radioButton6);
            lstMovePos.Add(radioButton7);
            lstMovePos.Add(radioButton8);
            lstMovePos.Add(radioButton9);
            lstMovePos.Add(radioButton10);

            lstRobotMove.Add(buttonRobotOrg);
            lstRobotMove.Add(buttonRobotDown);
            lstRobotMove.Add(buttonGripperOpen);
            lstRobotMove.Add(buttonGripperClose);
            lstRobotMove.Add(buttonRobotUp);
            lstRobotMove.Add(buttonAlignLeft);
            lstRobotMove.Add(buttonToolChange);
            lstRobotMove.Add(buttonAlignRight);
            lstRobotMove.Add(buttonUnUsedMove);
            lstRobotMove.Add(buttonUsedMove);

            #endregion            

            DefaultGUI();
            Xaxis_Init_DataGridView(Cores.Core_Object.GetPos_File);
            Robot_Init_DataGridView();
            XaxisState_Init_DataGridView();


        }

        void DefaultGUI()
        {

            this.Font = new Font(Fonts.FontLibrary.Families[0], 10);

            lbButton1.Font = new Font("Arial", 25, FontStyle.Bold);

            foreach (LBSoft.IndustrialCtrls.Leds.LBLed lds in leds)
            {
                lds.Font = new Font("Arial", 10);
            }


            int fontSize = 9;


            for (int i = 0; i < lstMovePos.Count; i++)
            {
                lstMovePos[i].Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            }

            for (int i = 0; i < lstRobotMove.Count; i++)
            {
                lstRobotMove[i].Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            }

            for (int i = 0; i < lstFastechControls.Count; i++)
            {
                lstFastechControls[i].Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            }
        }

        void Xaxis_Init_DataGridView(Pos_File pos)
        {
            try
            {
                DataGridView dgv = dataGridViewPositions;

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

                dgv.ColumnCount = 11;
                dgv.RowCount = 3;
                dgv.ColumnHeadersVisible = true;

                dgv.Columns[0].HeaderText = $"* 위치 값";
                dgv.Columns[0].Width = 120;
                dgv.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                string[] headers = new string[] { "Wait", "Load A", "Load B", "Load C", "Cooker 1-1", "Cooker 1-2", "Cooker 2-1", "Cooker 2-2", "Cooker 3-1", "Cooker 3-2" };
                for (int ix = 0; ix < dgv.ColumnCount; ix++)
                {
                    if (ix >= 1)
                    {
                        dgv.Columns[ix].HeaderText = $"{headers[ix - 1]}";
                        lstMovePos[ix - 1].Text = $"{headers[ix - 1]}";
                        dgv.Columns[ix].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        dgv.Columns[ix].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        if (pos.lstRealPositions.Count != 0)
                        {
                            dgv.Rows[0].Cells[ix].Value = pos.lstRealPositions[ix - 1].ToString("0.00");
                        }
                        else
                        {
                            dgv.Rows[0].Cells[ix].Value = 0;
                        }
                        if (pos.lstPositions.Count != 0)
                        {
                            dgv.Rows[1].Cells[ix].Value = pos.lstPositions[ix - 1].ToString();
                        }
                        else
                        {
                            dgv.Rows[1].Cells[ix].Value = 0;
                        }

                    }

                    dgv.Columns[ix].Resizable = DataGridViewTriState.False;
                    dgv.Columns[ix].SortMode = DataGridViewColumnSortMode.NotSortable;

                    dgv.Rows[0].Cells[ix].Style.BackColor = System.Drawing.Color.AliceBlue;
                    dgv.Rows[1].Cells[ix].Style.BackColor = System.Drawing.Color.Gray;
                }

                dgv.Rows[0].Cells[0].Value = "실제 위치";
                dgv.Rows[1].Cells[0].Value = "장치 위치";

                dgv.AllowUserToAddRows = false;

                dgv.ClearSelection();
            }
            catch { }
            
        }

        void Robot_Init_DataGridView()
        {
            System.Windows.Forms.TreeNode svrNode = new System.Windows.Forms.TreeNode("두산 협동 로봇");

            System.Windows.Forms.TreeNode jointSpace = new System.Windows.Forms.TreeNode("조인트 공간");
            jointSpace.Nodes.Add("현재 각도");
            jointSpace.Nodes.Add("현재 속도");

            System.Windows.Forms.TreeNode taskSpace_base = new System.Windows.Forms.TreeNode("태스크 공간(베이스)");
            taskSpace_base.Nodes.Add("현재 좌표");
            taskSpace_base.Nodes.Add("현재 속도");
            //taskSpace_base.Nodes.Add("현재 TCP속도");

            //TreeNode taskSpace_world = new TreeNode("태스크 공간(월드)");
            //taskSpace_world.Nodes.Add("현재 좌표");
            //taskSpace_world.Nodes.Add("현재 속도");
            //taskSpace_world.Nodes.Add("월드/베이스 관계");

            System.Windows.Forms.TreeNode taskSpace_user = new System.Windows.Forms.TreeNode("태스크 공간(사용자)");
            //TreeNode taskSpace_user = new TreeNode("툴 옵셋");
            taskSpace_user.Nodes.Add("툴 옵셋");
            //taskSpace_user.Nodes.Add("현재 속도");
            //taskSpace_user.Nodes.Add("사용자 좌표게 ID");
            //taskSpace_user.Nodes.Add("부모 좌표계");

            System.Windows.Forms.TreeNode force_torque = new System.Windows.Forms.TreeNode("힘/토크");
            force_torque.Nodes.Add("조인트 토크(센서)");
            //force_torque.Nodes.Add("모터 토크");
            //force_torque.Nodes.Add("힘토크 센서");
            //force_torque.Nodes.Add("가속도 센서");
            //force_torque.Nodes.Add("조이트 토크(중력/모델)");
            //force_torque.Nodes.Add("조인트 외력 토크");
            force_torque.Nodes.Add("태스크 외력(베이스)");
            force_torque.Nodes.Add("태스크 외력(월드)");
            force_torque.Nodes.Add("태스크 외력(사용자)");

            //TreeNode control_info = new TreeNode("제어 정보");
            //control_info.Nodes.Add("운용 속도 모드");
            //control_info.Nodes.Add("제어 상태");
            //control_info.Nodes.Add("툴 무게 설정");
            //control_info.Nodes.Add("TCP설정");
            //control_info.Nodes.Add("충돌 민감도");
            //control_info.Nodes.Add("특이점");

            System.Windows.Forms.TreeNode gpio = new System.Windows.Forms.TreeNode("IO");
            gpio.Nodes.Add("플렌지 DI");
            gpio.Nodes.Add("플렌지 DO");
            //gpio.Nodes.Add("디지털 입력");
            //gpio.Nodes.Add("디지털 출력");

            System.Windows.Forms.TreeNode etc = new System.Windows.Forms.TreeNode("기타");
            etc.Nodes.Add("모터 전류");
            etc.Nodes.Add("인버터 온도");
            //etc.Nodes.Add("제어 모드");
            //etc.Nodes.Add("제어 공간");
            //etc.Nodes.Add("DRCF 상태");
            //etc.Nodes.Add("DRCL 상태");
            //etc.Nodes.Add("브레이크 상태");
            //etc.Nodes.Add("로봇암 버튼 상태");
            //etc.Nodes.Add("스위치 상태");

            svrNode.Nodes.Add(jointSpace);
            svrNode.Nodes.Add(taskSpace_base);
            //svrNode.Nodes.Add(taskSpace_world);
            svrNode.Nodes.Add(taskSpace_user);
            svrNode.Nodes.Add(force_torque);
            //svrNode.Nodes.Add(control_info);
            svrNode.Nodes.Add(gpio);
            svrNode.Nodes.Add(etc);

            DataGridView dgv = dataGridViewRobotInfo;

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

            dgv.ColumnCount = 7;
            dgv.RowCount = svrNode.GetNodeCount(true) + 2;
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
            List<System.Windows.Forms.TreeNode> treeNodes = GetAllNodes(svrNode);
            string strNodeHeader = null;
            for (int ux = 0; ux < dgv.RowCount - 2; ux++)
            {
                dgv.Rows[ux].Cells[0].Value = treeNodes[ux].Text;
                if (ux == 0)
                {
                    strNodeHeader = treeNodes[ux].Text;
                    dgv.Rows[ux].HeaderCell.Style.BackColor = System.Drawing.Color.DarkGray;
                    dgv.Rows[ux].Cells[0].Style.BackColor = System.Drawing.Color.Gray;
                }

                if (treeNodes[ux].Parent != null)
                {
                    if (strNodeHeader.Contains(treeNodes[ux].Parent.Text))
                    {
                        dgv.Rows[ux].HeaderCell.Style.BackColor = System.Drawing.Color.DarkGray;
                        dgv.Rows[ux].Cells[0].Style.BackColor = System.Drawing.Color.AliceBlue;
                    }
                    else
                    {
                        //dgv.Rows[ux].Cells[0].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }
                //Console.WriteLine($"DEBUG ::: {ux} / {treeNodes[ux].Checked} / {treeNodes[ux].Index} / {treeNodes[ux].Text} / {treeNodes[ux].Name} / {treeNodes[ux].Parent}");
            }

            dgv.Rows[19].Cells[0].Value = "인버터 온도";

            dgv.AllowUserToAddRows = false;
        }

        void XaxisState_Init_DataGridView()
        {
            try
            {
                DataGridView dgv = dataGridViewXaxis;

                dgv.ReadOnly = true;
                dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // row 로 선택하기
                dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None; //row size 막기
                dgv.AllowUserToResizeRows = false; //row size 막기
                dgv.AllowUserToResizeColumns = false; //column size 막기
                dgv.MultiSelect = false;
                dgv.RowHeadersVisible = true;
                dgv.RowHeadersWidth = 15;
                dgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
                dgv.AllowUserToAddRows = false;
                dgv.Rows.Clear();
                dgv.Columns.Clear();

                dgv.ColumnHeadersHeight = 40;
                dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

                string[] headers = new string[] { "위치", "현재 값", "설정 값", "범위", "단위" };
                string[] rowheaders = new string[] { "명령 위치", "실제 위치", "에러 위치", "가감 위치", "가감속도", "이동 속도", "원점 옵셋 위치" };
                string[] rowheadersRanges = new string[] { "-10 ~ 1300", "-10 ~ 1300", "2", "0 ~ 100", "0~1000", "0~250", "-" };
                string[] rowheadersUnits = new string[] { "mm", "mm", "mm", "mm", "mm/sec", "mm/sec", "mm" };
                string[] rowheadersSets = new string[] { "0", "Read Only", "Read Only", "10", "150", "50", "-" };
                dgv.ColumnCount = headers.Length;
                dgv.RowCount = rowheaders.Length;
                dgv.ColumnHeadersVisible = true;

                dgv.Columns[0].HeaderText = $"이름";
                dgv.Columns[0].Width = 300;
                dgv.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                for (int ix = 0; ix < dgv.ColumnCount; ix++)
                {
                    if (ix >= 1)
                    {
                        dgv.Columns[ix].HeaderText = $"{headers[ix]}";
                        dgv.Columns[ix].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        dgv.Columns[ix].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    }

                    dgv.Columns[ix].Resizable = DataGridViewTriState.False;
                    dgv.Columns[ix].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                int fontSize = 11;
                for (int ux = 0; ux < dgv.RowCount; ux++)
                {
                    dgv.Rows[ux].Cells[0].Value = rowheaders[ux];

                    dgv.Rows[ux].Cells[2].Value = rowheadersSets[ux];
                    dgv.Rows[ux].Cells[3].Value = rowheadersRanges[ux];
                    dgv.Rows[ux].Cells[4].Value = rowheadersUnits[ux];

                    if (ux % 2 == 0)
                    {

                        dgv.Rows[ux].HeaderCell.Style.BackColor = System.Drawing.Color.DarkGray;
                        dgv.Rows[ux].Cells[0].Style.BackColor = System.Drawing.Color.White;
                    }
                    else
                    {
                        dgv.Rows[ux].HeaderCell.Style.BackColor = System.Drawing.Color.DarkGray;
                        dgv.Rows[ux].Cells[0].Style.BackColor = System.Drawing.Color.AliceBlue;
                    }

                    dgv.Rows[ux].Height = 40;
                    for (int col = 0; col < dgv.Columns.Count; col++)
                    {
                        dgv.Rows[ux].Cells[col].Style.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
                    }
                }
                dgv.ClearSelection();
            }
            catch { }
           
        }

        // 트리뷰에서 노드 하나 선택시에 모든 자식 노드를 가져오는 함수
        public static List<System.Windows.Forms.TreeNode> GetAllNodes(System.Windows.Forms.TreeNode node)
        {
            List<System.Windows.Forms.TreeNode> result = new List<System.Windows.Forms.TreeNode>();
            result.Add(node);
            foreach (System.Windows.Forms.TreeNode child in node.Nodes)
            {
                result.AddRange(GetAllNodes(child));
            }
            return result;

        }

        // 트리뷰의 전체 노드
        public static List<System.Windows.Forms.TreeNode> GetAllNodes(System.Windows.Forms.TreeView treeView)
        {
            List<System.Windows.Forms.TreeNode> result = new List<System.Windows.Forms.TreeNode>();
            foreach (System.Windows.Forms.TreeNode child in treeView.Nodes)
            {
                result.AddRange(GetAllNodes(child));
            }
            return result;
        }

        public void Robot_Verstion(string str)
        {
            labelRobotVersion.Text = $"Version {str}";
        }

        /// <summary>
        /// 2023.02.20
        /// UI ::: dataGridViewRobotInfo
        /// </summary>
        public void Robot_Update(Externs.Robot_Modbus_Table.Data accessData)
        {

            const int RowToolOffset = 8;
            const int RowMotorCurrent = 18;
            const int RowMotorTemperature = 19;
            const int RowMotorForce = 11;
            const int RowMotorTorque = 10;

            switch (accessData.Address)
            {
                case 0://Digital Input
                    //if (eSelectedData == RB_Data.RoBot_Digital_IO)
                    //{
                    //    for (int i = 0; i < 16; i++)
                    //    {
                    //        if (((accessData.iData >> i) & 1) != 0)
                    //        {
                    //            Inputs[i].BackgroundImage = global::Kyuchon_Robot.Properties.Resources.InputLedBlue;
                    //        }
                    //        else
                    //        {
                    //            Inputs[i].BackgroundImage = global::Kyuchon_Robot.Properties.Resources.LED_Gray;
                    //        }
                    //    }
                    //}
                    break;

                case 1://Digital Output
                    //if (eSelectedData == RB_Data.RoBot_Digital_IO)
                    //{
                    //    for (int i = 0; i < 16; i++)
                    //    {
                    //        if (((accessData.iData >> i) & 1) != 0)
                    //        {
                    //            Outputs[i].BackgroundImage = global::Kyuchon_Robot.Properties.Resources.OutputLedOragne;
                    //        }
                    //        else
                    //        {
                    //            Outputs[i].BackgroundImage = global::Kyuchon_Robot.Properties.Resources.LED_Gray;
                    //        }
                    //    }
                    //}
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
                        dataGridViewRobotInfo.Rows[15].Cells[i + 1].Value = (accessData.iData >> i) & 1;
                    }
                    break;

                case 22://Tool Output
                    for (int i = 0; i < 6; i++)
                    {
                        dataGridViewRobotInfo.Rows[16].Cells[i + 1].Value = (accessData.iData >> i) & 1;
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
                         //labelRobotState.Text = $"Robot State ::: { (Externs.Robot_Modbus_Table.RobotState_Ver_1_0)accessData.iData}";
                    labelRobotState.Text = $"Robot State ::: {(Externs.Robot_Modbus_Table.RobotState_Ver_1_1)accessData.iData}";
                    break;

                case 260://servo on robot
                    //Console.WriteLine();
                    break;

                case 261://emc stopped
                         // Console.WriteLine();
                    break;

                case 262://safety stopped
                    //Console.WriteLine();
                    break;

                case 263://direct tech button pressed
                    //Console.WriteLine();
                    break;

                case 264://power button pressed
                    //Console.WriteLine();
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
                    dataGridViewRobotInfo.Rows[RowMotorCurrent].Cells[1].Value = accessData.strData;
                    break;

                case 291://Joint Motor Current + Col Index
                    dataGridViewRobotInfo.Rows[RowMotorCurrent].Cells[2].Value = accessData.strData;
                    break;

                case 292://Joint Motor Current + Col Index
                    dataGridViewRobotInfo.Rows[RowMotorCurrent].Cells[3].Value = accessData.strData;
                    break;

                case 293://Joint Motor Current + Col Index
                    dataGridViewRobotInfo.Rows[RowMotorCurrent].Cells[4].Value = accessData.strData;
                    break;

                case 294://Joint Motor Current + Col Index
                    dataGridViewRobotInfo.Rows[RowMotorCurrent].Cells[5].Value = accessData.strData;
                    break;

                case 295://Joint Motor Current + Col Index
                    dataGridViewRobotInfo.Rows[RowMotorCurrent].Cells[6].Value = accessData.strData;
                    break;

                case 300://Joint Motor Temperature + Col Index
                    dataGridViewRobotInfo.Rows[RowMotorTemperature].Cells[1].Value = accessData.strData;
                    break;

                case 301://Joint Motor Temperature + Col Index
                    dataGridViewRobotInfo.Rows[RowMotorTemperature].Cells[2].Value = accessData.strData;
                    break;

                case 302://Joint Motor Temperature + Col Index
                    dataGridViewRobotInfo.Rows[RowMotorTemperature].Cells[3].Value = accessData.strData;
                    break;

                case 303://Joint Motor Temperature + Col Index
                    dataGridViewRobotInfo.Rows[RowMotorTemperature].Cells[4].Value = accessData.strData;
                    break;

                case 304://Joint Motor Temperature + Col Index
                    dataGridViewRobotInfo.Rows[RowMotorTemperature].Cells[5].Value = accessData.strData;
                    break;

                case 305://Joint Motor Temperature + Col Index
                    dataGridViewRobotInfo.Rows[RowMotorTemperature].Cells[6].Value = accessData.strData;
                    break;

                case 310://Joint Torque + Col Index
                    dataGridViewRobotInfo.Rows[RowMotorTorque].Cells[1].Value = accessData.strData;
                    break;

                case 311://Joint Torque + Col Index
                    dataGridViewRobotInfo.Rows[RowMotorTorque].Cells[2].Value = accessData.strData;
                    break;

                case 312://Joint Torque + Col Index
                    dataGridViewRobotInfo.Rows[RowMotorTorque].Cells[3].Value = accessData.strData;
                    break;

                case 313://Joint Torque + Col Index
                    dataGridViewRobotInfo.Rows[RowMotorTorque].Cells[4].Value = accessData.strData;
                    break;

                case 314://Joint Torque + Col Index
                    dataGridViewRobotInfo.Rows[RowMotorTorque].Cells[5].Value = accessData.strData;
                    break;

                case 315://Joint Torque + Col Index
                    dataGridViewRobotInfo.Rows[RowMotorTorque].Cells[6].Value = accessData.strData;
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
                    dataGridViewRobotInfo.Rows[RowMotorForce + 0].Cells[1].Value = accessData.strData;
                    dataGridViewRobotInfo.Rows[RowMotorForce + 1].Cells[1].Value = accessData.strData;
                    dataGridViewRobotInfo.Rows[RowMotorForce + 2].Cells[1].Value = accessData.strData;
                    break;

                case 431://Task External Force + Col Index
                    dataGridViewRobotInfo.Rows[RowMotorForce + 0].Cells[2].Value = accessData.strData;
                    dataGridViewRobotInfo.Rows[RowMotorForce + 1].Cells[2].Value = accessData.strData;
                    dataGridViewRobotInfo.Rows[RowMotorForce + 2].Cells[2].Value = accessData.strData;
                    break;

                case 432://Task External Force + Col Index
                    dataGridViewRobotInfo.Rows[RowMotorForce + 0].Cells[3].Value = accessData.strData;
                    dataGridViewRobotInfo.Rows[RowMotorForce + 1].Cells[3].Value = accessData.strData;
                    dataGridViewRobotInfo.Rows[RowMotorForce + 2].Cells[3].Value = accessData.strData;
                    break;

                case 433://Task External Force + Col Index
                    dataGridViewRobotInfo.Rows[RowMotorForce + 0].Cells[4].Value = accessData.strData;
                    dataGridViewRobotInfo.Rows[RowMotorForce + 1].Cells[4].Value = accessData.strData;
                    dataGridViewRobotInfo.Rows[RowMotorForce + 2].Cells[4].Value = accessData.strData;
                    break;

                case 434://Task External Force + Col Index
                    dataGridViewRobotInfo.Rows[RowMotorForce + 0].Cells[5].Value = accessData.strData;
                    dataGridViewRobotInfo.Rows[RowMotorForce + 1].Cells[5].Value = accessData.strData;
                    dataGridViewRobotInfo.Rows[RowMotorForce + 2].Cells[5].Value = accessData.strData;
                    break;

                case 435://Task External Force + Col Index
                    dataGridViewRobotInfo.Rows[RowMotorForce + 0].Cells[6].Value = accessData.strData;
                    dataGridViewRobotInfo.Rows[RowMotorForce + 1].Cells[6].Value = accessData.strData;
                    dataGridViewRobotInfo.Rows[RowMotorForce + 2].Cells[6].Value = accessData.strData;
                    break;

                case 420://Tool offset + Col Index
                    dataGridViewRobotInfo.Rows[RowToolOffset].Cells[1].Value = accessData.strData;
                    break;

                case 421://Tool offset + Col Index
                    dataGridViewRobotInfo.Rows[RowToolOffset].Cells[2].Value = accessData.strData;
                    break;

                case 422://Tool offset + Col Index
                    dataGridViewRobotInfo.Rows[RowToolOffset].Cells[3].Value = accessData.strData;
                    break;

                case 423://Tool offset + Col Index
                    dataGridViewRobotInfo.Rows[RowToolOffset].Cells[4].Value = accessData.strData;
                    break;

                case 424://Tool offset + Col Index
                    dataGridViewRobotInfo.Rows[RowToolOffset].Cells[5].Value = accessData.strData;
                    break;

                case 425://Tool offset + Col Index
                    dataGridViewRobotInfo.Rows[RowToolOffset].Cells[6].Value = accessData.strData;
                    break;


                default:
                    break;
            }
        }

        public void Xaxis_Update()
        {
            try
            {
                DataGridView dgv = dataGridViewPositions;
                for (int ix = 0; ix < dgv.ColumnCount; ix++)
                {
                    if (ix >= 1)
                    {
                        if (Cores.Core_Object.GetPos_File.lstRealPositions.Count != 0)
                        {
                            dgv.Rows[0].Cells[ix].Value = Cores.Core_Object.GetPos_File.lstRealPositions[ix - 1].ToString("0.00");
                        }
                        else
                        {
                            dgv.Rows[0].Cells[ix].Value = 0;
                        }
                        if (Cores.Core_Object.GetPos_File.lstPositions.Count != 0)
                        {
                            dgv.Rows[1].Cells[ix].Value = Cores.Core_Object.GetPos_File.lstPositions[ix - 1].ToString();
                        }
                        else
                        {
                            dgv.Rows[1].Cells[ix].Value = 0;
                        }

                    }
                }
            }
            catch { }
            
        }       

        public void UpdateInformation(object obj)
        {
            //BeginInvoke(new Cores.Core_Object.MotionDataEventHandler(UpdateInformation));

            //System.Threading.Thread.Sleep(20);

            //Cores.Core_Function function = new Cores.Core_Function();
            //function.Delay(20);

            Cores.Fas_Data _item = obj as Cores.Fas_Data;
            if (this.InvokeRequired)
            {
                //this.BeginInvoke(new Action(() => metroTextBoxCmdPos.Text = $"{Cores.Fas_Func.PPR_To_mm(_item.iCmdPos):0.00}"));
                //this.BeginInvoke(new Action(() => metroTextBoxActPos.Text = $"{Cores.Fas_Func.PPR_To_mm(_item.iActPos):0.00}"));
                //this.BeginInvoke(new Action(() => metroTextBoxPosError.Text = $"{Cores.Fas_Func.PPR_To_mm(_item.iPosError):0.00}"));
                //this.BeginInvoke(new Action(() => metroTextBoxActVel.Text = $"{Cores.Fas_Func.PPR_To_mm(_item.iActVel):0.00}"));
                //this.BeginInvoke(new Action(() => metroTextBoxOrgOffsetPos.Text = $"{Cores.Fas_Func.PPR_To_mm(_item.iHomeOffset):0.00}"));

                //this.BeginInvoke(new Action(() => lbLedAlarm.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Err_Sevo_Alarm] ? 1 : 0)));
                //this.BeginInvoke(new Action(() => lbLedHwPlusLimit.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.HW_Plus_Limit] ? 1 : 0)));
                //this.BeginInvoke(new Action(() => lbLedHwMinusLimit.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.HW_Miuns_Limit] ? 1 : 0)));
                //this.BeginInvoke(new Action(() => lbLedSwPlusLimit.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.SW_Plus_Limit] ? 1 : 0)));
                //this.BeginInvoke(new Action(() => lbLedSwMinusLimit.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.SW_Miuns_Limit] ? 1 : 0)));
                //this.BeginInvoke(new Action(() => lbLedEmgStop.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Emg_Stop] ? 1 : 0)));
                //this.BeginInvoke(new Action(() => lbLedSlowStop.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Slow_Stop] ? 1 : 0)));
                //this.BeginInvoke(new Action(() => lbLedOrgReturning.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Org_Returning] ? 1 : 0)));
                //this.BeginInvoke(new Action(() => lbLedInposition.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Inposition] ? 1 : 0)));
                //this.BeginInvoke(new Action(() => lbLedServoOn.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Servo_On] ? 1 : 0)));
                //this.BeginInvoke(new Action(() => lbLedAlarmReset.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Alarm_Reset] ? 1 : 0)));
                //this.BeginInvoke(new Action(() => lbLedPTStopped.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.PT_Stopped] ? 1 : 0)));
                //this.BeginInvoke(new Action(() => lbLedOriginSensor.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Origin_Sensor] ? 1 : 0)));
                //this.BeginInvoke(new Action(() => lbLedZPulse.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Z_Pulse] ? 1 : 0)));
                //this.BeginInvoke(new Action(() => lbLedOrgRetOk.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Org_Ret_OK] ? 1 : 0)));
                //this.BeginInvoke(new Action(() => lbLedMotionDIR.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Motion_DIR] ? 1 : 0)));
                //this.BeginInvoke(new Action(() => lbLedMotioning.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Motioning] ? 1 : 0)));
                //this.BeginInvoke(new Action(() => lbLedMotionPause.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Motion_Pause] ? 1 : 0)));
                //this.BeginInvoke(new Action(() => lbLedMotionAccel.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Motion_Accel] ? 1 : 0)));
                //this.BeginInvoke(new Action(() => lbLedMotionDecel.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Motion_Decel] ? 1 : 0)));
                //this.BeginInvoke(new Action(() => lbLedMotionConst.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Motion_Constant] ? 1 : 0)));


                if (DateTime.Now >= dtUpdate)
                {
                    tsUpdate = new TimeSpan(0, 0, 0, 0, 990);
                    dtUpdate = DateTime.Now.Add(tsUpdate);
                    this.BeginInvoke(new Action(() => this.Refresh()));
                }


                //this.Invoke(new MethodInvoker(delegate ()
                //{
                //    metroTextBoxCmdPos.Text = $"{Cores.Fas_Func.PPR_To_mm(_item.iCmdPos):0.00}";
                //    metroTextBoxActPos.Text = $"{Cores.Fas_Func.PPR_To_mm(_item.iActPos):0.00}";
                //    metroTextBoxPosError.Text = $"{Cores.Fas_Func.PPR_To_mm(_item.iPosError):0.00}";
                //    metroTextBoxActVel.Text = $"{Cores.Fas_Func.PPR_To_mm(_item.iActVel):0.00}";
                //    metroTextBoxOrgOffsetPos.Text = $"{Cores.Fas_Func.PPR_To_mm(_item.iHomeOffset):0.00}";
                //    //metroTextBoxCmdSpeed.Text = $"{_item.iCmdVel}";

                //    lbLedAlarm.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Err_Sevo_Alarm] ? 1 : 0);
                //    lbLedHwPlusLimit.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.HW_Plus_Limit] ? 1 : 0);
                //    lbLedHwMinusLimit.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.HW_Miuns_Limit] ? 1 : 0);
                //    lbLedSwPlusLimit.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.SW_Plus_Limit] ? 1 : 0);
                //    lbLedSwMinusLimit.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.SW_Miuns_Limit] ? 1 : 0);
                //    lbLedEmgStop.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Emg_Stop] ? 1 : 0);
                //    lbLedSlowStop.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Slow_Stop] ? 1 : 0);
                //    lbLedOrgReturning.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Org_Returning] ? 1 : 0);
                //    lbLedInposition.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Inposition] ? 1 : 0);
                //    lbLedServoOn.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Servo_On] ? 1 : 0);
                //    lbLedAlarmReset.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Alarm_Reset] ? 1 : 0);
                //    lbLedPTStopped.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.PT_Stopped] ? 1 : 0);
                //    lbLedOriginSensor.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Origin_Sensor] ? 1 : 0);
                //    lbLedZPulse.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Z_Pulse] ? 1 : 0);
                //    lbLedOrgRetOk.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Org_Ret_OK] ? 1 : 0);
                //    lbLedMotionDIR.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Motion_DIR] ? 1 : 0);
                //    lbLedMotioning.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Motioning] ? 1 : 0);
                //    lbLedMotionPause.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Motion_Pause] ? 1 : 0);
                //    lbLedMotionAccel.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Motion_Accel] ? 1 : 0);
                //    lbLedMotionDecel.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Motion_Decel] ? 1 : 0);
                //    lbLedMotionConst.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Motion_Constant] ? 1 : 0);

                //    if (metroTabControl1.SelectedIndex == 1)
                //    {
                //        //metroTrackBarVirturlPos

                //        if (_item.lstAxis_State[(int)Cores.EAxis_Status.Org_Returning] == false && _item.lstAxis_State[(int)Cores.EAxis_Status.Org_Ret_OK] == true)
                //        {
                //            double dMax = Math.Abs(_Pos_File.dMinusLimitPos) + _Pos_File.dPlusLimitPos;
                //            double iPos = Cores.Fas_Func.PPR_To_mm(_item.iActPos);
                //            uint iVal = (uint)(iPos / dMax * 100);
                //            if (iVal <= 100 && iVal >= 0)
                //            {
                //                metroTrackBarVirturlPos.Value = (int)iVal;
                //            }
                //        }
                //        else
                //        {
                //            metroTrackBarVirturlPos.Value = 0;
                //        }
                //        this.Refresh();
                //    }


                //}));
            }
            else
            {
                //metroTextBoxCmdPos.Text = $"{_item.iCmdPos}";
                //metroTextBoxActPos.Text = $"{_item.iActPos}";
                //metroTextBoxPosError.Text = $"{_item.iPosError}";
                //metroTextBoxActVel.Text = $"{_item.iActVel}";
                //metroTextBoxOrgOffsetPos.Text = $"{Cores.Fas_Func.PPR_To_mm(_item.iHomeOffset):0.00}";
                ////metroTextBoxCmdSpeed.Text = $"{_item.iCmdVel}";
                //
                //lbLedAlarm.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Err_Sevo_Alarm] ? 1 : 0);
                //lbLedHwPlusLimit.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.HW_Plus_Limit] ? 1 : 0);
                //lbLedHwMinusLimit.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.HW_Miuns_Limit] ? 1 : 0);
                //lbLedSwPlusLimit.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.SW_Plus_Limit] ? 1 : 0);
                //lbLedSwMinusLimit.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.SW_Miuns_Limit] ? 1 : 0);
                //lbLedEmgStop.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Emg_Stop] ? 1 : 0);
                //lbLedSlowStop.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Slow_Stop] ? 1 : 0);
                //lbLedOrgReturning.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Org_Returning] ? 1 : 0);
                //lbLedInposition.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Inposition] ? 1 : 0);
                //lbLedServoOn.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Servo_On] ? 1 : 0);
                //lbLedAlarmReset.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Alarm_Reset] ? 1 : 0);
                //lbLedPTStopped.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.PT_Stopped] ? 1 : 0);
                //lbLedOriginSensor.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Origin_Sensor] ? 1 : 0);
                //lbLedZPulse.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Z_Pulse] ? 1 : 0);
                //lbLedOrgRetOk.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Org_Ret_OK] ? 1 : 0);
                //lbLedMotionDIR.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Motion_DIR] ? 1 : 0);
                //lbLedMotioning.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Motioning] ? 1 : 0);
                //lbLedMotionPause.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Motion_Pause] ? 1 : 0);
                //lbLedMotionAccel.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Motion_Accel] ? 1 : 0);
                //lbLedMotionDecel.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Motion_Decel] ? 1 : 0);
                //lbLedMotionConst.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(_item.lstAxis_State[(int)Cores.EAxis_Status.Motion_Constant] ? 1 : 0);
                //this.Refresh();
            }


            //metroTextBoxAccDel.Text = $"{_item.";
            //metroTextBoxOrgSpeed.Text = $"{_item.iCmdPos}";
            //metroTextBoxOrgDefaultSpeed.Text = $"{_item.iCmdPos}";
            //metroTextBoxOrgAccDel.Text = $"{_item.iCmdPos}";
        }

        public void UpdateInformation()
        {
            DefaultGUI();


            if (lbLedInposition.LedColor != System.Drawing.Color.Lime)
            {
                lbLedInposition.LedColor = System.Drawing.Color.Lime;
            }
            if (lbLedOriginSensor.LedColor != System.Drawing.Color.Lime)
            {
                lbLedOriginSensor.LedColor = System.Drawing.Color.Lime;
            }
            if (lbLedOrgRetOk.LedColor != System.Drawing.Color.Lime)
            {
                lbLedOrgRetOk.LedColor = System.Drawing.Color.Lime;
            }
            if (lbLedPTStopped.LedColor != System.Drawing.Color.Lime)
            {
                lbLedPTStopped.LedColor = System.Drawing.Color.Lime;
            }
            if (lbLedOrgReturning.LedColor != System.Drawing.Color.Lime)
            {
                lbLedOrgReturning.LedColor = System.Drawing.Color.Lime;
            }
            if (lbLedServoOn.LedColor != System.Drawing.Color.Lime)
            {
                lbLedServoOn.LedColor = System.Drawing.Color.Lime;
            }

            //    metroTextBoxCmdPos.Text = $"{Cores.Fas_Func.PPR_To_mm(_item.iCmdPos):0.00}";
            //    metroTextBoxActPos.Text = $"{Cores.Fas_Func.PPR_To_mm(_item.iActPos):0.00}";
            //    metroTextBoxPosError.Text = $"{Cores.Fas_Func.PPR_To_mm(_item.iPosError):0.00}";
            //    metroTextBoxActVel.Text = $"{Cores.Fas_Func.PPR_To_mm(_item.iActVel):0.00}";
            //    metroTextBoxOrgOffsetPos.Text = $"{Cores.Fas_Func.PPR_To_mm(_item.iHomeOffset):0.00}";
            //    //metroTextBoxCmdSpeed.Text = $"{_item.iCmdVel}";

            DataGridView dgv = dataGridViewXaxis;
            dgv.Rows[0].Cells[1].Value = $"{Cores.Fas_Func.PPR_To_mm(Cores.Fas_Data.iCmdPos):0.00}";
            dgv.Rows[1].Cells[1].Value = $"{Cores.Fas_Func.PPR_To_mm(Cores.Fas_Data.iActPos):0.00}";
            dgv.Rows[2].Cells[1].Value = $"{Cores.Fas_Func.PPR_To_mm(Cores.Fas_Data.iPosError):0.00}";
            dgv.Rows[5].Cells[1].Value = $"{Cores.Fas_Func.PPR_To_mm(Cores.Fas_Data.iActVel):0.00}";
            dgv.Rows[6].Cells[1].Value = $"{Cores.Fas_Func.PPR_To_mm(Cores.Fas_Data.iHomeOffset):0.00}";


            lbLedAlarm.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(Cores.Fas_Data.lstAxis_State[(int)Cores.EAxis_Status.Err_Sevo_Alarm] ? 1 : 0);
            lbLedHwPlusLimit.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(Cores.Fas_Data.lstAxis_State[(int)Cores.EAxis_Status.HW_Plus_Limit] ? 1 : 0);
            lbLedHwMinusLimit.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(Cores.Fas_Data.lstAxis_State[(int)Cores.EAxis_Status.HW_Miuns_Limit] ? 1 : 0);
            lbLedSwPlusLimit.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(Cores.Fas_Data.lstAxis_State[(int)Cores.EAxis_Status.SW_Plus_Limit] ? 1 : 0);
            lbLedSwMinusLimit.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(Cores.Fas_Data.lstAxis_State[(int)Cores.EAxis_Status.SW_Miuns_Limit] ? 1 : 0);
            lbLedEmgStop.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(Cores.Fas_Data.lstAxis_State[(int)Cores.EAxis_Status.Emg_Stop] ? 1 : 0);
            lbLedSlowStop.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(Cores.Fas_Data.lstAxis_State[(int)Cores.EAxis_Status.Slow_Stop] ? 1 : 0);
            lbLedOrgReturning.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(Cores.Fas_Data.lstAxis_State[(int)Cores.EAxis_Status.Org_Returning] ? 1 : 0);
            lbLedInposition.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(Cores.Fas_Data.lstAxis_State[(int)Cores.EAxis_Status.Inposition] ? 1 : 0);
            lbLedServoOn.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(Cores.Fas_Data.lstAxis_State[(int)Cores.EAxis_Status.Servo_On] ? 1 : 0);
            lbLedAlarmReset.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(Cores.Fas_Data.lstAxis_State[(int)Cores.EAxis_Status.Alarm_Reset] ? 1 : 0);
            lbLedPTStopped.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(Cores.Fas_Data.lstAxis_State[(int)Cores.EAxis_Status.PT_Stopped] ? 1 : 0);
            lbLedOriginSensor.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(Cores.Fas_Data.lstAxis_State[(int)Cores.EAxis_Status.Origin_Sensor] ? 1 : 0);
            lbLedZPulse.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(Cores.Fas_Data.lstAxis_State[(int)Cores.EAxis_Status.Z_Pulse] ? 1 : 0);
            lbLedOrgRetOk.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(Cores.Fas_Data.lstAxis_State[(int)Cores.EAxis_Status.Org_Ret_OK] ? 1 : 0);
            lbLedMotionDIR.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(Cores.Fas_Data.lstAxis_State[(int)Cores.EAxis_Status.Motion_DIR] ? 1 : 0);
            lbLedMotioning.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(Cores.Fas_Data.lstAxis_State[(int)Cores.EAxis_Status.Motioning] ? 1 : 0);
            lbLedMotionPause.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(Cores.Fas_Data.lstAxis_State[(int)Cores.EAxis_Status.Motion_Pause] ? 1 : 0);
            lbLedMotionAccel.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(Cores.Fas_Data.lstAxis_State[(int)Cores.EAxis_Status.Motion_Accel] ? 1 : 0);
            lbLedMotionDecel.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(Cores.Fas_Data.lstAxis_State[(int)Cores.EAxis_Status.Motion_Decel] ? 1 : 0);
            lbLedMotionConst.State = (LBSoft.IndustrialCtrls.Leds.LBLed.LedState)(Cores.Fas_Data.lstAxis_State[(int)Cores.EAxis_Status.Motion_Constant] ? 1 : 0);


            var gripCommand = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 134);
            var gripComplted = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 150);

            if (gripCommand.iData != 0 && gripComplted.iData != 0)
            {
                main.core_Object.Modbus_Sender(134, 0);
            }

            if (IsCobotManual)
            {
                string listLog = null;
                string listMsg = null;
                string listStepLog = null;

                var toolOut = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 22);
                var toolIn = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 21);
                var manualCommand = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 129);
                var manualComplted = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 145);
                var manualStepNumber = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 153);

                listStepLog = $"[{manualStepNumber.iData:000}]";

                bool IsErrorCheck = false;

                int nDumLenth = 50;

                switch (nCobotManualNumber)
                {
                    default:

                        tsManualOperTime = DateTime.Now - dtManualOperTime;

                        if (manualCommand.iData == manualComplted.iData && manualComplted.iData != 0)
                        {
                            main.core_Object.Modbus_Sender(129, 0);

                            listMsg = "로봇 위치 이동 동작 완료 하였습니다.";
                            listLog = $"<<<{DateTime.Now:HH:mm:ss.fff} >>> {listMsg.PadRight(nDumLenth - 12)} <<< {tsManualOperTime} >>>";

                            devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
                              $"{System.Reflection.MethodBase.GetCurrentMethod().Name.PadLeft(20)}" +
                              $" | Manual Operation Time : {tsManualOperTime} | value : {manualComplted.iData:00}");


                            IsCobotManual = false;
                        }
                        else if (manualCommand.iData == manualComplted.iData && manualComplted.iData == 0)
                        {
                            main.core_Object.Modbus_Sender(129, nCobotManualNumber);

                        }
                        else
                        {
                            listMsg = "로봇 위치 이동 동작 중 입니다.";
                            listLog = $"<<<{DateTime.Now:HH:mm:ss.fff} >>> {listMsg.PadRight(nDumLenth)} <<< {tsManualOperTime} >>>";
                            //listLog += $" | {manualCommand.iData} | {manualComplted.iData} |";
                            IsErrorCheck = true;
                        }

                        //devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
                        //    $"{System.Reflection.MethodBase.GetCurrentMethod().Name.PadLeft(20)}" +
                        //    $" | Cobot Home Complted : {Cores.Fas_Data.lstIO_InState[3][11]} | Cobot Home Alarm : {Cores.Fas_Data.lstIO_InState[3][12]}");


                        break;

                    case 101:
                        if (Define.eGripper == EGRIPS.DH)
                        {
                            if (((toolOut.iData >> 1) & 1) != 0 && ((toolOut.iData >> 0) & 1) != 0)
                            {
                                listLog = $"<<<{DateTime.Now:HH:mm:ss.fff} >>> 그리퍼 열림 동작 중 입니다.";
                                IsErrorCheck = true;
                            }
                            else
                            {
                                listLog = $"<<<{DateTime.Now:HH:mm:ss.fff} >>> 그리퍼 열림 동작 완료 하였습니다.";
                                IsCobotManual = false;
                            }
                        }
                        if (Define.eGripper == EGRIPS.ZIMMER)
                        {
                            if (((toolIn.iData >> 1) & 1) != 0 && ((toolIn.iData >> 0) & 1) != 0)
                            {

                            }
                            else if ((toolIn.iData >> 1 & 1) != 0)
                            {

                            }
                            else if ((toolIn.iData >> 0 & 1) != 0)
                            {
                                listLog = $"<<<{DateTime.Now:HH:mm:ss.fff} >>> 그리퍼 열림 동작 완료 하였습니다.";
                                IsCobotManual = false;
                            }
                            else
                            {
                                listLog = $"<<<{DateTime.Now:HH:mm:ss.fff} >>> 그리퍼 열림 동작 중 입니다.";
                                IsErrorCheck = true;
                            }

                            if ((toolOut.iData >> 1 & 1) != 0)
                            {
                                main.core_Object.Modbus_Sender(Externs.Robot_Modbus_Table.Robot_Write.TOOL_IO, 2);
                            }
                            else if ((toolOut.iData >> 0 & 1) != 0)
                            {
                                main.core_Object.Modbus_Sender(Externs.Robot_Modbus_Table.Robot_Write.TOOL_IO, 1);
                            }
                        }
                        break;
                    case 100:
                        if (Define.eGripper == EGRIPS.DH)
                        {
                            if (((toolOut.iData >> 1) & 1) != 0 && ((toolOut.iData >> 0) & 1) != 0)
                            {
                                listLog = $"<<<{DateTime.Now:HH:mm:ss.fff} >>> 그리퍼 닫힘 동작 완료 하였습니다.";
                                IsCobotManual = false;
                            }
                            else
                            {
                                listLog = $"<<<{DateTime.Now:HH:mm:ss.fff} >>> 그리퍼 닫힘 동작 중 입니다.";
                                IsErrorCheck = true;
                            }
                        }     
                        if (Define.eGripper == EGRIPS.ZIMMER)
                        {
                            if (((toolIn.iData >> 1) & 1) != 0 && ((toolIn.iData >> 0) & 1) != 0)
                            {
                              
                            }
                            else if((toolIn.iData >> 1 & 1) != 0 )
                            {
                            
                            }
                            else if ((toolIn.iData >> 0 & 1) != 0)
                            {
                            
                            }
                            else
                            {
                                //listLog = $"<<<{DateTime.Now:HH:mm:ss.fff} >>> 그리퍼 닫힘 동작 중 입니다.";
                                //IsErrorCheck = true;
                                listLog = $"<<<{DateTime.Now:HH:mm:ss.fff} >>> 그리퍼 닫힘 동작 완료 하였습니다.";
                                IsCobotManual = false;
                            }

                            if ((toolOut.iData >> 1 & 1) != 0)
                            {
                                main.core_Object.Modbus_Sender(Externs.Robot_Modbus_Table.Robot_Write.TOOL_IO, 2);
                            }
                            else if ((toolOut.iData >> 0 & 1) != 0)
                            {
                                main.core_Object.Modbus_Sender(Externs.Robot_Modbus_Table.Robot_Write.TOOL_IO, 1);
                            }
                        }
                        break;

                }

                //if (manualCommand.iData == manualComplted.iData && manualComplted.iData == 0)
                //{
                //    listMsg = "명령 해제 되었습니다.";
                //    listLog = $"<<<{DateTime.Now:HH:mm:ss.fff} >>> {listMsg.PadRight(nDumLenth)} <<< {tsManualOperTime.TotalMilliseconds:0000.0000}ms >>>";
                //    //listLog += $" | {manualCommand.iData} | {manualComplted.iData} |";
                //
                //    swManualErrorTime.Stop();
                //    IsCobotManual = false;
                //}

                if (listLog != null)
                {
                    listBox2.Items.Insert(0, listLog + listStepLog);
                }

                if (swManualErrorTime.IsRunning == false && IsErrorCheck)
                {
                    swManualErrorTime.Restart();
                }
                else if (swManualErrorTime.IsRunning == true && IsErrorCheck == false)
                {
                    swManualErrorTime.Stop();
                }

                //if (swManualErrorTime.ElapsedMilliseconds >= 20000)
                //{
                //    listMsg = "동작 타임아웃 발생 되었습니다.";
                //    swManualErrorTime.Stop();
                //    listLog = $"<<<{DateTime.Now:HH:mm:ss.fff} >>> {listMsg.PadRight(nDumLenth)} <<< {swManualErrorTime.ElapsedMilliseconds:0000}ms >>>";
                //    listBox2.Items.Insert(0, listLog);
                //    main.core_Object.Modbus_Sender(129, 0);
                //    IsCobotManual = false;
                //}

                
            }

            //2023.05.04
            FuncTechingOffset();

            if (listBox2.Items.Count >= 6)
            {
                listBox2.Items.RemoveAt(listBox2.Items.Count - 1);
            }

            if (listBox2.ScrollAlwaysVisible)
            {
                listBox2.ScrollAlwaysVisible = false;
            }

            if (IsCobotAutoTest)
            {
                int iPosNumber = int.Parse(textBox1.Text);
                if (Cores.Core_StepModule.Cobot_Move_Action(iPosNumber))
                {
                    textBox1.Text = "0";
                    listBox2.Items.Insert(0, $"<<< {DateTime.Now:HH:mm:ss} >>> 자동 테스트 동작 완료 입니다. {iPosNumber}");
                    IsCobotAutoTest = false;
                }
                else if (iPosNumber == 0)
                {
                    listBox2.Items.Insert(0, $"<<< {DateTime.Now:HH:mm:ss} >>> 잘못 입력 하였습니다. {iPosNumber}");
                    IsCobotAutoTest = false;
                    bool[] OutBuff = Fas_Data.lstIO_OutState[3];

                    OutBuff[(int)COBOT_OUTPUT.Move_Pos1] = Convert.ToBoolean(0);
                    OutBuff[(int)COBOT_OUTPUT.Move_Pos2] = Convert.ToBoolean(0);
                    OutBuff[(int)COBOT_OUTPUT.Move_Pos3] = Convert.ToBoolean(0);
                    OutBuff[(int)COBOT_OUTPUT.Move_Pos4] = Convert.ToBoolean(0);
                    OutBuff[(int)COBOT_OUTPUT.Move_Cmd] = Convert.ToBoolean(0);

                    Cores.Fas_Motion.SetOutput((int)Core_StepModule.IO_Board.EzEtherNetIO_4, OutBuff);
                }
                else
                {
                    listBox2.Items.Insert(0, $"<<< {DateTime.Now:HH:mm:ss} >>> 자동 테스트 동작 중 입니다. {iPosNumber}");
                }
                
                //if (iPosNumber != 0)
                //{
                //    Cores.Core_StepModule.Cobot_Move_Action(iPosNumber);

                //    listBox2.Items.Insert(0, $"<<< {DateTime.Now:HH:mm:ss} >>> 자동 테스트 동작 중 입니다.");
                //}
                ////if (Fas_Data.lstIO_OutState[3][15] == true)
                ////{
                ////    textBox1.Text = "0";
                ////}

                //if (Fas_Data.lstIO_InState[3][15] == false && Fas_Data.lstIO_OutState[3][15] == false)
                //{
                //    listBox2.Items.Insert(0, $"<<< {DateTime.Now:HH:mm:ss} >>> 자동 테스트 동작 완료 입니다.");
                //    IsCobotAutoTest = false;
                //}

            }

            //2023.03.20 ::: 에러처리, 수동운전 화면이 아닌데 동작될 경우 회피
            if (iMyPageNumber != 2 && IsCobotManual == true)
            {
                IsCobotManual = false;
                IsCobotAutoTest = false;


                listBox2.Items.Insert(0, "!!!Page Number Wrong!!!");
                main.core_Object.Modbus_Sender(129, 0);

            }


            //2023.03.10 ::: 오토 클리어 테스트
            //var manuCommand = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 128);
            //var manuComplte = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 144);
            //if (((manuCommand.iData >> 13) & 1) != 0 && ((manuComplte.iData >> 13) & 1) != 0)
            //{
            //    int iBuff = 0;
            //    for (int idx = 0; idx < 16; idx++)
            //    {
            //        if (idx != 13)
            //        {
            //            if (((manuCommand.iData >> idx) & 1) != 0)
            //            {
            //                iBuff |= ((manuCommand.iData << idx) & 1);
            //            }
            //        }
            //    }
            //    main.core_Object.Modbus_Sender(128, iBuff);
            //}

            bool[] OutStateBuff = Cores.Fas_Data.lstIO_OutState[COBOT];
            bool[] InStateBuff = Cores.Fas_Data.lstIO_InState[COBOT];
            //2023.03.21 ::: 제어권이 아직 넘어온게 아니니까 어떻게 할 수가 없넹
            //로봇 원점 확인
            //if (OutStateBuff[(int)COBOT_INPUT.Home] == true)

            bool IsRobotOrgState = Robot_Orgin_Status();
            bool IsRobotUseState = Robot_UnUsed_Status();
            bool IsRobotDownState = Robot_Down_Status();

            var gripper = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 21);
            var gripperOut = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 22);

            //if (gripper.iData == 1)
            //{
            //    buttonGripperOpen.Enabled = true;
            //}

            if (Define.eGripper == EGRIPS.DH)
            {
                switch (gripper.iData)
                {
                    case 0:
                        break;
                    case 1:
                        if (gripperOut.iData == 0)
                        {
                            buttonGripperOpen.Enabled = false;
                            buttonGripperClose.Enabled = true;
                        }
                        else
                        {
                            buttonGripperOpen.Enabled = true;
                            buttonGripperClose.Enabled = false;
                        }

                        break;
                    case 2:
                        buttonGripperOpen.Enabled = true;
                        buttonGripperClose.Enabled = false;
                        break;
                    case 3:
                        break;
                }
            }
            else if (Define.eGripper == EGRIPS.ZIMMER)
            {
                switch (gripper.iData)
                {
                    case 0:
                        buttonGripperOpen.Enabled = true;
                        buttonGripperClose.Enabled = false;
                        break;
                    case 1:
                        buttonGripperOpen.Enabled = false;
                        buttonGripperClose.Enabled = true;
                     

                        break;
                    case 2:
                        buttonGripperOpen.Enabled = true;
                        buttonGripperClose.Enabled = false;
                        break;
                    case 3:
                        break;
                }

                //2023.06.14 임시
                buttonGripperOpen.Enabled = true;
                buttonGripperClose.Enabled = true;

            }


            //bool IsGripOpen = gripper.iData;

            //버튼 이벤트 막기           
            //foreach (Button btn in lstRobotMove)
            //{
            //    btn.Enabled = Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition];
            //}

            //2023.05.04
            //bool IsDebugTEST = false;

            if (Define.IsXaxisDebugMove)
            {
                foreach (System.Windows.Forms.RadioButton btn in lstMovePos)
                {
                    btn.Enabled = Fas_Data.IsOrgRetOk;
                }
            }
            else
            {
                foreach (System.Windows.Forms.RadioButton btn in lstMovePos)
                {
                    btn.Enabled = Fas_Data.IsOrgRetOk & IsRobotOrgState;
                }
            }

           

            buttonRobotOrg.Enabled = true;
            //buttonRobotOrg.Enabled = !IsRobotOrgState;
            //buttonRobotOrg.Enabled = !InStateBuff[(int)COBOT_INPUT.Home] & IsRobotOrgState;

            buttonAlignLeft.Enabled = IsRobotOrgState;
            buttonToolChange.Enabled = IsRobotOrgState;
            buttonAlignRight.Enabled = IsRobotOrgState;   
            
            buttonRobotDown.Enabled = !IsRobotDownState & IsRobotOrgState;
            buttonRobotUp.Enabled = IsRobotDownState;

            buttonUnUsedMove.Enabled = !IsRobotUseState & IsRobotOrgState;
            buttonUsedMove.Enabled = !buttonUnUsedMove.Enabled;

            //buttonUnUsedMove.Enabled = true;
            //buttonUsedMove.Enabled = true;

            //var cobotToolInput = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 021);
            //var joint1 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 270);
            //var joint2 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 271);
            //var joint3 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 272);
            //var joint4 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 273);
            //var joint5 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 274);
            //var joint6 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 275);

            //var cobotState = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 259);
            //var cobotServoOn = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 260);
            //var cobotEmcStop = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 261);
            //var cobotSafeStop = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 262);


            //double[] JointPos = new double[6];
            //JointPos[0] = double.Parse(joint1.strData);
            //JointPos[1] = double.Parse(joint2.strData);
            //JointPos[2] = double.Parse(joint3.strData);
            //JointPos[3] = double.Parse(joint4.strData);
            //JointPos[4] = double.Parse(joint5.strData);
            //JointPos[5] = double.Parse(joint6.strData);

        }      

        /// <summary>
        /// 2023.02.24
        /// Robot Operation & Gripper Operation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRobotOrg_Click(object sender, EventArgs e)
        {
            //2023.03.14 ::: 로봇 회피 이동 추가
            //2023.03.14 ::: 인터락 추가
            Common.FormMessageBox msg;
            if (Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition] != true)
            {
                msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None, "서보 정지 후 동작 바랍니다.");
                msg.ShowDialog();
                return;
            }

            if (main.core_Object.processCore.eCurrState != PackML.ECurrState.CurrentState_Stopped)
                return;



                int iModAddress = -1;
            System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
            string listLog = null;
            string listMsg = null;
            string dummyMsg = "#";
            iModAddress = 129;//디폴트 메뉴얼 위치 명령
            var manualComplted = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 145);
            var manualCommand = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == iModAddress);

            double dActualPos = Cores.Fas_Func.PPR_To_mm(Fas_Data.iActPos);

            nCobotManualNumber = 0;
            bool IsLargeBasket = true;

            switch (btn.Name)
            {
                case "buttonRobotOrg":
                    nCobotManualNumber = 13;
                    listMsg = "코봇 원점 동작 명령 실행 하였습니다.";
                    listLog = $"<<<{DateTime.Now:HH:mm:ss.fff} >>> {listMsg} {dummyMsg.PadRight(15, dummyMsg[0]).PadLeft(32)}";
                    RobotSoundEventEvent(14);
                    break;

                case "buttonUnUsedMove":
                    //2023.05.11
                    //엑스 축 위치가 대기 위치가 아닌 경우에는 하면 안됨
                  
                    if (dActualPos <= Cores.Core_Object.GetPos_File.lstRealPositions[(int)MyActionXStepBuffer.Wait - 1] + 0.5
                       && dActualPos >= Cores.Core_Object.GetPos_File.lstRealPositions[(int)MyActionXStepBuffer.Wait - 1] - 0.5)
                    {
                        nCobotManualNumber = 14;
                        listMsg = "코봇 회피 동작 명령 실행 하였습니다.";
                        listLog = $"<<<{DateTime.Now:HH:mm:ss.fff} >>> {listMsg} {dummyMsg.PadRight(15, dummyMsg[0]).PadLeft(32)}";
                        RobotSoundEventEvent(12);
                    }
                    else
                    {
                        msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None, "단축 로봇 대기위치에서 이동이 가능 합니다.");
                        msg.ShowDialog();
                        return;
                    }
                    break;

                case "buttonUsedMove":
                    nCobotManualNumber = 1;
                    listMsg = "코봇 사용 위치 동작 명령 실행 하였습니다.";
                    listLog = $"<<<{DateTime.Now:HH:mm:ss.fff} >>> {listMsg} {dummyMsg.PadRight(15, dummyMsg[0]).PadLeft(32)}";
                    RobotSoundEventEvent(13);
                    break;

                case "buttonRobotDown":

                    //if (dActualPos <= Cores.Core_Object.GetPos_File.lstRealPositions[(int)MyActionXStepBuffer.Wait - 1] + 0.5
                    //     && dActualPos >= Cores.Core_Object.GetPos_File.lstRealPositions[(int)MyActionXStepBuffer.Wait - 1] - 0.5)
                    //{ 
                    //
                    //}



                    for (int i = 0; i < lstMovePos.Count; i++)
                    {
                        if (lstMovePos[i].Checked)
                        {
                            nCobotManualNumber = i+1;
                            listMsg = "코봇 하강 동작 명령 실행 하였습니다.";
                            listLog = $"<<<{DateTime.Now:HH:mm:ss.fff} >>> {listMsg} {dummyMsg.PadRight(15, dummyMsg[0]).PadLeft(32)}";

                            IsLargeBasket &= false;
                            break;
                        }
                    }

                    //var openCloseGrip = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 21);
                    //if (openCloseGrip.iData == 2)
                    //{
                    //    msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None,
                    //                   "그리퍼를 열림 후 사용 바랍니다..");
                    //    msg.ShowDialog();
                    //    return;
                    //}

                    //    switch (nCobotManualNumber)
                    //{
                    //    default:
                    //        msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None,
                    //                 "단축 로봇의 위치를 선택 후 사용 바랍니다.");
                    //        msg.ShowDialog();
                    //        return;
                    //        break;

                    //    case 2:
                    //        if (Cores.Fas_Data.lstIO_InState[CHEFY][0 + iSensorLocateOffsset] == true)
                    //        {
                    //            msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None,
                    //                "해당 위치의 바스켓을 제거 후 사용 바랍니다.");
                    //            msg.ShowDialog();
                    //            return;
                    //        }
                    //        break;

                    //    case 3:
                    //        if (Cores.Fas_Data.lstIO_InState[CHEFY][1 + iSensorLocateOffsset] == true)
                    //        {
                    //            msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None,
                    //                "해당 위치의 바스켓을 제거 후 사용 바랍니다.");
                    //            msg.ShowDialog();
                    //            return;
                    //        }
                    //        break;

                    //    case 4:
                    //        if (Cores.Fas_Data.lstIO_InState[CHEFY][2 + iSensorLocateOffsset] == true)
                    //        {
                    //            msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None,
                    //                "해당 위치의 바스켓을 제거 후 사용 바랍니다.");
                    //            msg.ShowDialog();
                    //            return;
                    //        }
                    //        break;
                    //}



                    //if (IsLargeBasket == true)
                    //{
                    //    //double dActualPos = Cores.Fas_Func.PPR_To_mm(Fas_Data.iActPos);
                        
                    //    if (dActualPos == 458)
                    //    { }
                    //    if (dActualPos == 913)
                    //    { }
                    //    if (dActualPos == 1297)
                    //    { }

                    //    nCobotManualNumber = 5;
                    //}
                    break;
                case "buttonGripperOpen":
                    switch (Define.eGripper)
                    {
                        case EGRIPS.DH:
                            iModAddress = 22;
                            var open = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == iModAddress);
                            if (((open.iData >> 1) & 1) != 0 && ((open.iData >> 0) & 1) != 0)
                            {
                                //M-Serize
                                //main.core_Object.Modbus_Sender(Externs.Robot_Modbus_Table.Robot_Write.TOOL_IO, 2);
                                //main.core_Object.Modbus_Sender(Externs.Robot_Modbus_Table.Robot_Write.TOOL_IO, 3);

                                //E-Serize
                                main.core_Object.Modbus_Sender(Externs.Robot_Modbus_Table.Robot_Write.TOOL_IO, 1);
                                main.core_Object.Modbus_Sender(Externs.Robot_Modbus_Table.Robot_Write.TOOL_IO, 2);
                                listMsg = "그리퍼 열림 동작 명령 실행 하였습니다.";
                                listLog = $"<<<{DateTime.Now:HH:mm:ss.fff} >>> {listMsg} {dummyMsg.PadRight(15, dummyMsg[0]).PadLeft(32)}";
                            }
                            break;

                        case EGRIPS.ZIMMER:
                            iModAddress = 21;
                            var openClose = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == iModAddress);
                            //if (openClose.iData == 0 || openClose.iData == 2)
                            {
                                //M-Serize
                                //main.core_Object.Modbus_Sender(Externs.Robot_Modbus_Table.Robot_Write.TOOL_IO, 2);
                                //main.core_Object.Modbus_Sender(Externs.Robot_Modbus_Table.Robot_Write.TOOL_IO, 3);

                                //E-Serize
                                //main.core_Object.Modbus_Sender(Externs.Robot_Modbus_Table.Robot_Write.TOOL_IO, 2);
                                //main.core_Object.Modbus_Sender(Externs.Robot_Modbus_Table.Robot_Write.TOOL_IO, 1);

                                //A-Serize
                                main.core_Object.Modbus_Sender(Externs.Robot_Modbus_Table.Robot_Write.TOOL_IO, 1);
                                //main.core_Object.Modbus_Sender(Externs.Robot_Modbus_Table.Robot_Write.TOOL_IO, 2);
                                //

                                //홀딩 레지스터에 있는 것으로 제어
                                //main.core_Object.Modbus_Sender(134, 1);

                                listMsg = "그리퍼 열림 동작 명령 실행 하였습니다.";
                                listLog = $"<<<{DateTime.Now:HH:mm:ss.fff} >>> {listMsg} {dummyMsg.PadRight(15, dummyMsg[0]).PadLeft(32)}";


                                //Thread.Sleep(500);
                                //main.core_Object.Modbus_Sender(Externs.Robot_Modbus_Table.Robot_Write.TOOL_IO, 1);
                            }

                            break;
                    }

                  
                    nCobotManualNumber = 101;
                    break;
                case "buttonGripperClose":
               
                    switch (Define.eGripper)
                    {
                        case EGRIPS.DH:
                            iModAddress = 22;
                            var close = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == iModAddress);
                            if (((close.iData >> 1) & 1) != 0 && ((close.iData >> 0) & 1) != 0)
                            {
                                
                            }
                            else
                            {
                                //M-Serize
                                //main.core_Object.Modbus_Sender(Externs.Robot_Modbus_Table.Robot_Write.TOOL_IO, 2);
                                //main.core_Object.Modbus_Sender(Externs.Robot_Modbus_Table.Robot_Write.TOOL_IO, 3);

                                //E-Serize
                                main.core_Object.Modbus_Sender(Externs.Robot_Modbus_Table.Robot_Write.TOOL_IO, 1);
                                main.core_Object.Modbus_Sender(Externs.Robot_Modbus_Table.Robot_Write.TOOL_IO, 2);
                                listMsg = "그리퍼 열림 동작 명령 실행 하였습니다.";
                                listLog = $"<<<{DateTime.Now:HH:mm:ss.fff} >>> {listMsg} {dummyMsg.PadRight(15, dummyMsg[0]).PadLeft(32)}";
                            }
                            break;

                        case EGRIPS.ZIMMER:
                            iModAddress = 21;
                            var closeOpen = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == iModAddress);
                            //if (closeOpen.iData == 1 || closeOpen.iData == 3)
                            {
                                //M-Serize
                                //main.core_Object.Modbus_Sender(Externs.Robot_Modbus_Table.Robot_Write.TOOL_IO, 2);
                                //main.core_Object.Modbus_Sender(Externs.Robot_Modbus_Table.Robot_Write.TOOL_IO, 3);

                                //E-Serize
                                //main.core_Object.Modbus_Sender(Externs.Robot_Modbus_Table.Robot_Write.TOOL_IO, 1);
                                //main.core_Object.Modbus_Sender(Externs.Robot_Modbus_Table.Robot_Write.TOOL_IO, 2);

                                //A-Serize
                                //main.core_Object.Modbus_Sender(Externs.Robot_Modbus_Table.Robot_Write.TOOL_IO, 1);
                                main.core_Object.Modbus_Sender(Externs.Robot_Modbus_Table.Robot_Write.TOOL_IO, 2);
                                //

                                //홀딩 레지스터에 있는 것으로 제어
                                //main.core_Object.Modbus_Sender(134, 2);

                                listMsg = "그리퍼 열림 동작 명령 실행 하였습니다.";
                                listLog = $"<<<{DateTime.Now:HH:mm:ss.fff} >>> {listMsg} {dummyMsg.PadRight(15, dummyMsg[0]).PadLeft(32)}";

                                //Thread.Sleep(500);
                                //main.core_Object.Modbus_Sender(Externs.Robot_Modbus_Table.Robot_Write.TOOL_IO, 2);
                            }

                            break;
                    }
                    nCobotManualNumber = 100;
                    break;
                case "buttonRobotUp":
                    listMsg = "코봇 대기 위치 명령 실행 하였습니다.";
                    listLog = $"<<<{DateTime.Now:HH:mm:ss.fff} >>> {listMsg} {dummyMsg.PadRight(15, dummyMsg[0]).PadLeft(32)}";
                    nCobotManualNumber = 1;
                    break;
                case "buttonAlignLeft":
                    listMsg = "코봇 얼라인 왼쪽 위치 명령 실행 하였습니다.";
                    listLog = $"<<<{DateTime.Now:HH:mm:ss.fff} >>> {listMsg} {dummyMsg.PadRight(15, dummyMsg[0]).PadLeft(32)}";
                    nCobotManualNumber = 15;
                    break;
                case "buttonToolChange":

                    if (dActualPos <= Cores.Core_Object.GetPos_File.lstRealPositions[(int)MyActionXStepBuffer.Wait - 1] + 0.5
                     && dActualPos >= Cores.Core_Object.GetPos_File.lstRealPositions[(int)MyActionXStepBuffer.Wait - 1] - 0.5)
                    {
                        listMsg = "코봇 툴 교체 위치 명령 실행 하였습니다.";
                        listLog = $"<<<{DateTime.Now:HH:mm:ss.fff} >>> {listMsg} {dummyMsg.PadRight(15, dummyMsg[0]).PadLeft(32)}";
                        nCobotManualNumber = 12;
                    }
                    else
                    {
                        msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None, "단축 로봇 대기위치에서 이동이 가능 합니다.");
                        msg.ShowDialog();
                        return;
                    }

                    break;
                case "buttonAlignRight":
                    //2023.05.08 ::: 그리퍼 대기 위치 이동
                    listMsg = "코봇 얼라인 오른쪽 위치 명령 실행 하였습니다.";
                    listLog = $"<<<{DateTime.Now:HH:mm:ss.fff} >>> {listMsg} {dummyMsg.PadRight(15, dummyMsg[0]).PadLeft(32)}";
                    nCobotManualNumber = 16;
                    break;
            }

            if (listLog != null)
            {
                if (manualCommand != null && manualComplted != null)
                {
                    dtManualOperTime = DateTime.Now;
                    if (manualCommand.iData == 0 && manualComplted.iData == 0
                        && nCobotManualNumber != 100 && nCobotManualNumber != 101)
                    {
                      

                        //listLog += $" | addr : {iModAddress} | value : {nCobotManualNumber} |";
                        main.core_Object.Modbus_Sender(iModAddress, nCobotManualNumber);

                        devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
                            $"{System.Reflection.MethodBase.GetCurrentMethod().Name.PadLeft(45)}" +
                            $" | addr : {iModAddress} | value : {nCobotManualNumber:00}");
                    }
                    else
                    {
                        //listLog = $"<<<{DateTime.Now:HH:mm:ss.fff} >>> 그리퍼 열림 동작 명령 실행 하였습니다.";
                        main.core_Object.Modbus_Sender(iModAddress, 0);
                    }
                }

                

                listBox2.Items.Insert(0, listLog);
            }
            
            
            IsCobotManual = true;

        }

        /// <summary>
        /// 2023.02.24
        /// X Axis Move Command
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (main.core_Object.processCore.eCurrState != PackML.ECurrState.CurrentState_Stopped)
                    return;


                //로봇 원점 중을 확인 후 명령 줄 것
                int alarm = Cores.Fas_Data.lstAxis_State[(int)Cores.EAxis_Status.Err_Sevo_Alarm] ? 1 : 0;
                int orging = Cores.Fas_Data.lstAxis_State[(int)Cores.EAxis_Status.Org_Ret_OK] ? 1 : 0;
                //int orging = Fas_Data.IsOrgRetOk ? 1: 0;
                int inpos = Cores.Fas_Data.lstAxis_State[(int)Cores.EAxis_Status.Inposition] ? 1 : 0;
                int servoOn = Cores.Fas_Data.lstAxis_State[(int)Cores.EAxis_Status.Servo_On] ? 1 : 0;

                Common.FormMessageBox msg;
                System.Windows.Forms.RadioButton rb = sender as System.Windows.Forms.RadioButton;
                if (rb.Checked == true)
                {
                    if (alarm == 1)
                    {
                        msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None, "서보 알람 해제 후 동작 바랍니다.");
                        msg.ShowDialog();
                        rb.Checked = false;
                        return;
                    }
                    if (orging == 0)
                    {
                        msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None, "원점 완료 후 동작 바랍니다.");
                        msg.ShowDialog();
                        rb.Checked = false;
                        return;
                    }
                    if (inpos == 0)
                    {
                        msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None, "모션 이동 완료 후 동작 바랍니다.");
                        msg.ShowDialog();
                        rb.Checked = false;
                        return;
                    }
                    if (servoOn == 0)
                    {
                        msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None, "서보 오프 상태 입니다. 서보 온 동작 바랍니다.");
                        msg.ShowDialog();
                        rb.Checked = false;
                        return;
                    }

                    //TEST TEMP
                    //2023.03.10 로봇 원점 확인
                    if (Robot_Orgin_Status() == false && Define.IsXaxisDebugMove == false)
                    {
                        msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None, "로봇 원점 위치가 아닙니다. 확인 후 동작 바랍니다.");
                        msg.ShowDialog();
                        rb.Checked = false;
                        return;
                    }




                }

                //int iXaxisSpeed = 0;
                //int iXaxisAccDec = 0;
                //double dCommandPos = 0;
                //int fastechPos = Cores.Fas_Func.PPS_To_mm((double)dCommandPos);
                //int fastechSpd = Cores.Fas_Func.PPS_To_mm((double)iXaxisSpeed);            

                //for (int i = 0; i < lstMovePos.Count; i++)
                //{
                //    if (rb == lstMovePos[i] && rb.Checked)
                //    {
                //        DataGridView dgv = dataGridViewPositions;
                //        dCommandPos = double.Parse(dgv.Rows[0].Cells[i + 1].Value.ToString());
                //        fastechPos = int.Parse(dgv.Rows[1].Cells[i + 1].Value.ToString());
                //        Cores.Fas_Motion.MovePos(nFasBdNumber, 0, 1, (uint)fastechSpd, fastechPos);
                //    }
                //}

                //int fastechPos = Cores.Fas_Func.PPS_To_mm((double)dCommandPos);
                int fastechPos = Cores.Fas_Func.PPS_To_mm((double)dCommandPos);
                int fastechSpd = Cores.Fas_Func.PPS_To_mm((double)iXaxisSpeed);

                for (int i = 0; i < lstMovePos.Count; i++)
                {
                    if (rb == lstMovePos[i] && rb.Checked)
                    {
                        DataGridView dgv = dataGridViewPositions;
                        dCommandPos = double.Parse(dgv.Rows[0].Cells[i + 1].Value.ToString());
                        //fastechPos = int.Parse(dgv.Rows[1].Cells[i + 1].Value.ToString());
                        fastechPos = Cores.Fas_Func.PPS_To_mm((double)dCommandPos);
                        dgv.Rows[1].Cells[i + 1].Value = fastechPos;
                        //Cores.Fas_Motion.MovePos(nFasBdNumber, 0, 1, (uint)fastechSpd, fastechPos);
                        Cores.Fas_Motion.MovePos(nFasBdNumber, 0, 1, (uint)fastechSpd, fastechPos, Cores.Core_Object.GetObj_File.iXaxisAccDecTime);
                    }
                }
            }
                catch { }
            
        }

        private void dataGridViewXaxis_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;

                if (e.RowIndex == 1)
                    return;

                if (e.RowIndex == 2)
                    return;

                DataGridView view = sender as DataGridView;

                if (view.Rows[e.RowIndex].Cells[2].Value == null)
                    return;

                string prevData = view.Rows[e.RowIndex].Cells[2].Value.ToString();
                //strPreviousBuff[e.RowIndex] = prevData;
                Common.FormKeypad keypad = new Common.FormKeypad(prevData);

                if (keypad.ShowDialog() == DialogResult.OK)
                {
                    if (keypad.UserInputNo != null)
                    {
                        view.Rows[e.RowIndex].Cells[2].Value = keypad.UserInputNo;
                        switch (e.RowIndex)
                        {
                            case 0:
                                dCommandPos = double.Parse(keypad.UserInputNo);
                                break;
                            case 3:
                                dCmdPlusMinusPos = double.Parse(keypad.UserInputNo);
                                break;
                            case 4:
                                iXaxisAccDec = int.Parse(keypad.UserInputNo);
                                break;
                            case 5:
                                iXaxisSpeed = int.Parse(keypad.UserInputNo);
                                break;
                            case 6:
                                dOffsetPos = double.Parse(keypad.UserInputNo);
                                break;
                        }



                        devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
                            $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                            $" | Motion Recipe Changed {prevData} | {view.Rows[e.RowIndex].Cells[1].Value}");
                    }
                }
            }
            catch { }
           
        }

        private void buttonJogMinus_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                Common.FormMessageBox msg;
                //2023.03.10 로봇 원점 확인
                if (Robot_Orgin_Status() == false && Define.IsXaxisDebugMove == false)
                {
                    msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None, "로봇 원점 위치가 아닙니다. 확인 후 동작 바랍니다.");
                    msg.ShowDialog();
                    return;
                }

                //if (Robot_Orgin_Status() == false)
                //{
                //    msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None, "로봇 원점 위치가 아닙니다. 확인 후 동작 바랍니다.");
                //    msg.ShowDialog();
                //    return;
                //}

                bool IsInterlock = true;

                if (iXaxisAccDec <= iXaxisAccDecMin || iXaxisAccDec >= iXaxisAccDecMax)
                {
                    IsInterlock &= false;
                }

                if (iXaxisSpeed < 10 || iXaxisSpeed > 250)
                {
                    IsInterlock &= false;
                }
                DataGridView dgv = dataGridViewXaxis;
                iXaxisSpeed = int.Parse(dgv.Rows[5].Cells[2].Value.ToString());
                int fastechSpeed = Cores.Fas_Func.PPS_To_mm((double)iXaxisSpeed);
                Cores.Fas_Motion.JogMove(0, 0, iXaxisAccDec, fastechSpeed);
            }
            catch { }
           
        }

        private void buttonJogPlus_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                Common.FormMessageBox msg;
                //2023.03.10 로봇 원점 확인
                if (Robot_Orgin_Status() == false && Define.IsXaxisDebugMove == false)
                {
                    msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None, "로봇 원점 위치가 아닙니다. 확인 후 동작 바랍니다.");
                    msg.ShowDialog();
                    return;
                }

                bool IsInterlock = true;

                if (iXaxisAccDec <= iXaxisAccDecMin || iXaxisAccDec >= iXaxisAccDecMax)
                {
                    IsInterlock &= false;
                }

                if (iXaxisSpeed < 10 || iXaxisSpeed > 250)
                {
                    IsInterlock &= false;
                }
                DataGridView dgv = dataGridViewXaxis;
                iXaxisSpeed = int.Parse(dgv.Rows[5].Cells[2].Value.ToString());
                int fastechSpeed = Cores.Fas_Func.PPS_To_mm((double)iXaxisSpeed);
                Cores.Fas_Motion.JogMove(0, 1, iXaxisAccDec, fastechSpeed);
            }
            catch { }
            
        }

        private void buttonJogMinus_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                Cores.Fas_Motion.StopMotion(0, false);
            }
            catch { }
            
        }

        private void buttonJogPlus_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                Cores.Fas_Motion.StopMotion(0, false);
            }
            catch { }
            
        }

        /// <summary>
        /// 엑스 축 수동 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ManualAction(object sender, EventArgs e)
        {
            if (main.core_Object.processCore.eCurrState != PackML.ECurrState.CurrentState_Stopped)
                return;

            bool IsInterlock = true;

            if (iXaxisAccDec <= iXaxisAccDecMin || iXaxisAccDec >= iXaxisAccDecMax)
            {
                IsInterlock &= false;
            }

            if (iXaxisSpeed < 10 || iXaxisSpeed > 250)
            {
                IsInterlock &= false;
            }


            DataGridView dgv = dataGridViewXaxis;
            iXaxisSpeed = int.Parse(dgv.Rows[5].Cells[2].Value.ToString());
            dCommandPos = double.Parse(dgv.Rows[0].Cells[2].Value.ToString());
            dCmdPlusMinusPos = double.Parse(dgv.Rows[3].Cells[2].Value.ToString());

            int fastechSpeed = Cores.Fas_Func.PPS_To_mm((double)iXaxisSpeed);
            int fastechAbsPos = Cores.Fas_Func.PPS_To_mm((double)dCommandPos);
            int fastechIncPos = Cores.Fas_Func.PPS_To_mm((double)dCmdPlusMinusPos);

            Common.FormMessageBox msg;




            System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
            switch (btn.Name)
            {
                case "buttonServoOn":
                    Cores.Fas_Motion.SetServoOn(nFasBdNumber, 1);
                    break;
                case "buttonServoOff":
                    Cores.Fas_Motion.SetServoOn(nFasBdNumber, 0);
                    break;
                case "buttonAlarmReset":
                    Cores.Fas_Motion.CheckDriveErr(nFasBdNumber);


                    //2023.03.21 ::: 비상정지 눌렸을때 알람됨.
                    //아이오 전원 온오프 
                    bool[] OutStateBuff = null;
                    bool[] InStateBuff = null;
                    OutStateBuff = Cores.Fas_Data.lstIO_OutState[CHEFX];
                    InStateBuff = Cores.Fas_Data.lstIO_InState[CHEFX];

                    bool[] resetBuff = null;
                    resetBuff = Cores.Fas_Data.lstIO_OutState[CHEFZ];

                    resetBuff[0x0b] = true;
                    Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_3, resetBuff);

                    Thread.Sleep(1500);

                    resetBuff[0x0b] = false;
                    Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_3, resetBuff);

                    if (Fas_Data.lstAxis_State[(int)EAxis_Status.Err_Sevo_Alarm] == true)
                    {
                        OutStateBuff[0x0D] = true;
                        Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_1, OutStateBuff);                    

                        Stopwatch sw = Stopwatch.StartNew();
                        sw.Start();
                        while (Fas_Data.lstAxis_State[(int)EAxis_Status.Err_Sevo_Alarm] == true && Fas_Data.lstAxis_State[(int)EAxis_Status.Servo_On] == false)
                        {
                            Thread.Sleep(100);
                            OutStateBuff = Cores.Fas_Data.lstIO_OutState[CHEFX];
                            if (OutStateBuff[0x0D] == true)
                            {
                                Thread.Sleep(1000);
                                OutStateBuff[0x0D] = false;
                                Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_1, OutStateBuff);                               
                            }
                            else if (Fas_Data.lstAxis_State[(int)EAxis_Status.Servo_On] == false
                                && Fas_Data.lstAxis_State[(int)EAxis_Status.Err_Sevo_Alarm] == false
                                && OutStateBuff[0x0D] == false)
                            {
                                Cores.Fas_Motion.SetServoOn(nFasBdNumber, 1);

                                msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None, "모터 알람 해제 되었습니다.");
                                msg.ShowDialog();
                                return;
                            }
                            else if (sw.ElapsedMilliseconds >= 7000)
                            {
                                msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None, "모터 알람 해제 시간 초과되었습니다.");
                                msg.ShowDialog();
                                return;
                            }
                        }
                    }
                    break;
                case "buttonStop":
                    Cores.Fas_Motion.StopMotion(nFasBdNumber, false);
                    break;  
                case "buttonOrg":
                    //2023.03.10 로봇 원점 확인
                    if (Robot_Orgin_Status() == false && Define.IsXaxisDebugMove == false)
                    {
                        msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None, "로봇 원점 위치가 아닙니다. 확인 후 동작 바랍니다.");
                        msg.ShowDialog();                        
                        return;
                    }
                    Cores.Fas_Motion.OriginSearch(nFasBdNumber);
                    break;
                case "buttonJonAbs":
                    //2023.03.10 로봇 원점 확인
                    if (Robot_Orgin_Status() == false && Define.IsXaxisDebugMove == false)
                    {
                        msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None, "로봇 원점 위치가 아닙니다. 확인 후 동작 바랍니다.");
                        msg.ShowDialog();
                        return;
                    }
                    //Cores.Fas_Motion.MovePos(nFasBdNumber, 0, 1, (uint)fastechSpeed, fastechAbsPos);
                    Cores.Fas_Motion.MovePos(nFasBdNumber, 0, 1, (uint)fastechSpeed, fastechAbsPos, Cores.Core_Object.GetObj_File.iXaxisAccDecTime);
                    break;
                case "buttonJonMinus":
                    //2023.03.10 로봇 원점 확인
                    if (Robot_Orgin_Status() == false && Define.IsXaxisDebugMove == false)
                    {
                        msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None, "로봇 원점 위치가 아닙니다. 확인 후 동작 바랍니다.");
                        msg.ShowDialog();
                        return;
                    }
                    //Cores.Fas_Motion.MovePos(nFasBdNumber, 1, 0, (uint)fastechSpeed, fastechIncPos);
                    Cores.Fas_Motion.MovePos(nFasBdNumber, 1, 0, (uint)fastechSpeed, fastechIncPos, Cores.Core_Object.GetObj_File.iXaxisAccDecTime);
                    break;
                case "buttonJonPlus":
                    //2023.03.10 로봇 원점 확인
                    if (Robot_Orgin_Status() == false && Define.IsXaxisDebugMove == false)
                    {
                        msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None, "로봇 원점 위치가 아닙니다. 확인 후 동작 바랍니다.");
                        msg.ShowDialog();
                        return;
                    }
                    //Cores.Fas_Motion.MovePos(nFasBdNumber, 1, 1, (uint)fastechSpeed, fastechIncPos);
                    Cores.Fas_Motion.MovePos(nFasBdNumber, 1, 1, (uint)fastechSpeed, fastechIncPos, Cores.Core_Object.GetObj_File.iXaxisAccDecTime);
                    break;
            }

            for(int idx =0; idx< lstMovePos.Count; idx++)
            {
                if (lstMovePos[idx].Checked)
                {
                    lstMovePos[idx].Checked = false;

                }
            }
        }

        private void lbButton1_Click(object sender, EventArgs e)
        {
            try
            {
                Cores.Fas_Motion.StopMotion(nFasBdNumber, true);

                bool[] OutStateBuff = null;
                bool[] InStateBuff = null;
                OutStateBuff = Cores.Fas_Data.lstIO_OutState[COBOT];
                InStateBuff = Cores.Fas_Data.lstIO_InState[COBOT];
                for (int i = 0; i < OutStateBuff.Length; i++)
                {
                    OutStateBuff[i] = false;
                }
                Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);

            }
            catch { }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkBox1.Checked)
                {
                    IsCobotAutoTest = true;
                }
                else
                {
                    main.core_Object.Modbus_Sender(129, int.Parse(textBox1.Text));
                    //IsCobotManual = true;
                }

            }
            catch { }
            
        }

        private void buttonJogPlus_Click(object sender, EventArgs e)
        {

        }

        bool Robot_Orgin_Status()
        {
            //2023.03.09 ::: 로봇 조인트 값 확인
            var joint1 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 270);
            var joint2 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 271);
            var joint3 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 272);
            var joint4 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 273);
            var joint5 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 274);
            var joint6 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 275);
            double[] JointPos = new double[6];
            JointPos[0] = double.Parse(joint1.strData);
            JointPos[1] = double.Parse(joint2.strData);
            JointPos[2] = double.Parse(joint3.strData);
            JointPos[3] = double.Parse(joint4.strData);
            JointPos[4] = double.Parse(joint5.strData);
            JointPos[5] = double.Parse(joint6.strData);

            //2023.03.20 ::: 로봇 태스크 좌표 값 확인
            var task1 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 400);
            var task2 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 401);
            var task3 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 402);
            var task4 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 403);
            var task5 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 404);
            var task6 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 405);
            double[] TaskPos = new double[6];
            TaskPos[0] = double.Parse(task1.strData);
            TaskPos[1] = double.Parse(task2.strData);
            TaskPos[2] = double.Parse(task3.strData);
            TaskPos[3] = double.Parse(task4.strData);
            TaskPos[4] = double.Parse(task5.strData);
            TaskPos[5] = double.Parse(task6.strData);

            //return Cores.Core_StepModule.Cobot_Pos_Compare(JointPos, Cores.Core_Object.GetObj_File.dArrayCobotWaitPos);
            //return Cores.Core_StepModule.Cobot_Pos_Compare(JointPos, Cores.Core_Object.GetCos_File.Joint[0]);
            return Cores.Core_StepModule.Cobot_Pos_Compare(TaskPos, Cores.Core_Object.GetCos_File.Joint[(int)MyActionXStepBuffer.Wait - 1]);
        }

        bool Robot_UnUsed_Status()
        {
            //2023.03.09 ::: 로봇 조인트 값 확인
            var joint1 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 270);
            var joint2 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 271);
            var joint3 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 272);
            var joint4 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 273);
            var joint5 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 274);
            var joint6 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 275);
            double[] JointPos = new double[6];
            JointPos[0] = double.Parse(joint1.strData);
            JointPos[1] = double.Parse(joint2.strData);
            JointPos[2] = double.Parse(joint3.strData);
            JointPos[3] = double.Parse(joint4.strData);
            JointPos[4] = double.Parse(joint5.strData);
            JointPos[5] = double.Parse(joint6.strData);

            //2023.03.20 ::: 로봇 태스크 좌표 값 확인
            var task1 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 400);
            var task2 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 401);
            var task3 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 402);
            var task4 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 403);
            var task5 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 404);
            var task6 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 405);
            double[] TaskPos = new double[6];
            TaskPos[0] = double.Parse(task1.strData);
            TaskPos[1] = double.Parse(task2.strData);
            TaskPos[2] = double.Parse(task3.strData);
            TaskPos[3] = double.Parse(task4.strData);
            TaskPos[4] = double.Parse(task5.strData);
            TaskPos[5] = double.Parse(task6.strData);

            //return Cores.Core_StepModule.Cobot_Pos_Compare(JointPos, Cores.Core_Object.GetObj_File.dArrayCobotWaitPos);
            //return Cores.Core_StepModule.Cobot_Pos_Compare(JointPos, Cores.Core_Object.GetCos_File.Joint[12]);
            return Cores.Core_StepModule.Cobot_Pos_Compare(TaskPos, Cores.Core_Object.GetCos_File.Joint[(int)MyActionXStepBuffer.Clearing]);
        }

        bool Robot_Down_Status()
        {
            //2023.03.20 ::: 로봇 태스크 좌표 값 확인
            var task1 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 400);
            var task2 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 401);
            var task3 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 402);
            var task4 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 403);
            var task5 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 404);
            var task6 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 405);
            double[] TaskPos = new double[6];
            TaskPos[0] = double.Parse(task1.strData);
            TaskPos[1] = double.Parse(task2.strData);
            TaskPos[2] = double.Parse(task3.strData);
            TaskPos[3] = double.Parse(task4.strData);
            TaskPos[4] = double.Parse(task5.strData);
            TaskPos[5] = double.Parse(task6.strData);

            bool retBool = false;
            for (int idx = 1; idx < 10; idx++)
            {
                retBool = Cores.Core_StepModule.Cobot_Pos_Compare(TaskPos, Cores.Core_Object.GetCos_File.Joint[idx]);
                if (retBool == true)
                {
                    //Console.WriteLine();
                    break;
                }
            }

            return retBool;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                //2023.03.10
                var manuCommand = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 128);
                main.core_Object.Modbus_Sender(128, int.Parse(textBox1.Text));
            }
            catch { }
           
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            int iTestSpd = trackBar1.Value;

            double iSpeed = 250 * iTestSpd * 0.01;
            int fastechSpeed = Cores.Fas_Func.PPS_To_mm((double)iSpeed);

            Cores.Fas_Motion.VelocityOverride(MOTOR, fastechSpeed);
        }

        //2023.05.03 ::: 얼라인 계산을 위한 콧핏 사용 버튼
        private void button3_Click(object sender, EventArgs e)
        {
            ////1번부터 13 14 Resume
            //if (OutStateBuff[12] == true && OutStateBuff[13] == true)
            //{
            //    OutStateBuff[12] = false;
            //    OutStateBuff[13] = false;
            //}
            //else
            //{
            //    OutStateBuff[12] = true;
            //    OutStateBuff[13] = true;
            //}
            //Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
            ////1번부터 15 16 Enable
            //if (OutStateBuff[14] == true && OutStateBuff[15] == true)
            //{
            //    OutStateBuff[14] = false;
            //    OutStateBuff[15] = false;
            //}
            //else
            //{
            //    OutStateBuff[14] = true;
            //    OutStateBuff[15] = true;
            //}
            //Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);

            var robotState = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 259);
            Externs.Robot_Modbus_Table.RobotState_Ver_1_1 cobotState = (Externs.Robot_Modbus_Table.RobotState_Ver_1_1)robotState.iData;

            if (cobotState != Externs.Robot_Modbus_Table.RobotState_Ver_1_1.COLLABORATIVE_RUNNING)
            {
                return;
            }

            bool[] OutStateBuff = null;
            OutStateBuff = Cores.Fas_Data.lstIO_OutState[COBOT];
            if (OutStateBuff[(int)COBOT_OUTPUT_COLL.HGC_Enable] == false && OutStateBuff[(int)COBOT_OUTPUT_COLL.HGC_Start] == false)
            {
                OutStateBuff[(int)COBOT_OUTPUT_COLL.HGC_Enable] = true;
                OutStateBuff[(int)COBOT_OUTPUT_COLL.HGC_Start] = true;
                Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var robotState = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 259);
            Externs.Robot_Modbus_Table.RobotState_Ver_1_1 cobotState = (Externs.Robot_Modbus_Table.RobotState_Ver_1_1)robotState.iData;

            if (cobotState != Externs.Robot_Modbus_Table.RobotState_Ver_1_1.HANDGUIDING_CONTROL_STANDBY)
            {
                return;
            }

            bool[] OutStateBuff = null;
            OutStateBuff = Cores.Fas_Data.lstIO_OutState[COBOT];
            if (OutStateBuff[(int)COBOT_OUTPUT_COLL.HGC_Cmp] == false && OutStateBuff[(int)COBOT_OUTPUT_COLL.HGC_End] == false)
            {
                OutStateBuff[(int)COBOT_OUTPUT_COLL.HGC_Cmp] = true;
                OutStateBuff[(int)COBOT_OUTPUT_COLL.HGC_End] = true;
                Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
            }
        }

        private void lbButton1_Load(object sender, EventArgs e)
        {
            //없음 비상정지 화면 버튼
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var manualComplted = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 144);
            var manualCommand = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 128);

            int iTechingOffsetInitCmd = manualCommand.iData >> 6 & 1;
            int iTechingOffsetInitCmp = manualComplted.iData >> 6 & 1;

            if (iTechingOffsetInitCmd == 0 && iTechingOffsetInitCmp == 0)
            {
                Core_Object.GetObj_File.iTechingInt = 1;
                listBox2.Items.Insert(0, $"{DateTime.Now} TECH INIT CMDS");
            }
            else if (iTechingOffsetInitCmd == 1 && iTechingOffsetInitCmp == 0)
            {
                //Core_Object.GetObj_File.iTechingInt = 1;
                //wait
            }
            else if (iTechingOffsetInitCmd == 0 && iTechingOffsetInitCmp == 1)
            {
                //Core_Object.GetObj_File.iTechingInt = 1;
                //error
            }
            else if (iTechingOffsetInitCmd == 1 && iTechingOffsetInitCmp == 1)
            {
                //Core_Object.GetObj_File.iTechingInt = 0;
                //auto
            }

        }

        void FuncTechingOffset()
        {
            if(Define.IsXaxisDebugMove) { return; }

            var robotState = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 259);
            Externs.Robot_Modbus_Table.RobotState_Ver_1_1 cobotState = (Externs.Robot_Modbus_Table.RobotState_Ver_1_1)robotState.iData;

            if (cobotState == Externs.Robot_Modbus_Table.RobotState_Ver_1_1.STANDALONE_RUNNING)
            {
                return;
            }

            var manualComplted = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 144);
            var manualCommand = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 128);

            int iTechingOffsetInitCmd = manualCommand.iData >> 6 & 1;
            int iTechingOffsetInitCmp = manualComplted.iData >> 6 & 1;

            if (iTechingOffsetInitCmd == 0 && iTechingOffsetInitCmp == 0)
            {
                //Core_Object.GetObj_File.iTechingInt = 1;
            }
            else if (iTechingOffsetInitCmd == 1 && iTechingOffsetInitCmp == 0)
            {
                //Core_Object.GetObj_File.iTechingInt = 1;
                //wait
                listBox2.Items.Insert(0, $"{DateTime.Now} TECH INIT WAIT");
            }
            else if (iTechingOffsetInitCmd == 0 && iTechingOffsetInitCmp == 1)
            {
                //Core_Object.GetObj_File.iTechingInt = 1;
                //error

                //listBox2.Items.Insert(0, $"{DateTime.Now} TECH INIT ERRS");
            }
            else if (iTechingOffsetInitCmd == 1 && iTechingOffsetInitCmp == 1)
            {
                //Core_Object.GetObj_File.iTechingInt = 0;
                //auto

                Core_Object.GetObj_File.iTechingInt = 0;

                listBox2.Items.Insert(0, $"{DateTime.Now} TECH INIT COMP");
            }
        }

        public void AlignVisible(bool visible)
        {
            button1.Visible = visible;
            button3.Visible = visible;
            button5.Visible = visible;
        }
        
    }
}
