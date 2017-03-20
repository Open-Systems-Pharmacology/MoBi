using System.Collections.Generic;
using System.IO;
using MoBi.Assets;
using OSPSuite.Utility;
using OSPSuite.Utility.Validation;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.DTO;

namespace MoBi.Presentation.DTO
{
   /// <summary>
   ///    Augments ImportFileSelectionDTO by adding a SelectedSheet property
   /// </summary>
   public class ImportExcelSheetSelectionDTO : ImportFileSelectionDTO
   {
      public IReadOnlyCollection<string> AllSheetNames { get; set; }

      private string _sheet;

      public ImportExcelSheetSelectionDTO()
      {
         AllSheetNames = new List<string>();
         Rules.Add(AllRules.FileIsExcelFile);
      }

      public string SelectedSheet
      {
         get { return _sheet; }
         set
         {
            _sheet = value;
            OnPropertyChanged(() => SelectedSheet);
         }
      }

      private static class AllRules
      {
         public static IBusinessRule FileIsExcelFile
         {
            get
            {
               return CreateRule.For<ImportFileSelectionDTO>()
                  .Property((x => x.FilePath))
                  .WithRule((dto, path) =>
                  {
                     if (!FileHelper.FileExists(path))
                        return true;

                     return new FileInfo(path).Extension.IsOneOf(Constants.Filter.XLS_EXTENSION, Constants.Filter.XLSX_EXTENSION);
                  })
                  .WithError(AppConstants.Exceptions.FileInNotAnExcelFile);
            }
         }
      }
   }
}
