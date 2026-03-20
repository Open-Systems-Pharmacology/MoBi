using System;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Extensions;
using MoBi.Core.Serialization.Xml.Services;
using MoBi.Core.Services;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.R.Domain;

namespace MoBi.R.Services;

public interface IIndividualTask
{
   IndividualBuildingBlock CreateIndividual(IndividualCharacteristics individualCharacteristics);
   void SetIndividualParameter(IndividualBuildingBlock buildingBlock, string[] quantityPaths, double[] quantityValues);
   void SetIndividualParameter(IndividualBuildingBlock buildingBlock, string quantityPath, double quantityValue);
}

public class IndividualTask : PKSimPathAndValuesTask, IIndividualTask
{
   private readonly IXmlSerializationService _xmlSerializationService;
   private readonly IMoBiProjectRetriever _projectRetriever;
   private readonly IMoBiContext _context;
   private readonly IObjectTypeResolver _objectTypeResolver;

   public IndividualTask(IXmlSerializationService xmlSerializationService, IMoBiProjectRetriever projectRetriever, IMoBiContext context, IObjectTypeResolver objectTypeResolver)
   {
      _xmlSerializationService = xmlSerializationService;
      _projectRetriever = projectRetriever;
      _context = context;
      _objectTypeResolver = objectTypeResolver;
   }

   public IndividualBuildingBlock CreateIndividual(IndividualCharacteristics individualCharacteristics)
   {
      LoadPKSimAssembly();

      var serializedIndividual = ExecuteMethod(GetMethod("PKSim.R.Exchange.BuildingBlockCreator", "CreateIndividual"), [individualCharacteristics]) as string;

      return _xmlSerializationService.Deserialize<IndividualBuildingBlock>(serializedIndividual, _projectRetriever.Current);
   }

   public void SetIndividualParameter(IndividualBuildingBlock buildingBlock, string[] quantityPaths, double[] quantityValues)
   {
      if (!quantityPaths.HasConsistentLengthWith(quantityValues))
         throw new ArgumentException(AppConstants.Exceptions.AllArraysMustHaveTheSameLength);

      var macroCommand = new MoBiMacroCommand
      {
         CommandType = AppConstants.Commands.ExtendCommand,
         Description = AppConstants.Commands.ExtendDescription,
         ObjectType = _objectTypeResolver.TypeFor<IndividualBuildingBlock>()
      };

      macroCommand.AddRange(quantityPaths.Select((quantityPath, i) => UpdateValueCommandFor<IndividualBuildingBlock, IndividualParameter>(buildingBlock, quantityPath, quantityValues[i])));

      _context.AddToHistory(macroCommand.RunCommand(_context));
   }

   public void SetIndividualParameter(IndividualBuildingBlock buildingBlock, string quantityPath, double quantityValue) => SetIndividualParameter(buildingBlock, [quantityPath], [quantityValue]);
}