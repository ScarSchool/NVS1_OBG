﻿#pragma checksum "..\..\..\MainWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "091A1FD00968B0B8B638D55425E413E297B825EC"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using ImageServer;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
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


namespace ImageServer {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 18 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel pnlOptions;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnChooseImage;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnUploadImage;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblRed;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider sliderRed;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblGreen;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider sliderGreen;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblBlue;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider sliderBlue;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image imgPreview;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image afterImage;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ImageServer;component/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.pnlOptions = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 2:
            this.btnChooseImage = ((System.Windows.Controls.Button)(target));
            
            #line 19 "..\..\..\MainWindow.xaml"
            this.btnChooseImage.Click += new System.Windows.RoutedEventHandler(this.btnChooseImage_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.btnUploadImage = ((System.Windows.Controls.Button)(target));
            
            #line 20 "..\..\..\MainWindow.xaml"
            this.btnUploadImage.Click += new System.Windows.RoutedEventHandler(this.btnUploadImage_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.lblRed = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.sliderRed = ((System.Windows.Controls.Slider)(target));
            
            #line 22 "..\..\..\MainWindow.xaml"
            this.sliderRed.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.sliderRed_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 6:
            this.lblGreen = ((System.Windows.Controls.Label)(target));
            return;
            case 7:
            this.sliderGreen = ((System.Windows.Controls.Slider)(target));
            
            #line 24 "..\..\..\MainWindow.xaml"
            this.sliderGreen.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.sliderGreen_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 8:
            this.lblBlue = ((System.Windows.Controls.Label)(target));
            return;
            case 9:
            this.sliderBlue = ((System.Windows.Controls.Slider)(target));
            
            #line 26 "..\..\..\MainWindow.xaml"
            this.sliderBlue.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.sliderBlue_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 10:
            this.imgPreview = ((System.Windows.Controls.Image)(target));
            return;
            case 11:
            this.afterImage = ((System.Windows.Controls.Image)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

