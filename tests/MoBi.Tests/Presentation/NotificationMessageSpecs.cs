using System.Drawing;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core;

using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Assets;

namespace MoBi.Presentation
{
   public abstract class concern_for_NotificationMessage : ContextSpecification<NotificationMessage>
   {
      protected IObjectBase _objectBase;
      protected IBuildingBlock _buildingBlock;

      protected override void Context()
      {
         _objectBase = A.Fake<IObjectBase>().WithId("TOTO");
         _buildingBlock = A.Fake<IBuildingBlock>().WithName("BB");
         sut = new NotificationMessage(_objectBase, MessageOrigin.Formula,_buildingBlock,NotificationType.Error);
      }
   }

   public class When_asking_the_notification_message_for_the_representing_image : concern_for_NotificationMessage
   {
      [Observation]
      public void should_return_the_error_icon_for_an_error_message()
      {
         imageShouldBe(ApplicationIcons.Error);
      }

      [Observation]
      public void should_return_the_warning_icon_for_a_warning_message()
      {
         sut = new NotificationMessage(_objectBase, MessageOrigin.Formula, _buildingBlock, NotificationType.Warning);
         imageShouldBe(ApplicationIcons.Warning);
      }

      [Observation]
      public void should_return_the_message_icon_for_a_message_message()
      {
         sut = new NotificationMessage(_objectBase, MessageOrigin.Formula, _buildingBlock, NotificationType.Info);
         imageShouldBe(ApplicationIcons.Info);
      }

      private void imageShouldBe(Image image)
      {
         var image1 = new Bitmap(sut.Image);
         var image2 = new Bitmap(image);

         for (int i = 0; i < image1.Width; i++)
         {
            for (int j = 0; j < image1.Height; j++)
            {
               var pix1 = image1.GetPixel(i, j).ToString();
               var pix2 = image2.GetPixel(i, j).ToString();
               pix1.ShouldBeEqualTo(pix2);
            }
         }
      }
   }

   public class When_checking_if_two_notication_messages_are_equal : concern_for_NotificationMessage
   {
      [Observation]
      public void should_return_false_if_one_of_them_is_null()
      {
         Equals(sut, null).ShouldBeFalse();
         Equals(null, sut).ShouldBeFalse();
      }

      [Observation]
      public void should_return_false_if_the_notification_are_for_the_same_object_but_with_different_origins()
      {
         var another = new NotificationMessage(_objectBase, MessageOrigin.Simulation, A.Fake<IBuildingBlock>(), NotificationType.Error);
         Equals(sut, another).ShouldBeFalse();
         Equals(another, sut).ShouldBeFalse();
      }

      [Observation]
      public void should_return_false_if_the_notification_are_for_different_objects_with_the_same_origin()
      {
         var another = new NotificationMessage(A.Fake<IObjectBase>().WithId("TITI"), MessageOrigin.Formula, A.Fake<IBuildingBlock>(),NotificationType.Error);
         Equals(sut, another).ShouldBeFalse();
         Equals(another, sut).ShouldBeFalse();
      }

      [Observation]
      public void should_return_true_if_the_notification_are_for_the_same_objects_with_the_same_origin()
      {
         var another = new NotificationMessage(_objectBase, MessageOrigin.Formula, A.Fake<IBuildingBlock>(), NotificationType.Error);
         Equals(sut, another).ShouldBeTrue();
         Equals(another, sut).ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_if_the_notification_are_for_the_same_objects_with_the_same_origin_but_different_notification_type()
      {
         var another = new NotificationMessage(_objectBase, MessageOrigin.Formula, A.Fake<IBuildingBlock>(), NotificationType.Info);
         Equals(sut, another).ShouldBeFalse();
         Equals(another, sut).ShouldBeFalse();
      }
   }
}