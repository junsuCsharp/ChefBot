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

using devJace.Files;

namespace Forms
{
    public partial class FormConfigs : Form
    {
        public FormConfigs()
        {
            InitializeComponent();

            devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
             $"{System.Reflection.MethodBase.GetCurrentMethod().Name}");
        }

        public FormConfigs(Project_Main.FormMain mainForm)
        {
            InitializeComponent();

            main = mainForm;

            devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
             $"{System.Reflection.MethodBase.GetCurrentMethod().Name}");
        }      

        //public UserBtn_Click Btn_Click_Event;
        //public delegate void UserBtn_Click(object sender, EventArgs e);

        private Project_Main.FormMain main;

        List<RadioButton> radioButtons = new List<RadioButton>();

        const int iGridCount = 11;
        string[] strPreviousBuff = new string[iGridCount];
        int iSeltedDataGrid = 0;
        const int iCobotNumber = 3;

        private void FormConfigs_Load(object sender, EventArgs e)
        {
            radioButtons.Add(radioButton1);
            radioButtons.Add(radioButton2);
            radioButtons.Add(radioButton3);
            radioButtons.Add(radioButton4);
            radioButtons.Add(radioButton5);

            foreach (RadioButton rb in radioButtons)
            {
                //rb.Click += Rb_Click;
                //rb.CheckedChanged += Rb_CheckedChanged;
                rb.BackColor = devi.Define.colorSubButtonColor;
            }

            comboBox1.Items.Clear();
            comboBox1.Items.Add("Wait");
            comboBox1.Items.Add("Load_A");
            comboBox1.Items.Add("Load_B");
            comboBox1.Items.Add("Load_C");
            comboBox1.Items.Add("Cooker_1-1");
            comboBox1.Items.Add("Cooker_1-2");
            comboBox1.Items.Add("Cooker_2-1");
            comboBox1.Items.Add("Cooker_2-2");
            comboBox1.Items.Add("Cooker_3-1");
            comboBox1.Items.Add("Cooker_3-2");
            comboBox1.Items.Add("User_Origin");
            comboBox1.Items.Add("Package");
            comboBox1.Items.Add("Cleaning");

            DefaultGUI();
            Init_Grid_CobotPos();
            radioButtons[iSeltedDataGrid].Checked = true;
        }

        private void Rb_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbSeleted = sender as RadioButton;
            int index = 0;
            int iSelectedIndex = 0;
            foreach (RadioButton rb in radioButtons)
            {
                index++;
                if (rb.Checked)
                {
                    //rb.Checked = true;
                    rb.BackColor = devi.Define.colorMainButtonBoldColor;
                    iSelectedIndex = index;
                    Init_Grid(iSelectedIndex);

                    
                }
                else
                {
                    rb.BackColor = devi.Define.colorSubButtonColor;
                }
            }

            //2023.03.17 ::: 코봇 위치 창 활성화 추가
            if (rbSeleted == radioButton3)
            {
                panelCobot.Visible = true;
            }
            else
            {
                panelCobot.Visible = false;
            }

            iSeltedDataGrid = iSelectedIndex;
        }      

        void DefaultGUI()
        {
            int fontSize = 12;
            foreach (RadioButton rb in radioButtons)
            {
                rb.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
                rb.ForeColor = Color.White;
            }
            BtnCancel.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            BtnCancel.ForeColor = Color.Black;

            BtnOk.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            BtnOk.ForeColor = Color.Black;
            //fontSize = 30;
            //metroButtonDelete.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            //metroButtonCansel.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            //metroButtonSave.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            //
            //metroButtonDelete.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            //metroButtonCansel.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            //metroButtonSave.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);


            comboBox1.Font = new Font(Fonts.FontLibrary.Families[0], 23);
            dataGridView2.Width = 1010;
        }

        void Init_Grid(int recipeNumber)
        {
            DataGridView dgv = dataGridView1;

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

            dgv.ColumnHeadersHeight = 70;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            dgv.Rows.Clear();
            dgv.Columns.Clear();
            dgv.ColumnHeadersVisible = true;

            int iColWidth = 400;

            string[] header = new string[] { };            
            dgv.RowCount = iGridCount;
            switch (recipeNumber)
            {
                case 1:
                    header = new string[] {"리스트", "현재 값", "변경 값", "범위", "단위"  };
                    dgv.ColumnCount = header.Length;                    
                    dgv.Columns[0].Width = iColWidth;
                    dgv.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;                 
                    dgv.Columns[0].HeaderText = header[0];
                    for (int ix = 0; ix < dgv.ColumnCount; ix++)
                    {
                        if (ix >= 1)
                        {
                            dgv.Columns[ix].HeaderText = $"{header[ix]}";
                            dgv.Columns[ix].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            dgv.Columns[ix].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        }

                        dgv.Columns[ix].Resizable = DataGridViewTriState.False;
                        dgv.Columns[ix].SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                    for (int ux = 0; ux < Cores.Core_Object.GetObj_File.iMaxOperation; ux++)
                    {
                        dgv.Rows[ux].Cells[0].Value = Cores.Core_Object.GetObj_File.lstOperRecipeName[ux];
                        dgv.Rows[ux].Cells[3].Value = Cores.Core_Object.GetObj_File.lstOperRecipeRange[ux];
                        dgv.Rows[ux].Cells[4].Value = Cores.Core_Object.GetObj_File.lstOperRecipeUnit[ux];
                    }
                    //현재 값 순서 변경 하면 안됨
                    dgv.Rows[0].Cells[1].Value = Cores.Core_Object.GetObj_File.iCobotSpeed;
                    dgv.Rows[1].Cells[1].Value = Cores.Core_Object.GetObj_File.iXaxisSpeed;
                    dgv.Rows[2].Cells[1].Value = Cores.Core_Object.GetObj_File.iXaxisAccDecTime; 
                    dgv.Rows[3].Cells[1].Value = Cores.Core_Object.GetObj_File.iLoadDelayTime;
                    dgv.Rows[4].Cells[1].Value = Cores.Core_Object.GetObj_File.iUnLoadDelayTime;
                    dgv.Rows[5].Cells[1].Value = Cores.Core_Object.GetObj_File.iBasketDelayTime;
                    //dgv.Rows[5].Cells[1].Value = Cores.Core_Object.GetObj_File.iSwitch1SetTimeMin;
                    //dgv.Rows[6].Cells[1].Value = Cores.Core_Object.GetObj_File.iSwitch1SetTimeSec;
                    //dgv.Rows[7].Cells[1].Value = Cores.Core_Object.GetObj_File.iSwitch2SetTimeMin;
                    //dgv.Rows[8].Cells[1].Value = Cores.Core_Object.GetObj_File.iSwitch2SetTimeSec;
                    break;
                case 2:
                    header = new string[] { "리스트", "현재 값", "변경 값", "범위", "단위" };
                    dgv.ColumnCount = header.Length;                    
                    dgv.Columns[0].Width = iColWidth;
                    dgv.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;                    
                    dgv.Columns[0].HeaderText = header[0];
                    for (int ix = 0; ix < dgv.ColumnCount; ix++)
                    {
                        if (ix >= 1)
                        {
                            dgv.Columns[ix].HeaderText = $"{header[ix]}";
                            dgv.Columns[ix].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            dgv.Columns[ix].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        }

                        dgv.Columns[ix].Resizable = DataGridViewTriState.False;
                        dgv.Columns[ix].SortMode = DataGridViewColumnSortMode.NotSortable;

                    }

                    Cores.Core_Object.GetPos_File.lstPositions = new List<int>();
                    for (int ux = 0; ux < Cores.Core_Object.GetPos_File.iMaxPosition; ux++)
                    {
                        dgv.Rows[ux].Cells[0].Value = Cores.Core_Object.GetPos_File.strXposNames[ux];
                        if (Cores.Core_Object.GetPos_File.lstRealPositions.Count != 0)
                        {
                            dgv.Rows[ux].Cells[1].Value = Cores.Core_Object.GetPos_File.lstRealPositions[ux].ToString("000.00");

                            Cores.Core_Object.GetPos_File.lstPositions.Add(Cores.Fas_Func.PPS_To_mm(Cores.Core_Object.GetPos_File.lstRealPositions[ux]));

                            dgv.Rows[ux].Cells[3].Value = Cores.Core_Object.GetPos_File.strXposRanges[ux];
                            dgv.Rows[ux].Cells[4].Value = Cores.Core_Object.GetPos_File.strXposUnit[ux];
                        }
                        else
                        {
                            dgv.Rows[ux].Cells[1].Value = 0;
                            dgv.Rows[ux].Cells[3].Value = "-10 ~ +1600";
                            dgv.Rows[ux].Cells[4].Value = "mm";
                        }
                        
                    }
                    break;
                case 3:
                    header = new string[] { "리스트", "J1", "J2", "J3", "J4", "J5", "J6", "J1", "J2", "J3", "J4", "J5", "J6", "범위", "단위" };
                    dgv.ColumnCount = header.Length;
                    dgv.Columns[0].Width = iColWidth;
                    dgv.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    dgv.Columns[0].HeaderText = header[0];
                    dgv.RowCount = Cores.Core_Object.GetCos_File.iMaxLength;
                    dgv.Columns[14].Width = 20;
                    for (int ix = 0; ix < dgv.ColumnCount; ix++)
                    {
                        if (ix >= 1)
                        {
                            dgv.Columns[ix].HeaderText = $"{header[ix]}";
                            dgv.Columns[ix].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            dgv.Columns[ix].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        }

                        dgv.Columns[ix].Resizable = DataGridViewTriState.False;
                        dgv.Columns[ix].SortMode = DataGridViewColumnSortMode.NotSortable;

                        if (ix >= 1 && ix <= 12)
                        {
                            dgv.Columns[ix].HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomCenter;
                        }

                    }

                    for (int ux = 0; ux < Cores.Core_Object.GetCos_File.iMaxLength; ux++)
                    {
                        dgv.Rows[ux].Cells[0].Value = Cores.Core_Object.GetCos_File.strName[ux];
                        if (Cores.Core_Object.GetCos_File.Joint.Count != 0)
                        {
                            dgv.Rows[ux].Cells[1].Value = Cores.Core_Object.GetCos_File.Joint[ux][0].ToString("000.0");
                            dgv.Rows[ux].Cells[2].Value = Cores.Core_Object.GetCos_File.Joint[ux][1].ToString("000.0");
                            dgv.Rows[ux].Cells[3].Value = Cores.Core_Object.GetCos_File.Joint[ux][2].ToString("000.0");
                            dgv.Rows[ux].Cells[4].Value = Cores.Core_Object.GetCos_File.Joint[ux][3].ToString("000.0");
                            dgv.Rows[ux].Cells[5].Value = Cores.Core_Object.GetCos_File.Joint[ux][4].ToString("000.0");
                            dgv.Rows[ux].Cells[6].Value = Cores.Core_Object.GetCos_File.Joint[ux][5].ToString("000.0");

                            
                            dgv.Rows[ux].Cells[13].Value = "-360 ~ +360";
                            dgv.Rows[ux].Cells[14].Value = "℃";
                        }
                        else
                        {
                            
                        }

                    }
                    break;
                case 4:
                    //header = new string[] { "리스트", "현재 값", "변경 값", "-", "-" };
                    //dgv.ColumnCount = header.Length;
                    ////dgv.RowCount = main.core_Object.GetObj_File.iMaxDevice;
                    //dgv.Columns[0].Width = iColWidth;
                    //dgv.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    //dgv.Columns[0].HeaderText = header[0];
                    //for (int ix = 0; ix < dgv.ColumnCount; ix++)
                    //{
                    //    if (ix >= 1)
                    //    {
                    //        dgv.Columns[ix].HeaderText = $"{header[ix]}";
                    //        dgv.Columns[ix].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    //        dgv.Columns[ix].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    //    }
                    //
                    //    dgv.Columns[ix].Resizable = DataGridViewTriState.False;
                    //    dgv.Columns[ix].SortMode = DataGridViewColumnSortMode.NotSortable;
                    //
                    //}
                    //
                    //
                    //for (int ux = 0; ux < Cores.Core_Object.GetObj_File.iMaxDevice; ux++)
                    //{
                    //    dgv.Rows[ux].Cells[0].Value = Cores.Core_Object.GetObj_File.Device_Name[ux];
                    //    dgv.Rows[ux].Cells[1].Value = Cores.Core_Object.GetObj_File.Device_IP[ux];
                    //}
                    header = new string[] { "리스트", "현재 값", "변경 값", "범위", "단위" };
                    dgv.ColumnCount = header.Length;
                    //dgv.RowCount = main.core_Object.GetObj_File.iMaxOption;
                    dgv.Columns[0].Width = iColWidth;
                    dgv.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    dgv.Columns[0].HeaderText = header[0];
                    for (int ix = 0; ix < dgv.ColumnCount; ix++)
                    {
                        if (ix >= 1)
                        {
                            dgv.Columns[ix].HeaderText = $"{header[ix]}";
                            dgv.Columns[ix].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            dgv.Columns[ix].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        }

                        dgv.Columns[ix].Resizable = DataGridViewTriState.False;
                        dgv.Columns[ix].SortMode = DataGridViewColumnSortMode.NotSortable;

                    }


                    for (int ux = 0; ux < Cores.Core_Object.GetObj_File.iMaxExOption; ux++)
                    {
                        dgv.Rows[ux].Cells[0].Value = Cores.Core_Object.GetObj_File.strExOptionNames[ux];
                        dgv.Rows[ux].Cells[3].Value = Cores.Core_Object.GetObj_File.strExOptionRanges[ux];
                        dgv.Rows[ux].Cells[4].Value = Cores.Core_Object.GetObj_File.strExOptionUnits[ux];
                    }
                    //현재 값 순서 변경 하면 안됨
                    dgv.Rows[0].Cells[1].Value = Cores.Core_Object.GetObj_File.iBasketShakingSetMinTime;
                    dgv.Rows[1].Cells[1].Value = Cores.Core_Object.GetObj_File.iBasketShakingSetSecTime;
                    dgv.Rows[2].Cells[1].Value = Cores.Core_Object.GetObj_File.iBasketOxzenSetMinTime;
                    dgv.Rows[3].Cells[1].Value = Cores.Core_Object.GetObj_File.iBasketOxzenSetSecTime;
                    dgv.Rows[4].Cells[1].Value = Cores.Core_Object.GetObj_File.inputdelayMintime;
                    dgv.Rows[5].Cells[1].Value = Cores.Core_Object.GetObj_File.inputdelaySectime;
                    dgv.Rows[6].Cells[1].Value = Cores.Core_Object.GetObj_File.iSwitch1SetTimeMin;
                    dgv.Rows[7].Cells[1].Value = Cores.Core_Object.GetObj_File.iSwitch1SetTimeSec;
                    dgv.Rows[8].Cells[1].Value = Cores.Core_Object.GetObj_File.iSwitch2SetTimeMin;
                    dgv.Rows[9].Cells[1].Value = Cores.Core_Object.GetObj_File.iSwitch2SetTimeSec;
                    dgv.Rows[10].Cells[1].Value = Cores.Core_Object.GetObj_File.iMidOtwoShower;


                    break;
                case 5:
                    header = new string[] { "리스트", "현재 값", "변경 값", "범위", "단위" };
                    dgv.ColumnCount = header.Length;
                    //dgv.RowCount = main.core_Object.GetObj_File.iMaxOption;
                    dgv.Columns[0].Width = iColWidth;
                    dgv.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    dgv.Columns[0].HeaderText = header[0];
                    for (int ix = 0; ix < dgv.ColumnCount; ix++)
                    {
                        if (ix >= 1)
                        {
                            dgv.Columns[ix].HeaderText = $"{header[ix]}";
                            dgv.Columns[ix].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            dgv.Columns[ix].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        }

                        dgv.Columns[ix].Resizable = DataGridViewTriState.False;
                        dgv.Columns[ix].SortMode = DataGridViewColumnSortMode.NotSortable;

                    }


                    for (int ux = 0; ux < Cores.Core_Object.GetObj_File.iMaxOption; ux++)
                    {
                        dgv.Rows[ux].Cells[0].Value = Cores.Core_Object.GetObj_File.lstOptionName[ux];
                        dgv.Rows[ux].Cells[3].Value = Cores.Core_Object.GetObj_File.lstOptionRange[ux];
                        dgv.Rows[ux].Cells[4].Value = Cores.Core_Object.GetObj_File.lstOptionUnit[ux];
                    }
                    //현재 값 순서 변경 하면 안됨
                    dgv.Rows[0].Cells[1].Value = Cores.Core_Object.GetObj_File.iLanguage;
                    dgv.Rows[1].Cells[1].Value = Cores.Core_Object.GetObj_File.iXaxisControl;
                    dgv.Rows[2].Cells[1].Value = Cores.Core_Object.GetObj_File.iLaserScannerUse;
                    dgv.Rows[3].Cells[1].Value = Cores.Core_Object.GetObj_File.iGripperUse;
                    dgv.Rows[4].Cells[1].Value = Cores.Core_Object.GetObj_File.iBasketShaking;                    
                    dgv.Rows[5].Cells[1].Value = Cores.Core_Object.GetObj_File.iBasketShakingCount;
                    dgv.Rows[6].Cells[1].Value = Cores.Core_Object.GetObj_File.iOilDrainUse;
                    dgv.Rows[7].Cells[1].Value = Cores.Core_Object.GetObj_File.iOilDrainCount;  
                    dgv.Rows[8].Cells[1].Value = Cores.Core_Object.GetObj_File.dBasketWeight;

                    
                    dgv.Rows[9].Cells[1].Value = Cores.Core_Object.GetObj_File.iMidShakingCount;
                    dgv.Rows[10].Cells[1].Value = Cores.Core_Object.GetObj_File.iTechingUse;
                    //dgv.Rows[8].Cells[1].Value = Cores.Core_Object.GetObj_File.iLaserScannerUse;
                    break;
            }


            int fontSize = 11;
            for (int ux = 0; ux < dgv.RowCount; ux++)
            {   
                if (ux % 2 == 0)
                {
                    
                    dgv.Rows[ux].HeaderCell.Style.BackColor = Color.DarkGray;
                    dgv.Rows[ux].Cells[0].Style.BackColor = Color.White;
                }
                else
                {
                    dgv.Rows[ux].HeaderCell.Style.BackColor = Color.DarkGray;
                    dgv.Rows[ux].Cells[0].Style.BackColor = Color.AliceBlue;
                }

                dgv.Rows[ux].Height = 50;
                for (int col = 0; col < dgv.Columns.Count; col++)
                {
                    dgv.Rows[ux].Cells[col].Style.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
                }

                try
                {
                    if (dgv.Rows[ux].Cells[2].Value == null)
                    {
                        if (dgv.Rows[ux].Cells[1].Value == null)
                        {
                            strPreviousBuff[ux] = null;
                        }
                        else
                        {
                            strPreviousBuff[ux] = dgv.Rows[ux].Cells[1].Value.ToString();
                        }
                            
                    }
                    else
                    {
                        strPreviousBuff[ux] = dgv.Rows[ux].Cells[2].Value.ToString();
                    }
                }
                catch
                { 
                
                }
             
                
            }           
            dgv.ClearSelection();
        }

        void Init_Grid_CobotPos()
        {  
            DataGridView dgv = dataGridView2;

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

            dgv.ColumnHeadersHeight = 30;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            dgv.Rows.Clear();
            dgv.Columns.Clear();
            dgv.ColumnHeadersVisible = true;

            int iColWidth = 397;

            string[] header = new string[] { };
            dgv.RowCount = 2;

            header = new string[] { "위치", "J1", "J2", "J3", "J4", "J5", "J6" };
            dgv.ColumnCount = header.Length;
            //dgv.RowCount = main.core_Object.GetObj_File.iMaxOption;
            dgv.Columns[0].Width = iColWidth;
            dgv.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv.Columns[0].HeaderText = header[0];

            int fontSize = 11;
            for (int ux = 0; ux < dgv.RowCount; ux++)
            {
                if (ux % 2 == 0)
                {
                    dgv.Rows[ux].HeaderCell.Style.BackColor = Color.DarkGray;
                    dgv.Rows[ux].Cells[0].Style.BackColor = Color.White;
                }
                else
                {
                    dgv.Rows[ux].HeaderCell.Style.BackColor = Color.DarkGray;
                    dgv.Rows[ux].Cells[0].Style.BackColor = Color.AliceBlue;
                }

                dgv.Rows[ux].Height = 28;
                for (int col = 0; col < dgv.Columns.Count; col++)
                {
                    dgv.Rows[ux].Cells[col].Style.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
                    if (col >= 1)
                    {
                        dgv.Columns[col].HeaderText = $"{header[col]}";
                        dgv.Columns[col].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        dgv.Columns[col].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }

                    dgv.Columns[col].Resizable = DataGridViewTriState.False;
                    dgv.Columns[col].SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                try
                {
                    if (dgv.Rows[ux].Cells[2].Value == null)
                    {
                        if (dgv.Rows[ux].Cells[1].Value == null)
                        {
                            strPreviousBuff[ux] = null;
                        }
                        else
                        {
                            strPreviousBuff[ux] = dgv.Rows[ux].Cells[1].Value.ToString();
                        }

                    }
                    else
                    {
                        strPreviousBuff[ux] = dgv.Rows[ux].Cells[2].Value.ToString();
                    }
                }
                catch
                {

                }


            }
            dgv.ClearSelection();
        }

        public void Coobt_Pos_Update()
        {
            //Init_Grid_CobotPos();
            var joint1 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 270);
            var joint2 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 271);
            var joint3 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 272);
            var joint4 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 273);
            var joint5 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 274);
            var joint6 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 275);

            var task1 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 400);
            var task2 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 401);
            var task3 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 402);
            var task4 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 403);
            var task5 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 404);
            var task6 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 405);

            DataGridView dgv = dataGridView2;
            try
            {
                dgv.Rows[0].Cells[0].Value = "Joint";
                dgv.Rows[0].Cells[1].Value = joint1.strData;
                dgv.Rows[0].Cells[2].Value = joint2.strData;
                dgv.Rows[0].Cells[3].Value = joint3.strData;
                dgv.Rows[0].Cells[4].Value = joint4.strData;
                dgv.Rows[0].Cells[5].Value = joint5.strData;
                dgv.Rows[0].Cells[6].Value = joint6.strData;

                dgv.Rows[1].Cells[0].Value = "Task";
                dgv.Rows[1].Cells[1].Value = task1.strData;
                dgv.Rows[1].Cells[2].Value = task2.strData;
                dgv.Rows[1].Cells[3].Value = task3.strData;
                dgv.Rows[1].Cells[4].Value = task4.strData;
                dgv.Rows[1].Cells[5].Value = task5.strData;
                dgv.Rows[1].Cells[6].Value = task6.strData;
            }
            catch
            { }
            
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridView view = sender as DataGridView;

            if (view.Rows[e.RowIndex].Cells[0].Value == null)
                return;

            if (radioButton3.Checked && e.ColumnIndex < 7 || e.ColumnIndex > 12)
                return;


            string strRecipeName = null;
            foreach (RadioButton rb in radioButtons)
            {
                if (rb.Checked)
                {
                    //rb.Checked = true;
                    strRecipeName = rb.Name;
                    break;
                }
            }

            string prevData = null;
            if (radioButton3.Checked == false)
            {
                prevData = view.Rows[e.RowIndex].Cells[1].Value.ToString();
                //strPreviousBuff[e.RowIndex] = prevData;
                Common.FormKeypad keypad = new Common.FormKeypad(prevData);

                if (keypad.ShowDialog() == DialogResult.OK)
                {
                    if (keypad.UserInputNo != null)
                    {
                        view.Rows[e.RowIndex].Cells[2].Value = keypad.UserInputNo;
                        strPreviousBuff[e.RowIndex] = keypad.UserInputNo;

                        devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
                            $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                            $" | Seleted Recipe Header {strRecipeName} | { view.Rows[e.RowIndex].Cells[1].Value}");
                    }
                }
            }
            else
            {
                prevData = view.Rows[e.RowIndex].Cells[e.ColumnIndex - 6].Value.ToString();
                //strPreviousBuff[e.RowIndex] = prevData;
                Common.FormKeypad keypad = new Common.FormKeypad(prevData);

                if (keypad.ShowDialog() == DialogResult.OK)
                {
                    if (keypad.UserInputNo != null)
                    {
                        view.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = keypad.UserInputNo;
                        strPreviousBuff[e.RowIndex] = keypad.UserInputNo;

                        devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
                            $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                            $" | Seleted Recipe Header {strRecipeName} | { view.Rows[e.RowIndex].Cells[1].Value}");
                    }
                }

            }
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            for (int idx = 0; idx < radioButtons.Count; idx++)
            {
                if (radioButtons[idx].Checked)
                {
                    switch (idx + 1)
                    {
                        case 1:

                            if (strPreviousBuff[0] != null)
                            {
                                Cores.Core_Object.GetObj_File.iCobotSpeed = int.Parse(strPreviousBuff[0]);
                                //코봇 스피드 적용
                                main.core_Object.Modbus_Sender(143, Cores.Core_Object.GetObj_File.iCobotSpeed);
                            }
                            if (strPreviousBuff[1] != null)
                            {
                                Cores.Core_Object.GetObj_File.iXaxisSpeed = int.Parse(strPreviousBuff[1]);
                            }
                            if (strPreviousBuff[2] != null)
                            {
                                Cores.Core_Object.GetObj_File.iXaxisAccDecTime = int.Parse(strPreviousBuff[2]);
                            }
                            if (strPreviousBuff[3] != null)
                            {
                                Cores.Core_Object.GetObj_File.iLoadDelayTime = int.Parse(strPreviousBuff[3]);
                            }
                            if (strPreviousBuff[4] != null)
                            {
                                Cores.Core_Object.GetObj_File.iUnLoadDelayTime = int.Parse(strPreviousBuff[4]);
                            }
                            if (strPreviousBuff[5] != null)
                            {
                                Cores.Core_Object.GetObj_File.iBasketDelayTime = int.Parse(strPreviousBuff[5]);
                            }
                            //if (strPreviousBuff[5] != null)
                            //{
                            //    Cores.Core_Object.GetObj_File.iSwitch1SetTimeMin = int.Parse(strPreviousBuff[5]);
                            //}
                            //if (strPreviousBuff[6] != null)
                            //{
                            //    Cores.Core_Object.GetObj_File.iSwitch1SetTimeSec = int.Parse(strPreviousBuff[6]);
                            //}
                            //if (strPreviousBuff[7] != null)
                            //{
                            //    Cores.Core_Object.GetObj_File.iSwitch2SetTimeMin = int.Parse(strPreviousBuff[7]);
                            //}
                            //if (strPreviousBuff[8] != null)
                            //{
                            //    Cores.Core_Object.GetObj_File.iSwitch2SetTimeSec = int.Parse(strPreviousBuff[8]);
                            //}
                            
                            break;
                        case 2:
                            //Cores.Core_Object.GetPos_File.lstRealPositions = new List<double>();
                            //2023.03.20 :::: temp
                            for (int ux = 0; ux < Cores.Core_Object.GetPos_File.iMaxPosition; ux++)
                            {
                                //main.core_Object.GetPos_File.lstRealPositions[ux] = double.Parse(dataGridView1.Rows[ux].Cells[1].Value.ToString());

                                if (strPreviousBuff[ux] != null)
                                {
                                    Cores.Core_Object.GetPos_File.lstRealPositions[ux] = double.Parse(strPreviousBuff[ux]);
                                }


                                //Cores.Core_Object.GetPos_File.lstRealPositions.Add(double.Parse(strPreviousBuff[ux]));
                            }
                            break;
                        case 3:
                            
                            break;

                        case 4:
                            //for (int ux = 0; ux < Cores.Core_Object.GetObj_File.iMaxDevice; ux++)
                            //{
                            //    //main.core_Object.GetObj_File.Device_IP[ux] = dataGridView1.Rows[ux].Cells[1].Value.ToString(); 

                            //    if (strPreviousBuff[ux] != null)
                            //    {
                            //        Cores.Core_Object.GetObj_File.Device_IP[ux] = strPreviousBuff[ux];
                            //    }
                            //}
                            if (strPreviousBuff[0] != null)
                            {
                                Cores.Core_Object.GetObj_File.iBasketShakingSetMinTime = int.Parse(strPreviousBuff[0]);                                
                            }
                            if (strPreviousBuff[1] != null)
                            {
                                Cores.Core_Object.GetObj_File.iBasketShakingSetSecTime = int.Parse(strPreviousBuff[1]);
                            }
                            if (strPreviousBuff[2] != null)
                            {
                                Cores.Core_Object.GetObj_File.iBasketOxzenSetMinTime = int.Parse(strPreviousBuff[2]);
                            }
                            if (strPreviousBuff[3] != null)
                            {
                                Cores.Core_Object.GetObj_File.iBasketOxzenSetSecTime = int.Parse(strPreviousBuff[3]);
                            }
                            if (strPreviousBuff[4] != null)
                            {
                                Cores.Core_Object.GetObj_File.inputdelayMintime = int.Parse(strPreviousBuff[4]);
                            }
                            if (strPreviousBuff[5] != null)
                            {
                                Cores.Core_Object.GetObj_File.inputdelaySectime = int.Parse(strPreviousBuff[5]);
                            }
                            if (strPreviousBuff[6] != null)
                            {
                                Cores.Core_Object.GetObj_File.iSwitch1SetTimeMin = int.Parse(strPreviousBuff[6]);
                            }
                            if (strPreviousBuff[7] != null)
                            {
                                Cores.Core_Object.GetObj_File.iSwitch1SetTimeSec = int.Parse(strPreviousBuff[7]);
                            }
                            if (strPreviousBuff[8] != null)
                            {
                                Cores.Core_Object.GetObj_File.iSwitch2SetTimeMin = int.Parse(strPreviousBuff[8]);
                            }
                            if (strPreviousBuff[9] != null)
                            {
                                Cores.Core_Object.GetObj_File.iSwitch2SetTimeSec = int.Parse(strPreviousBuff[9]);
                            }
                            if (strPreviousBuff[10] != null)
                            {
                                Cores.Core_Object.GetObj_File.iMidOtwoShower = int.Parse(strPreviousBuff[10]);
                            }
                            break;
                        case 5:
                            try
                            {
                                if (strPreviousBuff[0] != null)
                                {
                                    Cores.Core_Object.GetObj_File.iLanguage = int.Parse(strPreviousBuff[0]);
                                }
                                if (strPreviousBuff[1] != null)
                                {
                                    Cores.Core_Object.GetObj_File.iXaxisControl = int.Parse(strPreviousBuff[1]);
                                }
                                if (strPreviousBuff[2] != null)
                                {
                                    Cores.Core_Object.GetObj_File.iLaserScannerUse = int.Parse(strPreviousBuff[2]);
                                }
                                if (strPreviousBuff[3] != null)
                                {
                                    Cores.Core_Object.GetObj_File.iGripperUse = int.Parse(strPreviousBuff[3]);
                                }
                                if (strPreviousBuff[4] != null)
                                {
                                    Cores.Core_Object.GetObj_File.iBasketShaking = int.Parse(strPreviousBuff[4]);
                                }
                                if (strPreviousBuff[5] != null)
                                {
                                    Cores.Core_Object.GetObj_File.iBasketShakingCount = int.Parse(strPreviousBuff[5]);
                                }
                                if (strPreviousBuff[6] != null)
                                {
                                    Cores.Core_Object.GetObj_File.iOilDrainUse = int.Parse(strPreviousBuff[6]);
                                }
                                if (strPreviousBuff[7] != null)
                                {
                                    Cores.Core_Object.GetObj_File.iOilDrainCount = int.Parse(strPreviousBuff[7]);
                                }
                                if (strPreviousBuff[8] != null)
                                {
                                    Cores.Core_Object.GetObj_File.dBasketWeight = double.Parse(strPreviousBuff[8]);
                                }
                                if (strPreviousBuff[9] != null)
                                {
                                    Cores.Core_Object.GetObj_File.iMidShakingCount = int.Parse(strPreviousBuff[9]);
                                }
                                if (strPreviousBuff[10] != null)
                                {
                                    Cores.Core_Object.GetObj_File.iTechingUse = int.Parse(strPreviousBuff[10]);
                                }
                            }
                            catch
                            {
                                devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Fatal,
                                   $"{System.Reflection.MethodBase.GetCurrentMethod().Name}" +
                                   $" | Data Error");
                            }

                            
                            //Cores.Core_Object.GetObj_File.iLaserScannerUse       = int.Parse(strPreviousBuff[8]);


                            //주소 쓰기
                            //main.core_Object.Modbus_Sender(128, Cores.Core_Object.GetObj_File.iCobotSpeed);
                            //main.core_Object.GetObj_File.iLanguage  = int.Parse(dataGridView1.Rows[0].Cells[1].Value.ToString());
                            //main.core_Object.GetObj_File.iXaxisControl = int.Parse(dataGridView1.Rows[1].Cells[1].Value.ToString());
                            //main.core_Object.GetObj_File.iSafetyLaserScanner = int.Parse(dataGridView1.Rows[2].Cells[1].Value.ToString());
                            //main.core_Object.GetObj_File.iBasketShaking = int.Parse(dataGridView1.Rows[3].Cells[1].Value.ToString());
                            //main.core_Object.GetObj_File.dBasketWeight = double.Parse(dataGridView1.Rows[4].Cells[1].Value.ToString());
                            //main.core_Object.GetObj_File.dMeterialWeight = double.Parse(dataGridView1.Rows[5].Cells[1].Value.ToString());
                            //main.core_Object.GetObj_File.dChikenWeight = double.Parse(dataGridView1.Rows[6].Cells[1].Value.ToString());
                            break;
                    }
                    break;
                }
            }

            for (int idx = 0; idx < dataGridView1.Rows.Count; idx++)
            {
                dataGridView1.Rows[idx].Cells[2].Value = null;
            }


            string path = null;
            path = $"{Application.StartupPath}\\whdmd_xbj.xml";
            devJace.Files.Xml.Save(path, Cores.Core_Object.GetObj_File);

            path = $"{Application.StartupPath}\\whdmd_xos.xml";
            devJace.Files.Xml.Save(path, Cores.Core_Object.GetPos_File);

            //Console.WriteLine();

            for (int idx = 0; idx < radioButtons.Count; idx++)
            {
                if (radioButtons[idx].Checked)
                {
                    Init_Grid(idx+1);
                    break;
                }
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            for (int idx = 0; idx < dataGridView1.Rows.Count; idx++)
            {
                dataGridView1.Rows[idx].Cells[2].Value = strPreviousBuff[idx];
            }

           
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (iSeltedDataGrid != iCobotNumber)
                return;
            if (e.RowIndex == -1 && e.ColumnIndex > -1)
            {
                Rectangle r = e.CellBounds;
                r.Y += e.CellBounds.Height / 2;
                r.Height = e.CellBounds.Height / 2;
                e.PaintBackground(r, true);
                e.PaintContent(r);
                e.Handled = true;
            }
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (iSeltedDataGrid != iCobotNumber)
                return;
            DataGridView gv = (DataGridView)sender;
            Rectangle rtHeader = gv.DisplayRectangle;
            rtHeader.Height = gv.ColumnHeadersHeight / 2;
            gv.Invalidate(rtHeader);
        }

        private void dataGridView1_Paint(object sender, PaintEventArgs e)
        {
            if (iSeltedDataGrid != iCobotNumber)
                return;

            DataGridView gv = (DataGridView)sender;

            if (gv == dataGridView2)
                return;

            try
            {
                string[] strHeaders = new string[] { "리스트", "현재 값", "변경 값", "범위", "단위" };
                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;
                // Category Painting
                {

                    Rectangle r1 = gv.GetCellDisplayRectangle(1, -1, false);
                    int widthMid = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        widthMid += gv.GetCellDisplayRectangle(i + 2, -1, false).Width;
                    }
                    int width1 = gv.GetCellDisplayRectangle(1, -1, false).Width;
                    int width2 = gv.GetCellDisplayRectangle(2, -1, false).Width;
                    int width3 = gv.GetCellDisplayRectangle(3, -1, false).Width;
                    int width4 = gv.GetCellDisplayRectangle(4, -1, false).Width;
                    int width5 = gv.GetCellDisplayRectangle(5, -1, false).Width;
                    int width6 = gv.GetCellDisplayRectangle(6, -1, false).Width;
                    r1.X += 1;
                    r1.Y += 1;
                    //r1.Width = r1.Width + width1 + width2 - 2;
                    //r1.Width = r1.Width + width1 + width2 + width3 + width4 + width5 + width6 - 2;
                    r1.Width = r1.Width + widthMid - 2;
                    r1.Height = (r1.Height / 2) - 2;
                    e.Graphics.DrawRectangle(new Pen(gv.BackgroundColor), r1);
                    e.Graphics.FillRectangle(new SolidBrush(gv.ColumnHeadersDefaultCellStyle.BackColor), r1);
                    e.Graphics.DrawString(strHeaders[1], gv.ColumnHeadersDefaultCellStyle.Font, new SolidBrush(gv.ColumnHeadersDefaultCellStyle.ForeColor), r1, format);

                }
                // Projection Painting
                {
                    Rectangle r2 = gv.GetCellDisplayRectangle(7, -1, false);
                    int width = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        width += gv.GetCellDisplayRectangle(i + 8, -1, false).Width;
                    }
                    r2.X += 1;
                    r2.Y += 1;
                    r2.Width = r2.Width + width - 2;
                    r2.Height = (r2.Height / 2) - 2;
                    e.Graphics.DrawRectangle(new Pen(gv.BackgroundColor), r2);
                    e.Graphics.FillRectangle(new SolidBrush(gv.ColumnHeadersDefaultCellStyle.BackColor), r2);
                    e.Graphics.DrawString(strHeaders[2], gv.ColumnHeadersDefaultCellStyle.Font, new SolidBrush(gv.ColumnHeadersDefaultCellStyle.ForeColor), r2, format);

                }
            }
            catch
            { }

            
        }

        private void dataGridView1_Scroll(object sender, ScrollEventArgs e)
        {
            if (iSeltedDataGrid != iCobotNumber)
                return;
            DataGridView gv = (DataGridView)sender;
            Rectangle rtHeader = gv.DisplayRectangle;
            rtHeader.Height = gv.ColumnHeadersHeight / 2;
            gv.Invalidate(rtHeader);
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (comboBox1.SelectedIndex < 0)
                return;

            var joint1 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 270);
            var joint2 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 271);
            var joint3 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 272);
            var joint4 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 273);
            var joint5 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 274);
            var joint6 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 275);


            var task1 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 400);
            var task2 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 401);
            var task3 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 402);
            var task4 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 403);
            var task5 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 404);
            var task6 = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == 405);

            DataGridView dgv = dataGridView1;

            if (comboBox1.SelectedIndex <= 10)
            {
                dgv.Rows[comboBox1.SelectedIndex].Cells[7].Value = task1.strData;
                dgv.Rows[comboBox1.SelectedIndex].Cells[8].Value = task2.strData;
                dgv.Rows[comboBox1.SelectedIndex].Cells[9].Value = task3.strData;
                dgv.Rows[comboBox1.SelectedIndex].Cells[10].Value = task4.strData;
                dgv.Rows[comboBox1.SelectedIndex].Cells[11].Value = task5.strData;
                dgv.Rows[comboBox1.SelectedIndex].Cells[12].Value = task6.strData;
            }
            else
            {
                dgv.Rows[comboBox1.SelectedIndex].Cells[7].Value = joint1.strData;
                dgv.Rows[comboBox1.SelectedIndex].Cells[8].Value = joint2.strData;
                dgv.Rows[comboBox1.SelectedIndex].Cells[9].Value = joint3.strData;
                dgv.Rows[comboBox1.SelectedIndex].Cells[10].Value = joint4.strData;
                dgv.Rows[comboBox1.SelectedIndex].Cells[11].Value = joint5.strData;
                dgv.Rows[comboBox1.SelectedIndex].Cells[12].Value = joint6.strData;
            }




            comboBox1.Text = null;
        }
    }//class
}//namespace
