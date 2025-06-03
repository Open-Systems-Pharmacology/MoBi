using MoBi.Core.Domain.UnitSystem;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.HelpersForTests
{
   public static class DimensionFactoryForSpecs
   {
      public static IMoBiDimensionFactory Factory { get; } = generateFactory();

      private static IMoBiDimensionFactory generateFactory()
      {
         var factory = new MoBiDimensionFactory();

         var massDimension = new Dimension(new BaseDimensionRepresentation(), DimensionNames.Mass, "g");
         massDimension.AddUnit(new Unit("kg", 1000, 0));
         massDimension.AddUnit(new Unit("mg", 0.001, 0));

         var concentrationDimension = new Dimension(new BaseDimensionRepresentation(), DimensionNames.Concentration, "mol");

         factory.AddDimension(massDimension);
         factory.AddDimension(concentrationDimension);
         factory.AddDimension(new Dimension(new BaseDimensionRepresentation(), DimensionNames.Time, "s"));

         return factory;
      }


      public static IDimension MassDimension => Factory.Dimension(DimensionNames.Mass);
      public static IDimension TimeDimension => Factory.Dimension(DimensionNames.Time);

      public static class DimensionNames
      {
         public static string Mass = "Mass";
         public static string Concentration = "Concentration";
         public static string Time = Constants.Dimension.TIME;
      }
   }
}