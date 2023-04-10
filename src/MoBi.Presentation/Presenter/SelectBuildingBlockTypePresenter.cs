using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
  
   public interface ISelectBuildingBlockTypePresenter : IDisposablePresenter
   {
      BuildingBlockType GetBuildingBlockType(Module module);
      List<BuildingBlockType> AllowedBuildingBlockTypes { get; }
   }

   public class SelectBuildingBlockTypePresenter : AbstractDisposablePresenter<ISelectBuildingBlockTypeView, ISelectBuildingBlockTypePresenter>, ISelectBuildingBlockTypePresenter
   {

      public SelectBuildingBlockTypePresenter(ISelectBuildingBlockTypeView view) : base(view)
      {
      }

      public BuildingBlockType GetBuildingBlockType(Module module)
      {
         AllowedBuildingBlockTypes = new List<BuildingBlockType>();

         if (module.Molecules != null)
            AllowedBuildingBlockTypes.Add(BuildingBlockType.Molecule);

         if (module.Reactions != null)
            AllowedBuildingBlockTypes.Add(BuildingBlockType.Reaction);

         if (module.SpatialStructure != null)
            AllowedBuildingBlockTypes.Add(BuildingBlockType.SpatialStructure);

         if (module.PassiveTransports != null)
            AllowedBuildingBlockTypes.Add(BuildingBlockType.PassiveTransport);

         if (module.EventGroups != null)
            AllowedBuildingBlockTypes.Add(BuildingBlockType.EventGroup);

         if (module.Observers != null)
            AllowedBuildingBlockTypes.Add(BuildingBlockType.Observer);

         AllowedBuildingBlockTypes.Add(BuildingBlockType.MoleculeStartValues);
         AllowedBuildingBlockTypes.Add(BuildingBlockType.ParameterStartValues);

         var selectBuildingBlockTypeDTO = new SelectBuildingBlockTypeDTO(module);
         _view.BindTo(selectBuildingBlockTypeDTO);
         _view.Display();

         return _view.Canceled ? BuildingBlockType.None : selectBuildingBlockTypeDTO.SelectedBuildingBlockType;
      }

      public List<BuildingBlockType> AllowedBuildingBlockTypes { get; private set; }
   }
}