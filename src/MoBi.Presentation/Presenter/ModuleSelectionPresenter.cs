using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public class ModuleSelectionPresenter : AbstractCommandCollectorPresenter<IModuleSelectionView, IModuleSelectionPresenter>, IModuleSelectionPresenter
   {
      private readonly IMoBiContext _context;
      private Module _editedModule;
      private ModuleSelectionDTO _moduleSelectionDTO;
      public event Action SelectionChanged = delegate { };

      public void Edit(Module module)
      {
         _editedModule = module;
         updateSelectionWithModule(_editedModule ?? AllAvailableModules.FirstOrDefault());
      }

      private void updateSelectionWithModule(Module module)
      {
         _moduleSelectionDTO = new ModuleSelectionDTO { SelectedObject = module };
         _view.BindTo(_moduleSelectionDTO);
      }

      public ModuleSelectionPresenter(IModuleSelectionView view, IMoBiContext context) : base(view)
      {
         _context = context;
      }

      public bool CanCreateBuildingBlock
      {
         set => View.NewVisible = value;
      }

      public void CreateNew()
      {
         // TODO SIMULATION_CONFIGURATION
         throw new NotImplementedException();
      }

      public IEnumerable<Module> AllAvailableModules => _context.CurrentProject.Modules.OrderBy(x => x.Name);
      public Module SelectedModule => _moduleSelectionDTO.SelectedObject;

      public string DisplayNameFor(Module module)
      {
         return module.Name;
      }

      public void SelectedModuleChanged()
      {
         SelectionChanged();
      }
   }

   public interface IModuleSelectionPresenter : ICommandCollectorPresenter, IPresenter<IModuleSelectionView>
   {
      void CreateNew();
      IEnumerable<Module> AllAvailableModules { get; }
      Module SelectedModule { get; }
      bool CanCreateBuildingBlock { set; }
      string DisplayNameFor(Module module);
      void SelectedModuleChanged();
      event Action SelectionChanged;
      void Edit(Module module);
   }
}