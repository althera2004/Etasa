﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EtasaDesktop.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.7.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("ArAf_79diFW_Em1qV8UQ3LJRiSlqMdSGwdTpDU9UojOMAg1h83RYPEv793D5iZWQ")]
        public string MapsCredentials {
            get {
                return ((string)(this["MapsCredentials"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::EtasaDesktop.Common.Auth.Session Session {
            get {
                return ((global::EtasaDesktop.Common.Auth.Session)(this["Session"]));
            }
            set {
                this["Session"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=10.0.0.52;Initial Catalog=Etasa;User ID=etasauser;Password=Et@sa2018!" +
            ";Connect Timeout=30;Encrypt=False;TrustServerCertificate=False")]
        public string EtasaConnectionString {
            get {
                return ((string)(this["EtasaConnectionString"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::EtasaDesktop.Distribution.Orders.Imports.ImportConfiguration ImportGalpConfig {
            get {
                return ((global::EtasaDesktop.Distribution.Orders.Imports.ImportConfiguration)(this["ImportGalpConfig"]));
            }
            set {
                this["ImportGalpConfig"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::EtasaDesktop.Distribution.Orders.Imports.ImportConfiguration ImportVitogasConfig {
            get {
                return ((global::EtasaDesktop.Distribution.Orders.Imports.ImportConfiguration)(this["ImportVitogasConfig"]));
            }
            set {
                this["ImportVitogasConfig"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::EtasaDesktop.Distribution.Orders.Imports.ImportConfiguration ImportCepsaConfig {
            get {
                return ((global::EtasaDesktop.Distribution.Orders.Imports.ImportConfiguration)(this["ImportCepsaConfig"]));
            }
            set {
                this["ImportCepsaConfig"] = value;
            }
        }
    }
}
