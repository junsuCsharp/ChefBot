using Microsoft.AspNetCore.Mvc.RazorPages;
using ChartDirector;

namespace NetCoreRazorCharts.Pages
{
    public class CdinfoModel : PageModel
    {
        public void OnGet()
        {
            ViewData["Description"] = Chart.getDescription();
            ViewData["MajorVersion"] = (Chart.getVersion() & 0xff000000) / 0x1000000;
			ViewData["MinorVersion"] = (Chart.getVersion() & 0xff0000) / 0x10000;
			ViewData["MicroVersion"] = Chart.getVersion() & 0xffff;
			ViewData["Copyright"] = Chart.getCopyright();
			ViewData["BootLog"] = Chart.getBootLog();
			ViewData["FontTest"] = Chart.libgTTFTest();
		}
    }
}