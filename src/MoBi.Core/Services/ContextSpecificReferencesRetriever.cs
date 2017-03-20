using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Services
{
   public interface IContextSpecificReferencesRetriever
   {
      IEnumerable<IObjectBase> RetrieveFor(IEventAssignmentBuilder eventAssignment);
      IEntity RetrieveLocalReferencePoint(IParameter parameter);
      IEnumerable<IObjectBase> RetrieveFor(IParameter parameter, IBuildingBlock buildingBlock);
   }

   public class ContextSpecificReferencesRetriever : IContextSpecificReferencesRetriever
   {
      public IEnumerable<IObjectBase> RetrieveFor(IEventAssignmentBuilder eventAssignment)
      {
         return new IObjectBase[] {eventAssignment.RootContainer};
      }

      public IEntity RetrieveLocalReferencePoint(IParameter parameter)
      {
         if (parameter.IsAtReaction() || parameter.IsAtMolecule())
         {
            if (parameter.BuildMode.Equals(ParameterBuildMode.Local))
               return null;
         }
         return parameter;
      }

      public IEnumerable<IObjectBase> RetrieveFor(IParameter parameter, IBuildingBlock buildingBlock)
      {
         if (parameter == null || parameter.ParentContainer == null)
            return Enumerable.Empty<IObjectBase>();

         if (parameter.ParentContainer.IsAnImplementationOf<TransporterMoleculeContainer>())
            return new List<IObjectBase> {parameter.RootContainer};

         IEnumerable<IObjectBase> entities = parameter.ParentContainer.GetChildren<IParameter>().OrderBy(x => x.Name);

       
         if (parameter.IsAtReaction())
            return entities.Union(getOther(parameter,(IBuildingBlock<IReactionBuilder>) buildingBlock));
         return entities;
      }

      private IEnumerable<IObjectBase> getOther<T>(IParameter parameter,IBuildingBlock<T> buildingBlock) where T: class, IObjectBase
      {
         var other = buildingBlock.Cast<IContainer>().ToList();
         other.Remove(parameter.ParentContainer);
         return other.OrderBy(r => r.Name);
      }
   }
}