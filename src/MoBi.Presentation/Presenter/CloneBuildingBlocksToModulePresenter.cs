using MoBi.Assets;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface ICloneBuildingBlocksToModulePresenter : IBaseModuleContentPresenter
   {
      /// <summary>
      ///    Selectively removes building blocks from <paramref name="clonedModule" /> based on user selection
      /// </summary>
      /// <returns>False if the user canceled, otherwise True</returns>
      bool SelectClonedBuildingBlocks(Module clonedModule);
   }

   public class CloneBuildingBlocksToModulePresenter : BaseModuleContentPresenter<ICloneBuildingBlocksToModuleView, ICloneBuildingBlocksToModulePresenter>,
      ICloneBuildingBlocksToModulePresenter
   {
      private readonly IMoBiProjectRetriever _projectRetriever;
      private CloneBuildingBlocksToModuleDTO _dto;

      public CloneBuildingBlocksToModulePresenter(ICloneBuildingBlocksToModuleView view, IMoBiProjectRetriever projectRetriever) : base(view)
      {
         _projectRetriever = projectRetriever;
      }

      public bool SelectClonedBuildingBlocks(Module clonedModule)
      {
         _view.Caption = AppConstants.Captions.SelectBuildingBlocksToCloneFrom(clonedModule);
         _dto = new CloneBuildingBlocksToModuleDTO(clonedModule);
         _dto.AddUsedNames(_projectRetriever.Current.Modules.AllNames());

         _view.BindTo(_dto);
         _view.Display();

         if (_view.Canceled)
            return false;

         _dto.BuildingBlocksToRemove.Each(clonedModule.Remove);

         clonedModule.Name = _dto.Name;
         clonedModule.MergeBehavior = _dto.MergeBehavior;

         return true;
      }

      public override MergeBehavior SelectedBehavior => _dto.MergeBehavior;
   }
}