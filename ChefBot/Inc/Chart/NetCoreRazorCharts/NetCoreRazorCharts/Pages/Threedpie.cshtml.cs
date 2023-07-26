using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ChartDirector;

namespace NetCoreRazorCharts.Pages
{
    public class ThreedpieModel : PageModel
    {
        public void OnGet()
        {
            ViewData["Title"] = "3D Pie Chart";
            RazorChartViewer viewer = new RazorChartViewer(HttpContext, "chart1");
            ViewData["Viewer"] = viewer;
            createChart(viewer);
        }

        //
        // Create chart
        //
        private void createChart(RazorChartViewer viewer)
        {
            // The data for the pie chart
            double[] data = {25, 18, 15, 12, 8, 30, 35};

            // The labels for the pie chart
            string[] labels = {"Labor", "Licenses", "Taxes", "Legal", "Insurance", "Facilities",
                "Production"};

            // Create a PieChart object of size 360 x 300 pixels
            PieChart c = new PieChart(360, 300);

            // Set the center of the pie at (180, 140) and the radius to 100 pixels
            c.setPieSize(180, 140, 100);

            // Add a title to the pie chart
            c.addTitle("Project Cost Breakdown");

            // Draw the pie in 3D
            c.set3D();

            // Set the pie data and the pie labels
            c.setData(data, labels);

            // Explode the 1st sector (index = 0)
            c.setExplode(0);

            // Output the chart
            viewer.Image = c.makeWebImage(Chart.PNG);

            // Include tool tip for the chart
            viewer.ImageMap = c.getHTMLImageMap("", "", "title='{label}: US${value}K ({percent}%)'");
        }
    }
}

