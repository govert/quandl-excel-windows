﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Quandl.Excel.Addin.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Code {0} is invalid!")]
        public string DataCodeValidationMessage {
            get {
                return ((string)(this["DataCodeValidationMessage"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Specify how you would like to filter data with any of the options below or none.")]
        public string DatatableFilterNoteOne {
            get {
                return ((string)(this["DatatableFilterNoteOne"]));
            }
            set {
                this["DatatableFilterNoteOne"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("If you skip this step, then you will receive all of the data for the for the colu" +
            "mns you have selected previously.")]
        public string DatatableFilterNoteTow {
            get {
                return ((string)(this["DatatableFilterNoteTow"]));
            }
            set {
                this["DatatableFilterNoteTow"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Select the {0} you want to see the data for.")]
        public string DatatableFilterDateRange {
            get {
                return ((string)(this["DatatableFilterDateRange"]));
            }
            set {
                this["DatatableFilterDateRange"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Type in the {0} that you would like to filter on. Separate each item with a comma" +
            ".")]
        public string DatatableFilterTicker {
            get {
                return ((string)(this["DatatableFilterTicker"]));
            }
            set {
                this["DatatableFilterTicker"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Enter in the {0} that you would like the data for.")]
        public string DatatableFilterFloatNumber {
            get {
                return ((string)(this["DatatableFilterFloatNumber"]));
            }
            set {
                this["DatatableFilterFloatNumber"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Enter in the {0} that you would like the data for.")]
        public string DatatableFilterIntegerNumber {
            get {
                return ((string)(this["DatatableFilterIntegerNumber"]));
            }
            set {
                this["DatatableFilterIntegerNumber"] = value;
            }
        }
    }
}
