   M S B A M L   `   `   `  ����J  FCSharpWPFCharts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:CSharpWPFChartsCSharpWPFCharts  5   	   R Nnetchartdir, Version=6.4.0.0, Culture=neutral, PublicKeyToken=2db5fc05de12e204B0clr-namespace:ChartDirector;assembly=netchartdirChartDirector 5      .    (CSharpWPFCharts.RealTimeZoomScrollWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:CSharpWPFCharts     5   	   JChartDirector0clr-namespace:ChartDirector;assembly=netchartdir     5      -   5          � UseLayoutRounding  .  � Title$4 .Realtime Chart with Zoom/Scroll and Track Line��5	   	    � SizeToContent$ WidthAndHeight=�6@    � 
ResizeMode$ NoResize=�6_   .��5      ]� .+�5   
   �� $#��Advanced Software Engineering��6   $��25��68   $	��Top=�6D   $��Arial)�6Y   $��Italic$�6l   $
��Bold"�6   $��13(�5      ���	#FF02098D���	#FFF4FF04$��Right=�6L   �� 5   
   $
��Left=�6   $	��120��6+   ���	#FFF0F0F0.+�5      	� -   5         	�Name$ 	pointerPB��5      #��[Bd�6-   $
��Left=�6\   .��5      ��  �� Orientation$ 
Horizontal=�6   $��5q�66   .+�5      �� $��pointer.gif��6   $��16��61   �� 5      $��Pointer��6    $��6,0,0,0q�6/   5      5      	� 5      -   5        $ zoomInPB��5      #��[Bd�6,   $
��Left=�6[   .��5      �� $ 
Horizontal=�6   $��5q�66   .+�5      �� $��zoomInIcon.gif��6   $��16��64   �� 5      $��Zoom In��6    $��6,0,0,0q�6/   5      5      	� 5      -   5        $ 	zoomOutPB��5      #��[Bd�6-   $
��Left=�6\   .��5      �� $ 
Horizontal=�6   $��5q�66   .+�5      �� $��zoomOutIcon.gif��6   $��16��65   �� 5      $��Zoom Out��6    $��6,0,0,0q�60   5      5       �� 5!      -   5         ��Name$ savePB��5!      $��0,32,0,0q�6%   $
��Left=�67   .��5"      �� $ 
Horizontal=�6   $��5q�66   .+�5#      �� $��saveIcon.gif��6   $��14��62   $��14��6=   �� 5$      $
��Save��6    $��6,0,0,0q�6,   5%      5&      �� 5'      $��Updated Rate (ms)��6   $��5,30,0,0q�61   $
��Bold"�6C   �� 5(      -   5         ��Name$ samplePeriod��5(      $��3q�6-   .@�5)      �� .��6   2506$   �� 5*      .��6   5006$   �� 5+      .��6   7506$   �� 5,       �� 
IsSelected . .��61   100067   �� 5-      .��6   12506%   �� 5.      .��6   15006%   �� 5/      .��6   17506%   �� 50      .��6   20006%   51      �� 52      $��Simulated Machine��6   $��3,30,0,1q�61   $
��Bold"�6C   � 53      $��3q�6   	 � RowDefinitions	 54      �� 55      $
��Auto �6$   �� 56      $
��Auto �6$   �� 57      $
��Auto �6$   
 � ColumnDefinitions
 59      �� 5:      $
��Auto �6'   �� 5;      $��* �6'   .+�5=      �� $��0��6   $��0��6)   $��Center=�69   $��Alpha��6T   �� 5>      $��1��6   $��0��6)   $��Center=�69   $
��Beta��6T   �� 5?      $��2��6   $��0��6)   $��Center=�69   $��Gamma��6T   �� 5@      -   5         ��Name$ valueA��5@      $��0��6(   $��1��65   $�� ��6E   $��6,0,0,0q�6Q   $��2q�6b   $��1q�6n   
�������6�   �� 5A      -   5        $ valueB��5A      $��1��6(   $��1��65   $�� ��6E   $��6,2,0,2q�6Q   $��2q�6b   $��1q�6n   
�������6�   �� 5B      -	   5        $ valueC��5B      $��2��6(   $��1��65   $�� ��6E   $��6,0,0,0q�6Q   $��2q�6b   $��1q�6n   
�������6�   5C      5D      "  ChartDirector.WPFChartViewer  5E   
   -
   5          Name$ WPFChartViewer1��5E   '   $	��Top=�6@   $	��640��6U   $	��350��6a   $��5q�6n   6�   �� 5F   
   -   5         ��Name$ hScrollBar1��5F      $	��Top=�6)    �� Orientation$ 
Horizontal=�6>   6W   5G      5H      