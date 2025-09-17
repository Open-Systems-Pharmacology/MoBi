using FakeItEasy;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.Mapper
{
   internal abstract class concern_for_QuantityToQuantityDTOMapper : ContextSpecification<QuantityToQuantityDTOMapper>
   {
      private IFormulaToFormulaBuilderDTOMapper _formulaToDTOFormulaMapper;
      protected IQuantity _quantity;

      protected override void Context()
      {
         _formulaToDTOFormulaMapper = A.Fake<IFormulaToFormulaBuilderDTOMapper>();
         sut = new QuantityToQuantityDTOMapper(_formulaToDTOFormulaMapper);
         _quantity = new MoleculeAmount().WithName("CONT");
      }

      
   }

   internal class When_mapping_quantity_to_dto_with_source : concern_for_QuantityToQuantityDTOMapper
   {
      private TrackableSimulation _trackableSimulation;
      private QuantityDTO _result;

      protected override void Context()
      {
         base.Context();
         _trackableSimulation = new TrackableSimulation(null, new SimulationEntitySourceReferenceCache());
         _trackableSimulation.ReferenceCache.Add(_quantity, new SimulationEntitySourceReference(null, null, null, _quantity));
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_quantity, _trackableSimulation);
      }

      [Observation]
      public void the_source_entity_reference_should_be_set()
      {
         _result.SourceReference.ShouldNotBeNull();
      }
   }
}