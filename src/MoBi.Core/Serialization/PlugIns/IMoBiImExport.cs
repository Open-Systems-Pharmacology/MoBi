using System;
using OSPSuite.Utility.Collections;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Serialization.PlugIns
{
   public interface IMoBiPlugIn
   {
      string Name { get; }
      Version Version { get; }
      bool RequireConfiguration { get; }

      void ConfigureWithGui();
      void Configure(ICache<string, IExtendedProperty> optionToSet);

      /// <summary>
      /// Gets the default options. Collection returned is read only
      /// </summary>
      /// <value>The default options.</value>
      ICache<string, IExtendedProperty> GetAllOptions();
   }

   public interface IMoBiImport : IMoBiPlugIn
   {
      MoBiProject LoadProject(bool silent);
   }

   public interface IMoBiExport : IMoBiPlugIn
   {
      bool SaveProject(MoBiProject root, bool silent);
   }

   public interface IMoBiDimensionImport : IMoBiPlugIn
   {
      IDimensionFactory LoadDimensions(string importDefinition, bool silent);
      bool SaveDimensinons(IDimensionFactory dimensionFactory);
   }
}