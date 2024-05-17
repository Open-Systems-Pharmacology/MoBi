using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.UnitSystem;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Presentation.Tasks
{
   public abstract class concern_for_InteractionTasksForDistributedParameter : ContextSpecification<InteractionTasksForDistributedParameter>
   {
      private IMoBiDimensionFactory _dimensionFactory;
      protected IInteractionTaskContext _context;
      private IEditTaskFor<IDistributedParameter> _editTask;
      private IParameterFactory _parameterFactory;
      private IDistributionFormulaFactory _distributionFactory;

      protected override void Context()
      {
         _context = A.Fake<IInteractionTaskContext>();
         _editTask = A.Fake<IEditTaskFor<IDistributedParameter>>();
         _dimensionFactory = A.Fake<IMoBiDimensionFactory>();
         _parameterFactory = A.Fake<IParameterFactory>();
         _distributionFactory = A.Fake<IDistributionFormulaFactory>();
         sut = new InteractionTasksForDistributedParameter(_context, _editTask, _dimensionFactory, _parameterFactory, _distributionFactory);
      }
   }

   public class When_adding_a_new_distributed_parameter : concern_for_InteractionTasksForDistributedParameter
   {
      private IContainer _parentContainer;
      private IBuildingBlock _buildingBlock;
      private IDistributedParameter _distributedParameter;

      protected override void Context()
      {
         base.Context();
         _buildingBlock = A.Fake<IBuildingBlock>();
         _parentContainer = A.Fake<IContainer>();
         var modalPresenter = A.Fake<IModalPresenter>();
         _distributedParameter = A.Fake<IDistributedParameter>();
         A.CallTo(_context.ApplicationController).WithReturnType<IModalPresenter>().Returns(modalPresenter);
         A.CallTo(() => modalPresenter.Show(null)).Returns(true);
         A.CallTo(() => _context.Context.Create<IDistributedParameter>()).Returns(_distributedParameter);
      }

      protected override void Because()
      {
         sut.AddNew(_parentContainer, _buildingBlock);
      }

      [Observation]
      public void should_create_a_parameter_with_a_default_normal_distribution()
      {
         _distributedParameter.Formula.ShouldBeAnInstanceOf<NormalDistributionFormula>();
      }
   }
}