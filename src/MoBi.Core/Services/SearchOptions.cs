namespace MoBi.Core.Services
{
   public class SearchOptions
   {
      public bool WholeWord{ set; get; }
      public bool RegEx{ set; get; }
      public bool CaseSensitive { set; get; }
      public SearchScope Scope { set; get; }

      public string Expression { get; set; }
   }

   public enum SearchScope
   {
      Project,
      AllOfSameType,
      Local
   }
}