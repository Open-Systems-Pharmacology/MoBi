using System.Linq;
using FakeItEasy;
using MoBi.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Core.Repositories;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;

namespace MoBi.Core
{
   public abstract class concern_for_MoBiSpatialStructureFactory : ContextSpecification<IMoBiSpatialStructureFactory>
   {
      private IObjectBaseFactory _objectBaseFactory;
      private IMoBiSpatialStructure _spatialStructure;
      private IParameterFactory _parameterFactory;
      protected IParameter _volumeParameter;
      private IIconRepository _iconRepository;
      private IDiagramManagerFactory _diagramManagerFactory;

      protected override void Context()
      {
         _objectBaseFactory= A.Fake<IObjectBaseFactory>();
         _parameterFactory= A.Fake<IParameterFactory>();
         _iconRepository= A.Fake<IIconRepository>();
         _volumeParameter = A.Fake<IParameter>().WithName(Constants.Parameters.VOLUME);
         A.CallTo(() => _parameterFactory.CreateVolumeParameter()).Returns(_volumeParameter);
         _spatialStructure = new MoBiSpatialStructure();
         A.CallTo(() => _objectBaseFactory.Create<IMoBiSpatialStructure>()).Returns(_spatialStructure);
         A.CallTo(() => _objectBaseFactory.Create<IContainer>()).ReturnsLazily(x => new Container());
         _diagramManagerFactory = A.Fake<IDiagramManagerFactory>();
         sut = new MoBiSpatialStructureFactory(_objectBaseFactory,_parameterFactory,_iconRepository, _diagramManagerFactory);
      }
   }

   public class When_creating_a_default_spatial_structure : concern_for_MoBiSpatialStructureFactory
   {
      private IMoBiSpatialStructure _result;

      protected override void Because()
      {
         _result = sut.CreateDefault(AppConstants.DefaultNames.SpatialStructure);
      }

      [Observation]
      public void should_have_added_a_default_top_container_of_type_organism()
      {
         var topContainer = _result.TopContainers.Find(x => x.ContainerType == ContainerType.Organism);
         topContainer.ShouldNotBeNull();
         topContainer.Mode.ShouldBeEqualTo(ContainerMode.Physical);
         topContainer.Children.Contains(_volumeParameter).ShouldBeTrue();
      }

      [Observation]
      public void should_have_added_a_molecule_container_under_the_organism_container()
      {
         var topContainer = _result.TopContainers.Find(x => x.ContainerType == ContainerType.Organism);
         var moleculeContainer = topContainer.EntityAt<IContainer>(Constants.MOLECULE_PROPERTIES);
         moleculeContainer.ShouldNotBeNull();
         moleculeContainer.Mode.ShouldBeEqualTo(ContainerMode.Logical);
      }
   }
}	