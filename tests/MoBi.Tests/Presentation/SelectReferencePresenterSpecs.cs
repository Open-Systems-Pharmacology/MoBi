using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
using MoBi.Core.Services;
using MoBi.Helpers;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Views;
using MoBi.UI.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.UI.Services;
using OSPSuite.Utility;
using System.Collections.Generic;
using System.Linq;

namespace MoBi.Presentation
{
   public abstract class concern_for_SelectReferencePresenter : ContextSpecification<ISelectReferencePresenter>
   {
      protected ISelectReferenceView _view;
      protected IMoBiContext _context;
      protected IObjectBaseToObjectBaseDTOMapper _objectBaseDTOMapper;
      protected IObjectBaseToDummyMoleculeDTOMapper _moleculeMapper;
      protected IParameterToDummyParameterDTOMapper _parameterMapper;
      protected IUserSettings _userSettings;
      protected IObjectPathCreatorAtParameter _objectPathCreator;
      protected IObjectBaseDTOToReferenceNodeMapper _referenceMapper;
      protected IBuildingBlockRepository _buildingBlockRepository;

      protected override void Context()
      {
         _view = A.Fake<ISelectReferenceView>();
         _context = A.Fake<IMoBiContext>();
         _objectBaseDTOMapper = A.Fake<IObjectBaseToObjectBaseDTOMapper>();
         _moleculeMapper = A.Fake<IObjectBaseToDummyMoleculeDTOMapper>();
         _parameterMapper = A.Fake<IParameterToDummyParameterDTOMapper>();
         _userSettings = A.Fake<IUserSettings>();
         _objectPathCreator = A.Fake<IObjectPathCreatorAtParameter>();
         _referenceMapper = A.Fake<ObjectBaseDTOToReferenceNodeMapper>();
         _buildingBlockRepository = new BuildingBlockRepository(new MoBiProjectRetriever(_context));

         sut = new SelectReferenceAtParameterPresenter(_view, _objectBaseDTOMapper, _context, _userSettings,
            _moleculeMapper, _parameterMapper, _referenceMapper,
            _objectPathCreator, _buildingBlockRepository);
      }
   }

   internal class When_getting_children_for_local_molecule_properties_in_a_physical_container :
      concern_for_SelectReferencePresenter
   {
      private ObjectBaseDTO _moleculePropertiesDTO;
      private IEnumerable<ObjectBaseDTO> _result;
      private DummyParameterDTO _dtoP1;
      private DummyParameterDTO _dtoPlocal;
      private DummyParameterDTO _dtoPglobal;

      protected override void Context()
      {
         base.Context();
         sut.SelectionPredicate = p => true;
         var id = "mp";

         var physical = new Container().WithName("PHYS").WithMode(ContainerMode.Physical);
         var moleculeProperties = new Container().WithName(Constants.MOLECULE_PROPERTIES).WithParentContainer(physical);
         var p1 = new Parameter().WithName("P1");
         _dtoP1 = new DummyParameterDTO(p1).WithName("P1");

         moleculeProperties.Add(p1);
         var moleculeName = "Drug";
         _moleculePropertiesDTO = new DummyMoleculeContainerDTO(new MoleculeAmount())
         {
            MoleculePropertiesContainer = new ObjectBaseDTO().WithId(id)
         }.WithId("ANY").WithName(moleculeName);

         A.CallTo(() => _context.Get<IContainer>(id)).Returns(moleculeProperties);
         var objectBaseRepository = A.Fake<IWithIdRepository>();
         A.CallTo(() => _context.ObjectRepository).Returns(objectBaseRepository);
         A.CallTo(() => objectBaseRepository.ContainsObjectWithId(id)).Returns(true);
         A.CallTo(() => _parameterMapper.MapFrom(p1, A<IContainer>._, A<ObjectBaseDTO>._)).Returns(_dtoP1);
         var project = DomainHelperForSpecs.NewProject();

         var moleculeBuildingBlock = new MoleculeBuildingBlock();
         var molecule = new MoleculeBuilder().WithName(moleculeName);
         var localP = new Parameter().WithName("local").WithMode(ParameterBuildMode.Local).WithParentContainer(molecule);
         _dtoPlocal = new DummyParameterDTO(localP).WithName("local");

         var globalP = new Parameter().WithName("global").WithMode(ParameterBuildMode.Global).WithParentContainer(molecule);
         _dtoPglobal = new DummyParameterDTO(localP).WithName("global");

         moleculeBuildingBlock.Add(molecule);

         project.AddModule(new Module { moleculeBuildingBlock });

         A.CallTo(() => _parameterMapper.MapFrom(localP, A<IContainer>._, A<ObjectBaseDTO>._)).Returns(_dtoPlocal);
         A.CallTo(() => _parameterMapper.MapFrom(globalP, A<IContainer>._, A<ObjectBaseDTO>._)).Returns(_dtoPglobal);
         A.CallTo(() => _context.CurrentProject).Returns(project);
      }

      protected override void Because()
      {
         _result = sut.GetChildObjects(_moleculePropertiesDTO);
      }

      [Observation]
      public void should_return_also_local_molecule_properties()
      {
         _result.ShouldContain(_dtoPlocal);
      }

      [Observation]
      public void should_return_molecule_properties_parameters()
      {
         _result.ShouldContain(_dtoP1);
      }

      [Observation]
      public void should_not_contain_global_molecule_parameter()
      {
         _result.Contains(_dtoPglobal).ShouldBeFalse();
      }
   }

   internal class When_getting_children_for_local_molecule_properties_in_a_logical_container :
      concern_for_SelectReferencePresenter
   {
      private ObjectBaseDTO _moleculePropertiesDTO;
      private IEnumerable<ObjectBaseDTO> _result;
      private DummyParameterDTO _dtoP1;
      private DummyParameterDTO _dtoPlocal;
      private DummyParameterDTO _dtoPglobal;

      protected override void Context()
      {
         base.Context();
         sut.SelectionPredicate = p => true;
         var id = "mp";

         var logical = new Container().WithName("PHYS").WithMode(ContainerMode.Logical);
         var moleculeProperties = new Container().WithName(Constants.MOLECULE_PROPERTIES).WithParentContainer(logical);
         var p1 = new Parameter().WithName("P1");
         _dtoP1 = new DummyParameterDTO(p1).WithName("P1");

         moleculeProperties.Add(p1);
         var moleculeName = "Drug";
         _moleculePropertiesDTO =
            new DummyMoleculeContainerDTO(new MoleculeAmount { Name = moleculeName })
            {
               MoleculePropertiesContainer = new ObjectBaseDTO().WithId(id)
            }.WithId("ANY")
               .WithName(moleculeName);

         A.CallTo(() => _context.Get<IContainer>(id)).Returns(moleculeProperties);
         var objectBaseRepository = A.Fake<IWithIdRepository>();
         A.CallTo(() => _context.ObjectRepository).Returns(objectBaseRepository);
         A.CallTo(() => objectBaseRepository.ContainsObjectWithId(id)).Returns(true);
         A.CallTo(() => _parameterMapper.MapFrom(p1, A<IContainer>._, A<ObjectBaseDTO>._)).Returns(_dtoP1);
         var project = DomainHelperForSpecs.NewProject();

         var moleculeBuildingBlock = new MoleculeBuildingBlock();
         var molecule = new MoleculeBuilder().WithName(moleculeName);
         var localP = new Parameter().WithName("local").WithMode(ParameterBuildMode.Local).WithParentContainer(molecule);

         _dtoPlocal = new DummyParameterDTO(localP).WithName("local");


         var globalP = new Parameter().WithName("global").WithMode(ParameterBuildMode.Global).WithParentContainer(molecule);
         _dtoPglobal = new DummyParameterDTO(localP).WithName("global");

         moleculeBuildingBlock.Add(molecule);

         A.CallTo(() => _parameterMapper.MapFrom(localP, A<IContainer>._, A<ObjectBaseDTO>._)).Returns(_dtoPlocal);
         A.CallTo(() => _parameterMapper.MapFrom(globalP, A<IContainer>._, A<ObjectBaseDTO>._)).Returns(_dtoPglobal);
         A.CallTo(() => _context.CurrentProject).Returns(project);
      }

      protected override void Because()
      {
         _result = sut.GetChildObjects(_moleculePropertiesDTO);
      }

      [Observation]
      public void should_return_also_local_molecule_properties()
      {
         _result.Contains(_dtoPlocal).ShouldBeFalse();
      }

      [Observation]
      public void should_return_molecule_properties_parameters()
      {
         _result.ShouldContain(_dtoP1);
      }

      [Observation]
      public void should_not_contain_global_molecule_parameter()
      {
         _result.Contains(_dtoPglobal).ShouldBeFalse();
      }
   }

   internal class When_getting_child_objects_for_an_global_molecule_properties_container :
      concern_for_SelectReferencePresenter
   {
      private ObjectBaseDTO _dtoDistributedParameter;
      private IEnumerable<ObjectBaseDTO> _result;

      protected override void Context()
      {
         base.Context();
         var id = "DIST";
         _dtoDistributedParameter = A.Fake<ObjectBaseDTO>().WithId(id);
         var distributeParameter = A.Fake<IDistributedParameter>();
         A.CallTo(() => distributeParameter.Children).Returns(new[] { A.Fake<IParameter>().WithName("Mean") });
         A.CallTo(() => _context.Get<IObjectBase>(id)).Returns(distributeParameter);
         var objectBaseRepository = A.Fake<IWithIdRepository>();
         A.CallTo(() => _context.ObjectRepository).Returns(objectBaseRepository);
         A.CallTo(() => objectBaseRepository.ContainsObjectWithId(id)).Returns(true);
      }

      protected override void Because()
      {
         _result = sut.GetChildObjects(_dtoDistributedParameter);
      }

      [Observation]
      public void should_return_an_empty_list()
      {
         _result.ShouldBeEmpty();
      }
   }

   public abstract class concern_for_SelectReferencePresenter_with_node_text : concern_for_SelectReferencePresenter
   {
      protected override void Context()
      {
         base.Context();
         _view = new SelectReferenceView(A.Fake<IImageListRetriever>());
         _objectBaseDTOMapper = new ObjectBaseToObjectBaseDTOMapper();
         _referenceMapper = new ObjectBaseDTOToReferenceNodeMapper(_objectBaseDTOMapper);
         _buildingBlockRepository = A.Fake<IBuildingBlockRepository>();
         sut = new SelectReferenceAtParameterPresenter(_view, _objectBaseDTOMapper, _context, _userSettings,
            _moleculeMapper, _parameterMapper, _referenceMapper,
            _objectPathCreator, _buildingBlockRepository);
      }
   }

   internal class When_getting_child_objects_for_an_global_molecule_properties_container_check_trees_text :
      concern_for_SelectReferencePresenter_with_node_text
   {
      private MoBiSpatialStructure _moBiSpatialStructure;
      private MoBiSpatialStructure _unselectedSpatialStructure;
      private MoBiSpatialStructure[] _moBiSpatialStructures;

      protected override void Context()
      {

         base.Context();
         _moBiSpatialStructure = new MoBiSpatialStructure
         {
            Name = "MoBiSpatialStructure 1",
            Id = ShortGuid.NewGuid(),
            Module = new Module { Name = "Module 1" }
         };
         _unselectedSpatialStructure = new MoBiSpatialStructure
         {
            Name = "MoBiSpatialStructure 2",
            Id = ShortGuid.NewGuid(),
            Module = new Module { Name = "Module 2" }
         };
         _moBiSpatialStructures = new[] { _unselectedSpatialStructure, _moBiSpatialStructure };
         A.CallTo(() => _buildingBlockRepository.SpatialStructureCollection).Returns(_moBiSpatialStructures);
      }

      protected override void Because()
      {
         sut.Init(null, new List<IObjectBase> { _moBiSpatialStructure }, A.Fake<IUsingFormula>());
      }

      [Observation]
      public void do_not_add_nodes_when_already_present()
      {
         var moBiSpatialStructureNodes = _view.GetNodes(_moBiSpatialStructure);
         moBiSpatialStructureNodes.Count.ShouldBeEqualTo(1);
      }

      [Observation]
      public void should_return_nodes_with_specific_properties()
      {
         var moBiSpatialStructureNodes = _view.GetNodes(_moBiSpatialStructure);
         var unselectedSpatialStructureNodes = _view.GetNodes(_unselectedSpatialStructure);

         var moBiSpatialStructureNode = moBiSpatialStructureNodes.FirstOrDefault();
         var unselectedSpatialStructureNode = unselectedSpatialStructureNodes.FirstOrDefault();

         moBiSpatialStructureNode.ShouldNotBeNull();
         unselectedSpatialStructureNode.ShouldNotBeNull();
         moBiSpatialStructureNode.Text.ShouldBeEqualTo(_moBiSpatialStructure.DisplayName);
         unselectedSpatialStructureNode.Text.ShouldBeEqualTo(_unselectedSpatialStructure.DisplayName);
      }
   }
}