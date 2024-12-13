using System;
using MoBi.Assets;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Utility.Exceptions;

namespace MoBi.Core.Exceptions
{
   public class MoBiException : OSPSuiteException
   {
      public MoBiException(string message) : base(message)
      {
      }

      public MoBiException()
      {
      }

      public MoBiException(string message, Exception innerException)
         : base(message, innerException)
      {
      }
   }

   public class CannotConvertConcentrationToAmountException : MoBiException
   {
      public CannotConvertConcentrationToAmountException(string objectType) :
         base(AppConstants.Exceptions.CannotConvertAConcentrationModelBasedIntoAnAmountBasedModel(objectType))
      {
      }
   }

   public class NotMatchingSerializationFileException : MoBiException
   {
      public NotMatchingSerializationFileException(string searchedElement) :
         base(AppConstants.Exceptions.NoInformationFoundException(searchedElement))
      {
      }

      public NotMatchingSerializationFileException(string fileName, string searchedElement, string foundElement) :
         base(AppConstants.Exceptions.NoInformationFoundException(fileName, searchedElement, foundElement))
      {
      }
   }

   public class InvalidProjectFileException : MoBiException
   {
      public InvalidProjectFileException(int projectVersion)
         : base(
            Captions.ProjectVersionCannotBeLoaded(
               projectVersion,
               ProjectVersions.OldestSupportedVersion.VersionDisplay,
               ProjectVersions.OldestSupportedVersion.Version,
               ProjectVersions.Current.VersionDisplay,
               ProjectVersions.Current.Version,
               Constants.PRODUCT_SITE_DOWNLOAD))
      {
      }
   }
}