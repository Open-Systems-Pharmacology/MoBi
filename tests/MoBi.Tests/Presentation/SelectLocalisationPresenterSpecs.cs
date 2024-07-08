using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation
{
   internal class concern_for_SelectLocalisationPresenter : ContextSpecification<SelectLocalisationPresenter>
   {
      protected ISelectLocalisationView _view;
      protected IMoBiContext _context;
      protected IModalPresenter _modalPresenter;
      protected IObjectBaseToObjectBaseDTOMapper _mapper;
      protected IContainerToContainerDTOMapper _dtoContainerMapper;
      protected IBuildingBlockRepository _buildingBlockRepository;

      protected override void Context()
      {
         _view = A.Fake<ISelectLocalisationView>();
         _context = A.Fake<IMoBiContext>();
         _modalPresenter = A.Fake<IModalPresenter>();
         _mapper = new ObjectBaseToObjectBaseDTOMapper();
         _dtoContainerMapper = A.Fake<IContainerToContainerDTOMapper>();
         _buildingBlockRepository = A.Fake<IBuildingBlockRepository>();

         sut = new SelectLocalisationPresenter(_view, _context, _modalPresenter, _mapper, _dtoContainerMapper, _buildingBlockRepository);
      }
   }

   internal class When_getting_child_objects_from_not_found_object : concern_for_SelectLocalisationPresenter
   {
      private string _parentId;
      private SpatialStructure _moBiSpatialStructure;
      private List<IObjectBase> _result;
      private IContainer _topContainer;

      protected override void Context()
      {
         base.Context();
         _moBiSpatialStructure = new MoBiSpatialStructure();
         _topContainer = new Container();
         _moBiSpatialStructure.AddTopContainer(_topContainer);
         _parentId = "parentId";
         A.CallTo(() => _context.Get(_parentId)).Returns(null);
      }

      protected override void Because()
      {
         _result = sut.GetChildObjects(_parentId).Select(x => x.ObjectBase).ToList();
      }

      [Observation]
      public void should_return_no_child_objects()
      {
         _result.ShouldBeEmpty();
      }
   }

   internal class When_getting_child_objects_from_spatial_structure : concern_for_SelectLocalisationPresenter
   {
      private string _parentId;
      private SpatialStructure _moBiSpatialStructure;
      private List<IObjectBase> _result;
      private IContainer _topContainer;

      protected override void Context()
      {
         base.Context();
         _moBiSpatialStructure = new MoBiSpatialStructure();
         _topContainer = new Container();
         _moBiSpatialStructure.AddTopContainer(_topContainer);
         _parentId = "parentId";
         A.CallTo(() => _context.Get(_parentId)).Returns(_moBiSpatialStructure);
      }

      protected override void Because()
      {
         _result = sut.GetChildObjects(_parentId).Select(x => x.ObjectBase).ToList();
      }

      [Observation]
      public void should_return_child_objects()
      {
         _result.ShouldContain(_topContainer);
      }
   }

   internal class When_getting_child_objects_from_container : concern_for_SelectLocalisationPresenter
   {
      private string _parentId;
      protected List<IObjectBase> _result;
      private IContainer _topContainer;
      protected IContainer _excludedContainer;
      protected IContainer _includedContainer;
      private Parameter _parameter;

      protected override void Context()
      {
         base.Context();
         _topContainer = new Container();
         _excludedContainer = new Container().WithName(Constants.MOLECULE_PROPERTIES).WithId("excluded");
         _parameter = new Parameter().WithId("parameter").WithName("parameter");
         _includedContainer = new Container().WithName("a container").WithId("included");
         _topContainer.Add(_excludedContainer);
         _topContainer.Add(_includedContainer);
         _topContainer.Add(_parameter);
         _parentId = "parentId";
         A.CallTo(() => _context.Get(_parentId)).Returns(_topContainer);
      }

      protected override void Because()
      {
         _result = sut.GetChildObjects(_parentId).Select(x => x.ObjectBase).ToList();
      }

      [Observation]
      public void should_not_include_excluded_container()
      {
         _result.ShouldNotContain(_excludedContainer);
         _result.ShouldNotContain(_parameter);
      }

      [Observation]
      public void should_include_other_container()
      {
         _result.ShouldContain(_includedContainer);
      }
   }
}