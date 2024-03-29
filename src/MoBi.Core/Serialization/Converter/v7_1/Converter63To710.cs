﻿using System.Xml.Linq;
using MoBi.Core.Domain.Model;

namespace MoBi.Core.Serialization.Converter.v7_1
{
   public class Converter63To710 : IMoBiObjectConverter
   {
      private readonly OSPSuite.Core.Converters.v7_1.Converter63To710 _coreConverter;

      public Converter63To710(OSPSuite.Core.Converters.v7_1.Converter63To710 coreConverter)
      {
         _coreConverter = coreConverter;
      }

      public bool IsSatisfiedBy(int version)
      {
         return version == ProjectVersions.V6_3_1;
      }

      public (int convertedToVersion, bool conversionHappened) Convert(object objectToUpdate, MoBiProject project)
      {
         return _coreConverter.Convert(objectToUpdate);
      }

      public (int convertedToVersion, bool conversionHappened) ConvertXml(XElement element, MoBiProject project)
      {
         return _coreConverter.ConvertXml(element);
     }
   }
}