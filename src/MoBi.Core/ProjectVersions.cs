using System.Linq;
using OSPSuite.Core;
using OSPSuite.Core.Serialization;
using OSPSuite.Utility.Collections;

namespace MoBi.Core
{
   public static class ProjectVersions
   {
      private static readonly Cache<int, ProjectVersion> _knownVersions = new Cache<int, ProjectVersion>(x => x.Version);

      public const int UNSUPPORTED = PKMLVersion.NON_CONVERTABLE_VERSION;
      // older versions not supported anymore starting V9

      public static readonly ProjectVersion V6_0_1 = addVersion(PKMLVersion.V6_0_1, "6.0.1");
      public static readonly ProjectVersion V6_1_1 = addVersion(PKMLVersion.V6_1_1, "6.1.1");
      public static readonly ProjectVersion V6_2_1 = addVersion(PKMLVersion.V6_2_1, "6.2.1");
      public static readonly ProjectVersion V6_3_1 = addVersion(PKMLVersion.V6_3_1, "6.3.1");
      public static readonly ProjectVersion V7_1_0 = addVersion(PKMLVersion.V7_1_0, "7.1.0");
      public static readonly ProjectVersion V7_3_0 = addVersion(PKMLVersion.V7_3_0, "7.3.0");
      public static readonly ProjectVersion V9_0 = addVersion(PKMLVersion.V9_0, "9.0");
      public static readonly ProjectVersion V10_0 = addVersion(PKMLVersion.V10_0, "10.0");
      public static readonly ProjectVersion V11_0 = addVersion(PKMLVersion.V11_0, "11.0");
      public static readonly ProjectVersion V12_0 = addVersion(PKMLVersion.V12_0, "12.0");
      public static readonly ProjectVersion Current = V12_0;

      private static ProjectVersion addVersion(int versionNumber, string versionDisplay)
      {
         var projectVersion = new ProjectVersion(versionNumber, versionDisplay);
         _knownVersions.Add(projectVersion);
         return projectVersion;
      }

      public static string CurrentAsString => Current.VersionAsString;

      public static bool CanLoadVersion(int projectVersion)
      {
         return (projectVersion <= Current.Version) && _knownVersions.Contains(projectVersion);
      }

      public static ProjectVersion OldestSupportedVersion => _knownVersions.OrderBy(x => x.Version).First();

      public static ProjectVersion FindBy(int version) => _knownVersions[version];
   }
}