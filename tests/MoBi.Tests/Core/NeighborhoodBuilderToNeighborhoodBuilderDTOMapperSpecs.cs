using System.Collections.Generic;
using FakeItEasy;
using MoBi.Core.Repositories;
using MoBi.Helpers;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Extensions;

namespace MoBi.Core
{
   public abstract class concern_for_NeighborhoodBuilderToNeighborhoodBuilderDTOMapper : ContextSpecification<INeighborhoodBuilderToNeighborhoodBuilderDTOMapper>
   {
      private IObjectPathFactory _objectPathFactory;
      private IIconRepository _iconRepository;

      protected override void Context()
      {
         _objectPathFactory = new ObjectPathFactoryForSpecs();
         _iconRepository = A.Fake<IIconRepository>();
         sut = new NeighborhoodBuilderToNeighborhoodBuilderDTOMapper(_objectPathFactory, _iconRepository);
      }
   }

   public class When_mapping_a_neighborhood_to_a_neighborhood_DTO : concern_for_NeighborhoodBuilderToNeighborhoodBuilderDTOMapper
   {
      private NeighborhoodBuilder _neighborhood;
      private NeighborhoodBuilderDTO _dto;

      protected override void Context()
      {
         base.Context();
         _neighborhood = new NeighborhoodBuilder
         {
            FirstNeighborPath = new ObjectPath("A", "B", "C"),
            Name = "TEST"
         };
      }

      protected override void Because()
      {
         _dto = sut.MapFrom(_neighborhood, new List<NeighborhoodBuilder>());
      }

      [Observation]
      public void should_return_a_DTO_object_with_the_expected_properties()
      {
         _dto.Name.ShouldBeEqualTo(_neighborhood.Name);
         _dto.FirstNeighborDTO.Path.ShouldBeEqualTo(_neighborhood.FirstNeighborPath.ToPathString());
         _dto.SecondNeighborDTO.Path.ShouldBeNullOrEmpty();
      }
   }
}