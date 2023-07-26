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

namespace Forms
{
    public partial class FormDebug : Form
    {
        public FormDebug()
        {
            InitializeComponent();
        }

        public FormDebug(Project_Main.FormMain mainForm)
        {
            InitializeComponent();

            main = mainForm;
        }

        private Project_Main.FormMain main;
        public List<MetroButton> lstProcessBtn = new List<MetroButton>();

        private void FormDebug_Load(object sender, EventArgs e)
        {
            lstProcessBtn.Add(metroButton2);
            lstProcessBtn.Add(metroButton3);
            lstProcessBtn.Add(metroButton4);
            lstProcessBtn.Add(metroButton5);
            lstProcessBtn.Add(metroButton6);
            lstProcessBtn.Add(metroButton7);
            lstProcessBtn.Add(metroButton8);
            lstProcessBtn.Add(metroButton9);
            lstProcessBtn.Add(metroButton10);
            lstProcessBtn.Add(metroButton1);
            lstProcessBtn.Add(metroButton12);

            foreach (MetroButton processCommandButton in lstProcessBtn)
            {
                processCommandButton.Click += ProcessCommandButton_Click;
            }
        }

        private void ProcessCommandButton_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();

            MetroButton processCommandButton = sender as MetroButton;

            //const int CurrentState_None = 00;
            //const int CurrentState_Starting = 02;
            //const int CurrentState_Completing = 04;
            //const int CurrentState_Resetting = 06;
            //const int CurrentState_Holding = 07;
            //const int CurrentState_Unholding = 09;
            //const int CurrentState_Suspending = 10;
            //const int CurrentState_Suspended = 11;
            //const int CurrentState_Stopping = 13;
            //const int CurrentState_Aborting = 15;
            //const int CurrentState_Clearing = 17;

            PackML.Command cmd = PackML.Command.CurrentState_None;
            PackML.EModeMatrix mode = PackML.EModeMatrix.None_Matrix;
            bool IsModeCommand = false;

            switch (processCommandButton.Text)
            {
                default: break;

                case "START":
                    cmd = PackML.Command.CurrentState_Starting;
                    break;
                case "COMPLETE":
                    cmd = PackML.Command.CurrentState_Completing;
                    break;
                case "RESET":
                    cmd = PackML.Command.CurrentState_Resetting;
                    break;
                case "HOLD":
                    cmd = PackML.Command.CurrentState_Holding;
                    break;
                case "UNHOLD":
                    cmd = PackML.Command.CurrentState_Unholding;
                    break;
                case "UNSUSPEND":
                    //2023.01.06 확인 필요, 명령이 없네....
                    cmd = PackML.Command.CurrentState_Suspending;
                    break;
                case "CLEAR":
                    cmd = PackML.Command.CurrentState_Clearing;
                    break;
                case "STOP":
                    cmd = PackML.Command.CurrentState_Stopping;
                    break;
                case "ABORT":
                    cmd = PackML.Command.CurrentState_Aborting;
                    break;

                case "Auto":
                    IsModeCommand = true;
                    mode = PackML.EModeMatrix.AutomaticMode_Matrix;
                    break;

                case "Manual":
                    IsModeCommand = true;
                    mode = PackML.EModeMatrix.ManualMode_Matrix;
                    break;
            }

            if (IsModeCommand == false)
            {
                main.core_Object.processCore.Set_State_Matrix(cmd);
                listBox1.Items.Add($"{DateTime.Now} >>> {cmd}");
            }
            else
            {
                main.core_Object.processCore.Set_StateMode(mode);
                listBox1.Items.Add($"{DateTime.Now} >>> {mode}");
            }
        }
    }
}
