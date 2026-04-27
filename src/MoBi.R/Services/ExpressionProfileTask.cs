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
using OSPSuite.Core.Serialization;
using ISerializationTask = MoBi.Presentation.Tasks.ISerializationTask;

namespace MoBi.R.Services;

public interface IExpressionProfileTask : IPathAndValuesTask<ExpressionProfileBuildingBlock, ExpressionParameter>
{
   ExpressionProfileBuildingBlock CreateExpressionProfile(string category, string moleculeName, string speciesName, string phenotype);
   void SetExpressionParameter(ExpressionProfileBuildingBlock buildingBlock, string[] quantityPaths, double[] quantityValues);
   void SetExpressionParameter(ExpressionProfileBuildingBlock buildingBlock, string quantityPath, double quantityValue);
   void ExportToPKML(ExpressionProfileBuildingBlock buildingBlock, string filePath);
}

public class ExpressionProfileTask : PKSimPathAndValuesTask<ExpressionProfileBuildingBlock, ExpressionParameter>, IExpressionProfileTask
{
   private readonly IXmlSerializationService _xmlSerializationService;
   private readonly IMoBiProjectRetriever _projectRetriever;
   private readonly IMoBiContext _context;
   private readonly IObjectTypeResolver _objectTypeResolver;

   public ExpressionProfileTask(IXmlSerializationService xmlSerializationService, IMoBiProjectRetriever projectRetriever, IMoBiContext context, IObjectTypeResolver objectTypeResolver, ISerializationTask serializationTask, IPKSimAssemblyLoader pkSimLoader) : base(serializationTask, pkSimLoader)
   {
      _xmlSerializationService = xmlSerializationService;
      _projectRetriever = projectRetriever;
      _context = context;
      _objectTypeResolver = objectTypeResolver;
   }

   public ExpressionProfileBuildingBlock CreateExpressionProfile(string category, string moleculeName, string speciesName, string phenotype)
   {
      _pkSimLoader.LoadPKSimAssembly();

      var serializedExpressionProfile = _pkSimLoader.ExecuteMethod("PKSim.R.Exchange.BuildingBlockCreator", "CreateExpressionProfile", [category, moleculeName, speciesName, phenotype]) as string;

      return _xmlSerializationService.Deserialize<ExpressionProfileBuildingBlock>(serializedExpressionProfile, _projectRetriever.Current);
   }

   public void SetExpressionParameter(ExpressionProfileBuildingBlock buildingBlock, string[] quantityPaths, double[] quantityValues)
   {
      if (!quantityPaths.HasConsistentLengthWith(quantityValues))
         throw new ArgumentException(AppConstants.Exceptions.AllArraysMustHaveTheSameLength);

      var macroCommand = new MoBiMacroCommand
      {
         CommandType = AppConstants.Commands.ExtendCommand,
         Description = AppConstants.Commands.ExtendDescription,
         ObjectType = _objectTypeResolver.TypeFor<ExpressionProfileBuildingBlock>()
      };

      macroCommand.AddRange(quantityPaths.Select((quantityPath, i) => UpdateValueCommandFor(buildingBlock, quantityPath, quantityValues[i])));

      _context.AddToHistory(macroCommand.RunCommand(_context));
   }

   public void SetExpressionParameter(ExpressionProfileBuildingBlock buildingBlock, string quantityPath, double quantityValue) => SetExpressionParameter(buildingBlock, [quantityPath], [quantityValue]);

   public void ExportToPKML(ExpressionProfileBuildingBlock buildingBlock, string filePath) =>
      _xmlSerializationService.SerializeModelPart(buildingBlock).PermissiveSave(filePath);
}