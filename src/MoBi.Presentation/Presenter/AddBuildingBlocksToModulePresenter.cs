using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Extensions;

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
         _view.Display();

         if (_view.Canceled)
            return new List<IBuildingBlock>();

         return _addBuildingBlocksToModuleDTOToBuildingBlocksListMapper.MapFrom(addBuildingBlocksToModuleDTO);
      }
   }
   
   public interface ICloneBuildingBlocksToModulePresenter : IDisposablePresenter
   {
      /// <summary>
      /// Selectively removes building blocks from <paramref name="clonedModule"/> based on user selection
      /// </summary>
      /// <returns>False if the user canceled, otherwise True</returns>
      bool SelectClonedBuildingBlocks(Module clonedModule);
   }
   
   public class CloneBuildingBlocksToModulePresenter : AbstractDisposablePresenter<ICloneBuildingBlocksToModuleView, ICloneBuildingBlocksToModulePresenter>,
      ICloneBuildingBlocksToModulePresenter
   {
      private readonly IMoBiContext _context;

      public CloneBuildingBlocksToModulePresenter(ICloneBuildingBlocksToModuleView view, IMoBiContext context) : base(view)
      {
         _context = context;
      }

      public bool SelectClonedBuildingBlocks(Module clonedModule)
      {
         _view.Caption = AppConstants.Captions.SelectBuildingBlocksToCloneFrom(clonedModule);
         var dto = new CloneBuildingBlocksToModuleDTO(clonedModule);
         dto.AddUsedNames(_context.CurrentProject.Modules.AllNames());

         _view.BindTo(dto);
         _view.Display();

         if (_view.Canceled)
            return false;
         
         if(dto.RemoveMolecule)
            clonedModule.Remove(clonedModule.Molecules);
         
         if(dto.RemoveEventGroup)
            clonedModule.Remove(clonedModule.EventGroups);

         if (dto.RemoveObserver)
            clonedModule.Remove(clonedModule.Observers);

         if (dto.RemovePassiveTransport)
            clonedModule.Remove(clonedModule.PassiveTransports);

         if (dto.RemoveReaction)
            clonedModule.Remove(clonedModule.Reactions);
         
         if (dto.RemoveSpatialStructure)
            clonedModule.Remove(clonedModule.SpatialStructure);

         if (dto.RemoveMoleculeStartValues)
            removeAll(clonedModule.MoleculeStartValuesCollection, clonedModule);

         if (dto.RemoveParameterStartValues)
            removeAll(clonedModule.ParameterStartValuesCollection, clonedModule);

         clonedModule.Name = dto.Name;

         return true;
      }

      private void removeAll(IReadOnlyList<IBuildingBlock> toList, Module module)
      {
         toList.Each(module.Remove);
      }
   }


}