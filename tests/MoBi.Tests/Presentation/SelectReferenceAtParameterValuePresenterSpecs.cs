using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using FakeItEasy.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Presentation.Nodes;

namespace MoBi.Presentation
{
   public abstract class concern_for_SelectReferenceAtParameterValuePresenter : ContextSpecification<SelectReferenceAtParameterValuePresenter>
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
      protected IObjectPathFactory _objectPathFactory;

      protected override void Context()
      {
         _view = A.Fake<ISelectReferenceView>();
         _objectPathFactory = A.Fake<IObjectPathFactory>();
         _context = A.Fake<IMoBiContext>();
         _objectBaseDTOMapper = A.Fake<IObjectBaseToObjectBaseDTOMapper>();
         _moleculeMapper = A.Fake<IObjectBaseToDummyMoleculeDTOMapper>();
         _parameterMapper = A.Fake<IParameterToDummyParameterDTOMapper>();
         _userSettings = A.Fake<IUserSettings>();
         _objectPathCreator = A.Fake<IObjectPathCreatorAtParameter>();
         _referenceMapper = A.Fake<IObjectBaseDTOToReferenceNodeMapper>();
         _buildingBlockRepository = A.Fake<IBuildingBlockRepository>();
         A.CallTo(() => _context.ObjectPathFactory).Returns(_objectPathFactory);
         A.CallTo(() => _view.Shows(A<IEntity>.Ignored)).Returns(true);
         sut = new SelectReferenceAtParameterValuePresenter(_view, _objectBaseDTOMapper, _context, _userSettings,
            _moleculeMapper, _parameterMapper, _referenceMapper, _objectPathCreator, _buildingBlockRepository);
      }
   }

   internal class When_selecting_multiple_references : concern_for_SelectReferenceAtParameterValuePresenter
   {
      private ObjectBaseDTO _parameterDTO2;
      private ObjectBaseDTO _parameterDTO1;

      protected override void Context()
      {
         base.Context();
         _parameterDTO1 = new ObjectBaseDTO(new Parameter().WithId("1"));
         _parameterDTO2 = new ObjectBaseDTO(new Parameter().WithId("2"));
         A.CallTo(() => _context.Get<IObjectBase>(A<string>._)).ReturnsLazily(objectBaseForId);
         A.CallTo(() => _view.AllSelectedDTOs).Returns(new[] { _parameterDTO1, _parameterDTO2 });
      }

      private IObjectBase objectBaseForId(IFakeObjectCall x)
      {
         var id = x.Arguments.Get<string>(0);
         if (id == _parameterDTO1.Id)
            return _parameterDTO1.ObjectBase;

         if (id == _parameterDTO2.Id)
            return _parameterDTO2.ObjectBase;

         return null;
      }

      [Observation]
      public void the_presenter_can_close()
      {
         sut.CanClose.ShouldBeTrue();
      }
   }

   internal class When_selecting_molecule_properties_with_global_params : concern_for_SelectReferenceAtParameterValuePresenter
   {
      private ObjectBaseDTO _parameter1;
      private ObjectBaseDTO _parameter2;
      private DummyParameterDTO _dtoMoleculeParameter;
      private readonly string _moleculePropertiesId = Constants.MOLECULE_PROPERTIES;
      private IReadOnlyList<ObjectPath> _selectedSections;
      private int _counter;
      private readonly string _moleculeParameterId = "MoleculeParameterId";

      private string _modelParentName;

      protected override void Context()
      {
         base.Context();
         _modelParentName = "modelParentName";
         var moleculeProperties = new Container().WithName(Constants.MOLECULE_PROPERTIES).WithId(_moleculePropertiesId);
         var parameter = new Parameter().WithName("parameter").WithId(_moleculeParameterId);
         _parameter1 = new ObjectBaseDTO(new Parameter().WithId("1").WithParentContainer(moleculeProperties));
         _parameter2 = new ObjectBaseDTO(new Parameter().WithId("2"));
         _dtoMoleculeParameter = new DummyParameterDTO(parameter).WithName("parameter");
         parameter.BuildMode = ParameterBuildMode.Global;
         _dtoMoleculeParameter.Id = _moleculeParameterId;
         _dtoMoleculeParameter.ModelParentName = _modelParentName;
         A.CallTo(() => _context.Get<IEntity>(A<string>._)).ReturnsLazily(objectBaseForId);
         A.CallTo(() => _view.AllSelectedDTOs).Returns(new[] { _parameter1, _parameter2, _dtoMoleculeParameter });
         int callCount = 0;
         A.CallTo(() => _objectPathFactory.CreateAbsoluteObjectPath(A<IEntity>.Ignored))
            .ReturnsLazily(() =>
            {
               callCount++;
               switch (callCount)
               {
                  case 1:
                     return new ObjectPath($"{_modelParentName}|Path1|Item1");
                  case 2:
                     return new ObjectPath($"{_modelParentName}|Path1|Item2");
                  case 3:
                     return new ObjectPath($"{Constants.MOLECULE_PROPERTIES}|Path1|Item3");
                  default:
                     return null;
               }
            });
      }

      private IEntity objectBaseForId(IFakeObjectCall x)
      {
         var id = x.Arguments.Get<string>(0);
         if (id == _moleculeParameterId)
         {
            return _dtoMoleculeParameter.ObjectBase as IEntity;
         }
         else
         {
            _counter++;
            return _counter % 2 == 0 ? _parameter1.ObjectBase as IEntity : _parameter2.ObjectBase as IEntity;
         }
      }

      protected override void Because()
      {
         _selectedSections = sut.GetAllSelections();
      }

      [Observation]
      public void the_selected_should_not_contain_molecule_properties()
      {
         _selectedSections.Any(x => x.PathAsString.Contains(Constants.MOLECULE_PROPERTIES)).ShouldBeFalse();
      }

      [Observation]
      public void the_selected_should_contain_model_parent_name_for_global_build_mode()
      {
         _selectedSections.Count(x => x.PathAsString.Contains(_modelParentName)).ShouldBeEqualTo(3);
      }
   }

   internal class When_selecting_molecule_properties_with_local_params : concern_for_SelectReferenceAtParameterValuePresenter
   {
      private DummyParameterDTO _dtoGlobalMoleculeParameter;
      private DummyParameterDTO _dtoLocalMoleculeParameter;
      private IReadOnlyList<ObjectPath> _selectedSections;
      private readonly string _globalMoleculeParameterId = "GlobalMoleculeParameterId";
      private readonly string _localMoleculeParameterId = "LocalMoleculeParameterId";
      private readonly string _modelParentName = "modelParentName";
      private readonly string _parentContainer = "ParentContainer";

      protected override void Context()
      {
         base.Context();
         var globalParameter = new Parameter().WithName("parameter").WithId(_globalMoleculeParameterId);
         globalParameter.BuildMode = ParameterBuildMode.Global;
         var localParameter = new Parameter().WithName("parameter").WithId(_localMoleculeParameterId);
         localParameter.BuildMode = ParameterBuildMode.Local;

         _dtoGlobalMoleculeParameter = new DummyParameterDTO(globalParameter).WithName("globalParameter");
         _dtoGlobalMoleculeParameter.Id = _globalMoleculeParameterId;
         _dtoGlobalMoleculeParameter.ModelParentName = _modelParentName;

         _dtoLocalMoleculeParameter = new DummyParameterDTO(localParameter).WithName("localParameter");
         _dtoLocalMoleculeParameter.Id = _localMoleculeParameterId;
         _dtoLocalMoleculeParameter.Parent = new Container().WithName(_parentContainer).WithId("Mol1");

         A.CallTo(() => _context.Get<IEntity>(A<string>._)).ReturnsLazily(objectBaseForId);
         A.CallTo(() => _view.AllSelectedDTOs).Returns(new[] { _dtoLocalMoleculeParameter, _dtoGlobalMoleculeParameter });
         int callCount = 0;
         A.CallTo(() => _objectPathFactory.CreateAbsoluteObjectPath(A<IEntity>.Ignored))
            .ReturnsLazily(() =>
            {
               callCount++;
               switch (callCount)
               {
                  case 1:
                     return new ObjectPath("Mol1|Path1|Item1");
                  case 2:
                     return new ObjectPath($"{Constants.MOLECULE_PROPERTIES}|Path1|Item3");
                  default:
                     return null;
               }
            });
      }

      private IEntity objectBaseForId(IFakeObjectCall x)
      {
         var id = x.Arguments.Get<string>(0);
         if (id == _globalMoleculeParameterId)
         {
            return _dtoGlobalMoleculeParameter.ObjectBase as IEntity;
         }

         if (id == _localMoleculeParameterId)
         {
            return _dtoLocalMoleculeParameter.ObjectBase as IEntity;
         }

         return null;
      }

      protected override void Because()
      {
         _selectedSections = sut.GetAllSelections();
      }

      [Observation]
      public void the_selected_should_not_contain_molecule_properties()
      {
         _selectedSections.Any(x => x.PathAsString.Contains(Constants.MOLECULE_PROPERTIES)).ShouldBeFalse();
      }

      [Observation]
      public void the_selected_global_parameters_should_have_correct_path()
      {
         _selectedSections.Count(x => x.PathAsString.Contains(_parentContainer)).ShouldBeEqualTo(1);
      }

      [Observation]
      public void the_selected_local_parameters_should_have_correct_path()
      {
         _selectedSections.Count(x => x.PathAsString.Contains(_modelParentName)).ShouldBeEqualTo(1);
      }
   }

   internal class When_selecting_multiple_references_and_not_all_are_parameters : concern_for_SelectReferenceAtParameterValuePresenter
   {
      private ObjectBaseDTO _parameterDTO2;
      private ObjectBaseDTO _reactionDTO1;

      protected override void Context()
      {
         base.Context();
         _reactionDTO1 = new ObjectBaseDTO(new Reaction().WithId("1"));
         _parameterDTO2 = new ObjectBaseDTO(new Parameter().WithId("2"));
         A.CallTo(() => _context.Get<IObjectBase>(A<string>._)).ReturnsLazily(objectBaseForId);
         A.CallTo(() => _view.AllSelectedDTOs).Returns(new[] { _reactionDTO1, _parameterDTO2 });
      }

      private IObjectBase objectBaseForId(IFakeObjectCall x)
      {
         var id = x.Arguments.Get<string>(0);
         if (id == _reactionDTO1.Id)
            return _reactionDTO1.ObjectBase;

         if (id == _parameterDTO2.Id)
            return _parameterDTO2.ObjectBase;

         return null;
      }

      [Observation]
      public void the_presenter_can_close()
      {
         sut.CanClose.ShouldBeFalse();
      }
   }

   internal class When_initializing_parameterValuePresenter : concern_for_SelectReferenceAtParameterValuePresenter
   {
      private IEntity _localReferencePoint;
      private IReadOnlyList<IObjectBase> _contextSpecificEntities;
      private IUsingFormula _editedObject;

      protected override void Context()
      {
         base.Context();
         _localReferencePoint = A.Fake<IEntity>();
         _contextSpecificEntities = A.CollectionOfFake<IObjectBase>(5).ToList();
         _editedObject = A.Fake<IUsingFormula>();
      }

      protected override void Because()
      {
         sut.Init(_localReferencePoint, _contextSpecificEntities, _editedObject);
      }

      [Observation]
      public void should_add_spatial_structures()
      {
         // Verify that AddSpatialStructures method is called indirectly via Init
         A.CallTo(() => _view.AddNodes(A<IEnumerable<ITreeNode>>._)).MustHaveHappened();
      }

      [Observation]
      public void should_not_allow_localisation_change()
      {
         _view.ChangeLocalisationAllowed.ShouldBeFalse();
      }
   }

   public class When_getting_child_objects_for_a_path_and_value_building_block : concern_for_SelectReferenceAtParameterValuePresenter
   {
      private ObjectBaseDTO _objectBaseDTO;
      private List<ObjectBaseDTO> _children;
      private IndividualBuildingBlock _individualBuildingBlock;

      protected override void Context()
      {
         base.Context();
         _individualBuildingBlock = new IndividualBuildingBlock().WithId("1");
         var pathAndValueEntity = new IndividualParameter().WithId("2");
         pathAndValueEntity.Path = new ObjectPath("Root", "Container", "named parameter");
         _individualBuildingBlock.Add(pathAndValueEntity);

         _objectBaseDTO = new ObjectBaseDTO(_individualBuildingBlock);

         A.CallTo(() => _context.ObjectRepository.ContainsObjectWithId(_individualBuildingBlock.Id)).Returns(true);
         A.CallTo(() => _context.Get<IndividualBuildingBlock>(_individualBuildingBlock.Id)).Returns(_individualBuildingBlock);
         A.CallTo(() => _context.Get<ExpressionProfileBuildingBlock>(A<string>._)).Returns(null);
         A.CallTo(() => _objectBaseDTOMapper.MapFrom(A<IObjectBase>._)).ReturnsLazily(x => new ObjectBaseDTO(x.Arguments.Get<IObjectBase>(0)));
      }

      protected override void Because()
      {
         _children = sut.GetChildObjects(_objectBaseDTO).ToList();
      }

      [Observation]
      public void the_child_objects_added_to_the_view_should_contain_the_individual_parameters()
      {
         _children.Count.ShouldBeEqualTo(1);
         _children[0].ObjectBase.Name.ShouldBeEqualTo("Root");
         var container = _children[0].ObjectBase as Container;
         container.Single().Name.ShouldBeEqualTo("Container");
         (container.Single() as Container).Single().Name.ShouldBeEqualTo("named parameter");
      }
   }

   public class When_getting_child_objects_for_a_path : concern_for_SelectReferenceAtParameterValuePresenter
   {
      private ObjectBaseDTO _objectBaseDTO;
      private IndividualBuildingBlock _individualBuildingBlock;
      private ObjectBaseDTO _pathChild;
      private List<ObjectBaseDTO> _children;

      protected override void Context()
      {
         base.Context();
         _individualBuildingBlock = new IndividualBuildingBlock().WithId("1");
         var pathAndValueEntity = new IndividualParameter().WithId("2");
         pathAndValueEntity.Path = new ObjectPath("Root", "Container", "named parameter");
         _individualBuildingBlock.Add(pathAndValueEntity);

         _objectBaseDTO = new ObjectBaseDTO(_individualBuildingBlock);

         A.CallTo(() => _context.ObjectRepository.ContainsObjectWithId(_individualBuildingBlock.Id)).Returns(true);
         A.CallTo(() => _context.Get<IndividualBuildingBlock>(_individualBuildingBlock.Id)).Returns(_individualBuildingBlock);
         A.CallTo(() => _context.Get<ExpressionProfileBuildingBlock>(A<string>._)).Returns(null);
         A.CallTo(() => _objectBaseDTOMapper.MapFrom(A<IObjectBase>._)).ReturnsLazily(x => new ObjectBaseDTO(x.Arguments.Get<IObjectBase>(0)));

         _pathChild = sut.GetChildObjects(_objectBaseDTO).ToList().Single();
      }

      protected override void Because()
      {
         _children = sut.GetChildObjects(_pathChild).ToList();
      }

      [Observation]
      public void the_children_resolved_should_reflect_the_path_structure_of_the_container()
      {
         _children[0].ObjectBase.Name.ShouldBeEqualTo("Container");
         var entity = (_children[0].ObjectBase as Container).Single();
         entity.Name.ShouldBeEqualTo("named parameter");
      }
   }

   public class When_getting_child_objects_with_a_distributed_parameter : concern_for_SelectReferenceAtParameterValuePresenter
   {
      private ObjectBaseDTO _objectBaseDTO;
      private List<ObjectBaseDTO> _children;
      private IndividualBuildingBlock _individualBuildingBlock;

      protected override void Context()
      {
         base.Context();
         _individualBuildingBlock = new IndividualBuildingBlock().WithId("1");
         var pathAndValueEntity = new IndividualParameter().WithId("2");
         pathAndValueEntity.Path = new ObjectPath("Root", "Container", "distributed parameter");

         pathAndValueEntity.DistributionType = DistributionType.Normal;
         _individualBuildingBlock.Add(pathAndValueEntity);

         pathAndValueEntity = new IndividualParameter().WithId("3");
         pathAndValueEntity.Path = new ObjectPath("Root", "Container", "distributed parameter", "mean");
         _individualBuildingBlock.Add(pathAndValueEntity);

         _objectBaseDTO = new ObjectBaseDTO(_individualBuildingBlock);

         A.CallTo(() => _context.ObjectRepository.ContainsObjectWithId(_individualBuildingBlock.Id)).Returns(true);
         A.CallTo(() => _context.Get<IndividualBuildingBlock>(_individualBuildingBlock.Id)).Returns(_individualBuildingBlock);
         A.CallTo(() => _context.Get<ExpressionProfileBuildingBlock>(A<string>._)).Returns(null);
         A.CallTo(() => _objectBaseDTOMapper.MapFrom(A<IObjectBase>._)).ReturnsLazily(x => new ObjectBaseDTO(x.Arguments.Get<IObjectBase>(0)));
      }

      protected override void Because()
      {
         _children = sut.GetChildObjects(_objectBaseDTO).ToList();
      }

      [Observation]
      public void the_child_objects_added_to_the_view_should_contain_the_individual_parameters()
      {
         _children.Count.ShouldBeEqualTo(1);
         _children[0].ObjectBase.Name.ShouldBeEqualTo("Root");
         var root = _children[0].ObjectBase as Container;
         var container = root.Single() as Container;
         container.Name.ShouldBeEqualTo("Container");
         var distributedParameterContainer = container.Single() as IndividualParameter;
         distributedParameterContainer.Name.ShouldBeEqualTo("distributed parameter");
      }
   }
}