using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface IEditParameterPresenter : IEditPresenter<IParameter>,
      IPresenterWithFormulaCache,
      ICreatePresenter<IParameter>
   {
      void SetUseRHSFormula(bool useRHS);
      IReadOnlyList<IDimension> AllDimensions();
      void SetDimension(IDimension dimension);
      IEnumerable<IObjectBase> LocalEntitiesToReference { set; }
      bool CanSetBuildMode { set; }
      ISelectReferenceAtParameterPresenter ValueReferencesPresenter { get; set; }
      ISelectReferenceAtParameterPresenter RhsReferencesPresenter { get; set; }
      bool BlackBoxAllowed { set; }
      void SetIsAdvancedParameter(bool isAdvancedParameter);
      void SetPersistable(bool isPersitable);
      void SetIsVariablePopulation(bool isVariableInPopulation);
      IEnumerable<IGroup> AllGroups();
      void SetGroup(IGroup group);
      string DisplayFor(IGroup group);
      void SetIsFavorite(bool isFavorite);

      /// <summary>
      ///    Rename the parameter being edited
      /// </summary>
      void RenameParameter();

      /// <summary>
      ///    Sets a new description on the parameter via command
      /// </summary>
      /// <param name="parameterDTO">The parameter being edited</param>
      /// <param name="newDescription">The new description</param>
      void SetDescription(ParameterDTO parameterDTO, string newDescription);

      /// <summary>
      ///    Sets a new build mode for the parameter. The user is warned about implications of setting build mode
      /// </summary>
      /// <param name="parameterDTO">The parameter being edited</param>
      /// <param name="newBuildMode">The new build mode</param>
      void SetBuildMode(ParameterDTO parameterDTO, ParameterBuildMode newBuildMode);

      /// <summary>
      ///    Sets a new name on the parameter. A dialog prompts user for the new name
      /// </summary>
      /// <param name="parameterDTO">The parameter being edited</param>
      /// <param name="newName">The new name</param>
      void SetName(ParameterDTO parameterDTO, string newName);

      /// <summary>
      ///    Build modes that can be used in this use case
      /// </summary>
      IEnumerable<ParameterBuildMode> ParameterBuildModes { get; set; }

      bool WarnOnBuildModeChange { get; set; }

      /// <summary>
      ///    Enables the Container criteria support for specific use cases
      /// </summary>
      void EnableContainerCriteriaSupport();
   }

   public class EditParameterPresenter : AbstractEntityEditPresenter<IEditParameterView, IEditParameterPresenter, IParameter>, IEditParameterPresenter
   {
      private IParameter _parameter;
      private readonly IEditFormulaPresenter _editRHSFormulaPresenter;
      private readonly IInteractionTaskContext _interactionTaskContext;
      private readonly IEditFormulaPresenter _editValueFormulaPresenter;
      private readonly IParameterToParameterDTOMapper _parameterMapper;
      private ParameterDTO _parameterDTO;
      public IBuildingBlock BuildingBlock { get; set; }
      public IEnumerable<IObjectBase> LocalEntitiesToReference { set; protected get; }
      private readonly IGroupRepository _groupRepository;
      private readonly IEditTaskFor<IParameter> _editTasks;
      private readonly IInteractionTasksForParameter _parameterTask;
      private readonly IContextSpecificReferencesRetriever _contextSpecificReferencesRetriever;
      private readonly IFavoriteTask _favoriteTask;
      private readonly IEditValueOriginPresenter _editValueOriginPresenter;
      private readonly ITagsPresenter _tagsPresenter;
      public IEnumerable<ParameterBuildMode> ParameterBuildModes { get; set; }
      public bool WarnOnBuildModeChange { get; set; }

      private readonly IDescriptorConditionListPresenter<IParameter> _containerCriteriaPresenter;

      public EditParameterPresenter(IEditParameterView view,
         IEditFormulaPresenter editValueFormulaPresenter,
         IParameterToParameterDTOMapper parameterMapper,
         IEditFormulaPresenter editRhsFormulaPresenter,
         IInteractionTaskContext interactionTaskContext,
         IGroupRepository groupRepository,
         IEditTaskFor<IParameter> editTasks,
         IInteractionTasksForParameter parameterTask,
         IContextSpecificReferencesRetriever contextSpecificReferencesRetriever,
         IFavoriteTask favoriteTask,
         IEditValueOriginPresenter editValueOriginPresenter,
         ITagsPresenter tagsPresenter,
         IDescriptorConditionListPresenter<IParameter> containerCriteriaPresenter)
         : base(view)
      {
         _editValueFormulaPresenter = editValueFormulaPresenter;
         _parameterTask = parameterTask;
         _contextSpecificReferencesRetriever = contextSpecificReferencesRetriever;
         _favoriteTask = favoriteTask;
         _editValueOriginPresenter = editValueOriginPresenter;
         _tagsPresenter = tagsPresenter;
         _containerCriteriaPresenter = containerCriteriaPresenter;
         _parameterMapper = parameterMapper;
         _groupRepository = groupRepository;
         _editTasks = editTasks;
         _editRHSFormulaPresenter = editRhsFormulaPresenter;
         _interactionTaskContext = interactionTaskContext;
         _view.SetFormulaView(_editValueFormulaPresenter.BaseView);
         _view.AddRHSView(_editRHSFormulaPresenter.BaseView);
         _view.AddValueOriginView(_editValueOriginPresenter.BaseView);
         _view.AddTagsView(_tagsPresenter.BaseView);

         AddSubPresenters(editRhsFormulaPresenter, editValueFormulaPresenter, _editValueOriginPresenter, _tagsPresenter, _containerCriteriaPresenter);

         _editRHSFormulaPresenter.IsRHS = true;
         _editRHSFormulaPresenter.RemoveAllFormulaTypes();
         _editRHSFormulaPresenter.AddFormulaType<ConstantFormula>();
         _editRHSFormulaPresenter.AddFormulaType<ExplicitFormula>();
         _editRHSFormulaPresenter.SetDefaultFormulaType<ExplicitFormula>();
         ParameterBuildModes = EnumHelper.AllValuesFor<ParameterBuildMode>();
         WarnOnBuildModeChange = true;
         _editValueOriginPresenter.ValueOriginUpdated += setValueOrigin;
         _editValueOriginPresenter.ShowCaption = false;
      }

      public void EnableContainerCriteriaSupport()
      {
         _view.AddContainerCriteriaView(_containerCriteriaPresenter.BaseView);
      }

      public void SetIsFavorite(bool isFavorite) => _favoriteTask.SetParameterFavorite(_parameter, isFavorite);

      public void RenameParameter() => _editTasks.Rename(_parameter, BuildingBlock);

      public void SetDescription(ParameterDTO parameterDTO, string newDescription)
      {
         AddCommand(_parameterTask.SetDescriptionForParameter(parameterDTO.Parameter, newDescription, BuildingBlock)
            .Run(_interactionTaskContext.Context));
      }

      public void SetBuildMode(ParameterDTO parameterDTO, ParameterBuildMode newBuildMode)
      {
         if (WarnOnBuildModeChange)
            _interactionTaskContext.DialogCreator.MessageBoxInfo(AppConstants.Validation.ChangeBuildModeWarning);

         AddCommand(_parameterTask.SetBuildModeForParameter(parameterDTO.Parameter, newBuildMode, BuildingBlock)
            .Run(_interactionTaskContext.Context));
      }

      public void SetName(ParameterDTO parameterDTO, string newName)
      {
         AddCommand(_parameterTask.SetNameForParameter(parameterDTO.Parameter, newName, BuildingBlock).Run(_interactionTaskContext.Context));
      }

      private void setValueOrigin(ValueOrigin newValueOrigin)
      {
         AddCommand(_parameterTask.SetValueOriginForParameter(_parameter, newValueOrigin, BuildingBlock));
      }

      public ISelectReferenceAtParameterPresenter ValueReferencesPresenter
      {
         set => _editValueFormulaPresenter.ReferencePresenter = value;
         get => _editValueFormulaPresenter.ReferencePresenter.DowncastTo<ISelectReferenceAtParameterPresenter>();
      }

      public ISelectReferenceAtParameterPresenter RhsReferencesPresenter
      {
         set => _editRHSFormulaPresenter.ReferencePresenter = value;
         get => _editRHSFormulaPresenter.ReferencePresenter.DowncastTo<ISelectReferenceAtParameterPresenter>();
      }

      public bool BlackBoxAllowed
      {
         set
         {
            _editValueFormulaPresenter.BlackBoxAllowed = value;
            _editRHSFormulaPresenter.BlackBoxAllowed = value;
         }
      }

      public override void Edit(IParameter parameter, IEnumerable<IObjectBase> existingObjectsInParent)
      {
         _parameter = parameter;
         _editValueFormulaPresenter.Init(_parameter, BuildingBlock);
         _editValueFormulaPresenter.ReferencePresenter.Init(_contextSpecificReferencesRetriever.RetrieveLocalReferencePoint(parameter),
            _contextSpecificReferencesRetriever.RetrieveFor(_parameter, BuildingBlock), _parameter);
         _editValueOriginPresenter.Edit(parameter);
         if (hasRHS(parameter))
            initRHSPresenter();

         _tagsPresenter.BuildingBlock = BuildingBlock;
         _tagsPresenter.Edit(parameter);
         _containerCriteriaPresenter.Edit(parameter, x => x.ContainerCriteria, BuildingBlock);
         _containerCriteriaPresenter.DescriptorCriteriaCreator = x =>
         {
            x.ContainerCriteria = new DescriptorCriteria();
            return x.ContainerCriteria;
         };

         _parameterDTO = _parameterMapper.MapFrom(parameter).DowncastTo<ParameterDTO>();
         _parameterDTO.AddUsedNames(_editTasks.GetForbiddenNamesWithoutSelf(parameter, existingObjectsInParent));
         _view.Show(_parameterDTO);
      }

      public override void ReleaseFrom(IEventPublisher eventPublisher)
      {
         base.ReleaseFrom(eventPublisher);
         _parameter = null;
      }

      private static bool hasRHS(IParameter parameter)
      {
         return parameter.RHSFormula != null;
      }

      private void initRHSPresenter()
      {
         _editRHSFormulaPresenter.Init(_parameter, BuildingBlock);
         _editRHSFormulaPresenter.ReferencePresenter.Init(_contextSpecificReferencesRetriever.RetrieveLocalReferencePoint(_parameter),
            _contextSpecificReferencesRetriever.RetrieveFor(_parameter, BuildingBlock), _parameter);
      }

      public override object Subject => _parameter;

      public IEnumerable<FormulaBuilderDTO> GetFormulas()
      {
         yield break;
      }

      public IFormulaCache FormulaCache => BuildingBlock.FormulaCache;

      public void SetUseRHSFormula(bool useRHS)
      {
         if (useRHS)
            initRHSPresenter();
         else
            AddCommand(_parameterTask.ResetRHSFormulaFor(_parameter, BuildingBlock));
      }

      public void SetIsAdvancedParameter(bool isAdvancedParameter)
      {
         addCommandToRun(new EditIsAdvancesParameterPropertyCommand(_parameter, isAdvancedParameter, BuildingBlock));
      }

      public void SetPersistable(bool isPersitable)
      {
         //no command required here
         _parameter.Persistable = isPersitable;
      }

      public void SetIsVariablePopulation(bool isVariableInPopulation)
      {
         addCommandToRun(new EditParameterCanBeVariedInPopulationCommand(_parameter, isVariableInPopulation, BuildingBlock));
      }

      public IEnumerable<IGroup> AllGroups()
      {
         return _groupRepository.All().OrderBy(x => x.FullName);
      }

      public void SetGroup(IGroup group)
      {
         addCommandToRun(new EditParameterGroupCommand(_parameter, group, BuildingBlock));
         _parameterDTO.Group = group;
      }

      public string DisplayFor(IGroup group) => @group.FullName;

      public IReadOnlyList<IDimension> AllDimensions()
      {
         return _interactionTaskContext.Context.DimensionFactory.DimensionsSortedByName;
      }

      private void addCommandToRun(ICommand<IMoBiContext> command)
      {
         AddCommand(command.Run(_interactionTaskContext.Context));
      }

      public void SetDimension(IDimension dimension)
      {
         AddCommand(_parameterTask.SetDimensionForParameter(_parameter, dimension, BuildingBlock));
         _interactionTaskContext.UserSettings.ParameterDefaultDimension = dimension.Name;
      }

      public bool CanSetBuildMode
      {
         set => _view.ShowBuildMode = value;
      }

      public override bool CanClose
      {
         get
         {
            if (_parameterDTO == null || _parameterDTO.HasRHS)
               return base.CanClose;

            //Do not include rhs formula presenter into error check if we do not use the rhs
            return !_view.HasError && _editValueFormulaPresenter.CanClose;
         }
      }
   }
}