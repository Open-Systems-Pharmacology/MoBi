using System;
using MoBi.Assets;
using MoBi.Core.Domain.Services;
using MoBi.Core.Events;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ObservedData;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Presenters.SensitivityAnalyses;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace MoBi.Presentation
{
   public interface IMoBiApplicationController : IApplicationController
   {
      void Select(IObjectBase objectToSelect, IObjectBase projectItem, ICommandCollector commandCollector);
      IModalPresenter GetCreateViewFor<T>(T objectToEdit, ICommandCollector commandCollector);
      IModalPresenter GetCreateParameterViewFor<T, TParent>(T objectToEdit, TParent parent, ICommandCollector commandCollector);
      IModalPresenter GetCreateViewForTransport<TPresenter>(ITransportBuilder transportBuilder, ICommandCollector commandCollector) where TPresenter : ICommandCollectorPresenter;
   }

   public class MoBiApplicationController : ApplicationController, IMoBiApplicationController
   {
      private readonly IContainer _container;
      private readonly IEventPublisher _eventPublisher;
      private readonly IObjectTypeResolver _objectTypeResolver;
      private readonly IRegisterTask _registerTask;

      public MoBiApplicationController(IContainer container, IEventPublisher eventPublisher, IObjectTypeResolver objectTypeResolver, IRegisterTask registerTask)
         : base(container, eventPublisher)
      {
         _container = container;
         _eventPublisher = eventPublisher;
         _objectTypeResolver = objectTypeResolver;
         _registerTask = registerTask;
      }

      public IModalPresenter GetCreateViewFor<T>(T objectToEdit, ICommandCollector commandCollector)
      {
         var modalPresenter = createModalPresenter(Start<ICreatePresenter<T>>(), commandCollector);
         modalPresenter.Text = AppConstants.Captions.NewWindow(_objectTypeResolver.TypeFor<T>());
         return modalPresenter;
      }

      public IModalPresenter GetCreateViewForTransport<TPresenter>(ITransportBuilder transportBuilder, ICommandCollector commandCollector) where TPresenter : ICommandCollectorPresenter
      {
         return createModalPresenter(Start<TPresenter>(), commandCollector);
      }

      private IModalPresenter createModalPresenter(ICommandCollectorPresenter editSubPresenter, ICommandCollector commandCollector)
      {
         var modalPresenter = _container.Resolve<IModalPresenter>();
         editSubPresenter.InitializeWith(commandCollector);
         modalPresenter.Encapsulate(editSubPresenter);
         return modalPresenter;
      }

      public IModalPresenter GetCreateParameterViewFor<T, TParent>(T objectToEdit, TParent parent, ICommandCollector commandCollector)
      {
         var modalPresenter = createModalPresenter(Start<IEditParameterPresenter>(), commandCollector);
         modalPresenter.Text = AppConstants.Captions.NewWindow(_objectTypeResolver.TypeFor<T>());
         return modalPresenter;
      }

      public override ISingleStartPresenter Open<TSubject>(TSubject subject, ICommandCollector commandCollector)
      {
         var presenter = base.Open(subject, commandCollector);
         var withId = subject as IWithId;
         if (withId != null)
            _registerTask.RegisterAllIn(withId);

         return presenter;
      }

      public void Select(IObjectBase objectToSelect, IObjectBase projectItem, ICommandCollector commandCollector)
      {
         if (objectToSelect == null || projectItem == null) return;
         var presenter = Open(projectItem, commandCollector);
         presenter.Edit(projectItem);
         _eventPublisher.PublishEvent(new EntitySelectedEvent(objectToSelect, this));
      }

      protected override ISingleStartPresenter CreatePresenterForSubject<TSubject>(TSubject subject)
      {
         if (subject.IsAnImplementationOf<IMoleculeBuildingBlock>())
            return Start<IEditMoleculeBuildingBlockPresenter>();

         if (subject.IsAnImplementationOf<IReactionBuildingBlock>())
            return Start<IEditReactionBuildingBlockPresenter>();

         if (subject.IsAnImplementationOf<ISpatialStructure>())
            return Start<IEditSpatialStructurePresenter>();

         if (subject.IsAnImplementationOf<IPassiveTransportBuildingBlock>())
            return Start<IEditPassiveTransportBuildingBlockPresenter>();

         if (subject.IsAnImplementationOf<IEventGroupBuildingBlock>())
            return Start<IEditEventGroupBuildingBlockPresenter>();

         if (subject.IsAnImplementationOf<IMoleculeStartValuesBuildingBlock>())
            return Start<IEditMoleculeStartValuesPresenter>();

         if (subject.IsAnImplementationOf<IParameterStartValuesBuildingBlock>())
            return Start<IEditParameterStartValuesPresenter>();

         if (subject.IsAnImplementationOf<IObserverBuildingBlock>())
            return Start<IEditObserverBuildingBlockPresenter>();

         if (subject.IsAnImplementationOf<IModelCoreSimulation>())
            return Start<IEditSimulationPresenter>();

         if (subject.IsAnImplementationOf<ISimulationSettings>())
            return Start<IEditSimulationSettingsPresenter>();

         if (subject.IsAnImplementationOf<CurveChart>())
            return Start<IProjectChartPresenter>();

         if (subject.IsAnImplementationOf<DataRepository>())
            return Start<IEditDataRepositoryPresenter>();

         if (subject.IsAnImplementationOf<ParameterIdentification>())
            return Start<IEditParameterIdentificationPresenter>();

         if (subject.IsAnImplementationOf<SensitivityAnalysis>())
            return Start<IEditSensitivityAnalysisPresenter>();

         if (subject.IsAnImplementationOf<ParameterIdentificationFeedback>())
            return Start<IParameterIdentificationFeedbackPresenter>();

         if (subject.IsAnImplementationOf<ExpressionProfileBuildingBlock>())
            return Start<IEditExpressionProfileBuildingBlockPresenter>();

         throw new ArgumentException(AppConstants.Exceptions.UnknownProjectItem(subject.GetType()));
      }
   }
}