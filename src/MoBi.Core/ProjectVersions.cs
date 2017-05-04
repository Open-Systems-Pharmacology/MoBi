using OSPSuite.Utility.Collections;
using OSPSuite.Core;
using OSPSuite.Core.Serialization;

namespace MoBi.Core
{
   public static class ProjectVersions
   {
      private static readonly Cache<int, ProjectVersion> _knownVersions = new Cache<int, ProjectVersion>(x => x.Version);

      //Version 3.0.1 to 3.0.3
      public static readonly ProjectVersion V3_0_1_to_3 = addVersion(0, "3.0.1");
      public static readonly ProjectVersion V3_0_4 = addVersion(1, "3.0.4");
      public static readonly ProjectVersion V3_1_3 = addVersion(2, "3.1.3");
      public static readonly ProjectVersion V3_2_1 = addVersion(3, "3.2.1");
      public static readonly ProjectVersion V3_3_1 = addVersion(PKMLVersion.V5_3_1, "3.3.1");
      public static readonly ProjectVersion V3_4_1 = addVersion(PKMLVersion.V5_4_1, "3.4.1");
      public static readonly ProjectVersion V3_5_1 = addVersion(PKMLVersion.V5_5_1, "3.5.1");
      public static readonly ProjectVersion V3_6_1 = addVersion(PKMLVersion.V5_6_1, "3.3.1");
      public static readonly ProjectVersion V6_0_1 = addVersion(PKMLVersion.V6_0_1, "6.0.1");
      public static readonly ProjectVersion V6_1_1 = addVersion(PKMLVersion.V6_1_1, "6.1.1");
      public static readonly ProjectVersion V6_2_1 = addVersion(PKMLVersion.V6_2_1, "6.2.1");
      public static readonly ProjectVersion V6_3_1 = addVersion(PKMLVersion.V6_3_1, "6.3.1");
      public static readonly ProjectVersion V7_1_0 = addVersion(PKMLVersion.V7_1_0, "7.1.0");
      public static readonly ProjectVersion Current = V7_1_0;

      private static ProjectVersion addVersion(int versionNumber, string versionDisplay)
      {
         var projectVersion = new ProjectVersion(versionNumber, versionDisplay);
         _knownVersions.Add(projectVersion);
         return projectVersion;
      }

      public static string CurrentAsString
      {
         get { return Current.VersionAsString; }
      }

      public static bool CanLoadVersion(int projectVersion)
      {
         return (projectVersion <= Current.Version) && _knownVersions.Contains(projectVersion);
      }

      public static ProjectVersion FindBy(int version)
      {
         return _knownVersions[version];
      }
   }
}  