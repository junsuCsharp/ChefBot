   M S B A M L   `   `   `  ����J  FCSharpWPFCharts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:CSharpWPFChartsCSharpWPFCharts  5   	   R Nnetchartdir, Version=6.4.0.0, Culture=neutral, PublicKeyToken=2db5fc05de12e204B0clr-namespace:ChartDirector;assembly=netchartdirChartDirector 5      ,    &CSharpWPFCharts.ZoomScrollTrack2Window   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:CSharpWPFCharts     5   	   JChartDirector0clr-namespace:ChartDirector;assembly=netchartdir     5      -   5          � UseLayoutRounding  .  � Title$/ )Zooming and Scrolling with Track Line (2)��5	   	    � SizeToContent$ WidthAndHeight=�6;    � 
ResizeMode$ NoResize=�6Z   .��5      ]� .+�5   
   �� $#��Advanced Software Engineering��6   $	��Top=�68   $��Arial)�6M   $��Italic$�6`   $
��Bold"�6s   $��13(�5      ���	#FF02098D���	#FFF4FF04$��Right=�6L   �� 5   
   $
��Left=�6   $	��120��6+   ���	#FFF0F0F0.+�5      	� -   5         	�Name$ 	pointerPB��5      #��[Bd�6-   $
��Left=�6\   .��5      ��  �� Orientation$ 
Horizontal=�6   $��5q�66   .+�5      �� $��pointer.gif��6   $��16��61   �� 5      $��Pointer��6    $��6,0,0,0q�6/   5      5      	� 5      -   5        $ zoomInPB��5      #��[Bd�6,   $
��Left=�6[   .��5      �� $ 
Horizontal=�6   $��5q�66   .+�5      �� $��zoomInIcon.gif��6   $��16��64   �� 5      $��Zoom In��6    $��6,0,0,0q�6/   5      5      	� 5      -   5        $ 	zoomOutPB��5      #��[Bd�6-   $
��Left=�6\   .��5      �� $ 
Horizontal=�6   $��5q�66   .+�5      �� $��zoomOutIcon.gif��6   $��16��65   �� 5      $��Zoom Out��6    $��6,0,0,0q�60   5      5       �� 5!      $��
Start Date��6   $��	3,110,0,2q�6*   $
��Bold"�6=   (  "System.Windows.Controls.DatePicker  5"      -   5          Name$ startDateCtrl��5"      60   �� 5#      $��End Date��6   $��3,20,0,2q�6(   $
��Bold"�6:     5$      -   5        $ endDateCtrl��5$      6.   5%      "  ChartDirector.WPFChartViewer  5&   
   -   5          Name$ WPFChartViewer1��5&   '   $	��Top=�6@   $	��640��6U   $	��350��6a   $��5q�6n   5'   ?   �� 5(   
   -   5         ��Name$ hScrollBar1��5(      $	��Top=�6)   	 �� Orientation$	 
Horizontal=�6>   6W   5)      5*      