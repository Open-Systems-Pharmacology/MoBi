using MoBi.Assets;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Assets;

namespace MoBi.Presentation.Nodes
{
   public class MoBiRootNodeTypes
   {
      public static readonly RootNodeType ExpressionProfilesFolder = new RootNodeType(AppConstants.Captions.ExpressionProfiles, ApplicationIcons.ExpressionProfileFolder);
      public static readonly RootNodeType IndividualsFolder = new RootNodeType(AppConstants.Captions.Individuals, ApplicationIcons.IndividualFolder);
   }
}