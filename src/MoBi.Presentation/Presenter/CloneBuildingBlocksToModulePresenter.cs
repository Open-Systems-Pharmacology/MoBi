using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface IBaseModuleContentPresenter : IDisposablePresenter
   {
      IReadOnlyList<MergeBehavior> AllMergeBehaviors { get; }
   }

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

      public CloneBuildingBlocksToModulePresenter(ICloneBuildingBlocksToModuleView view, IMoBiProjectRetriever projectRetriever) : base(view)
      {
         _projectRetriever = projectRetriever;
      }

      public bool SelectClonedBuildingBlocks(Module clonedModule)
      {
         _view.Caption = AppConstants.Captions.SelectBuildingBlocksToCloneFrom(clonedModule);
         var dto = new CloneBuildingBlocksToModuleDTO(clonedModule);
         dto.AddUsedNames(_projectRetriever.Current.Modules.AllNames());

         _view.BindTo(dto);
         _view.Display();

         if (_view.Canceled)
            return false;

         dto.BuildingBlocksToRemove.Each(clonedModule.Remove);

         clonedModule.Name = dto.Name;
         clonedModule.DefaultMergeBehavior = dto.DefaultMergeBehavior;

         return true;
      }
   }
}