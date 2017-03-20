using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Domain.Extensions
{
   public static class MoBiObjectBaseExtensions
   {
      public static bool CouldBeInMoleculeBuildingBlock(this IObjectBase objectbase)
      {
         return objectbase.IsAnImplementationOf<IMoleculeBuilder>() ||
                objectbase.IsAnImplementationOf<ITransportBuilder>() ||
                objectbase.IsAnImplementationOf<TransporterMoleculeContainer>() ||
                objectbase.IsAnImplementationOf<InteractionContainer>() ||
                objectbase.IsAnImplementationOf<IParameter>();
      }
   }
}