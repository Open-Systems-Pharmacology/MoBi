using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IAddBuildingBlocksToModulePresenter : IDisposablePresenter
   {
      Module AddBuildingBlocksToModule(Module module);
   }

   public class AddBuildingBlocksToModulePresenter : AbstractDisposablePresenter<IAddBuildingBlocksToModuleView, IAddBuildingBlocksToModulePresenter>,
      IAddBuildingBlocksToModulePresenter
   {
      private readonly IAddBuildingBlocksToModuleDTOToModuleMapper _addBuildingBlocksToModuleDTOToModuleMapper;

      public AddBuildingBlocksToModulePresenter(IAddBuildingBlocksToModuleView view,
         IAddBuildingBlocksToModuleDTOToModuleMapper addBuildingBlocksToModuleDTOToModuleMapper) : base(view)
      {
         _addBuildingBlocksToModuleDTOToModuleMapper = addBuildingBlocksToModuleDTOToModuleMapper;
      }

      public Module AddBuildingBlocksToModule(Module module)
      {
         _view.Caption = AppConstants.Captions.AddBuildingBlocksToModule(module.Name);

         var addBuildingBlocksToModuleDTO = new AddBuildingBlocksToModuleDTO(module);

         _view.BindTo(addBuildingBlocksToModuleDTO);
         _view.DisableExistingBuildingBlocks(addBuildingBlocksToModuleDTO);
         _view.Display();

         if (_view.Canceled)
            return null;

         return _addBuildingBlocksToModuleDTOToModuleMapper.MapFrom(addBuildingBlocksToModuleDTO);
      }
   }
}