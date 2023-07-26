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
    using ChartDirector;
    public partial class SubPageChart : Form
    {
        public SubPageChart()
        {
            InitializeComponent();
        }

        public SubPageChart(Unit id)
        {
            InitializeComponent();

            myUnitId = id;
        }

        public enum Unit
        { 
            Hour, Day, Week, Month, Actual, Maint, Max
        }

        enum EChart
        {
            Bar, Line
        }

        private Unit myUnitId;
        private int iCookerMax = 6;
        private Random random = new Random();
        private DateTime dtStartYear = new DateTime(2023,02,02);

        List<Size> szViewer = new List<Size>();

        string[] strMonthLabels = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sept", "Oct", "Nov", "Dec" };
        string[] strWeekLabels = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };

        private string[] strCookerLabels = new string[] { "01", "02", "03", "04", "05", "06" };

        Color colorHigh = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(56)))), ((int)(((byte)(100)))));
        Color colorLow = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(102)))), ((int)(((byte)(255)))));

        int iColorHigh = 32 << 16 | 56 << 8 | 100;
        int iColorLow = 51 << 16 | 102 << 8 | 255;


        int[] iColorMaint = new int[] {
                                        195 << 16 | 195 << 8 | 195,//gray
                                        185 << 16 | 122 << 8 | 087,//brown
                                        255 << 16 | 174 << 8 | 201,//pink
                                        255 << 16 | 201 << 8 | 014,//yellow
                                        153 << 16 | 217 << 8 | 234,//cyon
                                        112 << 16 | 146 << 8 | 190,//navy
                                        200 << 16 | 191 << 8 | 231,};//puple

        private void SubPageChart_Load(object sender, EventArgs e)
        {
            this.AutoScaleMode = AutoScaleMode.None;


            szViewer.Add(winChartViewer1.Size);
            //szViewer.Add(winChartViewer2.Size);

            ///Cooker label instans
            strCookerLabels = new string[iCookerMax];
            for (int idx = 0; idx < strCookerLabels.Length; idx++)
            {
                strCookerLabels[idx] = $"{idx+1:00}";
            }

            CreateChart(winChartViewer1);
            //CreateChart(winChartViewer2);
        }

        void CreateChart(WinChartViewer viewer)
        {
            //sample data
            double[] data0 = { 100, 125, 156, 147, 87, 124, 178, 109, 140, 106, 192, 122 };
            double[] data1 = { 122, 156, 179, 211, 198, 177, 160, 220, 190, 188, 220, 270 };
            //double[] data2 = { 167, 190, 213, 267, 250, 320, 212, 199, 245, 267, 240, 310 };
            string[] labels = Chart_X_String(myUnitId);


            // The data for the bar chart
            //double[] data0 = { 100, 115, 165, 107, 67 };
            //double[] data1 = { 85, 106, 129, 161, 123 };
            //double[] data2 = { 67, 87, 86, 167, 157 };

            data0 = Chart_SampleData(myUnitId);
            data1 = Chart_SampleData(myUnitId);

            // The labels for the bar chart
            //string[] labels = { "Mon", "Tue", "Wed", "Thu", "Fri" };

            // Create a XYChart object of size 600 x 360 pixels
            XYChart c = new XYChart(szViewer[(int)EChart.Bar].Width, szViewer[(int)EChart.Bar].Height + 20);

            // Set default text color to dark grey (0x333333)
            c.setColor(Chart.TextColor, 0x333333);

            // Set the plotarea at (70, 20) and of size 400 x 300 pixels, with transparent
            // background and border and light grey (0xcccccc) horizontal grid lines
            //c.setPlotArea(70, 20, 400, 300, Chart.Transparent, -1, Chart.Transparent, 0xcccccc);
            int offsetWidth = 70;
            int offsetHeight = 30;
            c.setPlotArea(offsetWidth, offsetHeight, szViewer[(int)EChart.Bar].Width - offsetWidth - 10, szViewer[(int)EChart.Bar].Height - 60, 0xf8f8f8, 0xffffff);

            // Add a legend box at (480, 20) using vertical layout and 12pt Arial font. Set
            // background and border to transparent and key icon border to the same as the fill
            // color.
            LegendBox b = c.addLegend(100, 5, false, "Arial", 8);
            b.setBackground(Chart.Transparent, Chart.Transparent);
            b.setKeyBorder(Chart.SameAsMainColor);

            // Set the x and y axis stems to transparent and the label font to 12pt Arial
            c.xAxis().setColors(Chart.Transparent);
            c.yAxis().setColors(Chart.Transparent);
            c.xAxis().setLabelStyle("Arial", 12);
            c.yAxis().setLabelStyle("Arial", 12);

            // Add a stacked bar layer
            BarLayer layer = c.addBarLayer2(Chart.Stack);

            // Add the three data sets to the bar layer
            //layer.addDataSet(data0, 0xaaccee, "Server # 1");
            layer.addDataSet(data0, iColorLow, "순살");
            layer.addDataSet(data1, iColorHigh, "뼈");
            //layer.addDataSet(data2, 0xeeaa66, "Server # 3");

            // Set the bar border to transparent
            layer.setBorderColor(Chart.Transparent);

            // Enable labelling for the entire bar and use 12pt Arial font
            layer.setAggregateLabelStyle("Arial", 12);

            // Enable labelling for the bar segments and use 12pt Arial font with center alignment
            layer.setDataLabelStyle("Arial", 10).setAlignment(Chart.Center);

            // For a vertical stacked bar with positive data, the first data set is at the bottom.
            // For the legend box, by default, the first entry at the top. We can reverse the legend
            // order to make the legend box consistent with the stacked bar.
            layer.setLegendOrder(Chart.ReverseLegend);

            // Set the labels on the x axis.
            c.xAxis().setLabels(labels);

            //c.xAxis().setTickDensity(5);

            // For the automatic y-axis labels, set the minimum spacing to 40 pixels.
            c.yAxis().setTickDensity(40);

            // Add a title to the y axis using dark grey (0x555555) 14pt Arial Bold font
            c.yAxis().setTitle("수량 [ 횟수 ]", "Arial Bold", 11, 0x555555);

            // Output the chart
            viewer.Chart = c;

            //include tool tip for the chart
            viewer.ImageMap = c.getHTMLImageMap("clickable", "", "title='{dataSetName} {xLabel}: {value}'");


        }

        void CreateChart(WinChartViewer viewer, double[] dataDay_Fried, double[] dataDay_French)
        {
            //sample data
            double[] data0 = { 100, 125, 156, 147, 87, 124, 178, 109, 140, 106, 192, 122 };
            double[] data1 = { 122, 156, 179, 211, 198, 177, 160, 220, 190, 188, 220, 270 };
            //double[] data2 = { 167, 190, 213, 267, 250, 320, 212, 199, 245, 267, 240, 310 };
            string[] labels = Chart_X_String(myUnitId);


            // The data for the bar chart
            //double[] data0 = { 100, 115, 165, 107, 67 };
            //double[] data1 = { 85, 106, 129, 161, 123 };
            //double[] data2 = { 67, 87, 86, 167, 157 };                

            data0 = dataDay_Fried;
            data1 = dataDay_French;

            //data0 = Chart_SampleData(myUnitId, dateTimePicker1);
            //data1 = Chart_SampleData(myUnitId, dateTimePicker1);

            // The labels for the bar chart
            //string[] labels = { "Mon", "Tue", "Wed", "Thu", "Fri" };

            // Create a XYChart object of size 600 x 360 pixels
            XYChart c = new XYChart(szViewer[(int)EChart.Bar].Width, szViewer[(int)EChart.Bar].Height + 20);

            // Set default text color to dark grey (0x333333)
            c.setColor(Chart.TextColor, 0x333333);

            // Set the plotarea at (70, 20) and of size 400 x 300 pixels, with transparent
            // background and border and light grey (0xcccccc) horizontal grid lines
            //c.setPlotArea(70, 20, 400, 300, Chart.Transparent, -1, Chart.Transparent, 0xcccccc);
            int offsetWidth = 70;
            int offsetHeight = 30;
            c.setPlotArea(offsetWidth, offsetHeight, szViewer[(int)EChart.Bar].Width - offsetWidth - 10, szViewer[(int)EChart.Bar].Height - 60, 0xf8f8f8, 0xffffff);

            // Add a legend box at (480, 20) using vertical layout and 12pt Arial font. Set
            // background and border to transparent and key icon border to the same as the fill
            // color.
            LegendBox b = c.addLegend(100, 5, false, "Arial", 8);
            b.setBackground(Chart.Transparent, Chart.Transparent);
            b.setKeyBorder(Chart.SameAsMainColor);

            // Set the x and y axis stems to transparent and the label font to 12pt Arial
            c.xAxis().setColors(Chart.Transparent);
            c.yAxis().setColors(Chart.Transparent);
            c.xAxis().setLabelStyle("Arial", 12);
            c.yAxis().setLabelStyle("Arial", 12);

            // Add a stacked bar layer
            BarLayer layer = c.addBarLayer2(Chart.Stack);

            // Add the three data sets to the bar layer
            //layer.addDataSet(data0, 0xaaccee, "Server # 1");
            layer.addDataSet(data0, iColorLow, "순살");
            layer.addDataSet(data1, iColorHigh, "뼈");
            //layer.addDataSet(data2, 0xeeaa66, "Server # 3");

            // Set the bar border to transparent
            layer.setBorderColor(Chart.Transparent);

            // Enable labelling for the entire bar and use 12pt Arial font
            layer.setAggregateLabelStyle("Arial", 12);

            // Enable labelling for the bar segments and use 12pt Arial font with center alignment
            layer.setDataLabelStyle("Arial", 10).setAlignment(Chart.Center);

            // For a vertical stacked bar with positive data, the first data set is at the bottom.
            // For the legend box, by default, the first entry at the top. We can reverse the legend
            // order to make the legend box consistent with the stacked bar.
            layer.setLegendOrder(Chart.ReverseLegend);

            // Set the labels on the x axis.
            c.xAxis().setLabels(labels);

            //c.xAxis().setTickDensity(5);

            // For the automatic y-axis labels, set the minimum spacing to 40 pixels.
            c.yAxis().setTickDensity(40);

            // Add a title to the y axis using dark grey (0x555555) 14pt Arial Bold font
            c.yAxis().setTitle("수량 [ 횟수 ]", "Arial Bold", 11, 0x555555);

            // Output the chart
            viewer.Chart = c;

            //include tool tip for the chart
            viewer.ImageMap = c.getHTMLImageMap("clickable", "", "title='{dataSetName} {xLabel}: {value}'");


        }

        void CreateChart(WinChartViewer viewer, int[] col, double[] dataDay_Fried, double[] dataDay_French)
        {
            //sample data
            double[] data0 = { 100, 125, 156, 147, 87, 124, 178, 109, 140, 106, 192, 122 };
            double[] data1 = { 122, 156, 179, 211, 198, 177, 160, 220, 190, 188, 220, 270 };
            //double[] data2 = { 167, 190, 213, 267, 250, 320, 212, 199, 245, 267, 240, 310 };
            //string[] labels = Chart_X_String(myUnitId);
            string[] labels = new string[col.Length];

            for (int i = 0; i < col.Length; i++)
            {
                labels[i] = col[i].ToString("00");
            }

            // The data for the bar chart
            //double[] data0 = { 100, 115, 165, 107, 67 };
            //double[] data1 = { 85, 106, 129, 161, 123 };
            //double[] data2 = { 67, 87, 86, 167, 157 };                

            data0 = dataDay_Fried;
            data1 = dataDay_French;

            //data0 = Chart_SampleData(myUnitId, dateTimePicker1);
            //data1 = Chart_SampleData(myUnitId, dateTimePicker1);

            // The labels for the bar chart
            //string[] labels = { "Mon", "Tue", "Wed", "Thu", "Fri" };

            // Create a XYChart object of size 600 x 360 pixels
            XYChart c = new XYChart(szViewer[(int)EChart.Bar].Width, szViewer[(int)EChart.Bar].Height + 20);

            // Set default text color to dark grey (0x333333)
            c.setColor(Chart.TextColor, 0x333333);

            // Set the plotarea at (70, 20) and of size 400 x 300 pixels, with transparent
            // background and border and light grey (0xcccccc) horizontal grid lines
            //c.setPlotArea(70, 20, 400, 300, Chart.Transparent, -1, Chart.Transparent, 0xcccccc);
            int offsetWidth = 70;
            int offsetHeight = 30;
            c.setPlotArea(offsetWidth, offsetHeight, szViewer[(int)EChart.Bar].Width - offsetWidth - 10, szViewer[(int)EChart.Bar].Height - 60, 0xf8f8f8, 0xffffff);

            // Add a legend box at (480, 20) using vertical layout and 12pt Arial font. Set
            // background and border to transparent and key icon border to the same as the fill
            // color.
            LegendBox b = c.addLegend(100, 5, false, "Arial", 8);
            b.setBackground(Chart.Transparent, Chart.Transparent);
            b.setKeyBorder(Chart.SameAsMainColor);

            // Set the x and y axis stems to transparent and the label font to 12pt Arial
            c.xAxis().setColors(Chart.Transparent);
            c.yAxis().setColors(Chart.Transparent);
            c.xAxis().setLabelStyle("Arial", 12);
            c.yAxis().setLabelStyle("Arial", 12);

            // Add a stacked bar layer
            BarLayer layer = c.addBarLayer2(Chart.Stack);

            // Add the three data sets to the bar layer
            //layer.addDataSet(data0, 0xaaccee, "Server # 1");
            layer.addDataSet(data0, iColorLow, "순살");
            layer.addDataSet(data1, iColorHigh, "뼈");
            //layer.addDataSet(data2, 0xeeaa66, "Server # 3");

            // Set the bar border to transparent
            layer.setBorderColor(Chart.Transparent);

            // Enable labelling for the entire bar and use 12pt Arial font
            layer.setAggregateLabelStyle("Arial", 12);

            // Enable labelling for the bar segments and use 12pt Arial font with center alignment
            layer.setDataLabelStyle("Arial", 10).setAlignment(Chart.Center);

            // For a vertical stacked bar with positive data, the first data set is at the bottom.
            // For the legend box, by default, the first entry at the top. We can reverse the legend
            // order to make the legend box consistent with the stacked bar.
            layer.setLegendOrder(Chart.ReverseLegend);

            // Set the labels on the x axis.
            c.xAxis().setLabels(labels);

            //c.xAxis().setTickDensity(5);

            // For the automatic y-axis labels, set the minimum spacing to 40 pixels.
            c.yAxis().setTickDensity(40);

            // Add a title to the y axis using dark grey (0x555555) 14pt Arial Bold font
            c.yAxis().setTitle("수량 [ 횟수 ]", "Arial Bold", 11, 0x555555);

            // Output the chart
            viewer.Chart = c;

            //include tool tip for the chart
            viewer.ImageMap = c.getHTMLImageMap("clickable", "", "title='{dataSetName} {xLabel}: {value}'");


        }

        void CreateChart(WinChartViewer viewer, List<int> days, List<int> oprs)
        {
            //sample data
            double[] data0 = { 100, 125, 156, 147, 87, 124, 178, 109, 140, 106, 192, 122 };
            double[] data1 = { 122, 156, 179, 211, 198, 177, 160, 220, 190, 188, 220, 270 };
            //double[] data2 = { 167, 190, 213, 267, 250, 320, 212, 199, 245, 267, 240, 310 };
            string[] labels = Chart_X_String(myUnitId);

            labels = new string[days.Count];
            for (int i = 0; i < labels.Length; i++)
            {
                labels[i] = days[i].ToString("00");
            }

            data0 = new double[oprs.Count];
            for (int i = 0; i < data0.Length; i++)
            {
                data0[i] = oprs[i];
            }

            // The data for the bar chart
            //double[] data0 = { 100, 115, 165, 107, 67 };
            //double[] data1 = { 85, 106, 129, 161, 123 };
            //double[] data2 = { 67, 87, 86, 167, 157 };

            //data0 = Chart_SampleData(myUnitId);
            //data1 = Chart_SampleData(myUnitId);

            // The labels for the bar chart
            //string[] labels = { "Mon", "Tue", "Wed", "Thu", "Fri" };

            // Create a XYChart object of size 600 x 360 pixels
            XYChart c = new XYChart(szViewer[(int)EChart.Bar].Width, szViewer[(int)EChart.Bar].Height + 20);

            // Set default text color to dark grey (0x333333)
            c.setColor(Chart.TextColor, 0x333333);

            // Set the plotarea at (70, 20) and of size 400 x 300 pixels, with transparent
            // background and border and light grey (0xcccccc) horizontal grid lines
            //c.setPlotArea(70, 20, 400, 300, Chart.Transparent, -1, Chart.Transparent, 0xcccccc);
            int offsetWidth = 70;
            int offsetHeight = 30;
            c.setPlotArea(offsetWidth, offsetHeight, szViewer[(int)EChart.Bar].Width - offsetWidth - 10, szViewer[(int)EChart.Bar].Height - 60, 0xf8f8f8, 0xffffff);

            // Add a legend box at (480, 20) using vertical layout and 12pt Arial font. Set
            // background and border to transparent and key icon border to the same as the fill
            // color.
            //LegendBox b = c.addLegend(100, 5, false, "Arial", 8);
            //b.setBackground(Chart.Transparent, Chart.Transparent);
            //b.setKeyBorder(Chart.SameAsMainColor);

            // Set the x and y axis stems to transparent and the label font to 12pt Arial
            c.xAxis().setColors(Chart.Transparent);
            c.yAxis().setColors(Chart.Transparent);
            c.xAxis().setLabelStyle("Arial", 12);
            c.yAxis().setLabelStyle("Arial", 12);

            // Add a stacked bar layer
            BarLayer layer = c.addBarLayer2(Chart.Stack);

            // Add the three data sets to the bar layer
            //layer.addDataSet(data0, 0xaaccee, "Server # 1");
            layer.addDataSet(data0, iColorLow, $"Data");
            //layer.addDataSet(data1, iColorHigh, "뼈");
            //layer.addDataSet(data2, 0xeeaa66, "Server # 3");

            // Set the bar border to transparent
            layer.setBorderColor(Chart.Transparent);

            // Enable labelling for the entire bar and use 12pt Arial font
            layer.setAggregateLabelStyle("Arial", 12);

            // Enable labelling for the bar segments and use 12pt Arial font with center alignment
            layer.setDataLabelStyle("Arial", 10).setAlignment(Chart.Center);

            // For a vertical stacked bar with positive data, the first data set is at the bottom.
            // For the legend box, by default, the first entry at the top. We can reverse the legend
            // order to make the legend box consistent with the stacked bar.
            layer.setLegendOrder(Chart.ReverseLegend);

            // Set the labels on the x axis.
            c.xAxis().setLabels(labels);

            //c.xAxis().setTickDensity(5);

            // For the automatic y-axis labels, set the minimum spacing to 40 pixels.
            c.yAxis().setTickDensity(40);

            // Add a title to the y axis using dark grey (0x555555) 14pt Arial Bold font
            c.yAxis().setTitle("시간 [ 주기 ]", "Arial Bold", 11, 0x555555);

            // Output the chart
            viewer.Chart = c;

            //include tool tip for the chart
            viewer.ImageMap = c.getHTMLImageMap("clickable", "", "title='{dataSetName} {xLabel}: {value}'");


        }

        void CreateChart(WinChartViewer viewer, List<int> days, List<List<int>> oprs)
        {
            //sample data
            double[] data0 = { 100, 125, 156, 147, 87, 124, 178, 109, 140, 106, 192, 122 };
            double[] data1 = { 122, 156, 179, 211, 198, 177, 160, 220, 190, 188, 220, 270 };
            double[] data2 = { 167, 190, 213, 267, 250, 320, 212, 199, 245, 267, 240, 310 };
            double[] data3 = { 167, 190, 213, 267, 250, 320, 212, 199, 245, 267, 240, 310 };
            string[] labels = Chart_X_String(myUnitId);

            labels = new string[days.Count];
            for (int i = 0; i < labels.Length; i++)
            {
                labels[i] = days[i].ToString("00");
            }

            data0 = new double[days.Count];
            data1 = new double[days.Count];
            data2 = new double[days.Count];
            data3 = new double[days.Count];
            for (int i = 0; i < data0.Length; i++)
            {
                data0[i] = oprs[1][i];
                data1[i] = oprs[2][i];
                data2[i] = oprs[3][i];
                data3[i] = oprs[4][i];
            }

            // The data for the bar chart
            //double[] data0 = { 100, 115, 165, 107, 67 };
            //double[] data1 = { 85, 106, 129, 161, 123 };
            //double[] data2 = { 67, 87, 86, 167, 157 };

            //data0 = Chart_SampleData(myUnitId);
            //data1 = Chart_SampleData(myUnitId);

            // The labels for the bar chart
            //string[] labels = { "Mon", "Tue", "Wed", "Thu", "Fri" };

            // Create a XYChart object of size 600 x 360 pixels
            XYChart c = new XYChart(szViewer[(int)EChart.Bar].Width, szViewer[(int)EChart.Bar].Height + 20);

            // Set default text color to dark grey (0x333333)
            c.setColor(Chart.TextColor, 0x333333);

            // Set the plotarea at (70, 20) and of size 400 x 300 pixels, with transparent
            // background and border and light grey (0xcccccc) horizontal grid lines
            //c.setPlotArea(70, 20, 400, 300, Chart.Transparent, -1, Chart.Transparent, 0xcccccc);
            int offsetWidth = 70;
            int offsetHeight = 30;
            c.setPlotArea(offsetWidth, offsetHeight, szViewer[(int)EChart.Bar].Width - offsetWidth - 10, szViewer[(int)EChart.Bar].Height - 60, 0xf8f8f8, 0xffffff);

            // Add a legend box at (480, 20) using vertical layout and 12pt Arial font. Set
            // background and border to transparent and key icon border to the same as the fill
            // color.
            //LegendBox b = c.addLegend(100, 5, false, "Arial", 8);
            //b.setBackground(Chart.Transparent, Chart.Transparent);
            //b.setKeyBorder(Chart.SameAsMainColor);

            // Set the x and y axis stems to transparent and the label font to 12pt Arial
            c.xAxis().setColors(Chart.Transparent);
            c.yAxis().setColors(Chart.Transparent);
            c.xAxis().setLabelStyle("Arial", 12);
            c.yAxis().setLabelStyle("Arial", 12);

            // Add a stacked bar layer
            BarLayer layer = c.addBarLayer2(Chart.Stack);

            // Add the three data sets to the bar layer
            //layer.addDataSet(data0, 0xaaccee, "Server # 1");
            layer.addDataSet(data0, iColorLow, $"data0");
            layer.addDataSet(data1, iColorLow, $"data1");
            layer.addDataSet(data2, iColorLow, $"data2");
            layer.addDataSet(data3, iColorLow, $"data3");
            //layer.addDataSet(data1, iColorHigh, "뼈");
            //layer.addDataSet(data2, 0xeeaa66, "Server # 3");

            // Set the bar border to transparent
            layer.setBorderColor(Chart.Transparent);

            // Enable labelling for the entire bar and use 12pt Arial font
            layer.setAggregateLabelStyle("Arial", 12);

            // Enable labelling for the bar segments and use 12pt Arial font with center alignment
            layer.setDataLabelStyle("Arial", 10).setAlignment(Chart.Center);

            // For a vertical stacked bar with positive data, the first data set is at the bottom.
            // For the legend box, by default, the first entry at the top. We can reverse the legend
            // order to make the legend box consistent with the stacked bar.
            layer.setLegendOrder(Chart.ReverseLegend);

            // Set the labels on the x axis.
            c.xAxis().setLabels(labels);

            //c.xAxis().setTickDensity(5);

            // For the automatic y-axis labels, set the minimum spacing to 40 pixels.
            c.yAxis().setTickDensity(40);

            // Add a title to the y axis using dark grey (0x555555) 14pt Arial Bold font
            c.yAxis().setTitle("시간 [ 주기 ]", "Arial Bold", 11, 0x555555);

            // Output the chart
            viewer.Chart = c;

            //include tool tip for the chart
            viewer.ImageMap = c.getHTMLImageMap("clickable", "", "title='{dataSetName} {xLabel}: {value}'");


        }

        void CreateChart(WinChartViewer viewer, List<string[]> dat)
        {
            //sample data
            double[] data0 = { 100, 125, 156, 147, 87, 124, 178, 109, 140, 106, 192, 122 };
            double[] data1 = { 122, 156, 179, 211, 198, 177, 160, 220, 190, 188, 220, 270 };
            double[] data2 = { 167, 190, 213, 267, 250, 320, 212, 199, 245, 267, 240, 310 };
            double[] data3 = { 167, 190, 213, 267, 250, 320, 212, 199, 245, 267, 240, 310 };
            double[] data4 = { 167, 190, 213, 267, 250, 320, 212, 199, 245, 267, 240, 310 };
            double[] data5 = { 167, 190, 213, 267, 250, 320, 212, 199, 245, 267, 240, 310 };
            double[] data6 = { 167, 190, 213, 267, 250, 320, 212, 199, 245, 267, 240, 310 };
            string[] labels = Chart_X_String(myUnitId);

            if (dat.Count < 30)
            {
                labels = new string[30];
                data0 = new double[30];
                data1 = new double[30];
                data2 = new double[30];
                data3 = new double[30];
                data4 = new double[30];
                data5 = new double[30];
                data6 = new double[30];
            }       
            else
            {
                labels = new string[dat.Count];
                data0 = new double[dat.Count];
            }

            if (myUnitId == Unit.Actual)
            {
                for (int i = 0; i < dat.Count; i++)
                {
                    if (i < dat.Count)
                    {
                        string dateBuff = dat[i][0];

                        DateTime dtBuff = DateTime.Parse(dateBuff);
                        labels[i] = $"{dtBuff:yyyy}\r\n{dtBuff:MM/dd}";

                        TimeSpan ts = TimeSpan.FromSeconds(double.Parse(dat[i][1]));
                        data0[i] = Math.Truncate(ts.TotalHours);
                    }
                    else
                    {
                        labels[i] = "0";
                        data0[i] = 0;
                    }
                }
            }
            else if(myUnitId == Unit.Maint)
            {
                for (int i = 0; i < dat.Count; i++)
                {
                    if (i < dat.Count)
                    {
                        string dateBuff = dat[i][0];
                        DateTime dtBuff = DateTime.Parse(dateBuff);
                        labels[i] = $"{dtBuff:yyyy}\r\n{dtBuff:MM/dd}";

                        data0[i] = (double.Parse(dat[i][1]));
                        data1[i] = (double.Parse(dat[i][2]));
                        data2[i] = (double.Parse(dat[i][3]));
                        data3[i] = (double.Parse(dat[i][4]));
                        data4[i] = (double.Parse(dat[i][5]));
                        data5[i] = (double.Parse(dat[i][6]));
                        data6[i] = (double.Parse(dat[i][7]));

                    }
                    else
                    {
                        labels[i] = "0";
                        data0[i] = 0;
                        data1[i] = 0;
                        data2[i] = 0;
                        data3[i] = 0;
                        data4[i] = 0;
                        data5[i] = 0;
                        data6[i] = 0;
                    }
                }
            }
            

            
            //data1 = new double[days.Count];
            //data2 = new double[days.Count];
            //data3 = new double[days.Count];
            //for (int i = 0; i < data0.Length; i++)
            //{
            //    data0[i] = oprs[1][i];
            //    data1[i] = oprs[2][i];
            //    data2[i] = oprs[3][i];
            //    data3[i] = oprs[4][i];
            //}

            // The data for the bar chart
            //double[] data0 = { 100, 115, 165, 107, 67 };
            //double[] data1 = { 85, 106, 129, 161, 123 };
            //double[] data2 = { 67, 87, 86, 167, 157 };

            //data0 = Chart_SampleData(myUnitId);
            //data1 = Chart_SampleData(myUnitId);

            // The labels for the bar chart
            //string[] labels = { "Mon", "Tue", "Wed", "Thu", "Fri" };

            // Create a XYChart object of size 600 x 360 pixels
            XYChart c = new XYChart(szViewer[(int)EChart.Bar].Width, szViewer[(int)EChart.Bar].Height + 20);

            // Set default text color to dark grey (0x333333)
            c.setColor(Chart.TextColor, 0x333333);

            // Set the plotarea at (70, 20) and of size 400 x 300 pixels, with transparent
            // background and border and light grey (0xcccccc) horizontal grid lines
            //c.setPlotArea(70, 20, 400, 300, Chart.Transparent, -1, Chart.Transparent, 0xcccccc);
            int offsetWidth = 70;
            //int offsetHeight = 30;
            int offsetHeight = 15;
            c.setPlotArea(offsetWidth, offsetHeight + 20, szViewer[(int)EChart.Bar].Width - offsetWidth - 10, szViewer[(int)EChart.Bar].Height - 80, 0xf8f8f8, 0xffffff);

            // Add a legend box at (480, 20) using vertical layout and 12pt Arial font. Set
            // background and border to transparent and key icon border to the same as the fill
            // color.
            //LegendBox b = c.addLegend(100, 5, false, "Arial", 8);
            //b.setBackground(Chart.Transparent, Chart.Transparent);
            //b.setKeyBorder(Chart.SameAsMainColor);

            // Set the x and y axis stems to transparent and the label font to 12pt Arial
            c.xAxis().setColors(Chart.Transparent);
            c.yAxis().setColors(Chart.Transparent);
            c.xAxis().setLabelStyle("Arial", 12);
            c.yAxis().setLabelStyle("Arial", 12);

            // Add a stacked bar layer
            BarLayer layer = c.addBarLayer2(Chart.Stack);

            // Add the three data sets to the bar layer
            //layer.addDataSet(data0, 0xaaccee, "Server # 1");

            //layer.addDataSet(data0, iColorLow, "순살");
            //layer.addDataSet(data1, iColorHigh, "뼈");

            LegendBox b = c.addLegend(100, 5, false, "Arial", 8);
            b.setBackground(Chart.Transparent, Chart.Transparent);
            b.setKeyBorder(Chart.SameAsMainColor);

            if (myUnitId == Unit.Actual)
            {
                layer.addDataSet(data0, iColorLow, $"Robot Operation Time");
            }
            else if (myUnitId == Unit.Maint)
            {
                layer.addDataSet(data0, iColorMaint[0], $"Gripper");
                layer.addDataSet(data1, iColorMaint[1], $"Robot Down");
                layer.addDataSet(data2, iColorMaint[2], $"Robot Up");
                layer.addDataSet(data3, iColorMaint[3], $"Robot Shake");
                layer.addDataSet(data4, iColorMaint[4], $"Robot Oxzen");
                layer.addDataSet(data5, iColorMaint[5], $"Robot Drain");
                layer.addDataSet(data6, iColorMaint[6], $"Robot Clean");
            }

            //layer.addDataSet(data1, iColorLow, $"data1");
            //layer.addDataSet(data2, iColorLow, $"data2");
            //layer.addDataSet(data3, iColorLow, $"data3");
            //layer.addDataSet(data1, iColorHigh, "뼈");
            //layer.addDataSet(data2, 0xeeaa66, "Server # 3");

            // Set the bar border to transparent
            //layer.setBorderColor(Chart.Transparent);
            //
            //// Enable labelling for the entire bar and use 12pt Arial font
            //layer.setAggregateLabelStyle("Arial", 12);
            //
            //// Enable labelling for the bar segments and use 12pt Arial font with center alignment
            //layer.setDataLabelStyle("Arial", 10).setAlignment(Chart.Center);
            //
            //// For a vertical stacked bar with positive data, the first data set is at the bottom.
            //// For the legend box, by default, the first entry at the top. We can reverse the legend
            //// order to make the legend box consistent with the stacked bar.
            //layer.setLegendOrder(Chart.ReverseLegend);

            // Set the bar border to transparent
            layer.setBorderColor(Chart.Transparent);

            // Enable labelling for the entire bar and use 12pt Arial font
            layer.setAggregateLabelStyle("Arial", 12);

            // Enable labelling for the bar segments and use 12pt Arial font with center alignment
            layer.setDataLabelStyle("Arial", 10).setAlignment(Chart.Center);

            // For a vertical stacked bar with positive data, the first data set is at the bottom.
            // For the legend box, by default, the first entry at the top. We can reverse the legend
            // order to make the legend box consistent with the stacked bar.
            layer.setLegendOrder(Chart.ReverseLegend);

            // Set the labels on the x axis.
            c.xAxis().setLabels(labels);

            //c.xAxis().setTickDensity(5);

            // For the automatic y-axis labels, set the minimum spacing to 40 pixels.
            c.yAxis().setTickDensity(40);

            // Add a title to the y axis using dark grey (0x555555) 14pt Arial Bold font
            if (myUnitId == Unit.Actual)
            {
                c.yAxis().setTitle("시간 [ 주기 ]", "Arial Bold", 11, 0x555555);
            }
            else if (myUnitId == Unit.Maint)
            {
                c.yAxis().setTitle("회수 [ 카운트 ]", "Arial Bold", 11, 0x555555);
            }
            

            // Output the chart
            viewer.Chart = c;

            //include tool tip for the chart
            viewer.ImageMap = c.getHTMLImageMap("clickable", "", "title='{dataSetName} {xLabel}: {value}'");
        }

        string[] Chart_X_String(Unit unit)
        {
            string[] strXaxis = new string[] { };
            switch (unit)
            {
                case Unit.Hour:
                    strXaxis = new string[24];
                    for (int idx = 0; idx < strXaxis.Length; idx++)
                    {
                        strXaxis[idx] = $"{idx:00}";
                    }
                    break;
                case Unit.Day:
                    strXaxis = new string[DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)];
                    for (int idx = 0; idx < strXaxis.Length; idx++)
                    {
                        strXaxis[idx] = $"{idx:00}";
                    }
                    break;
                case Unit.Week:
                    strXaxis = new string[strWeekLabels.Length];
                    for (int idx = 0; idx < strXaxis.Length; idx++)
                    {
                        strXaxis[idx] = strWeekLabels[idx];
                    }
                    break;
                case Unit.Month:
                    strXaxis = new string[strMonthLabels.Length];
                    for (int idx = 0; idx < strXaxis.Length; idx++)
                    {
                        strXaxis[idx] = strMonthLabels[idx];
                    }
                    break;
              
                case Unit.Actual:
                    break;
            }

            return strXaxis;
        }

        double[] Chart_SampleData(Unit unit)
        {
            double[] dXaxis = new double[] { };
            switch (unit)
            {
                case Unit.Hour:
                    dXaxis = new double[24];
                    for (int idx = 0; idx < dXaxis.Length; idx++)
                    {
                        dXaxis[idx] = random.Next(500);
                    }
                    break;
                case Unit.Day:
                    dXaxis = new double[DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)];
                    for (int idx = 0; idx < dXaxis.Length; idx++)
                    {
                        dXaxis[idx] = random.Next(500);
                    }
                    break;
                case Unit.Week:
                    dXaxis = new double[7];
                    for (int idx = 0; idx < dXaxis.Length; idx++)
                    {
                        dXaxis[idx] = random.Next(500);
                    }
                    break;
                case Unit.Month:
                    dXaxis = new double[12];
                    for (int idx = 0; idx < dXaxis.Length; idx++)
                    {
                        dXaxis[idx] = random.Next(500);
                    }
                    break;
             
                case Unit.Actual:
                    break;
            }

            return dXaxis;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CreateChart(winChartViewer1);
            //CreateChart(winChartViewer2);
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            CreateChart(winChartViewer1);
        }

        /// <summary>
        /// 2023.02.14 시간 생산량 업데이트
        /// </summary>
        /// <param name="dat"></param>
        public void UpdateChart(List<Cores.Core_Data.Chicken> dat)
        {

            double[] fried = new double[dat.Count];
            double[] french = new double[dat.Count];


            int index = 0;
            foreach (Cores.Core_Data.Chicken chicken in dat)
            {
                fried[chicken.iHour] = chicken.iFried;
                french[chicken.iHour] = chicken.iFrench;
            }

            CreateChart(winChartViewer1, fried, french);
        }

        /// <summary>
        /// 2023.02.14 시간 생산량 업데이트
        /// </summary>
        /// <param name="dat"></param>
        public void UpdateChart(List<Cores.Core_Data.ChickenCounter> dat)
        {

            double[] fried = new double[6];
            double[] french = new double[6];

            DateTime currTime = DateTime.Now;

            var item = dat.Where(x => x.dateTime.Hour == currTime.Hour).ToList();
            for (int idx = 0; idx < 6; idx++)
            {
                fried[idx] = item.FindAll(x => x.cookerIndex == (idx + 1)).FindAll(y => y.chickenType == Cores.Core_Data.EChickenType.Fried).Count;
                french[idx] = item.FindAll(x => x.cookerIndex == (idx + 1)).FindAll(y => y.chickenType == Cores.Core_Data.EChickenType.French).Count;
            } 

            //CreateChart(winChartViewer2, fried, french);
        }

        public void UpdateChart(int[][] dat)
        {
            double[] fried = new double[dat.Length];
            double[] french = new double[dat.Length];
            int[] name = new int[dat.Length];

            //Console.WriteLine();

            for (int i = 0; i < fried.Length-1; i++)
            {
                if (dat[i + 1][0] != 0)
                {
                    name[i] = dat[i + 1][0];
                }
                else
                {
                    name[i] = i+1;
                }
                french[i] = dat[i+1][1];
                fried[i] = dat[i+1][2];
            }
            if (fried.Length == 31)
            {
                name[30] = 31;
            }
            else if (fried.Length == 7)
            {
                name[6] = 7;
            }
            else if (fried.Length == 12)
            {
                name[11] = 12;
            }
            CreateChart(winChartViewer1, name, fried, french);
        }

        public void UpdateChart()
        {
            if (myUnitId == Unit.Actual)
            {
                //CreateChart(winChartViewer1, Cores.Core_Object.GetOperNAs[1].lstDays, Cores.Core_Object.GetOperNAs[1].lstOpers);
            }

            if (myUnitId == Unit.Maint)
            {

                List<List<int>> copyBuff = new List<List<int>>();
                copyBuff.Add(Cores.Core_Object.GetOperNAs[0].lstOpers);
                copyBuff.Add(Cores.Core_Object.GetOperNAs[1].lstOpers);
                copyBuff.Add(Cores.Core_Object.GetOperNAs[2].lstOpers);
                copyBuff.Add(Cores.Core_Object.GetOperNAs[3].lstOpers);
                copyBuff.Add(Cores.Core_Object.GetOperNAs[4].lstOpers);


                CreateChart(winChartViewer1, Cores.Core_Object.GetOperNAs[1].lstDays, copyBuff);
            }
        }

        public void UpdateChart(List<string[]> dat)
        {
            CreateChart(winChartViewer1, dat);
        }
    }
}
