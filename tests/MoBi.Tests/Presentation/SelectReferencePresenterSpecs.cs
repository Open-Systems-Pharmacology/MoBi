using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

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
      private IObjectPathCreatorAtParameter _objectPathCreator;
      private IObjectBaseDTOToReferenceNodeMapper _referenceMapper;

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
         sut = new SelectReferenceAtParameterPresenter(_view, _objectBaseDTOMapper, _context, _userSettings,
            _moleculeMapper, _parameterMapper, _referenceMapper,
            _objectPathCreator);
      }
   }

   internal class When_getting_children_for_local_molecule_properties_in_a_physical_container :
      concern_for_SelectReferencePresenter
   {
      private ObjectBaseDTO _moleculePropertiesDTO;
      private IEnumerable<ObjectBaseDTO> _result;
      private readonly DummyParameterDTO _dtoP1 = new DummyParameterDTO().WithName("P1");
      private readonly DummyParameterDTO _dtoPlocal = new DummyParameterDTO().WithName("local");
      private readonly DummyParameterDTO _dtoPglobal = new DummyParameterDTO().WithName("global");

      protected override void Context()
      {
         base.Context();
         sut.SelectionPredicate = p => true;
         var id = "mp";

         var physical = new Container().WithName("PHYS").WithMode(ContainerMode.Physical);
         var moleculeProperties = new Container().WithName(Constants.MOLECULE_PROPERTIES).WithParentContainer(physical);
         var p1 = new Parameter().WithName("P1");
         moleculeProperties.Add(p1);
         var moleculeName = "Drug";
         _moleculePropertiesDTO =
            new DummyMoleculeContainerDTO() {MoleculePropertiesContainer = new ObjectBaseDTO().WithId(id)}.WithId("ANY")
               .WithName(moleculeName);
         A.CallTo(() => _context.Get<IContainer>(id)).Returns(moleculeProperties);
         var objectBaseRepository = A.Fake<IWithIdRepository>();
         A.CallTo(() => _context.ObjectRepository).Returns(objectBaseRepository);
         A.CallTo(() => objectBaseRepository.ContainsObjectWithId(id)).Returns(true);
         A.CallTo(() => _parameterMapper.MapFrom(p1, A<IContainer>._, A<ObjectBaseDTO>._)).Returns(_dtoP1);
         var project = A.Fake<IMoBiProject>();

         var moleculeBuildingBlock = new MoleculeBuildingBlock();
         var molecule = new MoleculeBuilder().WithName(moleculeName);
         var localP = new Parameter().WithName("local").WithMode(ParameterBuildMode.Local).WithParentContainer(molecule);
         var globalP =
            new Parameter().WithName("global").WithMode(ParameterBuildMode.Global).WithParentContainer(molecule);
         moleculeBuildingBlock.Add(molecule);
         A.CallTo(() => project.MoleculeBlockCollection).Returns(new[] {moleculeBuildingBlock});
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
      public void should_return_molecule_properties_paramerters()
      {
         _result.ShouldContain(_dtoP1);
      }

      [Observation]
      public void should_not_contain_gklobal_molecule_parameter()
      {
         _result.Contains(_dtoPglobal).ShouldBeFalse();
      }
   }

   internal class When_getting_children_for_local_molecule_properties_in_a_logical_container :
      concern_for_SelectReferencePresenter
   {
      private ObjectBaseDTO _moleculePropertiesDTO;
      private IEnumerable<ObjectBaseDTO> _result;
      private readonly DummyParameterDTO _dtoP1 = new DummyParameterDTO().WithName("P1");
      private readonly DummyParameterDTO _dtoPlocal = new DummyParameterDTO().WithName("local");
      private readonly DummyParameterDTO _dtoPglobal = new DummyParameterDTO().WithName("global");

      protected override void Context()
      {
         base.Context();
         sut.SelectionPredicate = p => true;
         var id = "mp";

         var logical = new Container().WithName("PHYS").WithMode(ContainerMode.Logical);
         var moleculeProperties = new Container().WithName(Constants.MOLECULE_PROPERTIES).WithParentContainer(logical);
         var p1 = new Parameter().WithName("P1");
         moleculeProperties.Add(p1);
         var moleculeName = "Drug";
         _moleculePropertiesDTO =
            new DummyMoleculeContainerDTO() {MoleculePropertiesContainer = new ObjectBaseDTO().WithId(id)}.WithId("ANY")
               .WithName(moleculeName);
         A.CallTo(() => _context.Get<IContainer>(id)).Returns(moleculeProperties);
         var objectBaseRepository = A.Fake<IWithIdRepository>();
         A.CallTo(() => _context.ObjectRepository).Returns(objectBaseRepository);
         A.CallTo(() => objectBaseRepository.ContainsObjectWithId(id)).Returns(true);
         A.CallTo(() => _parameterMapper.MapFrom(p1, A<IContainer>._, A<ObjectBaseDTO>._)).Returns(_dtoP1);
         var project = A.Fake<IMoBiProject>();

         var moleculeBuildingBlock = new MoleculeBuildingBlock();
         var molecule = new MoleculeBuilder().WithName(moleculeName);
         var localP = new Parameter().WithName("local").WithMode(ParameterBuildMode.Local).WithParentContainer(molecule);
         var globalP =
            new Parameter().WithName("global").WithMode(ParameterBuildMode.Global).WithParentContainer(molecule);
         moleculeBuildingBlock.Add(molecule);
         A.CallTo(() => project.MoleculeBlockCollection).Returns(new[] {moleculeBuildingBlock});
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
      public void should_return_molecule_properties_paramerters()
      {
         _result.ShouldContain(_dtoP1);
      }

      [Observation]
      public void should_not_contain_gklobal_molecule_parameter()
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
         A.CallTo(() => distributeParameter.Children).Returns(new[] {A.Fake<IParameter>().WithName("Mean")});
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
}