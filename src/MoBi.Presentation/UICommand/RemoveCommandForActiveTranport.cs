using System.Linq;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.UICommand
{
   internal interface IActiveTransporterMoelculeRetriever
   {
      TransporterMoleculeContainer GetTransporterMoleculeFrom(ITransportBuilder transportBuilder);
   }

   internal class ActiveTransporterMoelculeRetriever : IActiveTransporterMoelculeRetriever
   {
      private readonly IMoBiContext _context;

      public ActiveTransporterMoelculeRetriever(IMoBiContext context)
      {
         _context = context;
      }

      public TransporterMoleculeContainer GetTransporterMoleculeFrom(ITransportBuilder transportBuilder)
      {
         foreach (var moleculeBuildingBlock in _context.CurrentProject.MoleculeBlockCollection)
         {
            foreach (var moleculeBuider in moleculeBuildingBlock)
            {
               var transporteMolecule = moleculeBuider.TransporterMoleculeContainerCollection
                  .FirstOrDefault(x => x.ActiveTransportRealizations.Contains(transportBuilder));

               if (transporteMolecule != null)
                  return transporteMolecule;
            }
         }
         return null;
      }
   }
}