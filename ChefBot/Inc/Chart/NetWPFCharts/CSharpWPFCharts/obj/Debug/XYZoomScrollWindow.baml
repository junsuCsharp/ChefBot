   M S B A M L   `   `   `  ����J  FCSharpWPFCharts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:CSharpWPFChartsCSharpWPFCharts  5   	   R Nnetchartdir, Version=6.4.0.0, Culture=neutral, PublicKeyToken=2db5fc05de12e204B0clr-namespace:ChartDirector;assembly=netchartdirChartDirector 5      (    "CSharpWPFCharts.XYZoomScrollWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:CSharpWPFCharts     5   	   JChartDirector0clr-namespace:ChartDirector;assembly=netchartdir     5      -   5          � UseLayoutRounding  .  � Title$ XY Zooming and Scrolling��5	   	   ���	#FFCCCCFF � SizeToContent$ WidthAndHeight=�6X    � 
ResizeMode$ NoResize=�6w   .��5      ]� .+�5   
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
��Save��6    $��6,0,0,0q�6,   5%      5&      �� 5'      $��
Zoom Level��6   $��0,46,0,0q�6*   $��Center=�6<   $
��Bold"�6Y   �� 5(      -   5         ��Name$ zoomBar��5(      $��5q�6&    �� TickPlacement$
 Both=�6U   ���	#FF5555556j   &   ChartDirector.WPFViewPortControl  5)      -   5        	  Name$	 ViewPortControl1��5)   /   $	��110��6I   $	��110��6U   $��0,68,0,0q�6b   
   ViewPortExternalColor$
 	#7F000000��5*         ViewPortBorderColor$ 	#7FFFFFFF��6'      SelectionBorderColor$ 	#7FFFFFFF��6G   5+      "  ChartDirector.WPFChartViewer  5,   
   -   5          Name$ WPFChartViewer1��5,   '   $	��500��6@   $	��480��6L      ScrollDirection$ HorizontalVertical=�5.         ZoomDirection$ HorizontalVertical=�62      MouseWheelZoomRatio$	 1.1Q�6U   5/      50      