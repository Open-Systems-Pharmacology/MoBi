using OSPSuite.Core.Domain.Builder;

namespace MoBi.R.Services
{
   public interface IIndividualTask
   {
      IndividualBuildingBlock CreateIndividual(string name);
   }

   public class IndividualTask : IIndividualTask
   {
      public IndividualBuildingBlock CreateIndividual(string name)
      {
         return new IndividualBuildingBlock { Name = name };
      }
   }
}