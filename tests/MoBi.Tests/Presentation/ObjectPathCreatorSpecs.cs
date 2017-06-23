using System;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.UnitSystem;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;


namespace MoBi.Presentation
{
    public abstract class concern_for_ObjectPathCreatorSpecs : ContextSpecification<IObjectPathCreator>
    {
        protected IMoBiContext _context;

        protected override void Context()
        {
            IAliasCreator aliasCreator=new AliasCreator();
            _context = A.Fake<IMoBiContext>();
            sut = new ObjectPathCreatorAtParameter(new ObjectPathFactory(aliasCreator),aliasCreator,_context);
        }
    }

    class When_creating_a_realtiv_path_to_a_local_moleculeProperty : concern_for_ObjectPathCreatorSpecs
    {
       private DummyParameterDTO _concentrationDTO;
       private IContainer _localCompartment;
       private ReferenceDTO _referenceDTO;
       private IDimension _dimension;
       private IParameter _localParameter;

       protected override void Context()
       {
          base.Context();
          _concentrationDTO = new DummyParameterDTO().WithId("Dum");

          ObjectBaseDTO concentrationParameterDTO = new ObjectBaseDTO().WithName("Concentration").WithId(Guid.NewGuid().ToString());

          _concentrationDTO.ParameterToUse = concentrationParameterDTO;
          _concentrationDTO.ModelParentName = "Drug";
          _localCompartment = new Container().WithName("Plasma");
          _localParameter = new Parameter().WithName("PRef").WithParentContainer(_localCompartment);
          IContainer moleculeProperties = new Container().WithName(Constants.MOLECULE_PROPERTIES).WithParentContainer(_localCompartment);
          _concentrationDTO.Parent = moleculeProperties;
          _dimension = A.Fake<IDimension>();
          var concentrationParameter =
             new Parameter().WithName("Concentration")
                            .WithId(concentrationParameterDTO.Id)
                            .WithDimension(_dimension)
                            .WithParentContainer(new MoleculeBuilder().WithName("Drug"));
          concentrationParameter.BuildMode = ParameterBuildMode.Local;
          A.CallTo(() => _context.Get<IParameter>(concentrationParameterDTO.Id)).Returns(concentrationParameter);
       }

       protected override void Because()
       {
          _referenceDTO = sut.CreatePathFromParameterDummy(_concentrationDTO, shouldCreateAbsolutePaths: false, refObject: _localParameter, editedObject:_localParameter);
       }

       [Observation]
       public void should_return_correct_refernece_dto()
       {
          _referenceDTO.BuildMode.ShouldBeEqualTo(ParameterBuildMode.Local);
          _referenceDTO.Path.ShouldOnlyContainInOrder(ObjectPath.PARENT_CONTAINER, "Drug", "Concentration");
          _referenceDTO.Path.Dimension.ShouldBeEqualTo(_dimension);
       }
    }

    class When_asking_objectpath_creator_for_an_absolute_path_for_an_molecule : concern_for_ObjectPathCreatorSpecs
    {
        private DummyMoleculeContainerDTO _dummyMolecule;
        private ReferenceDTO _result;
        private string _moleculeName = "Drug"; 
        private IObjectBaseDTO _moleculePropertiesDTO;
        private string _moleculePropertiesID = Constants.MOLECULE_PROPERTIES;
        private IContainer _moleculeProperties;
        private IDimension _rightDimension;
        private string _rootName="Root";
        private string _parentName="Parent";

        protected override void Context()
        {
            base.Context();
            _moleculePropertiesDTO = new ObjectBaseDTO().WithId(_moleculePropertiesID);
            _moleculeProperties = new Container().WithName(Constants.MOLECULE_PROPERTIES).WithId(_moleculePropertiesID);
            _dummyMolecule = new DummyMoleculeContainerDTO { Name = _moleculeName, MoleculePropertiesContainer = _moleculePropertiesDTO };
            A.CallTo(() => _context.Get<IContainer>(_moleculePropertiesID)).Returns(_moleculeProperties);
            var dimensionFactory = A.Fake<IMoBiDimensionFactory>();
            _rightDimension = A.Fake<IDimension>();
            A.CallTo(() => dimensionFactory.Dimension(Constants.Dimension.AMOUNT)).Returns(_rightDimension);
            A.CallTo(() => _context.DimensionFactory).Returns(dimensionFactory);
            var rootContainer = new Container().WithName(_rootName);
            var parentContainer = new Container().WithName(_parentName);
            rootContainer.Add(parentContainer);
            parentContainer.Add(_moleculeProperties);
        }

        protected override void Because()
        {
            _result = sut.CreateMoleculePath(_dummyMolecule, true, A.Fake<IEntity>());
        }

        [Observation]
        public void should_return_right_reference_dto()
        {
            _result.ShouldNotBeNull();
            _result.Path.ShouldOnlyContainInOrder(_rootName,_parentName,_moleculeName);
            _result.Path.Alias.ShouldBeEqualTo(_moleculeName);
            _result.Path.Dimension.ShouldBeEqualTo(_rightDimension);
            _result.BuildMode.ShouldBeEqualTo(ParameterBuildMode.Local);
        }
    }

    class When_asking_objectpath_creator_for_a_realtive_path_for_an_molecule : concern_for_ObjectPathCreatorSpecs
    {
        private DummyMoleculeContainerDTO _dummyMolecule;
        private ReferenceDTO _result;
        private string _moleculeName = "Drug";
        private IObjectBaseDTO _moleculePropertiesDTO;
        private string _moleculePropertiesID = Constants.MOLECULE_PROPERTIES;
        private IContainer _moleculeProperties;
        private IDimension _rightDimension;
        private string _rootName = "Root";
        private string _parentName = "Parent";
        private Parameter _refObject;

        protected override void Context()
        {
            base.Context();
            _moleculePropertiesDTO = new ObjectBaseDTO().WithId(_moleculePropertiesID);
            _moleculeProperties = new Container().WithName(Constants.MOLECULE_PROPERTIES).WithId(_moleculePropertiesID);
            _dummyMolecule = new DummyMoleculeContainerDTO { Name = _moleculeName, MoleculePropertiesContainer = _moleculePropertiesDTO };
            A.CallTo(() => _context.Get<IContainer>(_moleculePropertiesID)).Returns(_moleculeProperties);
            var dimensionFactory = A.Fake<IMoBiDimensionFactory>();
            _rightDimension = A.Fake<IDimension>();
            A.CallTo(() => dimensionFactory.Dimension(Constants.Dimension.AMOUNT)).Returns(_rightDimension);
            A.CallTo(() => _context.DimensionFactory).Returns(dimensionFactory);
            var rootContainer = new Container().WithName(_rootName);
            var parentContainer = new Container().WithName(_parentName);
            rootContainer.Add(parentContainer);
            parentContainer.Add(_moleculeProperties);
            _refObject = new Parameter();
            rootContainer.Add(_refObject);
        }

        protected override void Because()
        {
            _result = sut.CreateMoleculePath(_dummyMolecule, false,_refObject);
        }

        [Observation]
        public void should_return_right_reference_dto()
        {
            _result.ShouldNotBeNull();
            _result.Path.ShouldOnlyContainInOrder(ObjectPath.PARENT_CONTAINER, _parentName, _moleculeName);
            _result.Path.Alias.ShouldBeEqualTo(_moleculeName);
            _result.Path.Dimension.ShouldBeEqualTo(_rightDimension);
            _result.BuildMode.ShouldBeEqualTo(ParameterBuildMode.Local);
        }
    }

    class When_asking_objectpath_creator_for_an_realtive_path_for_an_molecule_then_ref_object_is_parent_container : concern_for_ObjectPathCreatorSpecs
    {
        private DummyMoleculeContainerDTO _dummyMolecule;
        private ReferenceDTO _result;
        private string _moleculeName = "Drug";
        private IObjectBaseDTO _moleculePropertiesDTO;
        private string _moleculePropertiesID = Constants.MOLECULE_PROPERTIES;
        private IContainer _moleculeProperties;
        private IDimension _rightDimension;
        private string _parentName = "Parent";
        private Container _parentContainer;

        protected override void Context()
        {
            base.Context();
            _moleculePropertiesDTO = new ObjectBaseDTO().WithId(_moleculePropertiesID);
            _moleculeProperties = new Container().WithName(Constants.MOLECULE_PROPERTIES).WithId(_moleculePropertiesID);
            _dummyMolecule = new DummyMoleculeContainerDTO { Name = _moleculeName, MoleculePropertiesContainer = _moleculePropertiesDTO };
            A.CallTo(() => _context.Get<IContainer>(_moleculePropertiesID)).Returns(_moleculeProperties);
            var dimensionFactory = A.Fake<IMoBiDimensionFactory>();
            _rightDimension = A.Fake<IDimension>();
            A.CallTo(() => dimensionFactory.Dimension(Constants.Dimension.AMOUNT)).Returns(_rightDimension);
            A.CallTo(() => _context.DimensionFactory).Returns(dimensionFactory);
            _parentContainer = new Container().WithName(_parentName);
            _parentContainer.Add(_moleculeProperties);
            
        }

        protected override void Because()
        {
            _result = sut.CreateMoleculePath(_dummyMolecule, false, _parentContainer);
        }

        [Observation]
        public void should_return_right_reference_dto()
        {
            _result.ShouldNotBeNull();
            _result.Path.ShouldOnlyContainInOrder( _moleculeName);
            _result.Path.Alias.ShouldBeEqualTo(_moleculeName);
            _result.Path.Dimension.ShouldBeEqualTo(_rightDimension);
            _result.BuildMode.ShouldBeEqualTo(ParameterBuildMode.Local);
        }
    }

    public class When_asking_for_a_realtive_path_in_a_container_hierarchie : concern_for_ObjectPathCreatorSpecs
    {
        private ReferenceDTO _result;
        private IParameter _parameter;
        private IParameter _referenceParameter;
        private string _paraName ="Para";
        private string _containerNameOne="One";
        private string _containerNameTwo="TWO";
        private IDimension _rightDimension;

        protected override void Context()
        {
            base.Context();
            _rightDimension = A.Fake<IDimension>();
            _parameter = new Parameter().WithName(_paraName).WithDimension(_rightDimension);
            _referenceParameter = new Parameter().WithName("test");
            var rootContainer = new Container().WithName("Root");
            var containerOne = new Container().WithName(_containerNameOne);
            var containerTwo = new Container().WithName(_containerNameTwo);
            rootContainer.Add(containerOne);
            rootContainer.Add(containerTwo);
            containerOne.Add(_parameter);
            containerTwo.Add(_referenceParameter);
        }
        protected override void Because()
        {
            _result = sut.CreatePathsFromEntity(_parameter, false, _referenceParameter, _referenceParameter);
        }

        [Observation]
        public void should_return_right_reference()
        {
            _result.ShouldNotBeNull();
            _result.Path.ShouldOnlyContainInOrder(ObjectPath.PARENT_CONTAINER,ObjectPath.PARENT_CONTAINER,_containerNameOne,_paraName);
            _result.Path.Alias.ShouldBeEqualTo(_paraName);
            _result.Path.Dimension.ShouldBeEqualTo(_rightDimension);
            _result.BuildMode.ShouldBeEqualTo(ParameterBuildMode.Local);
        }
    }

    public class When_asking_for_an_absolute_path_in_a_container_hierarchie : concern_for_ObjectPathCreatorSpecs
    {
        private ReferenceDTO _result;
        private IParameter _parameter;
        private IParameter _referenceParameter;
        private string _paraName = "Para";
        private string _containerNameOne = "One";
        private string _containerNameTwo = "TWO";
        private IDimension _rightDimension;
        private string _rootName;

        protected override void Context()
        {
            base.Context();
            _rightDimension = A.Fake<IDimension>();
            _parameter = new Parameter().WithName(_paraName).WithDimension(_rightDimension);
            _referenceParameter = new Parameter().WithName("test");
            _rootName = "Root";
            var rootContainer = new Container().WithName(_rootName);
            var containerOne = new Container().WithName(_containerNameOne);
            var containerTwo = new Container().WithName(_containerNameTwo);
            rootContainer.Add(containerOne);
            rootContainer.Add(containerTwo);
            containerOne.Add(_parameter);
            containerTwo.Add(_referenceParameter);
        }
        protected override void Because()
        {
            _result = sut.CreatePathsFromEntity(_parameter, true, _referenceParameter, _referenceParameter);
        }

        [Observation]
        public void should_return_right_reference()
        {
            _result.ShouldNotBeNull();
            _result.Path.ShouldOnlyContainInOrder(_rootName, _containerNameOne, _paraName);
            _result.Path.Alias.ShouldBeEqualTo(_paraName);
            _result.Path.Dimension.ShouldBeEqualTo(_rightDimension);
            _result.BuildMode.ShouldBeEqualTo(ParameterBuildMode.Local);
        }
    }

    public class When_asking_for_absolute_objectpath_for_an_global_Molecule_parameter : concern_for_ObjectPathCreatorSpecs
    {
        private IParameter _parameter;
        private IDimension _rightDimension;
        private string _moleculeName;
        private ReferenceDTO _result;
        private string _parameterName;

        protected override void Context()
        {
            base.Context();
            _rightDimension = A.Fake<IDimension>();
            _parameterName = "Para";
            _parameter = new Parameter().WithName("Global").WithDimension(_rightDimension).WithName(_parameterName);
            _parameter.BuildMode = ParameterBuildMode.Global;
            _moleculeName = "Drug";
            var moleculeBuilder = new MoleculeBuilder().WithName(_moleculeName);
            moleculeBuilder.AddParameter(_parameter);
        }

        protected override void Because()
        {
           _result = sut.CreatePathsFromEntity(_parameter, true, A.Fake<IEntity>(), A.Fake<IUsingFormula>());
        }

        [Observation]
        public void should_return_right_reference()
        {
            _result.ShouldNotBeNull();
            _result.Path.ShouldOnlyContainInOrder(_moleculeName,_parameterName);
            _result.Path.Alias.ShouldBeEqualTo(_parameterName);
            _result.Path.Dimension.ShouldBeEqualTo(_rightDimension);
            _result.BuildMode.ShouldBeEqualTo(ParameterBuildMode.Global);
        }
    }
    class When_asking_for_relative_objectpath_for_an_global_Molecule_parameter : concern_for_ObjectPathCreatorSpecs
    {
        private IParameter _parameter;
        private IDimension _rightDimension;
        private string _moleculeName;
        private ReferenceDTO _result;
        private string _parameterName;

        protected override void Context()
        {
            base.Context();
            _rightDimension = A.Fake<IDimension>();
            _parameterName = "Para";
            _parameter = new Parameter().WithName("Global").WithDimension(_rightDimension).WithName(_parameterName);
            _parameter.BuildMode = ParameterBuildMode.Global;
            _moleculeName = "Drug";
            var moleculeBuilder = new MoleculeBuilder().WithName(_moleculeName);
            moleculeBuilder.AddParameter(_parameter);
        }

        protected override void Because()
        {
           _result = sut.CreatePathsFromEntity(_parameter, false, A.Fake<IEntity>(), A.Fake<IUsingFormula>());
        }

        [Observation]
        public void should_return_right_reference()
        {
            _result.ShouldNotBeNull();
            _result.Path.ShouldOnlyContainInOrder(_moleculeName, _parameterName);
            _result.Path.Alias.ShouldBeEqualTo(_parameterName);
            _result.Path.Dimension.ShouldBeEqualTo(_rightDimension);
            _result.BuildMode.ShouldBeEqualTo(ParameterBuildMode.Global);
        }
    }

   
    class When_asking_for_relative_objectpath_for_an_global_Reaction_parameter : concern_for_ObjectPathCreatorSpecs
    {
        private IParameter _parameter;
        private IDimension _rightDimension;
        private string _reactionName;
        private ReferenceDTO _result;
        private string _parameterName;

        protected override void Context()
        {
            base.Context();
            _rightDimension = A.Fake<IDimension>();
            _parameterName = "Para";
            _parameter = new Parameter().WithName("Global").WithDimension(_rightDimension).WithName(_parameterName);
            _parameter.BuildMode = ParameterBuildMode.Global;
            _reactionName = "Reaction";
            var reactionBuilder = new ReactionBuilder().WithName(_reactionName);
            reactionBuilder.AddParameter(_parameter);
        }

        protected override void Because()
        {
           _result = sut.CreatePathsFromEntity(_parameter, false, A.Fake<IEntity>(), A.Fake<IUsingFormula>());
        }

        [Observation]
        public void should_return_right_reference()
        {
            _result.ShouldNotBeNull();
            _result.Path.ShouldOnlyContainInOrder(_reactionName, _parameterName);
            _result.Path.Alias.ShouldBeEqualTo(_parameterName);
            _result.Path.Dimension.ShouldBeEqualTo(_rightDimension);
            _result.BuildMode.ShouldBeEqualTo(ParameterBuildMode.Global);
        }
    }

    class When_asking_for_absolute_objectpath_for_an_local_Molecule_parameter : concern_for_ObjectPathCreatorSpecs
    {
        private IParameter _parameter;
        private IDimension _rightDimension;
        private string _moleculeName;
        private ReferenceDTO _result;
        private string _parameterName;
        private IParameter _referenceParameter;

        protected override void Context()
        {
            base.Context();
            _rightDimension = A.Fake<IDimension>();
            _parameterName = "Para";
            _parameter = new Parameter().WithName("Local").WithDimension(_rightDimension).WithName(_parameterName);
            _parameter.BuildMode = ParameterBuildMode.Local;
            _moleculeName = "Drug";
            var moleculeBuilder = new MoleculeBuilder().WithName(_moleculeName);
            moleculeBuilder.AddParameter(_parameter);
            _referenceParameter = new Parameter().WithName("Para");
            var parentContainer = new Container().WithName("Cont");
            parentContainer.Add(_referenceParameter);

        }

        protected override void Because()
        {
           _result = sut.CreatePathsFromEntity(_parameter, true, _referenceParameter, _referenceParameter);
        }

        [Observation]
        public void should_return_right_reference()
        {
            _result.ShouldNotBeNull();
            _result.Path.ShouldOnlyContainInOrder(_moleculeName, _parameterName);
            _result.Path.Alias.ShouldBeEqualTo(_parameterName);
            _result.Path.Dimension.ShouldBeEqualTo(_rightDimension);
            _result.BuildMode.ShouldBeEqualTo(ParameterBuildMode.Local);
        }
    }
    class When_asking_for_relative_objectpath_for_an_local_Molecule_parameter : concern_for_ObjectPathCreatorSpecs
    {
        private IParameter _parameter;
        private IDimension _rightDimension;
        private string _moleculeName;
        private ReferenceDTO _result;
        private string _parameterName;
        private IParameter _referenceParameter;

        protected override void Context()
        {
            base.Context();
            _rightDimension = A.Fake<IDimension>();
            _parameterName = "Para";
            _parameter = new Parameter().WithName("Local").WithDimension(_rightDimension).WithName(_parameterName);
            _parameter.BuildMode = ParameterBuildMode.Local;
            _moleculeName = "Drug";
            var moleculeBuilder = new MoleculeBuilder().WithName(_moleculeName);
            moleculeBuilder.AddParameter(_parameter);
            _referenceParameter = new Parameter().WithName("Para");
            var parentContainer = new Container().WithName("Cont");
            parentContainer.Add(_referenceParameter);
        }

        protected override void Because()
        {
            _result = sut.CreatePathsFromEntity(_parameter, false, _referenceParameter, _referenceParameter);
        }

        [Observation]
        public void should_return_right_reference()
        {
            _result.ShouldNotBeNull();
            _result.Path.ShouldOnlyContainInOrder(_moleculeName, _parameterName);
            _result.Path.Alias.ShouldBeEqualTo(_parameterName);
            _result.Path.Dimension.ShouldBeEqualTo(_rightDimension);
            _result.BuildMode.ShouldBeEqualTo(ParameterBuildMode.Local);
        }
    }


    class When_asking_for_relative_objectpath_for_an_local_Reaction_parameter : concern_for_ObjectPathCreatorSpecs
    {
        private IParameter _parameter;
        private IDimension _rightDimension;
        private string _reactionName;
        private ReferenceDTO _result;
        private string _parameterName;
        private IParameter _referenceParameter;
        private Container _parentContainer;


        protected override void Context()
        {
            base.Context();
            _rightDimension = A.Fake<IDimension>();
            _parameterName = "Para";
            _parameter = new Parameter().WithName("Local").WithDimension(_rightDimension).WithName(_parameterName);
            _parameter.BuildMode = ParameterBuildMode.Local;
            _reactionName = "Reaction";
            var reactionBuilder = new ReactionBuilder().WithName(_reactionName);
            reactionBuilder.AddParameter(_parameter);
            _referenceParameter = new Parameter().WithName("Para");
             _parentContainer = new Container().WithName("Cont");
            _parentContainer.Add(_referenceParameter);
        }

        protected override void Because()
        {
            _result = sut.CreatePathsFromEntity(_parameter, false, _referenceParameter, _referenceParameter);
        }

        [Observation]
        public void should_return_right_reference()
        {
            _result.ShouldNotBeNull();
            _result.Path.ShouldOnlyContainInOrder(ObjectPath.PARENT_CONTAINER,ObjectPath.PARENT_CONTAINER,_reactionName, _parameterName);
            _result.Path.Alias.ShouldBeEqualTo(_parameterName);
            _result.Path.Dimension.ShouldBeEqualTo(_rightDimension);
            _result.BuildMode.ShouldBeEqualTo(ParameterBuildMode.Local);
        }
    }

    class When_asking_object_path_creator_for_the_generic_Molecule_Reference : concern_for_ObjectPathCreatorSpecs
    {
        private IObjectBaseDTO _dtoMolecule;
        private ReferenceDTO _result;
        private IDimension _rightDimension;

        protected override void Context()
        {
            base.Context();
            var dimensionFactory = A.Fake<IMoBiDimensionFactory>();
            _rightDimension = A.Fake<IDimension>();
            A.CallTo(() => dimensionFactory.Dimension(Constants.Dimension.AMOUNT)).Returns(_rightDimension);
            A.CallTo(() => _context.DimensionFactory).Returns(dimensionFactory);
            _dtoMolecule = new ObjectBaseDTO().WithId(ObjectPathKeywords.MOLECULE);

        }

        protected override void Because()
        {
           _result = sut.CreatePathFromParameterDummy(_dtoMolecule, true, A.Fake<IEntity>(), A.Fake<IUsingFormula>());
        }

        [Observation]
        public void should_create_a_parent_reference()
        {
            _result.Path.ShouldOnlyContainInOrder(ObjectPath.PARENT_CONTAINER);
            _result.Path.Alias.ShouldBeEqualTo("M");
            _result.Path.Dimension.ShouldBeEqualTo(_rightDimension);
        }

    }



    class When_asking_abolute_object_path_creator_for_a_Molecule_Parameter_Reference : concern_for_ObjectPathCreatorSpecs
    {
        private DummyParameterDTO _dtoMoleculeParameter;
        private ReferenceDTO _result;
        private IDimension _rightDimension;
        private string _parameterName;
        private string _moleculeName;

        protected override void Context()
        {
            base.Context();
            var dimensionFactory = A.Fake<IMoBiDimensionFactory>();
            _rightDimension = A.Fake<IDimension>();
            A.CallTo(() => dimensionFactory.Dimension(Constants.Dimension.AMOUNT)).Returns(_rightDimension);
            A.CallTo(() => _context.DimensionFactory).Returns(dimensionFactory);
            _dtoMoleculeParameter = new DummyParameterDTO().WithId("ID");
            _parameterName = "Para";
            
            var paramterID = _parameterName;
            var parameterToUse = new ObjectBaseDTO().WithName(_parameterName).WithId(paramterID);
            IParameter parameter = new Parameter().WithName(_parameterName).WithId(paramterID).WithDimension(_rightDimension); ;
            A.CallTo(() => _context.Get<IParameter>(paramterID)).Returns(parameter);
            _dtoMoleculeParameter.ParameterToUse = parameterToUse;
            _moleculeName = "Drug";
            var moleculeBuilder = new MoleculeBuilder().WithName(_moleculeName);
            _dtoMoleculeParameter.Parent = moleculeBuilder;
            moleculeBuilder.AddParameter(parameter);
        }

        protected override void Because()
        {
            _result = sut.CreatePathFromParameterDummy(_dtoMoleculeParameter, true, A.Fake<IEntity>(), A.Fake<IUsingFormula>());
        }

        [Observation]
        public void should_create_a_parent_reference()
        {
            _result.ShouldNotBeNull();
            _result.Path.Alias.ShouldBeEqualTo(_parameterName);
            _result.Path.Dimension.ShouldBeEqualTo(_rightDimension);
            _result.Path.ShouldOnlyContain(_moleculeName,_parameterName);
        }
    }

   internal class When_asking_realtive_object_path_creator_for_a_Molecule_Parameter_Reference :
      concern_for_ObjectPathCreatorSpecs
   {
      private DummyParameterDTO _dtoMoleculeParameter;
      private IDimension _rightDimension;
      private string _parameterName;
      private string _moleculeName;
      private IParameter _refParameter;
      private IContainer _container;

      protected override void Context()
      {
         base.Context();
         var dimensionFactory = A.Fake<IMoBiDimensionFactory>();
         _rightDimension = A.Fake<IDimension>();
         A.CallTo(() => dimensionFactory.Dimension(Constants.Dimension.AMOUNT)).Returns(_rightDimension);
         A.CallTo(() => _context.DimensionFactory).Returns(dimensionFactory);
         _parameterName = "Para";
         var paramterId = _parameterName;
         _dtoMoleculeParameter = new DummyParameterDTO().WithId(_parameterName).WithName(_parameterName);
         var organ = new Container().WithName("Organ");
         _container = new Container().WithName("Container");
         organ.Add(_container);

         var parameterToUse = new ObjectBaseDTO().WithName(_parameterName).WithId(paramterId);

         _dtoMoleculeParameter.ParameterToUse = parameterToUse;

         var parameter = new Parameter().WithName(_parameterName).WithId(paramterId).WithDimension(_rightDimension);

         _moleculeName = "Drug";
         var moleculeProperties = new Container().WithName(Constants.MOLECULE_PROPERTIES);
         _dtoMoleculeParameter.ModelParentName = _moleculeName;
         moleculeProperties.Add(parameter);

         _dtoMoleculeParameter.Parent = organ;
         organ.Add(moleculeProperties);
         _refParameter = new Parameter().WithName("REF");
         _container.Add(_refParameter);
         A.CallTo(() => _context.Get<IParameter>(paramterId)).Returns(parameter);

      }
   }

   class When_asking__object_path_creator_for_an_absolute_Molecule_Parameter_Reference : concern_for_ObjectPathCreatorSpecs
    {
        private DummyParameterDTO _dtoMoleculeParameter;
        private ReferenceDTO _result;
        private IDimension _rightDimension;
        private string _parameterName;
        private string _moleculeName;
        private IParameter _refParameter;
        private IContainer _container;
      private IParameter _parameter;

      protected override void Context()
        {
            base.Context();
            var dimensionFactory = A.Fake<IMoBiDimensionFactory>();
            _rightDimension = A.Fake<IDimension>();
            A.CallTo(() => dimensionFactory.Dimension(Constants.Dimension.AMOUNT)).Returns(_rightDimension);
            A.CallTo(() => _context.DimensionFactory).Returns(dimensionFactory);
            _parameterName = "Para";
            var paramterId = _parameterName;
            _dtoMoleculeParameter = new DummyParameterDTO().WithId(_parameterName).WithName(_parameterName);
            var organ = new Container().WithName("Organ");
            _container = new Container().WithName("Compartment");
            organ.Add(_container);

            var parameterToUse = new ObjectBaseDTO().WithName(_parameterName).WithId(paramterId);

            _dtoMoleculeParameter.ParameterToUse = parameterToUse;

            _parameter = new Parameter().WithName(_parameterName).WithId(paramterId).WithDimension(_rightDimension); ;


            _moleculeName = "Drug";
            var moleculeProperties = new Container().WithName(Constants.MOLECULE_PROPERTIES);
            _dtoMoleculeParameter.ModelParentName = _moleculeName;
            moleculeProperties.Add(_parameter);

            _dtoMoleculeParameter.Parent = organ;
            organ.Add(moleculeProperties);
            _refParameter = new Parameter().WithName("REF");
            _container.Add(_refParameter);
            A.CallTo(() => _context.Get<IParameter>(paramterId)).Returns(_parameter);
        }

        protected override void Because()
        {
            _result = sut.CreatePathsFromEntity(_parameter, true, _refParameter, _refParameter);
        }

        [Observation]
        public void should_create_a_correct_reference()
        {
            _result.ShouldNotBeNull();
            _result.Path.Alias.ShouldBeEqualTo(_parameterName);
            _result.Path.Dimension.ShouldBeEqualTo(_rightDimension);
            _result.Path.ShouldOnlyContainInOrder(ObjectPath.PARENT_CONTAINER, ObjectPath.PARENT_CONTAINER,ObjectPathKeywords.MOLECULE, _parameterName);
        }

    }

  

   
    public abstract class concern_for_ObjectPathCreatorAtMoleculeParameterSpecs : ContextSpecification<IObjectPathCreator>
    {
        protected IMoBiContext _context;

        protected override void Context()
        {
            IAliasCreator aliasCreator = new AliasCreator();
            _context = A.Fake<IMoBiContext>();
            sut = new ObjectPathCreatorAtMoleculeParameter(new ObjectPathFactory(aliasCreator), aliasCreator, _context);
        }
    }

    public class When_asking_for_a_realtive_path_in_a_container_hierarchie_referenced_by_a_lokal_molecule_paramter : concern_for_ObjectPathCreatorAtMoleculeParameterSpecs
    {
        private ReferenceDTO _result;
        private IParameter _parameter;
        private IParameter _referenceParameter;
        private string _paraName = "Para";
        private string _containerNameOne = "One";
        private string _containerNameTwo = "TWO";
        private IDimension _rightDimension;
       private IContainer _containerTwo;

       protected override void Context()
        {
            base.Context();
            _rightDimension = A.Fake<IDimension>();
            _parameter = new Parameter().WithName(_paraName).WithDimension(_rightDimension);
            _referenceParameter = new Parameter().WithName("test");
            var rootContainer = new Container().WithName("Root");
            var containerOne = new Container().WithName(_containerNameOne);
            _containerTwo = new Container().WithName(_containerNameTwo);
            rootContainer.Add(containerOne);
            rootContainer.Add(_containerTwo);
            containerOne.Add(_parameter);
            new MoleculeBuilder().WithName("Drug").Add(_referenceParameter);
        }
        protected override void Because()
        {
           _result = sut.CreatePathsFromEntity(_parameter, false, _containerTwo, _referenceParameter);
        }

        [Observation]
        public void should_return_right_reference()
        {
            _result.ShouldNotBeNull();
            _result.Path.ShouldOnlyContainInOrder(ObjectPath.PARENT_CONTAINER, ObjectPath.PARENT_CONTAINER,ObjectPath.PARENT_CONTAINER, _containerNameOne, _paraName);
            _result.Path.Alias.ShouldBeEqualTo(_paraName);
            _result.Path.Dimension.ShouldBeEqualTo(_rightDimension);
            _result.BuildMode.ShouldBeEqualTo(ParameterBuildMode.Local);
        }
    }

    public class When_asking_for_a_realtive_path_to_an_other_locaslMoleculeParameter_referenced_by_a_lokal_molecule_paramter : concern_for_ObjectPathCreatorAtMoleculeParameterSpecs
    {
       private ReferenceDTO _result;
       private IParameter _parameter;
       private IParameter _referenceParameter;
       private string _paraName = "Para";
       private string _containerName = "TWO";
       private IDimension _rightDimension;
       private IContainer _referenceContainer;

       protected override void Context()
       {
          base.Context();
          _rightDimension = A.Fake<IDimension>();
          _parameter = new Parameter().WithName(_paraName).WithDimension(_rightDimension);
          _referenceParameter = new Parameter().WithName("test");
          var rootContainer = new Container().WithName("Root");
          _referenceContainer = new Container().WithName(_containerName);
          
          rootContainer.Add(_referenceContainer);
          
          var moleculebuilder = new MoleculeBuilder().WithName("Drug");
          moleculebuilder.Add(_referenceParameter);
          moleculebuilder.Add(_parameter);
          
          
       }
       protected override void Because()
       {
          _result = sut.CreatePathsFromEntity(_parameter, false, _referenceContainer, _referenceParameter);
       }

       [Observation]
       public void should_return_right_reference()
       {
          _result.ShouldNotBeNull();
          _result.Path.ShouldOnlyContainInOrder(ObjectPath.PARENT_CONTAINER, _paraName);
          _result.Path.Alias.ShouldBeEqualTo(_paraName);
          _result.Path.Dimension.ShouldBeEqualTo(_rightDimension);
          _result.BuildMode.ShouldBeEqualTo(ParameterBuildMode.Local);
       }
    }

    public class When_asking_for_a_realtive_path_to_Molecule_properties_parameter_for_the_same_molecule : concern_for_ObjectPathCreatorAtMoleculeParameterSpecs
    {
       private ReferenceDTO _result;
       private IParameter _parameter;
       private IParameter _referenceParameter;
       private string _paraName = "Para";
       private string _containerNameOne = "ONE";
       private string _containerNameTwo = "TWO";
       private IDimension _rightDimension;
       private IContainer _referenceContainer;
       private DummyParameterDTO _dtoDummy;

       protected override void Context()
       {
          base.Context();
          _rightDimension = A.Fake<IDimension>();
          _parameter = new Parameter().WithName(_paraName).WithDimension(_rightDimension).WithId("Para");
          _referenceParameter = new Parameter().WithName("test");
          var rootContainer = new Container().WithName("Root");
          var containerOne = new Container().WithName(_containerNameOne);
          _referenceContainer = new Container().WithName(_containerNameTwo);
          rootContainer.Add(containerOne);
          var moleculeProperties = new Container().WithName(Constants.MOLECULE_PROPERTIES);
          containerOne.Add(moleculeProperties);
          moleculeProperties.Add(_parameter);
          rootContainer.Add(_referenceContainer);

          var moleculebuilder = new MoleculeBuilder().WithName("Drug");
          moleculebuilder.Add(_referenceParameter);
          

          _dtoDummy = new DummyParameterDTO() {Parent = moleculeProperties, ModelParentName = "Drug",ParameterToUse = new ObjectBaseDTO(){Id="Para"},Id="Para"};

          A.CallTo(() => _context.Get<IParameter>("Para")).Returns(_parameter);

       }
       protected override void Because()
       {
          _result = sut.CreatePathFromParameterDummy(_dtoDummy, false, _referenceContainer, _referenceParameter);
       }

       [Observation]
       public void should_return_right_reference()
       {
          _result.ShouldNotBeNull();
          _result.Path.ShouldOnlyContainInOrder(ObjectPath.PARENT_CONTAINER, ObjectPath.PARENT_CONTAINER, ObjectPath.PARENT_CONTAINER,_containerNameOne, ObjectPathKeywords.MOLECULE, _paraName);
          _result.Path.Alias.ShouldBeEqualTo(_paraName);
          _result.Path.Dimension.ShouldBeEqualTo(_rightDimension);
          _result.BuildMode.ShouldBeEqualTo(ParameterBuildMode.Local);
       }
    }

    public class When_asking_for_a_realtive_path_to_Molecule_properties_parameter_for_an_other_molecule : concern_for_ObjectPathCreatorAtMoleculeParameterSpecs
    {
       private ReferenceDTO _result;
       private IParameter _parameter;
       private IParameter _referenceParameter;
       private string _paraName = "Para";
       private string _containerNameOne = "ONE";
       private string _containerNameTwo = "TWO";
       private IDimension _rightDimension;
       private IContainer _referenceContainer;
       private DummyParameterDTO _dtoDummy;

       protected override void Context()
       {
          base.Context();
          _rightDimension = A.Fake<IDimension>();
          _parameter = new Parameter().WithName(_paraName).WithDimension(_rightDimension).WithId("Para");
          _referenceParameter = new Parameter().WithName("test");
          var rootContainer = new Container().WithName("Root");
          var containerOne = new Container().WithName(_containerNameOne);
          _referenceContainer = new Container().WithName(_containerNameTwo);
          rootContainer.Add(containerOne);
          var moleculeProperties = new Container().WithName(Constants.MOLECULE_PROPERTIES);
          containerOne.Add(moleculeProperties);
          moleculeProperties.Add(_parameter);
          rootContainer.Add(_referenceContainer);

          var moleculebuilder = new MoleculeBuilder().WithName("Drug");
          moleculebuilder.Add(_referenceParameter);


          _dtoDummy = new DummyParameterDTO() { Parent = moleculeProperties, ModelParentName = "Other", ParameterToUse = new ObjectBaseDTO() { Id = "Para" }, Id = "Para" };

          A.CallTo(() => _context.Get<IParameter>("Para")).Returns(_parameter);

       }
       protected override void Because()
       {
          _result = sut.CreatePathFromParameterDummy(_dtoDummy, false, _referenceContainer, _referenceParameter);
       }

       [Observation]
       public void should_return_right_reference()
       {
          _result.ShouldNotBeNull();
          _result.Path.ShouldOnlyContainInOrder(ObjectPath.PARENT_CONTAINER, ObjectPath.PARENT_CONTAINER, ObjectPath.PARENT_CONTAINER, _containerNameOne, "Other", _paraName);
          _result.Path.Alias.ShouldBeEqualTo(_paraName);
          _result.Path.Dimension.ShouldBeEqualTo(_rightDimension);
          _result.BuildMode.ShouldBeEqualTo(ParameterBuildMode.Local);
       }
    }




    public abstract class concern_for_ObjectPathCreatorAtObserverSpecs : ContextSpecification<IObjectPathCreator>
    {
        protected IMoBiContext _context;

        protected override void Context()
        {
            IAliasCreator aliasCreator = new AliasCreator();
            _context = A.Fake<IMoBiContext>();
            sut = new ObjectPathCreatorAtAmountObserver(new ObjectPathFactory(aliasCreator), aliasCreator, _context);
        }
    }

    public abstract class concern_for_ObjectPathCreatorAtAmountObserverSpecs : ContextSpecification<IObjectPathCreator>
    {
       protected IMoBiContext _context;

       protected override void Context()
       {
          IAliasCreator aliasCreator = new AliasCreator();
          _context = A.Fake<IMoBiContext>();
          sut = new ObjectPathCreatorAtAmountObserver(new ObjectPathFactory(aliasCreator), aliasCreator, _context);
       }
    }

    public class When_asking_realtive_object_path_creator_for_a_Molecule_Parameter_Reference_used_in_a_observer : concern_for_ObjectPathCreatorAtObserverSpecs
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
            A.CallTo(() => dimensionFactory.Dimension(Constants.Dimension.AMOUNT)).Returns(_rightDimension);
            A.CallTo(() => _context.DimensionFactory).Returns(dimensionFactory);
            _parameterName = "Para";
            var paramterId = _parameterName;
            _dtoMoleculeParameter = new DummyParameterDTO().WithId(_parameterName).WithName(_parameterName);
            var organ = new Container().WithName("Organ");
            _container = new Container().WithName("Container");
            organ.Add(_container);

            var parameterToUse = new ObjectBaseDTO().WithName(_parameterName).WithId(paramterId);

            _dtoMoleculeParameter.ParameterToUse = parameterToUse;

            IParameter parameter = new Parameter().WithName(_parameterName).WithId(paramterId).WithDimension(_rightDimension); ;


            _moleculeName = "Drug";
            var moleculeProperties = new Container().WithName(Constants.MOLECULE_PROPERTIES);
            _dtoMoleculeParameter.ModelParentName = _moleculeName;
            moleculeProperties.Add(parameter);

            _dtoMoleculeParameter.Parent = moleculeProperties;
            organ.Add(moleculeProperties);
            A.CallTo(() => _context.Get<IParameter>(paramterId)).Returns(parameter);
        }

        protected override void Because()
        {
            _result = sut.CreatePathFromParameterDummy(_dtoMoleculeParameter, false, _container, A.Fake<IUsingFormula>());
        }

        [Observation]
        public void should_create_a_parent_reference()
        {
            _result.ShouldNotBeNull();
            _result.Path.Alias.ShouldBeEqualTo(_parameterName);
            _result.Path.Dimension.ShouldBeEqualTo(_rightDimension);
            _result.Path.ShouldOnlyContainInOrder(ObjectPath.PARENT_CONTAINER, ObjectPath.PARENT_CONTAINER,ObjectPath.PARENT_CONTAINER, ObjectPathKeywords.MOLECULE, _parameterName);
        }

    }

    public class When_asking_realtive_object_path_creator_for_a_Molecule_Reference_used_in_a_amount_observer : concern_for_ObjectPathCreatorAtAmountObserverSpecs
    {
       private DummyParameterDTO _dtoMoleculeParameter;
       private ReferenceDTO _result;
       private IDimension _rightDimension;
       private string _parameterName;
       private Container _container;

       protected override void Context()
       {
          base.Context();
          var dimensionFactory = A.Fake<IMoBiDimensionFactory>();
          _rightDimension = A.Fake<IDimension>();
          A.CallTo(() => dimensionFactory.Dimension(Constants.Dimension.AMOUNT)).Returns(_rightDimension);
          A.CallTo(() => _context.DimensionFactory).Returns(dimensionFactory);
          _parameterName = "Para";
          
          _dtoMoleculeParameter = new DummyParameterDTO().WithId(_parameterName).WithName(_parameterName);
          var organ = new Container().WithName("Organ");
          _container = new Container().WithName("Container");
          organ.Add(_container);
          _dtoMoleculeParameter.ParameterToUse = null;
          _dtoMoleculeParameter.Id = ObjectPathKeywords.MOLECULE;
       }

       protected override void Because()
       {
          _result = sut.CreatePathFromParameterDummy(_dtoMoleculeParameter, false, _container, A.Fake<IUsingFormula>());
       }

       [Observation]
       public void should_create_a_parent_reference()
       {
          _result.ShouldNotBeNull();
          _result.Path.Alias.ShouldBeEqualTo("M");
          _result.Path.Dimension.ShouldBeEqualTo(_rightDimension);
          _result.Path.ShouldOnlyContainInOrder(ObjectPath.PARENT_CONTAINER);
       }

    }

    public abstract class concern_for_ObjectPathCreatorMolecuelStartValueSpecs : ContextSpecification<IObjectPathCreator>
    {
        protected IMoBiContext _context;

        protected override void Context()
        {
            IAliasCreator aliasCreator = new AliasCreator();
            _context = A.Fake<IMoBiContext>();
            sut = new ObjectPathCreatorAtMoleculeStartValue(new ObjectPathFactory(aliasCreator), aliasCreator, _context);
        }
    }


    public class When_asking_for_objectpath_for_an_global_Molecule_parameter_used_in_A_molecule_StartValue : concern_for_ObjectPathCreatorMolecuelStartValueSpecs
    {
        private IParameter _parameter;
        private IDimension _rightDimension;
        private string _moleculeName;
        private ReferenceDTO _result;
        private string _parameterName;

        protected override void Context()
        {
            base.Context();
            _rightDimension = A.Fake<IDimension>();
            _parameterName = "Para";
            _parameter = new Parameter().WithName("Global").WithDimension(_rightDimension).WithName(_parameterName);
            _parameter.BuildMode = ParameterBuildMode.Global;
            _moleculeName = "Drug";
            var moleculeBuilder = new MoleculeBuilder().WithName(_moleculeName);
            moleculeBuilder.AddParameter(_parameter);
        }

        protected override void Because()
        {
            _result = sut.CreatePathsFromEntity(_parameter, true, A.Fake<IEntity>(), _parameter);
        }

        [Observation]
        public void should_return_right_reference()
        {
            _result.ShouldNotBeNull();
            _result.Path.ShouldOnlyContainInOrder(ObjectPathKeywords.MOLECULE, _parameterName);
            _result.Path.Alias.ShouldBeEqualTo(_parameterName);
            _result.Path.Dimension.ShouldBeEqualTo(_rightDimension);
            _result.BuildMode.ShouldBeEqualTo(ParameterBuildMode.Global);
        }
    }

    public class When_asking_for_realtive_objectpath_for_an_local_Molecule_parameter_used_in_A_molecule_StartValue : concern_for_ObjectPathCreatorMolecuelStartValueSpecs
    {
        private IParameter _parameter;
        private IDimension _rightDimension;
        private string _moleculeName;
        private ReferenceDTO _result;
        private string _parameterName;

        protected override void Context()
        {
            base.Context();
            _rightDimension = A.Fake<IDimension>();
            _parameterName = "Para";
            _parameter = new Parameter().WithName("Global").WithDimension(_rightDimension).WithName(_parameterName);
            _parameter.BuildMode = ParameterBuildMode.Local;
            _moleculeName = "Drug";
            var moleculeBuilder = new MoleculeBuilder().WithName(_moleculeName);
            moleculeBuilder.AddParameter(_parameter);
        }

        protected override void Because()
        {
            _result = sut.CreatePathsFromEntity(_parameter, false, A.Fake<IEntity>(), _parameter);
        }

        [Observation]
        public void should_return_right_reference()
        {
            _result.ShouldNotBeNull();
            _result.Path.ShouldOnlyContainInOrder( _parameterName);
            _result.Path.Alias.ShouldBeEqualTo(_parameterName);
            _result.Path.Dimension.ShouldBeEqualTo(_rightDimension);
            _result.BuildMode.ShouldBeEqualTo(ParameterBuildMode.Local);
        }
    }

    public class When_asking_for_absolute_objectpath_for_an_local_Molecule_parameter_used_in_A_molecule_StartValue : concern_for_ObjectPathCreatorMolecuelStartValueSpecs
    {
        private IParameter _parameter;
        private IDimension _rightDimension;
        private string _moleculeName;
        private ReferenceDTO _result;
        private string _parameterName;
        private IContainer _container;
        private string _containerName="Compartment";
        private string _organName="Organ";

        protected override void Context()
        {
            base.Context();
            _rightDimension = A.Fake<IDimension>();
            _parameterName = "Para";
            _parameter = new Parameter().WithName("Global").WithDimension(_rightDimension).WithName(_parameterName);
            _parameter.BuildMode = ParameterBuildMode.Local;
            _moleculeName = "Drug";
            var moleculeBuilder = new MoleculeBuilder().WithName(_moleculeName);
            moleculeBuilder.AddParameter(_parameter);
            _container = new Container().WithName(_containerName);
            var organ = new Container().WithName(_organName);
            organ.Add(_container);
        }

        protected override void Because()
        {
            _result = sut.CreatePathsFromEntity(_parameter, true, _container, _parameter);
        }

        [Observation]
        public void should_return_right_reference()
        {
            _result.ShouldNotBeNull();
            _result.Path.ShouldOnlyContainInOrder(_organName,_containerName,_moleculeName,_parameterName);
            _result.Path.Alias.ShouldBeEqualTo(_parameterName);
            _result.Path.Dimension.ShouldBeEqualTo(_rightDimension);
            _result.BuildMode.ShouldBeEqualTo(ParameterBuildMode.Local);
        }
    }

    public class When_asking_for_a_realtive_path_in_a_container_hierarchie_referenced_by_a_lokal_molecule_start_value : concern_for_ObjectPathCreatorMolecuelStartValueSpecs
    {
        private ReferenceDTO _result;
        private IParameter _parameter;
        private IParameter _referenceParameter;
        private string _paraName = "Para";
        private string _containerNameOne = "One";
        private string _containerNameTwo = "TWO";
        private IDimension _rightDimension;
        private IContainer _containerTwo;

        protected override void Context()
        {
            base.Context();
            _rightDimension = A.Fake<IDimension>();
            _parameter = new Parameter().WithName(_paraName).WithDimension(_rightDimension);
            _referenceParameter = new Parameter().WithName("test");
            var rootContainer = new Container().WithName("Root");
            var containerOne = new Container().WithName(_containerNameOne);
            _containerTwo = new Container().WithName(_containerNameTwo);
            rootContainer.Add(containerOne);
            rootContainer.Add(_containerTwo);
            containerOne.Add(_parameter);
            _containerTwo.Add(_referenceParameter);
        }
        protected override void Because()
        {
            _result = sut.CreatePathsFromEntity(_parameter, false, _containerTwo, _referenceParameter);
        }

        [Observation]
        public void should_return_right_reference()
        {
            _result.ShouldNotBeNull();
            _result.Path.ShouldOnlyContainInOrder(  ObjectPath.PARENT_CONTAINER, ObjectPath.PARENT_CONTAINER, _containerNameOne, _paraName);
            _result.Path.Alias.ShouldBeEqualTo(_paraName);
            _result.Path.Dimension.ShouldBeEqualTo(_rightDimension);
            _result.BuildMode.ShouldBeEqualTo(ParameterBuildMode.Local);
        }
    }

     public abstract class concern_for_ObjectPathCreatorAtMoleculeApllicationBuilderSpecs : ContextSpecification<IObjectPathCreator>
    {
        protected IMoBiContext _context;

        protected override void Context()
        {
            IAliasCreator aliasCreator = new AliasCreator();
            _context = A.Fake<IMoBiContext>();
            sut = new ObjectPathCreatorAtMoleculeApllicationBuilder(new ObjectPathFactory(aliasCreator), aliasCreator, _context);
        }
    }

    public class When_asking_for_a_realtive_path_in_a_container_hierarchie_referenced_by_a_local_molecule_application_builder : concern_for_ObjectPathCreatorAtMoleculeApllicationBuilderSpecs
    {
        private ReferenceDTO _result;
        private IParameter _parameter;
        private IParameter _referenceParameter;
        private string _paraName = "Para";
        private string _containerNameOne = "One";
        private string _containerNameTwo = "TWO";
        private IDimension _rightDimension;
        private IContainer _containerTwo;

        protected override void Context()
        {
            base.Context();
            _rightDimension = A.Fake<IDimension>();
            _parameter = new Parameter().WithName(_paraName).WithDimension(_rightDimension);
            _referenceParameter = new Parameter().WithName("test");
            var rootContainer = new Container().WithName("Root");
            var containerOne = new Container().WithName(_containerNameOne);
            _containerTwo = new Container().WithName(_containerNameTwo);
            rootContainer.Add(containerOne);
            rootContainer.Add(_containerTwo);
            containerOne.Add(_parameter);
            _containerTwo.Add(_referenceParameter);
        }
        protected override void Because()
        {
            _result = sut.CreatePathsFromEntity(_parameter, false, _containerTwo, _referenceParameter);
        }

        [Observation]
        public void should_return_right_reference()
        {
            _result.ShouldNotBeNull();
            _result.Path.ShouldOnlyContainInOrder(ObjectPath.PARENT_CONTAINER,ObjectPath.PARENT_CONTAINER, _containerNameOne, _paraName);
            _result.Path.Alias.ShouldBeEqualTo(_paraName);
            _result.Path.Dimension.ShouldBeEqualTo(_rightDimension);
            _result.BuildMode.ShouldBeEqualTo(ParameterBuildMode.Local);
        }
    }
}	