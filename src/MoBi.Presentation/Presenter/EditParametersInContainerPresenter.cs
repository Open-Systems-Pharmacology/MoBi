using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Events;
using MoBi.Core.Extensions;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface IEditParametersInContainerPresenter : IParameterPresenter,
      IPresenter<IEditParametersInContainerView>,
      IListener<AddedEvent>,
      IListener<RemovedEvent>,
      IListener<QuantityChangedEvent>,
      IListener<ParameterChangedEvent>
   {
      void AddParameter();
      void RemoveParameter(ParameterDTO parameterDTO);
      void Rename(ParameterDTO parameterDTO);
      void Select(ParameterDTO selectedParameter);
      void CopyToClipBoard(ParameterDTO parameterDTO);
      void CutToClipBoard(ParameterDTO parameterDTO);
      void PasteFromClipBoard();
      void LoadParameter();
      bool ShowAdvancedParameters { get; set; }
      bool GroupParameters { get; set; }
      void Select(IParameter parameter);
      bool ShowBuildMode { set; }
      bool BlackBoxAllowed { set; }
      void Edit(IContainer container);
      bool ChangeLocalisationAllowed { set; get; }
      EditParameterMode EditMode { set; }
      IEnumerable<ParameterBuildMode> ParameterBuildModes { get; set; }
      void SetBuildModeFor(ParameterDTO parameterDTO, ParameterBuildMode newMode);
      void SetIsPersistable(ParameterDTO parameterDTO, bool isPersistable);
      void SetDimensionFor(ParameterDTO parameterDTO, IDimension newDimension);

      /// <summary>
      ///    Enables the Container criteria support for specific use cases
      /// </summary>
      void EnableContainerCriteriaSupport();

      void CopyPathForParameter(ParameterDTO parameter);

      IReadOnlyList<IndividualBuildingBlock> AllIndividuals { get; }
      IndividualBuildingBlock SelectedIndividual { get; set; }
      void ShowIndividualSelection();
      void UpdatePreview();
      void EnableSimulationTracking(TrackableSimulation trackableSimulation);
   }

   public class EditParametersInContainerPresenter : AbstractParameterBasePresenter<IEditParametersInContainerView, IEditParametersInContainerPresenter>, IEditParametersInContainerPresenter
   {
      private readonly IClipboardManager _clipboardManager;
      private readonly IParameterToParameterDTOMapper _parameterToDTOParameterMapper;
      private readonly IIndividualParameterToParameterDTOMapper _individualParameterToDTOParameterMapper;
      private readonly ICache<string, IParameter> _parameters = new Cache<string, IParameter>(x => x.Id, x => null);
      private readonly List<ParameterDTO> _allParametersDTO = new List<ParameterDTO>();

      private IContainer _container;
      private readonly IEditTaskFor<IParameter> _editTask;
      private readonly ISelectReferencePresenterFactory _selectReferencePresenterFactory;
      private readonly IEditDistributedParameterPresenter _editDistributedParameterPresenter;
      private readonly IEditParameterPresenter _editParameterPresenter;
      private readonly IEditIndividualParameterPresenter _editIndividualParameterPresenter;
      private bool _ignoreAddEvents;
      private readonly IObjectTypeResolver _typeResolver;

      private readonly IEntityPathResolver _entityPathResolver;
      private readonly IObjectPathFactory _objectPathFactory;
      private readonly IndividualBuildingBlock _noIndividualSelected;
      private readonly Cache<ParameterDTO, IndividualParameter> _individualParameterCache = new Cache<ParameterDTO, IndividualParameter>(onMissingKey: dto => null);
      private TrackableSimulation _trackableSimulation;

      public bool ChangeLocalisationAllowed { set; get; }

      public EditParametersInContainerPresenter(IEditParametersInContainerView view,
         IFormulaToFormulaBuilderDTOMapper formulaMapper,
         IParameterToParameterDTOMapper parameterToDTOParameterMapper,
         IInteractionTasksForParameter parameterTask,
         IEditDistributedParameterPresenter editDistributedParameterPresenter,
         IEditParameterPresenter editParameterPresenter,
         IQuantityTask quantityTask,
         IInteractionTaskContext interactionTaskContext,
         IClipboardManager clipboardManager,
         IEditTaskFor<IParameter> editTask,
         ISelectReferencePresenterFactory selectReferencePresenterFactory,
         IFavoriteTask favoriteTask,
         IObjectTypeResolver typeResolver,
         IEntityPathResolver entityPathResolver,
         IObjectPathFactory objectPathFactory,
         IIndividualParameterToParameterDTOMapper individualParameterToDTOParameterMapper,
         IEditIndividualParameterPresenter editIndividualParameterPresenter)
         : base(view, quantityTask, interactionTaskContext, formulaMapper, parameterTask, favoriteTask)
      {
         _clipboardManager = clipboardManager;
         _editTask = editTask;
         _selectReferencePresenterFactory = selectReferencePresenterFactory;
         _editDistributedParameterPresenter = editDistributedParameterPresenter;
         _editParameterPresenter = editParameterPresenter;
         _editIndividualParameterPresenter = editIndividualParameterPresenter;
         EditMode = EditParameterMode.All;
         _parameterToDTOParameterMapper = parameterToDTOParameterMapper;
         _editParameterPresenter = editParameterPresenter;
         AddSubPresenters(_editDistributedParameterPresenter, _editParameterPresenter, _editIndividualParameterPresenter);
         ChangeLocalisationAllowed = true;
         _typeResolver = typeResolver;
         _entityPathResolver = entityPathResolver;
         _objectPathFactory = objectPathFactory;
         _individualParameterToDTOParameterMapper = individualParameterToDTOParameterMapper;
         _noIndividualSelected = new IndividualBuildingBlock().WithName(AppConstants.Captions.None);
         SelectedIndividual = _noIndividualSelected;
         _view.ShowIndividualSelection(false);
         AllIndividuals = new List<IndividualBuildingBlock> {_noIndividualSelected}.Concat(_interactionTaskContext.Context.CurrentProject.IndividualsCollection).ToList();
      }

      public IReadOnlyList<IndividualBuildingBlock> AllIndividuals { get; }

      public IndividualBuildingBlock SelectedIndividual { get; set; }

      public void Edit(IContainer container)
      {
         _container = container;
         ValueReference = getNewReferencePresenterFor(container);
         RhsReference = getNewReferencePresenterFor(container);
         ShowBuildMode = container.CanSetBuildModeForParameters();
         ParameterBuildModes = container.AvailableBuildModeForParameters();
         _view.ParentName = getContainerName(container);
         UpdatePreview();
      }

      private IReadOnlyList<IndividualParameter> individualParametersFor(IContainer container, IndividualBuildingBlock selectedIndividual)
      {
         if (Equals(_noIndividualSelected, SelectedIndividual))
            return Array.Empty<IndividualParameter>();

         var targetContainerPath = _objectPathFactory.CreateAbsoluteObjectPath(container);

         return selectedIndividual.Where(x => x.ContainerPath.Equals(targetContainerPath)).ToList();
      }

      public override void ReleaseFrom(IEventPublisher eventPublisher)
      {
         base.ReleaseFrom(eventPublisher);
         releaseParameterDTOs();
      }

      public void SetDimensionFor(ParameterDTO parameterDTO, IDimension newDimension)
      {
         var parameter = ParameterFrom(parameterDTO);
         AddCommand(_parameterTask.SetDimensionForParameter(parameter, newDimension, BuildingBlock));
         refreshViewAndSelect(parameterDTO);
      }

      public void EnableContainerCriteriaSupport() => _editParameterPresenter.EnableContainerCriteriaSupport();

      public void CopyPathForParameter(ParameterDTO parameter) => _view.CopyToClipBoard(_entityPathResolver.PathFor(parameter.Parameter));

      private void createParameterCache()
      {
         var parametersToEdit = getParametersFrom(_container);

         _parameters.Clear();
         _parameters.AddRange(parametersToEdit);

         releaseParameterDTOs();
         _allParametersDTO.Clear();
         _allParametersDTO.AddRange(_parameters.Select(x => _parameterToDTOParameterMapper.MapFrom(x, _trackableSimulation)));
         initializeIndividualParameterCache();

         _allParametersDTO.AddRange(_individualParameterCache.Keys);
         _allParametersDTO.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.InvariantCultureIgnoreCase));
      }

      private void initializeIndividualParameterCache()
      {
         _individualParameterCache.Clear();
         var individualParameters = individualParametersFor(_container, SelectedIndividual);

         individualParameters.Each(individualParameter =>
         {
            var parameterDTO = _individualParameterToDTOParameterMapper.MapFrom(SelectedIndividual, individualParameter);
            _individualParameterCache[parameterDTO] = individualParameter;
         });
      }

      private void releaseParameterDTOs() => _allParametersDTO.Each(dto => dto.Release());

      public EditParameterMode EditMode
      {
         set => View.EditMode = value;
      }

      public IEnumerable<ParameterBuildMode> ParameterBuildModes { get; set; }

      private ISelectReferenceAtParameterPresenter getNewReferencePresenterFor(IContainer container)
      {
         var referencePresenter = _selectReferencePresenterFactory.ReferenceAtParameterFor(container);
         referencePresenter.ChangeLocalisationAllowed = ChangeLocalisationAllowed;
         return referencePresenter;
      }

      private string getContainerName(IContainer container) =>
         string.IsNullOrEmpty(container.Name) ? AppConstants.Captions.NewWindow(_typeResolver.TypeFor(container)) : container.Name;

      public void Select(IParameter parameter)
      {
         setupEditPresenter(parameter);
         _view.Select(dtoFor(parameter));
      }

      private ParameterDTO dtoFor(IParameter parameter) => _allParametersDTO.FirstOrDefault(x => Equals(x.Parameter, parameter));

      private void showParameters()
      {
         var parametersToShowDTO = _allParametersDTO.Where(shouldShowParameter).ToList();
         _view.BindTo(parametersToShowDTO);
         setupEditPresenter(parametersToShowDTO.FirstOrDefault(x => !x.IsIndividualPreview)?.Parameter);
      }

      private bool shouldShowParameter(ParameterDTO parameterDTO) =>
         parameterDTO.IsIndividualPreview || ShowAdvancedParameters || ParameterFrom(parameterDTO).Visible;

      private void refreshList() => _view.RefreshList();

      public bool ShowBuildMode
      {
         set => View.ShowBuildMode = value;
      }

      public ISelectReferenceAtParameterPresenter ValueReference
      {
         get => _editParameterPresenter.ValueReferencesPresenter;
         set => _editParameterPresenter.ValueReferencesPresenter = value;
      }

      public ISelectReferenceAtParameterPresenter RhsReference
      {
         get => _editParameterPresenter.RhsReferencesPresenter;
         set => _editParameterPresenter.RhsReferencesPresenter = value;
      }

      public override IBuildingBlock BuildingBlock
      {
         get => base.BuildingBlock;
         set
         {
            base.BuildingBlock = value;
            _editDistributedParameterPresenter.BuildingBlock = value;
            _editParameterPresenter.BuildingBlock = value;
         }
      }

      public void Rename(ParameterDTO parameterDTO)
      {
         if (parameterDTO.IsIndividualPreview)
            return;

         var parameter = ParameterFrom(parameterDTO);
         _editTask.Rename(parameter, BuildingBlock);
      }

      public void Select(ParameterDTO selectedParameter)
      {
         if (selectedParameter.IsIndividualPreview)
            setupEditPresenter(_individualParameterCache[selectedParameter]);
         else
            setupEditPresenter(ParameterFrom(selectedParameter));
      }

      public void SetIsPersistable(ParameterDTO parameterDTO, bool isPersistable) =>
         //no need for a command here
         parameterDTO.Persistable = isPersistable;

      public void CopyToClipBoard(ParameterDTO parameterDTO) => _clipboardManager.CopyToClipBoard(ParameterFrom(parameterDTO));

      public void CutToClipBoard(ParameterDTO parameterDTO) =>
         _clipboardManager.CutToClipBoard(ParameterFrom(parameterDTO),
            para => AddCommand(_parameterTask.Remove(para, _container, buildingBlock: _buildingBlock, silent: true)));

      public void PasteFromClipBoard()
      {
         _ignoreAddEvents = true;
         try
         {
            _clipboardManager.PasteFromClipBoard<IParameter>(
               para => AddCommand(_parameterTask.AddTo(para, _container, BuildingBlock)));
         }
         finally
         {
            _ignoreAddEvents = false;
         }

         Edit(_container);
      }

      public void LoadParameter() => AddCommand(_parameterTask.AddExisting(_container, BuildingBlock));

      private void refreshViewAndSelect(ParameterDTO parameterDTO)
      {
         Edit(_container);
         Select(parameterDTO);
      }

      public bool ShowAdvancedParameters
      {
         get => userSettings.ShowAdvancedParameters;
         set
         {
            userSettings.ShowAdvancedParameters = value;
            showParameters();
         }
      }

      public bool GroupParameters
      {
         get => userSettings.GroupParameters;
         set
         {
            userSettings.GroupParameters = value;
            showParameters();
         }
      }

      private IUserSettings userSettings => _interactionTaskContext.UserSettings;

      public void SetBuildModeFor(ParameterDTO parameterDTO, ParameterBuildMode newMode)
      {
         var parameter = ParameterFrom(parameterDTO);
         AddCommand(_parameterTask.SetBuildModeForParameter(parameter, newMode, BuildingBlock).RunCommand(_interactionTaskContext.Context));
         _interactionTaskContext.DialogCreator.MessageBoxInfo(AppConstants.Validation.ChangeBuildModeWarning);
         Select(parameterDTO);
      }

      public void AddParameter() => AddCommand(_parameterTask.AddNew(_container, BuildingBlock));

      public void RemoveParameter(ParameterDTO parameterDTO) => AddCommand(_parameterTask.Remove(ParameterFrom(parameterDTO), _container, BuildingBlock));

      private bool shouldHandleRemove(RemovedEvent eventToHandle)
      {
         return eventToHandle.RemovedObjects
            .Where(removedObject => removedObject.IsAnImplementationOf<IParameter>())
            .Cast<IParameter>()
            .Any(para => _parameters.ContainsItem(para));
      }

      private bool shouldHandleAddEvent(AddedEvent eventToHandle)
      {
         //do not handle the event if the parameter is already available in the edited list
         var addedParameter = eventToHandle.AddedObject as IParameter;
         return !_ignoreAddEvents && addedParameter != null
                                  && Equals(eventToHandle.Parent, _container)
                                  && !_parameters.ContainsItem(addedParameter);
      }

      public void Handle(AddedEvent eventToHandle)
      {
         if (_container == null) return;
         if (!shouldHandleAddEvent(eventToHandle)) return;
         Edit(_container);
      }

      public void Handle(RemovedEvent eventToHandle)
      {
         if (_container == null) return;
         if (!shouldHandleRemove(eventToHandle)) return;
         Edit(_container);
      }

      public void Handle(QuantityChangedEvent eventToHandle)
      {
         if (_container == null) return;
         if (!shouldHandleSelect(eventToHandle.Quantity)) return;
         refreshList();
      }

      private bool shouldHandleSelect(IQuantity quantity)
      {
         if (!quantity.IsAnImplementationOf<IParameter>())
            return false;

         return _parameters.Contains(quantity.DowncastTo<IParameter>());
      }

      private void setupEditPresenter(IndividualParameter parameter)
      {
         if (parameter == null)
         {
            _view.SetEditParameterView(null);
         }
         else
         {
            _editIndividualParameterPresenter.Edit(parameter, SelectedIndividual);
            _view.SetEditParameterView(_editIndividualParameterPresenter.BaseView);
         }
      }

      private void setupEditPresenter(IParameter parameter)
      {
         if (_view.EditMode == EditParameterMode.ValuesOnly)
            return;

         if (parameter == null)
         {
            _view.SetEditParameterView(null);
            return;
         }

         if (parameter.IsAnImplementationOf<IDistributedParameter>())
         {
            _editDistributedParameterPresenter.Edit((IDistributedParameter) parameter);
            _view.SetEditParameterView(_editDistributedParameterPresenter.View);
         }
         else
         {
            setEditPresenterProperties(_editParameterPresenter);
            _editParameterPresenter.Edit(parameter);
            _editParameterPresenter.CanSetBuildMode = _view.ShowBuildMode;
            _editParameterPresenter.ParameterBuildModes = ParameterBuildModes;
            _view.SetEditParameterView(_editParameterPresenter.BaseView);
         }
      }

      private void setEditPresenterProperties(IEditParameterPresenter editParameterPresenter)
      {
         editParameterPresenter.LocalEntitiesToReference = getParametersFrom(_container);
         editParameterPresenter.BuildingBlock = BuildingBlock;
      }

      private IReadOnlyList<IParameter> getParametersFrom(IContainer container) => container.GetChildrenSortedByName<IParameter>().ToList();

      public bool BlackBoxAllowed
      {
         set => _editParameterPresenter.BlackBoxAllowed = value;
      }

      public void Handle(ParameterChangedEvent eventToHandle)
      {
         if (!canHandle(eventToHandle))
            return;
         refreshList();
      }

      private bool canHandle(ParameterChangedEvent eventToHandle) => _parameters.Any(parameter => eventToHandle.Parameters.Contains(parameter));

      public void ShowIndividualSelection() => _view.ShowIndividualSelection(true);

      public void UpdatePreview()
      {
         createParameterCache();
         showParameters();
      }

      public void EnableSimulationTracking(TrackableSimulation trackableSimulation) => _trackableSimulation = trackableSimulation;
   }
}