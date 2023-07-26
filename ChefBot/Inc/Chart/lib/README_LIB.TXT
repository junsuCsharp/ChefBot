The "netchartdir.dll" included in the ChartDirector for .NET download is 
for the .NET Framework. If you are developing a .NET Framework application,
you may reference "netchartdir.dll" to use ChartDirector in your project.

For .NET Core applications, instead of referencing the DLL, please include 
the NuGet package "ChartDirector.Net" (for web or console applications) or
"ChartDirector.Net.Desktop.Controls" (for desktop applications). This is by
right clicking on the "References" or "Dependencies" folder in your Visual 
Studio project and select "Manage NuGet Package".

For details, please refer to the Installation chapter of the ChartDirector 
documentation.
