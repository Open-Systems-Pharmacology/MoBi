using System.Collections.Generic;
using MoBi.Core.Domain.Model;
using OSPSuite.Core;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Events
{
   public class SimulationRunFinishedEvent
   {
      public IMoBiSimulation Simulation { get; set; }

      public SimulationRunFinishedEvent(IMoBiSimulation simulation)
      {
         Simulation = simulation;
      }
   }

   public class SimulationRunStartedEvent
   {
   }

   public class SimulationAddedEvent
   {
      public SimulationAddedEvent(IMoBiSimulation simulation)
      {
         Simulation = simulation;
      }

      public IMoBiSimulation Simulation { get; }
   }

   public class EntitySelectedEvent
   {
      public IObjectBase ObjectBase { get; set; }
      public object Sender { get; set; }

      public EntitySelectedEvent(IObjectBase objectBase, object sender)
      {
         ObjectBase = objectBase;
         Sender = sender;
      }
   }

   public class NeighborhoodChangedEvent
   {
      public NeighborhoodBuilder NeighborhoodBuilder { get; }

      public NeighborhoodChangedEvent(NeighborhoodBuilder neighborhoodBuilder)
      {
         NeighborhoodBuilder = neighborhoodBuilder;
      }
   }

   public class DefaultSimulationSettingsUpdatedEvent
   {
      public SimulationSettings NewSimulationSettings { get; }

      public DefaultSimulationSettingsUpdatedEvent(SimulationSettings newSimulationSettings)
      {
         NewSimulationSettings = newSimulationSettings;
      }
   }

   public abstract class AddedEvent
   {
      protected AddedEvent(IObjectBase addedObject, IObjectBase parent)
      {
         AddedObject = addedObject;
         Parent = parent;
      }

      public IObjectBase AddedObject { get; }
      public IObjectBase Parent { get; }
   }

   public class AddedEvent<T> : AddedEvent where T : IObjectBase
   {
      public AddedEvent(T addedObject, IObjectBase parent) : base(addedObject, parent)
      {
      }

      public new T AddedObject => base.AddedObject.DowncastTo<T>();
   }

   public class ObjectConvertedEvent
   {
      public object ConvertedObject { get; }
      public ProjectVersion FromVersion { get; }

      public ObjectConvertedEvent(object convertedObject, ProjectVersion fromVersion)
      {
         ConvertedObject = convertedObject;
         FromVersion = fromVersion;
      }
   }

   public class RemovedEvent
   {
      public IEnumerable<IObjectBase> RemovedObjects { get; }

      // Parent  is only available if only one object was removed
      public IObjectBase Parent { get; }

      public RemovedEvent(IEnumerable<IObjectBase> removedObjects)
      {
         RemovedObjects = removedObjects;
      }

      public RemovedEvent(IObjectBase removedObject, IObjectBase parent)
         : this(new[] { removedObject })
      {
         Parent = parent;
      }
   }

   public class AddedReactionPartnerEvent
   {
      public ReactionPartnerBuilder ReactionPartnerBuilder { get; set; }
      public ReactionBuilder Reaction { get; set; }

      public AddedReactionPartnerEvent(ReactionPartnerBuilder value, ReactionBuilder reaction)
      {
         ReactionPartnerBuilder = value;
         Reaction = reaction;
      }
   }

   public class RemovedReactionPartnerEvent
   {
      public ReactionPartnerBuilder ReactionPartnerBuilder { get; set; }
      public ReactionBuilder Reaction { get; set; }

      public RemovedReactionPartnerEvent(ReactionPartnerBuilder reactionPartnerBuilder, ReactionBuilder reaction)
      {
         ReactionPartnerBuilder = reactionPartnerBuilder;
         Reaction = reaction;
      }
   }

   public class ShowValidationResultsEvent
   {
      public ValidationResult ValidationResult { get; }

      public ShowValidationResultsEvent(ValidationResult validationResult)
      {
         ValidationResult = validationResult;
      }
   }

   public class FormulaValidEvent
   {
      public IFormula Formula { get; }
      public IBuildingBlock BuildingBlock { get; }

      public FormulaValidEvent(IFormula formula, IBuildingBlock buildingBlock)
      {
         Formula = formula;
         BuildingBlock = buildingBlock;
      }
   }

   public class FormulaInvalidEvent
   {
      public IFormula Formula { get; }
      public string Message { get; }
      public IBuildingBlock BuildingBlock { get; }

      public FormulaInvalidEvent(IFormula formula, IBuildingBlock buildingBlock, string message)
      {
         Formula = formula;
         BuildingBlock = buildingBlock;
         Message = message;
      }
   }

   public class ClearNotificationsEvent
   {
      public MessageOrigin MessageOrigin { get; }

      public ClearNotificationsEvent(MessageOrigin messageOrigin)
      {
         MessageOrigin = messageOrigin;
      }
   }

   public class ShowNotificationsEvent
   {
      public IReadOnlyList<NotificationMessage> NotificationMessages { get; }

      public ShowNotificationsEvent(NotificationMessage notification) : this(new[] { notification })
      {
      }

      public ShowNotificationsEvent(IReadOnlyList<NotificationMessage> notificationMessages)
      {
         NotificationMessages = notificationMessages;
      }
   }

   public class ModuleStatusChangedEvent
   {
      public Module Module { get; }

      public ModuleStatusChangedEvent(Module module)
      {
         Module = module;
      }
   }

   public class SimulationStatusChangedEvent
   {
      public IMoBiSimulation Simulation { get; }

      public SimulationStatusChangedEvent(IMoBiSimulation simulation)
      {
         Simulation = simulation;
      }
   }

   public class SimulationReloadEvent
   {
      public IMoBiSimulation Simulation { get; }

      public SimulationReloadEvent(IMoBiSimulation simulation)
      {
         Simulation = simulation;
      }
   }

   public class SimulationUnloadEvent
   {
      public IMoBiSimulation Simulation { get; }

      public SimulationUnloadEvent(IMoBiSimulation simulation)
      {
         Simulation = simulation;
      }
   }

   public abstract class TagConditionEvent
   {
      public IObjectBase TaggedObject { get; }

      protected TagConditionEvent(IObjectBase taggedObject)
      {
         TaggedObject = taggedObject;
      }
   }

   public class AddTagConditionEvent : TagConditionEvent
   {
      public AddTagConditionEvent(IObjectBase taggedObject) : base(taggedObject)
      {
      }
   }

   public class RemoveTagConditionEvent : TagConditionEvent
   {
      public RemoveTagConditionEvent(IObjectBase taggedObject) : base(taggedObject)
      {
      }
   }

   public class ChartAddedEvent
   {
      public CurveChart Chart { get; }

      public ChartAddedEvent(CurveChart chart)
      {
         Chart = chart;
      }
   }

   public class ChartDeletedEvent
   {
      public CurveChart Chart { get; }

      public ChartDeletedEvent(CurveChart chart)
      {
         Chart = chart;
      }
   }

   public class RemovedDataEvent
   {
      public DataRepository Repository { get; set; }

      public RemovedDataEvent(DataRepository repository)
      {
         Repository = repository;
      }
   }
}