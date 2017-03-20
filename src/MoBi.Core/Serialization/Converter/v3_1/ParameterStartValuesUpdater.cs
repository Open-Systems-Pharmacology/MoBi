using System;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Serialization.Converter.v3_1
{
   public interface IParameterStartValuesUpdater
   {
      void UpdateParameterStartvalues(IParameterStartValuesBuildingBlock parameterStartValuesBuildingBlock, IMoBiProject project);
      void UpdateParameterStartValue(IParameterStartValue parameterStartValue, IMoBiProject project);
      void UpdateParameterStartvalues(IParameterStartValuesBuildingBlock parameterStartValuesBuildingBlock, IMoBiSimulation simulation);
   }

   internal class ParameterStartValuesUpdater : IParameterStartValuesUpdater
   {
      private readonly IParameterStartValueDimensionRetriever _dimensionRetriever;
      private IParameterStartValuesBuildingBlock _parameterStartValuesBuildingBlock;
      private readonly IDimensionFactory _dimensionFactory;
      private readonly IProjectConverterLogger _projectConverterLogger;

      public ParameterStartValuesUpdater(IParameterStartValueDimensionRetriever dimensionRetriever, IDimensionFactory dimensionFactory, IProjectConverterLogger projectConverterLogger)
      {
         _dimensionRetriever = dimensionRetriever;
         _dimensionFactory = dimensionFactory;
         _projectConverterLogger = projectConverterLogger;
      }

      public void UpdateParameterStartvalues(IParameterStartValuesBuildingBlock parameterStartValuesBuildingBlock, IMoBiProject project)
      {
         _parameterStartValuesBuildingBlock = parameterStartValuesBuildingBlock;
         _projectConverterLogger.AddInfo(AppConstants.ProjectUpdateMessages.UpdatePSV(parameterStartValuesBuildingBlock.Name), parameterStartValuesBuildingBlock, parameterStartValuesBuildingBlock);
         parameterStartValuesBuildingBlock.Each(psv => UpdateParameterStartValue(psv, project));
      }

      public void UpdateParameterStartvalues(IParameterStartValuesBuildingBlock parameterStartValuesBuildingBlock, IMoBiSimulation simulation)
      {
         _parameterStartValuesBuildingBlock = parameterStartValuesBuildingBlock;
         _projectConverterLogger.AddInfo(AppConstants.ProjectUpdateMessages.UpdatePSV(parameterStartValuesBuildingBlock.Name), parameterStartValuesBuildingBlock, parameterStartValuesBuildingBlock);
         parameterStartValuesBuildingBlock.Each(psv => UpdateParameterStartValue(psv, simulation));
      }

      public void UpdateParameterStartValue(IParameterStartValue parameterStartValue, IMoBiProject project)
      {
         updateDimension(parameterStartValue, project);
         var parameterName = parameterStartValue.ParameterName;
         if (parameterName.Equals(Constants.Distribution.GEOMETRIC_DEVIATION))
         {
            parameterStartValue.StartValue = Math.Exp(parameterStartValue.StartValue.Value);
         }
      }

      public void UpdateParameterStartValue(IParameterStartValue parameterStartValue, IMoBiSimulation simulation)
      {
         updateDimension(parameterStartValue, simulation);
         var parameterName = parameterStartValue.ParameterName;
         if (parameterName.Equals(Constants.Distribution.GEOMETRIC_DEVIATION))
         {
            parameterStartValue.StartValue = Math.Exp(parameterStartValue.StartValue.Value);
         }
      }

      private void updateDimension(IParameterStartValue psv, IMoBiProject project)
      {
         var dimension = _dimensionRetriever.GetDimensionFor(psv, _parameterStartValuesBuildingBlock, project);
         if (dimension == null)
         {
            dimension = _dimensionFactory.NoDimension;
            _projectConverterLogger.AddSubMessage(AppConstants.ProjectUpdateMessages.UnableToGetDimensionFor(psv, _parameterStartValuesBuildingBlock.Name), _parameterStartValuesBuildingBlock);
         }
         psv.Dimension = dimension;
      }

      private void updateDimension(IParameterStartValue psv, IMoBiSimulation simulation)
      {
         var dimension = _dimensionRetriever.GetDimensionFor(psv, _parameterStartValuesBuildingBlock, simulation);
         if (dimension == null)
         {
            dimension = _dimensionFactory.NoDimension;
            _projectConverterLogger.AddSubMessage(AppConstants.ProjectUpdateMessages.UnableToGetDimensionFor(psv, _parameterStartValuesBuildingBlock.Name), _parameterStartValuesBuildingBlock);
         }
         psv.Dimension = dimension;
      }
   }
}
