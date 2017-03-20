using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Presenter
{
   public interface ITagVisitor : IVisitor
   {
      IEnumerable<string> AllTagsFrom(ISpatialStructure spatialStructure);

      /// <summary>
      /// Returns all tags defined in the current project
      /// </summary>
      /// <returns></returns>
      IEnumerable<string> AllTags();
   }

   public class TagVisitor : ITagVisitor,
      IVisitor<IContainer>, 
      IVisitor<IParameter>,
      IVisitor<IDistributedParameter>
   {
      private readonly IMoBiProjectRetriever _projectRetriever;
      private HashSet<string> _tags = new HashSet<string>();

      public TagVisitor(IMoBiProjectRetriever projectRetriever)
      {
         _projectRetriever = projectRetriever;
      }

      public IEnumerable<string> AllTagsFrom(ISpatialStructure spatialStructure)
      {
         _tags = new HashSet<string>();
         spatialStructure.AcceptVisitor(this);
         return _tags;
      }

      public IEnumerable<string> AllTags()
      {
         IEnumerable<string> tags = new HashSet<string>();
         return _projectRetriever.Current.SpatialStructureCollection
            .Aggregate(tags, (current, spatialStructure) => current.Union(AllTagsFrom(spatialStructure)))
            .OrderBy(x => x);
      }

      public void Visit(IContainer container)
      {
         container.Tags
            .Select(tag => tag.Value)
            .Each(tagValue => _tags.Add(tagValue));

         _tags.Add(container.Name);
      }

      public void Visit(IParameter parameter)
      {
         _tags.Add(parameter.Name);
      }

      public void Visit(IDistributedParameter distributedParameter)
      {
         Visit(distributedParameter.DowncastTo<IParameter>());
      }
   }
}