using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Events;
using MoBi.Core.Helper;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
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
      bool ChangeLocalizationAllowed { set; }
      EditParameterMode EditMode { set; }
      IEnumerable<ParameterBuildMode> ParameterBuildModes { get; set; }
      void SetBuildModeFor(ParameterDTO parameterDTO, ParameterBuildMode newMode);
      void SetIsPersistable(ParameterDTO parameterDTO, bool isPersistable);
      void SetDimensionFor(ParameterDTO parameterDTO, IDimension newDimension);
   }

   public class EditParametersInContainerPresenter : AbstractParameterBasePresenter<IEditParametersInContainerView, IEditParametersInContainerPresenter>, IEditParametersInContainerPresenter
   {
      private readonly IClipboardManager _clipboardManager;
      private readonly IParameterToParameterDTOMapper _parameterToDTOParameterMapper;
      private readonly ICache<string, IParameter> _parameters = new Cache<string, IParameter>(x => x.Id, x => null);
      private readonly List<ParameterDTO> _allParametersDTO = new List<ParameterDTO>();

      private IContainer _container;
      private readonly IEditTaskFor<IParameter> _editTask;
      private readonly ISelectReferencePresenterFactory _selectReferencePresenterFactory;
      private readonly Func<IContainer, IEnumerable<IParameter>> _getParametersFunc;
      private readonly IEditDistributedParameterPresenter _editDistributedParameterPresenter;
      private readonly IEditParameterPresenter _editParameterPresenter;
      private bool _ignoreAddEvents;
      public bool ChangeLocalizationAllowed { set; private get; }

      public EditParametersInContainerPresenter(IEditParametersInContainerView view,
         IFormulaToFormulaBuilderDTOMapper formulaMapper,
         IParameterToParameterDTOMapper parameterToDTOParameterMapper,
         IInteractionTasksForParameter parameterTask,
         IEditDistributedParameterPresenter editDistributedParameterPresenter,
         IEditParameterPresenter editParameterPresenter,
         IQuantityTask quantityTask, IInteractionTaskContext interactionTaskContext,
         IClipboardManager clipboardManager, IEditTaskFor<IParameter> editTask,
         ISelectReferencePresenterFactory selectReferencePresenterFactory, IFavoriteTask favoriteTask)
         : base(view, quantityTask, interactionTaskContext, formulaMapper, parameterTask, favoriteTask)
      {
         _clipboardManager = clipboardManager;
         _editTask = editTask;
         _selectReferencePresenterFactory = selectReferencePresenterFactory;
         _editDistributedParameterPresenter = editDistributedParameterPresenter;
         _editParameterPresenter = editParameterPresenter;
         EditMode = EditParameterMode.All;
         _parameterToDTOParameterMapper = parameterToDTOParameterMapper;
         _editParameterPresenter = editParameterPresenter;
         _editDistributedParameterPresenter = editDistributedParameterPresenter;
         AddSubPresenters(_editDistributedParameterPresenter, _editParameterPresenter);
         _getParametersFunc = x => x.GetChildrenSortedByName<IParameter>();
         ChangeLocalizationAllowed = true;
      }

      public void Edit(IContainer container)
      {
         _container = container;
         ValueReference = getNewReferencePresenterFor(container);
         RhsReference = getNewReferencePresenterFor(container);
         ShowBuildMode = container.CanSetBuildModeForParameters();
         ParameterBuildModes = container.AvailableBuildModeForParameters();
         _view.ParentName = container.Name;
         createParameterCache(_getParametersFunc(container));
         showParameters();
      }

      public override void ReleaseFrom(IEventPublisher eventPublisher)
      {
         base.ReleaseFrom(eventPublisher);
         releaseParameters();
      }

      public void SetDimensionFor(ParameterDTO parameterDTO, IDimension newDimension)
      {
         var parameter = ParameterFrom(parameterDTO);
         AddCommand(_parameterTask.SetDimensionForParameter(parameter, newDimension, BuildingBlock));
         refreshViewAndSelect(parameterDTO);
      }

      private void createParameterCache(IEnumerable<IParameter> parametersToEdit)
      {
         _parameters.Clear();
         _parameters.AddRange(parametersToEdit);

         releaseParameters();
         _allParametersDTO.Clear();
         _allParametersDTO.AddRange(_parameters.MapAllUsing(_parameterToDTOParameterMapper).Cast<ParameterDTO>());
      }

      private void releaseParameters()
      {
         _allParametersDTO.Each(dto => dto.Release());
      }

      public EditParameterMode EditMode
      {
         set => View.EditMode = value;
      }

      public IEnumerable<ParameterBuildMode> ParameterBuildModes { get; set; }

      private ISelectReferenceAtParameterPresenter getNewReferencePresenterFor(IContainer container)
      {
         var referencePresenter = _selectReferencePresenterFactory.ReferenceAtParameterFor(container);
         referencePresenter.ChangeLocalisationAllowed = ChangeLocalizationAllowed;
         return referencePresenter;
      }

      public void Select(IParameter parameter)
      {
         setupEditPresenter(parameter);
         _view.Select(dtoFor(parameter));
      }

      private ParameterDTO dtoFor(IParameter parameter)
      {
         return _allParametersDTO.FirstOrDefault(x => Equals(x.Parameter, parameter));
      }

      private void showParameters()
      {
         var parametersToShowDTO = _allParametersDTO.Where(shouldShowParameter).ToList();
         _view.BindTo(parametersToShowDTO);
         setupEditPresenter(parametersToShowDTO.FirstOrDefault()?.Parameter);

      }

      private bool shouldShowParameter(ParameterDTO parameterDTO)
      {
         return ShowAdvancedParameters || ParameterFrom(parameterDTO).Visible;
      }

      private void refreshList()
      {
         _view.RefreshList();
      }

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
         var parameter = ParameterFrom(parameterDTO);
         _editTask.Rename(parameter, BuildingBlock);
      }

      public void Select(ParameterDTO selectedParameter)
      {
         var parameter = ParameterFrom(selectedParameter);
         setupEditPresenter(parameter);
      }

      public void SetIsPersistable(ParameterDTO parameterDTO, bool isPersistable)
      {
         //no need for a command here
         parameterDTO.Persistable = isPersistable;
      }

      public void CopyToClipBoard(ParameterDTO parameterDTO)
      {
         _clipboardManager.CopyToClipBoard(ParameterFrom(parameterDTO));
      }

      public void CutToClipBoard(ParameterDTO parameterDTO)
      {
         _clipboardManager.CutToClipBoard(ParameterFrom(parameterDTO),
            para => AddCommand(_parameterTask.Remove(para, _container, buildingBlock: _buildingBlock, silent: true)));
      }

      public void PasteFromClipBoard()
      {
         _ignoreAddEvents = true;
         try
         {
            _clipboardManager.PasteFromClipBoard<IParameter>(
               para => AddCommand(_parameterTask.AddToProject(para, _container, BuildingBlock)));
         }
         finally
         {
            _ignoreAddEvents = false;
         }

         Edit(_container);
      }

      public void LoadParameter()
      {
         AddCommand(_parameterTask.AddExisting(_container, BuildingBlock));
      }

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
         AddCommand(_parameterTask.SetBuildModeForParameter(parameter, newMode, BuildingBlock).Run(_interactionTaskContext.Context));
         _interactionTaskContext.DialogCreator.MessageBoxInfo(AppConstants.Validation.ChangeBuildModeWarning);
         Select(parameterDTO);
      }

      public void AddParameter()
      {
         AddCommand(_parameterTask.AddNew(_container, BuildingBlock));
      }

      public void RemoveParameter(ParameterDTO parameterDTO)
      {
         AddCommand(_parameterTask.Remove(ParameterFrom(parameterDTO), _container, BuildingBlock));
      }

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
            SetEditPresenterProperties(_editParameterPresenter);
            _editParameterPresenter.Edit(parameter);
            _editParameterPresenter.CanSetBuildMode = _view.ShowBuildMode;
            _editParameterPresenter.ParameterBuildModes = ParameterBuildModes;
            _view.SetEditParameterView(_editParameterPresenter.BaseView);
         }
      }

      protected virtual void SetEditPresenterProperties(IEditParameterPresenter editParameterPresenter)
      {
         editParameterPresenter.LocalEntitiesToReference = _getParametersFunc(_container).ToEnumerable<IParameter, IObjectBase>();
         editParameterPresenter.BuildingBlock = BuildingBlock;
      }

      public bool BlackBoxAllowed
      {
         set => _editParameterPresenter.BlackBoxAllowed = value;
      }

      public void Handle(ParameterChangedEvent eventToHandle)
      {
         if (!canHandle(eventToHandle)) return;
         refreshList();
      }

      private bool canHandle(ParameterChangedEvent eventToHandle)
      {
         return _parameters.Any(parameter => eventToHandle.Parameters.Contains(parameter));
      }
   }
}