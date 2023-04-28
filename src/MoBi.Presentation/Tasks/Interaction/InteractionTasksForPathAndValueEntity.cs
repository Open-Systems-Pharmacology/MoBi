using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForPathAndValueEntity<in TBuildingBlock, in TBuilder>
   {
      /// <summary>
      ///    Adds a new formula to the building block formula cache and assigns it to the builder
      /// </summary>
      /// <param name="buildingBlock">The building block that has the formula added and contains the builder</param>
      /// <param name="builder">the builder being updated with a new formula</param>
      /// <param name="referenceParameter"></param>
      /// <returns>The command used to modify the building block and buildere</returns>
      ICommand<IMoBiContext> AddNewFormulaAtBuildingBlock(TBuildingBlock buildingBlock, TBuilder builder, IParameter referenceParameter);

      /// <summary>
      ///    Sets the display unit of a builder
      /// </summary>
      /// <param name="buildingBlock">The building block that contains the builder</param>
      /// <param name="builder">The builder being modified</param>
      /// <param name="newUnit">The new display unit</param>
      /// <returns>The command used to modify the builder</returns>
      IMoBiCommand SetUnit(TBuildingBlock buildingBlock, TBuilder builder, Unit newUnit);

      /// <summary>
      ///    Sets the formula for a builder
      /// </summary>
      /// <param name="buildingBlock">The building block that contains the builder</param>
      /// <param name="builder">The builder being modified</param>
      /// <param name="formula">The new formula for the builder</param>
      /// <returns>The command used to modify the builder</returns>
      IMoBiCommand SetFormula(TBuildingBlock buildingBlock, TBuilder builder, IFormula formula);

      /// <summary>
      ///    Sets the value of a builder
      /// </summary>
      /// <param name="buildingBlock">The building block that contains the builder</param>
      /// <param name="valueInDisplayUnit">The new value in display units</param>
      /// <param name="builder">The builder being modified</param>
      /// <returns>The command used to modify the builder</returns>
      IMoBiCommand SetValue(TBuildingBlock buildingBlock, double? valueInDisplayUnit, TBuilder builder);
   }

   public abstract class InteractionTasksForPathAndValueEntity<TBuildingBlock, TBuilder> : InteractionTasksForEnumerableBuildingBlock<TBuildingBlock, TBuilder>, IInteractionTasksForPathAndValueEntity<TBuildingBlock, TBuilder>
      where TBuildingBlock : class, IBuildingBlock, IBuildingBlock<TBuilder>
      where TBuilder : PathAndValueEntity, IUsingFormula, IWithDisplayUnit
   {
      protected readonly IMoBiFormulaTask _moBiFormulaTask;

      protected InteractionTasksForPathAndValueEntity(IInteractionTaskContext interactionTaskContext, IEditTasksForBuildingBlock<TBuildingBlock> editTask, IMoBiFormulaTask moBiFormulaTask) : base(interactionTaskContext, editTask)
      {
         _moBiFormulaTask = moBiFormulaTask;
      }

      protected IMoBiCommand AddFormulaToFormulaCacheAndSetOnBuilder<TFormula>(TBuildingBlock buildingBlock, TBuilder builder, IParameter referenceParameter)
         where TFormula : IFormula
      {
         var macroCommand = new MoBiMacroCommand
         {
            CommandType = AppConstants.Commands.AddCommand,
            Description = AppConstants.Commands.AddFormulaToBuildingBlock,
            ObjectType = _interactionTaskContext.GetTypeFor<TFormula>()
         };

         var newFormula = _moBiFormulaTask.CreateNewFormula<TFormula>(builder.Dimension);

         macroCommand.AddCommand(new AddFormulaToFormulaCacheCommand(buildingBlock, newFormula).Run(Context));

         if (!_moBiFormulaTask.EditNewFormula(newFormula, macroCommand, buildingBlock, referenceParameter))
            return CancelCommand(macroCommand);

         macroCommand.Add(SetFormula(buildingBlock, builder, newFormula, shouldClearValue: ValueFromBuilder(builder).HasValue));
         return macroCommand;
      }

      public IMoBiCommand SetFormula(TBuildingBlock buildingBlock, TBuilder builder, IFormula formula)
      {
         return SetFormula(buildingBlock, builder, formula, shouldClearValue: builder.Value.HasValue);
      }

      protected IMoBiCommand setFormula(TBuildingBlock buildingBlock, TBuilder builder, IFormula formula)
      {
         return ChangeValueFormulaCommand(buildingBlock, builder, formula);
      }

      protected IMoBiCommand SetFormula(TBuildingBlock buildingBlock, TBuilder builder, IFormula formula, bool shouldClearValue)
      {
         var macroCommand = new MoBiMacroCommand
         {
            CommandType = AppConstants.Commands.EditCommand,
            ObjectType = _interactionTaskContext.GetTypeFor<TBuilder>(),
            Description = AppConstants.Commands.SetValueAndFormula
         };

         macroCommand.Add(setFormula(buildingBlock, builder, formula));

         if (shouldClearValue)
            macroCommand.Add(setValue(builder, null, builder.DisplayUnit, buildingBlock));

         return macroCommand;
      }

      public IMoBiCommand SetDisplayValueWithUnit(TBuilder builder, double? newDisplayValue, Unit unit, TBuildingBlock buildingBlock)
      {
         return SetValueWithUnit(builder, unit.UnitValueToBaseUnitValue(newDisplayValue.GetValueOrDefault(double.NaN)), unit, buildingBlock);
      }

      protected IMoBiCommand setValue(TBuilder builder, double? newDisplayValue, Unit unit, TBuildingBlock buildingBlock)
      {
         return SetDisplayValueWithUnit(builder, newDisplayValue, unit, buildingBlock);
      }

      public IMoBiCommand SetValue(TBuildingBlock buildingBlock, double? valueInDisplayUnit, TBuilder builder)
      {
         var macroCommand = new MoBiMacroCommand
         {
            CommandType = AppConstants.Commands.EditCommand,
            ObjectType = _interactionTaskContext.GetTypeFor<TBuilder>(),
            Description = AppConstants.Commands.SetValueAndFormula
         };

         macroCommand.Add(setValue(builder, valueInDisplayUnit, builder.DisplayUnit, buildingBlock));
         if (builder.Formula != null)
            macroCommand.Add(setFormula(buildingBlock, builder, null));
         return macroCommand;
      }

      public IMoBiCommand SetUnit(TBuildingBlock buildingBlock, TBuilder builder, Unit newUnit)
      {
         return setValue(builder, builder.ConvertToDisplayUnit(ValueFromBuilder(builder)), newUnit, buildingBlock);
      }

      public virtual ICommand<IMoBiContext> AddNewFormulaAtBuildingBlock(TBuildingBlock buildingBlock, TBuilder builder, IParameter referenceParameter)
      {
         var macroCommand = new MoBiMacroCommand
         {
            CommandType = AppConstants.Commands.EditCommand,
            ObjectType = _interactionTaskContext.GetTypeFor(builder),
            Description = AppConstants.Commands.SetValueAndFormula
         };

         macroCommand.Add(AddFormulaToFormulaCacheAndSetOnBuilder<ExplicitFormula>(buildingBlock, builder, referenceParameter));

         return macroCommand;
      }

      protected virtual double? ValueFromBuilder(TBuilder builder)
      {
         return builder.Value;
      }

      public IMoBiCommand ChangeValueFormulaCommand(TBuildingBlock buildingBlock, TBuilder builder, IFormula formula)
      {
         return new ChangeValueFormulaCommand<TBuilder>(buildingBlock, builder, formula, builder.Formula).Run(Context);
      }

      protected IMoBiCommand SetValueWithUnit(TBuilder builder, double? unitValueToBaseUnitValue, Unit unit, TBuildingBlock buildingBlock)
      {
         return new PathAndValueEntityValueOrUnitChangedCommand<TBuilder, TBuildingBlock>(builder, unitValueToBaseUnitValue, unit, buildingBlock).Run(Context);
      }
   }
}