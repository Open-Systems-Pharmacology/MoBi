using System;
using System.IO;
using OSPSuite.BDDHelper;

namespace MoBi.R.Tests
{
   [IntegrationTests]
   public abstract class ContextForIntegration<T> : ContextSpecification<T>
   {
      public override void GlobalContext()
      {
         base.GlobalContext();
         var apiConfig = new ApiConfig
         {
            DimensionFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "OSPSuite.Dimensions.xml"),
            PKParametersFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "OSPSuite.PKParameters.xml"),
         };
         Api.InitializeOnce(apiConfig);

         Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
      }
   }
}