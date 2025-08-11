using MoBi.Core.Domain.Model;
using MoBi.Presentation.Presenter.ReactionDiagram;
using MoBi.Presentation.UICommand;
using MoBi.Presentation.Views;
using MoBi.UI.Presenters;
using MoBi.UI.Services;
using MoBi.UI.UICommands;
using MoBi.UI.Views;
using OSPSuite.Assets;
using OSPSuite.Core;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Presenters.Diagram;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;

namespace MoBi.UI
{
   public class UserInterfaceRegister : IRegister
   {
      public void RegisterInContainer(IContainer container)
      {
         container.AddScanner(scan =>
            {
               scan.AssemblyContainingType<UserInterfaceRegister>();

               scan.Include(x => x.IsAnImplementationOf<IView>());
               scan.Include(x => x.IsAnImplementationOf<IDiagramManager>());

               scan.ExcludeType<MoBiMainView>();
               scan.ExcludeType<SplashScreen>();

               scan.WithDefaultConvention();
            }
         );

         //Register services
         container.AddScanner(scan =>
         {
            scan.AssemblyContainingType<UserInterfaceRegister>();
            scan.IncludeNamespaceContainingType<DiagramUpdater>();
            scan.WithConvention<OSPSuiteRegistrationConvention>();
         });

         container.Register<ICopyTextCommand, CopyTextCommand>();
         container.Register<IExitCommand, ExitCommand>();
         container.Register<IMoBiMainView, IShell, IMainView, MoBiMainView>(LifeStyle.Singleton);

         container.Register<IContainerModalView, ModalForm>(LifeStyle.Transient);
         container.Register<IReactionDiagramPresenter, IBaseDiagramPresenter<MoBiReactionBuildingBlock>, ReactionDiagramPresenter>(LifeStyle.Transient);

         container.Register(typeof(ISelectManyView<>), typeof(SelectManyView<>));
         container.Register(typeof(ISelectSingleView<>), typeof(SelectSingleView<>));

         var mainView = container.Resolve<IMoBiMainView>();
         var exceptionView = container.Resolve<IExceptionView>();
         exceptionView.MainView = mainView;

         OSPSuite.UI.UIConstants.ICON_SIZE_TAB = IconSizes.Size16x16;
      }
   }
}