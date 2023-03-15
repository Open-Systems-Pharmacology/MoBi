using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.UnitSystem;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation
{
   public abstract class concern_for_ObjectPathCreatorAtReactionParameterSpecs : ContextSpecification<IObjectPathCreator>
   {
      protected IMoBiContext _context;

      protected override void Context()
      {
         IAliasCreator aliasCreator = new AliasCreator();
         _context = A.Fake<IMoBiContext>();
         sut = new ObjectPathCreatorAtReactionParameter(new ObjectPathFactory(aliasCreator), aliasCreator, _context);
      }
   }

   class When_asking_object_path_creator_for_a_relative_path_to_a_local_parameter_in_the_same_reaction : concern_for_ObjectPathCreatorAtReactionParameterSpecs
   {
      private ReferenceDTO _result;
      private IParameter _reacPara2;
      private IParameter _reacPara1;

      protected override void Context()
      {
         base.Context();
         var reaction = new ReactionBuilder().WithName("R1");
         _reacPara1 = new Parameter().WithName("P1").WithMode(ParameterBuildMode.Local).WithParentContainer(reaction);
         _reacPara2 = new Parameter().WithName("P2").WithMode(ParameterBuildMode.Local).WithParentContainer(reaction);
      }

      protected override void Because()
      {
         _result = sut.CreatePathsFromEntity(_reacPara2, false, A.Fake<IContainer>(), _reacPara1);
      }

      [Observation]
      public void should_return_the_right_reference_object()
      {
         _result.ShouldNotBeNull();
         _result.BuildMode.ShouldBeEqualTo(ParameterBuildMode.Local);
         _result.Path.ShouldOnlyContainInOrder(ObjectPath.PARENT_CONTAINER,_reacPara2.Name);
      }
   }

   class When_asking_object_path_creator_for_a_relative_path_to_a_local_parameter_in_a_other_reaction : concern_for_ObjectPathCreatorAtReactionParameterSpecs
   {
      private ReferenceDTO _result;
      private IParameter _reacPara2;
      private IParameter _reacPara1;
      private IReactionBuilder _reaction2;

      protected override void Context()
      {
         base.Context();
         var reaction = new ReactionBuilder().WithName("R1");
         _reaction2 = new ReactionBuilder().WithName("R1");
         _reacPara1 = new Parameter().WithName("P1").WithMode(ParameterBuildMode.Local).WithParentContainer(reaction);
         _reacPara2 = new Parameter().WithName("P2").WithMode(ParameterBuildMode.Local).WithParentContainer(_reaction2);
      }

      protected override void Because()
      {
         _result = sut.CreatePathsFromEntity(_reacPara2, false, A.Fake<IContainer>(), _reacPara1);
      }

      [Observation]
      public void should_return_the_right_reference_object()
      {
         _result.ShouldNotBeNull();
         _result.BuildMode.ShouldBeEqualTo(ParameterBuildMode.Local);
         _result.Path.ShouldOnlyContainInOrder(ObjectPath.PARENT_CONTAINER,ObjectPath.PARENT_CONTAINER,_reaction2.Name, _reacPara2.Name);
      }


   }


   class When_asking_object_path_creator_for_a_relative_spatial_structure_parameter_reference_in_a_reaction_parameter : concern_for_ObjectPathCreatorAtReactionParameterSpecs
   {
      private IContainer _refContainer;
      private IParameter _parameter;
      private ReferenceDTO _result;

      protected override void Context()
      {
         base.Context();
         _refContainer = new Container().WithName("Compartment");
         _parameter = new Parameter().WithName("Volume").WithParentContainer(_refContainer).WithDimension(A.Fake<IDimension>());
      }

      protected override void Because()
      {
         _result = sut.CreatePathsFromEntity(_parameter, false, _refContainer, _parameter);
      }

      [Observation]
      public void should_return_right_reference()
      {
         _result.ShouldNotBeNull();
         _result.Path.Alias.ShouldBeEqualTo(_parameter.Name);
         _result.Path.Dimension.ShouldBeEqualTo(_parameter.Dimension);
         _result.Path.ShouldOnlyContainInOrder(ObjectPath.PARENT_CONTAINER, ObjectPath.PARENT_CONTAINER, _parameter.Name);
      }
   }

   class When_asking_object_path_creator_for_a_absolute_spatial_structure_parameter_reference_in_a_reaction_parameter : concern_for_ObjectPathCreatorAtReactionParameterSpecs
   {
      private IContainer _refContainer;
      private IParameter _parameter;
      private ReferenceDTO _result;

      protected override void Context()
      {
         base.Context();
         _refContainer = new Container().WithName("Compartment");
         _parameter = new Parameter().WithName("Volume").WithParentContainer(_refContainer).WithDimension(A.Fake<IDimension>());
      }

      protected override void Because()
      {
         _result = sut.CreatePathsFromEntity(_parameter, true, _refContainer, _parameter);
      }

      [Observation]
      public void should_return_right_reference()
      {
         _result.ShouldNotBeNull();
         _result.Path.Alias.ShouldBeEqualTo(_parameter.Name);
         _result.Path.Dimension.ShouldBeEqualTo(_parameter.Dimension);
         _result.Path.ShouldOnlyContainInOrder(_refContainer.Name, _parameter.Name);
      }

   }

   class When_asking_abolute_object_path_creator_for_a_Molecule_Dummy_Parameter_Reference_used_in_a_reaction_parameter : concern_for_ObjectPathCreatorAtReactionParameterSpecs
   {
      private DummyParameterDTO _dtoMoleculeParameter;
      private ReferenceDTO _result;
      private IDimension _rightDimension;
      private string _parameterName;
      private string _moleculeName;
      private Container _container;

      protected override void Context()
      {
         base.Context();
         var dimensionFactory = A.Fake<IMoBiDimensionFactory>();
         _rightDimension = A.Fake<IDimension>();
         A.CallTo(() => dimensionFactory.Dimension(Constants.Dimension.MOLAR_AMOUNT)).Returns(_rightDimension);
         A.CallTo(() => _context.DimensionFactory).Returns(dimensionFactory);
         _parameterName = "Para";
         var parameter = new Parameter().WithName(_parameterName).WithId(_parameterName).WithDimension(_rightDimension);
         _dtoMoleculeParameter = new DummyParameterDTO(parameter).WithName(_parameterName);
         var organ = new Container().WithName("Organ");
         _container = new Container().WithName("Container");
         organ.Add(_container);

       

         _moleculeName = "Drug";
         var moleculeProperties = new Container().WithName(Constants.MOLECULE_PROPERTIES);
         _dtoMoleculeParameter.ModelParentName = _moleculeName;
         moleculeProperties.Add(parameter);

         _dtoMoleculeParameter.Parent = moleculeProperties;
         _container.Add(moleculeProperties);
      }

      protected override void Because()
      {
         _result = sut.CreatePathFromParameterDummy(_dtoMoleculeParameter, false, _container, A.Fake<IUsingFormula>());
      }

      [Observation]
      public void should_create_a_right_reference()
      {
         _result.ShouldNotBeNull();
         _result.Path.Alias.ShouldBeEqualTo(_parameterName);
         _result.Path.Dimension.ShouldBeEqualTo(_rightDimension);
         _result.Path.ShouldOnlyContainInOrder(ObjectPath.PARENT_CONTAINER, ObjectPath.PARENT_CONTAINER, _moleculeName, _parameterName);
      }
   }

}
