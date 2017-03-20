using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Serialization.Converter.v3_1;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Application.ProjectUpdate
{
   public abstract class concern_for_DimensionRetrieverSpecs : ContextSpecification<IParameterStartValueDimensionRetriever>
   {
      protected IMoBiContext _context;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         sut = new ParameterStartValueDimensionRetriever();
      }
   }

   internal class When_retieving_a_dimension_for_a_paramter_start_value : concern_for_DimensionRetrieverSpecs
   {
      private IParameterStartValuesBuildingBlock _parentBuidlidingBlock;
      private IParameterStartValue _startValue;
      private IMoBiProject _project;
      private IMoBiBuildConfiguration _buildConfiguration;
      private IObjectPath _parameterPath;
      private IContainer _root;
      private IDimension _reultDimension;
      private IParameter _para;
      private IMoBiSimulation _simulation;
      private IModel _model;

      protected override void Context()
      {
         base.Context();
         _project = A.Fake<IMoBiProject>();
         _startValue = A.Fake<IParameterStartValue>();
         _parameterPath = A.Fake<IObjectPath>();
         _parentBuidlidingBlock = A.Fake<IParameterStartValuesBuildingBlock>();
         _parentBuidlidingBlock.Id = "Parent";
         _para = A.Fake<IParameter>().WithName("Para").WithDimension(A.Fake<IDimension>());
         A.CallTo(() => _startValue.Path).Returns(_parameterPath);
         _simulation = A.Fake<IMoBiSimulation>();
         A.CallTo(() => _project.Simulations).Returns(new[] {_simulation,});
         _buildConfiguration = A.Fake<IMoBiBuildConfiguration>();
         _buildConfiguration.ParameterStartValuesInfo = new ParameterStartValuesBuildingBlockInfo();
         _buildConfiguration.ParameterStartValuesInfo.TemplateBuildingBlock = _parentBuidlidingBlock;
         _model = A.Fake<IModel>();
         _root = A.Fake<IContainer>();
         A.CallTo(() => _simulation.MoBiBuildConfiguration).Returns(_buildConfiguration);
         A.CallTo(() => _simulation.Model).Returns(_model);
         A.CallTo(() => _parameterPath.Resolve<IParameter>(_root)).Returns(_para);
         _model.Root = _root;
         A.CallTo(() => _context.CurrentProject).Returns(_project);
      }

      protected override void Because()
      {
         _reultDimension = sut.GetDimensionFor(_startValue, _parentBuidlidingBlock, _project);
      }

      [Observation]
      public void should_retrieve_parameter_in_simulation()
      {
         A.CallTo(() => _parameterPath.Resolve<IParameter>(_root)).MustHaveHappened();
      }

      [Observation]
      public void should_return_the_parameters_dimension()
      {
         _reultDimension.ShouldBeEqualTo(_para.Dimension);
      }
   }
}