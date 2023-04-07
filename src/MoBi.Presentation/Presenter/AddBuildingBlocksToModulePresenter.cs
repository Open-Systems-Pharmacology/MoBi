using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IAddBuildingBlocksToModulePresenter : IDisposablePresenter
   {
      IReadOnlyList<IBuildingBlock> AddBuildingBlocksToModule(Module module);
   }

   public class AddBuildingBlocksToModulePresenter : AbstractDisposablePresenter<IAddBuildingBlocksToModuleView, IAddBuildingBlocksToModulePresenter>,
      IAddBuildingBlocksToModulePresenter
   {
      private readonly IAddBuildingBlocksToModuleDTOToBuildingBlocksListMapper _addBuildingBlocksToModuleDTOToBuildingBlocksListMapper;

      public AddBuildingBlocksToModulePresenter(IAddBuildingBlocksToModuleView view,
         IAddBuildingBlocksToModuleDTOToBuildingBlocksListMapper addBuildingBlocksToModuleDTOToBuildingBlocksListMapper) : base(view)
      {
         _addBuildingBlocksToModuleDTOToBuildingBlocksListMapper = addBuildingBlocksToModuleDTOToBuildingBlocksListMapper;
      }

      public IReadOnlyList<IBuildingBlock> AddBuildingBlocksToModule(Module module)
      {
         _view.Caption = AppConstants.Captions.AddBuildingBlocksToModule(module.Name);

         var addBuildingBlocksToModuleDTO = new AddBuildingBlocksToModuleDTO(module);

         _view.BindTo(addBuildingBlocksToModuleDTO);
         _view.DisableExistingBuildingBlocks(addBuildingBlocksToModuleDTO);
         _view.Display();

         if (_view.Canceled)
            return new List<IBuildingBlock>();

         return _addBuildingBlocksToModuleDTOToBuildingBlocksListMapper.MapFrom(addBuildingBlocksToModuleDTO);
      }
   }
}