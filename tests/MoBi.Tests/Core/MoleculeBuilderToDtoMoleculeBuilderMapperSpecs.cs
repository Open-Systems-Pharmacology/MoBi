using FakeItEasy;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core
{
   public abstract class concern_for_MoleculeBuilderToDtoMoleculeBuilderMapper : ContextSpecification<IMoleculeBuilderToMoleculeBuilderDTOMapper>
   {
      protected ITransporterMoleculeContainerToTranpsorterMoleculeContainerDTOMapper _activeTransportToDTOTransporterMoleculeMapper;
      protected IFormulaToFormulaBuilderDTOMapper _formulaToDtoFormulaBuilderMapper;
      protected IUsedCalcualtionMethodToUsedCalcualtionMethodDTOMapper _usedCalculationMethodToDTOCalculationMethodMapper;
      protected IInteractionContainerToInteractionConatainerDTOMapper _interactionContainerToInteractionConatainerDTOMpaper;

      protected override void Context()
      {
         _activeTransportToDTOTransporterMoleculeMapper = A.Fake<ITransporterMoleculeContainerToTranpsorterMoleculeContainerDTOMapper>();
         _formulaToDtoFormulaBuilderMapper = A.Fake<IFormulaToFormulaBuilderDTOMapper>();
         _usedCalculationMethodToDTOCalculationMethodMapper =
            A.Fake<IUsedCalcualtionMethodToUsedCalcualtionMethodDTOMapper>();
         _interactionContainerToInteractionConatainerDTOMpaper = A.Fake<IInteractionContainerToInteractionConatainerDTOMapper>();
         sut = new MoleculeBuilderToMoleculeBuilderDTOMapper(_formulaToDtoFormulaBuilderMapper, _activeTransportToDTOTransporterMoleculeMapper, _usedCalculationMethodToDTOCalculationMethodMapper, _interactionContainerToInteractionConatainerDTOMpaper);
      }
   }

   public class When_mapping_from_a_MoleculeBuilder : concern_for_MoleculeBuilderToDtoMoleculeBuilderMapper
   {
      private IMoleculeBuilder _moleculeBuilder;
      private MoleculeBuilderDTO _result;
      private IFormula _startFormula;
      private ITransportBuilder _activeTransport;
      private IParameter _parameter;
      private TransporterMoleculeContainer _transporterMoleculeContainer;
      private UsedCalculationMethod _calculationMethod;
      private InteractionContainer _interactionContainer;


      protected override void Context()
      {
         base.Context();
         _moleculeBuilder = A.Fake<IMoleculeBuilder>();
         _startFormula = A.Fake<IFormula>();
         _moleculeBuilder.DefaultStartFormula = _startFormula;
         _moleculeBuilder.Description = "Description";
         _moleculeBuilder.Icon = "Icon";
         _moleculeBuilder.Id = "ID";
         _moleculeBuilder.IsFloating = true;
         _moleculeBuilder.QuantityType = QuantityType.Drug;
         _activeTransport = A.Fake<ITransportBuilder>();
         _transporterMoleculeContainer = A.Fake<TransporterMoleculeContainer>();
         A.CallTo(() => _moleculeBuilder.TransporterMoleculeContainerCollection).Returns(new[] {_transporterMoleculeContainer});
         A.CallTo(() => _transporterMoleculeContainer.ActiveTransportRealizations).Returns(new[] {_activeTransport});
         _parameter = A.Fake<IParameter>();
         A.CallTo(() => _moleculeBuilder.Parameters).Returns(new[] {_parameter});
         _calculationMethod = A.Fake<UsedCalculationMethod>();
         A.CallTo(() => _moleculeBuilder.UsedCalculationMethods).Returns(new[] {_calculationMethod});
         _interactionContainer = A.Fake<InteractionContainer>();
         A.CallTo(() => _moleculeBuilder.InteractionContainerCollection).Returns(new[] {_interactionContainer});
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_moleculeBuilder);
      }

      [Observation]
      public void should_call_child_mapper()
      {
         A.CallTo(() => _formulaToDtoFormulaBuilderMapper.MapFrom(_startFormula)).MustHaveHappened();
         A.CallTo(() => _activeTransportToDTOTransporterMoleculeMapper.MapFrom(_transporterMoleculeContainer)).MustHaveHappened();
         A.CallTo(() => _usedCalculationMethodToDTOCalculationMethodMapper.MapFrom(_calculationMethod)).MustHaveHappened();
         A.CallTo(() => _interactionContainerToInteractionConatainerDTOMpaper.MapFrom(_interactionContainer)).MustHaveHappened();
      }

      [Observation]
      public void should_set_the_right_properties()
      {
         _result.Description.ShouldBeEqualTo(_moleculeBuilder.Description);
         _result.Icon.ShouldBeEqualTo(_moleculeBuilder.Icon);
         _result.Stationary.ShouldBeEqualTo(!_moleculeBuilder.IsFloating);
         _result.Name.ShouldBeEqualTo(_moleculeBuilder.Name);
      }
   }
}