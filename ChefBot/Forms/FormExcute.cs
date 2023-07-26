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
using System.Reflection;
using Cores;
using Project_Main;
using System.Windows.Interop;
using devi;
using static Cores.Core_StepModule;

namespace Forms
{
    public partial class FormExcute : Form
    {

        public FormExcute()
        {
            InitializeComponent();
        }

        public FormExcute(Project_Main.FormMain mainForm)
        {
            InitializeComponent();

            main = mainForm;

            devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
                $"{System.Reflection.MethodBase.GetCurrentMethod().Name}");
        }

        private Project_Main.FormMain main;

        //2023.04.12 ::: 소리 재생 전달 
        public RobotSoundEventHandler RobotSoundEventEvent;
        public delegate void RobotSoundEventHandler(int index);

        //GUI 로봇 위치 X축 기준 0 ~ 100
        const int iRobotArmX = 206;
        const int iRobotArmY = 3;
        const int iRobotWidth = 1676;
        Point ptRobotArmLocation = new Point(iRobotArmX, iRobotArmY);        
        
        List<Panel> ucCooker = new List<Panel>();
        List<Panel> ucLoader = new List<Panel>();
        List<Panel> ucPanel = new List<Panel>();

        List<PictureBox> picBasketImage = new List<PictureBox>();

        Forms.UcLoader[] frmLoader = new UcLoader[3];
        Forms.UcCooker[] frmCooker = new UcCooker[6];

        List<Label> lstPeriodLabels = new List<Label>();
        List<LBSoft.IndustrialCtrls.Buttons.LBButton> lstUsedButtons = new List<LBSoft.IndustrialCtrls.Buttons.LBButton>();

        bool IsOilChangeWarning = true;
        bool IsOilChangeWarningShow = true;
        DateTime dtWarningTime = DateTime.Now;     
        
        Color startColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(112)))), ((int)(((byte)(255)))));
        Color stopColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));

        //2023.05.07 ::: 일시정지 진행 시간
        DateTime dtPauseTime = DateTime.Now;
        TimeSpan tsPauseTime = new TimeSpan();
        bool IsPauseLatch = false;      

        /// <summary>
        /// 폼 로딩
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormExcute_Load(object sender, EventArgs e)
        {
            this.Size = Parent.Size;

            DefaultGUI();

            //for (int idx = 0; idx < (int)EGripState.Max; idx++)
            //{
            //    pictures[idx] = new PictureBox();
            //    pictures[idx].SizeMode = PictureBoxSizeMode.CenterImage;
            //}
            //
            //pictures[(int)EGripState.None_Open].Image = global::Kyuchon_Robot.Properties.Resources.only_gripper_open;
            //pictures[(int)EGripState.None_Close].Image = global::Kyuchon_Robot.Properties.Resources.only_gripper_close;
            //pictures[(int)EGripState.Stick_Open].Image = global::Kyuchon_Robot.Properties.Resources.gripper_open;
            //pictures[(int)EGripState.Stick_Close].Image = global::Kyuchon_Robot.Properties.Resources.gripper_close; 

            pictureBoxCobotArm.Image = global::ChefBot.Properties.Resources.FormExcute_empty_L_181_83;
            //pictureBoxCobotArm.Image = global::ChefBot.Properties.Resources.FormExcute_empty_L_181_83;
            //pictureBoxCobotArm.Image = global::ChefBot.Properties.Resources.FormExcute_DarkYellow_L_181_83;
            //pictureBoxCobotArm.Image = global::ChefBot.Properties.Resources.FormExcute_DarkYellow_R_181_83;
            //pictureBoxCobotArm.Image = global::ChefBot.Properties.Resources.FormExcute_Yellow_L_181_83;
            //pictureBoxCobotArm.Image = global::ChefBot.Properties.Resources.FormExcute_DarkYellow_R_181_83;//투입 용도

            //progressBar1.Value = 100;
            //progressBar1.Style = ProgressBarStyle.Marquee;

            ucLoader.Add(panelUcModule1);
            ucLoader.Add(panelUcModule2);
            ucLoader.Add(panelUcModule3);

            ucCooker.Add(panelUcModule4);
            ucCooker.Add(panelUcModule5);
            ucCooker.Add(panelUcModule6);
            ucCooker.Add(panelUcModule7);
            ucCooker.Add(panelUcModule8);
            ucCooker.Add(panelUcModule9);

            ucPanel.Add(panelUcModule1);
            ucPanel.Add(panelUcModule2);
            ucPanel.Add(panelUcModule3);
            ucPanel.Add(panelUcModule4);
            ucPanel.Add(panelUcModule5);
            ucPanel.Add(panelUcModule6);
            ucPanel.Add(panelUcModule7);
            ucPanel.Add(panelUcModule8);
            ucPanel.Add(panelUcModule9); 

            frmLoader[0] = new UcLoader();
            frmLoader[1] = new UcLoader();
            frmLoader[2] = new UcLoader();

            frmCooker[0] = new UcCooker();
            frmCooker[1] = new UcCooker();
            frmCooker[2] = new UcCooker();
            frmCooker[3] = new UcCooker();
            frmCooker[4] = new UcCooker();
            frmCooker[5] = new UcCooker();

            picBasketImage.Add(pictureBox1);
            picBasketImage.Add(pictureBox2);
            picBasketImage.Add(pictureBox3);
            picBasketImage.Add(pictureBox4);
            picBasketImage.Add(pictureBox5);
            picBasketImage.Add(pictureBox6);
            picBasketImage.Add(pictureBox7);
            picBasketImage.Add(pictureBox8);
            picBasketImage.Add(pictureBox9);

            for (int i = 0; i < frmCooker.Length; i++)
            {
                frmCooker[i].SetTimeChangeEvent += new UcCooker.SetTimeChangeEventHandler(SetTimeChangeSender);
                frmCooker[i].ChienForceOutTrggierEvent += new UcCooker.ChienForceOutTrggierEventHandler(Func_ForceOutChiken);
                frmCooker[i].ChienForceDelTrggierEvent += new UcCooker.ChienForceDelTrggierEventHandler(Func_ForceDelChiken);
                frmCooker[i].ChienForceInTrggierEvent += new UcCooker.ChienForceInTrggierEventHandler(Func_ForceInChiken);
            }

            for (int i = 0; i < frmLoader.Length; i++)
            {
                frmLoader[i].SetLoaderChangeEvent += new UcLoader.SetGpioChangeEventHandler(SetLoaderChangeSender);
                frmLoader[i].SetSoundEvent += new UcLoader.SetSoundEventHandler(fromLoaderEvent);
            }

            lstPeriodLabels.Add(labelPeriod1);
            lstPeriodLabels.Add(labelPeriod2);
            lstPeriodLabels.Add(labelPeriod3);

            lstUsedButtons.Add(lbButtonPeriod1);
            lstUsedButtons.Add(lbButtonPeriod2);
            lstUsedButtons.Add(lbButtonPeriod3);


            //foreach (Panel con in ucLoader)
            //{
            //    con.Controls.Clear();
            //}
            //foreach (Panel con in ucCooker)
            //{
            //    con.Controls.Clear();
            //}
            //for (int idx = 0; idx < frmLoader.Length; idx++)
            //{
            //    ucLoader[idx].Controls.Add(frmLoader[idx]);
            //    ucLoader[idx].Show();
            //
            //    frmLoader[idx].SetNames(idx, Cores.Core_Object.GetObj_File.lstLoaderName[idx], Cores.Core_Object.GetObj_File.lstLoaderSetIO[idx]);
            //}
            //for (int idx = 0; idx < frmCooker.Length; idx++)
            //{
            //    ucCooker[idx].Controls.Add(frmCooker[idx]);
            //    ucCooker[idx].Show();
            //
            //    frmCooker[idx].SetNames(idx, Cores.Core_Object.GetObj_File.lstCookerName[idx]);
            //    frmCooker[idx].SetTimer(Cores.Core_Object.GetObj_File.tsChikenSetMinTime[idx], Cores.Core_Object.GetObj_File.tsChikenSetSecTime[idx]);
            //}

            int loaderCount = 0;
            int unloaderCount = 0;
            int cookerCount = 0;

            for (int idx = 0; idx < ucPanel.Count; idx++)
            {
                ucPanel[idx].Controls.Clear();

                switch (FormMain.gui_file.type[idx])
                {
                    case devJace.Files.ModuleType.Load:
                        ucPanel[idx].Controls.Add(frmLoader[loaderCount]);
                        ucPanel[idx].Show();
                        //frmLoader[loaderCount].SetNames(idx, Cores.Core_Object.GetObj_File.lstLoaderName[idx], Cores.Core_Object.GetObj_File.lstLoaderSetIO[idx]);
                        frmLoader[loaderCount].SetNames(idx, FormMain.gui_file.strName[idx], $"SW{loaderCount}", true);
                        loaderCount++;
                        unloaderCount++;
                        break;

                    case devJace.Files.ModuleType.UnLoad:
                        ucPanel[idx].Controls.Add(frmLoader[unloaderCount]);
                        ucPanel[idx].Show();
                        //frmLoader[loaderCount].SetNames(idx, Cores.Core_Object.GetObj_File.lstLoaderName[idx], Cores.Core_Object.GetObj_File.lstLoaderSetIO[idx]);
                        frmLoader[unloaderCount].SetNames(idx, FormMain.gui_file.strName[idx], $"SW{loaderCount}", false);
                        loaderCount++;
                        unloaderCount++;
                        break;

                    case devJace.Files.ModuleType.Cooker:

                        int nCookIndex = FormMain.gui_file.iCookIndex[cookerCount];

                        ucPanel[idx].Controls.Add(frmCooker[nCookIndex]);
                        ucPanel[idx].Show();

                        //frmCooker[cookerCount].SetNames(cookerCount, FormMain.gui_file.strName[idx]);
                        //frmCooker[cookerCount].SetNames(FormMain.gui_file.iCookIndex[idx], FormMain.gui_file.strName[idx]);
                        //frmCooker[cookerCount].SetTimer(FormMain.gui_file.iSetMin[idx], FormMain.gui_file.iSetSec[idx]);

                        frmCooker[nCookIndex].SetNames(nCookIndex, FormMain.gui_file.strName[idx]);
                        frmCooker[nCookIndex].SetTimer(FormMain.gui_file.iSetMin[idx], FormMain.gui_file.iSetSec[idx]);
                        cookerCount++;
                        break;
                }
                
            }


            for (int idx = 0; idx < lstPeriodLabels.Count; idx++)
            {
                lstPeriodLabels[idx].Text = $"{main.core_Object.stepModuleCore.lstOilCheckdCount[idx]}/{ Cores.Core_Object.GetObj_File.lstOilCheckdCount[idx]}";
            }

            //GetObject file

           
        }

        /// <summary>
        /// 2023.03.10 ::: 치킨 강제 배출 명령
        /// </summary>
        void Func_ForceOutChiken(int index, bool IsExist)
        {
            main.core_Object.stepModuleCore.mChiken[index].IsExist = IsExist;
            main.core_Object.stepModuleCore.mChiken[index].chickenState = Cores.Core_Data.EB_State.Cooked;
        }

        /// <summary>
        /// 2023.03.13 ::: 치킨 강제 삭제 명령
        /// </summary>
        void Func_ForceDelChiken(int index, bool IsExist)
        {
            //main.core_Object.stepModuleCore.mChiken[index].IsExist = IsExist;

            //main.core_Object.stepModuleCore.mChiken[index] = new Cores.ChikenModule();
            //main.core_Object.stepModuleCore.mChiken[index].stopwatch.Stop();
            //main.core_Object.stepModuleCore.mChiken[index].stopwatch.Reset();
            //main.core_Object.stepModuleCore.mChiken[index].stopwatchOutput.Stop();
            //main.core_Object.stepModuleCore.mChiken[index].stopwatchOutput.Reset();
            main.core_Object.stepModuleCore.mChiken[index].chickenState = Cores.Core_Data.EB_State.None;
        }

        void Func_ForceInChiken(int index, bool IsExist, int min, int sec)
        {
            //main.core_Object.stepModuleCore.mChiken[index].IsExist = IsExist;

            //main.core_Object.stepModuleCore.mChiken[index] = new Cores.ChikenModule();
            //main.core_Object.stepModuleCore.mChiken[index].stopwatch.Stop();
            //main.core_Object.stepModuleCore.mChiken[index].stopwatch.Reset();
            //main.core_Object.stepModuleCore.mChiken[index].stopwatchOutput.Stop();
            //main.core_Object.stepModuleCore.mChiken[index].stopwatchOutput.Reset();
            main.core_Object.stepModuleCore.mChiken[index].SetTimeChanege(new TimeSpan(0, min, sec));
            main.core_Object.stepModuleCore.mChiken[index].chickenState = Cores.Core_Data.EB_State.Cooking;
        }

        /// <summary>
        /// 2023.04.26 ::: 로더 폼에서 오는 소리 이벤트 발생
        /// </summary>
        /// <param name="soundNubmer"></param>
        void fromLoaderEvent(int soundNubmer)
        {
            RobotSoundEventEvent(soundNubmer);
        }

        /// <summary>
        /// 파스텍 모터 위치 0 ~ 100까지
        /// 바스켓 유무는 래치로
        /// 로봇 이미지 업데이트 하는 부분
        /// nMotionDir 0 : right, 1 : left
        /// </summary>
        /// <param name="nMotionDir"></param>
        /// <param name="IsGripState"></param>
        /// <param name="xPosition"></param>
        public void RobotLocation()
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            var cobotToolInput = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 021);
            var motionPostion = Fas_Data.iActPos;
            var motionDirection = Fas_Data.lstAxis_State[(int)EAxis_Status.Motion_DIR] ? 1 : 0;

            //int iImageSelected = ((IsGripState << 1) & 1 ) | (IsGripState & 1);
            switch (cobotToolInput.iData)
            {
                case 0:
                    if (motionDirection == 0)
                    {
                        pictureBoxCobotArm.Image = global::ChefBot.Properties.Resources.FormExcute_empty_R_181_83;
                    }
                    else
                    {
                        pictureBoxCobotArm.Image = global::ChefBot.Properties.Resources.FormExcute_empty_L_181_83;
                    }
                    break;
                case 1:
                    if (motionDirection == 0)
                    {
                        pictureBoxCobotArm.Image = global::ChefBot.Properties.Resources.FormExcute_empty_R_181_83;
                    }
                    else
                    {
                        pictureBoxCobotArm.Image = global::ChefBot.Properties.Resources.FormExcute_empty_L_181_83;
                    }
                    
                    break;
                case 2:
                    if (motionDirection == 0)
                    {
                        pictureBoxCobotArm.Image = global::ChefBot.Properties.Resources.FormExcute_Yellow_R_181_83;
                    }
                    else
                    {
                        pictureBoxCobotArm.Image = global::ChefBot.Properties.Resources.FormExcute_Yellow_L_181_83;
                    }
                    break;
                case 3:
                    //if (nMotionDir == 0)
                    //{
                    //    pictureBoxCobotArm.Image = global::ChefBot.Properties.Resources.FormExcute_empty_R_181_83;
                    //}
                    //else
                    //{
                    //    pictureBoxCobotArm.Image = global::ChefBot.Properties.Resources.FormExcute_empty_L_181_83;
                    //}
                    break;
            }
            //pictureBoxCobotArm.Image = pictures[iImageSelected].Image;          
            //Console.WriteLine($"{DateTime.Now} >>> {methodName} /// {(EGripState)iImageSelected} 0x{iImageSelected:X2} ");

            //pictureBoxCobotArm.Image = global::ChefBot.Properties.Resources.FormExcute_empty_L_181_83;
            //pictureBoxCobotArm.Image = global::ChefBot.Properties.Resources.FormExcute_empty_L_181_83;
            //pictureBoxCobotArm.Image = global::ChefBot.Properties.Resources.FormExcute_DarkYellow_L_181_83;
            //pictureBoxCobotArm.Image = global::ChefBot.Properties.Resources.FormExcute_DarkYellow_R_181_83;
            //pictureBoxCobotArm.Image = global::ChefBot.Properties.Resources.FormExcute_Yellow_L_181_83;
            //pictureBoxCobotArm.Image = global::ChefBot.Properties.Resources.FormExcute_DarkYellow_R_181_83;//투입 용도
            //const int iRobotArmX = 206;
            //const int iRobotArmY = 3;
            //const int iRobotWidth = 1676;

            //2640000 lstPositions
            //(Cores.Core_Object.GetPos_File.lstRealPositions

            try
            {
                double x2 = motionPostion * 100 / Cores.Core_Object.GetPos_File.lstRealPositions.Max();
                int x1 = (int)((iRobotWidth - iRobotArmX) * x2 * 0.01);

                ptRobotArmLocation = new Point(x1 + iRobotArmX, iRobotArmY);

                pictureBoxCobotArm.Location = ptRobotArmLocation;
            }
            catch
            { }

            

              //  devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(),
              //devJace.Program.ELogLevel.Info,
              //$"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
              //$" | {x1} {x2} {pictureBoxCobotArm.Location}");

            //DefaultGUI();
        }

        /// <summary>
        /// 바스켓 이미지 변환
        /// </summary>
        public void BasketImageUpdate()
        {
            if (main.core_Object.stepModuleCore.mLoders[0] == null
                || main.core_Object.stepModuleCore.mLoders[1] == null
                || main.core_Object.stepModuleCore.mLoders[2] == null
                || main.core_Object.stepModuleCore.mChiken == null)
            {
                picBasketImage[0].Image = null;
                picBasketImage[1].Image = null;
                picBasketImage[2].Image = null;
                for (int idx = 0; idx < main.core_Object.stepModuleCore.mChiken.Count; idx++)
                {
                    picBasketImage[idx + 3].Image = null;
                }
                return;
            }


            //sensor
            //if (main.core_Object.stepModuleCore.mLoders[0].IsCurrSensor)
            //{
            //    picBasketImage[0].Image = global::ChefBot.Properties.Resources.FormExcute_left_Y_Chicken;
            //}
            //else
            //{
            //    picBasketImage[0].Image = null;
            //}
            //
            //if (main.core_Object.stepModuleCore.mLoders[1].IsCurrSensor)
            //{
            //    picBasketImage[1].Image = global::ChefBot.Properties.Resources.FormExcute_left_Y_Chicken;
            //}
            //else
            //{
            //    picBasketImage[1].Image = null;
            //}
            //
            //if (main.core_Object.stepModuleCore.mPlaceUnload.IsCurrSensor)
            //{
            //    picBasketImage[2].Image = global::ChefBot.Properties.Resources.FormExcute_right_Y_Chicken;
            //}
            //else
            //{
            //    picBasketImage[2].Image = null;
            //}

            for (int idx = 0; idx < main.core_Object.stepModuleCore.mLoders.Length; idx++)
            {
                if (main.core_Object.stepModuleCore.mLoders[idx].IsCurrSensor)
                {
                    picBasketImage[idx].Image = global::ChefBot.Properties.Resources.FormExcute_left_Y_Chicken;
                }
                else
                {
                    picBasketImage[idx].Image = null;
                }
            }

            for (int idx = 0; idx < main.core_Object.stepModuleCore.mChiken.Count; idx++)
            {
                if (main.core_Object.stepModuleCore.mChiken[idx].chickenState == Cores.Core_Data.EB_State.None
                    || main.core_Object.stepModuleCore.mChiken[idx].chickenState == Cores.Core_Data.EB_State.Waiting)
                {
                    picBasketImage[idx + 3].Image = null;
                }
                else if (main.core_Object.stepModuleCore.mChiken[idx].chickenState == Cores.Core_Data.EB_State.Cooked)
                {
                    picBasketImage[idx + 3].Image = global::ChefBot.Properties.Resources.FormExcute_left_Y_Chicken;
                }
                else
                {
                    picBasketImage[idx + 3].Image = global::ChefBot.Properties.Resources.FormExcute_left_Y_Chicken;
                }

                
            }

            //picBasketImage[index]
        }             

        void DefaultGUI()
        {
            int fontSize = 11;
            this.labelPauseDesc.Font = new Font(Fonts.FontLibrary.Families[0], 11);

            //this.lbButtonCobot.Font = new Font(Fonts.FontLibrary.Families[0], 60);
            //this.lbButtonCobot.Font = new Font("Arial", 60);

            //this.lbButtonPeriod1.Font = new Font(Fonts.FontLibrary.Families[0], 80);
            //this.lbButtonPeriod2.Font = new Font(Fonts.FontLibrary.Families[0], 80);
            //this.lbButtonPeriod3.Font = new Font(Fonts.FontLibrary.Families[0], 80);

            fontSize = 100;
            this.lbButtonPeriod1.Font = new Font("Arial", fontSize);
            this.lbButtonPeriod2.Font = new Font("Arial", fontSize);
            this.lbButtonPeriod3.Font = new Font("Arial", fontSize);

            fontSize = 35;
            this.lbButtonPause.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            this.lbButtonStart.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            this.lbButtonReady.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);

            this.lbButtonPause.Text = "일시 정지";
            this.lbButtonReady.Text = "조리 준비";
            this.lbButtonStart.Text = "조리 시작";

            lbButtonPause.BackColor = Color.PaleGreen;
            lbButtonReady.BackColor = Color.WhiteSmoke;
            //this.lbButtonPause.Font = new Font("Arial", 80);
            //this.lbButtonStart.Font = new Font("Arial", 80);

            fontSize = 25;
            //this.labelSetTimerHeader.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            this.labelHoldTimeHeader.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);

            fontSize = 35;
            //this.labelSetTimer.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            this.labelHoldTime.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);


            fontSize = 18 ;
            //this.labelTempHeader1.Font = new Font(Fonts.FontLibrary.Families[0], 25);
            //this.labelTempHeader2.Font = new Font(Fonts.FontLibrary.Families[0], 25);
            //this.labelTempHeader3.Font = new Font(Fonts.FontLibrary.Families[0], 25);
            //this.labelPeriodHeader1.Font = new Font(Fonts.FontLibrary.Families[0], 25);
            //this.labelPeriodHeader2.Font = new Font(Fonts.FontLibrary.Families[0], 25);
            //this.labelPeriodHeader3.Font = new Font(Fonts.FontLibrary.Families[0], 25);
            this.labelTempHeader1.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            this.labelTempHeader2.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            this.labelTempHeader3.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            this.labelPeriodHeader1.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            this.labelPeriodHeader2.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            this.labelPeriodHeader3.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);

            fontSize = 20;
            this.labelTemp1.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            this.labelTemp2.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            this.labelTemp3.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);

            fontSize = 30;
            this.labelPeriod1.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            this.labelPeriod2.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            this.labelPeriod3.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);

            //this.labelPauseDesc.Text = "※ The robot pauses for a set amount of time.";
            this.labelPauseDesc.Text = "※ 로봇이 일정 시간 동안 중지합니다.";

            //lbButtonCobot.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);

        }

        /// <summary>
        /// 2023.02.25
        /// 상태 표시
        /// </summary>
        /// <param name="eCurrState"></param>
        /// <param name="eMode"></param>
        /// <param name="setMin"></param>
        /// <param name="setSec"></param>
        /// <param name="swTime"></param>
        public void Update_State()
        {
            DefaultGUI();


            //lbButtonPause.Enabled = false;

            if (Fas_Data.lstAxis_State[(int)EAxis_Status.Inposition] == false )
            { 
            
            }

            if (Fas_Data.lstAxis_State[(int)EAxis_Status.Motion_Pause])
            {
                lbButtonPause.BackColor = Color.Lime;
                lbButtonPause.Text = "정지 해제";

                lbButtonPause.Enabled = true;
            }
            else
            {
                lbButtonPause.BackColor = Color.PaleGreen;
                lbButtonPause.Text = "일시 정지";

            }

            switch (main.core_Object.processCore.eCurrState)
            {
                case PackML.ECurrState.CurrentState_Aborted:
                    break;

                case PackML.ECurrState.CurrentState_Stopped:
                    lbButtonStart.BackColor = startColor;
                    lbButtonStart.Text = "조리 시작";

                    lbButtonReady.Enabled = true;
                    lbButtonStart.Enabled = false;
                    break;

                case PackML.ECurrState.CurrentState_Idle:
                    lbButtonStart.BackColor = startColor;
                    lbButtonStart.Text = "조리 시작";

                    //lbButtonPause.BackColor = Color.PaleGreen;
                    //lbButtonPause.Text = "일시 정지";

                    lbButtonReady.Enabled = false;
                    lbButtonStart.Enabled = true;
                    break;

                case PackML.ECurrState.CurrentState_Starting:
                    lbButtonStart.BackColor = stopColor;
                    lbButtonStart.Text = "조리 정지";
                    break;

                case PackML.ECurrState.CurrentState_Hold:
                    //lbButtonStart.BackColor = Color.DodgerBlue;
                    //lbButtonStart.Label = "조리 시작";

                    //lbButtonPause.ButtonColor = Color.DodgerBlue;

                    if (Core_Object.GetObj_File.iLaserScannerUse == 0)
                    {
                        lbButtonPause.BackColor = Color.Lime;
                        lbButtonPause.Text = "정지 해제";

                        lbButtonPause.Enabled = true;
                        //if (main.core_Object.stepModuleCore.swPauseTimer.IsRunning != true)
                        //{
                        //    main.core_Object.stepModuleCore.swPauseTimer.Reset();
                        //    main.core_Object.stepModuleCore.swPauseTimer.Start();
                        //}
                        //else if (main.core_Object.stepModuleCore.swPauseTimer.IsRunning == true)
                        //{
                        //    TimeSpan setTime = new TimeSpan(0, setMin, setSec);
                        //    if (swTime.ElapsedTicks >= setTime.Ticks)
                        //    {
                        //        main.core_Object.processCore.Set_State_Matrix(PackML.Command.CurrentState_Unholding);
                        //        main.core_Object.stepModuleCore.swPauseTimer.Stop();
                        //        main.core_Object.stepModuleCore.swPauseTimer.Reset();
                        //    }
                        //}
                    }
                    break;

                case PackML.ECurrState.CurrentState_Excute:
                    lbButtonStart.BackColor = stopColor;
                    lbButtonStart.Text = "조리 정지";

                    //lbButtonPause.BackColor = Color.PaleGreen;
                    //lbButtonPause.Text = "일시 정지";

                    lbButtonPause.Enabled = true;
                    //if (main.core_Object.stepModuleCore.swPauseTimer.IsRunning == true)
                    //{
                    //    main.core_Object.stepModuleCore.swPauseTimer.Stop();
                    //    main.core_Object.stepModuleCore.swPauseTimer.Reset();
                    //}
                    break;

                case PackML.ECurrState.CurrentState_Clearing:
                    lbButtonStart.BackColor = startColor;
                    lbButtonStart.Text = "조리 시작";
                    break;

            }

            //labelSetTimer.Text = $"{setMin:00} : {setSec:00}";
            //labelHoldTime.Text = $"{swTime.Elapsed.Minutes:00} : {swTime.Elapsed.Seconds:00}";


            //2023.02.22
            for (int idx = 0; idx < lstPeriodLabels.Count; idx++)
            {
                lstPeriodLabels[idx].Text = $"{main.core_Object.stepModuleCore.lstOilCheckdCount[idx]:00}/{Cores.Core_Object.GetObj_File.lstOilCheckdCount[idx]:00}";

                //if(main.core_Object.stepModuleCore.lstOilCheckdCount[idx]== Cores.Core_Object.GetObj_File.lstOilCheckdCount[idx])
                //{
                //    MessageBox.Show(idx + 1 + "번 조리기의 기름을 교체해 주세요.");
                //}
            }

            //2023.02.22
            if (main.core_Object.stepModuleCore.mChiken.Count != 0)
            {
                for (int idx = 0; idx < frmCooker.Length; idx++)
                {
                    frmCooker[idx].CurrUpdate(main.core_Object.stepModuleCore.mChiken[idx]);
                }
            }    

            //2023.02.23
            string strOilNumbers = "| ";
            IsOilChangeWarning = true;
            for (int idx = 0; idx < lstUsedButtons.Count; idx++)
            {
                if (main.core_Object.stepModuleCore.lstOilCheckdCount[idx] >= Cores.Core_Object.GetObj_File.lstOilCheckdCount[idx])
                {
                    IsOilChangeWarning &= false;
                    strOilNumbers += $"{Cores.Core_Object.GetObj_File.lstFryerName[idx]} |";
                }
            }
            if (IsOilChangeWarning == false && IsOilChangeWarningShow == false)
            {
                IsOilChangeWarningShow = true;
                Common.FormMessageBox msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None, $"{strOilNumbers} 오일 교체가 필요 합니다.", 5);
                RobotSoundEventEvent(17);
                msg.Show();
                dtWarningTime = DateTime.Now.Add(new TimeSpan(0, 1, 0));

                //MessageBox.Show("오일 교체가 필요 합니다.");


            }
            else if (IsOilChangeWarning == true && IsOilChangeWarningShow == true)
            {
                IsOilChangeWarningShow = false;
            }
            else if (DateTime.Now >= dtWarningTime)
            {
                IsOilChangeWarningShow = false;
            }

            //2023.03.06
            for (int i = 0; i < frmLoader.Length; i++)
            {
                frmLoader[i].SensorUpdate(Cores.Fas_Data.lstIO_InState[Core_StepModule.CHEFY][i+Define.iSensorLocateOffsset] ? 1 : 0);

                if (main.core_Object.stepModuleCore.mLoders[i] != null && frmLoader[i].IsLoader == true)
                {

                    frmLoader[i].SwitchUpdate(main.core_Object.stepModuleCore.mLoders[i].IsStartLatch ? 1 : 0);
                }
            }


            //2023.02.23
            for (int idx = 0; idx < lstUsedButtons.Count; idx++)
            {
                if (Cores.Core_Object.GetObj_File.lstOilMeckUse[idx])
                {
                    lstUsedButtons[idx].Label = "USE";
                    lstUsedButtons[idx].ForeColor = Color.White;

                    lstUsedButtons[idx].ButtonColor = Color.WhiteSmoke;
                }
                else
                {
                    lstUsedButtons[idx].Label = "UnUSE";
                    lstUsedButtons[idx].ForeColor = Color.White;

                    lstUsedButtons[idx].ButtonColor = Color.Bisque;
                }
            }

            //2023.05.07
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

                if (IsPauseLatch == false)
                {
                    IsPauseLatch = true;
                    dtPauseTime = DateTime.Now;
                }

                tsPauseTime = DateTime.Now - dtPauseTime;
                labelHoldTime.Text = $"{tsPauseTime}";
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
                labelHoldTime.Text = "00:00:00";
                IsPauseLatch = false;
            }

        }

        private void lbButtonPause_Load(object sender, EventArgs e)
        {
        }

        private void lbButtonStart_Load(object sender, EventArgs e)
        {
        }

        private void lbButtonPause_Click(object sender, EventArgs e)
        {
            //2023.04.12 ::: 레이져 스캐너 사용 안할 경우 추가
            //if (Cores.Core_Object.GetObj_File.iLaserScannerUse == 1)
            //    return;
            //
            //PackML.Command cmd = PackML.Command.CurrentState_None;

            //switch (main.core_Object.processCore.eCurrState)
            //{
            //    case PackML.ECurrState.CurrentState_Excute:
            //        cmd = PackML.Command.CurrentState_Holding;
            //        while (main.core_Object.stepModuleCore.IsExcuteFlag() == false)
            //        {
            //            if (main.core_Object.processCore.eCurrState == PackML.ECurrState.CurrentState_Stopped)
            //            {
            //                break;
            //            }
            //        }
            //        break;
            //
            //    case PackML.ECurrState.CurrentState_Hold:
            //        cmd = PackML.Command.CurrentState_Unholding;
            //        break;
            //}
            //
            //if (cmd != PackML.Command.CurrentState_None)
            //{
            //    main.core_Object.processCore.Set_State_Matrix(cmd);
            //}

            //if (Fas_Data.lstAxis_State[(int)EAxis_Status.Motion_Pause])
            //        return;


            if (Fas_Data.lstIO_InState[Core_StepModule.CHEFY][(int)Core_StepModule.CHEFY_INPUT.L_PAUSE_SW] ||
                Fas_Data.lstIO_InState[Core_StepModule.CHEFY][(int)Core_StepModule.CHEFY_INPUT.R_PAUSE_SW])
            {
                return;
            }

                switch (Define.eCUSTOM)
            {
                case ECUSTOM.KongMan:
                    //2023.05.10 ::: 아이오 출력
                    bool[] OutStateBuff = Fas_Data.lstIO_OutState[Core_StepModule.CHEFY];
                    if (lbButtonPause.Text == "일시 정지")
                    {   
                        OutStateBuff[(int)Core_StepModule.CHEFY_OUTPUT.Pause] = true;
                        OutStateBuff[(int)Core_StepModule.CHEFY_OUTPUT.Pause_Spare] = true;
                        Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_2, OutStateBuff);

                        //lbButtonPause.Text = "정지 해제";
                    }
                    else
                    {
                        OutStateBuff[(int)Core_StepModule.CHEFY_OUTPUT.Pause] = false;
                        OutStateBuff[(int)Core_StepModule.CHEFY_OUTPUT.Pause_Spare] = false;
                        Cores.Fas_Motion.SetOutput((int)IO_Board.EzEtherNetIO_2, OutStateBuff);

                        //lbButtonPause.Text = "일시 정지";
                    }
                    break;

                case ECUSTOM.Demo:
                    break;
            }
            
        }
        /// <summary>
        /// 조리시작 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void lbButtonStart_Click(object sender, EventArgs e)
        {

            var robotState = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 259);
            Externs.Robot_Modbus_Table.RobotState_Ver_1_1 cobotState = (Externs.Robot_Modbus_Table.RobotState_Ver_1_1)robotState.iData;

            Common.FormMessageBox msg;

            var closeOpen = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 21);
            if (closeOpen.iData == 2)
            {
                msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None, "그리퍼를 수동으로 열림 후 조리 시작 바랍니다.");
                msg.ShowDialog();
                return;
            }



            PackML.Command cmd = PackML.Command.CurrentState_None;

            switch (main.core_Object.processCore.eCurrState)
            {
                case PackML.ECurrState.CurrentState_Idle:
                    cmd = PackML.Command.CurrentState_Starting;
                    break;
                case PackML.ECurrState.CurrentState_Excute:
                    cmd = PackML.Command.CurrentState_Stopping;
                    break;
                case PackML.ECurrState.CurrentState_Hold:
                    cmd = PackML.Command.CurrentState_Stopping;
                    break;
                case PackML.ECurrState.CurrentState_Stopped:
                    cmd = PackML.Command.CurrentState_Resetting;
                    break;
                case PackML.ECurrState.CurrentState_Starting:
                    cmd = PackML.Command.CurrentState_Stopping;
                    break;
            }
            if (cmd != PackML.Command.CurrentState_None)
            {
                main.core_Object.processCore.Set_State_Matrix(cmd);
            }

            //if (main.core_Object.stepModuleCore.swPauseTimer.IsRunning == true)
            //{
            //    main.core_Object.stepModuleCore.swPauseTimer.Stop();
            //    main.core_Object.stepModuleCore.swPauseTimer.Reset();
            //}
        }
     

        /// <summary>
        /// 2023.02.22
        /// 일시 정지 타이머 설정
        /// 하드웨어 및 세이프틱스 권고 사항으로 삭제
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void labelSetTimer_Click(object sender, EventArgs e)
        {
            //Forms.FormSetTimer setTime = new FormSetTimer(
            //  Cores.Core_Object.GetObj_File.tsPauseSetMinTime,
            //  Cores.Core_Object.GetObj_File.tsPauseSetSecTime, 
            //  "일시 정지 타이머 설정", false);
            //if (setTime.ShowDialog() == DialogResult.OK)
            //{
            //    Cores.Core_Object.GetObj_File.tsPauseSetMinTime = setTime.tsCurrSetTimeMin;
            //    Cores.Core_Object.GetObj_File.tsPauseSetSecTime = setTime.tsCurrSetTimeSec;
            //    
            //    main.core_Object.ObjectFIle_Save();
            //}
            
        }

        /// <summary>
        /// 2023.02.22
        /// 조리기구 횟수 초기화 / 조리기구 교체 경고 회수 설정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void labelPeriod1_Click(object sender, EventArgs e)
        {

            Label lb = sender as Label;

            for (int idx = 0; idx < lstPeriodLabels.Count; idx++)
            {
                if (lstPeriodLabels[idx] == lb)
                {
                    Forms.FormSetCounter setCount = new FormSetCounter(
                       main.core_Object.stepModuleCore.lstOilCheckdCount[idx], Cores.Core_Object.GetObj_File.lstOilCheckdCount[idx],
                       Cores.Core_Object.GetObj_File.lstFryerName[idx]);

                    if (setCount.ShowDialog() == DialogResult.OK)
                    {
                        main.core_Object.stepModuleCore.lstOilCheckdCount[idx] = setCount.tsCurrentCount;
                        Cores.Core_Object.GetObj_File.lstOilCheckdCount[idx] = setCount.tsCurrSetCount;

                        main.core_Object.ObjectFIle_Save();
                    }
                    break;
                }
            }

            
        }

        /// <summary>
        /// 2023.02.23
        /// 조리기구 사용유무 설정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbButtonPeriod1_Click(object sender, EventArgs e)
        {
            LBSoft.IndustrialCtrls.Buttons.LBButton lB = sender as LBSoft.IndustrialCtrls.Buttons.LBButton;
            for (int idx = 0; idx < lstUsedButtons.Count; idx++)
            {
                if (lB == lstUsedButtons[idx])
                {
                    if (devi.Define.eCUSTOM == devi.ECUSTOM.KongMan && idx == 2)
                        return;


                    if (main.core_Object.stepModuleCore.mChiken[idx].chickenState == Cores.Core_Data.EB_State.None)
                    {
                        if (Cores.Core_Object.GetObj_File.lstOilMeckUse[idx])
                        {
                            Cores.Core_Object.GetObj_File.lstOilMeckUse[idx] = false;

                            //오일 교체 진행 여부                        
                            Common.FormMessageBox msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None, "오일 교체를 진행 합니다.", 5);
                            int offsetUnUseSound = 21;
                            RobotSoundEventEvent(offsetUnUseSound + idx);
                            msg.Show();
                        }
                        else
                        {
                            Cores.Core_Object.GetObj_File.lstOilMeckUse[idx] = true;

                            //오일 교체 회수 초기화 여부
                            if (main.core_Object.stepModuleCore.lstOilCheckdCount[idx] != 0)
                            {
                                Common.FormMessageBox msg = new Common.FormMessageBox("오일 교체 회수를 초기화 하시겠습니까?");
                                if (msg.ShowDialog() == DialogResult.OK)
                                {
                                    main.core_Object.stepModuleCore.lstOilCheckdCount[idx] = 0;
                                }
                            }
                            else
                            {
                                Common.FormMessageBox msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None, "조리기구를 사용 합니다.", 5);

                                int offsetUseSound = 18;
                                RobotSoundEventEvent(offsetUseSound + idx);
                                msg.Show();
                            }
                        }
                        main.core_Object.ObjectFIle_Save();
                        break;
                    }
                    else
                    { 
                    
                    }
                   
                }
            }

      
            //mChiken[0].IsCookerUsed = Cores.Core_Object.GetObj_File.lstOilMeckUse[0];
            //mChiken[1].IsCookerUsed = Cores.Core_Object.GetObj_File.lstOilMeckUse[0];
            //mChiken[2].IsCookerUsed = Cores.Core_Object.GetObj_File.lstOilMeckUse[1];
            //mChiken[3].IsCookerUsed = Cores.Core_Object.GetObj_File.lstOilMeckUse[1];
            //mChiken[4].IsCookerUsed = Cores.Core_Object.GetObj_File.lstOilMeckUse[2];
            //mChiken[5].IsCookerUsed = Cores.Core_Object.GetObj_File.lstOilMeckUse[2];
        }

        /// <summary>
        /// TEST
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //input
            main.core_Object.stepModuleCore.lstOilCheckdCount[0]++;
            main.core_Object.stepModuleCore.lstOilCheckdCount[1]++;
            main.core_Object.stepModuleCore.lstOilCheckdCount[2]++;
        }

        /// <summary>
        /// TEST
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            //output
        }

        /// <summary>
        /// UcCooker
        /// 쿠커 폼에서 오는 타이머 설정 이벤트
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="min"></param>
        /// <param name="sec"></param>
        void SetTimeChangeSender(int idx, int min, int sec)
        {
            main.core_Object.stepModuleCore.mChiken[idx].SetTimeChanege(new TimeSpan(0, min, sec));
            Cores.Core_Object.GetObj_File.tsChikenSetMinTime[idx] = min;
            Cores.Core_Object.GetObj_File.tsChikenSetSecTime[idx] = sec;            
            main.core_Object.ObjectFIle_Save();
        }

        /// <summary>
        /// UcLoader
        /// 로더 폼에서 오는 투입배출 변경 이벤트
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="load"></param>
        void SetLoaderChangeSender(int idx, string load)
        {
            //main.core_Object.stepModuleCore.mChiken[idx].SetTimeChanege(new TimeSpan(0, min, sec));
            //Cores.Core_Object.GetObj_File.tsChikenSetMinTime[idx] = min;
            //Cores.Core_Object.GetObj_File.tsChikenSetSecTime[idx] = sec;

            Cores.Core_Object.GetObj_File.lstLoaderSetIO[idx] = load;
            main.core_Object.ObjectFIle_Save();

            if (load == "IN")
            {
                FormMain.gui_file.type[idx] = devJace.Files.ModuleType.Load;


                switch (idx)
                {
                    case 0:
                        FormMain.gui_file.strName[idx] = "Load A";
                        break;

                    case 1:
                        FormMain.gui_file.strName[idx] = "Load B";
                        break;

                    case 2:
                        FormMain.gui_file.strName[idx] = "Load C";
                        break;

                }

                
            }
            else if(load == "OUT")
            {
                FormMain.gui_file.type[idx] = devJace.Files.ModuleType.UnLoad;


                switch (idx)
                {
                    case 0:
                        FormMain.gui_file.strName[idx] = "UnLoad A";
                        break;

                    case 1:
                        FormMain.gui_file.strName[idx] = "UnLoad B";
                        break;

                    case 2:
                        FormMain.gui_file.strName[idx] = "UnLoad C";
                        break;

                }
            }


            
        }

        /// <summary>
        /// 조리 준비 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbButtonReady_Click(object sender, EventArgs e)
        {
            //2023.04.11 TeMp

            //2023.03.20 ::: 접속 안되어 있는 경우
            //if (Cores.Core_Object.cobotConneted == false)
            //{
            //    devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Fatal,
            //           $"| Cobot Connection : {Cores.Core_Object.cobotConneted}");
            //
            //    Common.FormMessageBox msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None, "코봇이 연결되지 않았습니다.");
            //    msg.ShowDialog();              
            //    return;
            //}
            //
            ////2023.03.21 ::: 모션 및 아이오 인터락
            //for (int idx = 0; idx < Cores.Core_Object.fasTechConneted.Length; idx++)
            //{
            //    if (Cores.Core_Object.fasTechConneted[idx] == false)
            //    {
            //        devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Fatal,
            //           $"| Motion & IO Connection : {Cores.Core_Object.fasTechConneted[idx]}/{idx+1}");
            //
            //        Common.FormMessageBox msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None, "모션 & 아이오가 연결되지 않았습니다.");
            //        msg.ShowDialog();
            //        return;
            //    }
            //}
            ////2023.03.21 ::: 로봇 상태에 따라 인터락 처리 부분
            //
            ////2023.04.04 ::: IO 상태에 따라.
            //if(Fas_Data.lstAxis_State[(int)Cores.EAxis_Status.Err_Sevo_Alarm] == true)
            //{
            //     devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Fatal,
            //              $"| Motion : {Fas_Data.lstAxis_State[(int)Cores.EAxis_Status.Err_Sevo_Alarm]}");
            //
            //     Common.FormMessageBox msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None, "모션 & 서보 알람 해제 후 사용하세요.");
            //     msg.ShowDialog();
            //     return;
            //
            //}

            //input
            //main.core_Object.stepModuleCore.lstOilCheckdCount[0]++;
            //main.core_Object.stepModuleCore.lstOilCheckdCount[1]++;
            //main.core_Object.stepModuleCore.lstOilCheckdCount[2]++;

            //for (int i = 0; i < 2; i++)
            //{
            //    main.core_Object.stepModuleCore.mLoders[i].IsStartLatch = true;
            //}

            if (Define.IsDebugPass == false)
            {
                bool IsConnect = true;
                Common.FormMessageBox msg;
                IsConnect &= Cores.Core_Object.cobotConneted;
                if (IsConnect == false)
                {
                    msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None, "로봇 연결 후 버튼 클릭 바랍니다.");
                    msg.ShowDialog();
                    return;
                }

                for (int idx = 0; idx < Cores.Core_Object.fasTechConneted.Length; idx++)
                {
                    IsConnect &= Cores.Core_Object.fasTechConneted[idx];
                    if (IsConnect == false)
                    {
                        msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None, $"디바이스 {idx} 장치 연결 후 버튼 클릭 바랍니다.");
                        msg.ShowDialog();
                        return;
                    }
                }


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


                //2023.05.22 ::: 회피위치에서 리셋팅 동작 인터락 막아버리기
                if (Cores.Core_StepModule.Cobot_Pos_Compare(TaskPos, Cores.Core_Object.GetCos_File.Joint[(int)MyActionXStepBuffer.Clearing]) == true)
                {
                    msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None, $"수동동작에서 로봇 사용위치로 변경 후 사용 바랍니다.");
                    msg.ShowDialog();
                    return;
                }
            }

         




            PackML.Command cmd = PackML.Command.CurrentState_None;

            switch (main.core_Object.processCore.eCurrState)
            {
                //case PackML.ECurrState.CurrentState_Idle:
                //    cmd = PackML.Command.CurrentState_Starting;
                //    break;
                //case PackML.ECurrState.CurrentState_Excute:
                //    cmd = PackML.Command.CurrentState_Stopping;
                //    break;
                //case PackML.ECurrState.CurrentState_Hold:
                //    cmd = PackML.Command.CurrentState_Stopping;
                //    break;
                case PackML.ECurrState.CurrentState_Stopped:
                    cmd = PackML.Command.CurrentState_Resetting;
                    break;
                //case PackML.ECurrState.CurrentState_Starting:
                //    cmd = PackML.Command.CurrentState_Stopping;
                //    break;
            }
            if (cmd != PackML.Command.CurrentState_None)
            {
                main.core_Object.processCore.Set_State_Matrix(cmd);
            }
        }
    }
}
