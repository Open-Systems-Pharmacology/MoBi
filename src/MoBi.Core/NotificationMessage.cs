using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Assets;

namespace MoBi.Core
{
   public class NotificationMessage : ObjectBase
   {
      private string _message;
      private readonly List<string> _details;
      public NotificationType Type { get; }

      /// <summary>
      ///    Building block containing the object for which the message was created
      /// </summary>
      public IBuildingBlock BuildingBlock { get; }

      /// <summary>
      ///    Type of the object for which the notification was created
      /// </summary>
      public string ObjectType { get; set; }

      /// <summary>
      ///    Type of the building block containing the object for which the notification was created
      /// </summary>
      public string BuildingBlockType { get; set; }

      /// <summary>
      ///    Specifies where the message comes from (Simulation Creation, formulation validation etc..)
      /// </summary>
      public MessageOrigin MessageOrigin { get; }

      /// <summary>
      ///    Object for which the message was initiated. Typically a builder object
      /// </summary>
      public IObjectBase Object { get; }

      public NotificationMessage(IObjectBase objectBase, MessageOrigin messageOrigin, IBuildingBlock buildingBlock, NotificationType notificationType)
      {
         MessageOrigin = messageOrigin;
         BuildingBlock = buildingBlock;
         Object = objectBase;
         _details = new List<string>();
         Type = notificationType;
         Id = $"{messageOrigin}{objectBase.Id}{notificationType}";
      }

      /// <summary>
      ///    Returns the name of the object for which the notification was created
      /// </summary>
      public string ObjectName => Object.Name;

      /// <summary>
      ///    Returns the name of the building block containing the object for which the notification was created
      /// </summary>
      public string BuildingBlockName
      {
         get
         {
            if (BuildingBlock != null)
               return BuildingBlock.Name;
            return string.Empty;
         }
      }

      //Description of error
      public string Message
      {
         get => _message;
         set
         {
            _message = value;
            OnPropertyChanged(() => Message);
         }
      }

      public void AddDetails(IEnumerable<string> details) => _details.AddRange(details);

      public IEnumerable<string> Details => _details;

      public ApplicationIcon Image
      {
         get
         {
            switch (Type)
            {
               case NotificationType.Error:
                  return ApplicationIcons.Error;
               case NotificationType.Warning:
                  return ApplicationIcons.Warning;
               case NotificationType.Info:
                  return ApplicationIcons.Info;
               default:
                  return ApplicationIcons.Info;
            }
         }
      }

      public override string ToString()
      {
         return _message;
      }
   }
}