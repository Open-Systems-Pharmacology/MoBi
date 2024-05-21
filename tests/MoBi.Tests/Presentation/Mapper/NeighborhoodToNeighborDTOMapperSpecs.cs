using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Mapper
{
   internal class concern_for_NeighborhoodToNeighborDTOMapper : ContextSpecification<NeighborhoodToNeighborDTOMapper>
   {
      protected IObjectPathFactory _objectPathFactory;

      protected override void Context()
      {
         _objectPathFactory = A.Fake<IObjectPathFactory>();
         sut = new NeighborhoodToNeighborDTOMapper(_objectPathFactory);
      }
   }

   internal class When_mapping_a_neighborhood : concern_for_NeighborhoodToNeighborDTOMapper
   {
      private Neighborhood _neighborhood;
      private ObjectPath _firstNeighborPath;
      private ObjectPath _secondNeighborPath;
      private IReadOnlyList<NeighborDTO> _result;

      protected override void Context()
      {
         base.Context();
         _neighborhood = new Neighborhood().WithName("Neighborhood");
         _neighborhood.FirstNeighbor = new Container();
         _neighborhood.SecondNeighbor = new Container();
         _firstNeighborPath = new ObjectPath("first");
         _secondNeighborPath = new ObjectPath("second");

         A.CallTo(() => _objectPathFactory.CreateAbsoluteObjectPath(_neighborhood.FirstNeighbor)).Returns(_firstNeighborPath);
         A.CallTo(() => _objectPathFactory.CreateAbsoluteObjectPath(_neighborhood.SecondNeighbor)).Returns(_secondNeighborPath);
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_neighborhood).ToList();
      }

      [Observation]
      public void should_create_a_dto_for_the_neighbors()
      {
         _result[0].Path.ShouldBeEqualTo(_firstNeighborPath);
         _result[1].Path.ShouldBeEqualTo(_secondNeighborPath);
      }

      [Observation]
      public void the_ids_should_contain_the_parent_id()
      {
         _result[0].Id.Contains(_neighborhood.Id).ShouldBeTrue();
         _result[1].Id.Contains(_neighborhood.Id).ShouldBeTrue();
      }
   }

   internal class When_mapping_an_incomplete_neighborhood : concern_for_NeighborhoodToNeighborDTOMapper
   {
      private NeighborhoodBuilder _neighborhood;
      private IReadOnlyList<NeighborDTO> _result;

      protected override void Context()
      {
         base.Context();
         _neighborhood = new NeighborhoodBuilder().WithName("Neighborhood");
         _neighborhood.FirstNeighborPath = new ObjectPath("first");
         _neighborhood.SecondNeighborPath = null;
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_neighborhood).ToList();
      }

      [Observation]
      public void should_create_a_dto_for_the_non_null_neighbors()
      {
         _result.Count.ShouldBeEqualTo(1);
      }
   }

   internal class When_mapping_a_neighborhood_builder : concern_for_NeighborhoodToNeighborDTOMapper
   {
      private NeighborhoodBuilder _neighborhood;
      private IReadOnlyList<NeighborDTO> _result;

      protected override void Context()
      {
         base.Context();
         _neighborhood = new NeighborhoodBuilder().WithName("Neighborhood");
         _neighborhood.FirstNeighborPath = new ObjectPath("first");
         _neighborhood.SecondNeighborPath = new ObjectPath("second");
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_neighborhood).ToList();
      }

      [Observation]
      public void should_create_a_dto_for_the_neighbors()
      {
         _result[0].Path.ShouldBeEqualTo(_neighborhood.FirstNeighborPath);
         _result[1].Path.ShouldBeEqualTo(_neighborhood.SecondNeighborPath);
      }

      [Observation]
      public void the_ids_should_contain_the_parent_id()
      {
         _result[0].Id.Contains(_neighborhood.Id).ShouldBeTrue();
         _result[1].Id.Contains(_neighborhood.Id).ShouldBeTrue();
      }
   }
}