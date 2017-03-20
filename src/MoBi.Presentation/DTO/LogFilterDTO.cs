namespace MoBi.Presentation.DTO
{
   public class LogFilterDTO
   {
      public bool Error { get; set; }
      public bool Warning { get; set; }

      public LogFilterDTO()
      {
         Error = false;
         Warning = true;
      }
   }
}