using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks
{
   public interface IEntityTask
   {
      IMoBiCommand AddNewTagTo(IEntity entity, IBuildingBlock buildingBlock);
      IMoBiCommand RemoveTagFrom(TagDTO tagDTO, IEntity entity, IBuildingBlock buildingBlock);
   }

   public class EntityTask : IEntityTask
   {
      private readonly IMoBiContext _context;
      private readonly ITagVisitor _tagVisitor;
      private readonly IObjectBaseNamingTask _namingTask;

      public EntityTask(IMoBiContext context, ITagVisitor tagVisitor, IObjectBaseNamingTask namingTask)
      {
         _context = context;
         _tagVisitor = tagVisitor;
         _namingTask = namingTask;
      }

      public IMoBiCommand AddNewTagTo(IEntity entity, IBuildingBlock buildingBlock)
      {
         var tag = _namingTask.NewName("New Tag for Container", "New Tag", string.Empty, entity.Tags.Select(x => x.Value), getUsedTags());
         if (string.IsNullOrEmpty(tag))
            return new MoBiEmptyCommand();

         return new AddTagCommand(tag, entity, buildingBlock).Run(_context);
      }

      private IEnumerable<string> getUsedTags() => _tagVisitor.AllTags();

      public IMoBiCommand RemoveTagFrom(TagDTO tagDTO, IEntity entity, IBuildingBlock buildingBlock)
      {
         return new RemoveTagCommand(tagDTO.Value, entity, buildingBlock).Run(_context);
      }
   }
}