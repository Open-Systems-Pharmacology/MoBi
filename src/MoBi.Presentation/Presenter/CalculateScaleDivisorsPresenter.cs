using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoBi.Assets;
using OSPSuite.Utility.Collections;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Extensions;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Extensions;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Assets;
using OSPSuite.Utility.Exceptions;

namespace MoBi.Presentation.Presenter
{
   public interface ICalculateScaleDivisorsPresenter : IDisposablePresenter, IBreadCrumbsPresenter
   {
      IMoBiCommand CalculateScaleDivisorFor(IMoBiSimulation simulation);
      Task StartScaleDivisorsCalculation();
      void ResetScaleDivisors();
      void UpdateScaleFactorValue(ScaleDivisorDTO scaleDivisorDTO, double newScaleDivisor);
   }

   public class CalculateScaleDivisorsPresenter : AbstractDisposableCommandCollectorPresenter<ICalculateScaleDivisorsView, ICalculateScaleDivisorsPresenter>, ICalculateScaleDivisorsPresenter
   {
      private readonly ICommandTask _commandTask;
      private readonly IContainerTask _containerTask;
      private readonly IScaleDivisorCalculator _scaleDivisorCalculator;
      private readonly IMoleculeAmountTask _moleculeAmountTask;
      private readonly IMoBiContext _context;
      private readonly IMoBiMacroCommand _commands;
      private readonly Cache<string, ScaleDivisorDTO> _scaleDivisors = new Cache<string, ScaleDivisorDTO>(x => x.PathAsString, onMissingKey: x => null);
      private IMoBiSimulation _simulation;
      private PathCache<IMoleculeAmount> _allMoleculeAmounts;

      public CalculateScaleDivisorsPresenter(ICalculateScaleDivisorsView view, ICommandTask commandTask, IContainerTask containerTask,
         IScaleDivisorCalculator scaleDivisorCalculator, IMoleculeAmountTask moleculeAmountTask, IMoBiContext context)
         : base(view)
      {
         _commandTask = commandTask;
         _containerTask = containerTask;
         _scaleDivisorCalculator = scaleDivisorCalculator;
         _moleculeAmountTask = moleculeAmountTask;
         _context = context;
         _commands = new MoBiMacroCommand
         {
            CommandType = AppConstants.Commands.EditCommand,
            ObjectType = ObjectTypes.Simulation
         };
      }

      public override void Initialize()
      {
         InitializeWith(_commands);
      }

      public IMoBiCommand CalculateScaleDivisorFor(IMoBiSimulation simulation)
      {
         _simulation = simulation;
         _commands.Description = AppConstants.Commands.UpdateScaleDivisorValuesInSimulation(simulation.Name);

         _allMoleculeAmounts = retrieveScalableMoleculeAmounts();

         createScaleDivisors();

         _view.BindTo(_scaleDivisors);
         _view.Display();

         if (_view.Canceled)
         {
            _commandTask.ResetChanges(_commands, _context);
            return new MoBiEmptyCommand();
         }


         return _commands;
      }

      /// <summary>
      ///    Only molecule amounts defined in molecule start values can be scaled
      /// </summary>
      /// <returns></returns>
      private PathCache<IMoleculeAmount> retrieveScalableMoleculeAmounts()
      {
         var allMoleculeAmounts = _containerTask.CacheAllChildren<IMoleculeAmount>(_simulation.Model.Root);
         var moleculeStartValues = _simulation.BuildConfiguration.MoleculeStartValues;

         foreach (var path in allMoleculeAmounts.Keys.ToList())
         {
            if (moleculeStartValues[new ObjectPath(path.ToPathArray())] == null)
               allMoleculeAmounts.Remove(path);
         }

         return allMoleculeAmounts;
      }

      private void createScaleDivisors()
      {
         _scaleDivisors.Clear();
         foreach (var keyValue in _allMoleculeAmounts.KeyValues)
         {
            _scaleDivisors.Add(scaleFactorDTOFor(keyValue.Key, keyValue.Value));
         }
      }

      private ScaleDivisorDTO scaleFactorDTOFor(string path, IMoleculeAmount moleculeAmount)
      {
         return new ScaleDivisorDTO(moleculeAmount)
         {
            //this is the full path
            ContainerPath = path.ToPathArray().ContainerPath()
         };
      }

      public bool HasAtLeastTwoDistinctValues(int pathElementIndex)
      {
         return _scaleDivisors.HasAtLeastTwoDistinctValues(pathElementIndex);
      }
      public async Task StartScaleDivisorsCalculation()
      {
         try
         {
            _view.Calculating = true;
            var scaleFactors = await _scaleDivisorCalculator.CalculateScaleDivisorsAsync(_simulation, new ScaleDivisorOptions(), _allMoleculeAmounts);
            updateScaleDivisors(scaleFactors);
         }
         catch (OperationCanceledException)
         {
            /*canceled*/
         }
         catch (Exception e)
         {
            throw new OSPSuiteException(e.ExceptionMessage(addContactSupportInfo:false));
         }
         finally
         {
            _view.Calculating = false;
         }
      }  

      public void ResetScaleDivisors()
      {
         var scaleFactors = _scaleDivisorCalculator.ResetScaleDivisors(_allMoleculeAmounts);
         updateScaleDivisors(scaleFactors);
      }

      public void UpdateScaleFactorValue(ScaleDivisorDTO scaleDivisorDTO, double newScaleDivisor)
      {
         AddCommand(_moleculeAmountTask.UpdateScaleDivisor(_simulation, scaleDivisorDTO.MoleculeAmount, newScaleDivisor));
      }

      private void updateScaleDivisors(IReadOnlyCollection<ScaleDivisor> scaleDivisors)
      {
         AddCommand(_moleculeAmountTask.UpdateScaleDivisors(_simulation, scaleDivisors));
         _view.RefreshData();
      }
   }
}