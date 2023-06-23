using OSPSuite.BDDHelper;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.MenusAndBars.ContextMenus;
using MoBi.Presentation.Nodes;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters.ContextMenus;

namespace MoBi.Presentation
{
   public abstract class concern_for_MoleculeListPresenterSpecs : ContextSpecification<IMoleculeListPresenter>
   {
      protected IMoleculeListView _view;
      protected IMoleculeBuilderToMoleculeBuilderDTOMapper _moleculeBuilderToDTOMoleculeBuilderMapper;
      protected IViewItemContextMenuFactory _viewItemContextMenuFactory;
      protected IMoBiContext _context;
      private ITreeNodeFactory _treeNodeFactory;


      protected override void Context()
      {
         _view = A.Fake<IMoleculeListView>();
         _moleculeBuilderToDTOMoleculeBuilderMapper = A.Fake<IMoleculeBuilderToMoleculeBuilderDTOMapper>();
         _viewItemContextMenuFactory = A.Fake<IViewItemContextMenuFactory>();
         _context = A.Fake<IMoBiContext>();
         _treeNodeFactory = A.Fake<ITreeNodeFactory>();
         sut = new MoleculeListPresenter(_view, _moleculeBuilderToDTOMoleculeBuilderMapper, _viewItemContextMenuFactory,
            _context, _treeNodeFactory);
      }
   }

   internal class When_handling_selected_event_for_MoleculeBuilder : concern_for_MoleculeListPresenterSpecs
   {
      private MoleculeBuilder _moleculeBuilder;
      private MoleculeBuildingBlock _moleculeBuildingBlock;

      protected override void Context()
      {
         base.Context();
         _moleculeBuildingBlock = new MoleculeBuildingBlock();
         sut.Edit(_moleculeBuildingBlock);
         _moleculeBuilder = new MoleculeBuilder();
         _moleculeBuildingBlock.Add(_moleculeBuilder);
      }

      protected override void Because()
      {
         sut.Handle(new EntitySelectedEvent(_moleculeBuilder, null));
      }

      [Observation]
      public void should_select_moleculebuilder_in_view()
      {
         A.CallTo(() => _view.SelectItem(_moleculeBuilder)).MustHaveHappened();
      }
   }

   internal class When_handling_selected_event_for_a_parameter_at_a_moleculeBuilder :
      concern_for_MoleculeListPresenterSpecs
   {
      private MoleculeBuilder _moleculeBuilder;
      private MoleculeBuildingBlock _moleculeBuildingBlock;
      private IParameter _parameter;

      protected override void Context()
      {
         base.Context();
         _moleculeBuildingBlock = new MoleculeBuildingBlock();
         sut.Edit(_moleculeBuildingBlock);
         _moleculeBuilder = new MoleculeBuilder();
         _moleculeBuildingBlock.Add(_moleculeBuilder);
         _parameter = new Parameter();
         _moleculeBuilder.AddParameter(_parameter);
      }

      protected override void Because()
      {
         sut.Handle(new EntitySelectedEvent(_moleculeBuilder, null));
      }

      [Observation]
      public void should_select_parent_moleculebuilder_in_view()
      {
         A.CallTo(() => _view.SelectItem(_moleculeBuilder)).MustHaveHappened();
      }
   }
}