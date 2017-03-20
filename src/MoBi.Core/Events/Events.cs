using System.Collections.Generic;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using OSPSuite.Core;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Formulas;

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

      public IMoBiSimulation Simulation { get; private set; }
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

   public abstract class AddedEvent
   {
      protected AddedEvent(IObjectBase addedObject, IObjectBase parent)
      {
         AddedObject = addedObject;
         Parent = parent;
      }

      public IObjectBase AddedObject { get; private set; }
      public IObjectBase Parent { get; private set; }
   }

   public class AddedEvent<T> : AddedEvent where T : IObjectBase
   {
      public AddedEvent(T addedObject, IObjectBase parent) : base(addedObject, parent)
      {
      }

      public new T AddedObject
      {
         get { return base.AddedObject.DowncastTo<T>(); }
      }
   }

   public class ObjectConvertedEvent
   {
      public object ConvertedObject { get; private set; }
      public ProjectVersion FromVersion { get; private set; }

      public ObjectConvertedEvent(object convertedObject, ProjectVersion fromVersion)
      {
         ConvertedObject = convertedObject;
         FromVersion = fromVersion;
      }
   }

   public class RemovedEvent
   {
      public IEnumerable<IObjectBase> RemovedObjects { get; private set; }

      // Parent  is only available if only one object was removed
      public IObjectBase Parent { get; private set; }

      public RemovedEvent(IEnumerable<IObjectBase> removedObjects)
      {
         RemovedObjects = removedObjects;
      }

      public RemovedEvent(IObjectBase removedObject, IObjectBase parent)
         : this(new[] {removedObject})
      {
         Parent = parent;
      }
   }

   public class AddedReactionPartnerEvent
   {
      public IReactionPartnerBuilder ReactionPartnerBuilder { get; set; }
      public IReactionBuilder Reaction { get; set; }

      public AddedReactionPartnerEvent(IReactionPartnerBuilder value, IReactionBuilder reaction)
      {
         ReactionPartnerBuilder = value;
         Reaction = reaction;
      }
   }

   public class RemovedReactionPartnerEvent
   {
      public IReactionPartnerBuilder ReactionPartnerBuilder { get; set; }
      public IReactionBuilder Reaction { get; set; }

      public RemovedReactionPartnerEvent(IReactionPartnerBuilder reactionPartnerBuilder, IReactionBuilder reaction)
      {
         ReactionPartnerBuilder = reactionPartnerBuilder;
         Reaction = reaction;
      }
   }

   public class ShowValidationResultsEvent
   {
      public ValidationResult ValidationResult { get; private set; }

      public ShowValidationResultsEvent(ValidationResult validationResult)
      {
         ValidationResult = validationResult;
      }
   }

   public class FormulaValidEvent
   {
      public IFormula Formula { get; private set; }
      public IBuildingBlock BuildingBlock { get; private set; }

      public FormulaValidEvent(IFormula formula, IBuildingBlock buildingBlock)
      {
         Formula = formula;
         BuildingBlock = buildingBlock;
      }
   }

   public class FormulaInvalidEvent
   {
      public IFormula Formula { get; private set; }
      public string Message { get; private set; }
      public IBuildingBlock BuildingBlock { get; private set; }

      public FormulaInvalidEvent(IFormula formula, IBuildingBlock buildingBlock, string message)
      {
         Formula = formula;
         BuildingBlock = buildingBlock;
         Message = message;
      }
   }

   public class ShowNotificationsEvent
   {
      public IReadOnlyList<NotificationMessage> NotificationMessages { get; private set; }

      public ShowNotificationsEvent(NotificationMessage notification) : this(new[] {notification})
      {
      }

      public ShowNotificationsEvent(IReadOnlyList<NotificationMessage> notificationMessages)
      {
         NotificationMessages = notificationMessages;
      }
   }

   /// <summary>
   /// </summary>
   public class SimulationStatusChangedEvent
   {
      public IMoBiSimulation Simulation { private set; get; }

      public SimulationStatusChangedEvent(IMoBiSimulation simulation)
      {
         Simulation = simulation;
      }
   }

   public class SimulationReloadEvent
   {
      public IMoBiSimulation Simulation { private set; get; }

      public SimulationReloadEvent(IMoBiSimulation simulation)
      {
         Simulation = simulation;
      }
   }

   public class SimulationUnloadEvent
   {
      public IMoBiSimulation Simulation { private set; get; }

      public SimulationUnloadEvent(IMoBiSimulation simulation)
      {
         Simulation = simulation;
      }
   }

   public abstract class TagConditionEvent
   {
      public IObjectBase TaggedObject { get; private set; }

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
      public ICurveChart Chart { get; private set; }

      public ChartAddedEvent(ICurveChart chart)
      {
         Chart = chart;
      }
   }

   public class ChartDeletedEvent
   {
      public ICurveChart Chart { get; private set; }

      public ChartDeletedEvent(ICurveChart chart)
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