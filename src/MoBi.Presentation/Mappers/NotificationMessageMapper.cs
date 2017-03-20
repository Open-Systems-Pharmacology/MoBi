using OSPSuite.Utility;
using MoBi.Core;
using MoBi.Core.Services;
using MoBi.Presentation.Presenter.Main;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Mappers
{
   public interface INotificationMessageMapper : IMapper<ValidationMessage, NotificationMessageDTO>
   {
      /// <summary>
      ///    Map the formula to a notification message. The building block is the one containing the formula
      /// </summary>
      NotificationMessageDTO MapFrom(IFormula formula, IBuildingBlock buildingBlock);
   }

   public class NotificationMessageMapper : INotificationMessageMapper
   {
      private readonly IObjectTypeResolver _objectTypeResolver;
      private readonly IBuildingBlockRetriever _buildingBlockRetriever;

      public NotificationMessageMapper(IObjectTypeResolver objectTypeResolver, IBuildingBlockRetriever buildingBlockRetriever)
      {
         _objectTypeResolver = objectTypeResolver;
         _buildingBlockRetriever = buildingBlockRetriever;
      }

      public NotificationMessageDTO MapFrom(IFormula formula, IBuildingBlock buildingBlock)
      {
         return new NotificationMessageDTO(createFor(NotificationType.Error, formula, MessageOrigin.Formula, buildingBlock));
      }

      public NotificationMessageDTO MapFrom(ValidationMessage validationMessage)
      {
         var buildingBlock = _buildingBlockRetriever.GetBuildingBlockFor(validationMessage.Object, validationMessage.BuildingBlock);
         var notification = createFor(validationMessage.NotificationType, validationMessage.Object, MessageOrigin.Simulation, buildingBlock);
         notification.Message = validationMessage.Text;
         notification.AddDetails(validationMessage.Details);
         return new NotificationMessageDTO(notification);
      }

      private NotificationMessage createFor(NotificationType notificationType, IObjectBase objectBase, MessageOrigin origin, IBuildingBlock buildingBlock)
      {
         return new NotificationMessage(objectBase, origin, buildingBlock, notificationType)
         {
            ObjectType = _objectTypeResolver.TypeFor(objectBase),
            BuildingBlockType = _objectTypeResolver.TypeFor(buildingBlock),
         };
      }
   }
}