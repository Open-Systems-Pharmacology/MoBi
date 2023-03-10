using MoBi.Assets;
using MoBi.Core.Domain.Model;
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

   public class AddBuildingBlocksToModulePresenter : AbstractDisposablePresenter<IAddBuildingBlocksToModuleView, IAddBuildingBlocksToModulePresenter>, IAddBuildingBlocksToModulePresenter
   {
      private readonly ICreateModuleDTOToModuleMapper _createModuleDTOToModuleMapper;

      public AddBuildingBlocksToModulePresenter(IAddBuildingBlocksToModuleView view, ICreateModuleDTOToModuleMapper createModuleDTOToModuleMapper) : base(view)
      {
         _createModuleDTOToModuleMapper = createModuleDTOToModuleMapper;
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

         return _createModuleDTOToModuleMapper.MapFrom(addBuildingBlocksToModuleDTO);
      }
   }
}