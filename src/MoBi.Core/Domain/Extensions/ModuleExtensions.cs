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
            case IMoleculeBuildingBlock molecule:
               module.Molecule = molecule;
               break;
            case IReactionBuildingBlock reaction:
               module.Reaction = reaction;
               break;
            case ISpatialStructure spatialStructure:
               module.SpatialStructure = spatialStructure;
               break;
            case IPassiveTransportBuildingBlock passiveTransport:
               module.PassiveTransport = passiveTransport;
               break;
            case IEventGroupBuildingBlock eventGroup:
               module.EventGroup = eventGroup;
               break;
            case IObserverBuildingBlock observer:
               module.Observer = observer;
               break;
            case IParameterStartValuesBuildingBlock parameterStartValues:
               module.AddParameterStartValueBlock(parameterStartValues);
               break;
            case IMoleculeStartValuesBuildingBlock moleculeStartValues:
               module.AddMoleculeStartValueBlock(moleculeStartValues);
               break;
            default:
               throw new MoBiException(AppConstants.Exceptions.BuildingBlockTypeNotSupported(buildingBlock));
         }
      }

      public static void RemoveBuildingBlock(this Module module, IBuildingBlock buildingBlock)
      {
         switch (buildingBlock)
         {
            case IMoleculeBuildingBlock molecule:
               module.Molecule = null;
               break;
            case IReactionBuildingBlock reaction:
               module.Reaction = null;
               break;
            case ISpatialStructure spatialStructure:
               module.SpatialStructure = null;
               break;
            case IPassiveTransportBuildingBlock passiveTransport:
               module.PassiveTransport = null;
               break;
            case IEventGroupBuildingBlock eventGroup:
               module.EventGroup = null;
               break;
            case IObserverBuildingBlock observer:
               module.Observer = null;
               break;
            case IParameterStartValuesBuildingBlock parameterStartValues:
               //module.AddParameterStartValueBlock(parameterStartValues);
               break;
            case IMoleculeStartValuesBuildingBlock moleculeStartValues:
               //module.AddMoleculeStartValueBlock(moleculeStartValues);
               break;
            default:
               throw new MoBiException(AppConstants.Exceptions.BuildingBlockTypeNotSupported(buildingBlock));
         }
      }
   }
}