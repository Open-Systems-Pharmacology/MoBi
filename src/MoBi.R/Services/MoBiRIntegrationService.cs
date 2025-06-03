using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using ISerializationTask = MoBi.Presentation.Tasks.ISerializationTask;

namespace MoBi.R.Services
{
   public interface IMoBiRIntegrationService
   {
      IReadOnlyList<ParameterIdentification> GetParameterIdentifications(MoBiProject project);
      IReadOnlyList<DataRepository> GetObservedDataSets(MoBiProject project);
      IndividualBuildingBlock CreateIndividual(string name);
      IReadOnlyList<Module> LoadModulesFromFile(string filePath);
      void SetParameterValue(ParameterValuesBuildingBlock pvBB, string parameterName, double newValue);
   }

   public class MoBiRIntegrationService : IMoBiRIntegrationService
   {
      private readonly IProjectTask _projectTask;
      private readonly ISerializationTask _serializationTask;

      public MoBiRIntegrationService(IProjectTask projectTask, ISerializationTask serializationTask)
      {
         _projectTask = projectTask;
         _serializationTask = serializationTask;
      }

      public IReadOnlyList<ParameterIdentification> GetParameterIdentifications(MoBiProject project) =>
         project.AllParameterIdentifications.ToList();

      public IReadOnlyList<DataRepository> GetObservedDataSets(MoBiProject project) =>
         project.AllObservedData.ToList();

      public IndividualBuildingBlock CreateIndividual(string name)
      {
         return new IndividualBuildingBlock { Name = name };
      }

      public IReadOnlyList<Module> LoadModulesFromFile(string filePath) =>
         _serializationTask.LoadMany<Module>(filePath).ToList();

      public void SetParameterValue(ParameterValuesBuildingBlock pvBB, string parameterName, double newValue)
      {
         var parameter = pvBB.FirstOrDefault(p => p.Name == parameterName);
         if (parameter != null)
            parameter.Value = newValue;
      }
   }
}