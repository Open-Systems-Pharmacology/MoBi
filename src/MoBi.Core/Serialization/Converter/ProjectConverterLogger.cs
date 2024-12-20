using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Serialization.Converter
{
   public interface IProjectConverterLogger
   {
      /// <summary>
      ///    Clear all previously defined messages
      /// </summary>
      void Clear();

      void AddMessage(NotificationMessage projectConverterMessage);
      void AddMessage(NotificationType type, string message, IObjectBase objectBase, IBuildingBlock buildingBlock);

      void AddError(string error, IObjectBase objectBase, IBuildingBlock buildingBlock);

      void AddInfo(string info, IObjectBase objectBase, IBuildingBlock buildingBlock);

      void AddWarning(string warning, IObjectBase objectBase, IBuildingBlock buildingBlock);
      IList<NotificationMessage> AllMessages();

      void AddSubMessage(string message, IBuildingBlock buildingBlock);
   }

   public class ProjectConverterLogger : IProjectConverterLogger
   {
      private readonly List<NotificationMessage> _messages;
      private readonly IObjectTypeResolver _objectTypeResolver;

      public ProjectConverterLogger(IObjectTypeResolver objectTypeResolver)
      {
         _objectTypeResolver = objectTypeResolver;
         _messages = new List<NotificationMessage>();
      }

      public void Clear()
      {
         _messages.Clear();
      }

      public bool ContainsError()
      {
         return _messages.Any(m => m.Type.Equals(NotificationType.Error));
      }

      public void AddMessage(NotificationMessage projectConverterMessage)
      {
         _messages.Add(projectConverterMessage);
      }

      public void AddMessage(NotificationType type, string message, IObjectBase objectBase, IBuildingBlock buildingBlock)
      {
         AddMessage(new NotificationMessage(objectBase, MessageOrigin.Formula, buildingBlock, type)
            {
               Message = message,
               BuildingBlockType = buildingBlock == null ? string.Empty : _objectTypeResolver.TypeFor(buildingBlock),
               ObjectType = _objectTypeResolver.TypeFor(objectBase),
               Name = objectBase.Name
            });
      }

      public void AddError(string error, IObjectBase objectBase, IBuildingBlock buildingBlock)
      {
         AddMessage(NotificationType.Error, error, objectBase, buildingBlock);
      }

      public void AddInfo(string info, IObjectBase objectBase, IBuildingBlock buildingBlock)
      {
         AddMessage(NotificationType.Info, info, objectBase, buildingBlock);
      }

      public void AddWarning(string warning, IObjectBase objectBase, IBuildingBlock buildingBlock)
      {
         AddMessage(NotificationType.Warning, warning, objectBase, buildingBlock);
      }

      public void AddSubMessage(string message, IBuildingBlock buildingBlock)
      {
         var mainMessage = _messages.FirstOrDefault(m => m.BuildingBlock.Equals(buildingBlock));
         mainMessage.AddDetails(new string[] {message});
      }

      public IList<NotificationMessage> AllMessages()
      {
         return _messages;
      }

      public IList<NotificationMessage> ErrorMessages
      {
         get { return _messages.Where(m => m.Type.Equals(NotificationType.Error)).ToList(); }
      }
   }
}