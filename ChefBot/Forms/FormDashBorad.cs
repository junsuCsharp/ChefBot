using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ChartDirector;

namespace Forms
{
    public partial class FormDashBorad : Form
    {
        public FormDashBorad()
        {
            InitializeComponent();

            devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
          $"{System.Reflection.MethodBase.GetCurrentMethod().Name}");
        }

        List<Size> szViewer = new List<Size>();

        string[] strYearLabels = {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sept", "Oct", "Nov", "Dec"};
        string[] strWeekLabels = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };

        List<Label> lbQuantity = new List<Label>();

        enum EChart
        { 
            Pie, Bar
        }

        enum EQuantity
        { 
            Year, Month, Weeks, Days
        }

        private void FormDashBorad_Load(object sender, EventArgs e)
        {
            const float fontDepth = 40;
            const int fontIndex = (int)Fonts.FontLibrary.ENotoSans.Normal;

            szViewer.Add(winChartViewer1.Size);
            szViewer.Add(winChartViewer2.Size);

            lbQuantity.Add(labelYear);
            lbQuantity.Add(labelMonth);
            lbQuantity.Add(labelWeeks);
            lbQuantity.Add(labelDays);

            foreach (Label lb in lbQuantity)
            { 
                lb.Font = new Font(Fonts.FontLibrary.Families[fontIndex], fontDepth);
            }

            CreateChart(winChartViewer1);
            CreateChart(winChartViewer2);

            metroComboBoxSel.SelectedIndex = 1;
        }

        void CreateChart(WinChartViewer viewer)
        {
            double[] data0 = { 100, 125, 156, 147, 87, 124, 178, 109, 140, 106, 192, 122 };
            double[] data1 = { 122, 156, 179, 211, 198, 177, 160, 220, 190, 188, 220, 270 };
            double[] data2 = { 167, 190, 213, 267, 250, 320, 212, 199, 245, 267, 240, 310 };
            string[] labels = { "# 1", "# 2", "# 3" };


            if (viewer == winChartViewer1)
            {
                int chartIndex = 0;//chart selected

                // The data for the pie chart
                double[] data = { data0.Sum(), data1.Sum(), data2.Sum() };

                // The labels for the pie chart
            

                // The colors to use for the sectors
                int[] colors = { 0x66ff66, 0xff6666, 0xffff00 };

                // Create a PieChart object of size 300 x 300 pixels. Set the background to a gradient
                // color from blue (aaccff) to sky blue (ffffff), with a grey (888888) border. Use
                // rounded corners and soft drop shadow.
                //PieChart c = new PieChart(300, 300);
                PieChart c = new PieChart(szViewer[(int)EChart.Pie].Width, szViewer[(int)EChart.Pie].Height);
                c.setBackground(c.linearGradientColor(0, 0, 0, c.getHeight() / 2, 0xaaccff, 0xffffff),
                    0x888888);
                c.setRoundedFrame();
                c.setDropShadow();

                if (chartIndex == 0)
                {
                    //============================================================
                    //    Draw a pie chart where the label is on top of the pie
                    //============================================================

                    int plotSize = 0;
                    List<int> lstPlotSize = new List<int>();
                    lstPlotSize.Add(szViewer[(int)EChart.Pie].Width);
                    lstPlotSize.Add(szViewer[(int)EChart.Pie].Height);
                    plotSize = (int)(lstPlotSize.Min() * 0.5) - 10;                    
                    // Set the center of the pie at (150, 150) and the radius to 120 pixels
                    c.setPieSize((int)(szViewer[(int)EChart.Pie].Width*0.5), (int)(szViewer[(int)EChart.Pie].Height * 0.5), plotSize);

                    // Set the label position to -40 pixels from the perimeter of the pie (-ve means
                    // label is inside the pie)
                    c.setLabelPos(-70);
                    c.setLabelStyle("", 20d);
                }
                else
                {
                    //============================================================
                    //    Draw a pie chart where the label is outside the pie
                    //============================================================

                    // Set the center of the pie at (150, 150) and the radius to 80 pixels
                    c.setPieSize(150, 150, 80);

                    // Set the sector label position to be 20 pixels from the pie. Use a join line to
                    // connect the labels to the sectors.
                    c.setLabelPos(20, Chart.LineColor);

                }

                // Set the pie data and the pie labels
                c.setData(data, labels);

                // Set the sector colors
                c.setColors2(Chart.DataColor, colors);

                // Use local gradient shading, with a 1 pixel semi-transparent black (bb000000) border
                c.setSectorStyle(Chart.LocalGradientShading, unchecked((int)0xbb000000), 1);

                // Output the chart
                viewer.Chart = c;

                //include tool tip for the chart
                viewer.ImageMap = c.getHTMLImageMap("clickable", "",
                    "title='{label}: {value} responses ({percent}%)'");

            }

            if (viewer == winChartViewer2)
            {
                //Name of demo module
                //public string getName() { return "Overlapping Bar Chart"; }
                //
                ////Number of charts produced in this demo module
                //public int getNoOfCharts() { return 1; }
                //
                ////Main code for creating chart.
                ////Note: the argument chartIndex is unused because this demo only has 1 chart.
                //public void createChart(WinChartViewer viewer, int chartIndex)

                // The data for the bar chart
              
                labels = new string[] {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sept",
                "Oct", "Nov", "Dec"};

                // Create a XYChart object of size 580 x 280 pixels
                XYChart c = new XYChart(szViewer[(int)EChart.Bar].Width, szViewer[(int)EChart.Bar].Height + 20);

                // Add a title to the chart using 14pt Arial Bold Italic font
                //c.addTitle("Product Revenue For Last 3 Years", "Arial Bold Italic", 14);

                // Set the plot area at (50, 50) and of size 500 x 200. Use two alternative background
                // colors (f8f8f8 and ffffff)
                //c.setPlotArea(50, 50, 500, 200, 0xf8f8f8, 0xffffff);
                int offset = 50;
                c.setPlotArea(offset, offset, szViewer[(int)EChart.Bar].Width - offset, szViewer[(int)EChart.Bar].Height - 70, 0xf8f8f8, 0xffffff);

                // Add a legend box at (50, 25) using horizontal layout. Use 8pt Arial as font, with
                // transparent background.
                c.addLegend(50, 25, false, "Arial", 8).setBackground(Chart.Transparent);

                // Set the x axis labels
                c.xAxis().setLabels(labels);

                // Draw the ticks between label positions (instead of at label positions)
                c.xAxis().setTickOffset(0.5);

                // Add a multi-bar layer with 3 data sets
                BarLayer layer = c.addBarLayer2(Chart.Side);
                layer.addDataSet(data0, 0xff8080, "# 1");
                layer.addDataSet(data1, 0x80ff80, "# 2");
                layer.addDataSet(data2, 0x8080ff, "# 3");

                // Set 50% overlap between bars
                layer.setOverlapRatio(0.5);

                // Add a title to the y-axis
                c.yAxis().setTitle("Revenue (USD in millions)");

                // Output the chart
                viewer.Chart = c;

                //include tool tip for the chart
                viewer.ImageMap = c.getHTMLImageMap("clickable", "",
                    "title='{xLabel} Revenue on {dataSetName}: {value} millions'");


            }
        }

        void CreateChart(WinChartViewer viewer, bool[] IsEnable)
        {
            if (viewer == winChartViewer1)
            {
                int chartIndex = 0;//chart selected

                // The data for the pie chart
                double[] data = { 45, 42, 8 };

                // The labels for the pie chart
                string[] labels = { "Agree", "Disagree", "Not Sure" };

                // The colors to use for the sectors
                int[] colors = { 0x66ff66, 0xff6666, 0xffff00 };

                // Create a PieChart object of size 300 x 300 pixels. Set the background to a gradient
                // color from blue (aaccff) to sky blue (ffffff), with a grey (888888) border. Use
                // rounded corners and soft drop shadow.
                //PieChart c = new PieChart(300, 300);
                PieChart c = new PieChart(szViewer[(int)EChart.Pie].Width, szViewer[(int)EChart.Pie].Height);
                c.setBackground(c.linearGradientColor(0, 0, 0, c.getHeight() / 2, 0xaaccff, 0xffffff),
                    0x888888);
                c.setRoundedFrame();
                c.setDropShadow();

                if (chartIndex == 0)
                {
                    //============================================================
                    //    Draw a pie chart where the label is on top of the pie
                    //============================================================

                    int plotSize = 0;
                    List<int> lstPlotSize = new List<int>();
                    lstPlotSize.Add(szViewer[(int)EChart.Pie].Width);
                    lstPlotSize.Add(szViewer[(int)EChart.Pie].Height);
                    plotSize = (int)(lstPlotSize.Min() * 0.5) - 10;
                    // Set the center of the pie at (150, 150) and the radius to 120 pixels
                    c.setPieSize((int)(szViewer[(int)EChart.Pie].Width * 0.5), (int)(szViewer[(int)EChart.Pie].Height * 0.5), plotSize);

                    // Set the label position to -40 pixels from the perimeter of the pie (-ve means
                    // label is inside the pie)
                    c.setLabelPos(-40);

                }
                else
                {
                    //============================================================
                    //    Draw a pie chart where the label is outside the pie
                    //============================================================

                    // Set the center of the pie at (150, 150) and the radius to 80 pixels
                    c.setPieSize(150, 150, 80);

                    // Set the sector label position to be 20 pixels from the pie. Use a join line to
                    // connect the labels to the sectors.
                    c.setLabelPos(20, Chart.LineColor);

                }

                // Set the pie data and the pie labels
                c.setData(data, labels);

                // Set the sector colors
                c.setColors2(Chart.DataColor, colors);

                // Use local gradient shading, with a 1 pixel semi-transparent black (bb000000) border
                c.setSectorStyle(Chart.LocalGradientShading, unchecked((int)0xbb000000), 1);

                // Output the chart
                viewer.Chart = c;

                //include tool tip for the chart
                viewer.ImageMap = c.getHTMLImageMap("clickable", "",
                    "title='{label}: {value} responses ({percent}%)'");

            }

            if (viewer == winChartViewer2)
            {
                //Name of demo module
                //public string getName() { return "Overlapping Bar Chart"; }
                //
                ////Number of charts produced in this demo module
                //public int getNoOfCharts() { return 1; }
                //
                ////Main code for creating chart.
                ////Note: the argument chartIndex is unused because this demo only has 1 chart.
                //public void createChart(WinChartViewer viewer, int chartIndex)

                // The data for the bar chart
                double[] data0 = { 100, 125, 156, 147, 87, 124, 178, 109, 140, 106, 192, 122 };
                double[] data1 = { 122, 156, 179, 211, 198, 177, 160, 220, 190, 188, 220, 270 };
                double[] data2 = { 167, 190, 213, 267, 250, 320, 212, 199, 245, 267, 240, 310 };
                string[] labels = {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sept",
                "Oct", "Nov", "Dec"};

                // Create a XYChart object of size 580 x 280 pixels
                XYChart c = new XYChart(szViewer[(int)EChart.Bar].Width, szViewer[(int)EChart.Bar].Height + 20);

                // Add a title to the chart using 14pt Arial Bold Italic font
                //c.addTitle("Product Revenue For Last 3 Years", "Arial Bold Italic", 14);

                // Set the plot area at (50, 50) and of size 500 x 200. Use two alternative background
                // colors (f8f8f8 and ffffff)
                //c.setPlotArea(50, 50, 500, 200, 0xf8f8f8, 0xffffff);
                int offset = 50;
                c.setPlotArea(offset, offset, szViewer[(int)EChart.Bar].Width - offset, szViewer[(int)EChart.Bar].Height - 70, 0xf8f8f8, 0xffffff);

                // Add a legend box at (50, 25) using horizontal layout. Use 8pt Arial as font, with
                // transparent background.
                c.addLegend(50, 25, false, "Arial", 8).setBackground(Chart.Transparent);

                // Set the x axis labels
                c.xAxis().setLabels(labels);

                // Draw the ticks between label positions (instead of at label positions)
                c.xAxis().setTickOffset(0.5);

                // Add a multi-bar layer with 3 data sets
                BarLayer layer = c.addBarLayer2(Chart.Side);
                if (IsEnable[0] == true)
                {
                    layer.addDataSet(data0, 0xff8080, "# 1");
                }
                if (IsEnable[1] == true)
                {
                    layer.addDataSet(data1, 0x80ff80, "# 2");
                }
                if (IsEnable[2] == true)
                {
                    layer.addDataSet(data2, 0x8080ff, "# 3");
                }

                // Set 50% overlap between bars
                layer.setOverlapRatio(0.5);

                // Add a title to the y-axis
                c.yAxis().setTitle("Revenue (USD in millions)");

                // Output the chart
                viewer.Chart = c;

                //include tool tip for the chart
                viewer.ImageMap = c.getHTMLImageMap("clickable", "",
                    "title='{xLabel} Revenue on {dataSetName}: {value} millions'");


            }
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            CreateChart(winChartViewer1);
            CreateChart(winChartViewer2);
        }

        private void IsEnable(object sender, EventArgs e)
        {
            bool[] IsEnable = new bool[] {metroToggle1.Checked, metroToggle2.Checked, metroToggle3.Checked };
            CreateChart(winChartViewer2, IsEnable);
        }

        private void metroComboBoxSel_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
