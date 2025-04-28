using MPFitLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSPSuite.BDDHelper;
using System.IO;

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
