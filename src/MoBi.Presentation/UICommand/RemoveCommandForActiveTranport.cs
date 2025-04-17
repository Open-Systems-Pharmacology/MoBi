using System.Linq;
using OSPSuite.Core.Domain.Builder;
using IBuildingBlockRepository = MoBi.Core.Domain.Repository.IBuildingBlockRepository;

namespace MoBi.Presentation.UICommand
{
   internal interface IActiveTransporterMoleculeRetriever
   {
      TransporterMoleculeContainer GetTransporterMoleculeFrom(TransportBuilder transportBuilder);
   }

   internal class ActiveTransporterMoleculeRetriever : IActiveTransporterMoleculeRetriever
   {
      private readonly IBuildingBlockRepository _buildingBlockRepository;

      public ActiveTransporterMoleculeRetriever(IBuildingBlockRepository buildingBlockRepository)
      {
         _buildingBlockRepository = buildingBlockRepository;
      }

      public TransporterMoleculeContainer GetTransporterMoleculeFrom(TransportBuilder transportBuilder)
      {
         foreach (var moleculeBuildingBlock in _buildingBlockRepository.MoleculeBlockCollection)
         {
            foreach (var moleculeBuilder in moleculeBuildingBlock)
            {
               var transporterMolecule = moleculeBuilder.TransporterMoleculeContainerCollection
                  .FirstOrDefault(x => x.ActiveTransportRealizations.Contains(transportBuilder));

               if (transporterMolecule != null)
                  return transporterMolecule;
            }
         }

         return null;
      }
   }
}