   M S B A M L   `   `   `  ����J  FCSharpWPFCharts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:CSharpWPFChartsCSharpWPFCharts  5   	   R Nnetchartdir, Version=6.4.0.0, Culture=neutral, PublicKeyToken=2db5fc05de12e204B0clr-namespace:ChartDirector;assembly=netchartdirChartDirector 5   	   !    CSharpWPFCharts.ParamViewer   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:CSharpWPFCharts     5   	   JChartDirector0clr-namespace:ChartDirector;assembly=netchartdir     5   	     � Title$  ParamViewer��5
   	   $	��391��6   $	��332��6*    � 
ResizeMode$ NoResize=�66    � ShowInTaskbar .  .��5      � .+�5   
   �� -   5         ��Name$ 	textBlock��5      $
��Left=�6'   $��8,5,8,0q�6B   $
��Wrap=�6S   $����This is to demonstrate that ChartDirector charts are clickable. In this demo program, we just display the information provided to the ClickHotSpot event handler. ��6g   $	��Top=�6  $��55��6)  $	��308��65  &   System.Windows.Controls.DataGrid  5   
   -   5        $��dataGrid��5      $��8,60,8,8q�6#      HorizontalGridLinesBrush
 �����   VerticalGridLinesBrush
 �����   HeadersVisibility$ Column=�6}      
IsReadOnly .    Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn	   Header$		 Key��6%   
   Width5  /System.Windows.Controls.DataGridLengthConverter$	
 100 6J   1  +System.Windows.Controls.DataGridBoundColumn   Binding 62   �� *Key+6J     5      $	 Value��6%   $
 * 6N    64   �� *Value+6N   5      5      5      