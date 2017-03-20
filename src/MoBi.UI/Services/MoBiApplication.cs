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
   public class MoBiApplication : WindowsFormsApplicationBase
   {
      private readonly ApplicationStartup _applicationStartup;
      private readonly StartOptions _startOptions;

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
         var splahPresenter = IoC.Resolve<ISplashScreenPresenter>();
         SplashScreen = splahPresenter.View.DowncastTo<Form>();
      }

      protected override void OnCreateMainForm()
      {
         var mainPresenter = IoC.Resolve<IMainViewPresenter>().DowncastTo<IMoBiMainViewPresenter>();
         mainPresenter.Initialize();
         mainPresenter.Run(_startOptions);
         MainForm = mainPresenter.BaseView.DowncastTo<Form>();
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
   }
}