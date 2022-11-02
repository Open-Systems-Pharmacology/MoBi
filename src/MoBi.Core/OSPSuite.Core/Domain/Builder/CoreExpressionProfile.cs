using System.Collections;
using OSPSuite.Utility.Collections;
using System.Collections.Generic;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;
using static OSPSuite.Core.CoreConstants.ContainerName;

namespace OSPSuite.Core.Domain.Builder
{
   public class CoreExpressionProfile : BuildingBlock, IEnumerable<ExpressionParameter>
   {
      private readonly ICache<IObjectPath, ExpressionParameter> _allValues;
      public CoreExpressionProfile()
      {
         _allValues = new Cache<IObjectPath, ExpressionParameter>(x => x.Path, x => null);
      }

      public virtual string MoleculeName { set; get; }

      public string Species { set; get; }

      public ExpressionType Type { set; get; }

      public int PKSimVersion { set; get; }

      public virtual string Category { set; get; }

      public override string Name
      {
         get => ExpressionProfileName(MoleculeName, Species, Category);
         set
         {
            if (string.Equals(Name, value))
            {
               return;
            }

            var (moleculeName, _, category) = NamesFromExpressionProfileName(value);
            if (string.IsNullOrEmpty(moleculeName))
               return;

            Category = category;
         }
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         var sourceExpressionProfile = source as CoreExpressionProfile;
         if (sourceExpressionProfile == null) return;

         sourceExpressionProfile.Each(value => Add(cloneManager.Clone(value)));
         MoleculeName = sourceExpressionProfile.MoleculeName;
         Species = sourceExpressionProfile.Species;
         Type = sourceExpressionProfile.Type;
         PKSimVersion = sourceExpressionProfile.PKSimVersion;
      }

      public void Add(ExpressionParameter startValue)
      {
         _allValues.Add(startValue);
      }

      public void Remove(ExpressionParameter newValue)
      {
         if (newValue == null) 
            return;
         _allValues.Remove(newValue.Path);
      }

      public IEnumerator<ExpressionParameter> GetEnumerator()
      {
         return _allValues.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }
   }

   public enum ExpressionType
   {
      TransportProtein,
      MetabolizingEnzyme,
      ProteinBindingPartner
   }
}
