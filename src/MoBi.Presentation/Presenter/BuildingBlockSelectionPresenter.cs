using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IModuleSelectionPresenter : ICommandCollectorPresenter, IPresenter<IModuleSelectionView>
   {
      void CreateNew();
      IEnumerable<Module> AllAvailableModules { get; }
      Module SelectedModule { get; }
      string DisplayNameFor(Module module);
      void SelectedModuleChanged();
      event Action SelectionChanged;
      void Edit(Module module);
   }

   public interface IBuildingBlockSelectionPresenter : ICommandCollectorPresenter, IPresenter<IBuildingBlockSelectionView>
   {
      IEnumerable<IBuildingBlock> AllAvailableBlocks { get; }
      void SelectedBuildingBlockChanged();
      void CreateNew();
      string DisplayNameFor(IBuildingBlock buildingBlock);
   }

   public interface IBuildingBlockSelectionPresenter<T> : IBuildingBlockSelectionPresenter where T : class, IBuildingBlock
   {
      bool CanCreateBuildingBlock { set; }
      T SelectedBuildingBlock { get; }
      void Edit(T buildingBlock);
      event Action SelectionChanged;
      void UpdateBuildingBlock(IBuildingBlock buildingBlock);
   }

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
         _moduleSelectionDTO = new ModuleSelectionDTO { SelectedObject = module};
         _view.BindTo(_moduleSelectionDTO);
      }

      public ModuleSelectionPresenter(IModuleSelectionView view, IMoBiContext context) : base(view)
      {
         _context = context;
      }

      public void CreateNew()
      {
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

   public abstract class BaseBuildingBlockSelectionPresenter<TBuildingBlock> : AbstractCommandCollectorPresenter<IBuildingBlockSelectionView, IBuildingBlockSelectionPresenter>, 
      IBuildingBlockSelectionPresenter<TBuildingBlock> where TBuildingBlock : class, IBuildingBlock
   {
      private readonly IInteractionTasksForBuildingBlock<TBuildingBlock> _interactionTasks;
      private readonly IMoBiContext _context;
      private BuildingBlockSelectionDTO _buildingBlockSelectionDTO;
      protected TBuildingBlock _editedBuildingBlock;

      public event Action SelectionChanged = delegate { };

      protected BaseBuildingBlockSelectionPresenter(IBuildingBlockSelectionView view,
         IInteractionTasksForBuildingBlock<TBuildingBlock> interactionTasks, IMoBiContext context)
         : base(view)
      {
         _interactionTasks = interactionTasks;
         _context = context;
         _buildingBlockSelectionDTO = new BuildingBlockSelectionDTO();
      }

      public void UpdateBuildingBlock(IBuildingBlock buildingBlock)
      {
         refreshBuildingBlocks();
         selectBuildingBlock(buildingBlock);
      }

      public bool CanCreateBuildingBlock
      {
         set => View.NewVisible = value;
      }

      public TBuildingBlock SelectedBuildingBlock => _buildingBlockSelectionDTO.SelectedObject.DowncastTo<TBuildingBlock>();

      public void Edit(TBuildingBlock buildingBlock)
      {
         _editedBuildingBlock = buildingBlock;
         updateSelectionWithBuildingBlock(_editedBuildingBlock ?? AllAvailableBlocks.FirstOrDefault());
      }

      public abstract IEnumerable<IBuildingBlock> AllAvailableBlocks { get; }

      public void CreateNew()
      {
         //Directly add to context as this action should be added to history
         _context.AddToHistory(_interactionTasks.AddNew());
         updateSelectionWithBuildingBlock(AllAvailableBlocks.LastOrDefault());
         SelectedBuildingBlockChanged();
      }

      public string DisplayNameFor(IBuildingBlock buildingBlock)
      {
         return buildingBlock.Name;
      }

      public void SelectedBuildingBlockChanged()
      {
         SelectionChanged();
      }

      private void refreshBuildingBlocks()
      {
         _view.RefreshElementList();
      }

      private void selectBuildingBlock(IBuildingBlock buildingBlock)
      {
         _buildingBlockSelectionDTO.SelectedObject = buildingBlock;
      }

      private void updateSelectionWithBuildingBlock(IBuildingBlock buildingBlock)
      {
         _buildingBlockSelectionDTO = new BuildingBlockSelectionDTO { SelectedObject = buildingBlock };
         _view.BindTo(_buildingBlockSelectionDTO);
      }
   }

   public interface IStartValuesSelectionPresenter<TBuildingBlock> : IBuildingBlockSelectionPresenter<TBuildingBlock> where TBuildingBlock : class, IBuildingBlock
   {
      void SetAvailableStartValueBuildingBlocks(IReadOnlyCollection<TBuildingBlock> buildingBlocks);
   }

   public class StartValueBuildingBlockSelectionPresenter<TBuildingBlock> : BaseBuildingBlockSelectionPresenter<TBuildingBlock>, IStartValuesSelectionPresenter<TBuildingBlock> where TBuildingBlock : class, IBuildingBlock
   {
      private readonly List<IBuildingBlock> _startValuesCollection = new List<IBuildingBlock>();

      public StartValueBuildingBlockSelectionPresenter(IBuildingBlockSelectionView view, IInteractionTasksForBuildingBlock<TBuildingBlock> interactionTasks, IMoBiContext context) : base(view, interactionTasks, context)
      {
         
      }

      public void SetAvailableStartValueBuildingBlocks(IReadOnlyCollection<TBuildingBlock> buildingBlocks)
      {
         _startValuesCollection.Clear();
         _startValuesCollection.AddRange(buildingBlocks);
         _editedBuildingBlock = null;
         Edit(_editedBuildingBlock);
      }

      public override IEnumerable<IBuildingBlock> AllAvailableBlocks => _startValuesCollection;
   }

   public class BuildingBlockSelectionPresenter<TBuildingBlock> : BaseBuildingBlockSelectionPresenter<TBuildingBlock>, IBuildingBlockSelectionPresenter<TBuildingBlock> where TBuildingBlock : class, IBuildingBlock
   {
      private readonly IBuildingBlockRepository _buildingBlockRepository;

      public BuildingBlockSelectionPresenter(IBuildingBlockSelectionView view, IBuildingBlockRepository buildingBlockRepository,
         IInteractionTasksForBuildingBlock<TBuildingBlock> interactionTasks, IMoBiContext context) : base(view, interactionTasks, context)
      {
         _buildingBlockRepository = buildingBlockRepository;
      }

      public override IEnumerable<IBuildingBlock> AllAvailableBlocks => _buildingBlockRepository.All<TBuildingBlock>();
   }
}