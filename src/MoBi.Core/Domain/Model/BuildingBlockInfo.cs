using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Assets;

namespace MoBi.Core.Domain.Model
{
   public interface IBuildingBlockInfo : IWithName
   {
      /// <summary>
      ///    This is the simulation building block that was used when the simulation was created. This is not the template
      ///    building block
      /// </summary>
      IBuildingBlock UntypedBuildingBlock { get; set; }

      /// <summary>
      ///    This is the template building block that was used when the simulation was created.
      /// </summary>
      IBuildingBlock UntypedTemplateBuildingBlock { get; set; }

      /// <summary>
      ///    Returns true if the building block in the simulation has changed otherwise false
      /// </summary>
      bool SimulationHasChanged { get; }

      uint SimulationChanges { get; set; }

      /// <summary>
      ///    Gets a value indicating whether original building block changed after being used to create simulation or an object created
      ///    from the building block is changed in simulation.
      /// </summary>
      /// <value>
      ///    <c>true</c> if building block changed; otherwise, <c>false</c>.
      /// </value>
      bool BuildingBlockChanged { get; }

      string IconName { get; }

      /// <summary>
      ///    Id of the template building block. This is required only for serialization and should not be used otherwise
      /// </summary>
      string TemplateBuildingBlockId { get; set; }

      /// <summary>
      /// Returns <c>true</c> of the referenced building block is the template building block otherwise false (e.g. TemplateBuildingBlock == BuildingBlock)
      /// </summary>
      bool BuildingBlockIsTemplate { get; }
   }

   public interface IBuildingBlockInfo<T> : IBuildingBlockInfo where T : class, IBuildingBlock
   {
      /// <summary>
      ///    Building block used in the simulation
      /// </summary>
      T BuildingBlock { get; set; }

      /// <summary>
      ///    Reference to template building block used in the simulation. This is the template building block that was used when
      ///    the simulation was created
      /// </summary>
      T TemplateBuildingBlock { get; set; }
   }

   public abstract class BuildingBlockInfo<T> : IBuildingBlockInfo<T> where T : class, IBuildingBlock
   {
      public string TemplateBuildingBlockId { get; set; }
      private T _templateBuildingBlock;
      private readonly string _typeName;
      public string IconName { get; private set; }
      public uint SimulationChanges { get; set; }
      public virtual T BuildingBlock { get; set; }

      public virtual T TemplateBuildingBlock
      {
         get { return _templateBuildingBlock; }
         set
         {
            _templateBuildingBlock = value;
            TemplateBuildingBlockId = _templateBuildingBlock != null ? _templateBuildingBlock.Id : string.Empty;
         }
      }

      protected BuildingBlockInfo(ApplicationIcon icon, string typeName)
      {
         _typeName = typeName;
         IconName = icon.IconName;
         SimulationChanges = 0;
      }

      public virtual bool BuildingBlockChanged
      {
         get
         {
            if (BuildingBlock == null || TemplateBuildingBlock == null)
               return false;

            return SimulationHasChanged || BuildingBlock.Version != TemplateBuildingBlock.Version;
         }
      }

      public virtual IBuildingBlock UntypedTemplateBuildingBlock
      {
         get { return TemplateBuildingBlock; }
         set { TemplateBuildingBlock = value.DowncastTo<T>(); }
      }

      public virtual bool SimulationHasChanged
      {
         get { return SimulationChanges > 0; }
      }

      public virtual IBuildingBlock UntypedBuildingBlock
      {
         get { return BuildingBlock; }
         set { BuildingBlock = value.DowncastTo<T>(); }
      }

      public string Name
      {
         get { return BuildingBlock.Name; }
         set { }
      }

      public override string ToString()
      {
         return $"{_typeName}:'{Name}'";
      }

      public bool BuildingBlockIsTemplate
      {
         get
         {
            if (BuildingBlock == null)
               return true;

            return Equals(TemplateBuildingBlock, BuildingBlock);
         }
      }
   }

   public class SpatialStructureInfo : BuildingBlockInfo<IMoBiSpatialStructure>
   {
      public SpatialStructureInfo() : base(ApplicationIcons.SpatialStructure, ObjectTypes.SpatialStructure)
      {
      }
   }

   public class MoleculesInfo : BuildingBlockInfo<IMoleculeBuildingBlock>
   {
      public MoleculesInfo() : base(ApplicationIcons.Molecule, ObjectTypes.MoleculeBuildingBlock)
      {
      }
   }

   public class ReactionBuildingBlockInfo : BuildingBlockInfo<IMoBiReactionBuildingBlock>
   {
      public ReactionBuildingBlockInfo() : base(ApplicationIcons.Reaction, ObjectTypes.ReactionBuildingBlock)
      {
      }
   }

   public class PassiveTransportBuildingBlockInfo : BuildingBlockInfo<IPassiveTransportBuildingBlock>
   {
      public PassiveTransportBuildingBlockInfo() : base(ApplicationIcons.PassiveTransport, ObjectTypes.PassiveTransportBuildingBlock)
      {
      }
   }

   public class ObserverBuildingBlockInfo : BuildingBlockInfo<IObserverBuildingBlock>
   {
      public ObserverBuildingBlockInfo() : base(ApplicationIcons.Observer, ObjectTypes.ObserverBuildingBlock)
      {
      }
   }

   public class EventGroupBuildingBlockInfo : BuildingBlockInfo<IEventGroupBuildingBlock>
   {
      public EventGroupBuildingBlockInfo() : base(ApplicationIcons.Event, ObjectTypes.EventGroupBuildingBlock)
      {
      }
   }

   public class ParameterStartValuesBuildingBlockInfo : BuildingBlockInfo<IParameterStartValuesBuildingBlock>
   {
      public ParameterStartValuesBuildingBlockInfo() : base(ApplicationIcons.ParameterStartValues, ObjectTypes.ParameterStartValuesBuildingBlock)
      {
      }
   }

   public class MoleculeStartValuesBuildingBlockInfo : BuildingBlockInfo<IMoleculeStartValuesBuildingBlock>
   {
      public MoleculeStartValuesBuildingBlockInfo() : base(ApplicationIcons.MoleculeStartValues, ObjectTypes.MoleculeStartValuesBuildingBlock)
      {
      }
   }

   public class SimulationSettingsBuildingBlockInfo : BuildingBlockInfo<ISimulationSettings>
   {
      public SimulationSettingsBuildingBlockInfo() : base(ApplicationIcons.SimulationSettings, ObjectTypes.SimulationSettings)
      {
      }
   }
}