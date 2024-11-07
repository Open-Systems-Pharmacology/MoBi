using System.Collections.Generic;
using MoBi.Presentation.DTO;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation
{
   public abstract class concern_for_NeighborhoodBuilderDTO : ContextSpecification<NeighborhoodBuilderDTO>
   {
      protected NeighborhoodBuilderDTO _neighborhoodBuilderDTO;
      private NeighborhoodBuilder _neighborhoodBuilder;

      protected override void Context()
      {
         _neighborhoodBuilder = new NeighborhoodBuilder();
         _neighborhoodBuilderDTO = new NeighborhoodBuilderDTO(_neighborhoodBuilder, GetExistingNeighborhoods());
      }

      protected abstract List<NeighborhoodBuilder> GetExistingNeighborhoods();
   }

   public class When_validating_a_neighborhood_with_identical_paths_and_with_a_name : concern_for_NeighborhoodBuilderDTO
   {
      protected override void Context()
      {
         base.Context();
         _neighborhoodBuilderDTO.Name = "NeighborhoodName";
         _neighborhoodBuilderDTO.FirstNeighborDTO.Path = "a path";
         _neighborhoodBuilderDTO.SecondNeighborDTO.Path = "a path";
      }

      protected override List<NeighborhoodBuilder> GetExistingNeighborhoods()
      {
         return new List<NeighborhoodBuilder>();
      }

      [Observation]
      public void should_have_one_invalid_message_for_each_path()
      {
         _neighborhoodBuilderDTO.Validate().IsEmpty.ShouldBeTrue();
         _neighborhoodBuilderDTO.FirstNeighborDTO.Validate().Count.ShouldBeEqualTo(1);
         _neighborhoodBuilderDTO.SecondNeighborDTO.Validate().Count.ShouldBeEqualTo(1);
      }
   }

   public class When_validating_a_neighborhood_with_empty_paths_and_with_a_name : concern_for_NeighborhoodBuilderDTO
   {
      protected override void Context()
      {
         base.Context();
         _neighborhoodBuilderDTO.Name = "NeighborhoodName";
         _neighborhoodBuilderDTO.FirstNeighborDTO.Path = "a path";
      }

      protected override List<NeighborhoodBuilder> GetExistingNeighborhoods()
      {
         return new List<NeighborhoodBuilder>();
      }

      [Observation]
      public void should_have_one_invalid_message_for_empty_path()
      {
         _neighborhoodBuilderDTO.Validate().IsEmpty.ShouldBeTrue();
         _neighborhoodBuilderDTO.FirstNeighborDTO.Validate().IsEmpty.ShouldBeTrue();
         _neighborhoodBuilderDTO.SecondNeighborDTO.Validate().Count.ShouldBeEqualTo(1);
      }
   }

   public class When_validating_a_neighborhood_with_colliding_neighborhoods_with_a_name : concern_for_NeighborhoodBuilderDTO
   {
      protected override void Context()
      {
         base.Context();
         _neighborhoodBuilderDTO.FirstNeighborDTO.Path = "FirstNeighborPath";
         _neighborhoodBuilderDTO.SecondNeighborDTO.Path = "SecondNeighborPath";
         _neighborhoodBuilderDTO.Name = "NeighborhoodName";
      }

      protected override List<NeighborhoodBuilder> GetExistingNeighborhoods()
      {
         return new List<NeighborhoodBuilder>
         {
            new NeighborhoodBuilder { FirstNeighborPath = new ObjectPath("FirstNeighborPath"), SecondNeighborPath =  new ObjectPath("SecondNeighborPath") },
         };
      }

      [Observation]
      public void should_have_invalid_paths_for_both_neighbors()
      {
         _neighborhoodBuilderDTO.Validate().IsEmpty.ShouldBeTrue();
         _neighborhoodBuilderDTO.FirstNeighborDTO.Validate().Count.ShouldBeEqualTo(1);
         _neighborhoodBuilderDTO.SecondNeighborDTO.Validate().Count.ShouldBeEqualTo(1);
      }
   }

   public class When_validating_a_neighborhood_without_colliding_neighborhoods_and_without_a_name : concern_for_NeighborhoodBuilderDTO
   {
      protected override void Context()
      {
         base.Context();
         _neighborhoodBuilderDTO.FirstNeighborDTO.Path = "FirstNeighborPath1";
         _neighborhoodBuilderDTO.SecondNeighborDTO.Path = "SecondNeighborPath1";
      }

      protected override List<NeighborhoodBuilder> GetExistingNeighborhoods()
      {
         return new List<NeighborhoodBuilder>
         {
            new NeighborhoodBuilder { FirstNeighborPath = new ObjectPath("FirstNeighborPath2"), SecondNeighborPath =  new ObjectPath("SecondNeighborPath2") },
         };
      }

      [Observation]
      public void should_have_one_invalid_message()
      {
         _neighborhoodBuilderDTO.Validate().Count.ShouldBeEqualTo(1);
         _neighborhoodBuilderDTO.FirstNeighborDTO.Validate().IsEmpty.ShouldBeTrue();
         _neighborhoodBuilderDTO.SecondNeighborDTO.Validate().IsEmpty.ShouldBeTrue();
      }
   }

   public class When_validating_a_neighborhood_without_colliding_neighborhoods_and_a_name : concern_for_NeighborhoodBuilderDTO
   {
      protected override void Context()
      {
         base.Context();
         _neighborhoodBuilderDTO.FirstNeighborDTO.Path = "FirstNeighborPath2";
         _neighborhoodBuilderDTO.SecondNeighborDTO.Path = "SecondNeighborPath2";
         _neighborhoodBuilderDTO.Name = "NeighborhoodName";
      }

      protected override List<NeighborhoodBuilder> GetExistingNeighborhoods()
      {
         return new List<NeighborhoodBuilder>
         {
            new NeighborhoodBuilder { FirstNeighborPath = new ObjectPath("FirstNeighborPath1"), SecondNeighborPath =  new ObjectPath("SecondNeighborPath1") },
         };
      }

      [Observation]
      public void should_be_valid()
      {
         _neighborhoodBuilderDTO.Validate().IsEmpty.ShouldBeTrue();
         _neighborhoodBuilderDTO.FirstNeighborDTO.Validate().IsEmpty.ShouldBeTrue();
         _neighborhoodBuilderDTO.SecondNeighborDTO.Validate().IsEmpty.ShouldBeTrue();
      }
   }
}
