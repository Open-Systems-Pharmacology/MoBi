namespace MoBi.Core
{
   public interface IApplicationSettings : OSPSuite.Core.IApplicationSettings
   {
      /// <summary>
      ///    Path of PK-Sim installation. Only used if the installation path is not found in the registry
      /// </summary>
      string PKSimPath { get; set; }
   }

   public class ApplicationSettings : OSPSuite.Core.ApplicationSettings, IApplicationSettings
   {
      public string PKSimPath { get; set; }
   }
}