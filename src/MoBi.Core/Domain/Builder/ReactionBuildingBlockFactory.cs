﻿using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;

namespace MoBi.Core.Domain.Builder
{
   public interface IReactionBuildingBlockFactory
   {
      MoBiReactionBuildingBlock Create();
   }

   public class ReactionBuildingBlockFactory : IReactionBuildingBlockFactory
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IDiagramManagerFactory _diagramManagerFactory;

      public ReactionBuildingBlockFactory(IObjectBaseFactory objectBaseFactory, IDiagramManagerFactory diagramManagerFactory)
      {
         _objectBaseFactory = objectBaseFactory;
         _diagramManagerFactory = diagramManagerFactory;
      }

      public MoBiReactionBuildingBlock Create()
      {
         var buildingBlock = _objectBaseFactory.Create<MoBiReactionBuildingBlock>();
         buildingBlock.DiagramManager = _diagramManagerFactory.Create<IMoBiReactionDiagramManager>();
         return buildingBlock;
      }
   }
}