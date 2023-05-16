using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.UICommand
{
   internal interface IActiveTransporterMoelculeRetriever
   {
      TransporterMoleculeContainer GetTransporterMoleculeFrom(TransportBuilder transportBuilder);
   }

   internal class ActiveTransporterMoelculeRetriever : IActiveTransporterMoelculeRetriever
   {
      private readonly IMoBiContext _context;
      private readonly IBuildingBlockRepository _buildingBlockRepository;

      public ActiveTransporterMoelculeRetriever(IMoBiContext context, IBuildingBlockRepository buildingBlockRepository)
      {
         _context = context;
         _buildingBlockRepository = buildingBlockRepository;
      }

      public TransporterMoleculeContainer GetTransporterMoleculeFrom(TransportBuilder transportBuilder)
      {
         foreach (var moleculeBuildingBlock in _buildingBlockRepository.MoleculeBlockCollection)
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