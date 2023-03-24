using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_ResetMoleculeValuesToDefaultFromStartValuesInSimulationCommand : ContextSpecification<ResetMoleculeValuesToDefaultFromStartValuesInSimulationCommand>
   {
      private IMoBiSimulation _simulation;
      protected MoleculeStartValuesBuildingBlock _moleculeStartValues;
      protected IContainer _liver;
      protected IMoBiContext _context;
      protected IMoBiFormulaTask _formulaTask;
      protected IEntityPathResolver _entityPathResolver;
      protected ICloneManagerForModel _cloneManagerForModel;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         _liver = new Container().WithName("LIVER");
         _simulation = new MoBiSimulation();
         _simulation.Configuration = new SimulationConfiguration();
         _simulation.Model.Root = new Container().WithContainerType(ContainerType.Simulation);
         _simulation.Model.Root.Add(_liver);
         _moleculeStartValues = new MoleculeStartValuesBuildingBlock();

         _simulation.Configuration.Module.AddMoleculeStartValueBlock(_moleculeStartValues);

         sut = new ResetMoleculeValuesToDefaultFromStartValuesInSimulationCommand(_simulation);

         _formulaTask = A.Fake<IMoBiFormulaTask>();
         _entityPathResolver = new EntityPathResolver(new ObjectPathFactory(new AliasCreator()));
         _cloneManagerForModel = A.Fake<ICloneManagerForModel>();

         A.CallTo(_formulaTask).WithReturnType<ConstantFormula>().Returns(new ConstantFormula());
         A.CallTo(() => _context.Resolve<IMoBiFormulaTask>()).Returns(_formulaTask);
         A.CallTo(() => _context.Resolve<IEntityPathResolver>()).Returns(_entityPathResolver);
         A.CallTo(() => _context.Resolve<ICloneManagerForModel>()).Returns(_cloneManagerForModel);
      }
   }

   public class When_reseting_the_molecule_values_to_default_from_start_values : concern_for_ResetMoleculeValuesToDefaultFromStartValuesInSimulationCommand
   {
      private MoleculeStartValue _msv1;
      private MoleculeStartValue _msv2;
      private IMoleculeAmount _m1Liver;
      private IMoleculeAmount _m2Liver;
      private IMoleculeAmount _unchangedMoleculeAmount;
      private IParameter _startValueParameter;

      protected override void Context()
      {
         base.Context();


         _msv1 = new MoleculeStartValue { ContainerPath = new ObjectPath("LIVER"), Name = "M1" };
         _msv1.Formula = new ExplicitFormula("3+4"); ;

         A.CallTo(() => _cloneManagerForModel.Clone(_msv1.Formula)).Returns(_msv1.Formula);
         _msv2 = new MoleculeStartValue { ContainerPath = new ObjectPath("LIVER"), Name = "M2" };
         _msv2.Value = 17;

         _unchangedMoleculeAmount = new MoleculeAmount().WithName("Unchanged").WithFormula(new ConstantFormula(2));
         _m1Liver = new MoleculeAmount().WithName("M1").WithFormula(new ConstantFormula(1));
         _m1Liver.Value = 2;

         _m2Liver = new MoleculeAmount().WithName("M2").WithFormula(new ExplicitFormula(Constants.START_VALUE_ALIAS));
         _startValueParameter = new Parameter().WithName(Constants.Parameters.START_VALUE).WithFormula(new ExplicitFormula("1+2"));
         _m2Liver.Add(_startValueParameter);
         _startValueParameter.Value = 8;

         _liver.Add(_m1Liver);
         _liver.Add(_m2Liver);
         _liver.Add(_unchangedMoleculeAmount);

         _moleculeStartValues.Add(_msv1);
         _moleculeStartValues.Add(_msv2);
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_unfixed_all_fixed_parameters()
      {
         _m1Liver.IsFixedValue.ShouldBeFalse();
         _startValueParameter.IsFixedValue.ShouldBeFalse();
      }

      [Observation]
      public void should_reset_the_formula_of_all_fixed_parameters_according_to_the_value_defined_in_the_PSV()
      {
         _m1Liver.Value.ShouldBeEqualTo(7);
         _m1Liver.Formula.IsExplicit().ShouldBeTrue();

         _startValueParameter.Value.ShouldBeEqualTo(17);
         _startValueParameter.Formula.IsConstant().ShouldBeTrue();
      }
   }
}