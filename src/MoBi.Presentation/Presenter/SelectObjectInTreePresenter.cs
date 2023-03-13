using System;
using System.Collections.Generic;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public class SelectedEntityChangedArgs : EventArgs
   {
      public IEntity Entity { get; }
      public ObjectPath Path { get; }

      public SelectedEntityChangedArgs(IEntity entity, ObjectPath path)
      {
         Entity = entity;
         Path = path;
      }
   }

   public interface ISelectEntityInTreePresenter : IPresenter<ISelectEntityInTreeView>
   {
      Func<ObjectBaseDTO, IReadOnlyList<ObjectBaseDTO>> GetChildren { get; set; }
      bool IsValidSelection(ObjectBaseDTO selectedDTO);
      void InitTreeStructure(IReadOnlyList<ObjectBaseDTO> entityDTOs);
      IEntity SelectedEntity { get; }
      ObjectBaseDTO SelectedDTO { get; }
      ITreeNode TreeNodeFor(ObjectBaseDTO dto);
      ObjectPath SelectedEntityPath { get; }
      void SelectObjectBaseDTO(ObjectBaseDTO dto);
      event EventHandler<SelectedEntityChangedArgs> OnSelectedEntityChanged;
   }

   public class SelectEntityInTreePresenter : AbstractPresenter<ISelectEntityInTreeView, ISelectEntityInTreePresenter>, ISelectEntityInTreePresenter
   {
      private readonly IObjectPathFactory _objectPathFactory;
      private readonly IMoBiContext _context;
      public event EventHandler<SelectedEntityChangedArgs> OnSelectedEntityChanged = delegate { };

      public SelectEntityInTreePresenter(ISelectEntityInTreeView view, IObjectPathFactory objectPathFactory, IMoBiContext context) : base(view)
      {
         _objectPathFactory = objectPathFactory;
         _context = context;
      }

      protected IEntity EntityFrom(ObjectBaseDTO dto) => _context.Get<IEntity>(dto.Id);

      public ObjectPath SelectedEntityPath => SelectedEntity != null ? _objectPathFactory.CreateAbsoluteObjectPath(SelectedEntity) : null;

      public virtual void SelectObjectBaseDTO(ObjectBaseDTO dto)
      {
         var entity = EntityFrom(dto);
         if (entity == null)
            return;

         OnSelectedEntityChanged(this, new SelectedEntityChangedArgs(entity, _objectPathFactory.CreateAbsoluteObjectPath(entity)));
      }

      public Func<ObjectBaseDTO, IReadOnlyList<ObjectBaseDTO>> GetChildren { get; set; }

      public bool IsValidSelection(ObjectBaseDTO selectedDTO)
      {
         return selectedDTO != null;
      }

      public void InitTreeStructure(IReadOnlyList<ObjectBaseDTO> entityDTOs)
      {
         _view.BindTo(entityDTOs);
      }

      public IEntity SelectedEntity => EntityFrom(_view.Selected);

      public ObjectBaseDTO SelectedDTO => _view.Selected;

      public ITreeNode TreeNodeFor(ObjectBaseDTO dto) => _view.GetNode(dto.Id);
   }
}