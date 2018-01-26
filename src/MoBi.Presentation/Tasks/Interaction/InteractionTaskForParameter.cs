using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Domain.UnitSystem;
using MoBi.Core.Exceptions;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForParameter : IInteractionTasksForChildren<IContainer, IParameter>
   {
      /// <summary>
      ///    Sets the build mode for a parameter. The command is not run during execution.
      /// </summary>
      /// <param name="parameter">The parameter being edited</param>
      /// <param name="buildingBlock">The building block containing the parameter</param>
      /// <param name="newMode">the new build mode</param>
      IMoBiCommand SetBuildModeForParameter(IParameter parameter, ParameterBuildMode newMode, IBuildingBlock buildingBlock);

      /// <summary>
      ///    Sets the dimension for a parameter. The command is run during execution.
      /// </summary>
      /// <param name="parameter">The parameter being edited</param>
      /// <param name="newDimension">The new dimension</param>
      /// <param name="buildingBlock">The building block containing the parameter</param>
      IMoBiCommand SetDimensionForParameter(IParameter parameter, IDimension newDimension, IBuildingBlock buildingBlock);

      /// <summary>
      /// Creates but does not run a command that changes the RHS formula to null for the <paramref name="parameter"/> in the <paramref name="buildingBlock"/>
      /// </summary>
      /// <returns>The command that was created</returns>
      IMoBiCommand ResetRHSFormulaFor(IParameter parameter, IBuildingBlock buildingBlock);

      /// <summary>
      ///    Sets the name for a parameter. The command is not run during execution.
      /// </summary>
      /// <param name="parameter">The parameter being edited</param>
      /// <param name="buildingBlock">The building block containing the parameter</param>
      /// <param name="newName">The new name</param>
      IMoBiCommand SetNameForParameter(IParameter parameter, string newName, IBuildingBlock buildingBlock);

      /// <summary>
      ///    Sets the description for a parameter
      /// </summary>
      /// <param name="parameter">The parameter being edited</param>
      /// <param name="buildingBlock">The building block containing the parameter</param>
      /// <param name="newDescription">The new description</param>
      IMoBiCommand SetDescriptionForParameter(IParameter parameter, string newDescription, IBuildingBlock buildingBlock);

      /// <summary>
      ///    Sets the value origin for a parameter
      /// </summary>
      ICommand SetValueOriginForParameter(IParameter parameter, ValueOrigin valueOrigin);
   }

   public class InteractionTasksForParameter : InteractionTasksForChildren<IContainer, IParameter>, IInteractionTasksForParameter
   {
      private readonly IMoBiDimensionFactory _dimensionFactory;
      private readonly IMoBiFormulaTask _formulaTask;

      public InteractionTasksForParameter(IInteractionTaskContext interactionTaskContext, IEditTaskFor<IParameter> editTasks, IMoBiDimensionFactory dimensionFactory, IMoBiFormulaTask formulaTask) :
         base(interactionTaskContext, editTasks)
      {
         _dimensionFactory = dimensionFactory;
         _formulaTask = formulaTask;
      }

      public override IParameter CreateNewEntity(IContainer parent)
      {
         var parameter = base.CreateNewEntity(parent)
            .WithParentContainer(parent)
            .WithDimension(_dimensionFactory.TryGetDimension(_interactionTaskContext.UserSettings.ParameterDefaultDimension))
            .WithMode(parent.DefaultParameterBuildMode())
            .WithGroup(Constants.Groups.MOBI);

         parameter.Formula = _formulaTask.CreateNewFormula<ConstantFormula>(parameter.Dimension);
         parameter.DisplayUnit = _interactionTaskContext.DisplayUnitFor(parameter);
         parameter.Visible = true;
         return parameter;
      }


      public override IMoBiCommand Remove(IParameter parameter, IContainer container, IBuildingBlock buildingBlock, bool silent)
      {
         if (!parameterCanBeRemoved(parameter, container))
            throw new MoBiException(AppConstants.Exceptions.CannotRemoveParameter(parameter.Name, container.Name, _interactionTaskContext.InteractionTask.TypeFor(container)));

         return base.Remove(parameter, container, buildingBlock, silent);
      }

      public IMoBiCommand SetBuildModeForParameter(IParameter parameter, ParameterBuildMode newMode, IBuildingBlock buildingBlock)
      {
         return new EditParameterBuildModeInBuildingBlockCommand(newMode, parameter, buildingBlock);
      }

      public IMoBiCommand SetDimensionForParameter(IParameter parameter, IDimension newDimension, IBuildingBlock buildingBlock)
      {
         return new SetParameterDimensionInBuildingBlockCommand(parameter, newDimension, buildingBlock).Run(Context);
      }

      public IMoBiCommand ResetRHSFormulaFor(IParameter parameter, IBuildingBlock buildingBlock)
      {
         if (parameter.RHSFormula==null)
            return new MoBiEmptyCommand();

         return new EditParameterRHSFormulaInBuildingBlockCommand(null, parameter.RHSFormula, parameter, buildingBlock).Run(Context);
      }

      public IMoBiCommand SetNameForParameter(IParameter parameter, string newName, IBuildingBlock buildingBlock)
      {
         return new EditParameterNameInBuildingBlockCommand(newName, parameter.Name, parameter, buildingBlock);
      }

      public IMoBiCommand SetDescriptionForParameter(IParameter parameter, string newDescription, IBuildingBlock buildingBlock)
      {
         return new EditParameterDescriptionInBuildingBlockComand(newDescription, parameter.Description, parameter, buildingBlock);
      }

      public ICommand SetValueOriginForParameter(IParameter parameter, ValueOrigin valueOrigin)
      {
         return new UpdateValueOriginCommand(valueOrigin, parameter, Context).Run(Context);
      }

      private bool parameterCanBeRemoved(IParameter parameter, IContainer container)
      {
         if (container == null)
            return true;

         if (parameter.IsNamed(Constants.Parameters.VOLUME) && container.Mode == ContainerMode.Physical)
            return false;

         if (parameter.IsNamed(Constants.Parameters.CONCENTRATION) && container.IsAnImplementationOf<IMoleculeBuilder>())
            return false;

         return true;
      }

      public override IMoBiCommand GetRemoveCommand(IParameter entityToRemove, IContainer parent, IBuildingBlock buildingBlock)
      {
         return new RemoveParameterFromContainerCommand(parent, entityToRemove, buildingBlock);
      }

      protected override void SetAddCommandDescription(IParameter newEntity, IContainer parent, IMoBiCommand addCommand, MoBiMacroCommand macroCommand, IBuildingBlock buildingBlock)
      {
         addCommand.Description = AppConstants.Commands.AddParameterToContainerDescription(parent.EntityPath(), newEntity.Name, buildingBlock.Name);
         macroCommand.Description = addCommand.Description;
      }

      public override IMoBiCommand GetAddCommand(IParameter parameter, IContainer parent, IBuildingBlock buildingBlock)
      {
         return new AddParameterToContainerCommand(parent, parameter, buildingBlock);
      }
   }
}