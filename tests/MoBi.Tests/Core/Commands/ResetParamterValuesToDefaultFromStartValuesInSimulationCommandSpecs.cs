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
   public abstract class concern_for_ResetParamterValuesToDefaultFromStartValuesInSimulationCommand : ContextSpecification<ResetParameterValuesToDefaultFromStartValuesInSimulationCommand>
   {
      private IMoBiSimulation _simulation;
      protected ParameterStartValuesBuildingBlock _parameterStartValues;
      protected IContainer _liver;
      protected IMoBiContext _context;
      protected IMoBiFormulaTask _formulaTask;
      protected IEntityPathResolver _entityPathResolver;
      protected ICloneManagerForModel _cloneManagerForModel;

      protected override void Context()
      {
         _context= A.Fake<IMoBiContext>();
         _liver = new Container().WithName("LIVER");
         _simulation= A.Fake<IMoBiSimulation>();
         _simulation.Model.Root = new Container().WithContainerType(ContainerType.Simulation);
         _simulation.Model.Root.Add(_liver);
         _parameterStartValues= new ParameterStartValuesBuildingBlock();
         _simulation.BuildConfiguration.ParameterStartValues = _parameterStartValues;
         sut = new ResetParameterValuesToDefaultFromStartValuesInSimulationCommand(_simulation);

         _formulaTask= A.Fake<IMoBiFormulaTask>();
         _entityPathResolver=new EntityPathResolver(new ObjectPathFactory(new AliasCreator()));
         _cloneManagerForModel = A.Fake<ICloneManagerForModel>();

         A.CallTo(_formulaTask).WithReturnType<ConstantFormula>().Returns(new ConstantFormula());
         A.CallTo(() => _context.Resolve<IMoBiFormulaTask>()).Returns(_formulaTask);
         A.CallTo(() => _context.Resolve<IEntityPathResolver>()).Returns(_entityPathResolver);
         A.CallTo(() => _context.Resolve<ICloneManagerForModel>()).Returns(_cloneManagerForModel);
      }
   }

   public class When_reseting_the_parameter_values_to_default_from_start_values: concern_for_ResetParamterValuesToDefaultFromStartValuesInSimulationCommand
   {
      private ParameterStartValue _psv1;
      private ParameterStartValue _psv2;
      private IParameter _p1Parameter;
      private IParameter _p2Parameter;
      private IParameter _unchangedParameter;

      protected override void Context()
      {
         base.Context();


         _psv1 = new ParameterStartValue {ContainerPath = new ObjectPath("LIVER"), Name="P1"};
         _psv1.Formula = new ExplicitFormula("3+4"); ;

         A.CallTo(() => _cloneManagerForModel.Clone(_psv1.Formula)).Returns(_psv1.Formula);
         _psv2 = new ParameterStartValue {ContainerPath = new ObjectPath("LIVER"), Name="P2"};
         _psv2.Value = 17;

         _unchangedParameter = new Parameter().WithName("Unchanged").WithFormula(new ConstantFormula(2));
         _p1Parameter = new Parameter().WithName("P1").WithFormula(new ConstantFormula(1));
         _p1Parameter.Value = 2;

         _p2Parameter = new Parameter().WithName("P2").WithFormula(new ExplicitFormula("1+2"));
         _p2Parameter.Value = 2;

         _liver.Add(_p1Parameter);
         _liver.Add(_p2Parameter);
         _liver.Add(_unchangedParameter);

         _parameterStartValues.Add(_psv1);
         _parameterStartValues.Add(_psv2);
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_unfixed_all_fixed_parameters()
      {
         _p1Parameter.IsFixedValue.ShouldBeFalse();
         _p2Parameter.IsFixedValue.ShouldBeFalse();
      }

      [Observation]
      public void should_reset_the_formula_of_all_fixed_parameters_according_to_the_value_defined_in_the_PSV()
      {
         _p1Parameter.Value.ShouldBeEqualTo(7);
         _p1Parameter.Formula.IsExplicit().ShouldBeTrue();

         _p2Parameter.Value.ShouldBeEqualTo(17);
         _p2Parameter.Formula.IsConstant().ShouldBeTrue();
      }
   }
}	