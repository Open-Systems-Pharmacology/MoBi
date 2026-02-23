using System;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.R.Services;

public interface IExpressionProfileTask
{
   void SetExpressionParameter(ExpressionProfileBuildingBlock buildingBlock, string[] quantityPaths, double[] quantityValues);
}

public class ExpressionProfileTask : PKSimPathAndValuesTask, IExpressionProfileTask
{
   private readonly IMoBiContext _context;
   private readonly IObjectTypeResolver _objectTypeResolver;

   public ExpressionProfileTask(IMoBiContext context, IObjectTypeResolver objectTypeResolver)
   {
      _context = context;
      _objectTypeResolver = objectTypeResolver;
   }

   public void SetExpressionParameter(ExpressionProfileBuildingBlock buildingBlock, string[] quantityPaths, double[] quantityValues)
   {
      if (!quantityPaths.HasConsistentLengthWith(quantityValues))
         throw new ArgumentException(AppConstants.Exceptions.AllArraysMustHaveTheSameLength);

      var macroCommand = new MoBiMacroCommand
      {
         CommandType = AppConstants.Commands.ExtendCommand,
         Description = AppConstants.Commands.ExtendDescription,
         ObjectType = _objectTypeResolver.TypeFor<IndividualBuildingBlock>()
      };

      macroCommand.AddRange(quantityPaths.Select((quantityPath, i) => UpdateValueCommandFor<ExpressionProfileBuildingBlock, ExpressionParameter>(buildingBlock, quantityPath, quantityValues[i])));

      _context.AddToHistory(macroCommand.RunCommand(_context));
   }
}