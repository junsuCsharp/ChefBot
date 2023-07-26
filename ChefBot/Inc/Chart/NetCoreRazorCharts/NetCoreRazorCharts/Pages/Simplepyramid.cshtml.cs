using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ChartDirector;

namespace NetCoreRazorCharts.Pages
{
    public class SimplepyramidModel : PageModel
    {
        public void OnGet()
        {
            ViewData["Title"] = "Simple Pyramid Chart";
            RazorChartViewer viewer = new RazorChartViewer(HttpContext, "chart1");
            ViewData["Viewer"] = viewer;
            createChart(viewer);
        }

        //
        // Create chart
        //
        private void createChart(RazorChartViewer viewer)
        {
            // The data for the pyramid chart
            double[] data = {156, 123, 211, 179};

            // The labels for the pyramid chart
            string[] labels = {"Funds", "Bonds", "Stocks", "Cash"};

            // Create a PyramidChart object of size 360 x 360 pixels
            PyramidChart c = new PyramidChart(360, 360);

            // Set the pyramid center at (180, 180), and width x height to 150 x 180 pixels
            c.setPyramidSize(180, 180, 150, 300);

            // Set the pyramid data and labels
            c.setData(data, labels);

            // Add labels at the center of the pyramid layers using Arial Bold font. The labels will have
            // two lines showing the layer name and percentage.
            c.setCenterLabel("{label}\n{percent}%", "Arial Bold");

            // Output the chart
            viewer.Image = c.makeWebImage(Chart.PNG);

            // Include tool tip for the chart
            viewer.ImageMap = c.getHTMLImageMap("", "", "title='{label}: US$ {value}M ({percent}%)'");
        }
    }
}

