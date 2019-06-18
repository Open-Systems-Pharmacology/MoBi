using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Domain.UnitSystem
{
   public interface IMoBiDimensionFactory : IDimensionFactory
   {
      IDimensionFactory BaseFactory { get; }
      IDimensionFactory ProjectFactory { get; set; }

      /// <summary>
      ///    Create the RHS Dimension for the given dimension and add it to the project factory
      ///    if not available yet
      /// </summary>
      IDimension RHSDimensionFor(IDimension dimension);

      /// <summary>
      ///    Retrieve the dimension given a unit string
      /// </summary>
      /// <param name="unitName">The unit string that will be searched for. For example 'mg' or 's'</param>
      /// <returns>The dimension associated with the unit name</returns>
      IDimension DimensionForUnit(string unitName);

      /// <summary>
      ///    Returns the <see cref="IDimension" /> named <paramref name="dimensionName" /> or the default <c>NO_DIMENSION</c> if
      ///    not found
      /// </summary>
      IDimension TryGetDimension(string dimensionName);

      IDimension TryGetDimensionCaseInsensitive(string dimensionName);
   }

   public class MoBiDimensionFactory : IMoBiDimensionFactory
   {
      private IDimensionFactory _projectFactory;
      public IDimensionFactory BaseFactory { get; }

      public IDimensionFactory ProjectFactory
      {
         get => _projectFactory;
         set => _projectFactory = value ?? new MoBiMergedDimensionFactory();
      }

      public IDimension DimensionForUnit(string unitName)
      {
         var matches = Dimensions.Where(dimension => dimension.Units.Any(unit => unitName.Equals(unit.Name, StringComparison.OrdinalIgnoreCase))).ToList();

         if (!matches.Any())
            return null;

         return matches.Count() > 1
            ? matches.FirstOrDefault(dimension => dimension.Units.Any(unit => unitName.Equals(unit.Name, StringComparison.Ordinal)))
            : matches.First();
      }

      public MoBiDimensionFactory()
      {
         BaseFactory = new MoBiMergedDimensionFactory();
         ProjectFactory = new MoBiMergedDimensionFactory();
      }

      public IDimension AddDimension(BaseDimensionRepresentation baseRepresentation, string dimensionName, string baseUnitName)
      {
         var newDim = new Dimension(baseRepresentation, dimensionName, baseUnitName);
         AddDimension(newDim);
         return newDim;
      }

      public void AddDimension(IDimension dimension)
      {
         ProjectFactory.AddDimension(dimension);
      }

      public IEnumerable<string> DimensionNames => Dimensions.Select(x => x.Name);

      public IDimension Dimension(string name)
      {
         return
            BaseFactory.TryGetDimension(name) ??
            ProjectFactory.TryGetDimension(name) ??
            rhsDimensionForName(name);
      }

      public IDimension TryGetDimension(string dimensionName)
      {
         try
         {
            return Dimension(dimensionName);
         }
         catch (KeyNotFoundException)
         {
            return NoDimension;
         }
      }

      public IDimension TryGetDimensionCaseInsensitive(string dimensionName)
      {
         var dimension = Dimensions.FirstOrDefault(d => d.Name.Equals(dimensionName, StringComparison.OrdinalIgnoreCase));
         return dimension ?? NoDimension;
      }

      /// <summary>
      ///    Try to find a dimension that could be the origin dimension for the given name
      /// </summary>
      /// <exception cref="KeyNotFoundException">is thrown when match could not be found</exception>
      private IDimension rhsDimensionForName(string name)
      {
         if (string.IsNullOrEmpty(name))
            throw new KeyNotFoundException();

         if (!name.Contains(AppConstants.RHSDimensionSuffix))
            throw new KeyNotFoundException(AppConstants.Exceptions.UnknownDimension(name));

         var dimensionName = name.Replace(AppConstants.RHSDimensionSuffix, "");
         var originDimension = Dimension(dimensionName);
         return RHSDimensionFor(originDimension);
      }

      public void Clear()
      {
         /* nothing to do */
      }

      public void RemoveDimension(string dimensionName)
      {
         ProjectFactory.RemoveDimension(dimensionName);
      }

      public void RemoveDimension(IDimension dimension)
      {
         RemoveDimension(dimension.Name);
      }

      public IDimension RHSDimensionFor(IDimension dimension)
      {
         var dimensionName = AppConstants.RHSDimensionName(dimension);
         if (DimensionNames.Contains(dimensionName))
            return Dimension(dimensionName);

         var unitName = AppConstants.RHSDefaultUnitName(dimension);

         // RHS is per Time hence -1
         var rhsDimensionRepresentation = new BaseDimensionRepresentation(dimension.BaseRepresentation);
         rhsDimensionRepresentation.TimeExponent = rhsDimensionRepresentation.TimeExponent - 1;

         var rhsDimension = new Dimension(rhsDimensionRepresentation, dimensionName, unitName);
         var equivalentRHSDimension = findFirstEquivalentDimension(rhsDimension, unitName);
         if (equivalentRHSDimension != null)
            return equivalentRHSDimension;

         AddDimension(rhsDimension);
         return rhsDimension;
      }

      private IDimension findFirstEquivalentDimension(Dimension rhsDimension, string unitName)
      {
         var equivalentRHSDimension = Dimensions.FirstOrDefault(x => x.IsEquivalentTo(rhsDimension));
         if (equivalentRHSDimension != null && equivalentRHSDimension.HasUnit(unitName))
            return equivalentRHSDimension;

         return null;
      }

      public IEnumerable<IDimension> Dimensions
      {
         get
         {
            var dimensions = BaseFactory.Dimensions
               .Union(ProjectFactory.Dimensions);

            return dimensions.OrderBy(dim => dim.DisplayName);
         }
      }

      public IDimension NoDimension => Dimension(Constants.Dimension.DIMENSIONLESS);

      public IDimension MergedDimensionFor<T>(T hasDimension) where T : IWithDimension
      {
         if (hasDimension.Dimension == null)
            return null;

         var dim = BaseFactory.MergedDimensionFor(hasDimension);

         if (!Equals(dim, hasDimension.Dimension))
            return dim;

         return ProjectFactory.MergedDimensionFor(hasDimension);
      }

      public void AddMergingInformation(IDimensionMergingInformation mergingInforamtion)
      {
         ProjectFactory.AddMergingInformation(mergingInforamtion);
      }
   }
}