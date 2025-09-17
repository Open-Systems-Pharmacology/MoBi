using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.UnitSystem;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Presentation.Tasks.Interaction
{
   public class InteractionTasksForDistributedParameter : InteractionTasksForChildren<IContainer, IDistributedParameter>
   {
      private readonly IMoBiDimensionFactory _dimensionFactory;
      private readonly IParameterFactory _parameterFactory;
      private readonly IDistributionFormulaFactory _distributionFormulaFactory;

      public InteractionTasksForDistributedParameter(IInteractionTaskContext interactionTaskContext, IEditTaskFor<IDistributedParameter> editTasks,
         IMoBiDimensionFactory dimensionFactory, IParameterFactory parameterFactory, IDistributionFormulaFactory distributionFormulaFactory)
         : base(interactionTaskContext, editTasks)
      {
         _dimensionFactory = dimensionFactory;
         _parameterFactory = parameterFactory;
         _distributionFormulaFactory = distributionFormulaFactory;
      }

      public override IMoBiCommand GetRemoveCommand(IDistributedParameter entityToRemove, IContainer parent, IBuildingBlock buildingBlock)
      {
         return new RemoveParameterFromContainerCommand(parent, entityToRemove, buildingBlock);
      }

      public override IMoBiCommand GetAddCommand(IDistributedParameter parameter, IContainer parent, IBuildingBlock buildingBlock)
      {
         return new AddParameterToContainerCommand(parent, parameter, buildingBlock);
      }

      public override IDistributedParameter CreateNewEntity(IContainer container)
      {
         var parameter = base.CreateNewEntity(container)
            .WithDimension(_dimensionFactory.TryGetDimension(_interactionTaskContext.UserSettings.ParameterDefaultDimension, fallBackDimension: _dimensionFactory.NoDimension));

         parameter.DisplayUnit = _interactionTaskContext.DisplayUnitFor(parameter);

         var percentile = _parameterFactory.CreateParameter(Constants.Distribution.PERCENTILE, AppConstants.DEFAULT_PERCENTILE, _dimensionFactory.Dimension(AppConstants.DimensionNames.FRACTION));
         var mean = _parameterFactory.CreateParameter(Constants.Distribution.MEAN, AppConstants.DEFAULT_PARAMETER_START_VALUE, parameter.Dimension);
         var deviation = _parameterFactory.CreateParameter(Constants.Distribution.DEVIATION, AppConstants.DEFAULT_PARAMETER_START_VALUE, parameter.Dimension);

         parameter.Add(percentile);
         parameter.Add(mean);
         parameter.Add(deviation);

         parameter.Formula = _distributionFormulaFactory.CreateNormalDistributionFormulaFor(parameter, mean, deviation);
         return parameter;
      }
   }
}