using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation
{
   public abstract class concern_for_SelectEventAssingmentTargetPresenter : ContextSpecification<ISelectEventAssignmentTargetPresenter>
   {
      protected ISelectObjectPathView _view;
      protected IMoBiContext _context;
      protected IObjectBaseToObjectBaseDTOMapper _objectBaseDTOMapper;
      protected IContainerToContainerDTOMapper _containerDTOMapper;
      protected IReactionBuilderToDummyReactionDTOMapper _reactionMapper;
      protected IMoleculeBuilderToDummyMoleculeDTOMapper _moleculeMapper;
      protected IObjectPathFactory _objectPathFactory;
      protected IParameterToDummyParameterDTOMapper _parameterMapper;
      protected IReactionDimensionRetriever _dimensionRetriever;
      private IMoBiProject _mobiProject;
      private Container _rootContainer;
      private IMoBiReactionBuildingBlock _reactionBB;
      private IMoleculeBuildingBlock _moleculeBB;
      protected IReactionBuilder _reaction;
      protected IMoleculeBuilder _moleculeBuilder;
      private IParameter _localParameter;
      private IParameter _globalParameter;

      protected override void Context()
      {
         _view = A.Fake<ISelectObjectPathView>();
         _context = A.Fake<IMoBiContext>();
         _objectBaseDTOMapper = A.Fake<IObjectBaseToObjectBaseDTOMapper>();
         _containerDTOMapper = A.Fake<IContainerToContainerDTOMapper>();
         _reactionMapper = A.Fake<IReactionBuilderToDummyReactionDTOMapper>();
         _moleculeMapper = A.Fake<IMoleculeBuilderToDummyMoleculeDTOMapper>();
         _objectPathFactory = A.Fake<IObjectPathFactory>();
         _parameterMapper = A.Fake<IParameterToDummyParameterDTOMapper>();
         _dimensionRetriever = A.Fake<IReactionDimensionRetriever>();
         sut = new SelectEventAssignmentTargetPresenter(_view, _context, _objectBaseDTOMapper, _containerDTOMapper, _reactionMapper,
            _moleculeMapper, _objectPathFactory, _parameterMapper, _dimensionRetriever);

         _mobiProject= A.Fake<IMoBiProject>();
         A.CallTo(() => _context.CurrentProject).Returns(_mobiProject);
         _rootContainer=new Container();
         _moleculeBuilder = new MoleculeBuilder().WithName("M");
         _reaction = new ReactionBuilder().WithName("R");
         _localParameter = new Parameter().WithMode(ParameterBuildMode.Local).WithName("LocalParam");
         _globalParameter = new Parameter().WithMode(ParameterBuildMode.Global).WithName("GlobalParam");
         _reaction.Add(_localParameter);
         _reaction.Add(_globalParameter);
         _reactionBB = new MoBiReactionBuildingBlock() {_reaction};
         _moleculeBB = new MoleculeBuildingBlock {_moleculeBuilder};
         A.CallTo(() => _mobiProject.ReactionBlockCollection).Returns(new [] {_reactionBB});
         A.CallTo(() => _mobiProject.MoleculeBlockCollection).Returns(new [] {_moleculeBB});
         sut.Init(_rootContainer);
      }
   }

   internal class When_geting_children_for_a_distributeg_parameter : concern_for_SelectEventAssingmentTargetPresenter
   {
      private IObjectBaseDTO _distributedParameterDTO;
      private IEnumerable<IObjectBaseDTO> _result;

      protected override void Context()
      {
         base.Context();
         var id = "DIST";
         _distributedParameterDTO = A.Fake<IObjectBaseDTO>().WithId(id);
         var distributeParameter = A.Fake<IDistributedParameter>();
         A.CallTo(() => distributeParameter.Children).Returns(new[] {A.Fake<IParameter>().WithName("Mean")});
         A.CallTo(() => _context.Get<IObjectBase>(id)).Returns(distributeParameter);
      }

      protected override void Because()
      {
         _result = sut.GetChildren(_distributedParameterDTO);
      }

      [Observation]
      public void should_return_an_empty_list()
      {
         _result.ShouldBeEmpty();
      }
   }


   internal class When_geting_children_for_a_container : concern_for_SelectEventAssingmentTargetPresenter
   {
      private IObjectBaseDTO _containerDTO;
      private Container _container;
      private IParameter _parameter1;
      private IParameter _parameter2;
      private List<IObjectBaseDTO> _result;
      private Container _subContainer;
      private Container _moleculeProperties;
      private ObjectBaseDTO _subContainerDTO;
      private ObjectBaseDTO _parameterDTO1;
      private ObjectBaseDTO _parameterDTO2;
      private DummyReactionDTO _dummyReactionDTO;
      private DummyMoleculeDTO _dummyMoleculeDTO;

      protected override void Context()
      {
         base.Context();
         _parameter1 = new Parameter().WithName("P1").WithMode(ParameterBuildMode.Local);
         _parameter2 = new Parameter().WithName("P2").WithMode(ParameterBuildMode.Local);

         _subContainer = new Container().WithName("SubContainer").WithId("SubContainerId");
         _moleculeProperties = new Container().WithName(Constants.MOLECULE_PROPERTIES);
         _container = new Container
         {
            _parameter1,
            _parameter2,
            _subContainer,
            _moleculeProperties,
         }.WithMode(ContainerMode.Physical)
         .WithId("ContId");


         _containerDTO = new ContainerDTO {Id = _container.Id};
         A.CallTo(() => _context.Get<IObjectBase>(_container.Id)).Returns(_container);

         _subContainerDTO = new ObjectBaseDTO();
         _parameterDTO1 = new ObjectBaseDTO();
         _parameterDTO2 = new ObjectBaseDTO();
         _dummyReactionDTO=new DummyReactionDTO();
         _dummyMoleculeDTO = new DummyMoleculeDTO();
         A.CallTo(() => _objectBaseDTOMapper.MapFrom(_subContainer)).Returns(_subContainerDTO);
         A.CallTo(() => _objectBaseDTOMapper.MapFrom(_parameter1)).Returns(_parameterDTO1);
         A.CallTo(() => _objectBaseDTOMapper.MapFrom(_parameter2)).Returns(_parameterDTO2);
         A.CallTo(() => _reactionMapper.MapFrom(_reaction,_container)).Returns(_dummyReactionDTO);
         A.CallTo(() => _moleculeMapper.MapFrom(_moleculeBuilder,_container)).Returns(_dummyMoleculeDTO);
      }

      protected override void Because()
      {
         _result = sut.GetChildren(_containerDTO).ToList();
      }

      [Observation]
      public void should_add_all_local_parameters_and_sub_containers()
      {
         var objectBaseDTO = _result.OfType<ObjectBaseDTO>().ToList();
         objectBaseDTO.ShouldContain(_parameterDTO1,_parameterDTO2,_subContainerDTO);
      }

      [Observation]
      public void should_add_local_reaction_and_molecule_parmaeters_for_physical_container()
      {
         var dummyContainerDTO = _result.OfType<IDummyContainer>().ToList();
         dummyContainerDTO.ShouldOnlyContain(_dummyReactionDTO,_dummyMoleculeDTO);
      }
   }
}