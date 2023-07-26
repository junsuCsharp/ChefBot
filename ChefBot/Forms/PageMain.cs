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
    public partial class PageMain : Form
    {
        List<SubPageChart> lstFormChart = new List<SubPageChart>();
        List<RadioButton> lstManuButtons = new List<RadioButton>();

        List<List<Label>> labels = new List<List<Label>>();
        List<PictureBox> pictureBoxes = new List<PictureBox>();

        List<Label> labelsHeader = new List<Label>();

        public PageMain()
        {
            InitializeComponent();
        }

        private void PageMain_Load(object sender, EventArgs e)
        {
           
                DefaultInitializeComponent();

                lstFormChart.Add(new SubPageChart(SubPageChart.Unit.Hour));
                lstFormChart.Add(new SubPageChart(SubPageChart.Unit.Day));
                lstFormChart.Add(new SubPageChart(SubPageChart.Unit.Week));
                lstFormChart.Add(new SubPageChart(SubPageChart.Unit.Month));
                //lstFormChart.Add(new SubPageChart(SubPageChart.Unit.Year));
                lstFormChart.Add(new SubPageChart(SubPageChart.Unit.Actual));
                lstFormChart.Add(new SubPageChart(SubPageChart.Unit.Maint));

                panelMain.Controls.Clear();

                foreach (SubPageChart frm in lstFormChart)
                {
                    devJace.Program.Performance(frm, false);
                    panelMain.Controls.Add(frm);
                }

                foreach (Control frm in panelMain.Controls)
                {
                    frm.Show();
                    frm.Hide();
                }

                //panelMain.Controls[(int)SubPageChart.Unit.Hour].Show();

                lstManuButtons.Add(radioButton1);
                lstManuButtons.Add(radioButton2);
                lstManuButtons.Add(radioButton3);
                lstManuButtons.Add(radioButton7);
                //lstManuButtons.Add(radioButton4);
                lstManuButtons.Add(radioButton5);
                lstManuButtons.Add(radioButton6);

                foreach (RadioButton rb in lstManuButtons)
                {
                    rb.Click += Rb_Click;
                }


                List<Label> labelBuff = null;
                labelBuff = new List<Label>();
                labelBuff.Add(labelChikenTotal);
                labelBuff.Add(labelChikenMonth);
                labelBuff.Add(labelChikenDay);
                labels.Add(labelBuff);

                labelBuff = new List<Label>();
                labelBuff.Add(labelRobotTotal);
                labelBuff.Add(labelRobotMonth);
                labelBuff.Add(labelRobotDay);
                labels.Add(labelBuff);

                labelBuff = new List<Label>();
                labelBuff.Add(labelOilTotal);
                labelBuff.Add(labelOilMonth);
                labelBuff.Add(labelOilDay);
                labels.Add(labelBuff);

                labelBuff = new List<Label>();
                labelBuff.Add(labelCookerTotal);
                labelBuff.Add(labelCookerMonth);
                labelBuff.Add(labelCookerDay);
                labels.Add(labelBuff);

                labelBuff = new List<Label>();
                labelBuff.Add(labelAirFilterTotal);
                labelBuff.Add(labelAirFilterMonth);
                labelBuff.Add(labelAirFilterDay);
                labels.Add(labelBuff);

                pictureBoxes.Add(pictureBox1);
                pictureBoxes.Add(pictureBox2);
                pictureBoxes.Add(pictureBox3);
                pictureBoxes.Add(pictureBox4);
                pictureBoxes.Add(pictureBox5);

                labelsHeader.Add(labelChiken);
                labelsHeader.Add(labelRobot);
                labelsHeader.Add(labelOil);
                labelsHeader.Add(labelCooker);
                labelsHeader.Add(labelAirFilter);

                Rb_Click(lstManuButtons[(int)SubPageChart.Unit.Hour], null);

                this.Focus();
           
            
        }

        private void Rb_Click(object sender, EventArgs e)
        {
            try
            {
                //throw new NotImplementedException();

                RadioButton rbButton = sender as RadioButton;
                for (int idx = 0; idx < (int)SubPageChart.Unit.Max; idx++)
                {
                    if (rbButton == lstManuButtons[idx])
                    {
                        panelMain.Controls[idx].Show();
                        //lstManuButtons[idx].BackColor = Color.DimGray;
                        lstManuButtons[idx].BackColor = devi.Define.colorSubButtonColor;
                        lstManuButtons[idx].ForeColor = Color.White;
                        DataBaseLoad(idx);
                    }
                    else
                    {
                        panelMain.Controls[idx].Hide();
                        lstManuButtons[idx].BackColor = Color.Transparent;
                        lstManuButtons[idx].ForeColor = Color.Black;
                    }
                }
            }
            catch { }
           
        }

        void DataBaseLoad(int idx)
        {
            Externs.Action_Loading actLoad = new Externs.Action_Loading();
            actLoad.Open();
            try
            {
                if (Cores.Core_Data.m_db.IsConnect() == false)
                    return;
             
                Common.FormMessageBox MsgBox = null;

                switch (idx)
                {
                    case 0://hour                 
                        if (Cores.Core_Data.m_HoutDB.ReadData($"ProductDB_{DateTime.Now:yyyyMMdd}", out List<Cores.Core_Data.Chicken> dat))
                        {
                            lstFormChart[idx].UpdateChart(dat);
                        }
                        //else
                        //{
                        //    DisplayMessageBox("시간별 데이터가 존재 하지 않습니다.");
                        //}
                        break;

                    case 1://day
                        if (Cores.Core_Data.m_ProductDB.ReadDayData(out int[][] dayCount))
                        {

                            lstFormChart[idx].UpdateChart(dayCount);
                        }
                        else
                        {
                            actLoad.Close();
                            MsgBox = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None, "일일 데이터가 존재 하지 않습니다.");
                            MsgBox.ShowDialog();
                        }
                        break;

                    case 2://week
                        if (Cores.Core_Data.m_ProductDB.ReadWeekData(out int[][] weekCount))
                        {
                            lstFormChart[idx].UpdateChart(weekCount);
                        }
                        else
                        {
                            actLoad.Close();
                            MsgBox = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None, "주간 데이터가 존재 하지 않습니다.");
                            MsgBox.ShowDialog();
                        }
                        break;

                    case 3://month
                        if (Cores.Core_Data.m_ProductDB.ReadMonthData(out int[][] monthCount))
                        {
                            lstFormChart[idx].UpdateChart(monthCount);
                        }
                        else
                        {
                            actLoad.Close();
                            MsgBox = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None, "월별 데이터가 존재 하지 않습니다.");
                            MsgBox.ShowDialog();
                        }
                        break;

                    case 4://mc
                        if (Cores.Core_Data.m_CobotDB.ReadData(out List<string[]> robotOperData))
                        {
                            lstFormChart[idx].UpdateChart(robotOperData);
                        }
                        else
                        {
                            actLoad.Close();
                            MsgBox = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None, "로봇 구동 데이터가 존재 하지 않습니다.");
                            MsgBox.ShowDialog();
                        }
                        break;

                    case 5://as
                        if (Cores.Core_Data.m_MaintDB.ReadData(out List<string[]> maintOperData))
                        {
                            lstFormChart[idx].UpdateChart(maintOperData);
                        }
                        else
                        {
                            actLoad.Close();
                            MsgBox = new Common.FormMessageBox(Common.FormMessageBox.EButtons.None, "유지 보수 데이터가 존재 하지 않습니다.");
                            MsgBox.ShowDialog();
                        }
                        break;

                    case 6://
                        break;
                }
                
            }
            catch
            {
               
               
            }
            finally { actLoad.Close(); }
            
        }     

        void DefaultInitializeComponent()
        {
            this.AutoScaleMode = AutoScaleMode.None;
            try
            {
                //타이틀바 헤더
                int sizFont = 15;
                labelChiken.Font = new Font(Fonts.FontLibrary.Families[0], sizFont);
                labelRobot.Font = new Font(Fonts.FontLibrary.Families[0], sizFont);
                labelOil.Font = new Font(Fonts.FontLibrary.Families[0], sizFont);
                labelCooker.Font = new Font(Fonts.FontLibrary.Families[0], sizFont);
                labelAirFilter.Font = new Font(Fonts.FontLibrary.Families[0], sizFont);

                //전체, 월별, 일별
                sizFont = 10;
                label2.Font = new Font(Fonts.FontLibrary.Families[0], sizFont);
                label3.Font = new Font(Fonts.FontLibrary.Families[0], sizFont);
                label4.Font = new Font(Fonts.FontLibrary.Families[0], sizFont);
                label5.Font = new Font(Fonts.FontLibrary.Families[0], sizFont);
                label6.Font = new Font(Fonts.FontLibrary.Families[0], sizFont);
                label7.Font = new Font(Fonts.FontLibrary.Families[0], sizFont);
                label8.Font = new Font(Fonts.FontLibrary.Families[0], sizFont);
                label9.Font = new Font(Fonts.FontLibrary.Families[0], sizFont);
                label10.Font = new Font(Fonts.FontLibrary.Families[0], sizFont);
                label11.Font = new Font(Fonts.FontLibrary.Families[0], sizFont);
                label12.Font = new Font(Fonts.FontLibrary.Families[0], sizFont);
                label13.Font = new Font(Fonts.FontLibrary.Families[0], sizFont);
                label14.Font = new Font(Fonts.FontLibrary.Families[0], sizFont);
                label15.Font = new Font(Fonts.FontLibrary.Families[0], sizFont);
                label16.Font = new Font(Fonts.FontLibrary.Families[0], sizFont);

                //count
                sizFont = 20;
                labelChikenDay.Font = new Font(Fonts.FontLibrary.Families[0], sizFont);
                labelChikenMonth.Font = new Font(Fonts.FontLibrary.Families[0], sizFont);
                labelChikenTotal.Font = new Font(Fonts.FontLibrary.Families[0], sizFont);
                labelRobotDay.Font = new Font(Fonts.FontLibrary.Families[0], sizFont);
                labelRobotMonth.Font = new Font(Fonts.FontLibrary.Families[0], sizFont);
                labelRobotTotal.Font = new Font(Fonts.FontLibrary.Families[0], sizFont);
                labelOilDay.Font = new Font(Fonts.FontLibrary.Families[0], sizFont);
                labelOilMonth.Font = new Font(Fonts.FontLibrary.Families[0], sizFont);
                labelOilTotal.Font = new Font(Fonts.FontLibrary.Families[0], sizFont);
                labelCookerDay.Font = new Font(Fonts.FontLibrary.Families[0], sizFont);
                labelCookerMonth.Font = new Font(Fonts.FontLibrary.Families[0], sizFont);
                labelCookerTotal.Font = new Font(Fonts.FontLibrary.Families[0], sizFont);
                labelAirFilterDay.Font = new Font(Fonts.FontLibrary.Families[0], sizFont);
                labelAirFilterMonth.Font = new Font(Fonts.FontLibrary.Families[0], sizFont);
                labelAirFilterTotal.Font = new Font(Fonts.FontLibrary.Families[0], sizFont);

                Color color = Color.White;
                //타이틀바 헤더            
                labelChiken.ForeColor = color;
                labelRobot.ForeColor = color;
                labelOil.ForeColor = color;
                labelCooker.ForeColor = color;
                labelAirFilter.ForeColor = color;

                //전체, 월별, 일별            
                label2.ForeColor = color;
                label3.ForeColor = color;
                label4.ForeColor = color;
                label5.ForeColor = color;
                label6.ForeColor = color;
                label7.ForeColor = color;
                label8.ForeColor = color;
                label9.ForeColor = color;
                label10.ForeColor = color;
                label11.ForeColor = color;
                label12.ForeColor = color;
                label13.ForeColor = color;
                label14.ForeColor = color;
                label15.ForeColor = color;
                label16.ForeColor = color;

                //count

                labelChikenDay.ForeColor = color;
                labelChikenMonth.ForeColor = color;
                labelChikenTotal.ForeColor = color;
                labelRobotDay.ForeColor = color;
                labelRobotMonth.ForeColor = color;
                labelRobotTotal.ForeColor = color;
                labelOilDay.ForeColor = color;
                labelOilMonth.ForeColor = color;
                labelOilTotal.ForeColor = color;
                labelCookerDay.ForeColor = color;
                labelCookerMonth.ForeColor = color;
                labelCookerTotal.ForeColor = color;
                labelAirFilterDay.ForeColor = color;
                labelAirFilterMonth.ForeColor = color;
                labelAirFilterTotal.ForeColor = color;

                label36.ForeColor = color;
                label37.ForeColor = color;
                label38.ForeColor = color;
                label39.ForeColor = color;
                label40.ForeColor = color;
                label41.ForeColor = color;
                label42.ForeColor = color;
                label43.ForeColor = color;
                label44.ForeColor = color;
                label45.ForeColor = color;
                label46.ForeColor = color;
                label47.ForeColor = color;
                label48.ForeColor = color;
                label49.ForeColor = color;
                label50.ForeColor = color;

                sizFont = 12;
                radioButton1.Font = new Font(Fonts.FontLibrary.Families[0], sizFont);
                radioButton2.Font = new Font(Fonts.FontLibrary.Families[0], sizFont);
                radioButton3.Font = new Font(Fonts.FontLibrary.Families[0], sizFont);
                //radioButton4.Font = new Font(Fonts.FontLibrary.Families[0], sizFont);
                radioButton5.Font = new Font(Fonts.FontLibrary.Families[0], sizFont);
                radioButton6.Font = new Font(Fonts.FontLibrary.Families[0], sizFont);
                radioButton7.Font = new Font(Fonts.FontLibrary.Families[0], sizFont);
            }
            catch { }
            

        }

        public void CurrentUpdate()
        {

            DefaultInitializeComponent();

            try
            {
                for (int row = 0; row < Cores.Core_Object.GetCounters.Count; row++)
                {
                    //Console.WriteLine(String.Format("{0:#,0}", num));
                    if (row == 0)
                    {
                        labels[row][0].Text = String.Format("{0:#,0}", Cores.Core_Object.GetCounters[row].iTotal);
                        labels[row][1].Text = String.Format("{0:#,0}", Cores.Core_Object.GetCounters[row].iMonth);
                        labels[row][2].Text = String.Format("{0:#,0}", Cores.Core_Object.GetCounters[row].iDays);
                    }
                    else
                    {
                        //labels[row][0].Text = Cores.Core_Object.GetCounters[row].iTotal.ToString("0.0000");
                        //labels[row][1].Text = Cores.Core_Object.GetCounters[row].iMonth.ToString("0.0000");
                        //labels[row][2].Text = Cores.Core_Object.GetCounters[row].iDays.ToString("0.0000");

                        //TimeSpan tH = TimeSpan.FromSeconds(Cores.Core_Object.GetCounters[row].iTotal);
                        labels[row][0].Text = $"{Cores.Core_Object.GetCounters[row].tsTotal.TotalHours:0.0}";

                        //TimeSpan tM = TimeSpan.FromMilliseconds(Cores.Core_Object.GetCounters[row].iMonth);
                        labels[row][1].Text = $"{Cores.Core_Object.GetCounters[row].tsMonth.TotalHours:0.0000}";

                        //TimeSpan ts = TimeSpan.FromMilliseconds(Cores.Core_Object.GetCounters[row].iDays);
                        //labels[row][2].Text = $"{Cores.Core_Object.GetCounters[row].tsDay:00:00:00}";

                        string temp = Cores.Core_Object.GetCounters[row].tsDay.ToString();
                        temp = temp.Substring(0, temp.LastIndexOf('.'));
                        labels[row][2].Text = $"{temp}";
                    }

                }

                //lstFormChart[4].UpdateChart();
                //lstFormChart[5].UpdateChart();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            try
            {
                PictureBox pic = sender as PictureBox;
                int index = -1;
                for (int idx = 0; idx < pictureBoxes.Count; idx++)
                {
                    if (pictureBoxes[idx] == pic)
                    {
                        index = idx;
                    }
                }

                if (index < 0)
                    return;

                Common.FormMessageBox msg;
                msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.Normal, $"{labelsHeader[index].Text} 데이터 초기화 하시겠습니까?");
                if (msg.ShowDialog() == DialogResult.OK)
                {
                    if (index == 1)
                    { 
                        Cores.Core_Data.m_db.DropTable("MechineDB");
                    }

                    Cores.Core_Object.GetCounters[index].currTime = DateTime.Now;
                    Cores.Core_Object.GetCounters[index].DayTime = Cores.Core_Object.GetCounters[index].currTime.AddDays(1);
                    Cores.Core_Object.GetCounters[index].MonthTime = Cores.Core_Object.GetCounters[index].currTime.AddMonths(1);

                    Cores.Core_Object.GetCounters[index].ProduceTime = Cores.Core_Object.GetCounters[index].currTime;
                    Cores.Core_Object.GetCounters[index].ProduceM_Time = Cores.Core_Object.GetCounters[index].currTime;
                    Cores.Core_Object.GetCounters[index].ProduceD_Time = Cores.Core_Object.GetCounters[index].currTime;

                    Cores.Core_Object.GetCounters[index].iTotal = 0;
                    Cores.Core_Object.GetCounters[index].iMonth = 0;
                    Cores.Core_Object.GetCounters[index].iDays = 0;

                    Cores.Core_Object.GetCounters[index].tsTotal = new TimeSpan();
                    Cores.Core_Object.GetCounters[index].tsMonth = new TimeSpan();
                    Cores.Core_Object.GetCounters[index].tsDay = new TimeSpan();

                    //Core_Object.GetCounters[(int)Core_Data.MainType.Robot].iMonth

                    string path = null;
                    path = $"{Application.StartupPath}\\whdmd_cnt.xml";
                    devJace.Files.Xml.Save(path, Cores.Core_Object.GetCounters);
                }
            }
            catch
            { }
           
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Common.FormMessageBox msg;
            msg = new Common.FormMessageBox(Common.FormMessageBox.EButtons.Normal, $"치킨 조리 수량 데이터 초기화 하시겠습니까?");
            if (msg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    List<string> tbName = new List<string>();
                    Cores.Core_Data.m_db.SearchDBTable("ProductDB", ref tbName);

                    if (tbName.Count != 0)
                    {
                        for (int idx = 0; idx < tbName.Count; idx++)
                        {
                            Cores.Core_Data.m_db.DropTable(tbName[idx]);
                        }

                    }

                    if (Cores.Core_Data.m_ProductDB.ReadTotalCount(out int totalCount))
                    {
                        Cores.Core_Object.GetCounters[(int)Cores.Core_Data.MainType.Chiken].iTotal = totalCount;
                    }
                    else
                    {
                        Cores.Core_Object.GetCounters[(int)Cores.Core_Data.MainType.Chiken].iTotal = 0;
                    }

                    if (Cores.Core_Data.m_ProductDB.ReadMonthCount(out int monthCount))
                    {
                        Cores.Core_Object.GetCounters[(int)Cores.Core_Data.MainType.Chiken].iMonth = monthCount;
                    }
                    else
                    {
                        Cores.Core_Object.GetCounters[(int)Cores.Core_Data.MainType.Chiken].iMonth = 0;
                    }

                    if (Cores.Core_Data.m_ProductDB.ReadDayCount(out int dayCount))
                    {
                        Cores.Core_Object.GetCounters[(int)Cores.Core_Data.MainType.Chiken].iDays = dayCount;
                    }
                    else
                    {
                        Cores.Core_Object.GetCounters[(int)Cores.Core_Data.MainType.Chiken].iDays = 0;
                    }

                    string path = null;
                    path = $"{Application.StartupPath}\\whdmd_cnt.xml";
                    devJace.Files.Xml.Save(path, Cores.Core_Object.GetCounters);
                }
                catch
                { }

            }
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {

        }

        #region MyRegion
        //개별 초기화
        private void labelRobotTotal_Click(object sender, EventArgs e)
        {

        }

        private void labelRobotMonth_Click(object sender, EventArgs e)
        {

        }

        private void labelRobotDay_Click(object sender, EventArgs e)
        {

        }

        private void labelOilTotal_Click(object sender, EventArgs e)
        {

        }

        private void labelOilMonth_Click(object sender, EventArgs e)
        {

        }

        private void labelOilDay_Click(object sender, EventArgs e)
        {

        }

        private void labelCookerTotal_Click(object sender, EventArgs e)
        {

        }

        private void labelCookerMonth_Click(object sender, EventArgs e)
        {

        }

        private void labelCookerDay_Click(object sender, EventArgs e)
        {

        }

        private void labelAirFilterTotal_Click(object sender, EventArgs e)
        {

        }

        private void labelAirFilterMonth_Click(object sender, EventArgs e)
        {

        }

        private void labelAirFilterDay_Click(object sender, EventArgs e)
        {

        } 
        #endregion
    }
}

