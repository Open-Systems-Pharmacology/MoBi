using OSPSuite.Core.Domain.Services;
using static OSPSuite.Core.CoreConstants.ContainerName;
using static OSPSuite.Core.CoreConstants.ExpressionTypeNames;

namespace OSPSuite.Core.Domain.Builder
{
   // On promotion to core, StartValueBuildingBlock, StartValueBase and PathAndValueEntity should get a refactoring
   // This building block uses much of the same features as StartValueBB, but is not really a 'StartValue' kind of bb
   public class ExpressionProfileBuildingBlock : StartValueBuildingBlock<ExpressionParameter>, IExpressionProfileBuildingBlock
   {
      public override string Icon => iconNameFor(Type);

      private string iconNameFor(ExpressionType type)
      {
         switch (type)
         {
            case ExpressionType.TransportProtein:
               return Transporter;
            case ExpressionType.ProteinBindingPartner:
               return Protein;
            case ExpressionType.MetabolizingEnzyme:
               return Enzyme;
         }

         return string.Empty;
      }

      public virtual string MoleculeName { get; private set; }

      public string Species { get; private set; }

      public ExpressionType Type { set; get; }

      public int PKSimVersion { set; get; }

      public virtual string Category { get; private set; }

      public override string Name
      {
         get => ExpressionProfileName(MoleculeName, Species, Category);
         set
         {
            if (string.Equals(Name, value))
            {
               return;
            }

            var (moleculeName, species, category) = NamesFromExpressionProfileName(value);
            if (string.IsNullOrEmpty(moleculeName))
               return;

            Species = species;
            Category = category;
            MoleculeName = moleculeName;
         }
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         var sourceExpressionProfile = source as ExpressionProfileBuildingBlock;
         if (sourceExpressionProfile == null) return;

         Type = sourceExpressionProfile.Type;
         PKSimVersion = sourceExpressionProfile.PKSimVersion;
         Name = sourceExpressionProfile.Name;
      }
   }

   public interface IExpressionProfileBuildingBlock : IBuildingBlock
   {
   }

   public enum ExpressionType
   {
      TransportProtein,
      MetabolizingEnzyme,
      ProteinBindingPartner
   }
}
