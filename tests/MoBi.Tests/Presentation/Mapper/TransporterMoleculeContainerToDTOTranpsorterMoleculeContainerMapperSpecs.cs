using FakeItEasy;

using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;


namespace MoBi.Presentation.Mapper
{
   public abstract class concern_for_TransporterMoleculeContainerToTransporterMoleculeContainerDTOMapper : ContextSpecification<ITransporterMoleculeContainerToTransporterMoleculeContainerDTOMapper>
   {
      protected IParameterToParameterDTOMapper _parameterToParameterDTOMapper;
      protected ITransportBuilderToTransportBuilderDTOMapper _transportBuilderToDTOTransporterBuilderMapper;

      protected override void Context()
      {
         _parameterToParameterDTOMapper = A.Fake<IParameterToParameterDTOMapper>();
         _transportBuilderToDTOTransporterBuilderMapper = A.Fake<ITransportBuilderToTransportBuilderDTOMapper>();
         sut = new TransporterMoleculeContainerToTransporterMoleculeContainerDTOMapper(_transportBuilderToDTOTransporterBuilderMapper,_parameterToParameterDTOMapper);
      }
   }

   class MappingATransporterMoleculeContainerToTransporterMoleculeContainerADTO : concern_for_TransporterMoleculeContainerToTransporterMoleculeContainerDTOMapper
   {
      private TransporterMoleculeContainer _transporterMoleculeContainer;
      private TransporterMoleculeContainerDTO _result;
      private ITransportBuilder _transporter;
      private IParameter _parameter;

      protected override void Context()
      {
         base.Context();
         _transporterMoleculeContainer = A.Fake<TransporterMoleculeContainer>();
         _transporterMoleculeContainer.Name = "Cyp";
         _parameter = A.Fake<IParameter>();
         _transporter = A.Fake<ITransportBuilder>();
         A.CallTo(() => _transporterMoleculeContainer.ActiveTransportRealizations).Returns(new []{_transporter});
         A.CallTo(() => _transporterMoleculeContainer.Parameters).Returns(new[] { _parameter });
         
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_transporterMoleculeContainer);
      }

      [Observation]
      public void should_do_something_reasonable()
      {
        _result.ShouldNotBeNull();
        _result.Name.ShouldBeEqualTo(_transporterMoleculeContainer.Name);
      }

      [Observation]
      public void should_map_children()
      {
         A.CallTo(() => _transportBuilderToDTOTransporterBuilderMapper.MapFrom(_transporter)).MustHaveHappened();
         A.CallTo(() => _parameterToParameterDTOMapper.MapFrom(_parameter)).MustHaveHappened();
      }
   }
}	