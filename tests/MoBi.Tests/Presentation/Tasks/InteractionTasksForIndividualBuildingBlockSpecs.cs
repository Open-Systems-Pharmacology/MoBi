using System.Collections.Generic;
using FakeItEasy;
using FakeItEasy.Core;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Services;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks
{
   internal class concern_for_InteractionTasksForIndividualBuildingBlock : ContextSpecification<InteractionTasksForIndividualBuildingBlock>
   {
      protected IInteractionTaskContext _interactionTaskContext;
      protected IEditTasksForIndividualBuildingBlock _editTasksForIndividualBuildingBlock;
      protected IMoBiFormulaTask _moBiFormulaTask;
      protected IParameterFactory _parameterFactory;

      protected override void Context()
      {
         _interactionTaskContext = A.Fake<IInteractionTaskContext>();
         _editTasksForIndividualBuildingBlock = A.Fake<IEditTasksForIndividualBuildingBlock>();
         _moBiFormulaTask = A.Fake<IMoBiFormulaTask>();
         _parameterFactory = A.Fake<IParameterFactory>();
         sut = new InteractionTasksForIndividualBuildingBlock(_interactionTaskContext, _editTasksForIndividualBuildingBlock, _moBiFormulaTask, _parameterFactory);
         A.CallTo(() => _parameterFactory.CreateDistributedParameter(A<string>._, A<DistributionType>._, A<double?>._, A<IDimension>._, A<string>._, A<Unit>._)).ReturnsLazily(newDistributedParameterFrom);
         A.CallTo(() => _parameterFactory.CreateParameter(A<string>._,A<double?>._, A<IDimension>._, A<string>._, A<IFormula>._, A<Unit>._)).ReturnsLazily(newParameterFrom);
      }

      private IParameter newParameterFrom(IFakeObjectCall fakeObjectCall)
      {
         return new Parameter().WithName(fakeObjectCall.GetArgument<string>(0));
      }

      private IParameter newDistributedParameterFrom(IFakeObjectCall fakeObjectCall)
      {
         // We can just use 3 here as we are not creating real distributed formulas that will be calculated
         var distributedParameter = new DistributedParameter().WithName(fakeObjectCall.GetArgument<string>(0)).WithValue(3);
         return distributedParameter;
      }
   }

   internal class When_converting_a_distributed_individual_parameter_to_constant_formula : concern_for_InteractionTasksForIndividualBuildingBlock
   {
      private IndividualParameter _individualParameter;
      private IndividualBuildingBlock _individualBuildingBlock;
      private List<IndividualParameter> _subParameters;

      protected override void Context()
      {
         base.Context();
         _individualParameter = new IndividualParameter
         {
            DistributionType = DistributionType.Normal
         };

         _individualBuildingBlock = new IndividualBuildingBlock();

         _subParameters = new List<IndividualParameter>
         {
            new IndividualParameter{Value = 3.0}.WithName(Constants.Distribution.MEAN),
            new IndividualParameter().WithName(Constants.Distribution.PERCENTILE)
         };

         _individualBuildingBlock.AddRange(_subParameters);
      }

      protected override void Because()
      {
         sut.ConvertDistributedParameterToConstantParameter(_individualParameter, _individualBuildingBlock, _subParameters);
      }

      [Observation]
      public void sub_parameters_should_be_removed_from_the_building_block()
      {
         _subParameters.Each(x => _individualBuildingBlock.ShouldNotContain(x));
      }

      [Observation]
      public void should_set_the_value_of_the_parameter_to_the_mean()
      {
         _individualParameter.Value.ShouldBeEqualTo(3.0);
      }
   }
}