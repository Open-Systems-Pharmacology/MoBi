using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Helper;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Views;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface IEditMoleculeBuilderPresenter : ICanEditPropertiesPresenter, IEditPresenterWithParameters<MoleculeBuilder>, IPresenterWithFormulaCache
   {
      IEnumerable<string> GetCalculationMethodsForCategory(string category);
      void SetCalculationMethodForCategory(string category, string newValue, string oldValue);
      IReadOnlyList<QuantityType> MoleculeTypes { get; }
      void SetMoleculeType(QuantityType newType, QuantityType oldType);
      void SetStationaryProperty(bool isStationaryNewValue, bool oldValue);
   }

   internal class EditMoleculeBuilderPresenter : AbstractSubPresenterWithFormula<IEditMoleculeBuilderView, IEditMoleculeBuilderPresenter>, IEditMoleculeBuilderPresenter, IListener<ChangedCalculationMethodEvent>
   {
      private MoleculeBuilder _moleculeBuilder;
      private readonly IMoleculeBuilderToMoleculeBuilderDTOMapper _moleculeBuilderDTOMapper;
      private readonly IEditTaskFor<MoleculeBuilder> _editTasks;
      private readonly IEditParametersInContainerPresenter _editMoleculeParameters;
      private readonly IMoBiContext _context;
      private readonly ICoreCalculationMethodRepository _calculationMethodsRepository;

      public EditMoleculeBuilderPresenter(
         IEditMoleculeBuilderView view,
         IMoleculeBuilderToMoleculeBuilderDTOMapper moleculeBuilderDTOMapper,
         IEditParametersInContainerPresenter editMoleculeParameters,
         IEditTaskFor<MoleculeBuilder> editTasks,
         IEditFormulaPresenter editFormulaPresenter,
         IMoBiContext context,
         ISelectReferenceAtMoleculePresenter selectReferencePresenter,
         IReactionDimensionRetriever dimensionRetriever,
         ICoreCalculationMethodRepository calculationMethodsRepository)
         : base(view, editFormulaPresenter, selectReferencePresenter)
      {
         _context = context;
         _calculationMethodsRepository = calculationMethodsRepository;
         _moleculeBuilderDTOMapper = moleculeBuilderDTOMapper;
         _editTasks = editTasks;
         _editMoleculeParameters = editMoleculeParameters;
         AddSubPresenters(_editMoleculeParameters);
         _view.SetParametersView(_editMoleculeParameters.BaseView);
         _view.UpdateStartAmountDisplay(dimensionRetriever.SelectedDimensionMode == ReactionDimensionMode.AmountBased
            ? AppConstants.Captions.Amount
            : AppConstants.Captions.Concentration);

         //We support container criteria for parameters in molecule builder
         _editMoleculeParameters.EnableContainerCriteriaSupport();
      }

      public override void InitializeWith(ICommandCollector commandRegister)
      {
         base.InitializeWith(commandRegister);
         _editMoleculeParameters.InitializeWith(CommandCollector);
      }

      public void Edit(object objectToEdit)
      {
         Edit(objectToEdit.DowncastTo<MoleculeBuilder>());
      }

      public override void ReleaseFrom(IEventPublisher eventPublisher)
      {
         base.ReleaseFrom(eventPublisher);
         _editMoleculeParameters.ReleaseFrom(eventPublisher);
      }

      public void SetPropertyValueFromView<T>(string propertyName, T newValue, T oldValue)
      {
         AddCommand(new EditObjectBasePropertyInBuildingBlockCommand(propertyName, newValue, oldValue, _moleculeBuilder, BuildingBlock).Run(_context));
      }

      public void RenameSubject()
      {
         _editTasks.Rename(_moleculeBuilder, _moleculeBuilder.ParentContainer, BuildingBlock);
      }

      public void Edit(MoleculeBuilder moleculeBuilder, IReadOnlyList<IObjectBase> existingObjectsInParent)
      {
         _moleculeBuilder = moleculeBuilder;
         _editMoleculeParameters.Edit(moleculeBuilder);
         _referencePresenter.Init(null, Enumerable.Empty<IObjectBase>(), null);
         setUpFormulaEditView();
         var dto = _moleculeBuilderDTOMapper.MapFrom(moleculeBuilder);
         dto.AddUsedNames(_editTasks.GetForbiddenNamesWithoutSelf(moleculeBuilder, existingObjectsInParent));
         _view.Show(dto);
      }

      public void Edit(MoleculeBuilder moleculeBuilder)
      {
         Edit(moleculeBuilder, moleculeBuilder.ParentContainer.Children);
      }

      public object Subject => _moleculeBuilder;

      public void SelectParameter(IParameter parameter)
      {
         _view.ShowParameters();
         _editMoleculeParameters.Select(parameter);
      }

      private void setUpFormulaEditView()
      {
         _editFormulaPresenter.Init(_moleculeBuilder, BuildingBlock, new DefaultStartFormulaDecoder());
      }

      public IEnumerable<FormulaBuilderDTO> GetFormulas()
      {
         yield break;
      }

      public IBuildingBlock BuildingBlock
      {
         get => _editMoleculeParameters.BuildingBlock;
         set => _editMoleculeParameters.BuildingBlock = value;
      }

      public IFormulaCache FormulaCache => BuildingBlock.FormulaCache;

      public IEnumerable<string> GetCalculationMethodsForCategory(string category)
      {
         return _calculationMethodsRepository
            .GetAllCalculationMethodsFor(category)
            .Select(cm => cm.Name);
      }

      public void SetCalculationMethodForCategory(string category, string newValue, string oldValue)
      {
         AddCommand(new ChangeCalculationMethodForCategoryCommand(_moleculeBuilder, category, newValue, oldValue, BuildingBlock).Run(_context));
      }

      public IReadOnlyList<QuantityType> MoleculeTypes { get; } = new[]
      {
         QuantityType.Drug,
         QuantityType.Enzyme,
         QuantityType.Transporter,
         QuantityType.Complex,
         QuantityType.Metabolite,
         QuantityType.Protein,
         QuantityType.OtherProtein,
      };

      public void SetMoleculeType(QuantityType newType, QuantityType oldType)
      {
         AddCommand(new ChangeMoleculeTypeCommand(_moleculeBuilder, newType, oldType, BuildingBlock).Run(_context));
         _context.PublishEvent(new MoleculeIconChangedEvent(_moleculeBuilder));
      }

      public void SetStationaryProperty(bool isStationaryNewValue, bool oldValue)
      {
         AddCommand(new SetStationaryPropertyCommand(_moleculeBuilder, isStationaryNewValue, oldValue, BuildingBlock).Run(_context));
      }

      public void Handle(ChangedCalculationMethodEvent eventToHandle)
      {
         if (eventToHandle.MoleculeBuilder.Equals(_moleculeBuilder))
         {
            Edit(_moleculeBuilder);
         }
      }
   }
}