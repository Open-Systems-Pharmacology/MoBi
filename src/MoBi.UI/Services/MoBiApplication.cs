using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;
using MoBi.Presentation;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Presenters.Main;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;

namespace MoBi.UI.Services
{
   public class MoBiApplication : WindowsFormsApplicationBase, IMessageFilter
   {
      private readonly ApplicationStartup _applicationStartup;
      private readonly StartOptions _startOptions;
      private IMoBiMainViewPresenter _mainPresenter;

      public MoBiApplication(ApplicationStartup applicationStartup, StartOptions startOptions)
      {
         _applicationStartup = applicationStartup;
         _startOptions = startOptions;
      }

      protected override bool OnInitialize(ReadOnlyCollection<string> commandLineArgs)
      {
         _startOptions.InitializeFrom(commandLineArgs.ToArray());
         return base.OnInitialize(commandLineArgs);
      }

      protected override void OnCreateSplashScreen()
      {
         var splashPresenter = IoC.Resolve<ISplashScreenPresenter>();
         SplashScreen = splashPresenter.View.DowncastTo<Form>();
      }

      protected override void OnCreateMainForm()
      {
         _mainPresenter = IoC.Resolve<IMainViewPresenter>().DowncastTo<IMoBiMainViewPresenter>();
         Application.AddMessageFilter(this);
         _mainPresenter.Initialize();
         _mainPresenter.Run(_startOptions);
         MainForm = _mainPresenter.BaseView.DowncastTo<Form>();
      }

      protected override void OnRun()
      {
         try
         {
            _applicationStartup.Start();
            Application.DoEvents();
            base.OnRun();
         }
         catch (Exception)
         {
            HideSplashScreen();
            throw;
         }
      }

      public bool PreFilterMessage(ref Message m)
      {
         if (!isWindowsKeyDownMessage(m))
            return false;

         switch (keyCodeFrom(m))
         {
            case (int)(Keys.Control | Keys.W):
               _mainPresenter.ActivePresenter?.Close();
               return true;
         }
         return false;
      }

      private static int keyCodeFrom(Message m)
      {
         return (int)m.WParam | (int)Control.ModifierKeys;
      }

      private static bool isWindowsKeyDownMessage(Message m)
      {
         return m.Msg == 256;
      }
   }
}