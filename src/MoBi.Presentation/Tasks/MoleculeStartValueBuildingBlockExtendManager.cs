using MoBi.Core.Domain.Model;
using MoBi.Presentation.Mappers;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;

namespace MoBi.Presentation.Tasks
{
   public interface IMoleculeStartValueBuildingBlockExtendManager : IExtendStartValuesManager<InitialCondition>
   {
   }

   public class MoleculeStartValueBuildingBlockExtendManager : ExtendStartValuesManager<InitialCondition>, IMoleculeStartValueBuildingBlockExtendManager
   {
      public MoleculeStartValueBuildingBlockExtendManager(
         IApplicationController applicationController,
         IMoleculeStartValueToObjectBaseSummaryDTOMapper dtoMapper,
         IMoBiContext context)
         : base(applicationController, dtoMapper, context)
      {
      }
   }
}