using System;
using System.Windows.Forms;
using MoBi.UI.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Services;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;

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
         try
         {
            ApplicationStartup.Initialize();
            IoC.Container.Register<MoBiApplication, MoBiApplication>(LifeStyle.Singleton);
            IoC.Resolve<MoBiApplication>().Run(args);
         }
         catch (Exception e)
         {
            MessageBox.Show(e.ExceptionMessageWithStackTrace(), "Unhandled Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }
   }
}