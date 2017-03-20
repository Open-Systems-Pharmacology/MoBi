using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Helper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Assets;

namespace MoBi.Core
{
   public abstract class concern_for_TypeDictionary : ContextSpecification<IObjectTypeResolver>
   {
      protected override void Context()
      {
         sut = new ObjectTypeResolver();
      }
   }

   public class When_retrieving_the_type_for_known_types : concern_for_TypeDictionary
   {
      [Observation]
      public void should_return_the_expected_types()
      {
         sut.TypeFor(new[] {new DataRepository()}).ShouldBeEqualTo("List of Observed Data");
         sut.TypeFor(new[] {new ReactionBuildingBlock()}).ShouldBeEqualTo("List of Reaction Building Blocks");
         sut.TypeFor(new MoleculeBuildingBlock()).ShouldBeEqualTo(ObjectTypes.MoleculeBuildingBlock);
      }
   }

   public class When_retrieving_the_type_for_a_null_object : concern_for_TypeDictionary
   {
      [Observation]
      public void should_return_the_expected_types()
      {
         sut.TypeFor<MoleculeBuildingBlock>(null).ShouldBeEqualTo(ObjectTypes.MoleculeBuildingBlock);
      }
   }
}