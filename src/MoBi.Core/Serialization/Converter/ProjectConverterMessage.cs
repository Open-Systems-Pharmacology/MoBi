namespace MoBi.Core.Serialization.Converter
{
   public enum ConverterMessageType
   {
      Info,
      Warning,
      Error
   }

   public class ProjectConverterMessage
   {
      public ConverterMessageType MessageType { get; set; }
      public string Message { get; set; }

      public ProjectConverterMessage(ConverterMessageType messageType, string message)
      {
         MessageType = messageType;
         Message = message;
      }
   }
}