﻿#pragma checksum "..\..\ZoomScrollTrack2Window.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "317671649E27A898F0B6AB46F5497EFEFA760DBD"
//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.42000
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

using CSharpWPFCharts;
using ChartDirector;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace CSharpWPFCharts {
    
    
    /// <summary>
    /// ZoomScrollTrack2Window
    /// </summary>
    public partial class ZoomScrollTrack2Window : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 15 "..\..\ZoomScrollTrack2Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton pointerPB;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\ZoomScrollTrack2Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton zoomInPB;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\ZoomScrollTrack2Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton zoomOutPB;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\ZoomScrollTrack2Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker startDateCtrl;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\ZoomScrollTrack2Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker endDateCtrl;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\ZoomScrollTrack2Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal ChartDirector.WPFChartViewer WPFChartViewer1;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\ZoomScrollTrack2Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.ScrollBar hScrollBar1;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/CSharpWPFCharts;component/zoomscrolltrack2window.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\ZoomScrollTrack2Window.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 9 "..\..\ZoomScrollTrack2Window.xaml"
            ((CSharpWPFCharts.ZoomScrollTrack2Window)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.pointerPB = ((System.Windows.Controls.RadioButton)(target));
            
            #line 15 "..\..\ZoomScrollTrack2Window.xaml"
            this.pointerPB.Checked += new System.Windows.RoutedEventHandler(this.pointerPB_Checked);
            
            #line default
            #line hidden
            return;
            case 3:
            this.zoomInPB = ((System.Windows.Controls.RadioButton)(target));
            
            #line 21 "..\..\ZoomScrollTrack2Window.xaml"
            this.zoomInPB.Checked += new System.Windows.RoutedEventHandler(this.zoomInPB_Checked);
            
            #line default
            #line hidden
            return;
            case 4:
            this.zoomOutPB = ((System.Windows.Controls.RadioButton)(target));
            
            #line 27 "..\..\ZoomScrollTrack2Window.xaml"
            this.zoomOutPB.Checked += new System.Windows.RoutedEventHandler(this.zoomOutPB_Checked);
            
            #line default
            #line hidden
            return;
            case 5:
            this.startDateCtrl = ((System.Windows.Controls.DatePicker)(target));
            
            #line 34 "..\..\ZoomScrollTrack2Window.xaml"
            this.startDateCtrl.SelectedDateChanged += new System.EventHandler<System.Windows.Controls.SelectionChangedEventArgs>(this.startDateCtrl_SelectedDateChanged);
            
            #line default
            #line hidden
            return;
            case 6:
            this.endDateCtrl = ((System.Windows.Controls.DatePicker)(target));
            
            #line 36 "..\..\ZoomScrollTrack2Window.xaml"
            this.endDateCtrl.SelectedDateChanged += new System.EventHandler<System.Windows.Controls.SelectionChangedEventArgs>(this.endDateCtrl_SelectedDateChanged);
            
            #line default
            #line hidden
            return;
            case 7:
            this.WPFChartViewer1 = ((ChartDirector.WPFChartViewer)(target));
            
            #line 39 "..\..\ZoomScrollTrack2Window.xaml"
            this.WPFChartViewer1.ViewPortChanged += new ChartDirector.WPFViewPortEventHandler(this.WPFChartViewer1_ViewPortChanged);
            
            #line default
            #line hidden
            
            #line 39 "..\..\ZoomScrollTrack2Window.xaml"
            this.WPFChartViewer1.MouseMovePlotArea += new System.Windows.Input.MouseEventHandler(this.WPFChartViewer1_MouseMovePlotArea);
            
            #line default
            #line hidden
            return;
            case 8:
            this.hScrollBar1 = ((System.Windows.Controls.Primitives.ScrollBar)(target));
            
            #line 40 "..\..\ZoomScrollTrack2Window.xaml"
            this.hScrollBar1.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.hScrollBar1_ValueChanged);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

