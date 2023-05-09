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
         if (subject.IsAnImplementationOf<MoleculeBuildingBlock>())
            return Start<IEditMoleculeBuildingBlockPresenter>();

         if (subject.IsAnImplementationOf<ReactionBuildingBlock>())
            return Start<IEditReactionBuildingBlockPresenter>();

         if (subject.IsAnImplementationOf<SpatialStructure>())
            return Start<IEditSpatialStructurePresenter>();

         if (subject.IsAnImplementationOf<PassiveTransportBuildingBlock>())
            return Start<IEditPassiveTransportBuildingBlockPresenter>();

         if (subject.IsAnImplementationOf<EventGroupBuildingBlock>())
            return Start<IEditEventGroupBuildingBlockPresenter>();

         if (subject.IsAnImplementationOf<InitialConditionsBuildingBlock>())
            return Start<IEditInitialConditionsPresenter>();

         if (subject.IsAnImplementationOf<ParameterValuesBuildingBlock>())
            return Start<IEditParameterValuesPresenter>();

         if (subject.IsAnImplementationOf<ObserverBuildingBlock>())
            return Start<IEditObserverBuildingBlockPresenter>();

         if (subject.IsAnImplementationOf<IModelCoreSimulation>())
            return Start<IEditSimulationPresenter>();

         if (subject.IsAnImplementationOf<SimulationSettings>())
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

         if (subject.IsAnImplementationOf<IndividualBuildingBlock>())
            return Start<IEditIndividualBuildingBlockPresenter>();

         throw new ArgumentException(AppConstants.Exceptions.UnknownProjectItem(subject.GetType()));
      }
   }
}