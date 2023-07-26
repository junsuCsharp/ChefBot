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

namespace Forms
{
    public partial class UcCooker : UserControl
    {
        public UcCooker()
        {
            InitializeComponent();
        }

        private string strName = null;
        private int iMyIndex = -1;

        private int iSetMinTime = 0;
        private int iSetSecTime = 0;

        private bool IsChikenForceOut = false;

        public SetTimeChangeEventHandler SetTimeChangeEvent;
        public delegate void SetTimeChangeEventHandler(int index, int min, int sec);

        public ChienForceOutTrggierEventHandler ChienForceOutTrggierEvent;
        public delegate void ChienForceOutTrggierEventHandler(int index, bool IsExist);

        public ChienForceDelTrggierEventHandler ChienForceDelTrggierEvent;
        public delegate void ChienForceDelTrggierEventHandler(int index, bool IsDelete);

        public ChienForceInTrggierEventHandler ChienForceInTrggierEvent;
        public delegate void ChienForceInTrggierEventHandler(int index, bool IsStart, int min, int sec);

        private TimeSpan tsCompltedTime = new TimeSpan(0, 0, 0);
        private Cores.Core_Data.EB_State currState = Cores.Core_Data.EB_State.None;
        private Cores.Core_Data.EB_State prevState = Cores.Core_Data.EB_State.None;

        public void SetNames(int index, string indexName)
        {
            iMyIndex = index;
            strName = indexName;

            labelIndexName.Text = strName;
            labelState.Text = "None";

            //lbButton1.ButtonColor = Color.White;
            //lbButtonState.ButtonColor = Color.White;
            //lbButton3.ButtonColor = Color.White;

            lbLedState.LedColor = Color.White;
            //lbLed2.LedColor = Color.White;
            //lbLed3.LedColor = Color.White;
            //lbLed2.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.On;
            //lbLed3.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.On;

            int fontSize = 36;
            labelSetTime.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            labelCurrTime.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);


            fontSize = 20;
            labelState.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            labelIndexName.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
        }

        public void SetTimer(int min, int sec)
        {
            iSetMinTime = min;
            iSetSecTime = sec;
        }

        public void CurrUpdate(Cores.ChikenModule module)
        {
            //labelState.Text = $"{state}";
            //labelSetTime.Text = $"{setTime.Minutes:00}:{setTime.Seconds:00}";
            //labelCurrTime.Text = $"{swTime.Elapsed.Minutes:00}:{swTime.Elapsed.Seconds:00}";

            labelState.Text = $"{module.chickenState}";
            labelSetTime.Text = $"{module.tsSetTime.Minutes:00}:{module.tsSetTime.Seconds:00}";
            //labelCurrTime.Text = $"{module.stopwatch.Elapsed.Minutes:00}:{module.stopwatch.Elapsed.Seconds:00}";

            currState = module.chickenState;

            if (currState != prevState && currState == Cores.Core_Data.EB_State.None)
            {
                tsCompltedTime = module.tsCurTime;
            }

            if (module.chickenState == Cores.Core_Data.EB_State.Cooking || module.chickenState == Cores.Core_Data.EB_State.Cooked)
            {
                labelCurrTime.Text = $"{module.tsCurTime.Minutes:00}:{module.tsCurTime.Seconds:00}";
            }
            else
            {
                //tsCompltedTime = module.tsCurTime;
                labelCurrTime.Text = $"{tsCompltedTime.Minutes:00}:{tsCompltedTime.Seconds:00}";
            }

            prevState = currState;

            switch (module.chickenState)
            {
                case Cores.Core_Data.EB_State.None://사용안함                    
                    lbLedState.LedColor = Color.Black;
                    lbLedState.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.On;
                    break;
                case Cores.Core_Data.EB_State.Waiting:
                    lbLedState.LedColor = Color.White;
                    lbLedState.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.On;
                    break;
                case Cores.Core_Data.EB_State.Cooking:
                    lbLedState.LedColor = Color.Lime;
                    lbLedState.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.On;
                    IsChikenForceOut = true;
                    break;
                case Cores.Core_Data.EB_State.Cooked:
                    lbLedState.LedColor = Color.Lime;
                    lbLedState.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Blink;
                    IsChikenForceOut = true;
                    break;
            }

            int fontSize = 36;
            labelSetTime.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            labelCurrTime.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);


            fontSize = 20;
            labelState.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            labelIndexName.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);


        }

        private void labelSetTime_Click(object sender, EventArgs e)
        {
            Forms.FormSetTimer setTime = new FormSetTimer(iSetMinTime, iSetSecTime, $"{strName} 타이머 설정");
            if (setTime.ShowDialog() == DialogResult.OK)
            {
                if (setTime.IsChikenExist)
                {
                    if (IsChikenForceOut == true)
                    {
                        ChienForceOutTrggierEvent(iMyIndex, setTime.IsChikenExist);
                    }
                    else
                    {
                        Common.FormMessageBox msg;
                        msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None, "치킨 조리 중이 아닙니다.");
                        msg.ShowDialog();
                        return;
                    }
                }
                else if (setTime.IsChikenDelete)
                {
                    ChienForceDelTrggierEvent(iMyIndex, setTime.IsChikenDelete);
                }
                else
                {
                    iSetMinTime = setTime.tsCurrSetTimeMin;
                    iSetSecTime = setTime.tsCurrSetTimeSec;

                    SetTimeChangeEvent(iMyIndex, iSetMinTime, iSetSecTime);
                }
            }
        }
       
    }
}
