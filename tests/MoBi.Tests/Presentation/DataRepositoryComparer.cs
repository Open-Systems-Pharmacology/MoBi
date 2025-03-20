using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;

namespace MoBi.Presentation
{
   public class DataRepositoryComparer : ArgumentEqualityComparer<DataRepository>
   {
      protected override bool AreEqual(DataRepository expectedValue, DataRepository argumentValue)
      {
         return ReferenceEquals(expectedValue, argumentValue);
      }
   }

   public class ContainerComparer : ArgumentEqualityComparer<Container>
   {
      protected override bool AreEqual(Container expectedValue, Container argumentValue)
      {
         return ReferenceEquals(expectedValue, argumentValue);
      }
   }

   public class MoBiReactionBuildingBlockComparer : ArgumentEqualityComparer<MoBiReactionBuildingBlock>
   {
      protected override bool AreEqual(MoBiReactionBuildingBlock expectedValue, MoBiReactionBuildingBlock argumentValue)
      {
         return ReferenceEquals(expectedValue, argumentValue);
      }
   }

   public class ModuleComparer : ArgumentEqualityComparer<Module>
   {
      protected override bool AreEqual(Module expectedValue, Module argumentValue)
      {
         return ReferenceEquals(expectedValue, argumentValue);
      }
   }
}