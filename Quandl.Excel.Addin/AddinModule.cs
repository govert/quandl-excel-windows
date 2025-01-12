﻿using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Windows.Forms;
using AddinExpress.MSO;
using MicrosoftExcel = Microsoft.Office.Interop.Excel;

namespace Quandl.Excel.Addin
{
    /// <summary>
    ///   Add-in Express Add-in Module
    /// </summary>
    [GuidAttribute("B521C90D-9652-4074-A2E4-A4D088A2ED4A"), 
     ProgId("QuandlExcelAddin2.AddinModule")]
    [ComVisible(true)]
    public partial class AddinModule : AddinExpress.MSO.ADXAddinModule
    {
        public AddinModule()
        {
            Application.EnableVisualStyles();
            InitializeComponent();
            // Please add any initialization code to the AddinInitialize event handler
        }
 
        #region Add-in Express automatic code
 
        // Required by Add-in Express - do not modify
        // the methods within this region
 
        public override System.ComponentModel.IContainer GetContainer()
        {
            if (components == null)
                components = new System.ComponentModel.Container();
            return components;
        }
 
        [ComRegisterFunctionAttribute]
        public static void AddinRegister(Type t)
        {
            AddinExpress.MSO.ADXAddinModule.ADXRegister(t);
        }
 
        [ComUnregisterFunctionAttribute]
        public static void AddinUnregister(Type t)
        {
            AddinExpress.MSO.ADXAddinModule.ADXUnregister(t);
        }
 
        public override void UninstallControls()
        {
            base.UninstallControls();
        }

        #endregion

        public static new AddinModule CurrentInstance 
        {
            get
            {
                return AddinExpress.MSO.ADXAddinModule.CurrentInstance as AddinModule;
            }
        }

        private MainLogic logic;

        internal MainLogic Logic
        {
            get { return logic; }
        }
        public MicrosoftExcel._Application ExcelApp
        {
            get
            {
                return (HostApplication as MicrosoftExcel._Application);
            }
        }

        private void adxRibbonTabQuandlUdfBuilder_OnClick(object sender, IRibbonControl control, bool pressed)
        {
            //udfBuilder_Click
            this.logic.TaskPaneUpdater.Show<UI.WizardGuideControlHost>(control.Context, h=>h.Reset());
        }

        internal static IWin32Window GetWindowOwner(object controlContext)
        {
            IntPtr hwnd = IntPtr.Zero;
            if (CurrentInstance.HostMajorVersion > 14) // Excel 2013
            {
                var wnd = controlContext as MicrosoftExcel.Window;
                if (wnd != null)
                {
                    try
                    {
                        hwnd = new IntPtr((int) ((dynamic) wnd).Hwnd);
                    }
                    catch // ignore
                    {
                    }
                }

               
            }
            if (CurrentInstance.HostMajorVersion > 14 && hwnd == IntPtr.Zero)
            {
                try
                {
                    hwnd = new IntPtr(CurrentInstance.ExcelApp.Hwnd);
                }
                catch // ignore
                {
                }
            }

            if (hwnd == IntPtr.Zero)
            {
                hwnd = NativeMethods.GetActiveWindow();
                // make sure it belongs to the correct process
                uint processId;
                if (NativeMethods.GetWindowThreadProcessId(hwnd, out processId) == 0)
                {
                    processId = 0;
                }

                if (processId == 0)
                {
                    hwnd = IntPtr.Zero;
                }
                else
                {
                    using (var p = System.Diagnostics.Process.GetCurrentProcess())
                    {
                        if (p.Id != processId)
                        {
                            hwnd = IntPtr.Zero;
                        }
                    }
                }
            }

            if (hwnd == IntPtr.Zero)
            {
                hwnd = CurrentInstance.NativeWindowHandle;
            }

            return new WindowWrapper(hwnd);

        }

        class WindowWrapper : IWin32Window
        {
            private readonly IntPtr href;

            public WindowWrapper(IntPtr handle)
            {
                href = handle;
            }

            public IntPtr Handle
            {
                get
                {
                    return href;
                }
            }
        }
        private void adxRibbonTabQuandlRefreshSheet_OnClick(object sender, IRibbonControl control, bool pressed)
        {
            object worksheet = null;

            try
            {
                worksheet = this.ExcelApp.ActiveSheet;
                var comSheet = worksheet as MicrosoftExcel.Worksheet;
                if (comSheet != null)
                {
                    FunctionUpdater.RecalculateQuandlFunctions(comSheet);
                }
            }
            catch (System.Exception ex)
            {
                logic.UpdateStatusBar(ex);
            }
            finally
            {
                if (worksheet != null)
                {
                    Marshal.ReleaseComObject(worksheet);
                }
            }
        }

        private void adxRibbonTabQuandlRefreshWorkbook_OnClick(object sender, IRibbonControl control, bool pressed)
        {
            //btnRefreshWorkbook_Click

            MicrosoftExcel.Workbook workbook = null;

            try
            {
                workbook = this.ExcelApp.ActiveWorkbook;
                if (workbook != null)
                {
                    FunctionUpdater.RecalculateQuandlFunctions(workbook);
                }
            }
            catch (System.Exception ex)
            {
                logic.UpdateStatusBar(ex);
            }
            finally
            {
                if (workbook != null)
                {
                    Marshal.ReleaseComObject(workbook);
                }
            }
        }

        private void adxRibbonTabQuandlStopAll_OnClick(object sender, IRibbonControl control, bool pressed)
        {
            //btnStopAll_Click
            Shared.QuandlConfig.StopCurrentExecution = true;
        }

        private void adxRibbonTabQuandlFormulaToggleSplitEnable_OnClick(object sender, IRibbonControl control, bool pressed)
        {
            //btnEnableFormula_Click
            Quandl.Shared.QuandlConfig.PreventCurrentExecution = false;
            SetExecutionToggleIcon();
        }

        private void adxRibbonTabQuandlFormulaToggleSplitDisable_OnClick(object sender, IRibbonControl control, bool pressed)
        {
            //btnDisableFormula_Click
            Quandl.Shared.QuandlConfig.PreventCurrentExecution = true;
            SetExecutionToggleIcon();
        }

        internal void SetExecutionToggleIcon()
        {
            if (Shared.QuandlConfig.PreventCurrentExecution)
            {
                adxRibbonTabQuandlFormulaToggleSplit.Image = adxRibbonTabQuandlFormulaToggleSplitDisable.Image;
                adxRibbonTabQuandlFormulaToggleSplitEnable.Enabled = true;
                adxRibbonTabQuandlFormulaToggleSplitDisable.Enabled = false;
            }
            else
            {
                adxRibbonTabQuandlFormulaToggleSplit.Image = adxRibbonTabQuandlFormulaToggleSplitEnable.Image;
                adxRibbonTabQuandlFormulaToggleSplitEnable.Enabled = false;
                adxRibbonTabQuandlFormulaToggleSplitDisable.Enabled = true;
            }
        }
        private void adxRibbonTabQuandlOpenSettings_OnClick(object sender, IRibbonControl control, bool pressed)
        {
            this.logic.TaskPaneUpdater.Show<UI.SettingsControlHost>(control.Context, h=>h.Reset());
        }

        private void adxRibbonTabQuandlAbout_OnClick(object sender, IRibbonControl control, bool pressed)
        {
            this.logic.TaskPaneUpdater.Show<UI.AboutControlHost>(control.Context);
        }
        
        private void adxRibbonTabCheckUpdate_OnClick(object sender, IRibbonControl control, bool pressed)
        {
            //btnCheckUpdate_Click
            this.logic.TaskPaneUpdater.Show<UI.UpdateControlHost>(control.Context, a=>a.UpdateContent());
        }

        private void AddinModule_AddinInitialize(object sender, EventArgs e)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(false);
#endif
            // Note by Stan Ostashewski, 2018/10/29
            // Displaying task panes to the right eliminates visual flickering.
            // Also Excel will display Floating Pane on the primary display only, even if Excel is displayed on another display.
            // In our case it is cleaner to display task panes docked to the right.
            // To enable floating task panes set registry key;
            // refer to Product.wxs for placeholder values:
            // HKEY_CURRENT_USER\Software\[Manufacturer]\[ProductName]
            // EnableFloatingTaskPane = 0x1 (DWORD)
            if (!Quandl.Shared.QuandlConfig.EnableFloatingTaskPane)
            {
                for (var taskPaneIndex = 0; taskPaneIndex < this.TaskPanes.Count; taskPaneIndex++)
                {
                    var taskPane = TaskPanes[taskPaneIndex];
                    // floating position also seems to be provoking initialization error for Excel 2010 and 2016
                    // could have been consequence to WPF loading?
                    if (taskPane.DockPosition == ADXCTPDockPosition.ctpDockPositionFloating)
                    {
                        taskPane.DockPosition = ADXCTPDockPosition.ctpDockPositionRight;
                    }
                }
            }

            logic = new MainLogic();
            logic.OnStart();// starts background initialization
            SetExecutionToggleIcon(); // update the ribbon icon
            logic.TaskPaneUpdater.UpdateTaskPane<UI.AboutControlHost>(adxTaskPaneAbout,false);
            logic.TaskPaneUpdater.UpdateTaskPane<UI.UpdateControlHost>(adxTaskPaneUpdater,false);
            logic.TaskPaneUpdater.UpdateTaskPane<UI.SettingsControlHost>(adxTaskPaneSettings,false);
            logic.TaskPaneUpdater.UpdateTaskPane<UI.WizardGuideControlHost>(adxTaskPaneBuilder,true);
#if DEBUG
            //adxTaskPaneUpdater.ControlProgID = "";
            //adxTaskPaneSettings.ControlProgID = "";
            //adxTaskPaneBuilder.ControlProgID = "";
#endif
        }


        private void AddinModule_AddinStartupComplete(object sender, EventArgs e)
        {
      
            logic.Connect(this.ExcelApp);
            DoCheckUpdate();
        }

        private void adxExcelAppEvents1_SheetSelectionChange(object sender, object sheet, object range)
        {
            this.Logic?.OnSheetSelectionChange();
        }
      
    
        private void timerCheckUI_Tick(object sender, EventArgs e)
        {
            DoCheckUpdate();
        }

        void DoCheckUpdate()
        {
            bool runCheck = true;
            if (logic != null)
            {
                var hasUpdate = logic.UpdateAvailable;
                if (hasUpdate.HasValue)
                {
                    adxRibbonTabCheckUpdate.Visible = hasUpdate.Value;
                    runCheck = false;
                }
            }
            timerCheckUI.Enabled = runCheck;

        }

        private void AddinModule_AddinFinalize(object sender, EventArgs e)
        {
            if (logic != null)
            {
                logic.Dispose();
                logic = null;
            }
        }

        private void AddinModule_AddinBeginShutdown(object sender, EventArgs e)
        {
            if (logic != null)
            {
                logic.Shutdown();
            }
        }

        private void AddinModule_OnTaskPaneAfterCreate(object sender, ADXTaskPane.ADXCustomTaskPaneInstance instance, object control)
        {

            this.Logic.TaskPaneUpdater.SetCustomPanePositionWhenFloating(instance, control);

        }

        private void AddinModule_OnError(ADXErrorEventArgs e)
        {
            System.Diagnostics.Debug.Assert(false);
            System.Diagnostics.Trace.TraceError(e.ADXError.ToString());
            e.Handled = true;
        }

        private void adxExcelAppEvents1_WorkbookOpen(object sender, object hostObj)
        {
            var wb = hostObj as MicrosoftExcel.Workbook;
            if (wb != null)
            {
                this.Logic.OnWorkbookOpen(wb);
            }
        }
    }
}

