using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectLocalisationPresenter : IPresenter, IDisposable
   {
      IEntity Select(Localisations localisation);
      IEnumerable<ObjectBaseDTO> GetChildObjects(string parentId);
      bool SelectionIsValid(ObjectBaseDTO selectedDTO);
   }

   internal class SelectLocalisationPresenter : AbstractPresenter<ISelectLocalisationView, ISelectLocalisationPresenter>, ISelectLocalisationPresenter
   {
      private Localisations _localisation;
      private readonly IMoBiContext _context;
      private readonly IModalPresenter _modalPresenter;
      private readonly IObjectBaseToObjectBaseDTOMapper _mapper;
      private readonly IContainerToContainerDTOMapper _dtoContainerMapper;

      public SelectLocalisationPresenter(ISelectLocalisationView view, IMoBiContext context, IModalPresenter modalPresenter,
                                         IObjectBaseToObjectBaseDTOMapper mapper, IContainerToContainerDTOMapper dtoContainerMapper)
         : base(view)
      {
         _context = context;
         _modalPresenter = modalPresenter;
         _dtoContainerMapper = dtoContainerMapper;
         _mapper = mapper;
         _modalPresenter.Encapsulate(this);
         _modalPresenter.Text = AppConstants.Captions.SelectLocalReferencePoint;
      }

      private SpatialStructureDTO createSpatialStructureDTOFrom(IMoBiSpatialStructure spatialStructure)
      {
         var dto = new SpatialStructureDTO(spatialStructure)
         {
            Id = spatialStructure.Id,
            Name = spatialStructure.Name,
            Icon = spatialStructure.Icon
         };

         if (_localisation.Is(Localisations.ContainerOnly))
            dto.TopContainer = spatialStructure.TopContainers.MapAllUsing(_dtoContainerMapper);

         if (_localisation.Is(Localisations.NeighborhoodsOnly))
            dto.Neighborhoods = _dtoContainerMapper.MapFrom(spatialStructure.NeighborhoodsContainer);

         if (_localisation.Is(Localisations.Everywhere))
         {
            dto.TopContainer = spatialStructure.TopContainers.MapAllUsing(_dtoContainerMapper);
            dto.Neighborhoods = _dtoContainerMapper.MapFrom(spatialStructure.NeighborhoodsContainer);
         }
 
         return dto;
      }

      public virtual IEntity Select(Localisations localisation)
      {
         _localisation = localisation;
         var spatialStructures = _context.CurrentProject.SpatialStructureCollection;
         _view.Show(spatialStructures.Select(createSpatialStructureDTOFrom).ToList());

         if (!_modalPresenter.Show())
            return null;
         
         var dto = _view.Selected;
         var selectedEntity = getSelectedEntity(dto);
         
         return isUsableLocalisation(selectedEntity) ? selectedEntity : null;
      }

      private bool isUsableLocalisation(IContainer selectedEntity)
      {
         if (selectedEntity == null) return false;
         if (_localisation.Is(Localisations.PhysicalOnly))
            return selectedEntity.Mode.Equals(ContainerMode.Physical);

         return true;
      }

      private IContainer getSelectedEntity(ObjectBaseDTO dto)
      {
         return _context.Get<IContainer>(dto.Id);
      }

      public IEnumerable<ObjectBaseDTO> GetChildObjects(string parentId)
      {
         var parent = _context.Get<IContainer>(parentId);
         if (parent == null)
            return Enumerable.Empty<ObjectBaseDTO>();

         return parent.GetChildrenSortedByName<IContainer>(canAddContainer)
            .MapAllUsing(_mapper);
      }

      private bool canAddContainer(IContainer container)
      {
         return !container.IsNamed(Constants.MOLECULE_PROPERTIES)
                && !container.IsAnImplementationOf<IParameter>();
      }

      public bool SelectionIsValid(ObjectBaseDTO selectedDTO)
      {
         return isUsableLocalisation(getSelectedEntity(selectedDTO));
      }

      public void Dispose()
      {
         _modalPresenter.Dispose();
      }
   }
}