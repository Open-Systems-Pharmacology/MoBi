using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Events;
using MoBi.Core.Helper;
using MoBi.Core.Services;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks
{
   public class MoBiFormulaTask : IMoBiFormulaTask
   {
      private readonly IMoBiContext _context;
      private readonly IMoBiApplicationController _applicationController;
      private readonly IFormulaTask _formulaTask;
      private readonly INameCorrector _nameCorrector;
      private readonly IDialogCreator _dialogCreator;
      private readonly IQuantityTask _quantityTask;
      private readonly IEntitiesInBuildingBlockRetriever<IParameter> _parameterInBuildingBlockRetriever;

      public MoBiFormulaTask(
         IMoBiContext context,
         IMoBiApplicationController applicationController,
         IFormulaTask formulaTask,
         INameCorrector nameCorrector,
         IDialogCreator dialogCreator,
         IQuantityTask quantityTask,
         IEntitiesInBuildingBlockRetriever<IParameter> parameterInBuildingBlockRetriever)
      {
         _context = context;
         _applicationController = applicationController;
         _formulaTask = formulaTask;
         _nameCorrector = nameCorrector;
         _dialogCreator = dialogCreator;
         _quantityTask = quantityTask;
         _parameterInBuildingBlockRetriever = parameterInBuildingBlockRetriever;
      }

      public bool EditNewFormula(IFormula formula, ICommandCollector command, IBuildingBlock buildingBlock, IParameter parameter)
      {
         using (var presenter = _applicationController.Start<INewFormulaPresenter>())
         {
            presenter.InitializeWith(command);
            return presenter.Edit(formula, buildingBlock, parameter);
         }
      }

      public IMoBiCommand AddFormulaToCacheOrFixReferenceCommand(IBuildingBlock targetBuildingBlock, IUsingFormula usingFormulaObject)
      {
         return AddFormulaToCacheOrFixReferenceCommand(targetBuildingBlock, usingFormulaObject, new UsingFormulaDecoder());
      }

      public IMoBiCommand AddFormulaToCacheOrFixReferenceCommand<T>(IBuildingBlock targetBuildingBlock, T usingFormulaObject, FormulaDecoder<T> decoder)
      {
         if (decoder.GetFormula(usingFormulaObject) == null)
            return new MoBiEmptyCommand();

         var existingFormula = targetBuildingBlock.FormulaCache.FindByName(decoder.GetFormula(usingFormulaObject).Name);

         // There is no existing formula in the cache with the same name
         if (existingFormula == null)
            return new AddFormulaToFormulaCacheCommand(targetBuildingBlock, decoder.GetFormula(usingFormulaObject));

         // There is a formula with the same name, but it is not equal to the formula in the object
         if (!_formulaTask.FormulasAreTheSame(existingFormula, decoder.GetFormula(usingFormulaObject)))
         {
            _nameCorrector.AutoCorrectName(targetBuildingBlock.FormulaCache.Select(formula => formula.Name), decoder.GetFormula(usingFormulaObject));
            return new AddFormulaToFormulaCacheCommand(targetBuildingBlock, decoder.GetFormula(usingFormulaObject));
         }

         // not null and formulas are the same
         decoder.SetFormula(existingFormula, usingFormulaObject);
         return new MoBiEmptyCommand();
      }

      public string GetFormulaCaption(IUsingFormula usingObject, ReactionDimensionMode reactionMode, bool isRHS)
      {
         if (usingObject == null)
            return string.Empty;

         if (usingObject.IsAnImplementationOf<TransportBuilder>())
            return AppConstants.Captions.AmountRightHandSide;

         if (usingObject.IsAnImplementationOf<IParameter>() && isRHS)
            return AppConstants.Captions.ParameterRightHandSide(usingObject.Name);

         if (usingObject.IsAnImplementationOf<ReactionBuilder>())
            return reactionMode == ReactionDimensionMode.AmountBased ? AppConstants.Captions.AmountRightHandSide : AppConstants.Captions.ConcentrationRightHandSide;

         return string.Empty;
      }

      private IMoBiCommand withUpdatedDefaultStateAndValueOrigin(IMoBiCommand executedCommand, IFormula formula, IBuildingBlock buildingBlock)
      {
         if (executedCommand.IsEmpty())
            return executedCommand;

         var parametersUsingFormula = _parameterInBuildingBlockRetriever.AllFrom(buildingBlock, p => Equals(p.Formula, formula));
         if (!parametersUsingFormula.Any())
            return executedCommand;

         var updateValueOriginCommand = new MoBiMacroCommand();
         parametersUsingFormula.Each(p => updateValueOriginCommand.Add(_quantityTask.UpdateDefaultStateAndValueOriginFor(p, buildingBlock)));

         //we have depending parameters but they all have default state and value origin set;
         if (updateValueOriginCommand.IsEmpty)
            return executedCommand;

         var macroCommand = new MoBiMacroCommand().WithHistoryEntriesFrom(executedCommand);
         macroCommand.Add(executedCommand);
         macroCommand.AddRange(updateValueOriginCommand.All());
         return macroCommand;
      }

      public IMoBiCommand SetFormulaString(FormulaWithFormulaString formula, string newFormulaString, IBuildingBlock buildingBlock)
      {
         var command = new EditFormulaStringCommand(newFormulaString, formula, buildingBlock).Run(_context);
         return withUpdatedDefaultStateAndValueOrigin(command, formula, buildingBlock);
      }

      public IMoBiCommand EditAliasInFormula(IFormula formula, string newAlias, string oldAlias, FormulaUsablePath formulaUsablePath, IBuildingBlock buildingBlock)
      {
         return new EditFormulaAliasCommand(formula, newAlias, oldAlias, buildingBlock).Run(_context);
      }

      public IMoBiCommand SetFormulaPathDimension(IFormula formula, IDimension newDimension, string alias, IBuildingBlock buildingBlock)
      {
         return new UpdateDimensionOfFormulaUsablePathCommand(newDimension, formula, alias, buildingBlock).Run(_context);
      }

      public IMoBiCommand RemoveFormulaUsablePath(IFormula formula, FormulaUsablePath path, IBuildingBlock buildingBlock)
      {
         return new RemoveFormulaUsablePathCommand(formula, path, buildingBlock).Run(_context);
      }

      public (bool valid, string validationMessage) Validate(string formulaString, FormulaWithFormulaString formula, IBuildingBlock buildingBlock)
      {
         var (valid, validationMessage) = formula.IsValid(formulaString);

         if (valid)
            _context.PublishEvent(new FormulaValidEvent(formula, buildingBlock));
         else
            _context.PublishEvent(new FormulaInvalidEvent(formula, buildingBlock, validationMessage));

         return (valid, validationMessage);
      }

      public IMoBiCommand ChangePathInFormula(IFormula formula, ObjectPath newPath, FormulaUsablePath formulaUsablePath, IBuildingBlock buildingBlock)
      {
         var command = new EditPathAtUsablePathCommand(formula, newPath, formulaUsablePath, buildingBlock).Run(_context);
         return withUpdatedDefaultStateAndValueOrigin(command, formula, buildingBlock);
      }

      public IMoBiCommand AddFormulaUsablePath(IFormula formula, FormulaUsablePath path, IBuildingBlock buildingBlock)
      {
         return new AddFormulaUsablePathCommand(formula, path, buildingBlock).Run(_context);
      }

      public IMoBiCommand ChangeVariableName(SumFormula formula, string newVariableName, IBuildingBlock buildingBlock)
      {
         return new ChangeVariableNameCommand(formula, newVariableName, buildingBlock).Run(_context);
      }

      public IMoBiCommand AddValuePoint(TableFormula formula, ValuePoint newValuePoint, IBuildingBlock buildingBlock)
      {
         var command = new AddValuePointCommand(formula, newValuePoint, buildingBlock).Run(_context);
         return withUpdatedDefaultStateAndValueOrigin(command, formula, buildingBlock);
      }

      public IMoBiCommand EditUseDerivedValues(TableFormula formula, bool newValue, bool oldValue, IBuildingBlock buildingBlock)
      {
         var command = new EditUseDerivedValuesCommand(formula, newValue, oldValue, buildingBlock).Run(_context);
         return withUpdatedDefaultStateAndValueOrigin(command, formula, buildingBlock);
      }

      public IMoBiCommand RemoveValuePointFromTableFormula(TableFormula formula, ValuePoint valuePoint, IBuildingBlock buildingBlock)
      {
         var command = new RemoveValuePointFromTableFormulaCommand(formula, valuePoint, buildingBlock).Run(_context);
         return withUpdatedDefaultStateAndValueOrigin(command, formula, buildingBlock);
      }

      public IMoBiCommand ChangeOffsetObject(TableFormulaWithOffset formula, FormulaUsablePath path, IBuildingBlock buildingBlock)
      {
         var command = new ChangeTableFormulaWithOffsetOffsetObjectPathCommand(formula, path, buildingBlock).Run(_context);
         return withUpdatedDefaultStateAndValueOrigin(command, formula, buildingBlock);
      }

      public IMoBiCommand ChangeTableObject(TableFormulaWithOffset formula, FormulaUsablePath path, IBuildingBlock buildingBlock)
      {
         var command = new ChangeTableFormulaWithOffsetTableObjectPathCommand(formula, path, buildingBlock).Run(_context);
         return withUpdatedDefaultStateAndValueOrigin(command, formula, buildingBlock);
      }

      public IMoBiCommand ChangeXArgumentObject(TableFormulaWithXArgument formula, FormulaUsablePath path, IBuildingBlock buildingBlock)
      {
         var command = new ChangeTableFormulaWithXArgumentXArgumentObjectPathCommand(formula, path, buildingBlock).Run(_context);
         return withUpdatedDefaultStateAndValueOrigin(command, formula, buildingBlock);
      }

      public IMoBiCommand ChangeTableObject(TableFormulaWithXArgument formula, FormulaUsablePath path, IBuildingBlock buildingBlock)
      {
         var command = new ChangeTableFormulaWithXArgumentTableObjectPathCommand(formula, path, buildingBlock).Run(_context);
         return withUpdatedDefaultStateAndValueOrigin(command, formula, buildingBlock);
      }

      public IMoBiCommand SetConstantFormulaValue(ConstantFormula formula, double kernelValue, Unit newDisplayUnit, Unit oldDisplayUnit, IBuildingBlock buildingBlock, IEntity formulaOwner)
      {
         var command = new SetConstantFormulaValueCommand(formula, kernelValue, newDisplayUnit, oldDisplayUnit, buildingBlock, formulaOwner).Run(_context);
         return withUpdatedDefaultStateAndValueOrigin(command, formula, buildingBlock);
      }

      public IMoBiCommand UpdateFormula(IEntity usingFormula, IFormula oldFormula, IFormula newFormula, FormulaDecoder formulaDecoder, IBuildingBlock buildingBlock)
      {
         var command = new EditObjectBasePropertyInBuildingBlockCommand(formulaDecoder.PropertyName, newFormula, oldFormula, usingFormula, buildingBlock).Run(_context);
         var quantity = usingFormula as IQuantity;
         if (quantity == null)
            return command;

         var updateCommand = _quantityTask.UpdateDefaultStateAndValueOriginFor(quantity, buildingBlock);
         if (updateCommand.IsEmpty() || updateCommand.IsEmptyMacro())
            return command;

         var macroCommand = new MoBiMacroCommand().WithHistoryEntriesFrom(command);
         macroCommand.Add(command);
         macroCommand.Add(updateCommand);
         return macroCommand;
      }

      public (IMoBiCommand command, IFormula formula) CreateNewFormulaInBuildingBlock(Type formulaType, IDimension formulaDimension, IEnumerable<string> existingFormulaNames, IBuildingBlock buildingBlock)
      {
         var newName = _dialogCreator.AskForInput(AppConstants.Captions.NewName, AppConstants.Captions.EnterNewFormulaName, string.Empty, existingFormulaNames);
         if (string.IsNullOrEmpty(newName))
            return (new MoBiEmptyCommand(), null);

         var formula = CreateNewFormula(formulaType, formulaDimension).WithName(newName);

         return (new AddFormulaToFormulaCacheCommand(buildingBlock, formula).Run(_context), formula);
      }

      public IFormula CreateNewFormula(Type formulaType, IDimension formulaDimension)
      {
         return createFormulaFromType(formulaType).WithDimension(formulaDimension);
      }

      public TFormula CreateNewFormula<TFormula>(IDimension formulaDimension) where TFormula : IFormula
      {
         return CreateNewFormula(typeof(TFormula), formulaDimension).DowncastTo<TFormula>();
      }

      public IMoBiCommand UpdateDistributedFormula(IDistributedParameter distributedParameter, DistributionFormula newDistributedFormula, string formulaType, IBuildingBlock buildingBlock)
      {
         _context.Register(newDistributedFormula);
         _context.Register(distributedParameter.Formula);
         var command = new UpdateDistributedFormulaCommand(distributedParameter, newDistributedFormula, formulaType, buildingBlock).Run(_context);
         return withUpdatedDefaultStateAndValueOrigin(command, newDistributedFormula, buildingBlock);
      }

      private IFormula createFormulaFromType(Type formulaType)
      {
         if (formulaType == typeof(ConstantFormula))
            return _context.Create<ConstantFormula>();

         if (formulaType == typeof(BlackBoxFormula))
            return _context.Create<BlackBoxFormula>();

         if (formulaType == typeof(TableFormula))
            return createTableFormula();

         if (formulaType == typeof(SumFormula))
            return _context.Create<SumFormula>();

         if (formulaType == typeof(TableFormulaWithOffset))
            return createTableFormulaWithOffset();

         if (formulaType == typeof(TableFormulaWithXArgument))
            return createTableFormulaWithXArgument();

         //default
         return _context.Create<ExplicitFormula>();
      }

      private IFormula createTableFormulaWithOffset()
      {
         var formula = _context.Create<TableFormulaWithOffset>();
         formula.OffsetObjectAlias = AppConstants.OFFSET_ALIAS;
         formula.TableObjectAlias = AppConstants.TABLE_ALIAS;
         return formula;
      }

      private IFormula createTableFormulaWithXArgument()
      {
         var formula = _context.Create<TableFormulaWithXArgument>();
         formula.XArgumentAlias = AppConstants.X_ARGUMENT_ALIAS;
         formula.TableObjectAlias = AppConstants.TABLE_ALIAS;
         return formula;
      }

      private IFormula createTableFormula()
      {
         var formula = _context.Create<TableFormula>();
         formula.UseDerivedValues = false;
         formula.XDimension = _context.DimensionFactory.Dimension(Constants.Dimension.TIME);
         formula.XDisplayUnit = formula.XDimension.DefaultUnit;
         formula.XName = formula.XDimension.DisplayName;
         formula.YName = AppConstants.Captions.DisplayNameYValue;
         formula.AddPoint(0, 0);
         return formula;
      }
   }
}