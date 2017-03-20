namespace MoBi.Presentation.DTO
{
   public class TagDTO
   {
      public string Value { get; set; }

      public TagDTO(string tag)
      {
         Value = tag;
      }
   }
}