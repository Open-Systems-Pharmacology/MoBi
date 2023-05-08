using System.Collections.Generic;
using FluentNHibernate.Utils;
using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class UpdateMoleculeAmountScaleDivisorsInSimulationCommand : SimulationChangeCommandBase
   {
      private readonly List<ScaleDivisor> _oldScaleFactors;
      private IReadOnlyCollection<ScaleDivisor> _scaleFactors;

      public UpdateMoleculeAmountScaleDivisorsInSimulationCommand(IReadOnlyCollection<ScaleDivisor> scaleFactors, IMoBiSimulation simulation)
         : base(scaleFactors, simulation)
      {
         _scaleFactors = scaleFactors;
         _oldScaleFactors = new List<ScaleDivisor>();
         CommandType = AppConstants.Commands.EditCommand;
         ObjectType = ObjectTypes.Simulation;
         Description = AppConstants.Commands.UpdateScaleDivisorValuesInSimulation(simulation.Name);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new UpdateMoleculeAmountScaleDivisorsInSimulationCommand(_oldScaleFactors, _simulation).AsInverseFor(this);
      }

      protected override void DoExecute(IMoBiContext context)
      {
         var containerTask = context.Resolve<IContainerTask>();
         var initialConditionsBuildingBlockSynchronizer = context.Resolve<IQuantitySynchronizer>();
         var allMoleculeAmounts = containerTask.CacheAllChildren<MoleculeAmount>(_simulation.Model.Root);
         var startValueBuildingBlocks = _simulation.Configuration.All<InitialConditionsBuildingBlock>();

         foreach (var scaleDivisor in _scaleFactors)
         {
            var moleculeAmount = allMoleculeAmounts[scaleDivisor.QuantityPath];
            if (moleculeAmount == null) continue;

            if (ValueComparer.AreValuesEqual(moleculeAmount.ScaleDivisor, scaleDivisor.Value))
               continue;

            _oldScaleFactors.Add(new ScaleDivisor { QuantityPath = scaleDivisor.QuantityPath, Value = moleculeAmount.ScaleDivisor });
            moleculeAmount.ScaleDivisor = scaleDivisor.Value;
            startValueBuildingBlocks.Each(initialConditionsBuildingBlock => initialConditionsBuildingBlockSynchronizer.SynchronizeInitialConditions(moleculeAmount, initialConditionsBuildingBlock));
         }
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _scaleFactors = null;
      }
   }
}