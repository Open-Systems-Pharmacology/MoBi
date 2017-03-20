using OSPSuite.Utility.Validation;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.DTO;

namespace MoBi.BatchTool.Presenters
{
   public class FileFromFolderDTO : ValidatableDTO
   {
      private string _inputFolder;

      public string InputFolder
      {
         get { return _inputFolder; }
         set
         {
            _inputFolder = value;
            OnPropertyChanged(() => InputFolder);
         }
      }

      public FileFromFolderDTO()
      {
         Rules.Add(AllRules.InputFolderExists);
      }

      private static class AllRules
      {
         public static IBusinessRule InputFolderExists
         {
            get { return GenericRules.NonEmptyRule<FileFromFolderDTO>(x => x.InputFolder); }
         }
      }
   }
}