using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Presentation.MenusAndBars.ContextMenus;
using MoBi.Presentation.Nodes;
using MoBi.Presentation.Presenter.Main;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters.ContextMenus;

namespace MoBi.Presentation
{
   public abstract class concern_for_RootContextMenuFactoryForProjectItem : ContextSpecification<IContextMenuSpecificationFactory<IViewItem>>
   {
      protected override void Context()
      {
         sut = new RootContextMenuFactoryForMoleculeBuildingBlock();
      }
   }

   public class When_asked_if_Satsifyed_by_a_correct_root_view_item : concern_for_RootContextMenuFactoryForProjectItem
   {
      private bool _result;

      protected override void Because()
      {
         _result = sut.IsSatisfiedBy(MoBiRootNodeTypes.MoleculeFolder, A.Fake<IModuleExplorerPresenter>());
      }

      [Observation]
      public void should_return_true()
      {
         _result.ShouldBeTrue();
      }
   }
}