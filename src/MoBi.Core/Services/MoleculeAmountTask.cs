using System.Collections.Generic;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Services
{
   public interface IMoleculeAmountTask
   {
      /// <summary>
      ///    Updates all scale factor values for the <see cref="MoleculeAmount" /> that can be matched in the
      ///    <paramref name="simulation" />.
      ///    Matching is done by path.
      /// </summary>
      /// <param name="simulation">The simulation containing all  <see cref="MoleculeAmount" /> to update</param>
      /// <param name="scaleDivisors">Collection of scale factors that should be pushed in the simulation</param>
      IMoBiCommand UpdateScaleDivisors(IMoBiSimulation simulation, IReadOnlyCollection<ScaleDivisor> scaleDivisors);

      /// <summary>
      ///    Set the scale factor of the <paramref name="moleculeAmount" /> to <paramref name="newScaleDivisor" />
      /// </summary>
      /// <param name="simulation">Simulation containing the <paramref name="moleculeAmount" /> to update</param>
      /// <param name="moleculeAmount">Molecule amount whose scale factor will be updated</param>
      /// <param name="newScaleDivisor">Scale factor value to set in the molecule amount</param>
      IMoBiCommand UpdateScaleDivisor(IMoBiSimulation simulation, IMoleculeAmount moleculeAmount, double newScaleDivisor);
   }

   public class MoleculeAmountTask : IMoleculeAmountTask
   {
      private readonly IMoBiContext _context;

      public MoleculeAmountTask(IMoBiContext context)
      {
         _context = context;
      }

      public IMoBiCommand UpdateScaleDivisors(IMoBiSimulation simulation, IReadOnlyCollection<ScaleDivisor> scaleDivisors)
      {
         return new UpdateMoleculeAmountScaleDivisorsInSimulationCommand(scaleDivisors, simulation).Run(_context);
      }

      public IMoBiCommand UpdateScaleDivisor(IMoBiSimulation simulation, IMoleculeAmount moleculeAmount, double newScaleDivisor)
      {
         return new UpdateMoleculeAmountScaleDivisorInSimulationCommand(moleculeAmount, newScaleDivisor, simulation).Run(_context);
      }
   }
}