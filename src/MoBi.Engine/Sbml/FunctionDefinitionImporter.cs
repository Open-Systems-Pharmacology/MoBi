using System.Collections.Generic;
using libsbmlcs;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using Model = libsbmlcs.Model;

namespace MoBi.Engine.Sbml
{
   public class FunctionDefinitionImporter : SBMLImporter
   {
      private readonly List<FunctionDefinition> _functionDefinitions;
      internal List<FunctionDefinition> FunctionDefinitions { get => _functionDefinitions; }

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
   }
}
