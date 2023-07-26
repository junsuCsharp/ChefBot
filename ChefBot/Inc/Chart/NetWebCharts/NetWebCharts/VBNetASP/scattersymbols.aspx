<%@ Page Language="VB" Debug="true" %>
<%@ Import Namespace="ChartDirector" %>
<%@ Register TagPrefix="chart" Namespace="ChartDirector" Assembly="netchartdir" %>

<!DOCTYPE html>

<script runat="server">

'
' Page Load event handler
'
Protected Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)

    ' The XY points for the scatter chart
    Dim dataX() As Double = {200, 400, 300, 250, 500}
    Dim dataY() As Double = {40, 100, 50, 150, 250}

    ' The custom symbols for the points
    Dim symbols() As String = {"robot1.png", "robot2.png", "robot3.png", "robot4.png", _
        "robot5.png"}

    ' Create a XYChart object of size 450 x 400 pixels
    Dim c As XYChart = New XYChart(450, 400)

    ' Set the plotarea at (55, 40) and of size 350 x 300 pixels, with a light grey border
    ' (0xc0c0c0). Turn on both horizontal and vertical grid lines with light grey color (0xc0c0c0)
    c.setPlotArea(55, 40, 350, 300, -1, -1, &Hc0c0c0, &Hc0c0c0, -1)

    ' Add a title to the chart using 18pt Times Bold Itatic font.
    c.addTitle("Battle Robots", "Times New Roman Bold Italic", 18)

    ' Add a title to the y axis using 12pt Arial Bold Italic font
    c.yAxis().setTitle("Speed (km/s)", "Arial Bold Italic", 12)

    ' Add a title to the y axis using 12pt Arial Bold Italic font
    c.xAxis().setTitle("Range (km)", "Arial Bold Italic", 12)

    ' Set the axes line width to 3 pixels
    c.xAxis().setWidth(3)
    c.yAxis().setWidth(3)

    ' Add each point of the data as a separate scatter layer, so that they can have a different
    ' symbol
    For i As Integer = 0 To UBound(dataX)
        c.addScatterLayer(New Double() {dataX(i)}, New Double() {dataY(i)}).getDataSet(0 _
            ).setDataSymbol2(Server.MapPath(symbols(i)))
    Next

    ' Output the chart
    WebChartViewer1.Image = c.makeWebImage(Chart.PNG)

    ' Include tool tip for the chart
    WebChartViewer1.ImageMap = c.getHTMLImageMap("", "", _
        "title='Range = {x} km, Speed = {value} km/s'")

End Sub

</script>

<html>
<head>
    <title>Custom Scatter Symbols</title>
</head>
<body style="margin:5px 0px 0px 5px">
    <div style="font:bold 18pt verdana">
        Custom Scatter Symbols
    </div>
    <hr style="border:solid 1px #000080" />
    <div style="font:10pt verdana; margin-bottom:1.5em">
        <a href='viewsource.aspx?file=<%=Request("SCRIPT_NAME")%>'>View Source Code</a>
    </div>
    <chart:WebChartViewer id="WebChartViewer1" runat="server" />
</body>
</html>

