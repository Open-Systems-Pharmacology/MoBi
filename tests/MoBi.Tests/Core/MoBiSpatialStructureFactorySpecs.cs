using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Core.Repositories;
using MoBi.Core.Services;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;

namespace MoBi.Core
{
   public abstract class concern_for_MoBiSpatialStructureFactory : ContextSpecification<MoBiSpatialStructureFactory>
   {
      private IObjectBaseFactory _objectBaseFactory;
      private MoBiSpatialStructure _spatialStructure;
      private IParameterFactory _parameterFactory;
      protected IParameter _volumeParameter;
      private IIconRepository _iconRepository;
      private IDiagramManagerFactory _diagramManagerFactory;

      protected override void Context()
      {
         _objectBaseFactory = A.Fake<IObjectBaseFactory>();
         _parameterFactory = A.Fake<IParameterFactory>();
         _iconRepository = A.Fake<IIconRepository>();
         _volumeParameter = A.Fake<IParameter>().WithName(Constants.Parameters.VOLUME);
         A.CallTo(() => _parameterFactory.CreateVolumeParameter()).Returns(_volumeParameter);
         _spatialStructure = new MoBiSpatialStructure();
         A.CallTo(() => _objectBaseFactory.Create<MoBiSpatialStructure>()).Returns(_spatialStructure);
         A.CallTo(() => _objectBaseFactory.Create<IContainer>()).ReturnsLazily(x => new Container());
         _diagramManagerFactory = A.Fake<IDiagramManagerFactory>();
         sut = new MoBiSpatialStructureFactory(_objectBaseFactory, _parameterFactory, _iconRepository, _diagramManagerFactory);
      }
   }

   public class When_creating_a_default_spatial_structure : concern_for_MoBiSpatialStructureFactory
   {
      private MoBiSpatialStructure _result;

      protected override void Because()
      {
         _result = sut.CreateDefault();
      }

      [Observation]
      public void the_name_should_be_default()
      {
         _result.Name.ShouldBeEqualTo(DefaultNames.SpatialStructure);
      }

      [Observation]
      public void should_only_include_events_container()
      {
         _result.TopContainers.Single().Name.ShouldBeEqualTo(Constants.EVENTS);
      }
   }
}