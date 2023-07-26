using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ChartDirector;

namespace NetCoreRazorCharts.Pages
{
    public class ThreedbarModel : PageModel
    {
        public void OnGet()
        {
            ViewData["Title"] = "3D Bar Chart";
            RazorChartViewer viewer = new RazorChartViewer(HttpContext, "chart1");
            ViewData["Viewer"] = viewer;
            createChart(viewer);
        }

        //
        // Create chart
        //
        private void createChart(RazorChartViewer viewer)
        {
            // The data for the bar chart
            double[] data = {85, 156, 179.5, 211, 123};

            // The labels for the bar chart
            string[] labels = {"Mon", "Tue", "Wed", "Thu", "Fri"};

            // Create a XYChart object of size 300 x 280 pixels
            XYChart c = new XYChart(300, 280);

            // Set the plotarea at (45, 30) and of size 200 x 200 pixels
            c.setPlotArea(45, 30, 200, 200);

            // Add a title to the chart
            c.addTitle("Weekly Server Load");

            // Add a title to the y axis
            c.yAxis().setTitle("MBytes");

            // Add a title to the x axis
            c.xAxis().setTitle("Work Week 25");

            // Add a bar chart layer with green (0x00ff00) bars using the given data
            c.addBarLayer(data, 0x00ff00).set3D();

            // Set the labels on the x axis.
            c.xAxis().setLabels(labels);

            // Output the chart
            viewer.Image = c.makeWebImage(Chart.PNG);

            // Include tool tip for the chart
            viewer.ImageMap = c.getHTMLImageMap("", "", "title='{xLabel}: {value} MBytes'");
        }
    }
}

