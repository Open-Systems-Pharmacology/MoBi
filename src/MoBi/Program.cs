using System;
using System.Windows.Forms;
using DevExpress.LookAndFeel;
using DevExpress.XtraEditors;
using MoBi.Engine;
using MoBi.UI.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Container;

namespace MoBi
{
   internal static class Program
   {
      /// <summary>
      ///    The main entry point for the application.
      /// </summary>
      [STAThread]
      private static void Main(string[] args)
      {
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);

         WindowsFormsSettings.SetDPIAware();
         WindowsFormsSettings.SetPerMonitorDpiAware();
         WindowsFormsSettings.TouchUIMode = TouchUIMode.False;
         try
         {
            ApplicationStartup.Initialize(registrationAction);
            IoC.Container.Register<MoBiApplication, MoBiApplication>(LifeStyle.Singleton);
            IoC.Resolve<MoBiApplication>().Run(args);
         }
         catch (Exception e)
         {
            MessageBox.Show(e.ExceptionMessageWithStackTrace(), "Unhandled Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private static void registrationAction(IContainer container)
      {
         container.AddRegister(x => x.FromType<EngineRegister>());
      }
   }
}