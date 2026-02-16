using MoBi.Assets;
using MoBi.Core.Exceptions;
using MoBi.Core.Serialization.Xml.Services;
using MoBi.Core.Services;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.R.Domain;
using OSPSuite.Utility;
using System.IO;
using System.Reflection;

namespace MoBi.R.Services;

public interface IIndividualTask
{
   IndividualBuildingBlock CreateIndividual(IndividualCharacteristics individualCharacteristics);
}

public class IndividualTask : PKSimAssemblyLoader, IIndividualTask
{
   private readonly IXmlSerializationService _xmlSerializationService;
   private readonly IMoBiProjectRetriever _projectRetriever;

   public IndividualTask(IXmlSerializationService xmlSerializationService, IMoBiProjectRetriever projectRetriever)
   {
      _xmlSerializationService = xmlSerializationService;
      _projectRetriever = projectRetriever;
   }

   private const string PKSIM_R_DLL = "PKSim.R.dll";

   public IndividualBuildingBlock CreateIndividual(IndividualCharacteristics individualCharacteristics)
   {
      LoadPKSimAssembly();

      var serializedIndividual = ExecuteMethod(GetMethod("PKSim.R.Exchange.BuildingBlockCreator", "CreateIndividual"), [individualCharacteristics]) as string;

      return _xmlSerializationService.Deserialize<IndividualBuildingBlock>(serializedIndividual, _projectRetriever.Current);
   }

   protected override string RetrievePKSimAssemblyPath()
   {
      var assemblyFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), PKSIM_R_DLL);
      if (FileHelper.FileExists(assemblyFile))
         return assemblyFile;

      throw new MoBiException(AppConstants.PKSim.CouldNotFindCompatiblePKSimAssemblies(assemblyFile));
   }
}