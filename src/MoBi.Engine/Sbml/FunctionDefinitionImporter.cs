using System.Collections.Generic;
using libsbmlcs;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Utility;
using Model = libsbmlcs.Model;

namespace MoBi.Engine.Sbml
{
   public interface IFunctionDefinitionImporter : ISBMLImporter
   {
      List<FunctionDefinition> FunctionDefinitions { get; }
   }

   public class FunctionDefinitionImporter : SBMLImporter, IStartable, IFunctionDefinitionImporter
   {
      private readonly List<FunctionDefinition> _functionDefinitions;
      public List<FunctionDefinition> FunctionDefinitions { get => _functionDefinitions; }

      public FunctionDefinitionImporter(IObjectPathFactory objectPathFactory, IObjectBaseFactory objectBaseFactory, ASTHandler astHandler, IMoBiContext context) : base(objectPathFactory, objectBaseFactory, astHandler, context)
      {
         _functionDefinitions = new List<FunctionDefinition>();
      }

      protected override void Import(Model model)
      {
         for (long i = 0; i < model.getNumFunctionDefinitions(); i++)
         {
            _functionDefinitions.Add(model.getFunctionDefinition(i));
         }
      }

      public override void AddToProject()
      {
      }

      public void Start()
      {
         _functionDefinitions.Clear();
      }
   }
}
