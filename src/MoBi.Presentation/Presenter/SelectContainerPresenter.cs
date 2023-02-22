using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectContainerPresenter : ISelectObjectPathPresenter
   {
      ObjectPath Select();
   }

   public class SelectContainerPresenter : AbstractDisposablePresenter<ISelectObjectPathView, ISelectObjectPathPresenter>, ISelectContainerPresenter
   {
      private readonly IContainerToContainerDTOMapper _containerDTOMapper;
      private readonly IObjectPathFactory _objectPathFactory;
      private readonly IMoBiContext _context;

      public SelectContainerPresenter(
         ISelectObjectPathView view,
         IContainerToContainerDTOMapper containerDTOMapper,
         IObjectPathFactory objectPathFactory,
         IMoBiContext context) : base(view)
      {
         _containerDTOMapper = containerDTOMapper;
         _objectPathFactory = objectPathFactory;
         _context = context;
         _view.Caption = AppConstants.Captions.SelectContainer;
      }

      private IObjectBase getObjectFrom(ObjectBaseDTO dto)
      {
         return _context.Get<IObjectBase>(dto.Id);
      }

      public IEnumerable<ObjectBaseDTO> GetChildren(ObjectBaseDTO parentDTO)
      {
         var parent = getObjectFrom(parentDTO);
         if (parent.IsAnImplementationOf<IDistributedParameter>())
            return Enumerable.Empty<ObjectBaseDTO>();

         var container = parent as IContainer;
         if (container == null)
            return Enumerable.Empty<ObjectBaseDTO>();

         //Add sub containers removing molecule properties and and parameters
         var subContainers = container.GetChildrenSortedByName<IContainer>(x =>
            !x.IsNamed(Constants.MOLECULE_PROPERTIES) && !x.IsAnImplementationOf<IParameter>()
         );
         return subContainers.MapAllUsing(_containerDTOMapper);
      }

      public bool IsValidSelection(ObjectBaseDTO selectedDTO)
      {
         return selectedDTO != null;
      }

      public ObjectPath Select()
      {
         init();
         _view.Display();
         return _view.Canceled ? null : generatePathFromDTO(_view.Selected);
      }

      private IContainer getContainerFrom(ObjectBaseDTO dto) => getObjectFrom(dto) as IContainer;

      private ObjectPath generatePathFromDTO(ObjectBaseDTO dto)
      {
         var container = getContainerFrom(dto);
         if (container == null)
            return null;

         return _objectPathFactory.CreateAbsoluteObjectPath(container);
      }

      private void init()
      {
         var project = _context.CurrentProject;
         var list = project.SpatialStructureCollection.Select(createSpatialStructureDTOFrom).ToList();
         _view.BindTo(list);
      }

      private SpatialStructureDTO createSpatialStructureDTOFrom(IMoBiSpatialStructure spatialStructure)
      {
         return new SpatialStructureDTO
         {
            Id = spatialStructure.Id,
            Name = spatialStructure.Name,
            Icon = spatialStructure.Icon,
            TopContainer = spatialStructure.TopContainers.MapAllUsing(_containerDTOMapper),
            Neighborhoods = _containerDTOMapper.MapFrom(spatialStructure.NeighborhoodsContainer)
         };
      }
   }
}