using MoBi.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation.DTO
{
   public class ApplicationMoleculeBuilderDTO : ObjectBaseDTO
   {
      public FormulaBuilderDTO Formula { get; set; }

      public ApplicationMoleculeBuilderDTO()
      {
         Rules.Add(createRelativeContainerPathNotEmptyRule());
      }

      private IBusinessRule createRelativeContainerPathNotEmptyRule()
      {
         return CreateRule.For<ApplicationMoleculeBuilderDTO>()
            .Property(x => x.RelativeContainerPath)
            .WithRule((dto, path) => !path.IsNullOrEmpty())
            .WithError(AppConstants.Validation.RelativeContainerPathNotSet);
      }

      private string _relativeContainerPath;

      public string RelativeContainerPath
      {
         get { return _relativeContainerPath; }
         set
         {
            _relativeContainerPath = value;
            OnPropertyChanged(() => RelativeContainerPath);
         }
      }
   }
}