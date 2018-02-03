using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Helper;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Presenter
{
   public interface IEditMoleculeBuilderPresenter : ICanEditPropertiesPresenter, IEditPresenterWithParameters<IMoleculeBuilder>, IPresenterWithFormulaCache
   {
      IEnumerable<string> GetCalculationMethodsForCategory(string category);
      void SetCalculationMethodForCategory(string category, string newValue, string oldValue);
      IEnumerable<QuantityType> GetMoleculeTypes();
      void SetMoleculeType(QuantityType newType, QuantityType oldType);
      void SetStationaryProperty(bool isStationaryNewValue, bool oldValue);
   }

   internal class EditMoleculeBuilderPresenter : AbstractSubPresenterWithFormula<IEditMoleculeBuilderView, IEditMoleculeBuilderPresenter>, IEditMoleculeBuilderPresenter, IListener<ChangedCalculationMethodEvent>
   {
      private IMoleculeBuilder _moleculeBuilder;
      private readonly IMoleculeBuilderToMoleculeBuilderDTOMapper _moleculeBuilderDTOMapper;
      private readonly IEditTaskFor<IMoleculeBuilder> _editTasks;
      private readonly IEditParametersInContainerPresenter _editMoleculeParameters;
      private readonly IMoBiContext _context;
      private readonly ICoreCalculationMethodRepository _calculationMethodsRepository;

      public EditMoleculeBuilderPresenter(IEditMoleculeBuilderView view, IMoleculeBuilderToMoleculeBuilderDTOMapper moleculeBuilderDTOMapper,
         IEditParametersInContainerPresenter editMoleculeParameters, IEditTaskFor<IMoleculeBuilder> editTasks,
         IEditFormulaPresenter editFormulaPresenter, IMoBiContext context, ISelectReferenceAtMoleculePresenter selectReferencePresenter,
         IReactionDimensionRetriever dimensionRetriever, ICoreCalculationMethodRepository calculationMethodsRepository)
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
      }

      public override void InitializeWith(ICommandCollector commandRegister)
      {
         base.InitializeWith(commandRegister);
         _editMoleculeParameters.InitializeWith(CommandCollector);
      }

      public void Edit(object objectToEdit)
      {
         Edit(objectToEdit.DowncastTo<IMoleculeBuilder>());
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

      public void Edit(IMoleculeBuilder moleculeBuilder, IEnumerable<IObjectBase> existingObjectsInParent)
      {
         _moleculeBuilder = moleculeBuilder;
         _editMoleculeParameters.Edit(moleculeBuilder);
         _referencePresenter.Init(null, Enumerable.Empty<IObjectBase>(), null);
         setUpFormulaEditView();
         var dto = _moleculeBuilderDTOMapper.MapFrom(moleculeBuilder);
         dto.AddUsedNames(_editTasks.GetForbiddenNamesWithoutSelf(moleculeBuilder, existingObjectsInParent));
         _view.Show(dto);
      }

      public void Edit(IMoleculeBuilder moleculeBuilder)
      {
         Edit(moleculeBuilder, moleculeBuilder.ParentContainer);
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
         get { return _editMoleculeParameters.BuildingBlock; }
         set { _editMoleculeParameters.BuildingBlock = value; }
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

      public IEnumerable<QuantityType> GetMoleculeTypes()
      {
         yield return QuantityType.Drug;
         yield return QuantityType.Enzyme;
         yield return QuantityType.Transporter;
         yield return QuantityType.Complex;
         yield return QuantityType.Metabolite;
         yield return QuantityType.Protein;
         yield return QuantityType.OtherProtein;
      }

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