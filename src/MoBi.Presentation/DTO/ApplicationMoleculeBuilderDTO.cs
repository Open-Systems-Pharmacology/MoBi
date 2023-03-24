using MoBi.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation.DTO
{
   public class ApplicationMoleculeBuilderDTO : ObjectBaseDTO
   {
      public FormulaBuilderDTO Formula { get; set; }

      public ApplicationMoleculeBuilderDTO(IApplicationMoleculeBuilder applicationMoleculeBuilder) : base(applicationMoleculeBuilder)
      {
         Rules.Add(createRelativeContainerPathNotEmptyRule);
      }

      private static IBusinessRule createRelativeContainerPathNotEmptyRule { get; } = CreateRule.For<ApplicationMoleculeBuilderDTO>()
         .Property(x => x.RelativeContainerPath)
         .WithRule((dto, path) => !path.IsNullOrEmpty())
         .WithError(AppConstants.Validation.RelativeContainerPathNotSet);

      private string _relativeContainerPath;

      public virtual string RelativeContainerPath
      {
         get => _relativeContainerPath;
         set => SetProperty(ref _relativeContainerPath, value);
      }
   }
}