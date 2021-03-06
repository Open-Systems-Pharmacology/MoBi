﻿using System;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Services
{
   public interface ICreateCommitChangesToBuildingBlockCommandTask : ISpecification<IBuildingBlock>
   {
      IMoBiCommand CreateCommitToBuildingBlockCommand(IMoBiSimulation simulation, IBuildingBlock templateBuildingBlock);
   }

   public abstract class CreateCommitChangesToBuildingBlockCommandTask<T> : ICreateCommitChangesToBuildingBlockCommandTask where T : class, IBuildingBlock
   {
      private readonly ICloneManagerForBuildingBlock _cloneManager;
      private readonly Func<IBuildConfiguration, T> _buildingBlockRetriever;

      protected CreateCommitChangesToBuildingBlockCommandTask(ICloneManagerForBuildingBlock cloneManager, Func<IBuildConfiguration, T> buildingBlockRetriever)
      {
         _cloneManager = cloneManager;
         _buildingBlockRetriever = buildingBlockRetriever;
      }

      public virtual IMoBiCommand CreateCommitToBuildingBlockCommand(IMoBiSimulation simulation, IBuildingBlock templateBuildingBlock)
      {
         var macroCommand = new MoBiMacroCommand();
         macroCommand.Add(CreateCommitCommand(simulation, templateBuildingBlock));
         //hide this command that is only required for separation of concerns
         macroCommand.Add(new ResetFixedConstantParametersToDefaultInSimulationCommand<T>(simulation, _buildingBlockRetriever(simulation.BuildConfiguration)) { Visible = false });
         return macroCommand;
      }

      protected IMoBiCommand CreateCommitCommand(IMoBiSimulation simulation, IBuildingBlock templateBuildingBlock)
      {
         var typedTemplateBuildingBlock = templateBuildingBlock.DowncastTo<T>();
         var cloneOfBuildingBlockInSimulation = _cloneManager.CloneBuildingBlock(_buildingBlockRetriever(simulation.BuildConfiguration));

         return new UpdateTemplateBuildingBlockFromSimulationBuildingBlockCommand<T>(typedTemplateBuildingBlock, cloneOfBuildingBlockInSimulation, simulation);
      }

      public bool IsSatisfiedBy(IBuildingBlock templateBuildingBlock)
      {
         return templateBuildingBlock.IsAnImplementationOf<T>();
      }
   }
}