using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Helper;
using MoBi.Core.Services;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.Tasks
{
   public class MoBiFormulaTask : IMoBiFormulaTask
   {
      private readonly IMoBiContext _context;
      private readonly IMoBiApplicationController _applicationController;
      private readonly IFormulaTask _formulaTask;
      private readonly INameCorrector _nameCorrector;
      private readonly IDialogCreator _dialogCreator;

      public MoBiFormulaTask(IMoBiContext context, IMoBiApplicationController applicationController, IFormulaTask formulaTask, INameCorrector nameCorrector, IDialogCreator dialogCreator)
      {
         _context = context;
         _applicationController = applicationController;
         _formulaTask = formulaTask;
         _nameCorrector = nameCorrector;
         _dialogCreator = dialogCreator;
       }

      public bool EditFormula(IFormula formula, ICommandCollector command, IBuildingBlock buildingBlock, IParameter parameter)
      {
         using (var newFormulaPresenter = _applicationController.Start<INewFormulaPresenter>())
         {
            newFormulaPresenter.InitializeWith(command);
            return newFormulaPresenter.Edit(formula, buildingBlock, parameter);
         }
      }

      public IMoBiCommand AddFormulaToCacheOrFixReferenceCommand(IBuildingBlock targetBuildingBlock, IUsingFormula usingFormulaObject)
      {
         var decoder = new UsingFormulaDecoder();

         return AddFormulaToCacheOrFixReferenceCommand(targetBuildingBlock, usingFormulaObject, decoder);
      }

      public IMoBiCommand AddFormulaToCacheOrFixReferenceCommand<T>(IBuildingBlock targetBuildingBlock, T usingFormulaObject, FormulaDecoder<T> decoder)
      {
         if (decoder.GetFormula(usingFormulaObject) == null)
            return new MoBiEmptyCommand();

         var existingFormula = targetBuildingBlock.FormulaCache.FindByName(decoder.GetFormula(usingFormulaObject).Name);

         // There is no existing formula in the cache with the same name
         if (existingFormula == null)
         {
            return new AddFormulaToFormulaCacheCommand(targetBuildingBlock, decoder.GetFormula(usingFormulaObject));
         }

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

         if (usingObject.IsAnImplementationOf<ITransportBuilder>())
            return AppConstants.Captions.AmountRightHandSide;

         if (usingObject.IsAnImplementationOf<IParameter>() && isRHS)
            return AppConstants.Captions.ParameterRightHandSide(usingObject.Name);

         if (usingObject.IsAnImplementationOf<IReactionBuilder>())
            return reactionMode == ReactionDimensionMode.AmountBased ? AppConstants.Captions.AmountRightHandSide : AppConstants.Captions.ConcentrationRightHandSide;

         return string.Empty;
      }

      public IMoBiCommand SetFormulaString(ExplicitFormula formula, string newFormulaString, string oldFormulaString, IBuildingBlock buildingBlock)
      {
         return new EditFormulaStringCommand(newFormulaString, oldFormulaString, formula, buildingBlock).Run(_context);
      }

      public IMoBiCommand EditAliasInFormula(ExplicitFormula formula, string newAlias, string oldAlias, IFormulaUsablePath formulaUsablePath, IBuildingBlock buildingBlock)
      {
         return new EditFormulaAliasCommand(formula, newAlias, oldAlias, buildingBlock).Run(_context);
      }

      public ICommand SetFormulaPathDimension(ExplicitFormula formula, IDimension newDimension, string alias, IBuildingBlock buildingBlock)
      {
         return new UpdateDimensionOfFormulaUsablePathCommand(newDimension, formula, alias, buildingBlock).Run(_context);
      }

      public ICommand RemoveFormulaUsablePath(ExplicitFormula formula, IFormulaUsablePath path, IBuildingBlock buildingBlock)
      {
         return new RemoveFormulaUsablePathCommand(formula, path, buildingBlock).Run(_context);
      }

      public IMoBiCommand ChangePathInFormula(ExplicitFormula formula, ObjectPath newPath, IFormulaUsablePath formulaUsablePath, IBuildingBlock buildingBlock)
      {
         return new EditPathAtUsablePathCommand(formula, newPath, formulaUsablePath, buildingBlock).Run(_context);
      }

      public IMoBiCommand AddFormulaUsablePath(ExplicitFormula formula, IFormulaUsablePath path, IBuildingBlock buildingBlock)
      {
         return new AddFormulaUsablePathCommand(formula, path, buildingBlock).Run(_context);
      }

      public IMoBiCommand ChangeVariableName(SumFormula formula, string newVariableName, string oldVariableName, IBuildingBlock buildingBlock)
      {
         return new ChangeVariableNameCommand(formula, newVariableName, oldVariableName, buildingBlock).Run(_context);
      }

      public IMoBiCommand AddValuePoint(TableFormula formula, ValuePoint newValuePoint, IBuildingBlock buildingBlock)
      {
         return new AddValuePointCommand(formula, newValuePoint, buildingBlock).Run(_context);
      }

      public IMoBiCommand EditUseDerivedValues(TableFormula formula, bool newValue, bool oldValue, IBuildingBlock buildingBlock)
      {
         return new EditUseDerivedValuesCommand(formula, newValue, oldValue, buildingBlock).Run(_context);
      }

      public IMoBiCommand RemoveValuePointFromTableFormula(TableFormula formula, ValuePoint valuePoint, IBuildingBlock buildingBlock)
      {
         return new RemoveValuePointFromTableFormulaCommand(formula, valuePoint, buildingBlock).Run(_context);
      }

      public IMoBiCommand ChangeOffsetObject(TableFormulaWithOffset formula, IFormulaUsablePath path, IBuildingBlock buildingBlock)
      {
         return new ChangeTableFormulaWithOffsetOffsetObjectPathCommand(formula, path, buildingBlock).Run(_context);
      }

      public IMoBiCommand ChangeTableObject(TableFormulaWithOffset formula, IFormulaUsablePath path, IBuildingBlock buildingBlock)
      {
         return new ChangeTableFormulaWithOffsetTableObjectPathCommand(formula, path, buildingBlock).Run(_context);
      }

      public IMoBiCommand SetConstantFormulaValue(ConstantFormula formula, double kernelValue, Unit newDisplayUnit, Unit oldDisplayUnit, IBuildingBlock buildingBlock, IEntity formulaOwner)
      {
         return new SetConstantFormulaValueCommand(formula, kernelValue, newDisplayUnit, oldDisplayUnit, buildingBlock, formulaOwner).Run(_context);
      }

      public Tuple<IMoBiCommand, IFormula> CreateNewFormulaInBuildingBlock(Type formulaType, IDimension formulaDimension, IEnumerable<string> existingFormulaNames, IBuildingBlock buildingBlock)
      {
         var newName = _dialogCreator.AskForInput(AppConstants.Captions.NewName, AppConstants.Captions.EnterNewFormulaName, string.Empty, existingFormulaNames);
         if (string.IsNullOrEmpty(newName))
            return new Tuple<IMoBiCommand, IFormula>(new MoBiEmptyCommand(), null);

         var formula = CreateNewFormula(formulaType, formulaDimension).WithName(newName);

         return new Tuple<IMoBiCommand, IFormula>(new AddFormulaToFormulaCacheCommand(buildingBlock, formula).Run(_context), formula);
      }

      public IFormula CreateNewFormula(Type formulaType, IDimension formulaDimension)
      {
         return createFormulaFromType(formulaType).WithDimension(formulaDimension);
      }

      public TFormula CreateNewFormula<TFormula>(IDimension formulaDimension) where TFormula : IFormula
      {
         return CreateNewFormula(typeof (TFormula), formulaDimension).DowncastTo<TFormula>();
      }

      public IMoBiCommand UpdateDistributedFormula(IDistributedParameter distributedParameter, IDistributionFormula newDistributedFormula, string formulaType, IBuildingBlock buildingBlock)
      {
         _context.Register(newDistributedFormula);
         _context.Register(distributedParameter.Formula);
         return new UpdateDistributedFormulaCommand(distributedParameter, newDistributedFormula, formulaType, buildingBlock).Run(_context);
      }

      private IFormula createFormulaFromType(Type formulaType)
      {
         if (formulaType == typeof (ConstantFormula))
            return _context.Create<ConstantFormula>();

         if (formulaType == typeof (BlackBoxFormula))
            return _context.Create<BlackBoxFormula>();

         if (formulaType == typeof (TableFormula))
            return getTableFormula();

         if (formulaType == typeof (SumFormula))
            return _context.Create<SumFormula>();

         if (formulaType == typeof (TableFormulaWithOffset))
         {
            var tableFormulaWithOffset = _context.Create<TableFormulaWithOffset>();
            tableFormulaWithOffset.OffsetObjectAlias = AppConstants.OffsetAlias;
            tableFormulaWithOffset.TableObjectAlias = AppConstants.TableAlias;
            return tableFormulaWithOffset;
         }

         //default
         return _context.Create<ExplicitFormula>();
      }

      private IFormula getTableFormula()
      {
         var newFormula = _context.Create<TableFormula>();
         newFormula.UseDerivedValues = false;
         newFormula.XDimension = _context.DimensionFactory.Dimension(Constants.Dimension.TIME);
         newFormula.XDisplayUnit = newFormula.XDimension.DefaultUnit;
         newFormula.XName = newFormula.XDimension.DisplayName;
         newFormula.YName = AppConstants.Captions.DisplayNameYValue;
         newFormula.AddPoint(0, 0);
         return newFormula;
      }
   }
}