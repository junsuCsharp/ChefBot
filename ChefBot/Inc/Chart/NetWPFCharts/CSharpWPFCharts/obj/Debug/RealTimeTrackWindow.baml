   M S B A M L   `   `   `  ����J  FCSharpWPFCharts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:CSharpWPFChartsCSharpWPFCharts  5   	   R Nnetchartdir, Version=6.4.0.0, Culture=neutral, PublicKeyToken=2db5fc05de12e204B0clr-namespace:ChartDirector;assembly=netchartdirChartDirector 5      )    #CSharpWPFCharts.RealTimeTrackWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:CSharpWPFCharts     5   	   JChartDirector0clr-namespace:ChartDirector;assembly=netchartdir     5      -   5          � UseLayoutRounding  .  � Title$$ Realtime Chart with Track Line��5	   	    � SizeToContent$ WidthAndHeight=�60    � 
ResizeMode$ NoResize=�6O   .��5      ]� .+�5   
   �� $#��Advanced Software Engineering��6   $��25��68   $	��Top=�6D   $��Arial)�6Y   $��Italic$�6l   $
��Bold"�6   $��13(�5      ���	#FF02098D���	#FFF4FF04$��Right=�6L   �� 5   
   $
��Left=�6   $	��120��6+   ���	#FFF0F0F0.+�5      	� -   5         	�Name$ runPB��5      #��[Bd�6)   $
��Left=�6X   .��5      ��  �� Orientation$ 
Horizontal=�6   $��5q�66   .+�5      �� $��	runPB.gif��6   $��16��6/   $��16��6:   �� 5      $��	Run Chart��6    $��6,0,0,0q�61   5      5      	� 5      -   5        $ freezePB��5      #��[Bd�6,   $
��Left=�6[   .��5      �� $ 
Horizontal=�6   $��5q�66   .+�5      �� $��freezePB.gif��6   $��16��62   $��16��6=   �� 5      $��Freeze Chart��6    $��6,0,0,0q�64   5      5      �� 5      $��Updated Rate (ms)��6   $��5,30,0,0q�61   $
��Bold"�6C   �� 5      -   5         ��Name$ samplePeriod��5      $��3q�6-   .@�5      �� .��6   2506$   �� 5      .��6   5006$   �� 5       .��6   7506$   �� 5!       �� 
IsSelected . .��61   100067   �� 5"      .��6   12506%   �� 5#      .��6   15006%   �� 5$      .��6   17506%   �� 5%      .��6   20006%   5&      �� 5'      $��Simulated Machine��6   $��3,30,0,1q�61   $
��Bold"�6C   � 5(      $��3q�6    � RowDefinitions 5)      �� 5*      $
��Auto �6$   �� 5+      $
��Auto �6$   �� 5,      $
��Auto �6$   	 � ColumnDefinitions	 5.      �� 5/      $
��Auto �6'   �� 50      $��* �6'   .+�52      �� $��0��6   $��0��6)   $��Center=�69   $��Alpha��6T   �� 53      $��1��6   $��0��6)   $��Center=�69   $
��Beta��6T   �� 54      $��2��6   $��0��6)   $��Center=�69   $��Gamma��6T   �� 55      -   5        
 ��Name$
 valueA��55      $��0��6(   $��1��65   $�� ��6E   $��6,0,0,0q�6Q   $��2q�6b   $��1q�6n   
�������6�   �� 56      -   5        $
 valueB��56      $��1��6(   $��1��65   $�� ��6E   $��6,2,0,2q�6Q   $��2q�6b   $��1q�6n   
�������6�   �� 57      -   5        $
 valueC��57      $��2��6(   $��1��65   $�� ��6E   $��6,0,0,0q�6Q   $��2q�6b   $��1q�6n   
�������6�   58      59      "  ChartDirector.WPFChartViewer  5:   
   -   5          Name$ WPFChartViewer1��5:   '   $	��Top=�6@   $	��600��6U   $	��270��6a   $��5q�6n   6�   5;      5<      