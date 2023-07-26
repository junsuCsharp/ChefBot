using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ChartDirector;

namespace NetCoreRazorCharts.Pages
{
    public class PyramidrotationModel : PageModel
    {
        public void OnGet()
        {
            ViewData["Title"] = "Pyramid Rotation";

            // This example contains 7 charts.
            var viewers = new RazorChartViewer[7];
            ViewData["Viewer"] = viewers;

            for (int i = 0; i < viewers.Length; ++i)
                createChart(viewers[i] = new RazorChartViewer(HttpContext, "chart" + i), i);

        }

        //
        // Create chart
        //
        private void createChart(RazorChartViewer viewer, int chartIndex)
        {
            // The data for the pyramid chart
            double[] data = {156, 123, 211, 179};

            // The semi-transparent colors for the pyramid layers
            int[] colors = {0x400000cc, 0x4066aaee, 0x40ffbb00, 0x40ee6622};

            // The rotation angle
            int angle = chartIndex * 15;

            // Create a PyramidChart object of size 200 x 200 pixels, with white (ffffff) background and
            // grey (888888) border
            PyramidChart c = new PyramidChart(200, 200, 0xffffff, 0x888888);

            // Set the pyramid center at (100, 100), and width x height to 60 x 120 pixels
            c.setPyramidSize(100, 100, 60, 120);

            // Set the elevation to 15 degrees and use the given rotation angle
            c.addTitle("Rotation = " + angle, "Arial Italic", 15);
            c.setViewAngle(15, angle);

            // Set the pyramid data
            c.setData(data);

            // Set the layer colors to the given colors
            c.setColors2(Chart.DataColor, colors);

            // Leave 1% gaps between layers
            c.setLayerGap(0.01);

            // Output the chart
            viewer.Image = c.makeWebImage(Chart.PNG);
        }
    }
}

