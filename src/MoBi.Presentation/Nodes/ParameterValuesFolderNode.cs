using MoBi.Assets;
using MoBi.Presentation.DTO;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Utility;

namespace MoBi.Presentation.Nodes
{
   public class ParameterValuesFolderNode : AbstractNode<ModuleViewItem>
   {
      public ParameterValuesFolderNode(ClassifiableModule classifiableModule) : base(new ModuleViewItem(classifiableModule).WithTarget(classifiableModule.Subject.ParameterValuesCollection))
      {
         Id = ShortGuid.NewGuid();
         Text = AppConstants.Captions.ParameterValues;
         Icon = ApplicationIcons.ParameterValuesFolder;
      }
      public override string Id { get; }
   }
}