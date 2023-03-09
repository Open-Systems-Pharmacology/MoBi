using System.Collections.Generic;
using Antlr.Runtime.Misc;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectEntityInTreePresenter : IPresenter<ISelectEntityInTreeView>
   {
      Func<ObjectBaseDTO, IReadOnlyList<ObjectBaseDTO>> GetChildren { get; set; }
      bool IsValidSelection(ObjectBaseDTO selectedDTO);
      void InitTreeStructure(IReadOnlyList<ObjectBaseDTO> entityDTOs);
      IEntity SelectedEntity { get; }
      ObjectBaseDTO SelectedDTO { get; }
      ITreeNode TreeNodeFor(ObjectBaseDTO dto);
      IObjectBase ObjectFrom(ObjectBaseDTO parentDTO);
   }

   public class SelectEntityInTreePresenter : AbstractPresenter<ISelectEntityInTreeView, ISelectEntityInTreePresenter>, ISelectEntityInTreePresenter
   {
      private readonly IMoBiContext _context;

      public SelectEntityInTreePresenter(ISelectEntityInTreeView view, IMoBiContext context) : base(view)
      {
         _context = context;
      }

      public IObjectBase ObjectFrom(ObjectBaseDTO dto) => _context.Get<IObjectBase>(dto.Id);

      public Func<ObjectBaseDTO, IReadOnlyList<ObjectBaseDTO>> GetChildren { get; set; }

      public bool IsValidSelection(ObjectBaseDTO selectedDTO)
      {
         return selectedDTO != null;
      }

      public void InitTreeStructure(IReadOnlyList<ObjectBaseDTO> entityDTOs)
      {
         _view.BindTo(entityDTOs);
      }

      public IEntity SelectedEntity => ObjectFrom(_view.Selected) as IEntity;

      public ObjectBaseDTO SelectedDTO => _view.Selected;

      public ITreeNode TreeNodeFor(ObjectBaseDTO dto) => _view.GetNode(dto.Id);
   }
}