﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SLStudio.Core.Modules.Shell.Resources {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.4.0.0")]
    internal sealed partial class ShellSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static ShellSettings defaultInstance = ((ShellSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new ShellSettings())));
        
        public static ShellSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1")]
        public string Test {
            get {
                return ((string)(this["Test"]));
            }
            set {
                this["Test"] = value;
            }
        }
    }
}