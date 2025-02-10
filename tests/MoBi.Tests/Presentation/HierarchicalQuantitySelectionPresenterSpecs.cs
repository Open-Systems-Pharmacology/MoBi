using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Presenters.ContextMenus;
using ISimulationPersistableUpdater = MoBi.Core.Services.ISimulationPersistableUpdater;

namespace MoBi.Presentation
{
   public class concern_for_HierarchicalQuantitySelectionPresenter : ContextSpecification<HierarchicalQuantitySelectionPresenter>
   {
      protected IHierarchicalStructureView _view;
      protected ISimulationPersistableUpdater _simulationPersistableUpdater;
      protected IEntityPathResolver _entityPathResolver;
      protected IObjectBaseToObjectBaseDTOMapper _objectBaseMapper;
      protected IMoBiContext _context;
      private IViewItemContextMenuFactory _contextMenuFactory;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         _objectBaseMapper = A.Fake<IObjectBaseToObjectBaseDTOMapper>();
         _simulationPersistableUpdater = A.Fake<ISimulationPersistableUpdater>();
         _entityPathResolver = A.Fake<IEntityPathResolver>();
         _view = A.Fake<IHierarchicalStructureView>();
         _contextMenuFactory = A.Fake<IViewItemContextMenuFactory>();
         sut = new HierarchicalQuantitySelectionPresenter(_view, _context, _objectBaseMapper, _simulationPersistableUpdater, _entityPathResolver, _contextMenuFactory);
      }
   }

   public class When_getting_child_objects_of_a_simulation : concern_for_HierarchicalQuantitySelectionPresenter
   {
      private ObjectBaseDTO _dto;
      private IQuantity _quantity;

      protected override void Context()
      {
         base.Context();
         var moBiSimulation = new MoBiSimulation
         {
            Model = new Model
            {
               Root = new Container()
            }
         };

         _dto = new ObjectBaseDTO(moBiSimulation);
         _quantity = new Observer();
         moBiSimulation.Model.Root.Add(_quantity);
      }

      protected override void Because()
      {
         sut.GetChildObjects(_dto, x => true);
      }

      [Observation]
      public void the_simulation_persistable_updater_tests_if_entities_are_selectable()
      {
         A.CallTo(() => _simulationPersistableUpdater.QuantityIsSelectable(_quantity, true)).MustHaveHappened();
      }
   }

   public class When_selecting_paths_from_simulations : concern_for_HierarchicalQuantitySelectionPresenter
   {
      private IReadOnlyList<ISimulation> _simulations;
      private ISimulation _simulation1;
      private ISimulation _simulation2;

      protected override void Context()
      {
         base.Context();
         _simulation1 = new MoBiSimulation();
         _simulation2 = new MoBiSimulation();
         _simulations = new List<ISimulation> { _simulation1, _simulation2 };

         A.CallTo(() => _objectBaseMapper.MapFrom(_simulation1)).Returns(new ObjectBaseDTO(_simulation1));
         A.CallTo(() => _objectBaseMapper.MapFrom(_simulation2)).Returns(new ObjectBaseDTO(_simulation2));
      }

      protected override void Because()
      {
         sut.SelectPathFrom(_simulations);
      }

      [Observation]
      public void should_show_simulations()
      {
         A.CallTo(() => _view.Show(A<IReadOnlyList<ObjectBaseDTO>>.That.Matches(x => hasAllSimulations(x)))).MustHaveHappened();
      }

      private bool hasAllSimulations(IReadOnlyList<ObjectBaseDTO> objectBaseDTOs)
      {
         return objectBaseDTOs.Count == 2 && objectBaseDTOs.Any(x => Equals(x.ObjectBase, _simulation1)) && objectBaseDTOs.Any(x => Equals(x.ObjectBase, _simulation2));
      }
   }

   public class When_pre_selecting_an_entity_by_path : concern_for_HierarchicalQuantitySelectionPresenter
   {
      private Observer _quantity;

      protected override void Context()
      {
         base.Context();
         base.Context();
         var organismContainer = new Container();
         var moBiSimulation = new MoBiSimulation
         {
            Model = new Model
            {
               Root = new Container
               {
                  organismContainer.WithName("Organism")
               }.WithName("Simulation")
            }
         };

         A.CallTo(() => _objectBaseMapper.MapFrom(moBiSimulation)).Returns(new ObjectBaseDTO(moBiSimulation));

         _quantity = new Observer().WithName("Observer");
         organismContainer.Add(_quantity);
         sut.SelectPathFrom(new[] { moBiSimulation });

         A.CallTo(() => _simulationPersistableUpdater.QuantityIsSelectable(_quantity, true)).Returns(true);
         A.CallTo(() => _view.Select(_quantity)).Invokes(x => sut.Select(new ObjectBaseDTO(x.Arguments.Get<IWithId>(0) as IObjectBase)));
      }

      protected override void Because()
      {
         sut.SelectQuantityFromPath(new ObjectPath("Organism", "Observer"));
      }

      [Observation]
      public void the_presenter_should_be_able_to_close_with_selection()
      {
         sut.CanClose.ShouldBeTrue();
      }

      [Observation]
      public void the_selected_path_of_the_presenter_is_set()
      {
         sut.SelectedPath.Equals(new ObjectPath("Organism", "Observer"));
      }

      [Observation]
      public void the_view_should_select_the_entity()
      {
         A.CallTo(() => _view.Select(_quantity)).MustHaveHappened();
      }
   }
}