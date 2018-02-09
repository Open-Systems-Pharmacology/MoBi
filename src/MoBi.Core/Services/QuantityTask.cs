using System;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Extensions;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Services
{
   public interface IQuantityTask : ISetParameterTask
   {
      /// <summary>
      ///    Set the dimension in the parameter and all distribution parameters
      /// </summary>
      ICommand SetDistributedParameterDimension(IDistributedParameter distributedParameter, IDimension dimension, IBuildingBlock buildingBlock);

      /// <summary>
      ///    Set the dimension in the parameter and all distribution parameters
      /// </summary>
      ICommand SetQuantityDimension(IQuantity quantity, IDimension dimension, IBuildingBlock buildingBlock);

      ICommand ResetQuantityValue(IQuantity quantity, IMoBiSimulation simulation);

      ICommand ResetQuantityValue(IQuantity quantity, IBuildingBlock buildingBlock);

      /// <summary>
      ///    Set the value in the IQuantity.
      /// </summary>
      /// <param name="quantity">Quantity</param>
      /// <param name="valueInDisplayUnit">Value in display unit</param>
      /// <param name="buildingBlock">The building block that the <paramref name="quantity" />belongs to</param>
      /// ///
      /// <returns>The command used to modify the quantity</returns>
      ICommand SetQuantityDisplayValue(IQuantity quantity, double valueInDisplayUnit, IBuildingBlock buildingBlock);

      /// <summary>
      ///    Set the value in the IQuantity.
      /// </summary>
      /// <param name="quantity">Quantity</param>
      /// <param name="valueInDisplayUnit">Value in display unit</param>
      /// <param name="simulation">The simulation that the <paramref name="quantity" />belongs to</param>
      /// ///
      /// <returns>The command used to modify the quantity</returns>
      ICommand SetQuantityDisplayValue(IQuantity quantity, double valueInDisplayUnit, IMoBiSimulation simulation);

      /// <summary>
      ///    Set the current unit in the parameter.
      /// </summary>
      /// <param name="quantity">The quantity being modified</param>
      /// <param name="displayUnit">The new display unit</param>
      /// <param name="buildingBlock">Building block that the quantity belongs to</param>
      ICommand SetQuantityDisplayUnit(IQuantity quantity, Unit displayUnit, IBuildingBlock buildingBlock);

      ICommand SetQuantityDisplayUnit(IQuantity quantity, Unit displayUnit, IMoBiSimulation simulation);

      /// <summary>
      ///    Set the quantity in base units in a simulation
      /// </summary>
      /// <param name="quantity">The quanitity being updated</param>
      /// <param name="valueInBaseUnit">The new value in base units</param>
      /// <param name="simulation">The simulation that is targeted by the update</param>
      /// <returns></returns>
      ICommand SetQuantityBaseValue(IQuantity quantity, double valueInBaseUnit, IMoBiSimulation simulation);

      ICommand UpdateDefaultStateAndValueOriginFor(IQuantity quantity, IBuildingBlock buildingBlock);

      /// <summary>
      ///    Updates the value origin in the provided <paramref name="quantity"/>.
      /// </summary>
      /// <param name="quantity">Quantity</param>
      /// <param name="newValueOrigin">Value origin to use </param>
      /// <param name="buildingBlock">The building block that the <paramref name="quantity" />belongs to</param>
      /// ///
      /// <returns>The command used to update the value origin</returns>
      ICommand UpdateQuantityValueOriginInBuildingBlock(IQuantity quantity, ValueOrigin newValueOrigin, IBuildingBlock buildingBlock);

      /// <summary>
      ///    Updates the value origin in the provided <paramref name="quantity"/>.
      /// </summary>
      /// <param name="quantity">Quantity</param>
      /// <param name="newValueOrigin">Value origin to use </param>
      /// <param name="simulation">The simulation that the <paramref name="quantity" />belongs to</param>
      /// ///
      /// <returns>The command used to update the value origin</returns>
      ICommand UpdateQuantityValueOriginInSimulation(IQuantity quantity, ValueOrigin newValueOrigin, IMoBiSimulation simulation);

   }

   public class QuantityTask : IQuantityTask
   {
      private readonly IMoBiContext _context;
      private readonly IQuantitySynchronizer _quantitySynchronizer;

      public QuantityTask(IMoBiContext context, IQuantitySynchronizer quantitySynchronizer)
      {
         _context = context;
         _quantitySynchronizer = quantitySynchronizer;
      }

      public ICommand SetQuantityDisplayValue(IQuantity quantity, double valueInDisplayUnit, IBuildingBlock buildingBlock)
      {
         var valueInBaseUnit = quantity.ConvertToBaseUnit(valueInDisplayUnit);
         return withUpdatedDefaultStateAndValueOrigin(new SetQuantityValueInBuildingBlockCommand(quantity, valueInBaseUnit, buildingBlock).Run(_context), quantity, buildingBlock);
      }

      public ICommand SetQuantityDisplayValue(IQuantity quantity, double valueInDisplayUnit, IMoBiSimulation simulation)
      {
         var valueInBaseUnit = quantity.ConvertToBaseUnit(valueInDisplayUnit);
         return SetQuantityBaseValue(quantity, valueInBaseUnit, simulation);
      }

      public ICommand SetQuantityBaseValue(IQuantity quantity, double valueInBaseUnit, IMoBiSimulation simulation)
      {
         return synchronizedCommand(quantity, simulation, new SetQuantityValueInSimulationCommand(quantity, valueInBaseUnit, simulation));
      }

      public ICommand SetQuantityDisplayUnit(IQuantity quantity, Unit displayUnit, IBuildingBlock buildingBlock)
      {
         return withUpdatedDefaultStateAndValueOrigin(new SetQuantityUnitInBuildingBlockCommand(quantity, displayUnit, buildingBlock).Run(_context), quantity, buildingBlock);
      }

      public ICommand SetQuantityDisplayUnit(IQuantity quantity, Unit displayUnit, IMoBiSimulation simulation)
      {
         return synchronizedCommand(quantity, simulation, new SetQuantityUnitInSimulationCommand(quantity, displayUnit, simulation));
      }

      private ICommand withUpdatedDefaultStateAndValueOrigin(IOSPSuiteCommand executedCommand, IQuantity quantity, IBuildingBlock buildingBlock)
      {
         return withUpdatedDefaultStateAndValueOrigin(executedCommand, quantity, buildingBlock, setParameterDefaultStateInBuildingBlock, UpdateQuantityValueOriginInBuildingBlock);
      }

      private ICommand withUpdatedDefaultStateAndValueOrigin(IOSPSuiteCommand executedCommand, IQuantity quantity, IMoBiSimulation simulation)
      {
         return withUpdatedDefaultStateAndValueOrigin(executedCommand, quantity, simulation, setParameterDefaultStateInSimulation, UpdateQuantityValueOriginInSimulation);
      }

      private ICommand withUpdatedDefaultStateAndValueOrigin<T>(
         IOSPSuiteCommand executedCommand,
         IQuantity quantity,
         T buildingBlockOrSimulation,
         Func<IParameter, bool, T, ICommand> setParameterDefaultStateFunc,
         Func<IParameter, ValueOrigin, T, ICommand> setParameterValueOriginFunc
      )
      {
         if (executedCommand.IsEmpty() || executedCommand.IsEmptyMacro())
            return executedCommand;

         var updateCommand = new MoBiMacroCommand();
         addUpdateDefaultStateAndValueOriginCommand(updateCommand, quantity, buildingBlockOrSimulation, setParameterDefaultStateFunc, setParameterValueOriginFunc);

         if (updateCommand.IsEmtpy)
            return executedCommand;

         var macroCommand = new MoBiMacroCommand().WithHistoryEntriesFrom(executedCommand);
         macroCommand.Add(executedCommand);
         macroCommand.AddRange(updateCommand.All());
         return macroCommand;
      }

      public ICommand UpdateDefaultStateAndValueOriginFor(IQuantity quantity, IBuildingBlock buildingBlock)
      {
         var macroCommand = new MoBiMacroCommand();
         addUpdateDefaultStateAndValueOriginCommand(macroCommand, quantity, buildingBlock, setParameterDefaultStateInBuildingBlock, UpdateQuantityValueOriginInBuildingBlock);
         return macroCommand;
      }

      private void addUpdateDefaultStateAndValueOriginCommand<T>(
         MoBiMacroCommand macroCommand,
         IQuantity quantity,
         T buildingBlockOrSimulation,
         Func<IParameter, bool, T, ICommand> setParameterDefaultStateFunc,
         Func<IParameter, ValueOrigin, T, ICommand> setParameterValueOriginFunc)
      {
         var parameter = quantity as IParameter;

         if (parameter == null || !parameter.IsDefault)
            return;

         var setParameterDefaultStateCommand = setParameterDefaultStateFunc(parameter, false, buildingBlockOrSimulation).AsHidden();
         macroCommand.Add(setParameterDefaultStateCommand);

         if (!parameter.ValueOrigin.IsUndefined)
            return;

         var undefinedValueOrigin = new ValueOrigin
         {
            Source = ValueOriginSources.Unknown,
            Method = ValueOriginDeterminationMethods.Undefined
         };

         var setValueOriginCommand = setParameterValueOriginFunc(parameter, undefinedValueOrigin, buildingBlockOrSimulation).AsHidden();
         macroCommand.Add(setValueOriginCommand);
      }

      public ICommand UpdateQuantityValueOriginInBuildingBlock(IQuantity quantity, ValueOrigin newValueOrigin, IBuildingBlock buildingBlock)
      {
         return new UpdateValueOriginInBuildingBlockCommand(quantity, newValueOrigin, buildingBlock).Run(_context);
      }

      public ICommand UpdateQuantityValueOriginInSimulation(IQuantity quantity, ValueOrigin newValueOrigin, IMoBiSimulation simulation)
      {
         return new UpdateValueOriginInSimulationCommand(quantity, newValueOrigin, simulation).Run(_context);
      }

      private ICommand setParameterDefaultStateInBuildingBlock(IParameter parameter, bool defaultState, IBuildingBlock buildingBlock)
      {
         return new SetParameterDefaultStateInBuildingBlockCommand(parameter, defaultState, buildingBlock).Run(_context);
      }

      private ICommand setParameterDefaultStateInSimulation(IParameter parameter, bool defaultState, IMoBiSimulation simulation)
      {
         return new SetParameterDefaultStateInSimulationCommand(parameter, defaultState, simulation).Run(_context);
      }

      private ICommand synchronizedCommand(IQuantity quantity, IMoBiSimulation simulation, IMoBiCommand simulationCommand)
      {
         var macroCommand = new MoBiMacroCommand();

         //add one before setting the value in the simulation to enable correct undo
         macroCommand.Add(_quantitySynchronizer.Synchronize(quantity, simulation));
         macroCommand.Add(withUpdatedDefaultStateAndValueOrigin(simulationCommand.AsHidden().Run(_context), quantity, simulation));
         macroCommand.Add(_quantitySynchronizer.Synchronize(quantity, simulation));

         //needs to be done at the end because description might be set only after run
         return macroCommand.WithHistoryEntriesFrom(simulationCommand);
      }

      public ICommand SetDistributedParameterDimension(IDistributedParameter distributedParameter, IDimension dimension, IBuildingBlock buildingBlock)
      {
         return new SetDistributedParameterDimensionCommand(distributedParameter, dimension, buildingBlock).Run(_context);
      }

      public ICommand SetQuantityDimension(IQuantity quantity, IDimension dimension, IBuildingBlock buildingBlock)
      {
         if (quantity.IsAnImplementationOf<IDistributedParameter>())
            return SetDistributedParameterDimension(quantity.DowncastTo<IDistributedParameter>(), dimension, buildingBlock);

         return new SetQuantityDimensionCommand(quantity, dimension, buildingBlock).Run(_context);
      }

      public ICommand ResetQuantityValue(IQuantity quantity, IMoBiSimulation simulation)
      {
         return synchronizedCommand(quantity, simulation, new ResetQuantityValueInSimulationCommand(quantity, simulation));
      }

      public ICommand ResetQuantityValue(IQuantity quantity, IBuildingBlock buildingBlock)
      {
         return new ResetQuantityValueInBuildingBlockCommand(quantity, buildingBlock).Run(_context);
      }

      public ICommand SetParameterValue(IParameter parameter, double value, ISimulation simulation)
      {
         return SetQuantityBaseValue(parameter, value, simulation.DowncastTo<IMoBiSimulation>());
      }

      public ICommand UpdateParameterValueOrigin(IParameter parameter, ValueOrigin valueOrigin, ISimulation simulation)
      {
         return UpdateQuantityValueOriginInSimulation(parameter, valueOrigin, simulation.DowncastTo<IMoBiSimulation>());
      }
   }
}