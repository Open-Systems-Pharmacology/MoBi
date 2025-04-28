using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.R.MinimalImplementations
{
   public class RDimensionFactory : DimensionFactory
   {
      protected override IDimensionConverter CreateConverterFor<T>(IDimension sourceDimension, IDimension targetDimension, T hasDimension)
      {
         switch (hasDimension)
         {
            case DoubleArrayContext doubleArrayContext:
               return createDoubleArrayConverter(doubleArrayContext, sourceDimension, targetDimension);
            case QuantityPKParameterContext quantityPKParameterContext:
               return createQuantityPKParameterConverter(quantityPKParameterContext, sourceDimension, targetDimension);
            default:
               return null;
         }
      }

      private IDimensionConverter createDoubleArrayConverter(DoubleArrayContext doubleArrayContext, IDimension sourceDimension,
         IDimension targetDimension)
      {
         switch (sourceDimension.Name)
         {
            case Constants.Dimension.MOLAR_AMOUNT:
            case Constants.Dimension.MOLAR_CONCENTRATION:
            case Constants.Dimension.MOLAR_AUC:
               return new DoubleArrayMolarToMassConverter(doubleArrayContext, sourceDimension, targetDimension);
            case Constants.Dimension.MASS_AMOUNT:
            case Constants.Dimension.MASS_CONCENTRATION:
            case Constants.Dimension.MASS_AUC:
               return new DoubleArrayMassToMolarConverter(doubleArrayContext, sourceDimension, targetDimension);
         }

         return null;
      }

      private IDimensionConverter createQuantityPKParameterConverter(QuantityPKParameterContext quantityPKParameterContext, IDimension sourceDimension,
         IDimension targetDimension)
      {
         switch (sourceDimension.Name)
         {
            case Constants.Dimension.MOLAR_AMOUNT:
            case Constants.Dimension.MOLAR_CONCENTRATION:
            case Constants.Dimension.MOLAR_AUC:
               return new QuantityPKParameterMolarToMassConverter(quantityPKParameterContext, sourceDimension, targetDimension);
            case Constants.Dimension.MASS_AMOUNT:
            case Constants.Dimension.MASS_CONCENTRATION:
            case Constants.Dimension.MASS_AUC:
               return new QuantityPKParameterMassToMolarConverter(quantityPKParameterContext, sourceDimension, targetDimension);
         }

         return null;
      }
   }

   public class DoubleArrayContext : IWithDimension
   {
      public IDimension Dimension { get; set; }
      public double? MolWeight { get; set; }

      public DoubleArrayContext(IDimension dimension, double? molWeight)
      {
         Dimension = dimension;
         MolWeight = molWeight;
      }
   }

   public class DoubleArrayMolarToMassConverter : MolarToMassDimensionConverter<DoubleArrayContext>
   {
      public DoubleArrayMolarToMassConverter(DoubleArrayContext context, IDimension molarDimension, IDimension massDimension) : base(molarDimension,
         massDimension, context, x => x.MolWeight)
      {
      }
   }

   public class DoubleArrayMassToMolarConverter : MassToMolarDimensionConverter<DoubleArrayContext>
   {
      public DoubleArrayMassToMolarConverter(DoubleArrayContext context, IDimension massDimension, IDimension molarDimension) : base(massDimension,
         molarDimension, context, x => x.MolWeight)
      {
      }
   }
}