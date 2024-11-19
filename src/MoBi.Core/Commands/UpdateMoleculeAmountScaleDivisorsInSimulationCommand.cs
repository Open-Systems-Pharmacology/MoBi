using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Assets;
using MoBi.Core.Services;

namespace MoBi.Core.Commands
{
   public class UpdateMoleculeAmountScaleDivisorsInSimulationCommand : SimulationChangeCommandBase
   {
      private readonly List<ScaleDivisor> _oldScaleFactors;
      private IReadOnlyCollection<ScaleDivisor> _scaleFactors;

      public UpdateMoleculeAmountScaleDivisorsInSimulationCommand(IReadOnlyCollection<ScaleDivisor> scaleFactors, IMoBiSimulation simulation)
         : base(simulation)
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
         var allMoleculeAmounts = containerTask.CacheAllChildren<MoleculeAmount>(_simulation.Model.Root);
         var changeTracker = context.Resolve<IQuantityValueInSimulationChangeTracker>();
         
         foreach (var scaleDivisor in _scaleFactors)
         {
            var moleculeAmount = allMoleculeAmounts[scaleDivisor.QuantityPath];
            if (moleculeAmount == null) continue;

            if (ValueComparer.AreValuesEqual(moleculeAmount.ScaleDivisor, scaleDivisor.Value))
               continue;

            _oldScaleFactors.Add(new ScaleDivisor { QuantityPath = scaleDivisor.QuantityPath, Value = moleculeAmount.ScaleDivisor });
            changeTracker.TrackScaleChange(moleculeAmount, _simulation, x => x.ScaleDivisor = scaleDivisor.Value);
         }
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _scaleFactors = null;
      }
   }
}