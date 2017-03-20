namespace MoBi.Presentation.DTO
{
   public class ListItemDTO<T>
   {
      public string DisplayName { set; get; }
      public T Item { set; get; }

      public override string ToString()
      {
         return DisplayName;
      }

   }
}