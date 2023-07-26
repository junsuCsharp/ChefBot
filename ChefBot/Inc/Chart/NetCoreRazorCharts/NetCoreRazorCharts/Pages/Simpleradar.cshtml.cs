using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ChartDirector;

namespace NetCoreRazorCharts.Pages
{
    public class SimpleradarModel : PageModel
    {
        public void OnGet()
        {
            ViewData["Title"] = "Simple Radar Chart";
            RazorChartViewer viewer = new RazorChartViewer(HttpContext, "chart1");
            ViewData["Viewer"] = viewer;
            createChart(viewer);
        }

        //
        // Create chart
        //
        private void createChart(RazorChartViewer viewer)
        {
            // The data for the chart
            double[] data = {90, 60, 65, 75, 40};

            // The labels for the chart
            string[] labels = {"Speed", "Reliability", "Comfort", "Safety", "Efficiency"};

            // Create a PolarChart object of size 450 x 350 pixels
            PolarChart c = new PolarChart(450, 350);

            // Set center of plot area at (225, 185) with radius 150 pixels
            c.setPlotArea(225, 185, 150);

            // Add an area layer to the polar chart
            c.addAreaLayer(data, 0x9999ff);

            // Set the labels to the angular axis as spokes
            c.angularAxis().setLabels(labels);

            // Output the chart
            viewer.Image = c.makeWebImage(Chart.PNG);

            // Include tool tip for the chart
            viewer.ImageMap = c.getHTMLImageMap("", "", "title='{label}: score = {value}'");
        }
    }
}

