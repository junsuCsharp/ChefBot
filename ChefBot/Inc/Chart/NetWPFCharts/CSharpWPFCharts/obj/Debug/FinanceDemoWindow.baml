   M S B A M L   `   `   `  ����J  FCSharpWPFCharts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:CSharpWPFChartsCSharpWPFCharts  5   	   R Nnetchartdir, Version=6.4.0.0, Culture=neutral, PublicKeyToken=2db5fc05de12e204B0clr-namespace:ChartDirector;assembly=netchartdirChartDirector 5   	   '    !CSharpWPFCharts.FinanceDemoWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:CSharpWPFCharts     5   	   JChartDirector0clr-namespace:ChartDirector;assembly=netchartdir     5   	   -   5          � UseLayoutRounding  .  � Title$1 +ChartDirector Financial Chart Demonstration��5	   	    � SizeToContent$ WidthAndHeight=�6=    � 	Resources 5
       �� 
TargetType   
LabelStyle �� Property �� Value��%}   &	        �� 5   
   " ��6   .�5      ��  � ��$ 0,6,0,0q�6'   �� 5       � ��$
 Bold"�6+   5      5      .��5      ]� .+�5   
   �� $#��Advanced Software Engineering��6   $	��Top=�68   $��Arial)�6M   $��Italic$�6`   $
��Bold"�6s   $��13(�5      ���	#FF02098D���	#FFF4FF04$��Right=�6L   � 5   
   $
��Left=�6   $	��160��6%   ���	#FFBADDFF.+�5      �� -   5        $��8,0,8,8q�5      .+�5      �� $��Ticker Symbol��6   #��[  61   �� 5      -   5        $��tickerSymbol��5      .��6p   
ASE.Symbol6|   �� 5      $��Compare Width��6   #��[  61   �� 5      -   5        $��compareWith��5      6o   �� 5      $��Time Period��6   #��[  6/   �� 5      -   5        $��	timeRange��5      �� 5      $��
Chart Size��6   #��[  6.   �� 5      -   5        $��	chartSize��5      �� 5      $��
Chart Type��6   #��[  6.   �� 5      -   5        $��	chartType��5      �� 5       -   5        $��
volumeBars��5       $��0,8,0,3q�6-    d� 	IsChecked$
 TrueY�6>   .��6O   Show Volume Bars6a   �� 5!      -	   5        $��parabolicSAR��5!      $��0,0,0,3q�6/   .��6@   Parabolic SAR6O   �� 5"      -
   5        $��logScale��5"      $��0,0,0,3q�6+   .��6<   	Log Scale6G   �� 5#      -   5        $��percentageScale��5#      .��62   Percentage Scale6D   �� 5$      $��
Price Band��6   #��[  6.   �� 5%      -   5        $��	priceBand��5%      �� 5&      $��Moving Averages��6   #��[  63   ]� 5'      $��0,0,0,1q�6   .+�5(      �� -   5        $��movAvg1��5(   @   $��Right=�6   $��40��65   .��6q   106u   �� 5)      -   5        $��avgType1��5)      5*      ]� 5+      .+�5,      �� -   5        $��movAvg2��5,   @   $��Right=�6   $��40��65   .��6q   256u   �� 5-      -   5        $��avgType2��5-      5.      �� 5/      $��Technical Indicators��6   #��[  68   �� 50      -   5        $��
indicator1��50      $��0,0,0,1q�6-   �� 51      -   5        $��
indicator2��51      $��0,0,0,1q�6-   �� 52      -   5        $��
indicator3��52      $��0,0,0,1q�6-   �� 53      -   5        $��
indicator4��53      54      55      "  ChartDirector.WPFChartViewer  56   
   -   5          Name$ WPFChartViewer1��56   '   $	��Top=�6@   $	��780��6U   $	��560��6a   $��8q�6n   57      58      