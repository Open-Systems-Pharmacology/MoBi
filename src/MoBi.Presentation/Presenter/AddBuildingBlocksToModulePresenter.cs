using System;
using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Presenter
{
   public interface IAddBuildingBlocksToModulePresenter : IAddContentToModulePresenter
   {
      IReadOnlyList<IBuildingBlock> AddBuildingBlocksToModule(Module module);
      IReadOnlyList<IBuildingBlock> AddInitialConditionsToModule(Module module);
      IReadOnlyList<IBuildingBlock> AddParameterValuesToModule(Module module);
   }

   public class AddBuildingBlocksToModulePresenter : AddContentToModulePresenter<IAddBuildingBlocksToModuleView, IAddBuildingBlocksToModulePresenter>,
      IAddBuildingBlocksToModulePresenter
   {
      private readonly IAddBuildingBlocksToModuleDTOToBuildingBlocksListMapper _addBuildingBlocksToModuleDTOToBuildingBlocksListMapper;
      private readonly IModuleToAddBuildingBlocksToModuleDTOMapper _moduleToAddBuildingBlocksToModuleDTOMapper;
      private AddBuildingBlocksToModuleDTO _addBuildingBlocksToModuleDTO;
      private Module _module;

      public AddBuildingBlocksToModulePresenter(IAddBuildingBlocksToModuleView view,
         IAddBuildingBlocksToModuleDTOToBuildingBlocksListMapper addBuildingBlocksToModuleDTOToBuildingBlocksListMapper, 
         IModuleToAddBuildingBlocksToModuleDTOMapper moduleToAddBuildingBlocksToModuleDTOMapper) : base(view)
      {
         _addBuildingBlocksToModuleDTOToBuildingBlocksListMapper = addBuildingBlocksToModuleDTOToBuildingBlocksListMapper;
         _moduleToAddBuildingBlocksToModuleDTOMapper = moduleToAddBuildingBlocksToModuleDTOMapper;
      }

      public IReadOnlyList<IBuildingBlock> AddBuildingBlocksToModule(Module module)
      {
         _module = module;
         return addBuildingBlocksToModule(module);
      }

      private IReadOnlyList<IBuildingBlock> addBuildingBlocksToModule(Module module, Action<AddBuildingBlocksToModuleDTO> configureDTOAction = null)
      {
         _view.Caption = AppConstants.Captions.AddBuildingBlocksToModule(module.Name);

         _addBuildingBlocksToModuleDTO = _moduleToAddBuildingBlocksToModuleDTOMapper.MapFrom(module);
         configureDTOAction?.Invoke(_addBuildingBlocksToModuleDTO);

         _view.BindTo(_addBuildingBlocksToModuleDTO);
         _view.Display();

         if (_view.Canceled)
            return new List<IBuildingBlock>();

         return _addBuildingBlocksToModuleDTOToBuildingBlocksListMapper.MapFrom(_addBuildingBlocksToModuleDTO);
      }

      public IReadOnlyList<IBuildingBlock> AddParameterValuesToModule(Module module)
      {
         return addBuildingBlocksToModule(module, dto => dto.AllowOnlyParameterValues = true);
      }

      public IReadOnlyList<IBuildingBlock> AddInitialConditionsToModule(Module module)
      {
         return addBuildingBlocksToModule(module, dto => dto.AllowOnlyInitialConditions = true);
      }

      protected override Module Module => _module;
      protected override ModuleContentDTO ContentDTO => _addBuildingBlocksToModuleDTO;
   }
}