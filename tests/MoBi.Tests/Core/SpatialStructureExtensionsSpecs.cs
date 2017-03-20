using FakeItEasy;

using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;


namespace MoBi.Core
{
   public class When_asking_if_a_container_is_in_a_SpatialStructure : StaticContextSpecification
   {
      private ISpatialStructure _testStructure;
      private IContainer _testContainer;
      private bool _result;

      protected override void Context()
      {
         base.Context();
         IContainer topContainer = new Container();
         _testContainer = new Container();
         topContainer.Add(_testContainer);
         _testStructure = new SpatialStructure().WithTopContainer(topContainer);
         _testStructure.NeighborhoodsContainer = new Container();
         _testStructure.GlobalMoleculeDependentProperties = new Container();
      }

      protected override void Because()
      {
         _result = SpatialStructureExtensions.IsInSpatialStructure(_testStructure, _testContainer);
      }

      [Observation]
      public void should_return_true()
      {
         _result.ShouldBeTrue();
      }
   }

   class When_Asking_If_A_Top_Container_Is_In_A_SpatialStructure : StaticContextSpecification
   {
      private ISpatialStructure _testStructure;
      private IContainer _testContainer;
      private bool _result;

      protected override void Context()
      {
         base.Context();
         IContainer topContainer = new Container();
         _testContainer = topContainer;
         _testStructure = new SpatialStructure().WithTopContainer(topContainer);
         _testStructure.NeighborhoodsContainer = new Container();
         _testStructure.GlobalMoleculeDependentProperties = new Container();
      }

      protected override void Because()
      {
         _result = SpatialStructureExtensions.IsInSpatialStructure(_testStructure, _testContainer);
      }

      [Observation]
      public void should_return_true()
      {
         _result.ShouldBeTrue();
      }
   }
}	