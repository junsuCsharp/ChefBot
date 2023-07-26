using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ChartDirector;

namespace NetCoreRazorCharts.Pages
{
    public class TicksModel : PageModel
    {
        public void OnGet()
        {
            ViewData["Title"] = "Tick Density";

            // This example contains 2 charts.
            var viewers = new RazorChartViewer[2];
            ViewData["Viewer"] = viewers;

            for (int i = 0; i < viewers.Length; ++i)
                createChart(viewers[i] = new RazorChartViewer(HttpContext, "chart" + i), i);

        }

        //
        // Create chart
        //
        private void createChart(RazorChartViewer viewer, int chartIndex)
        {
            // The data for the chart
            double[] data = {100, 125, 265, 147, 67, 105};
            string[] labels = {"Jan", "Feb", "Mar", "Apr", "May", "Jun"};

            // Create a XYChart object of size 250 x 250 pixels
            XYChart c = new XYChart(250, 250);

            // Set the plot area at (27, 25) and of size 200 x 200 pixels
            c.setPlotArea(27, 25, 200, 200);

            if (chartIndex == 1) {
                // High tick density, uses 10 pixels as tick spacing
                c.addTitle("Tick Density = 10 pixels");
                c.yAxis().setTickDensity(10);
            } else {
                // Normal tick density, just use the default setting
                c.addTitle("Default Tick Density");
            }

            // Set the labels on the x axis
            c.xAxis().setLabels(labels);

            // Add a color bar layer using the given data. Use a 1 pixel 3D border for the bars.
            c.addBarLayer3(data).setBorderColor(-1, 1);

            // Output the chart
            viewer.Image = c.makeWebImage(Chart.PNG);

            // Include tool tip for the chart
            viewer.ImageMap = c.getHTMLImageMap("", "", "title='Revenue for {xLabel}: US${value}M'");
        }
    }
}

