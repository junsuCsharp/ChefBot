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
    public partial class UcLoader : UserControl
    {
        public UcLoader()
        {
            InitializeComponent();
        }        

        private string strName = null;
        private int iMyIndex = -1;

        public SetGpioChangeEventHandler SetLoaderChangeEvent;
        public delegate void SetGpioChangeEventHandler(int index, string loadName);

        private int iCurrSensor = 0;
        private int iPrevSensor = 0;

        private int iCurrSwitch = 0;
        private int iPrevSwitch = 0;

        TimeSpan tsUnloadExistTime = new TimeSpan(0, 0, 0);
        DateTime dtUnloadTime = DateTime.Now;

        public bool IsLoader = false;


        public SetSoundEventHandler SetSoundEvent;
        public delegate void SetSoundEventHandler(int soundNumber);

        private bool IsSoundLatch = false;


        public void SetNames(int index, string indexName, string setBuff, bool load)
        {
            iMyIndex = index;
            strName = indexName;

            labelIndexName.Text = strName;
            labelState.Text = "None";
            labelSetIO.Text = setBuff;
            labelEventName.Text = null;
            //lbButton1.ButtonColor = Color.White;
            //lbButton2.ButtonColor = Color.White;
            //lbButton3.ButtonColor = Color.White;

            int fontSize = 20;
            labelIndexName.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            labelState.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            labelSetIO.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            labelEventName.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);

            IsLoader = load;

            if (IsLoader)
            {
                labelSetIO.Text = "IN";
            }
            else
            {
                labelSetIO.Text = "OUT";
            }

        }

        /// <summary>
        /// 요고 디버깅 일단 여기는 2개고, 업데이트는 3개고
        /// </summary>
        /// <param name="module"></param>
        public void CurrUpdate(Cores.LoderModule module)
        {
            //labelState.Text = $"{state}";
            //labelSetTime.Text = $"{setTime.Minutes:00}:{setTime.Seconds:00}";
            //labelCurrTime.Text = $"{swTime.Elapsed.Minutes:00}:{swTime.Elapsed.Seconds:00}";

            //labelState.Text = $"{module.chickenState}";
            //labelEventName.Text = $"{module.tsSetTime.Minutes:00}:{module.tsSetTime.Seconds:00}";
            //labelSetIO.Text = $"{module.strSwitch}";

            labelEventName.Text = $"{module.strSwitch}";

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
                    break;
                case Cores.Core_Data.EB_State.Cooked:
                    lbLedState.LedColor = Color.Lime;
                    lbLedState.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Blink;
                    break;
            }


        }

        public void SensorUpdate(int sensor)
        {         
            iCurrSensor = sensor;

            if (iCurrSensor == 1 && iCurrSensor != iPrevSensor)
            {
                lbLedState.LedColor = Color.Lime;
                lbLedState.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.On;

                labelState.Text = "Wait";
                dtUnloadTime = DateTime.Now;
                if (IsLoader == false)
                {
                    SetSoundEvent(9);
                }
                IsSoundLatch = false;

            }
            else if (iCurrSensor == 0 && iCurrSensor != iPrevSensor)
            {
                lbLedState.LedColor = Color.Black;
                lbLedState.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.On;

                labelState.Text = "None";
                tsUnloadExistTime = new TimeSpan(0, 0, 0);

                //lbLedSw2.LedColor = Color.Black;
                //lbLedSw2.BlinkInterval = 500;
                if (lbLedSw2.State != LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Off)
                {
                    lbLedSw2.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Off;
                }

                IsSoundLatch = false;
            }

            if (IsLoader == false && iCurrSensor == 1)
            {
                tsUnloadExistTime = DateTime.Now - dtUnloadTime;
                labelEventName.Text = $"{tsUnloadExistTime.Minutes:00}:{tsUnloadExistTime.Seconds:00}";


                if (tsUnloadExistTime.TotalSeconds >= 150)
                {
                    if (lbLedSw2.State != LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Blink)
                    {
                        lbLedSw2.LedColor = Color.Red;
                        lbLedSw2.BlinkInterval = 500;
                        lbLedSw2.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Blink;


                        devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(),
                            devJace.Program.ELogLevel.Warn,
                            $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                         $" | Output Error ::: {tsUnloadExistTime}");

                       
                    }

                    //24번 경고음 주기
                    if (IsSoundLatch == false && IsLoader == false)
                    {
                        IsSoundLatch = true;
                        SetSoundEvent(24);
                    }

                    SetSoundEvent(24);

                }
            }

            iPrevSensor = iCurrSensor;


            int fontSize = 20;
            labelIndexName.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            labelState.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            labelSetIO.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            labelEventName.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);

        }

        public void SwitchUpdate(int swButton)
        {

            iCurrSwitch = swButton;

            TimeSpan ts = new TimeSpan();

            if (iCurrSwitch == 1 && iCurrSwitch != iPrevSwitch)
            {
                switch (iMyIndex)
                {
                    case 0:
                        //labelEventName.Text = "SW0";
                        ts = new TimeSpan(0, Cores.Core_Object.GetObj_File.iSwitch1SetTimeMin, Cores.Core_Object.GetObj_File.iSwitch1SetTimeSec);
                        labelEventName.Text = $"SW0 / {ts.Minutes:00}:{ts.Seconds:00}";
                        break;

                    case 1:
                        //labelEventName.Text = "SW1";
                        ts = new TimeSpan(0, Cores.Core_Object.GetObj_File.iSwitch2SetTimeMin, Cores.Core_Object.GetObj_File.iSwitch2SetTimeSec);
                        labelEventName.Text = $"SW1 / {ts.Minutes:00}:{ts.Seconds:00}";
                        break;

                    case 2:
                        labelEventName.Text = "SW2";
                        break;
                }
            }
            else if (iCurrSwitch == 0 && iCurrSwitch != iPrevSwitch)
            {
                labelEventName.Text = null;
            }

            iPrevSwitch = iCurrSwitch;
        }


        private void labelSetIO_Click(object sender, EventArgs e)
        {
            if (iCurrSensor == 1)
            {
                Common.FormMessageBox msgBoxy = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None,
                    "바스켓을 제거 후 변경 하시기 바랍니다.");
                msgBoxy.ShowDialog();
                return;
            }


            string msg = null;
            int loaderSelected = -1;
            switch (labelSetIO.Text)
            {
                case "IN":
                    loaderSelected = 0;
                    msg = "현재 투입 설정 되어있습니다. \r\n 배출로 변경 하시겠습니까?";
                    break;

                case "OUT":
                    loaderSelected = 1;
                    msg = "현재 배출 설정 되어있습니다. \r\n 투입으로 변경 하시겠습니까?";
                    break;
            }
            Common.FormMessageBox msgBox = new Common.FormMessageBox(msg);
            if (msgBox.ShowDialog() == DialogResult.OK)
            {
                switch (loaderSelected)
                {
                    case 0:
                        labelSetIO.Text = "OUT";

                        switch (iMyIndex)
                        {
                            case 0:
                                strName = "UnLoad A";
                                break;

                            case 1:
                                strName = "UnLoad B";
                                break;

                            case 2:
                                strName = "UnLoad C";
                                break;

                        }

                        labelIndexName.Text = strName;
                        break;
                    case 1:
                        labelSetIO.Text = "IN";

                        switch (iMyIndex)
                        {
                            case 0:
                                strName = "Load A";
                                break;

                            case 1:
                                strName = "Load B";
                                break;

                            case 2:
                                strName = "Load C";
                                break;

                        }

                        labelIndexName.Text = strName;
                        break;
                }
                SetLoaderChangeEvent(iMyIndex, labelSetIO.Text);
            }
        }
    }
}
