using System.Collections.Generic;
using System.Data;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Extensions;
using MoBi.Core.Helper;
using MoBi.Core.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;
using OSPSuite.Utility;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForPathAndValueEntity<in TBuildingBlock, in TBuilder>
   {
      /// <summary>
      ///    Edits a formula to the building block formula cache and assigns it to the builder. If the formula is not set,
      ///    the formula is created for editing.
      /// </summary>
      /// <param name="buildingBlock">The building block that has the formula added and contains the builder</param>
      /// <param name="builder">the builder being updated with a new formula</param>
      /// <returns>The command used to modify the building block and builders</returns>
      ICommand<IMoBiContext> EditFormulaAtBuildingBlock(TBuildingBlock buildingBlock, TBuilder builder);

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

      /// <summary>
      ///    Sets the value origin of a path and value entity
      /// </summary>
      /// <param name="buildingBlock">The building block that contains the start value</param>
      /// <param name="valueOrigin">The new value origin</param>
      /// <param name="pathAndValueEntity">The start value being modified</param>
      /// <returns>The command used to modify the start value</returns>
      ICommand SetValueOrigin(TBuildingBlock buildingBlock, ValueOrigin valueOrigin, TBuilder pathAndValueEntity);

      IMoBiCommand ConvertDistributedParameterToConstantParameter(TBuilder distributedParameter, TBuildingBlock buildingBlock, IReadOnlyList<TBuilder> subParameters);

      /// <summary>
      ///    Exports the building block to an excel file
      /// </summary>
      /// <param name="subject">Building Block to export.</param>
      void ExportToExcel(TBuildingBlock subject);
   }

   public abstract class InteractionTasksForPathAndValueEntity<TParent, TBuildingBlock, TBuilder> : InteractionTasksForEnumerableBuildingBlock<TParent, TBuildingBlock, TBuilder>, IInteractionTasksForPathAndValueEntity<TBuildingBlock, TBuilder>
      where TBuildingBlock : class, IBuildingBlock, IBuildingBlock<TBuilder>, ILookupBuildingBlock<TBuilder>
      where TBuilder : PathAndValueEntity, IUsingFormula, IWithDisplayUnit where TParent : class, IObjectBase
   {
      protected readonly IMoBiFormulaTask _moBiFormulaTask;
      private readonly IExportDataTableToExcelTask _exportDataTableToExcelTask;
      private readonly IMapper<TBuildingBlock, List<DataTable>> _dataTableMapper;
      private readonly IPathAndValueEntityToDistributedParameterMapper _pathAndValueEntityToDistributedParameterMapper;
      protected readonly ICloneManagerForBuildingBlock _cloneManagerForBuildingBlock;

      protected InteractionTasksForPathAndValueEntity(IInteractionTaskContext interactionTaskContext,
         IEditTasksForBuildingBlock<TBuildingBlock> editTask,
         IMoBiFormulaTask moBiFormulaTask,
         IExportDataTableToExcelTask exportDataTableToExcelTask,
         IMapper<TBuildingBlock, List<DataTable>> dataTableMapper,
         IPathAndValueEntityToDistributedParameterMapper pathAndValueEntityToDistributedParameterMapper,
         ICloneManagerForBuildingBlock cloneManagerForBuildingBlock)
         : base(interactionTaskContext, editTask)
      {
         _moBiFormulaTask = moBiFormulaTask;
         _exportDataTableToExcelTask = exportDataTableToExcelTask;
         _dataTableMapper = dataTableMapper;
         _pathAndValueEntityToDistributedParameterMapper = pathAndValueEntityToDistributedParameterMapper;
         _cloneManagerForBuildingBlock = cloneManagerForBuildingBlock;
      }

      public ICommand SetValueOrigin(TBuildingBlock buildingBlock, ValueOrigin valueOrigin, TBuilder pathAndValueEntity)
      {
         return new UpdateValueOriginInPathAndValueEntityCommand<TBuilder>(pathAndValueEntity, valueOrigin, buildingBlock).RunCommand(Context);
      }

      public IMoBiCommand ConvertDistributedParameterToConstantParameter(TBuilder distributedParameter, TBuildingBlock buildingBlock, IReadOnlyList<TBuilder> subParameters)
      {
         if (distributedParameter.DistributionType == null)
         {
            return new MoBiEmptyCommand();
         }

         var objectType = new ObjectTypeResolver().TypeFor(distributedParameter);
         var moBiMacroCommand = new MoBiMacroCommand
         {
            CommandType = AppConstants.Commands.UpdateCommand,
            ObjectType = objectType,
            Description = AppConstants.Commands.ConvertDistributedPathAndValueEntityToConstantValue(objectType, distributedParameter.Path)
         };

         var temporaryParameter = _pathAndValueEntityToDistributedParameterMapper.MapFrom(distributedParameter, distributedParameter.DistributionType.Value, subParameters);

         foreach (var subParameter in subParameters)
         {
            moBiMacroCommand.Add(new RemovePathAndValueEntityFromBuildingBlockCommand<TBuilder>(buildingBlock, subParameter.Path));
         }

         moBiMacroCommand.Add(new PathAndValueEntityValueOrUnitChangedCommand<TBuilder, TBuildingBlock>(distributedParameter, temporaryParameter.Value, temporaryParameter.DisplayUnit, buildingBlock));

         return moBiMacroCommand.RunCommand(Context);
      }

      public void ExportToExcel(TBuildingBlock subject)
      {
         var currentProject = Context.CurrentProject;
         var projectName = currentProject.Name;
         if (string.IsNullOrEmpty(projectName))
            projectName = AppConstants.Undefined;

         var defaultFileName = AppConstants.DefaultFileNameForBuildingBlockExport(projectName, subject);
         var excelFileName = _interactionTaskContext.DialogCreator.AskForFileToSave(AppConstants.Captions.ExportToExcel, Constants.Filter.EXCEL_SAVE_FILE_FILTER, Constants.DirectoryKey.MODEL_PART, defaultFileName);

         if (string.IsNullOrEmpty(excelFileName))
            return;

         var mappedValues = _dataTableMapper.MapFrom(subject);
         _exportDataTableToExcelTask.ExportDataTablesToExcel(mappedValues, excelFileName, openExcel: false);
      }

      protected virtual string GetNewNameForClone(TBuildingBlock buildingBlockToClone)
      {
         var name = _interactionTaskContext.NamingTask.NewName(
            AppConstants.Dialog.AskForNewName(AppConstants.CloneName(buildingBlockToClone)),
            AppConstants.Captions.NewName,
            AppConstants.CloneName(buildingBlockToClone),
            _editTask.GetForbiddenNames(buildingBlockToClone, GetNamedObjectsInParent(buildingBlockToClone)));
         return name;
      }

      protected abstract IReadOnlyCollection<IObjectBase> GetNamedObjectsInParent(TBuildingBlock buildingBlockToClone);

      private IMoBiCommand editFormulaAndSetOnBuilder(TBuildingBlock buildingBlock, TBuilder builder)
      {
         var macroCommand = new MoBiMacroCommand
         {
            CommandType = AppConstants.Commands.AddCommand,
            Description = AppConstants.Commands.AddFormulaToBuildingBlock,
            ObjectType = _interactionTaskContext.GetTypeFor<TBuilder>()
         };

         using (var modalPresenter = ApplicationController.Start<IModalPresenter>())
         {
            var editFormulaPresenter = Context.Resolve<IEditFormulaInPathAndValuesPresenter>();
            modalPresenter.Encapsulate(editFormulaPresenter);
            modalPresenter.Text = AppConstants.Captions.EditFormula;
            var usingFormulaDecoder = new UsingFormulaDecoder();

            var clonedBuildingBlock = _cloneManagerForBuildingBlock.Clone(buildingBlock);
            var clonedBuilder = clonedBuildingBlock.ByPath(builder.Path);
            editFormulaPresenter.InitializeWith(new MoBiMacroCommand());
            editFormulaPresenter.Init(clonedBuilder, clonedBuildingBlock, usingFormulaDecoder);

            if (!modalPresenter.Show())
               return new MoBiEmptyCommand();

            var newFormula = clonedBuilder.Formula;
            if (newFormula is ConstantFormula constantFormula)
            {
               macroCommand.Add(setValue(builder, constantFormula.Value, constantFormula.Dimension.BaseUnit, buildingBlock));
            }
            else
            {
               macroCommand.Add(_interactionTaskContext.MoBiFormulaTask.UpdateFormula(builder, builder.Formula, newFormula, usingFormulaDecoder, buildingBlock));
               macroCommand.Add(_interactionTaskContext.MoBiFormulaTask.AddFormulaToCacheOrFixReferenceCommand(buildingBlock, builder).RunCommand(_interactionTaskContext.Context));
               macroCommand.Add(setValue(builder, null, builder.DisplayUnit, buildingBlock));
            }

            return macroCommand;
         }
      }

      public IMoBiCommand SetFormula(TBuildingBlock buildingBlock, TBuilder builder, IFormula formula)
      {
         return setFormula(buildingBlock, builder, formula, shouldClearValue: builder.Value.HasValue || builder.DistributionType.HasValue);
      }

      private IMoBiCommand setFormula(TBuildingBlock buildingBlock, TBuilder builder, IFormula formula)
      {
         return ChangeValueFormulaCommand(buildingBlock, builder, formula);
      }

      private IMoBiCommand setFormula(TBuildingBlock buildingBlock, TBuilder builder, IFormula formula, bool shouldClearValue)
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

      private IMoBiCommand setValue(TBuilder builder, double? newDisplayValue, Unit unit, TBuildingBlock buildingBlock)
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

      public virtual ICommand<IMoBiContext> EditFormulaAtBuildingBlock(TBuildingBlock buildingBlock, TBuilder builder)
      {
         var macroCommand = new MoBiMacroCommand
         {
            CommandType = AppConstants.Commands.EditCommand,
            ObjectType = _interactionTaskContext.GetTypeFor(builder),
            Description = AppConstants.Commands.SetValueAndFormula
         };

         macroCommand.Add(editFormulaAndSetOnBuilder(buildingBlock, builder));

         return macroCommand;
      }

      protected virtual double? ValueFromBuilder(TBuilder builder) => builder.Value;

      public IMoBiCommand ChangeValueFormulaCommand(TBuildingBlock buildingBlock, TBuilder builder, IFormula formula) => 
         new ChangeValueFormulaCommand<TBuilder>(buildingBlock, builder, formula, builder.Formula).RunCommand(Context);

      protected IMoBiCommand SetValueWithUnit(TBuilder builder, double? unitValueToBaseUnitValue, Unit unit, TBuildingBlock buildingBlock) => 
         new PathAndValueEntityValueOrUnitChangedCommand<TBuilder, TBuildingBlock>(builder, unitValueToBaseUnitValue, unit, buildingBlock).RunCommand(Context);
   }
}