using MoBi.Assets;
using MoBi.Core.Exceptions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Domain.Extensions
{
   public static class ModuleExtensions
   {
      public static void AddBuildingBlock(this Module module, IBuildingBlock buildingBlock)
      {
         switch (buildingBlock)
         {
            case MoleculeBuildingBlock molecule:
               module.Molecules = molecule;
               break;
            case IReactionBuildingBlock reaction:
               module.Reactions = reaction;
               break;
            case ISpatialStructure spatialStructure:
               module.SpatialStructure = spatialStructure;
               break;
            case IPassiveTransportBuildingBlock passiveTransport:
               module.PassiveTransports = passiveTransport;
               break;
            case IEventGroupBuildingBlock eventGroup:
               module.EventGroups = eventGroup;
               break;
            case IObserverBuildingBlock observer:
               module.Observers = observer;
               break;
            case ParameterStartValuesBuildingBlock parameterStartValues:
               module.AddParameterStartValueBlock(parameterStartValues);
               break;
            case MoleculeStartValuesBuildingBlock moleculeStartValues:
               module.AddMoleculeStartValueBlock(moleculeStartValues);
               break;
            case null:
               return;
            default:
               throw new MoBiException(AppConstants.Exceptions.BuildingBlockTypeNotSupported(buildingBlock));
         }
      }

      public static void RemoveBuildingBlock(this Module module, IBuildingBlock buildingBlock)
      {
         switch (buildingBlock)
         {
            case MoleculeBuildingBlock molecule:
               module.Molecules = null;
               break;
            case IReactionBuildingBlock reaction:
               module.Reactions = null;
               break;
            case ISpatialStructure spatialStructure:
               module.SpatialStructure = null;
               break;
            case IPassiveTransportBuildingBlock passiveTransport:
               module.PassiveTransports = null;
               break;
            case IEventGroupBuildingBlock eventGroup:
               module.EventGroups = null;
               break;
            case IObserverBuildingBlock observer:
               module.Observers = null;
               break;
            case ParameterStartValuesBuildingBlock parameterStartValues:
               //module.RemoveParameterStartValueBlock(parameterStartValues);
               break;
            case MoleculeStartValuesBuildingBlock moleculeStartValues:
               //module.RemoveMoleculeStartValueBlock(moleculeStartValues);
               break;
            case null:
               return;
            default:
               throw new MoBiException(AppConstants.Exceptions.BuildingBlockTypeNotSupported(buildingBlock));
         }
      }
   }
}