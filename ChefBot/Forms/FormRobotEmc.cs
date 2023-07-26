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
    public partial class FormRobotEmc : Form
    {
        public FormRobotEmc()
        {
            InitializeComponent();

            devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
              $"{System.Reflection.MethodBase.GetCurrentMethod().Name}");
        }

        public FormRobotEmc(Project_Main.FormMain mainForm)
        {
            InitializeComponent();

            devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
              $"{System.Reflection.MethodBase.GetCurrentMethod().Name}");

            main = mainForm;
        }

        private Project_Main.FormMain main;

        public ClearEventHandler ClearEvent;
        public delegate void ClearEventHandler();

        private void BtnOk_Click(object sender, EventArgs e)
        {
            ClearEvent();
            this.Close();
        }

        private void FormRobotEmc_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            //gripper open

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
                        if (openClose.iData == 0)
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
                        }

                        break;
                }
            }
            catch
            { }

        
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            //robot reboot
            bool[] OutStateBuff = null;
            OutStateBuff = Cores.Fas_Data.lstIO_OutState[Cores.Core_StepModule.CHEFX];
            OutStateBuff[(int)Cores.Core_StepModule.CHEFX_OUTPUT.RobotPower_CP] = true;
            OutStateBuff[(int)Cores.Core_StepModule.CHEFX_OUTPUT.RobotPower_CP_Spare] = true;
            OutStateBuff[(int)Cores.Core_StepModule.CHEFX_OUTPUT.RobotPower_CP_Demo] = true;
            Cores.Fas_Motion.SetOutput((int)Cores.Core_StepModule.IO_Board.EzEtherNetIO_1, OutStateBuff);
            System.Threading.Thread.Sleep(700);
            OutStateBuff[(int)Cores.Core_StepModule.CHEFX_OUTPUT.RobotPower_CP] = false;
            OutStateBuff[(int)Cores.Core_StepModule.CHEFX_OUTPUT.RobotPower_CP_Spare] = false;
            OutStateBuff[(int)Cores.Core_StepModule.CHEFX_OUTPUT.RobotPower_CP_Demo] = false;
            Cores.Fas_Motion.SetOutput((int)Cores.Core_StepModule.IO_Board.EzEtherNetIO_1, OutStateBuff);
        }

        public void Status()
        {
            //robot state, gripper state, pause state, emc state

            var gripper = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 21);
            var gripperOut = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 22);          

            if (devi.Define.eGripper == devi.EGRIPS.DH)
            {
                switch (gripper.iData)
                {
                    case 0:
                        break;
                    case 1:
                        if (gripperOut.iData == 0)
                        {
                            label1.BackColor = Color.Black;
                        }
                        else
                        {
                            label1.BackColor = Color.DodgerBlue;
                        }

                        break;
                    case 2:
                        label1.BackColor = Color.DodgerBlue;
                        break;
                    case 3:
                        break;
                }
            }
            else if (devi.Define.eGripper == devi.EGRIPS.ZIMMER)
            {
                switch (gripper.iData)
                {
                    case 0:
                        radioButton1.Enabled = true;
                        break;
                    case 1:
                        label1.BackColor = Color.DodgerBlue;
                        radioButton1.Enabled= false;
                        break;
                    case 2:
                        label1.BackColor = Color.Black;
                        radioButton1.Enabled = true;
                        break;
                    case 3:
                        break;
                }

                if (((gripper.iData >> 1) & 1) != 0 && ((gripper.iData >> 0) & 1) != 0)
                {

                }
                else if ((gripper.iData >> 1 & 1) != 0)
                {

                }
                else if ((gripper.iData >> 0 & 1) != 0)
                {

                }
                else
                {
                    //listLog = $"<<<{DateTime.Now:HH:mm:ss.fff} >>> 그리퍼 닫힘 동작 중 입니다.";
                    //IsErrorCheck = true;
                    //listLog = $"<<<{DateTime.Now:HH:mm:ss.fff} >>> 그리퍼 닫힘 동작 완료 하였습니다.";
                    //IsCobotManual = false;
                }

                if ((gripperOut.iData >> 1 & 1) != 0)
                {
                    main.core_Object.Modbus_Sender(Externs.Robot_Modbus_Table.Robot_Write.TOOL_IO, 2);
                }
                else if ((gripperOut.iData >> 0 & 1) != 0)
                {
                    main.core_Object.Modbus_Sender(Externs.Robot_Modbus_Table.Robot_Write.TOOL_IO, 1);
                }

            }

            var robotState = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 259);
            Externs.Robot_Modbus_Table.RobotState_Ver_1_1 cobotState = (Externs.Robot_Modbus_Table.RobotState_Ver_1_1)robotState.iData;

            if (cobotState == Externs.Robot_Modbus_Table.RobotState_Ver_1_1.INTERRUPTED)
            {
                label2.BackColor = Color.DodgerBlue;
            }
            else
            {
                label2.BackColor = Color.Black;
            }

            bool IsEmergency = true;
            IsEmergency &= !Cores.Fas_Data.lstIO_InState[Cores.Core_StepModule.CHEFY][(int)Cores.Core_StepModule.CHEFY_INPUT.L_EMC_SW];
            IsEmergency &= !Cores.Fas_Data.lstIO_InState[Cores.Core_StepModule.CHEFY][(int)Cores.Core_StepModule.CHEFY_INPUT.R_EMC_SW];
            IsEmergency &= Cores.Fas_Data.lstIO_InState[Cores.Core_StepModule.COBOT][(int)Cores.Core_StepModule.COBOT_INPUT.EMC_1];
            IsEmergency &= Cores.Fas_Data.lstIO_InState[Cores.Core_StepModule.COBOT][(int)Cores.Core_StepModule.COBOT_INPUT.EMC_2];

            if (IsEmergency)
            {
                label3.BackColor = Color.DodgerBlue;
            }
            else
            {
                label3.BackColor = Color.Black;
            }

        }
    }
}
