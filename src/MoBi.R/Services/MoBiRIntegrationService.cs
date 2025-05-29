using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Serialization;
using OSPSuite.Infrastructure.Import.Core;
using ISerializationTask = MoBi.Presentation.Tasks.ISerializationTask;
namespace MoBi.R.Services
{

   public interface IMoBiRIntegrationService
   {
      IReadOnlyList<ParameterIdentification> GetParameterIdentifications(MoBiProject project);
      IReadOnlyList<DataRepository> GetObservedDataSets(MoBiProject project);
      IndividualBuildingBlock CreateIndividual(string name);
      IReadOnlyList<Module> LoadModulesFromFile(string filePath);
      void ExtendInitialConditionsWithModule(InitialConditionsBuildingBlock icBB, Module module);
      void AddProteinExpression(ParameterValuesBuildingBlock pvBB, string moleculeName, double expressionValue);
      void SetParameterValue(ParameterValuesBuildingBlock pvBB, string parameterName, double newValue);
   }


   public class MoBiRIntegrationService : IMoBiRIntegrationService
   {
      private readonly IProjectTask _projectTask;
      private readonly ISerializationTask _serializationTask;

      public MoBiRIntegrationService(IProjectTask projectTask, ISerializationTask  serializationTask)
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

      public void ExtendInitialConditionsWithModule(InitialConditionsBuildingBlock icBB, Module module)
      {
         throw new NotImplementedException();
         //foreach (var molecule in module.Molecules)
         //{
         //   if (!icBB.Molecules.Contains(molecule))
         //   {
         //      icBB.AddMolecule(molecule);
         //   }
         //}
      }

      public void AddProteinExpression(ParameterValuesBuildingBlock pvBB, string moleculeName, double expressionValue)
      {
         throw new NotImplementedException();

         //var parameter = new Parameter
         //{
         //   Name = moleculeName + " Expression",
         //   Value = expressionValue,
         //   Dimension = "concentration"
         //};

         //pvBB.Add(parameter);
      }

      public void SetParameterValue(ParameterValuesBuildingBlock pvBB, string parameterName, double newValue)
      {
         var parameter = pvBB.FirstOrDefault(p => p.Name == parameterName);
         if (parameter != null)
            parameter.Value = newValue;
      }
   }
}