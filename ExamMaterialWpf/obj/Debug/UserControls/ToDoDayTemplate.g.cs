﻿#pragma checksum "..\..\..\UserControls\ToDoDayTemplate.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "36D17B73C8B82D40C6A8EFCBE72AE29E"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using ExamMaterialWpf.UserControls;
using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Transitions;
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


namespace ExamMaterialWpf.UserControls {
    
    
    /// <summary>
    /// ToDoDayTemplate
    /// </summary>
    public partial class ToDoDayTemplate : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 15 "..\..\..\UserControls\ToDoDayTemplate.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox FruitTextBox;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\UserControls\ToDoDayTemplate.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbToDoDate;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\UserControls\ToDoDayTemplate.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbToDoId;
        
        #line default
        #line hidden
        
        
        #line 59 "..\..\..\UserControls\ToDoDayTemplate.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox FruitListBox;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\..\UserControls\ToDoDayTemplate.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbToDoDateName;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\..\UserControls\ToDoDayTemplate.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button bDelToDoItem;
        
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
            System.Uri resourceLocater = new System.Uri("/ExamMaterialWpf;component/usercontrols/tododaytemplate.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\UserControls\ToDoDayTemplate.xaml"
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
            this.FruitTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            
            #line 17 "..\..\..\UserControls\ToDoDayTemplate.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.tbToDoDate = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.tbToDoId = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.FruitListBox = ((System.Windows.Controls.ListBox)(target));
            return;
            case 6:
            this.tbToDoDateName = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 7:
            this.bDelToDoItem = ((System.Windows.Controls.Button)(target));
            
            #line 74 "..\..\..\UserControls\ToDoDayTemplate.xaml"
            this.bDelToDoItem.Click += new System.Windows.RoutedEventHandler(this.bDelToDoItem_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

