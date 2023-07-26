using Cores;
using devi;
using Project_Main;
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
using System.Windows.Interop;

namespace Forms
{
    public partial class FormErrorBox : Form
    {
        public FormErrorBox()
        {
            InitializeComponent();

            this.TopMost = true;
        }

        public FormErrorBox(FormMain formMain)
        {
            InitializeComponent();

            this.TopMost = true;

            main = formMain;

            devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
          $"{System.Reflection.MethodBase.GetCurrentMethod().Name}");
        }

        private FormMain main;
        public ClearEventHandler ClearEvent;
        public delegate void ClearEventHandler();

        public FormErrorBox(AlarmLevel alarmLevel, string name, string desc)
        {
            InitializeComponent();

            EAlarmLevel = alarmLevel;

            this.TopMost = true;

            strName = name;
            strDesc = desc;
        }

        private AlarmLevel EAlarmLevel;
        const int rowCount = 7;
        string strName = null;
        string strDesc = null;


        public enum AlarmLevel
        { 
            Warning, Error, Fetal, Unkown,
        }

        private void FormErrorBox_Load(object sender, EventArgs e)
        {
            int fontSize = 12;
            radioButton1.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            radioButton2.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            radioButton3.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            label1.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            label2.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            label3.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            buttonAlarmClear.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);


            Alarm_Init_DatagridView(ref dataGridView1);

            //Update_Alarm(strName, strDesc, (int)EAlarmLevel);

            timer1.Start();
        }
        void Alarm_Init_DatagridView(ref DataGridView dgv)
        {
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

            dgv.ColumnCount = 5;
            dgv.RowCount = rowCount;
            dgv.ColumnHeadersVisible = true;

            dgv.Columns[0].HeaderText = $"날짜";
            dgv.Columns[0].Width = 100;
            dgv.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dgv.Columns[1].HeaderText = $"시간";
            dgv.Columns[1].Width = 110;

            dgv.Columns[2].HeaderText = $"이름";
            dgv.Columns[2].Width = 150;

            dgv.Columns[3].HeaderText = $"내용";
            dgv.Columns[3].Width = 150;

            dgv.Columns[4].HeaderText = $"레벨";
            dgv.Columns[4].Width = 120;
            //
            //dgv.Columns[5].HeaderText = $"해제";
            //dgv.Columns[5].Width = 150;
            //
            //dgv.Columns[6].HeaderText = $"시간";
            //dgv.Columns[6].Width = 150;

            for (int ix = 0; ix < dgv.ColumnCount; ix++)
            {
                if (ix == 3)
                {
                    dgv.Columns[ix].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgv.Columns[ix].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                dgv.Columns[ix].Resizable = DataGridViewTriState.False;
                dgv.Columns[ix].SortMode = DataGridViewColumnSortMode.NotSortable;

            }

            for (int ux = 0; ux < dgv.RowCount; ux++)
            {
                //dgv.Rows[ux].Cells[0].Value = treeNodes[ux].Text;
                if (ux % 2 == 0)
                {
                    //strNodeHeader = treeNodes[ux].Text;
                    dgv.Rows[ux].HeaderCell.Style.BackColor = Color.DarkGray;
                    dgv.Rows[ux].Cells[0].Style.BackColor = Color.White;
                }
                else
                {
                    dgv.Rows[ux].HeaderCell.Style.BackColor = Color.DarkGray;
                    dgv.Rows[ux].Cells[0].Style.BackColor = Color.AliceBlue;
                }

                dgv.Rows[ux].Height = 50;

                int fontSize = 11;
                for (int col = 0; col < dgv.Columns.Count; col++)
                {
                    dgv.Rows[ux].Cells[col].Style.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
                }
            }


            dgv.ClearSelection();

        }

        public void Update_Alarm(string name, string desc, int level)
        {
            DataGridView dgv = dataGridView1;

            List<List<string>> alarmBuff = new List<List<string>>();
            for (int row = 0; row < dgv.RowCount - 1; row++)
            {
                List<string> colBuff = new List<string>();
                for (int col = 0; col < dgv.ColumnCount; col++)
                {
                    if (dgv.Rows[row].Cells[col].Value == null)
                    {
                        colBuff.Add(null);
                    }
                    else
                    {
                        colBuff.Add(dgv.Rows[row].Cells[col].Value.ToString());
                    }

                }
                alarmBuff.Add(colBuff);
            }

            List<string> colData = new List<string>();
            colData.Add(DateTime.Now.ToString("yyyy-MM-dd"));
            colData.Add(DateTime.Now.ToString("HH:mm:ss.fff"));
            colData.Add(name);
            colData.Add(desc);
            switch (level)
            {
                case 1:
                    colData.Add("Warning");
                    radioButton1.Checked = true;
                    break;
                case 2:
                    colData.Add("Error");
                    radioButton2.Checked = true;
                    break;

                case 3:
                    colData.Add("Fatal");
                    radioButton3.Checked = true;
                    break;

                default:
                    colData.Add("Unkown");
                    break;
            }
            alarmBuff.Insert(0, colData);
            //Console.WriteLine();
            for (int row = 0; row < dgv.RowCount; row++)
            {

                for (int col = 0; col < dgv.ColumnCount; col++)
                {
                    dgv.Rows[row].Cells[col].Value = alarmBuff[row][col];
                }

            }
        }

        private void buttonAlarmClear_Click(object sender, EventArgs e)
        {
            //2023.05.23 ::: 프로세스 상태가 어보트일 경우....


            Common.FormMessageBox msg;

            var robotState = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 259);
            Externs.Robot_Modbus_Table.RobotState_Ver_1_1 cobotState = (Externs.Robot_Modbus_Table.RobotState_Ver_1_1)robotState.iData;

            //if (Fas_Data.lstIO_InState[Core_StepModule.COBOT][Core_StepModule.COBOT_INPUT.])

            var closeOpen = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 21);
            if (closeOpen.iData == 2 && main.core_Object.processCore.eCurrState == PackML.ECurrState.CurrentState_Aborted)
            {
                msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None, "그리퍼를 수동으로 열림 후 알람 해제 바랍니다.");
                msg.ShowDialog();
                return;
            }

            int resValue = 1;
            resValue = Fas_Data.lstIO_InState[Core_StepModule.CHEFY][(int)Core_StepModule.CHEFY_INPUT.L_EMC_SW] ? 1 : 0;
            if (resValue == 1)
            {
                msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None, "왼쪽 비상정지 버튼 해제후 알람 해제 바랍니다.");
                msg.ShowDialog();
                return;
            }

            resValue = Fas_Data.lstIO_InState[Core_StepModule.CHEFY][(int)Core_StepModule.CHEFY_INPUT.R_EMC_SW] ? 1 : 0;
            if (resValue == 1)
            {
                msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None, "오른쪽 비상정지 버튼 해제후 알람 해제 바랍니다.");
                msg.ShowDialog();
                return;
            }

            resValue = Fas_Data.lstIO_InState[Core_StepModule.CHEFY][(int)Core_StepModule.CHEFY_INPUT.L_PAUSE_SW] ? 1 : 0;
            if (resValue == 1)
            {
                msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None, "왼쪽 일시정지 버튼 해제후 알람 해제 바랍니다.");
                msg.ShowDialog();
                return;
            }

            resValue = Fas_Data.lstIO_InState[Core_StepModule.CHEFY][(int)Core_StepModule.CHEFY_INPUT.R_PAUSE_SW] ? 1 : 0;
            if (resValue == 1)
            {
                msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None, "오른쪽 일시정지 버튼 해제후 알람 해제 바랍니다.");
                msg.ShowDialog();
                return;
            }
            Define.IsRobotReCoverlyMode = false;
            ClearEvent();
            this.Close();
            //Dispose(true);
        }

        bool IsManualGripperUsed = false;
        private void buttonGripperOpen_Click(object sender, EventArgs e)
        {

            try
            {
                switch (Define.eGripper)
                {
                    case EGRIPS.DH:
                        var open = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 22);
                        if (((open.iData >> 1) & 1) != 0 && ((open.iData >> 0) & 1) != 0)
                        {
                            //M-Serize
                            //main.core_Object.Modbus_Sender(Externs.Robot_Modbus_Table.Robot_Write.TOOL_IO, 2);
                            //main.core_Object.Modbus_Sender(Externs.Robot_Modbus_Table.Robot_Write.TOOL_IO, 3);

                            //E-Serize
                            main.core_Object.Modbus_Sender(Externs.Robot_Modbus_Table.Robot_Write.TOOL_IO, 1);
                            main.core_Object.Modbus_Sender(Externs.Robot_Modbus_Table.Robot_Write.TOOL_IO, 2);
                            //listMsg = "그리퍼 열림 동작 명령 실행 하였습니다.";
                            //listLog = $"<<<{DateTime.Now:HH:mm:ss.fff} >>> {listMsg} {dummyMsg.PadRight(15, dummyMsg[0]).PadLeft(32)}";
                        }
                        break;

                    case EGRIPS.ZIMMER:
                        var openClose = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 21);
                        if (openClose.iData == 0 || openClose.iData == 2)
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

                            //listMsg = "그리퍼 열림 동작 명령 실행 하였습니다.";
                            //listLog = $"<<<{DateTime.Now:HH:mm:ss.fff} >>> {listMsg} {dummyMsg.PadRight(15, dummyMsg[0]).PadLeft(32)}";


                            //Thread.Sleep(500);
                            //main.core_Object.Modbus_Sender(Externs.Robot_Modbus_Table.Robot_Write.TOOL_IO, 1);

                            IsManualGripperUsed = true;
                        }

                        break;
                }
            }
            catch
            { }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //2023.06.14 ::: 모드버스 클리어
            //로봇 상태에 따라서도 추가 할 것
            var toolOut = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 22);
            var toolIn = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 21);
            if (Define.eGripper == EGRIPS.DH)
            {
                if (((toolOut.iData >> 1) & 1) != 0 && ((toolOut.iData >> 0) & 1) != 0)
                {
                    //listLog = $"<<<{DateTime.Now:HH:mm:ss.fff} >>> 그리퍼 열림 동작 중 입니다.";
                    //IsErrorCheck = true;
                }
                else
                {
                    //listLog = $"<<<{DateTime.Now:HH:mm:ss.fff} >>> 그리퍼 열림 동작 완료 하였습니다.";
                    //IsCobotManual = false;
                }

                switch (toolIn.iData)
                {
                    case 0:
                        break;
                    case 1:
                        if (toolOut.iData == 0)
                        {
                            buttonGripperOpen.Enabled = false;
                            //buttonGripperClose.Enabled = true;
                        }
                        else
                        {
                            buttonGripperOpen.Enabled = true;
                            //buttonGripperClose.Enabled = false;
                        }

                        break;
                    case 2:
                        buttonGripperOpen.Enabled = true;
                        //buttonGripperClose.Enabled = false;
                        break;
                    case 3:
                        break;
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
                    //listLog = $"<<<{DateTime.Now:HH:mm:ss.fff} >>> 그리퍼 열림 동작 완료 하였습니다.";
                    //IsCobotManual = false;
                }
                else
                {
                    //listLog = $"<<<{DateTime.Now:HH:mm:ss.fff} >>> 그리퍼 열림 동작 중 입니다.";
                    //IsErrorCheck = true;
                }
              

                if (IsManualGripperUsed == true)
                {

                    IsManualGripperUsed = false;
                    if ((toolOut.iData >> 1 & 1) != 0)
                    {
                        main.core_Object.Modbus_Sender(Externs.Robot_Modbus_Table.Robot_Write.TOOL_IO, 2);
                    }
                    else if ((toolOut.iData >> 0 & 1) != 0)
                    {
                        main.core_Object.Modbus_Sender(Externs.Robot_Modbus_Table.Robot_Write.TOOL_IO, 1);
                    }
                }


                switch (toolIn.iData)
                {
                    case 0:
                        buttonGripperOpen.Enabled = true;
                        //buttonGripperClose.Enabled = false;
                        break;
                    case 1:
                        buttonGripperOpen.Enabled = false;
                        //buttonGripperClose.Enabled = true;


                        break;
                    case 2:
                        buttonGripperOpen.Enabled = true;
                        //buttonGripperClose.Enabled = false;
                        break;
                    case 3:
                        break;
                }
            }        


            if (Fas_Data.lstAxis_State[(int)EAxis_Status.Motion_Pause] == true)
            {
                if (Fas_Data.lstIO_InState[Cores.Core_StepModule.CHEFY][(int)Cores.Core_StepModule.CHEFY_INPUT.L_PAUSE_SW])
                {
                    if (lbLedSwLeftPause.State != LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Blink)
                    {
                        lbLedSwLeftPause.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Blink;
                    }
                }
                else
                {
                    if (lbLedSwLeftPause.State != LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Off)
                    {
                        lbLedSwLeftPause.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Off;
                    }
                }

                if (Fas_Data.lstIO_InState[Cores.Core_StepModule.CHEFY][(int)Cores.Core_StepModule.CHEFY_INPUT.R_PAUSE_SW])
                {
                    if (lbLedSwRightPause.State != LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Blink)
                    {
                        lbLedSwRightPause.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Blink;
                    }
                }
                else
                {
                    if (lbLedSwRightPause.State != LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Off)
                    {
                        lbLedSwRightPause.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Off;
                    }
                }
            }
            else
            {
                if (lbLedSwLeftPause.State != LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Off)
                {
                    lbLedSwLeftPause.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Off;
                }
                if (lbLedSwRightPause.State != LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Off)
                {
                    lbLedSwRightPause.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Off;
                }
            }

            
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            Common.FormMessageBox msg;

            int resValue = 1;
            resValue = Fas_Data.lstIO_InState[Core_StepModule.CHEFY][(int)Core_StepModule.CHEFY_INPUT.L_EMC_SW] ? 1 : 0;
            if (resValue == 1)
            {
                msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None, "왼쪽 비상정지 버튼 해제후 알람 해제 바랍니다.");
                msg.ShowDialog();
                return;
            }

            resValue = Fas_Data.lstIO_InState[Core_StepModule.CHEFY][(int)Core_StepModule.CHEFY_INPUT.R_EMC_SW] ? 1 : 0;
            if (resValue == 1)
            {
                msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None, "오른쪽 비상정지 버튼 해제후 알람 해제 바랍니다.");
                msg.ShowDialog();
                return;
            }

            resValue = Fas_Data.lstIO_InState[Core_StepModule.CHEFY][(int)Core_StepModule.CHEFY_INPUT.L_PAUSE_SW] ? 1 : 0;
            if (resValue == 1)
            {
                msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None, "왼쪽 일시정지 버튼 해제후 알람 해제 바랍니다.");
                msg.ShowDialog();
                return;
            }

            resValue = Fas_Data.lstIO_InState[Core_StepModule.CHEFY][(int)Core_StepModule.CHEFY_INPUT.R_PAUSE_SW] ? 1 : 0;
            if (resValue == 1)
            {
                msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None, "오른쪽 일시정지 버튼 해제후 알람 해제 바랍니다.");
                msg.ShowDialog();
                return;
            }


            Define.IsRobotReCoverlyMode = true;

            ClearEvent();

            RadioButton rb = sender as RadioButton;
            rb.Checked= false;
        }
    }
}
