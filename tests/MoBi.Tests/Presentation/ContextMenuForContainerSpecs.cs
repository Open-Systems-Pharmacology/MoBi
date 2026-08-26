using System.Linq;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.MenusAndBars.ContextMenus;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Container.Castle;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views.ContextMenus;

namespace MoBi.Presentation
{
   public abstract class concern_for_ContextMenuForContainer : ContextSpecification<ContextMenuForContainer>
   {
      protected IMoBiContext _context;
      protected IActiveSubjectRetriever _activeSubjectRetriever;
      protected IContainer _moleculeProperties;
      protected ObjectBaseDTO _dto;

      public override void GlobalContext()
      {
         base.GlobalContext();
         var container = new CastleWindsorContainer();
         OSPSuite.Utility.Container.IoC.InitializeWith(container);
         container.RegisterImplementationOf(A.Fake<IContextMenuView>());
      }

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         _activeSubjectRetriever = A.Fake<IActiveSubjectRetriever>();
         var objectTypeResolver = A.Fake<IObjectTypeResolver>();
         A.CallTo(() => objectTypeResolver.TypeFor<IParameter>()).Returns(ObjectTypes.Parameter);
         A.CallTo(() => objectTypeResolver.TypeFor<IContainer>()).Returns(ObjectTypes.Container);
         A.CallTo(() => objectTypeResolver.TypeFor<IDistributedParameter>()).Returns(ObjectTypes.DistributedParameter);

         _moleculeProperties = new Container().WithName(Constants.MOLECULE_PROPERTIES).WithMode(ContainerMode.Logical);
         _dto = new ObjectBaseDTO(_moleculeProperties) {Name = Constants.MOLECULE_PROPERTIES};
         A.CallTo(() => _context.Get<IContainer>(A<string>._)).Returns(_moleculeProperties);

         sut = new ContextMenuForContainer(_context, objectTypeResolver, A.Fake<OSPSuite.Utility.Container.IContainer>(), A.Fake<IEntityPathResolver>(), _activeSubjectRetriever);
      }

      protected override void Because()
      {
         sut.InitializeWith(_dto, A.Fake<IPresenter>());
      }

      protected bool hasDeleteItem => sut.AllMenuItems().Any(x => Equals(x.Caption, AppConstants.MenuNames.Delete));
   }

   public class When_creating_the_context_menu_for_the_molecule_properties_of_a_physical_container : concern_for_ContextMenuForContainer
   {
      protected override void Context()
      {
         base.Context();
         new Container().WithName("Liver").WithMode(ContainerMode.Physical).Add(_moleculeProperties);
      }

      [Observation]
      public void should_allow_the_molecule_properties_to_be_deleted()
      {
         hasDeleteItem.ShouldBeTrue();
      }
   }

   public class When_creating_the_context_menu_for_the_global_molecule_properties_of_a_spatial_structure : concern_for_ContextMenuForContainer
   {
      protected override void Context()
      {
         base.Context();
         var spatialStructure = new MoBiSpatialStructure {GlobalMoleculeDependentProperties = _moleculeProperties};
         A.CallTo(() => _activeSubjectRetriever.Active<MoBiSpatialStructure>()).Returns(spatialStructure);
      }

      [Observation]
      public void should_allow_the_molecule_properties_to_be_deleted()
      {
         hasDeleteItem.ShouldBeTrue();
      }
   }

   public class When_creating_the_context_menu_for_a_parentless_molecule_properties_container_outside_the_active_spatial_structure : concern_for_ContextMenuForContainer
   {
      [Observation]
      public void should_not_allow_the_molecule_properties_to_be_deleted()
      {
         hasDeleteItem.ShouldBeFalse();
      }
   }

   public class When_creating_the_context_menu_for_the_molecule_properties_of_a_logical_container : concern_for_ContextMenuForContainer
   {
      protected override void Context()
      {
         base.Context();
         new Container().WithName("Organism").WithMode(ContainerMode.Logical).Add(_moleculeProperties);
      }

      [Observation]
      public void should_allow_the_molecule_properties_to_be_deleted()
      {
         hasDeleteItem.ShouldBeTrue();
      }
   }

   public class When_creating_the_context_menu_for_the_molecule_properties_of_a_neighborhood : concern_for_ContextMenuForContainer
   {
      protected override void Context()
      {
         base.Context();
         new NeighborhoodBuilder().WithName("Liver_to_Kidney").Add(_moleculeProperties);
      }

      [Observation]
      public void should_allow_the_molecule_properties_to_be_deleted()
      {
         hasDeleteItem.ShouldBeTrue();
      }
   }

   public class When_creating_the_context_menu_for_a_physical_container : concern_for_ContextMenuForContainer
   {
      protected override void Context()
      {
         base.Context();
         _moleculeProperties = new Container().WithName("Liver").WithMode(ContainerMode.Physical);
         _dto = new ObjectBaseDTO(_moleculeProperties) {Name = "Liver"};
         A.CallTo(() => _context.Get<IContainer>(A<string>._)).Returns(_moleculeProperties);
      }

      [Observation]
      public void should_allow_the_container_to_be_deleted()
      {
         hasDeleteItem.ShouldBeTrue();
      }
   }
}
