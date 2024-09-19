using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Core.Serialization.Xml.Services;
using MoBi.Core.Services;
using MoBi.Presentation.Core;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.MenusAndBars.ContextMenus;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Presenter.BaseDiagram;
using MoBi.Presentation.Presenter.Main;
using MoBi.Presentation.Presenter.ModelDiagram;
using MoBi.Presentation.Presenter.SpaceDiagram;
using MoBi.Presentation.Serialization.Xml;
using MoBi.Presentation.Serialization.Xml.Serializer;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.UICommand;
using OSPSuite.Assets;
using OSPSuite.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.Comparisons;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Presenters.Diagram;
using OSPSuite.Presentation.Presenters.Journal;
using OSPSuite.Presentation.Presenters.Main;
using OSPSuite.Presentation.Repositories;
using OSPSuite.Presentation.Services;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Container.Conventions;
using OSPSuite.Utility.Extensions;
using IApplicationSettings = MoBi.Core.IApplicationSettings;
using IContainer = OSPSuite.Utility.Container.IContainer;
using JournalDiagramMainPresenter = MoBi.Presentation.Presenter.Main.JournalDiagramMainPresenter;
using JournalPresenter = MoBi.Presentation.Presenter.Main.JournalPresenter;
using MainComparisonPresenter = MoBi.Presentation.Presenter.Main.MainComparisonPresenter;
using MenuBarItemRepository = MoBi.Presentation.Repositories.MenuBarItemRepository;
using ObservedDataTask = MoBi.Presentation.Tasks.ObservedDataTask;

namespace MoBi.Presentation
{
   public class PresentationRegister : Register
   {
      private readonly bool _registerMainViewPresenter;

      public PresentationRegister() : this(true)
      {
      }

      public PresentationRegister(bool registerMainViewPresenter)
      {
         _registerMainViewPresenter = registerMainViewPresenter;
      }

      public override void RegisterInContainer(IContainer container)
      {
         container.AddScanner(scan =>
         {
            scan.AssemblyContainingType<PresentationRegister>();

            scan.ExcludeNamespaceContainingType<IMenuAndToolBarPresenter>();
            scan.ExcludeNamespaceContainingType<ContextMenuBase>();
            scan.ExcludeNamespaceContainingType<IExitCommand>();

            scan.ExcludeType<PKSimStarter>();
            scan.ExcludeType<MenuBarItemRepository>();
            scan.ExcludeType<SimulationRunner>();
            scan.ExcludeType<MoBiApplicationController>();
            scan.ExcludeType<MoBiXmlSerializerRepository>();
            scan.ExcludeType<MoBiMainViewPresenter>();
            scan.Exclude(t => t.IsAnImplementationOf<IMoBiBaseDiagramPresenter>());

            //exclude presenter already registered at startup
            scan.ExcludeType<SplashScreenPresenter>();

            scan.WithConvention<OSPSuiteRegistrationConvention>();
         });

         container.Register<IPKSimStarter, PKSimStarter>(LifeStyle.Singleton);
         container.Register<IMenuBarItemRepository, MenuBarItemRepository>(LifeStyle.Singleton);
         container.Register<ISimulationRunner, SimulationRunner>(LifeStyle.Singleton);
         container.Register<IMoBiApplicationController, IApplicationController, MoBiApplicationController>(LifeStyle.Singleton);

         registerMainPresenters(container);
         registerSingleStartPresenters(container);
         registerCreatePresenter(container);
         registerContextMenuAndCommands(container);
         registerDiagramPresenter(container);

         //selection presenter
         container.Register(typeof(IDescriptorConditionListPresenter<>), typeof(DescriptorConditionListPresenter<>));
         container.Register<ISelectManyPresenter<XElement>, SelectXmlElementPresenter>();
         container.Register<ISelectManyPresenter<OSPSuite.Core.Domain.IContainer>, SelectManyObjectBasePresenter<OSPSuite.Core.Domain.IContainer>>();
         container.Register<ISelectManyPresenter<EventGroupBuilder>, SelectManyObjectBasePresenter<EventGroupBuilder>>();
         container.Register(typeof(ISelectSinglePresenter<>), typeof(SelectSingleObjectBasePresenter<>));

         container.Register<ISettingsPersistor<IUserSettings>, UserSettingsPersistor>();
         container.Register<ISettingsPersistor<IApplicationSettings>, ApplicationSettingsPersistor>();

         container.Register(typeof(IItemToListItemMapper<>), typeof(ItemToListItemMapper<>));
         container.RegisterFactory<IHeavyWorkPresenterFactory>();

         container.Register<IObservedDataConfiguration, ObservedDataTask>();
         container.Register<IPathToPathElementsMapper, PathToPathElementsMapper>();
         container.Register<IDataColumnToPathElementsMapper, DataColumnToPathElementsMapper>();
         container.Register<IDisplayNameProvider, DisplayNameProvider>();
         container.Register<IRenameObjectDTOFactory, RenameObjectDTOFactory>();

         registerTasks(container);
         registerContextMenus(container);

         ApplicationIcons.DefaultIcon = ApplicationIcons.MoBi;

         //Create one instance of the invoker so that the object is available
         //since it is not created anywhere and is only used as event listener
         container.RegisterImplementationOf(container.Resolve<ICloseSubjectPresenterInvoker>());
         container.Register<IWithWorkspaceLayout, Workspace>(LifeStyle.Singleton);

         container.Register<IQuantityPathToQuantityDisplayPathMapper, MoBiQuantityPathToQuantityDisplayPathMapper>();

         container.Register<IMoBiXmlSerializerRepository, MoBiXmlSerializerRepository>(LifeStyle.Singleton);
      }

      private void registerDiagramPresenter(IContainer container)
      {
         container.Register<ISimulationDiagramPresenter, IMoBiBaseDiagramPresenter<IMoBiSimulation>, IBaseDiagramPresenter<IMoBiSimulation>, SimulationDiagramPresenter>(LifeStyle.Transient);
         container.Register<ISpatialStructureDiagramPresenter, IMoBiBaseDiagramPresenter<MoBiSpatialStructure>, IBaseDiagramPresenter<MoBiSpatialStructure>, SpatialStructureDiagramPresenter>(LifeStyle.Transient);
      }

      private static void registerContextMenuAndCommands(IContainer container)
      {
         container.AddScanner(scan =>
         {
            scan.AssemblyContainingType<PresentationRegister>();
            scan.IncludeNamespaceContainingType<ContextMenuBase>();
            scan.IncludeNamespaceContainingType<IExitCommand>();
            scan.ExcludeType(typeof(ContextMenuFactoryForBuildingBlock<>));
            scan.WithConvention<AllInterfacesAndConcreteTypeRegistrationConvention>();
         });
      }

      private void registerMainPresenters(IContainer container)
      {
         if (_registerMainViewPresenter)
            container.Register<IMainViewPresenter, IChangePropagator, MoBiMainViewPresenter>(LifeStyle.Singleton);

         container.Register<IModuleExplorerPresenter, IMainViewItemPresenter, ModuleExplorerPresenter>(LifeStyle.Singleton);
         container.Register<IHistoryPresenter, IMainViewItemPresenter, HistoryPresenter>(LifeStyle.Singleton);
         container.Register<IJournalDiagramMainPresenter, IMainViewItemPresenter, JournalDiagramMainPresenter>(LifeStyle.Singleton);
         container.Register<IJournalPresenter, IMainViewItemPresenter, JournalPresenter>(LifeStyle.Singleton);
         container.Register<IMenuAndToolBarPresenter, IMainViewItemPresenter, MenuAndToolBarPresenter>(LifeStyle.Singleton);
         container.Register<ISimulationExplorerPresenter, IMainViewItemPresenter, SimulationExplorerPresenter>(LifeStyle.Singleton);
         container.Register<IStatusBarPresenter, IMainViewItemPresenter, StatusBarPresenter>(LifeStyle.Singleton);
         container.Register<IMainComparisonPresenter, IMainViewItemPresenter, MainComparisonPresenter>(LifeStyle.Singleton);
         container.Register<ISearchPresenter, IMainViewItemPresenter, SearchPresenter>(LifeStyle.Singleton);
         container.Register<INotificationPresenter, IMainViewItemPresenter, NotificationPresenter>(LifeStyle.Singleton);
      }

      private static void registerSingleStartPresenters(IContainer container)
      {
         container.AddScanner(scan =>
         {
            scan.AssemblyContainingType<PresentationRegister>();
            scan.IncludeNamespaceContainingType<AboutPresenter>();
            scan.WithConvention(new RegisterTypeConvention<ISingleStartPresenter>(registerWithDefaultConvention: false));
         });
      }

      private static void registerCreatePresenter(IContainer container)
      {
         container.AddScanner(scan =>
         {
            scan.AssemblyContainingType<PresentationRegister>();
            scan.IncludeNamespaceContainingType<AboutPresenter>();
            scan.WithConvention<RegisterCreateAndEditPresenterConvention>();
         });
      }

      private static void registerTasks(IContainer container)
      {
         container.AddScanner(scan =>
         {
            scan.AssemblyContainingType<InteractionTaskConvention>();
            scan.IncludeNamespaceContainingType<InteractionTaskConvention>();
            scan.WithConvention<InteractionTaskConvention>();
            scan.ExcludeType(typeof(InitialConditionsTask<>));
         });
         container.Register<IInteractionTasksForChildren<Module, InitialConditionsBuildingBlock>, InitialConditionsTask<InitialConditionsBuildingBlock>>();
        
         container.Register<IEditTaskFor<InteractionContainer>, EditTaskForInteractionContainer>();
         container.Register<IEditTaskFor<MoleculeBuilder>, EditTasksForMoleculeBuilder>();
         container.Register<IEditTaskFor<ReactionBuilder>, EditTasksForReactionBuilder>();
         container.Register<IEditTaskFor<TransportBuilder>, EditTasksForTransportBuilder>();
         container.Register<IEditTaskFor<EventGroupBuilder>, EditTasksForEventGroupBuilder>();
         container.Register<IEditTaskFor<ApplicationBuilder>, EditTasksForApplicationBuilder>();
         container.Register<IEditTaskFor<ContainerObserverBuilder>, EditTasksForContainerObserverBuilder>();
         container.Register<IEditTaskFor<AmountObserverBuilder>, EditTasksForAmountObserverBuilder>();
         container.Register<IEditTaskFor<OSPSuite.Core.Domain.IContainer>, EditTaskForContainer>();
         container.Register<IEditTaskFor<IDistributedParameter>, EditTasksForDistributedParameter>();
         container.Register<IEditTaskFor<IParameter>, EditTasksForParameter>();
         container.Register<IEditTaskFor<EventBuilder>, EditTaskForEventBuilder>();
         container.Register<IEditTaskFor<TransporterMoleculeContainer>, EditTasksForTransporterMoleculeContainer>();
         container.Register<IEditTaskFor<ApplicationMoleculeBuilder>, EditTaskForApplicationMoleculeBuilder>();
         container.Register<IEditTaskFor<EventAssignmentBuilder>, EditTaskForEventAssignmentBuilder>();
         container.Register<IEditTaskFor<NeighborhoodBuilder>, EditTaskForNeighborhoodBuilder>();
         container.Register<IEditTaskFor<IMoBiSimulation>, EditTasksForSimulation>();
         container.Register<IEditTaskFor<MoBiSpatialStructure>, EditTasksForSpatialStructure>();
         container.Register<IEditTaskFor<ExpressionProfileBuildingBlock>, EditTasksForExpressionProfileBuildingBlock>();
         container.Register<IEditTaskFor<IndividualBuildingBlock>, EditTasksForIndividualBuildingBlock>();
         container.Register<IEditTaskFor<ParameterValuesBuildingBlock>, EditTasksForParameterValuesBuildingBlock>();
         container.Register<IEditTaskFor<InitialConditionsBuildingBlock>, EditTasksForInitialConditionsBuildingBlock>();
         container.Register<IEditTaskFor<Module>, EditTaskForModule>();
         container.Register(typeof(IEditTasksForBuildingBlock<>), typeof(EditTasksForBuildingBlock<>));
         container.Register(typeof(IEditTaskFor<>), typeof(EditTasksForBuildingBlock<>));
         container.Register(typeof(IInitialConditionsTask<>), typeof(InitialConditionsTask<>));
      }

      private void registerContextMenus(IContainer container)
      {
         //Generic context menus
         container.Register<IRootContextMenuFor<MoBiProject, IMoBiSimulation>, RootContextMenuForSimulation>();
         container.Register<IRootContextMenuFor<MoBiProject, ParameterIdentification>, RootContextMenuForParameterIdentification>();
         container.Register<IRootContextMenuFor<MoBiProject, SensitivityAnalysis>, RootContextMenuForSensitivityAnalysis>();
         container.Register<IContextMenuFor<IMoBiSimulation>, ContextMenuForSimulation>();
         container.Register<IContextMenuForBuildingBlock<ParameterValuesBuildingBlock>, ContextMenuForParameterValuesBuildingBlock>();
         container.Register<IContextMenuForBuildingBlock<InitialConditionsBuildingBlock>, ContextMenuForInitialConditionsBuildingBlock>();
         container.Register<IContextMenuForBuildingBlock<ExpressionProfileBuildingBlock>, ContextMenuForExpressionProfileBuildingBlock>();
         container.Register<IContextMenuForBuildingBlock<IndividualBuildingBlock>, ContextMenuForIndividualBuildingBlock>();
         container.Register<IContextMenuForBuildingBlock<MoBiReactionBuildingBlock>, ContextMenuForModuleBuildingBlock<MoBiReactionBuildingBlock>>();
         container.Register<IContextMenuForBuildingBlock<ObserverBuildingBlock>, ContextMenuForModuleBuildingBlock<ObserverBuildingBlock>>();
         container.Register<IContextMenuForBuildingBlock<PassiveTransportBuildingBlock>, ContextMenuForModuleBuildingBlock<PassiveTransportBuildingBlock>>();
         container.Register<IContextMenuForBuildingBlock<MoleculeBuildingBlock>, ContextMenuForModuleBuildingBlock<MoleculeBuildingBlock>>();
         container.Register<IContextMenuForBuildingBlock<EventGroupBuildingBlock>, ContextMenuForModuleBuildingBlock<EventGroupBuildingBlock>>();
         container.Register<IContextMenuForBuildingBlock<MoBiSpatialStructure>, ContextMenuForModuleBuildingBlock<MoBiSpatialStructure>>();

         container.Register(typeof(IContextMenuForBuildingBlock<>), typeof(ContextMenuForModuleBuildingBlock<>));
         container.Register(typeof(IContextMenuFor<>), typeof(ContextMenuFor<>));
         container.Register(typeof(IRootContextMenuFor<,>), typeof(RootContextMenuFor<,>));

         //Generic context menu factory: One for each building block type
         registerContextMenuForBuildingBlockFactory<MoleculeBuildingBlock>(container);
         registerContextMenuForBuildingBlockFactory<MoBiReactionBuildingBlock>(container);
         registerContextMenuForBuildingBlockFactory<PassiveTransportBuildingBlock>(container);
         registerContextMenuForBuildingBlockFactory<MoBiSpatialStructure>(container);
         registerContextMenuForBuildingBlockFactory<ObserverBuildingBlock>(container);
         registerContextMenuForBuildingBlockFactory<EventGroupBuildingBlock>(container);
         registerContextMenuForBuildingBlockFactory<InitialConditionsBuildingBlock>(container);
         registerContextMenuForBuildingBlockFactory<ExpressionProfileBuildingBlock>(container);
         registerContextMenuForBuildingBlockFactory<IndividualBuildingBlock>(container);
         registerContextMenuForBuildingBlockFactory<ParameterValuesBuildingBlock>(container);
         registerContextMenuForBuildingBlockFactory<SimulationSettings>(container);
      }

      private void registerContextMenuForBuildingBlockFactory<TBuildingBlock>(IContainer container) where TBuildingBlock : IBuildingBlock
      {
         container.Register<IContextMenuSpecificationFactory<IViewItem>, ContextMenuFactoryForBuildingBlock<TBuildingBlock>>();
      }

      public class RegisterCreateAndEditPresenterConvention : IRegistrationConvention
      {
         public void Process(Type concreteType, IContainer container, LifeStyle lifeStyle)
         {
            var services = new List<Type>();
            var createTypes = concreteType.GetDeclaredTypesForGeneric(typeof(ICreatePresenter<>));
            services.AddRange(createTypes.Select(x => x.GenericType));

            if (!services.Any())
               return;

            container.Register(services, concreteType, lifeStyle);
         }
      }
   }
}