using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Cores.Core_StepModule;

namespace Forms
{
    public partial class FormDIO : Form
    {
        public FormDIO()
        {
            InitializeComponent();
        }

        public FormDIO(Project_Main.FormMain mainForm)
        {
            InitializeComponent();

            main = mainForm;

            devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
          $"{System.Reflection.MethodBase.GetCurrentMethod().Name}");
        }
        private Project_Main.FormMain main;

        List<Label> InNames = new List<Label>();
        List<Label> OutNames = new List<Label>();

        List<Label> InLabels = new List<Label>();
        List<Label> OutLabels = new List<Label>();

        List<Panel> Inputs = new List<Panel>();
        List<Panel> Outputs = new List<Panel>();

        List<Button> GetButtons = new List<Button>();

        List<RadioButton> GetRadioButtons = new List<RadioButton>();

        private int[] iPageNumber = new int[] {0,0 };
        private RB_Data eSelectedData = RB_Data.EzEtherNetIO_1;

        //lbInputName
        //lbOutputName

        const int On = 1;
        const int Off = 0;
        enum IO
        { 
            IN, OUT
        }

        enum RB_Data
        { 
           EzEtherNetIO_1, EzEtherNetIO_2, EzEtherNetIO_3, EzEtherNetIO_4, RoBot_Digital_IO, Max//RoBot_Tool_IO, RoBot_IO, Max
        }

        private void FormDIO_Load(object sender, EventArgs e)
        {
            this.BackColor = Parent.BackColor;
            int iRepNumber = 0;

            #region MyRegion
            InNames.Add(LabelPLC0);
            InNames.Add(LabelPLC1);
            InNames.Add(LabelPLC2);
            InNames.Add(LabelPLC3);
            InNames.Add(LabelPLC4);
            InNames.Add(LabelPLC5);
            InNames.Add(LabelPLC6);
            InNames.Add(LabelPLC7);
            InNames.Add(LabelPLC8);
            InNames.Add(LabelPLC9);
            InNames.Add(LabelPLC10);
            InNames.Add(LabelPLC11);
            InNames.Add(LabelPLC12);
            InNames.Add(LabelPLC13);
            InNames.Add(LabelPLC14);
            InNames.Add(LabelPLC15);

            OutNames.Add(LabelPC0);
            OutNames.Add(LabelPC1);
            OutNames.Add(LabelPC2);
            OutNames.Add(LabelPC3);
            OutNames.Add(LabelPC4);
            OutNames.Add(LabelPC5);
            OutNames.Add(LabelPC6);
            OutNames.Add(LabelPC7);
            OutNames.Add(LabelPC8);
            OutNames.Add(LabelPC9);
            OutNames.Add(LabelPC10);
            OutNames.Add(LabelPC11);
            OutNames.Add(LabelPC12);
            OutNames.Add(LabelPC13);
            OutNames.Add(LabelPC14);
            OutNames.Add(LabelPC15);
            #endregion

            #region MyRegion
            InLabels.Add(LabelPLCB0);
            InLabels.Add(LabelPLCB1);
            InLabels.Add(LabelPLCB2);
            InLabels.Add(LabelPLCB3);
            InLabels.Add(LabelPLCB4);
            InLabels.Add(LabelPLCB5);
            InLabels.Add(LabelPLCB6);
            InLabels.Add(LabelPLCB7);
            InLabels.Add(LabelPLCB8);
            InLabels.Add(LabelPLCB9);
            InLabels.Add(LabelPLCB10);
            InLabels.Add(LabelPLCB11);
            InLabels.Add(LabelPLCB12);
            InLabels.Add(LabelPLCB13);
            InLabels.Add(LabelPLCB14);
            InLabels.Add(LabelPLCB15);

            OutLabels.Add(LabelPCB0);
            OutLabels.Add(LabelPCB1);
            OutLabels.Add(LabelPCB2);
            OutLabels.Add(LabelPCB3);
            OutLabels.Add(LabelPCB4);
            OutLabels.Add(LabelPCB5);
            OutLabels.Add(LabelPCB6);
            OutLabels.Add(LabelPCB7);
            OutLabels.Add(LabelPCB8);
            OutLabels.Add(LabelPCB9);
            OutLabels.Add(LabelPCB10);
            OutLabels.Add(LabelPCB11);
            OutLabels.Add(LabelPCB12);
            OutLabels.Add(LabelPCB13);
            OutLabels.Add(LabelPCB14);
            OutLabels.Add(LabelPCB15);
            #endregion

            #region MyRegion
            Inputs.Add(PanelPLCB0);
            Inputs.Add(PanelPLCB1);
            Inputs.Add(PanelPLCB2);
            Inputs.Add(PanelPLCB3);
            Inputs.Add(PanelPLCB4);
            Inputs.Add(PanelPLCB5);
            Inputs.Add(PanelPLCB6);
            Inputs.Add(PanelPLCB7);
            Inputs.Add(PanelPLCB8);
            Inputs.Add(PanelPLCB9);
            Inputs.Add(PanelPLCB10);
            Inputs.Add(PanelPLCB11);
            Inputs.Add(PanelPLCB12);
            Inputs.Add(PanelPLCB13);
            Inputs.Add(PanelPLCB14);
            Inputs.Add(PanelPLCB15);

            Outputs.Add(PanelPCB0);
            Outputs.Add(PanelPCB1);
            Outputs.Add(PanelPCB2);
            Outputs.Add(PanelPCB3);
            Outputs.Add(PanelPCB4);
            Outputs.Add(PanelPCB5);
            Outputs.Add(PanelPCB6);
            Outputs.Add(PanelPCB7);
            Outputs.Add(PanelPCB8);
            Outputs.Add(PanelPCB9);
            Outputs.Add(PanelPCB10);
            Outputs.Add(PanelPCB11);
            Outputs.Add(PanelPCB12);
            Outputs.Add(PanelPCB13);
            Outputs.Add(PanelPCB14);
            Outputs.Add(PanelPCB15);
            #endregion

            #region MyRegion
            GetButtons.Add(BtnPlcPageTop);
            GetButtons.Add(BtnPlcPageUp);
            GetButtons.Add(BtnPlcPageDown);
            GetButtons.Add(BtnPlcPageBot);
            GetButtons.Add(BtnPCPageTop);
            GetButtons.Add(BtnPCPageUp);
            GetButtons.Add(BtnPCPageDown);
            GetButtons.Add(BtnPCPageBot);

            GetRadioButtons.Add(radioButton1);
            GetRadioButtons.Add(radioButton2);
            GetRadioButtons.Add(radioButton3);
            GetRadioButtons.Add(radioButton4);
            GetRadioButtons.Add(radioButton5);
            //GetRadioButtons.Add(radioButton6);
            //GetRadioButtons.Add(radioButton7);
            //GetRadioButtons.Add(radioButton8);

            //LBSoft.IndustrialCtrls.Buttons.LBButton[] inputLed = new LBSoft.IndustrialCtrls.Buttons.LBButton[Inputs.Count];
            //LBSoft.IndustrialCtrls.Buttons.LBButton[] outputLed = new LBSoft.IndustrialCtrls.Buttons.LBButton[Outputs.Count];
            //
            for (int idx = 0; idx < Inputs.Count; idx++)
            {
                //inputLed[idx] = new LBSoft.IndustrialCtrls.Buttons.LBButton();
                //inputLed[idx].BackColor = Color.Gray;
                //inputLed[idx].Size = Inputs[idx].Size;

                //Inputs[idx].BackColor = Color.Transparent;
                //Inputs[idx].Controls.Clear();
                //Inputs[idx].BackgroundImage = null;
                //Inputs[idx].Controls.Add(inputLed[idx]);
                Inputs[idx].BackgroundImage = global::ChefBot.Properties.Resources.LED_Gray; ;
                Inputs[idx].BackColor = Color.Transparent;
            }
            //
            for (int idx = 0; idx < Outputs.Count; idx++)
            {
                //outputLed[idx] = new LBSoft.IndustrialCtrls.Buttons.LBButton();
                //outputLed[idx].BackColor = Color.Gray;
                //outputLed[idx].Size = Outputs[idx].Size;

                //Outputs[idx].BackColor = Color.Transparent;
                //Outputs[idx].Controls.Clear();
                //Outputs[idx].BackgroundImage = null;
                //Outputs[idx].Controls.Add(inputLed[idx]);
                Outputs[idx].BackgroundImage = global::ChefBot.Properties.Resources.LED_Gray; ;
                Outputs[idx].BackColor = Color.Transparent;
            }

            //Inputs[0].BackgroundImage.Save($"{Application.StartupPath}\\InputLedBlue.jpg");
            //Outputs[0].BackgroundImage.Save($"{Application.StartupPath}\\OutputLedOragne.jpg");


            foreach (Button btn in GetButtons)
            {
                btn.Click += Btn_Click;
            }

            foreach (RadioButton rb in GetRadioButtons)
            {
                rb.CheckedChanged += Rb_CheckedChanged;
                rb.Text = $"{(RB_Data)iRepNumber}";
                iRepNumber++;
            }

            foreach (Panel panel in Outputs)
            {
                panel.Click += Panel_Click;
                panel.MouseDown += Panel_MouseDown;
                panel.MouseUp += Panel_MouseUp;
            }

            DefaultGUI();

            GetRadioButtons[(int)RB_Data.EzEtherNetIO_4].Checked = true;
            GetRadioButtons[(int)RB_Data.EzEtherNetIO_4].BackColor = devi.Define.colorMainButtonBoldColor;

            #endregion

            
        }

        public void AlignVisible(bool visible)
        {
            panel1.Visible = visible;           
        }

        /// <summary>
        /// 아이오 출려 끄기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Panel_MouseUp(object sender, MouseEventArgs e)
        {
            //throw new NotImplementedException();
            if (radioButtonUsed.Checked && eSelectedData != RB_Data.RoBot_Digital_IO && checkBoxOutStatic.Checked != true)
            {
                int nBdNmber = (int)eSelectedData + 1;
                int iBitIndex = -1;
                int iBitNumber = -1;
                Panel output = sender as Panel;
                for (int i = 0; i < Outputs.Count; i++)
                {
                    if (output == Outputs[i])
                    {
                        iBitIndex = (1 << i);
                        iBitNumber = i;
                        break;
                    }
                }

                uint outBuff = Cores.Fas_Data.usOutputState[(int)eSelectedData];
                bool[] OutStateBuff = Cores.Fas_Data.lstIO_OutState[(int)eSelectedData];                

                //devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Debug,
                //  $"{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                //  $"| Debug GPIO Output {eSelectedData} | 0x{outBuff:X2}");

                //ResetBit
                //dat &= ~(1 << idx);
                //outBuff &= (uint)~(1 << iBitNumber);

                outBuff |= (uint)iBitIndex;

                //devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Debug,
                //$"{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                //$"| Debug GPIO Output {eSelectedData} | 0x{outBuff:X2}");

                if (OutStateBuff[iBitNumber])
                {
                    OutStateBuff[iBitNumber] = !OutStateBuff[iBitNumber];
                    Cores.Fas_Motion.SetOutput(nBdNmber, OutStateBuff);
                }
            }
        }

        /// <summary>
        /// 아이오 출력 켜기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Panel_MouseDown(object sender, MouseEventArgs e)
        {
            //throw new NotImplementedException();
            if (radioButtonUsed.Checked && eSelectedData != RB_Data.RoBot_Digital_IO)
            {
                int nBdNmber = (int)eSelectedData + 1;
                int iBitIndex = -1;
                int iBitNumber = -1;
                Panel output = sender as Panel;

                for (int i = 0; i < Outputs.Count; i++)
                {
                    if (output == Outputs[i])
                    {
                        iBitIndex = (1 << i);
                        iBitNumber = i;
                        break;
                    }
                }

                uint outBuff = Cores.Fas_Data.usOutputState[(int)eSelectedData];
                bool[] OutStateBuff = Cores.Fas_Data.lstIO_OutState[(int)eSelectedData];
              

                //devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Debug, 
                //    $"{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                //    $"| Debug GPIO Output {eSelectedData} | 0x{outBuff:X2}");                

                outBuff |= (uint)iBitIndex;

                //devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Debug,
                //    $"{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                //    $"| Debug GPIO Output {eSelectedData} | 0x{outBuff:X2}");

                if (Cores.Fas_Data.lstIO_OutState[(int)eSelectedData][iBitNumber])
                {
                    OutStateBuff[iBitNumber] = !OutStateBuff[iBitNumber];
                    Cores.Fas_Motion.SetOutput(nBdNmber, OutStateBuff);

                }
                else
                {
                    Cores.Fas_Motion.SetOutput(nBdNmber, On, outBuff);
                }



            }
        }

        private void Panel_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();

            if (radioButtonUsed.Checked && eSelectedData != RB_Data.RoBot_Digital_IO)
            { 
                
            }
        }

        /// <summary>
        /// 선택된 라디오 버튼에 따라 입출력 데이터를 표시 한다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Rb_CheckedChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            for (int i = 0; i < GetRadioButtons.Count; i++)
            {
                if (GetRadioButtons[i].Checked)
                {
                    eSelectedData = (RB_Data)i;
                    GetRadioButtons[i].BackColor = devi.Define.colorMainButtonBoldColor;


                    if (eSelectedData == RB_Data.RoBot_Digital_IO)
                    {
                        for (int idx = 0; idx < 16; idx++)
                        {
                            InLabels[idx].Text  = Cores.Core_Object.GetDIO_File.OutLabels[(int)RB_Data.EzEtherNetIO_4 * 16 + idx];
                            InNames[idx].Text   = Cores.Core_Object.GetDIO_File.OutNames[(int)RB_Data.EzEtherNetIO_4 * 16 + idx];

                            OutLabels[idx].Text = Cores.Core_Object.GetDIO_File.InLabels[(int)RB_Data.EzEtherNetIO_4 * 16 + idx];
                            OutNames[idx].Text  = Cores.Core_Object.GetDIO_File.InNames[(int)RB_Data.EzEtherNetIO_4 * 16 + idx];

                            Inputs[i].BackgroundImage = global::ChefBot.Properties.Resources.LED_Gray;
                            Outputs[i].BackgroundImage = global::ChefBot.Properties.Resources.LED_Gray;
                        }
                    }
                    else
                    {
                        for (int idx = 0; idx < 16; idx++)
                        {
                            InLabels[idx].Text = Cores.Core_Object.GetDIO_File.InLabels[i * 16 + idx];
                            InNames[idx].Text  = Cores.Core_Object.GetDIO_File.InNames[i * 16 + idx];

                            OutLabels[idx].Text = Cores.Core_Object.GetDIO_File.OutLabels[i * 16 + idx];
                            OutNames[idx].Text  = Cores.Core_Object.GetDIO_File.OutNames[i * 16 + idx];


                            Inputs[i].BackgroundImage = global::ChefBot.Properties.Resources.LED_Gray;
                            Outputs[i].BackgroundImage = global::ChefBot.Properties.Resources.LED_Gray;
                        }
                    }
                    this.Refresh();
                }
                else
                {
                    GetRadioButtons[i].BackColor = devi.Define.colorSubButtonColor;
                }
            }
        }

        /// <summary>
        /// 표시되는 아이오 화면
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();

            Button btn = sender as Button;
            switch (btn.Name)
            {
                default:
                    break;

                case "BtnPlcPageTop":
                    break;
                case "BtnPlcPageUp":
                    break;
                case "BtnPlcPageDown":
                    break;
                case "BtnPlcPageBot":
                    break;
                case "BtnPCPageTop":
                    break;
                case "BtnPCPageUp":
                    break;
                case "BtnPCPageDown":
                    break;
                case "BtnPCPageBot":
                    break;
            }
        }

        /// <summary>
        /// 화면 리플레시 전용
        /// </summary>
        public void UpdateInformation()
        {   
            //Cores.Fas_Data _item = obj as Cores.Fas_Data;
            //if (this.InvokeRequired)
            //{
            //    this.Invoke(new MethodInvoker(delegate ()
            //    {
            //       
            //    }));
            //}

            if (eSelectedData != RB_Data.RoBot_Digital_IO)
            {
                for (int i = 0; i < Cores.Fas_Data.lstIO_InState[(int)eSelectedData].Length; i++)
                {
                    if (Cores.Fas_Data.lstIO_InState[(int)eSelectedData][i])
                    {
                        Inputs[i].BackgroundImage = global::ChefBot.Properties.Resources.InputLedBlue;
                    }
                    else
                    {
                        Inputs[i].BackgroundImage = global::ChefBot.Properties.Resources.LED_Gray;
                    }

                    //Inputs[i].BackgroundImage = global::Kyuchon_Robot.Properties.Resources.InputLedBlue;
                }
                for (int i = 0; i < Cores.Fas_Data.lstIO_OutState[(int)eSelectedData].Length; i++)
                {
                    if (Cores.Fas_Data.lstIO_OutState[(int)eSelectedData][i])
                    {
                        Outputs[i].BackgroundImage = global::ChefBot.Properties.Resources.OutputLedOragne;
                    }
                    else
                    {
                        Outputs[i].BackgroundImage = global::ChefBot.Properties.Resources.LED_Gray;
                    }
                }
            }
            //this.Refresh();
            //switch (eSelectedData)
            //{
            //    case RB_Data.EzEtherNetIO_1:
            //        //lbInputName.Text = "입력 (1)";
            //        //lbOutputName.Text = "입력 (2)";
            //        break;
            //    case RB_Data.EzEtherNetIO_2:
            //        break;
            //    case RB_Data.EzEtherNetIO_3:
            //        break;
            //    case RB_Data.EzEtherNetIO_4:

            //        break;

            //    case RB_Data.RoBot_Digital_IO:
            //        break;

            //    default: break;
            //}
        }

        void DefaultGUI()
        {
            int fontSize = 11;
            foreach (RadioButton btn in GetRadioButtons)
            {
                btn.BackColor = devi.Define.colorSubButtonColor;
                btn.ForeColor = Color.White;
                btn.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            }
            this.labelRobotVersion.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            this.labelRobotVersion.Text = null;
            this.labelRobotState.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            this.labelRobotState.Text = null;




            fontSize = 13;
            lbInputName.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            lbOutputName.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);

            labelCobotStatusHeader.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);

            TreeNode svrNode = new TreeNode("두산 협동 로봇");

            TreeNode jointSpace = new TreeNode("조인트 공간");
            jointSpace.Nodes.Add("현재 각도");
            jointSpace.Nodes.Add("현재 속도");

            TreeNode taskSpace_base = new TreeNode("태스크 공간(베이스)");
            taskSpace_base.Nodes.Add("현재 좌표");
            taskSpace_base.Nodes.Add("현재 속도");
            //taskSpace_base.Nodes.Add("현재 TCP속도");

            //TreeNode taskSpace_world = new TreeNode("태스크 공간(월드)");
            //taskSpace_world.Nodes.Add("현재 좌표");
            //taskSpace_world.Nodes.Add("현재 속도");
            //taskSpace_world.Nodes.Add("월드/베이스 관계");

            TreeNode taskSpace_user = new TreeNode("태스크 공간(사용자)");
            //TreeNode taskSpace_user = new TreeNode("툴 옵셋");
            taskSpace_user.Nodes.Add("툴 옵셋");
            //taskSpace_user.Nodes.Add("현재 속도");
            //taskSpace_user.Nodes.Add("사용자 좌표게 ID");
            //taskSpace_user.Nodes.Add("부모 좌표계");

            TreeNode force_torque = new TreeNode("힘/토크");
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

            TreeNode gpio = new TreeNode("IO");
            gpio.Nodes.Add("플렌지 DI");
            gpio.Nodes.Add("플렌지 DO");
            //gpio.Nodes.Add("디지털 입력");
            //gpio.Nodes.Add("디지털 출력");

            TreeNode etc = new TreeNode("기타");
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
            List<TreeNode> treeNodes = GetAllNodes(svrNode);
            string strNodeHeader = null;
            for (int ux = 0; ux < dgv.RowCount  -2; ux++)
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

            dgv.Rows[19].Cells[0].Value = "인버터 온도";

            dgv.AllowUserToAddRows = false;
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

        /// <summary>
        /// 로봇 버젼 표시
        /// </summary>
        /// <param name="str"></param>
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
                    if (eSelectedData == RB_Data.RoBot_Digital_IO)
                    {

                        for (int i = 0; i < 16; i++)
                        {
                            if (((accessData.iData >> i) & 1) != 0)
                            {
                                Inputs[i].BackgroundImage = global::ChefBot.Properties.Resources.InputLedBlue;
                            }
                            else
                            {
                                Inputs[i].BackgroundImage = global::ChefBot.Properties.Resources.LED_Gray;
                            }
                        }
                    }
                    break;

                case 1://Digital Output
                    if (eSelectedData == RB_Data.RoBot_Digital_IO)
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            if (((accessData.iData >> i) & 1) != 0)
                            {
                                Outputs[i].BackgroundImage = global::ChefBot.Properties.Resources.OutputLedOragne;
                            }
                            else
                            {
                                Outputs[i].BackgroundImage = global::ChefBot.Properties.Resources.LED_Gray;
                            }
                        }
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

        private void button1_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            bool[] OutStateBuff = null;
            bool[] InStateBuff = null;
            OutStateBuff = Cores.Fas_Data.lstIO_OutState[COBOT];
            InStateBuff = Cores.Fas_Data.lstIO_InState[COBOT];
            switch (btn.Name)
            {
                case "button1":
                    for (int i = 0; i < OutStateBuff.Length; i++)
                    {
                        OutStateBuff[i] = false;
                    }
                    Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                    Thread.Sleep(500);
                    OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset1] = true;
                    OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset2] = true;
                    Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                    Thread.Sleep(500);
                    OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset1] = false;
                    OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset2] = false;
                    Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                    break;
                case "button2":
                    //OutStateBuff[(int)COBOT_OUTPUT.Cobot_On] = true;
                    //OutStateBuff[(int)COBOT_OUTPUT.Cobot_Off] = false;
                    //Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                    //Thread.Sleep(500);
                    //OutStateBuff[(int)COBOT_OUTPUT.Cobot_On] = false;
                    //OutStateBuff[(int)COBOT_OUTPUT.Cobot_Off] = false;
                    Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                    break;
                case "button3":
                    OutStateBuff[(int)COBOT_OUTPUT.Task_Pause] = true;
                    OutStateBuff[(int)COBOT_OUTPUT.Task_Stop] = true;
                    Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                    break;
                case "button4":
                    OutStateBuff[(int)COBOT_OUTPUT.Remmote_On1] = true;
                    OutStateBuff[(int)COBOT_OUTPUT.Remmote_On2] = true;
                    Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                    break;
                case "button5":
                    OutStateBuff[(int)COBOT_OUTPUT.Serovo_On] = true;
                    Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                    Thread.Sleep(500);
                    OutStateBuff[(int)COBOT_OUTPUT.Serovo_On] = false;
                    Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                    break;
                case "button6":
                    OutStateBuff[(int)COBOT_OUTPUT.Task_Start] = true;
                    Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                    Thread.Sleep(500);
                    OutStateBuff[(int)COBOT_OUTPUT.Task_Start] = false;
                    Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                    break;
                case "button7":
                    OutStateBuff[(int)COBOT_OUTPUT.Task_Pause] = false;
                    Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                    Thread.Sleep(500);
                    OutStateBuff[(int)COBOT_OUTPUT.Task_Pause] = true;
                    Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                    break;
                case "button8":
                    OutStateBuff[(int)COBOT_OUTPUT.Task_Resume] = true;
                    Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                    Thread.Sleep(500);
                    OutStateBuff[(int)COBOT_OUTPUT.Task_Resume] = false;
                    Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                    break;

                case "button9":
                    OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset1] = true;
                    OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset2] = true;
                    Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                    Thread.Sleep(500);
                    OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset1] = false;
                    OutStateBuff[(int)COBOT_OUTPUT.Cobot_Reset2] = false;
                    Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                    break;

                case "button10":
                    //1번부터 13 14 Resume
                    if (OutStateBuff[(int)COBOT_OUTPUT_COLL.HGC_Cmp] == true && OutStateBuff[(int)COBOT_OUTPUT_COLL.HGC_End] == true)
                    {
                        OutStateBuff[12] = false;
                        OutStateBuff[13] = false;
                    }
                    else
                    {
                        OutStateBuff[12] = true;
                        OutStateBuff[13] = true;
                    }
                    Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);                   
                    break;

                case "button11":
                    //1번부터 15 16 Enable
                    if (OutStateBuff[(int)COBOT_OUTPUT_COLL.HGC_Enable] == true && OutStateBuff[(int)COBOT_OUTPUT_COLL.HGC_Start] == true)
                    {
                        OutStateBuff[14] = false;
                        OutStateBuff[15] = false;
                    }
                    else
                    {
                        OutStateBuff[14] = true;
                        OutStateBuff[15] = true;
                    }
                    Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                    break;

                case "button12":
                    //Auto Reset Resume
                    if (OutStateBuff[10] == true && OutStateBuff[11] == true)
                    {
                        OutStateBuff[10] = false;
                        OutStateBuff[11] = false;
                    }
                    else
                    {
                        OutStateBuff[10] = true;
                        OutStateBuff[11] = true;
                    }
                    Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_4, OutStateBuff);
                    break;
            }
        }
    }
}
