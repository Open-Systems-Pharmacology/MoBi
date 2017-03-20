﻿using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Model.Diagram;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Domain.Model
{
   public interface IMoBiReactionBuildingBlock : IReactionBuildingBlock, IWithDiagramFor<IMoBiReactionBuildingBlock>
   {
      IEnumerable<string> AllMolecules { get; }
   }

   public class MoBiReactionBuildingBlock : ReactionBuildingBlock, IMoBiReactionBuildingBlock
   {
      public IDiagramManager<IMoBiReactionBuildingBlock> DiagramManager { get; set; }
      public IDiagramModel DiagramModel { get; set; }

      public override void UpdatePropertiesFrom(IUpdatable sourceObject, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(sourceObject, cloneManager);
         var sourceReactionBuildingBlock = sourceObject as IMoBiReactionBuildingBlock;
         if (sourceReactionBuildingBlock == null) return;

         this.UpdateDiagramFrom(sourceReactionBuildingBlock);
      }

      public IEnumerable<string> AllMolecules => DiagramManager.DowncastTo<IMoBiReactionDiagramManager>().GetMoleculeNodes().Select(x => x.Name);
   }
}