using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ChartDirector;

namespace NetCoreRazorCharts.Pages
{
    public class VlinearmeterorientationModel : PageModel
    {
        public void OnGet()
        {
            ViewData["Title"] = "V-Linear Meter Orientation";

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
            // The value to display on the meter
            double value = 75.35;

            // Create a LinearMeter object of size 70 x 240 pixels with very light grey (0xeeeeee)
            // backgruond and a light grey (0xccccccc) 3-pixel thick rounded frame
            LinearMeter m = new LinearMeter(70, 240, 0xeeeeee, 0xcccccc);
            m.setRoundedFrame(Chart.Transparent);
            m.setThickFrame(3);

            // This example demonstrates putting the text labels at the left or right side by setting the
            // label alignment and scale position.
            if (chartIndex == 0) {
                m.setMeter(28, 18, 20, 205, Chart.Left);
            } else {
                m.setMeter(20, 18, 20, 205, Chart.Right);
            }

            // Set meter scale from 0 - 100, with a tick every 10 units
            m.setScale(0, 100, 10);

            // Add a smooth color scale to the meter
            double[] smoothColorScale = {0, 0x6666ff, 25, 0x00bbbb, 50, 0x00ff00, 75, 0xffff00, 100,
                0xff0000};
            m.addColorScale(smoothColorScale);

            // Add a blue (0x0000cc) pointer at the specified value
            m.addPointer(value, 0x0000cc);

            // Output the chart
            viewer.Image = m.makeWebImage(Chart.PNG);
        }
    }
}

