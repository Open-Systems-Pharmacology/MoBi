using System.Linq;
using libsbmlcs;
using MoBi.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using Model = libsbmlcs.Model;

namespace MoBi.Engine.Sbml
{
   public abstract class AssignmentImporterBase : SBMLImporter
   {
      protected AssignmentImporterBase(IObjectPathFactory objectPathFactory, IObjectBaseFactory objectBaseFactory, ASTHandler astHandler, IMoBiContext context) : base(objectPathFactory, objectBaseFactory, astHandler, context)
      {
         _astHandler.NeedAbsolutePath = true;
      }

      public bool IsSpeciesAssignment(string symbol)
      {
         var moleculeExistant = false;
         foreach (var mbb in _sbmlProject.MoleculeBlockCollection)
         {
            if (mbb.ExistsByName(symbol))
               moleculeExistant = true;
         }
         return moleculeExistant;
      }

      protected internal bool IsParameter(string id)
      {
         var topcontainerParameter = GetMainTopContainer().GetAllChildren<IParameter>();
         return topcontainerParameter.Any(param => param.Name == id);
      }

      protected internal IParameter GetParameter(string id)
      {
         var topcontainerParameter = GetMainTopContainer().GetAllChildren<IParameter>();
         return topcontainerParameter.FirstOrDefault(param => param.Name == id);
      }

      protected internal bool IsContainerSizeParameter(string id)
      {
         var containerList = GetMainTopContainer().GetAllChildren<OSPSuite.Core.Domain.IContainer>();
         return (from container in containerList
            where container.Name == id
                 select container.GetAllChildren<IParameter>().ExistsByName(SBMLConstants.SIZE)).FirstOrDefault();
      }

      protected internal IParameter GetContainerSizeParameter(string id)
      {
         var topcontainerContainer = GetMainTopContainer().GetAllChildren<OSPSuite.Core.Domain.IContainer>();
         foreach (var container in topcontainerContainer)
         {
            if (container.Name == id)
            {
               return container.GetAllChildren<IParameter>().FirstOrDefault(param => param.Name == SBMLConstants.SIZE);
            }
         }
         return null;
      }

      protected void CreateErrorMsg()
      {
         var msg = new NotificationMessage(GetMainParameterStartValuesBuildingBlock(), MessageOrigin.All, null,
            NotificationType.Warning)
         {
            Message = "Something with SBML Assignments or SBML Rules went wrong."
         };
         _sbmlInformation.NotificationMessages.Add(msg);
      }

      /// <summary>
      ///     Sets the Parameter Start Value of a given Parameter to the given Math Formula.
      /// </summary>
      protected internal void SetPSV(ASTNode math, IParameter parameter, string containerName)
      {
         if (parameter == null)
         {
            CreateErrorMsg();
            return;
         }

         var formula = _astHandler.Parse(math, parameter.Name, false, _sbmlProject,_sbmlInformation);
         if (formula == null) return;

         var psvbb = GetMainParameterStartValuesBuildingBlock();
         if (psvbb == null) return;
         psvbb.AddFormula(formula);

         foreach (var declaredPSV in psvbb.Where(declaredPSV => declaredPSV.Name == parameter.Name))
         {
            if (string.IsNullOrEmpty(containerName))
            {
               declaredPSV.Formula = formula;
               return;
            }
            if (!declaredPSV.Path.Contains(containerName)) continue;
            declaredPSV.Formula = formula;
            return;
         }

         var psv = ObjectBaseFactory.Create<ParameterStartValue>()
            .WithName(parameter.Name)
            .WithFormula(formula)
            .WithDimension(parameter.Dimension);

         psvbb.Add(psv);
      }

      /// <summary>
      ///     Executes a Species Inital Assignment.
      /// </summary>
      public void DoSpeciesAssignment(string symbol, ASTNode math, bool isInitialAssignment)
      {
         SetMSV(symbol, math, isInitialAssignment);
      }

      /// <summary>
      ///     Overwrites a MSV to import an Initial Assignment.
      /// </summary>
      private void SetMSV(string symbol, ASTNode math, bool isInitialAssignment)
      {
         var addon = SBMLConstants.MSV + SBMLConstants.SBML_INITIAL_ASSIGNMENT;
         if (!isInitialAssignment) addon = SBMLConstants.SBML_ASSIGNMENT;

         var preciseId = addon + symbol;
         var formula = _astHandler.Parse(math, preciseId, false, _sbmlProject,_sbmlInformation);

         foreach (var msv in GetMainMSVBuildingBlock())
         {
            if (msv.Name != symbol) continue;
            if (formula != null) msv.Formula = formula;
            msv.IsPresent = true;
            return;
         }

         GetMainMSVBuildingBlock().AddFormula(formula);
      }

      /// <summary>
      ///     Checks if the given Initial Assignment wants to assign a stoichiometry (species Reference).
      ///     This is not supported and causes a Notification.
      /// </summary>
      public void CheckSpeciesReferences(string assignmentId, string paramName, Model model)
      {
         foreach (var sp in _sbmlInformation.SpeciesReferences)
         {
            if (sp.getId() != paramName) continue;

            var msg = new NotificationMessage(GetMainSpatialStructure(model), MessageOrigin.All, null,
               NotificationType.Warning)
            {
               Message = SBMLConstants.SBML_FEATURE_NOT_SUPPORTED + ": Stoichiometry of " + assignmentId +
                         " was set to default value: " + SBMLConstants.SBML_STOICHIOMETRY_DEFAULT
            };
            _sbmlInformation.NotificationMessages.Add(msg);
         }
      }
   }
}