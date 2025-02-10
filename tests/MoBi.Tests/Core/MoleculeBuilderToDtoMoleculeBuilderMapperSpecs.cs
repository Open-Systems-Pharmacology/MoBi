using DevExpress.Utils.Extensions;
using FakeItEasy;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core
{
   public abstract class concern_for_MoleculeBuilderToDtoMoleculeBuilderMapper : ContextSpecification<IMoleculeBuilderToMoleculeBuilderDTOMapper>
   {
      protected ITransporterMoleculeContainerToTransporterMoleculeContainerDTOMapper _activeTransportToDTOTransporterMoleculeMapper;
      protected IFormulaToFormulaBuilderDTOMapper _formulaToDtoFormulaBuilderMapper;
      protected IUsedCalcualtionMethodToUsedCalcualtionMethodDTOMapper _usedCalculationMethodToDTOCalculationMethodMapper;
      protected IInteractionContainerToInteractionContainerDTOMapper _interactionContainerToInteractionContainerDTOMapper;

      protected override void Context()
      {
         _activeTransportToDTOTransporterMoleculeMapper = A.Fake<ITransporterMoleculeContainerToTransporterMoleculeContainerDTOMapper>();
         _formulaToDtoFormulaBuilderMapper = A.Fake<IFormulaToFormulaBuilderDTOMapper>();
         _usedCalculationMethodToDTOCalculationMethodMapper =
            A.Fake<IUsedCalcualtionMethodToUsedCalcualtionMethodDTOMapper>();
         _interactionContainerToInteractionContainerDTOMapper = A.Fake<IInteractionContainerToInteractionContainerDTOMapper>();
         sut = new MoleculeBuilderToMoleculeBuilderDTOMapper(_formulaToDtoFormulaBuilderMapper, _activeTransportToDTOTransporterMoleculeMapper, _usedCalculationMethodToDTOCalculationMethodMapper, _interactionContainerToInteractionContainerDTOMapper);
      }
   }

   public class When_mapping_from_a_MoleculeBuilder : concern_for_MoleculeBuilderToDtoMoleculeBuilderMapper
   {
      private MoleculeBuilder _moleculeBuilder;
      private MoleculeBuilderDTO _result;
      private IFormula _startFormula;
      private TransportBuilder _activeTransport;
      private IParameter _parameter;
      private TransporterMoleculeContainer _transporterMoleculeContainer;
      private UsedCalculationMethod _calculationMethod;
      private InteractionContainer _interactionContainer;

      protected override void Context()
      {
         base.Context();
         _moleculeBuilder = new MoleculeBuilder();
         _startFormula = A.Fake<IFormula>();
         _moleculeBuilder.DefaultStartFormula = _startFormula;
         _moleculeBuilder.Description = "Description";
         _moleculeBuilder.Icon = ApplicationIcons.Molecule.IconName;
         _moleculeBuilder.Id = "ID";
         _moleculeBuilder.IsFloating = true;
         _moleculeBuilder.QuantityType = QuantityType.Drug;
         _activeTransport = A.Fake<TransportBuilder>();
         _transporterMoleculeContainer = new TransporterMoleculeContainer().WithName("transport");
         
         _moleculeBuilder.AddTransporterMoleculeContainer(_transporterMoleculeContainer);
         
         _transporterMoleculeContainer.AddActiveTransportRealization(_activeTransport);
         
         _parameter = A.Fake<IParameter>().WithName("parameter");
         _moleculeBuilder.AddParameter(_parameter);
         
         _calculationMethod = new UsedCalculationMethod("category", "method");
         _moleculeBuilder.AddUsedCalculationMethod(_calculationMethod);
         
         _interactionContainer = A.Fake<InteractionContainer>();
         _moleculeBuilder.AddInteractionContainer(_interactionContainer);
         
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
         A.CallTo(() => _interactionContainerToInteractionContainerDTOMapper.MapFrom(_interactionContainer)).MustHaveHappened();
      }

      [Observation]
      public void should_set_the_right_properties()
      {
         _result.Description.ShouldBeEqualTo(_moleculeBuilder.Description);
         _result.Icon.ShouldBeEqualTo(ApplicationIcons.Molecule);
         _result.Stationary.ShouldBeEqualTo(!_moleculeBuilder.IsFloating);
         _result.Name.ShouldBeEqualTo(_moleculeBuilder.Name);
      }
   }
}