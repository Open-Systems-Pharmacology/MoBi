using MoBi.Assets;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface IEditQuantityInSimulationPresenter : IEditPresenter<IQuantity>, IEditInSimulationPresenter, IEditPresenterWithParameters,
      IListener<QuantityChangedEvent>
   {
      void ResetValue();
      void SetValue(double valueInGuiUnit);
      void SetDisplayUnit(Unit displayUnit);
   }

   public class EditQuantityInSimulationPresenter : AbstractEditPresenter<IEditQuantityInSimulationView, IEditQuantityInSimulationPresenter, IQuantity>, IEditQuantityInSimulationPresenter
   {
      private IQuantity _quantity;
      private readonly IQuantityToQuantityDTOMapper _quantityToQuantityDTOMapper;
      private readonly IFormulaPresenterCache _formulaPresenterCache;
      private readonly IEditParametersInContainerPresenter _parameterPresenter;
      private readonly IQuantityTask _quantityTask;
      private QuantityDTO _quantityDTO;
      private IQuantity _quantityToEdit;
      private IEditTypedFormulaPresenter _formulaPresenter;
      public IMoBiSimulation Simulation { get; set; }

      public EditQuantityInSimulationPresenter(IEditQuantityInSimulationView view, IQuantityToQuantityDTOMapper quantityToQuantityDTOMapper,
         IFormulaPresenterCache formulaPresenterCache, IEditParametersInContainerPresenter parameterPresenter, IQuantityTask quantityTask, IReactionDimensionRetriever reactionDimensionRetriever)
         : base(view)
      {
         _quantityTask = quantityTask;
         _parameterPresenter = parameterPresenter;
         _parameterPresenter.EditMode = EditParameterMode.ValuesOnly;
         _quantityToQuantityDTOMapper = quantityToQuantityDTOMapper;
         _formulaPresenterCache = formulaPresenterCache;
         _view.SetInitialValueLabel = initialValueLabel(reactionDimensionRetriever.SelectedDimensionMode);
         AddSubPresenters(_parameterPresenter);
      }

      private string initialValueLabel(ReactionDimensionMode dimensionMode)
      {
         return dimensionMode == ReactionDimensionMode.AmountBased ? AppConstants.Captions.Amount : AppConstants.Captions.Concentration;
      }

      public override void ReleaseFrom(IEventPublisher eventPublisher)
      {
         base.ReleaseFrom(eventPublisher);
         _formulaPresenterCache.ReleaseFrom(eventPublisher);
         _formulaPresenter?.ReleaseFrom(eventPublisher);
      }

      public override void Edit(IQuantity objectToEdit)
      {
         _view.ReadOnly = true;

         var selectionChanged = _quantity != objectToEdit;

         _quantity = objectToEdit;
         _quantityToEdit = objectToEdit.QuantityToEdit();
         rebind();

         initializeFormulaPresenter();
         if (objectToEdit.IsAnImplementationOf<IContainer>())
         {
            _view.SetParametersView(_parameterPresenter.BaseView);
            _parameterPresenter.Edit(objectToEdit.DowncastTo<IContainer>());
            if (selectionChanged)
               _view.ShowParameters();
         }
         else
            _view.HideParametersView();

         _view.AllowValueChange = !objectToEdit.IsAnImplementationOf<Observer>();
      }

      private void rebind()
      {
         _quantityDTO = _quantityToQuantityDTOMapper.MapFrom(_quantity);
         _view.BindTo(_quantityDTO);
         checkValueOverride();
      }

      private void initializeFormulaPresenter()
      {
         var formula = _quantityToEdit.Formula;
         _formulaPresenter = _formulaPresenterCache.PresenterFor(formula);
         _view.SetFormulaView(_formulaPresenter.BaseView);
         _formulaPresenter.ReadOnly = true;
         _formulaPresenter.InitializeWith(CommandCollector);
         _formulaPresenter.Edit(formula, _quantityToEdit);
      }

      private void checkValueOverride()
      {
         if (_quantityToEdit.IsFixedValue)
            _view.SetWarning(AppConstants.Validation.FixedValueSimulationWarning);
         else
            _view.ClearWarning();

         _view.EnableResetButton(_quantityToEdit.IsFixedValue);
      }

      public override object Subject => _quantity;

      public void ResetValue()
      {
         if (!_quantityToEdit.IsFixedValue)
            return;

         AddCommand(_quantityTask.ResetQuantityValue(_quantityToEdit, Simulation));
         rebind();
      }

      public void SetValue(double valueInGuiUnit)
      {
         AddCommand(_quantityTask.SetQuantityDisplayValue(_quantityToEdit, valueInGuiUnit, Simulation));
      }

      public void SetDisplayUnit(Unit displayUnit)
      {
         AddCommand(_quantityTask.SetQuantityDisplayUnit(_quantityToEdit, displayUnit, Simulation));
      }

      public override void AddCommand(ICommand command)
      {
         base.AddCommand(command);
         checkValueOverride();
      }

      public void SelectParameter(IParameter parameter)
      {
         _view.ShowParameters();
         _parameterPresenter.Select(parameter);
      }

      public void Handle(QuantityChangedEvent quantityChangedEvent)
      {
         if (!canHandle(quantityChangedEvent))
            return;

         Edit(_quantity);
      }

      private bool canHandle(QuantityChangedEvent quantityChangedEvent)
      {
         return Equals(quantityChangedEvent.Quantity, _quantityToEdit) ||
                Equals(quantityChangedEvent.Quantity, _quantity);
      }
   }
}