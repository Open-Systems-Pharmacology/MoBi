using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Core.Serialization.Exchange;
using MoBi.HelpersForTests;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Tasks
{
   public abstract class concern_for_SpatialStructureContentExporter : ContextSpecification<SpatialStructureContentExporter>
   {
      protected IMoBiSpatialStructureFactory _spatialStructureFactory;
      protected IInteractionTaskContext _interactionTaskContext;
      protected IInteractionTask _interactionTask;
      protected IObjectPathFactory _objectPathFactory;
      private IMoBiApplicationController _applicationController;
      protected ISelectFolderAndIndividualAndExpressionFromProjectPresenter _selectIndividualAndExpressionFromProjectPresenter;
      protected ICloneManagerForBuildingBlock _cloneManager;
      protected IFormulaFactory _formulaFactory;
      protected IPathAndValueEntityToParameterValueMapper _pathAndValueEntityToParameterValueMapper;
      private IObjectBaseFactory _objectBaseFactory;
      private IParameterValueToParameterMapper _individualParameterToParameterMapper;
      protected SpatialStructure _spatialStructure;

      protected override void Context()
      {
         _formulaFactory = A.Fake<IFormulaFactory>();
         _spatialStructureFactory = A.Fake<IMoBiSpatialStructureFactory>();
         _interactionTaskContext = A.Fake<IInteractionTaskContext>();
         _applicationController = A.Fake<IMoBiApplicationController>();
         _selectIndividualAndExpressionFromProjectPresenter = A.Fake<ISelectFolderAndIndividualAndExpressionFromProjectPresenter>();
         _individualParameterToParameterMapper = A.Fake<IParameterValueToParameterMapper>();
         _cloneManager = A.Fake<ICloneManagerForBuildingBlock>();
         _objectBaseFactory = A.Fake<IObjectBaseFactory>();
         _spatialStructure = new MoBiSpatialStructure
         {
            NeighborhoodsContainer = new Container()
         };
         A.CallTo(() => _objectBaseFactory.Create<ParameterValue>()).ReturnsLazily(() => new ParameterValue());
         _pathAndValueEntityToParameterValueMapper = new PathAndValueEntityToParameterValueMapper(_objectBaseFactory, A.Fake<ICloneManagerForModel>());
         A.CallTo(() => _applicationController.Start<ISelectFolderAndIndividualAndExpressionFromProjectPresenter>()).Returns(_selectIndividualAndExpressionFromProjectPresenter);
         A.CallTo(() => _interactionTaskContext.ApplicationController).Returns(_applicationController);
         _interactionTask = A.Fake<IInteractionTask>();
         _objectPathFactory = new ObjectPathFactoryForSpecs();
         A.CallTo(() => _interactionTaskContext.InteractionTask).Returns(_interactionTask);
         A.CallTo(() => _individualParameterToParameterMapper.MapFrom(A<IndividualParameter>._)).ReturnsLazily(x => newParameter(x.Arguments.Get<IndividualParameter>(0)));
         A.CallTo(() => _spatialStructureFactory.Create()).Returns(_spatialStructure);
         sut = new SpatialStructureContentExporter(_spatialStructureFactory, _cloneManager, _objectPathFactory, _pathAndValueEntityToParameterValueMapper, _formulaFactory, _individualParameterToParameterMapper, _interactionTaskContext, _applicationController);
      }

      private static IParameter newParameter(IndividualParameter individualParameter)
      {
         if (individualParameter.DistributionType == null)
            return new Parameter().WithName(individualParameter.Name);

         return new DistributedParameter().WithName(individualParameter.Name);
      }
   }

   public class When_saving_an_neighborhood_to_pkml : concern_for_SpatialStructureContentExporter
   {
      private NeighborhoodBuilder _entityToSave;
      private NeighborhoodBuilder _clonedEntity;

      protected override void Context()
      {
         base.Context();
         _clonedEntity = new NeighborhoodBuilder();
         _entityToSave = new NeighborhoodBuilder();
         A.CallTo(() => _interactionTask.AskForFileToSave(A<string>._, A<string>._, A<string>._, A<string>._)).Returns("filename");

         A.CallTo(() => _cloneManager.Clone(_entityToSave, _spatialStructure.FormulaCache)).Returns(_clonedEntity);
      }

      protected override void Because()
      {
         sut.Save(_entityToSave);
      }

      [Observation]
      public void a_temporary_spatial_structure_is_created()
      {
         A.CallTo(() => _spatialStructureFactory.Create()).MustHaveHappened();
      }

      [Observation]
      public void the_saved_neighborhood_is_cloned()
      {
         A.CallTo(() => _cloneManager.Clone(_entityToSave, _spatialStructure.FormulaCache)).MustHaveHappened();
      }

      [Observation]
      public void the_cloned_entity_is_added_to_the_temporary_structure()
      {
         _spatialStructure.Neighborhoods.ShouldContain(_clonedEntity);
      }

      [Observation]
      public void the_file_is_saved()
      {
         A.CallTo(() => _interactionTask.Save(A<MoBiSpatialStructure>._, "filename")).MustHaveHappened();
      }
   }

   public class When_saving_an_neighborhood_to_pkml_but_the_file_dialog_is_canceled : concern_for_SpatialStructureContentExporter
   {
      private NeighborhoodBuilder _entityToSave;

      protected override void Context()
      {
         base.Context();
         _entityToSave = new NeighborhoodBuilder();
         A.CallTo(() => _interactionTask.AskForFileToSave(A<string>._, A<string>._, A<string>._, A<string>._)).Returns(string.Empty);
      }

      protected override void Because()
      {
         sut.Save(_entityToSave);
      }

      [Observation]
      public void A_call_to_create_temporary_structure_must_not_have_happened()
      {
         A.CallTo(() => _spatialStructureFactory.Create()).MustNotHaveHappened();
      }
   }

   public class When_saving_a_container_to_pkml_but_the_file_dialog_is_canceled : concern_for_SpatialStructureContentExporter
   {
      private IContainer _entityToSave;

      protected override void Context()
      {
         base.Context();
         _entityToSave = new Container();
         A.CallTo(() => _interactionTask.AskForFileToSave(A<string>._, A<string>._, A<string>._, A<string>._)).Returns(string.Empty);
      }

      protected override void Because()
      {
         sut.Save(_entityToSave);
      }

      [Observation]
      public void A_call_to_create_temporary_structure_must_not_have_happened()
      {
         A.CallTo(() => _spatialStructureFactory.Create()).MustNotHaveHappened();
      }
   }

   public class When_saving_a_container_with_individual_to_pkml : concern_for_SpatialStructureContentExporter
   {
      private IContainer _clonedContainer;
      private Container _parentContainer;
      private MoBiSpatialStructure _tmpSpatialStructure;
      private IndividualBuildingBlock _individual;
      private IContainer _containerToSave;
      private Parameter _replacedParameter;
      private IndividualParameter _parameterWithoutContainer;
      protected ParameterValuesBuildingBlock _parameterValuesBuildingBlock;
      private List<ExpressionProfileBuildingBlock> _expressionProfiles;
      private SpatialStructureTransfer _transfer;

      protected override void Context()
      {
         base.Context();
         _parentContainer = new Container().WithName("Parent");
         _individual = new IndividualBuildingBlock().WithName("Individual");
         _individual.Add(new IndividualParameter { ContainerPath = new ObjectPath("Parent", "Container1") }.WithName("parameter1"));


         _individual.Add(new IndividualParameter { DistributionType = DistributionType.Normal, ContainerPath = new ObjectPath("Parent", "Container1") }.WithName("distributedParameter1"));
         _individual.Add(new IndividualParameter { ContainerPath = new ObjectPath("Parent", "Container1", "distributedParameter1") }.WithName("Mean"));
         _individual.Add(new IndividualParameter { ContainerPath = new ObjectPath("Parent", "Container1", "distributedParameter1") }.WithName("StandardDeviation"));

         _individual.Add(new IndividualParameter { DistributionType = DistributionType.Normal, ContainerPath = new ObjectPath("Parent", "Container1") }.WithName("distributedParameter1-WS"));
         _individual.Add(new IndividualParameter { ContainerPath = new ObjectPath("Parent", "Container1", "distributedParameter1-WS") }.WithName("Mean"));
         _individual.Add(new IndividualParameter { ContainerPath = new ObjectPath("Parent", "Container1", "distributedParameter1-WS") }.WithName("StandardDeviation"));

         var replacementIndividualParameter = new IndividualParameter { ContainerPath = new ObjectPath("Parent", "Container1") }.WithName("ShouldBeReplaced");
         replacementIndividualParameter.Value = 1;
         _individual.Add(replacementIndividualParameter);
         _individual.Add(new IndividualParameter { ContainerPath = new ObjectPath("Parent", "Container2") }.WithName("parameter2"));

         _parameterWithoutContainer = new IndividualParameter { ContainerPath = new ObjectPath("Parent", "Container1", "Container3") }.WithName("ContainerDoesNotExist");
         _individual.Add(_parameterWithoutContainer);

         _containerToSave = new Container().WithName("Container1").WithMode(ContainerMode.Physical).Under(_parentContainer);
         _clonedContainer = new Container().WithName("Container1").WithMode(ContainerMode.Physical);
         _clonedContainer.Add(new DistributedParameter().WithName("distributedParameter1"));

         _replacedParameter = new Parameter().WithName("ShouldBeReplaced");
         _clonedContainer.Add(_replacedParameter);

         var expressionProfileBuildingBlock = new ExpressionProfileBuildingBlock
         {
            new InitialCondition { ContainerPath = new ObjectPath("Parent", "Container1") }.WithName("initialCondition1"),
            new ExpressionParameter { ContainerPath = new ObjectPath("Parent", "Container1") }.WithName("expression1")
         };

         _expressionProfiles = new List<ExpressionProfileBuildingBlock>
         {
            expressionProfileBuildingBlock
         };

         _parameterValuesBuildingBlock = new ParameterValuesBuildingBlock();

         _tmpSpatialStructure = new MoBiSpatialStructure
         {
            NeighborhoodsContainer = new Container().WithName(Constants.NEIGHBORHOODS)
         };
         _tmpSpatialStructure.AddTopContainer(new Container().WithName(Constants.EVENTS));

         A.CallTo(() => _selectIndividualAndExpressionFromProjectPresenter.GetPathIndividualAndExpressionsForExport(_containerToSave)).Returns(("FilePath", _individual, _expressionProfiles));
         A.CallTo(() => _spatialStructureFactory.Create()).Returns(_tmpSpatialStructure);
         A.CallTo(() => _cloneManager.Clone(_containerToSave, _tmpSpatialStructure.FormulaCache)).Returns(_clonedContainer);
         A.CallTo(() => _formulaFactory.ConstantFormula(replacementIndividualParameter.Value.Value, replacementIndividualParameter.Dimension)).ReturnsLazily(x => new ConstantFormula(x.Arguments.Get<double>(0)));
         A.CallTo(() => _interactionTaskContext.Context.Create<ParameterValuesBuildingBlock>()).Returns(_parameterValuesBuildingBlock);
         A.CallTo(() => _interactionTask.Save(A<SpatialStructureTransfer>._, A<string>._)).Invokes(x => _transfer = x.Arguments.Get<SpatialStructureTransfer>(0));
         A.CallTo(() => _cloneManager.Clone(A<InitialCondition>._, A<FormulaCache>._)).ReturnsLazily(x => newInitialCondition(x.Arguments.Get<InitialCondition>(0)));
      }

      private static InitialCondition newInitialCondition(InitialCondition oldInitialCondition)
      {
         return new InitialCondition
         {
            Path = oldInitialCondition.Path
         };
      }

      protected override void Because()
      {
         sut.SaveWithIndividualAndExpression(_containerToSave);
      }

      [Observation]
      public void transfer_must_contain_objects_to_export()
      {
         _transfer.ParameterValues.ShouldBeEqualTo(_parameterValuesBuildingBlock);
         _transfer.ParameterValues.FindByPath("Parent|Container1|expression1").ShouldNotBeNull();
         _transfer.InitialConditions.FindByPath("Parent|Container1|initialCondition1").ShouldNotBeNull();
         _transfer.SpatialStructure.ShouldBeEqualTo(_tmpSpatialStructure);
         _transfer.SpatialStructure.TopContainers.SingleOrDefault(x => x.IsNamed(Constants.EVENTS)).ShouldBeNull();
      }

      [Observation]
      public void the_parameter_is_not_created_when_the_container_does_not_exist()
      {
         _parameterValuesBuildingBlock.FindByPath("Parent|Container1|Container3|ContainerDoesNotExist").ShouldBeNull();
      }

      [Observation]
      public void the_top_container_should_be_created()
      {
         _tmpSpatialStructure.TopContainers.ShouldOnlyContain(_clonedContainer);
      }

      [Observation]
      public void simple_parameters_are_included_in_the_exported_spatial_structure()
      {
         _clonedContainer.FindByName("ShouldBeReplaced").ShouldNotBeNull();
         _clonedContainer.FindByName("parameter1").ShouldNotBeNull();
      }

      [Observation]
      public void the_distributed_parameter_should_contain_sub_parameters_when_the_distributed_parameter_is_not_in_the_container()
      {
         _clonedContainer.FindByName("distributedParameter1-WS").ShouldNotBeNull();
         _clonedContainer.GetAllContainersAndSelf<IDistributedParameter>().FindByName("distributedParameter1-WS").FindByName("Mean").ShouldNotBeNull();
         _clonedContainer.GetAllContainersAndSelf<IDistributedParameter>().FindByName("distributedParameter1-WS").FindByName("StandardDeviation").ShouldNotBeNull();
      }

      [Observation]
      public void the_distributed_parameter_should_contain_sub_parameters_when_the_distributed_parameter_is_in_the_container()
      {
         _clonedContainer.FindByName("distributedParameter1").ShouldNotBeNull();
         _clonedContainer.GetAllContainersAndSelf<IDistributedParameter>().FindByName("distributedParameter1").FindByName("Mean").ShouldNotBeNull();
         _clonedContainer.GetAllContainersAndSelf<IDistributedParameter>().FindByName("distributedParameter1").FindByName("StandardDeviation").ShouldNotBeNull();
      }

      [Observation]
      public void constant_formulas_should_be_used_instead_of_fixed_values()
      {
         _tmpSpatialStructure.TopContainers.Single().GetAllChildren<IParameter>().All(x => x.IsFixedValue == false).ShouldBeTrue();
      }
   }

   public class When_saving_a_container_to_pkml : concern_for_SpatialStructureContentExporter
   {
      private IContainer _entityToSave;
      private Container _parentContainer;
      private MoBiSpatialStructure _tmpSpatialStructure;
      private MoBiSpatialStructure _activeSpatialStructure;
      private NeighborhoodBuilder _neighborhood1;
      private NeighborhoodBuilder _neighborhood2;

      protected override void Context()
      {
         base.Context();
         _parentContainer = new Container().WithName("Parent");
         _entityToSave = new Container().WithName("Container").WithMode(ContainerMode.Physical).Under(_parentContainer);
         //this is a spatial structure that will be created on the fly to export the containers and neighborhoods to the DB
         _tmpSpatialStructure = new MoBiSpatialStructure
         {
            NeighborhoodsContainer = new Container().WithName(Constants.NEIGHBORHOODS)
         };

         //this is the spatial structure that is active at the moment and that will be used to find all neighborhoods to export
         _activeSpatialStructure = new MoBiSpatialStructure
         {
            NeighborhoodsContainer = new Container().WithName(Constants.NEIGHBORHOODS)
         };

         _neighborhood1 = new NeighborhoodBuilder().WithName("_neighborhood1");
         _neighborhood2 = new NeighborhoodBuilder().WithName("_neighborhood2");

         _neighborhood1.FirstNeighborPath = _objectPathFactory.CreateAbsoluteObjectPath(_entityToSave);

         _activeSpatialStructure.AddNeighborhood(_neighborhood1);
         _activeSpatialStructure.AddNeighborhood(_neighborhood2);
         A.CallTo(_interactionTask).WithReturnType<string>().Returns("FilePath");
         A.CallTo(() => _spatialStructureFactory.Create()).Returns(_tmpSpatialStructure);
         A.CallTo(() => _interactionTaskContext.Active<MoBiSpatialStructure>()).Returns(_activeSpatialStructure);
      }

      protected override void Because()
      {
         sut.Save(_entityToSave);
      }

      [Observation]
      public void should_have_kept_the_reference_to_the_parent_container()
      {
         _entityToSave.ParentContainer.ShouldBeEqualTo(_parentContainer);
      }

      [Observation]
      public void should_have_named_the_spatial_structure_that_was_created()
      {
         _tmpSpatialStructure.Name.ShouldBeEqualTo(DefaultNames.SpatialStructure);
      }

      [Observation]
      public void should_have_exported_all_neighborhoods_connected_to_the_container_or_one_of_its_sub_containers()
      {
         _tmpSpatialStructure.Neighborhoods.ShouldContain(_neighborhood1);
         _tmpSpatialStructure.Neighborhoods.ShouldNotContain(_neighborhood2);
      }
   }
}