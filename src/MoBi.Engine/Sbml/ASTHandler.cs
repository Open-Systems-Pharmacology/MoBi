using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using libsbmlcs;
using MoBi.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.UnitSystem;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Engine.Sbml
{
   public class ASTHandler
   {
      private IMoBiProject _sbmlProject;
      public List<FunctionDefinition> FunctionDefinitions { get; set; }
      public bool NeedAbsolutePath { get; set; }

      private readonly IObjectPathFactory _objectPathFactory;
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly List<IFormulaUsablePath> _objectPaths;
      private readonly IAliasCreator _aliasCreator;
      private readonly IMoBiDimensionFactory _moBiDimensionFactory;
      private int _counter;
      private IReactionBuilder _reactionBuilder;
      private SBMLInformation _sbmlInformation;

      private readonly Dictionary<string, string> _functionDefDictionary;
      private readonly string[] _forbiddenNames = new[] { "E", "PI" };

      public ASTHandler(IObjectBaseFactory obf, IObjectPathFactory objectPathFactory, IAliasCreator aliasCreator,
         IMoBiDimensionFactory moBiDimensionFactory)
      {
         _objectBaseFactory = obf;
         _objectPathFactory = objectPathFactory;
         _objectPaths = new List<IFormulaUsablePath>();
         _functionDefDictionary = new Dictionary<string, string>();
         _aliasCreator = aliasCreator;
         _moBiDimensionFactory = moBiDimensionFactory;
         _counter = 0;
         NeedAbsolutePath = false;
      }

      /// <summary>
      ///     Parses a SBML MathML expression from a SBML Reaction into a MoBi Formula. 
      /// </summary>
      /// <param name="rootNode"> The root of the MathMl expression. </param>
      /// <param name="reactionBuilder"> The MoBi reactionBuilder the SBML reaction should be build with. </param>
      /// <param name="sbmlProject"></param>
      /// <param name="sbmlInformation"></param>
      public IFormula Parse(ASTNode rootNode, IReactionBuilder reactionBuilder, IMoBiProject sbmlProject,
         SBMLInformation sbmlInformation)
      {
         try
         {
            _objectPaths.Clear();
            _sbmlProject = sbmlProject;
            _sbmlInformation = sbmlInformation;
            _reactionBuilder = reactionBuilder;

            var formulaString = Eval(rootNode);
            if (string.IsNullOrEmpty(formulaString)) return null;

            var formula = _objectBaseFactory.Create<ExplicitFormula>()
               .WithName(SBMLConstants.SBML_KINETIC_LAW + reactionBuilder.Name)
               .WithFormulaString(formulaString);

            foreach (var path in _objectPaths.Where(path => path != null))
            {
               formula.AddObjectPath(path);
            }

            if (string.IsNullOrEmpty(formula?.FormulaString))
            {
               createErrorMsg(rootNode);
               return null;
            }
            else
               return formula;
         }
         finally
         {
            _reactionBuilder = null;
            _sbmlProject = null;
            _sbmlInformation = null;
         }
      }

      /// <summary>
      ///     Creates a Warning if a formula could not have been parsed.
      /// </summary>
      private void createErrorMsg(ASTNode rootNode)
      {
         var msg = new NotificationMessage(_sbmlProject, MessageOrigin.All, null, NotificationType.Warning)
         {
            Message = "Problem occured parsing Formula: " + rootNode.getName() + libsbml.formulaToL3String(rootNode)
         };
         _sbmlInformation.NotificationMessages.Add(msg);
      }

      /// <summary>
      ///     Parses a SBML MathML expression from a SBML Event Assignment into a MoBi Formula. 
      /// </summary>
      /// <param name="rootNode"> The MathMl Expression of the SBML Event Assignment the assignmentVariable should be assigned with.</param>
      /// <param name="eventAssignmentBuilder"> The MoBi Event Assignment Builder the SBML Event Assigment should be build with. </param>
      /// <param name="assignmentVariable"> The Parameter, Molecule, Species or SpeciesReference that should be assigned when the Event is triggered. </param>
      /// <param name="sbmlProject"></param>
      /// <param name="sbmlInformation"></param>
      public IFormula Parse(ASTNode rootNode, IEventAssignmentBuilder eventAssignmentBuilder, string assignmentVariable,
         IMoBiProject sbmlProject, SBMLInformation sbmlInformation)
      {
         try
         {
            _sbmlProject = sbmlProject;
            _sbmlInformation = sbmlInformation;
            _counter++;
            var formulaString = Eval(rootNode);
            if (string.IsNullOrEmpty(formulaString)) return null;

            var formula = _objectBaseFactory.Create<ExplicitFormula>()
               .WithName(SBMLConstants.SBML_EVENT_ASSIGNMENT + assignmentVariable + _counter)
               .WithFormulaString(formulaString);
            foreach (var opath in _objectPaths.Where(opath => opath != null))
            {
               formula.AddObjectPath(opath);
            }

            var path = getObjectPathForAssignment(assignmentVariable);
            if (path != null) eventAssignmentBuilder.ObjectPath = path;

            if (string.IsNullOrEmpty(formula?.FormulaString))
            {
               createErrorMsg(rootNode);
               return null;
            }
            else
               return formula;
         }
         finally
         {
            _sbmlProject = null;
            _sbmlInformation = null;
         }
      }

      /// <summary>
      ///     Parses a SBML MathML expression into a MoBi Formula. 
      /// </summary>
      /// <param name="rootNode"></param>
      /// <param name="rootObjectId"> The id of the parent object of the rootNode to set the name of </param>
      /// <param name="isRateRule"></param>
      /// <param name="sbmlProject"></param>
      /// <param name="sbmlInformation"></param>
      public IFormula Parse(ASTNode rootNode, string rootObjectId, bool isRateRule, IMoBiProject sbmlProject,
         SBMLInformation sbmlInformation)
      {
         try
         {
            _sbmlProject = sbmlProject;
            _sbmlInformation = sbmlInformation;
            _objectPaths.Clear();

            _counter++;
            var formulaString = Eval(rootNode);
            if (string.IsNullOrEmpty(formulaString)) return null;

            var formula = _objectBaseFactory.Create<ExplicitFormula>()
               .WithName(rootObjectId + _counter)
               .WithFormulaString(formulaString);

            if (isRateRule)
            {
               getObjectPathName(ObjectPath.PARENT_CONTAINER, rootObjectId);
            }

            foreach (var path in _objectPaths.Where(path => path != null))
            {
               formula.AddObjectPath(path);
            }
            if (string.IsNullOrEmpty(formula?.FormulaString))
            {
               createErrorMsg(rootNode);
               return null;
            }
            else
               return formula;
         }
         finally
         {
            _sbmlProject = null;
            _sbmlInformation = null;
         }
      }

      public IFormula Parse(ASTNode rootNode, string rootObjectId, IMoBiProject sbmlProject,
         SBMLInformation sbmlInformation)
      {
         try
         {
            _sbmlProject = sbmlProject;
            _sbmlInformation = sbmlInformation;
            _objectPaths.Clear();

            _counter++;
            var formulaString = Eval(rootNode);
            if (string.IsNullOrEmpty(formulaString)) return null;

            var formula = _objectBaseFactory.Create<ExplicitFormula>()
               .WithName(SBMLConstants.FORMULA + SBMLConstants.SPACE + rootObjectId + _counter)
               .WithFormulaString(formulaString);

            foreach (var path in _objectPaths.Where(path => path != null))
            {
               formula.AddObjectPath(path);
            }
            if (string.IsNullOrEmpty(formula?.FormulaString))
            {
               createErrorMsg(rootNode);
               return null;
            }
            else
               return formula;
         }
         finally
         {
            _sbmlProject = null;
            _sbmlInformation = null;
         }
      }

      public string Eval(ASTNode rootNode)
      {
         if (rootNode == null) return String.Empty;
         var res = "";
         var nodeType = rootNode.getType();

         switch (nodeType)
         {
            //Binop
            case (libsbml.AST_MINUS):
               res = parseMinus(rootNode);
               break;
            case (libsbml.AST_DIVIDE):
               res = parseDivide(rootNode);
               break;
            case (libsbml.AST_POWER):
               res = parsePower(rootNode);
               break;
            case (libsbml.AST_FUNCTION_POWER):
               res = parsePower(rootNode);
               break;
            case (libsbml.AST_FUNCTION_DELAY):
               createFeatureNotSupportedMessage("Delay");
               break;
            case (libsbml.AST_FUNCTION_ROOT):
               createFeatureNotSupportedMessage("Root");
               break;

            //"Multiop"
            case (libsbml.AST_PLUS):
               res = parsePlus(rootNode);
               break;
            case (libsbml.AST_TIMES):
               res = parseTimes(rootNode);
               break;
            case (libsbml.AST_FUNCTION_PIECEWISE):
               createFeatureNotSupportedMessage("Piecewise");
               break;

            //Logical Operator
            case (libsbml.AST_LOGICAL_AND):
               res = parseAnd(rootNode);
               break;
            case (libsbml.AST_LOGICAL_OR):
               res = parseOr(rootNode);
               break;
            case (libsbml.AST_LOGICAL_NOT):
               res = parseNot(rootNode);
               break;
            case (libsbml.AST_LOGICAL_XOR):
               res = parseXor(rootNode);
               break;

            //Relational Operator
            case (libsbml.AST_RELATIONAL_EQ):
               res = parseEqual(rootNode);
               break;
            case (libsbml.AST_RELATIONAL_GEQ):
               res = parseGeq(rootNode);
               break;
            case (libsbml.AST_RELATIONAL_LEQ):
               res = parseLeq(rootNode);
               break;
            case (libsbml.AST_RELATIONAL_GT):
               res = parseGt(rootNode);
               break;
            case (libsbml.AST_RELATIONAL_LT):
               res = parseLt(rootNode);
               break;
            case (libsbml.AST_RELATIONAL_NEQ):
               res = parseNeq(rootNode);
               break;

            //Function
            case (libsbml.AST_FUNCTION):
               res = parseFunction(rootNode);
               break;
            case (libsbml.AST_FUNCTION_LN):
               res = parseLn(rootNode);
               break;
            // log x (basis 10) or log(base x, y)
            case (libsbml.AST_FUNCTION_LOG):
               res = parseLog(rootNode);
               break;
            case (libsbml.AST_FUNCTION_EXP):
               res = parseExp(rootNode);
               break;

            //Sin, Cos, Tan
            case (libsbml.AST_FUNCTION_COS):
               res = parseTrigonometric(rootNode);
               break;
            case (libsbml.AST_FUNCTION_ARCCOS):
               res = parseTrigonometric(rootNode);
               break;
            case (libsbml.AST_FUNCTION_COSH):
               res = parseTrigonometric(rootNode);
               break;
            case (libsbml.AST_FUNCTION_SIN):
               res = parseTrigonometric(rootNode);
               break;
            case (libsbml.AST_FUNCTION_ARCSIN):
               res = parseTrigonometric(rootNode);
               break;
            case (libsbml.AST_FUNCTION_SINH):
               res = parseTrigonometric(rootNode);
               break;
            case (libsbml.AST_FUNCTION_TAN):
               res = parseTrigonometric(rootNode);
               break;
            case (libsbml.AST_FUNCTION_ARCTAN):
               res = parseTrigonometric(rootNode);
               break;
            case (libsbml.AST_FUNCTION_TANH):
               res = parseTrigonometric(rootNode);
               break;

            // <ci> 
            case (libsbml.AST_NAME):
               res = parseName(rootNode);
               break;
            case (libsbml.AST_NAME_AVOGADRO):
               res = SBMLConstants.LBRACE + "6,02214129 * 10^23" + SBMLConstants.RBRACE;
               break;

            // <cn>
            case (libsbml.AST_REAL):
               res = parseReal(rootNode);
               break;
            case (libsbml.AST_REAL_E):
               //Real number with e-notation (MathML <cn type="e-notation"> [number] <sep/> [number] </cn>)
               res = parseReal(rootNode);
               //res = SBMLConstants.LBRACE + rootNode.getReal().ToString(CultureInfo.InvariantCulture) + SBMLConstants.RBRACE;
               break;
            case (libsbml.AST_INTEGER): //<cn type="integer">
               res = parseInteger(rootNode);
               break;
            case (libsbml.AST_RATIONAL):
               res = parseRational(rootNode);
               break;

            //SBMLConstants
            case (libsbml.AST_CONSTANT_E):
               res = "E";
               break;
            case (libsbml.AST_CONSTANT_PI):
               res = "PI";
               break;
            case (libsbml.AST_CONSTANT_TRUE):
               res = "1";
               break;
            case (libsbml.AST_CONSTANT_FALSE):
               res = "0";
               break;

            //Not supported
            case (libsbml.AST_FUNCTION_ABS):
               createFeatureNotSupportedMessage("Abs");
               break;
            case (libsbml.AST_FUNCTION_FLOOR):
               createFeatureNotSupportedMessage("Floor");
               break;
            case (libsbml.AST_FUNCTION_CEILING):
               createFeatureNotSupportedMessage("Ceiling");
               break;

            case (libsbml.AST_NAME_TIME):
               res = parseTime();
               break;
         }
         return res;
      }

      private void createFeatureNotSupportedMessage(string functionNotSupported)
      {
         var msg = new NotificationMessage(_sbmlProject, MessageOrigin.All, null, NotificationType.Warning)
         {
            Message =
               SBMLConstants.SBML_FEATURE_NOT_SUPPORTED + ": MathML Function '" + functionNotSupported +
               "' not supported."
         };
         _sbmlInformation.NotificationMessages.Add(msg);
      }

      /// <summary>
      ///     Parses an Integer.
      /// </summary>
      private string parseInteger(ASTNode rootNode)
      {
         if (rootNode.getInteger().ToString(CultureInfo.InvariantCulture).Contains("-"))
            return SBMLConstants.LBRACE + rootNode.getInteger().ToString(CultureInfo.InvariantCulture) +
                   SBMLConstants.RBRACE;
         return rootNode.getInteger().ToString(CultureInfo.InvariantCulture);
      }

      /// <summary>
      ///     Parses a real.
      /// </summary>
      private string parseReal(ASTNode rootNode)
      {
         if (rootNode.getReal().ToString(CultureInfo.InvariantCulture).Contains("-"))
            return SBMLConstants.LBRACE + rootNode.getReal().ToString(CultureInfo.InvariantCulture) +
                   SBMLConstants.RBRACE;
         return rootNode.getReal().ToString(CultureInfo.InvariantCulture);
      }

      /// <summary>
      ///     Parses a rational.
      /// </summary>
      private string parseRational(ASTNode rootNode)
      {
         var denominator = rootNode.getDenominator();
         var numerator = rootNode.getNumerator();
         return SBMLConstants.LBRACE + numerator + SBMLConstants.DIVIDE + denominator + SBMLConstants.RBRACE;
      }

      /// <summary>
      ///     Gets the "Time" attribute of the MoBi simulation.
      /// </summary>
      private string parseTime()
      {
         if (!_objectPaths.Exists(x => x.Alias == Constants.Dimension.TIME))
            _objectPaths.Add(
               _objectPathFactory.CreateTimePath(_moBiDimensionFactory.Dimension(Constants.Dimension.TIME)));
         return Constants.Dimension.TIME;
      }

      /// <summary>
      ///     Checks the possibilites of a variable:
      ///         1.) Function Definition 
      ///         2.) Local Parameter of the reaction
      ///         3.) Global Parameter (Top Container)
      ///         4.) Molecule
      ///         5.) Container size Parameter
      /// </summary>
      private string parseName(ASTNode rootNode)
      {
         var tmp = rootNode.getName();

         if (_functionDefDictionary.ContainsKey(tmp))
            return _functionDefDictionary[tmp];

         var res = checkFunctionDefinitions(rootNode);
         if (!string.IsNullOrEmpty(res)) return res;

         res = getLocalReactionParamIfExistant(rootNode);
         if (!string.IsNullOrEmpty(res)) return res;

         res = getMoleculeIfExistant(rootNode);
         if (!string.IsNullOrEmpty(res)) return res;

         res = getGlobalParamIfExistant(rootNode);
         if (!string.IsNullOrEmpty(res)) return res;

         res = getContainerSizeParamIfExistant(rootNode);
         return res;
      }

      private string parseUserDefinedFunction(ASTNode rootNode, string functionId)
      {
         FunctionDefinition function = null;
         foreach (var funcDef in FunctionDefinitions.Where(funcDef => funcDef.getId() == functionId))
            function = funcDef;

         if (function?.getNumArguments() != rootNode.getNumChildren())
            return string.Empty; //error 

         var tmpDictionary = new Dictionary<string, string>();
         for (long i = 0; i < function.getNumArguments(); i++)
         {
            var argument = function.getArgument(i).getName();
            var replacement = Eval(rootNode.getChild(i));
            tmpDictionary.Add(argument, replacement);
            _functionDefDictionary[argument] = replacement;
         }
         foreach (var entry in tmpDictionary)
         {
            _functionDefDictionary[entry.Key] = entry.Value;
         }
         var res = Eval(function.getBody());
         return res;
      }

      /// <summary>
      ///     Parses a user defined function by looking it up in the pre-imported user defined 
      ///     functions. 
      /// </summary>
      private string parseFunction(ASTNode rootNode)
      {
         if (FunctionDefinitions == null) return String.Empty;
         foreach (var func in FunctionDefinitions.Where(func => rootNode.getName() == func.getId()))
         {
            return parseUserDefinedFunction(rootNode, func.getId());
         }
         return string.Empty;
      }

      /// <summary>
      ///     Checks if a given ASTNode is a SBML FunctionDefinition and trys to parse it into a 
      ///     MoBi Formula. 
      /// </summary>
      private string checkFunctionDefinitions(ASTNode rootNode)
      {
         if (FunctionDefinitions == null)
            return string.Empty;

         foreach (var function in FunctionDefinitions)
         {
            try
            {
               if (function.isSetName())
               {
                  if (rootNode.getName() == function.getName())
                     return parseUserDefinedFunction(rootNode, function.getId());
               }
               else if (function.isSetId())
               {
                  if (rootNode.getName() == function.getId())
                     return parseUserDefinedFunction(rootNode, function.getId());
               }
            }
            catch (AccessViolationException)
            {
            }
         }
         return string.Empty;
      }

      private string parseMinus(ASTNode rootNode)
      {
         if (rootNode.isUMinus())
            return SBMLConstants.LBRACE + SBMLConstants.MINUS + Eval(rootNode.getLeftChild()) + SBMLConstants.RBRACE;
         return SBMLConstants.LBRACE + SBMLConstants.LBRACE + Eval(rootNode.getLeftChild()) + SBMLConstants.RBRACE +
                SBMLConstants.MINUS + SBMLConstants.LBRACE + Eval(rootNode.getRightChild()) + SBMLConstants.RBRACE +
                SBMLConstants.RBRACE;
      }

      private string parseDivide(ASTNode rootNode)
      {
         return SBMLConstants.LBRACE + SBMLConstants.LBRACE + Eval(rootNode.getLeftChild()) + SBMLConstants.RBRACE +
                SBMLConstants.DIVIDE + SBMLConstants.LBRACE + Eval(rootNode.getRightChild()) + SBMLConstants.RBRACE +
                SBMLConstants.RBRACE;
      }

      private string parsePower(ASTNode rootNode)
      {
         return SBMLConstants.LBRACE + Eval(rootNode.getLeftChild()) + SBMLConstants.POW + SBMLConstants.LBRACE +
                Eval(rootNode.getRightChild()) + SBMLConstants.RBRACE + SBMLConstants.RBRACE;
      }

      private string parseExp(ASTNode rootNode)
      {
         if (rootNode.getNumChildren() == 1)
            return SBMLConstants.LBRACE + SBMLConstants.EXP + SBMLConstants.POW + Eval(rootNode.getChild(0)) +
                   SBMLConstants.RBRACE;
         return "";
      }

      private string parsePlus(ASTNode rootNode)
      {
         var res = "";
         for (long i = 0; i < rootNode.getNumChildren(); i++)
         {
            if (res == "")
               res = SBMLConstants.LBRACE + Eval(rootNode.getChild(i));
            else
               res += SBMLConstants.PLUS + Eval(rootNode.getChild(i));
         }
         res += SBMLConstants.RBRACE;
         return res;
      }

      private string parseTimes(ASTNode rootNode)
      {
         var res = "";
         for (long i = 0; i < rootNode.getNumChildren(); i++)
         {
            if (res == "")
            {
               res = Eval(rootNode.getChild(i));
            }
            else
            {
               res += SBMLConstants.TIMES + Eval(rootNode.getChild(i));
            }
         }
         return res;
      }

      private string parseAnd(ASTNode rootNode)
      {
         var res = "";
         for (long i = 0; i < rootNode.getNumChildren(); i++)
         {
            if (res == "")
               res = Eval(rootNode.getChild(i));
            else
               res += SBMLConstants.AND + Eval(rootNode.getChild(i));
         }
         return res;
      }

      private string parseOr(ASTNode rootNode)
      {
         var res = "";
         for (long i = 0; i < rootNode.getNumChildren(); i++)
         {
            if (res == "")
               res = Eval(rootNode.getChild(i));
            else
               res += SBMLConstants.OR + Eval(rootNode.getChild(i));
         }
         return res;
      }

      private string parseNot(ASTNode rootNode)
      {
         var res = "";
         for (long i = 0; i < rootNode.getNumChildren(); i++)
         {
            res += SBMLConstants.NOT + Eval(rootNode.getChild(i));
         }
         return res;
      }

      private string parseXor(ASTNode rootNode)
      {
         var a = "";
         var res = "";
         for (long i = 0; i < rootNode.getNumChildren(); i++)
         {
            string b;
            if (string.IsNullOrEmpty(a))
            {
               a = Eval(rootNode.getChild(i));
               i++;
               b = Eval(rootNode.getChild(i));
               res += SBMLConstants.LBRACE + a + SBMLConstants.OR + b + SBMLConstants.RBRACE + SBMLConstants.AND +
                      SBMLConstants.LBRACE + SBMLConstants.NOT + a + SBMLConstants.NOT + b + SBMLConstants.RBRACE;
               a = res;
            }
            b = Eval(rootNode.getChild(i));
            res += SBMLConstants.LBRACE + a + SBMLConstants.OR + b + SBMLConstants.RBRACE + SBMLConstants.AND +
                   SBMLConstants.LBRACE + SBMLConstants.NOT + a + SBMLConstants.NOT + b + SBMLConstants.RBRACE;
            a = res;
         }
         return res;
      }

      private string parseEqual(ASTNode rootNode)
      {
         var res = "";
         for (long i = 0; i < rootNode.getNumChildren(); i++)
         {
            if (res == "")
               res = Eval(rootNode.getChild(i));
            else
               res += SBMLConstants.EQUAL + Eval(rootNode.getChild(i));
         }
         return res;
      }

      private string parseGeq(ASTNode rootNode)
      {
         var res = "";
         for (long i = 0; i < rootNode.getNumChildren(); i++)
         {
            if (res == "")
               res = Eval(rootNode.getChild(i));
            else
               res += SBMLConstants.GEQ + Eval(rootNode.getChild(i));
         }
         return res;
      }

      private string parseLeq(ASTNode rootNode)
      {
         var res = "";
         for (long i = 0; i < rootNode.getNumChildren(); i++)
         {
            if (res == "")
               res = Eval(rootNode.getChild(i));
            else
               res += SBMLConstants.LEQ + Eval(rootNode.getChild(i));
         }
         return res;
      }

      private string parseGt(ASTNode rootNode)
      {
         var res = "";
         for (long i = 0; i < rootNode.getNumChildren(); i++)
         {
            if (res == "")
               res = Eval(rootNode.getChild(i));
            else
               res += SBMLConstants.GT + Eval(rootNode.getChild(i));
         }
         return res;
      }

      private string parseLt(ASTNode rootNode)
      {
         var res = "";
         for (long i = 0; i < rootNode.getNumChildren(); i++)
         {
            if (res == "")
               res = Eval(rootNode.getChild(i));
            else
               res += SBMLConstants.LT + Eval(rootNode.getChild(i));
         }
         return res;
      }

      private string parseNeq(ASTNode rootNode)
      {
         var res = "";
         for (long i = 0; i < rootNode.getNumChildren(); i++)
         {
            if (res == "")
               res = Eval(rootNode.getChild(i));
            else
               res += SBMLConstants.UNEQUAL + Eval(rootNode.getChild(i));
         }
         return res;
      }

      private string parseTrigonometric(ASTNode rootNode)
      {
         var res = "";
         for (long i = 0; i < rootNode.getNumChildren(); i++)
         {
            //sin, cos, tan
            if (rootNode.getType() == libsbml.AST_FUNCTION_SIN)
               res += SBMLConstants.SIN + SBMLConstants.LBRACE + Eval(rootNode.getChild(i)) + SBMLConstants.RBRACE;
            if (rootNode.getType() == libsbml.AST_FUNCTION_COS)
               res += SBMLConstants.COS + SBMLConstants.LBRACE + Eval(rootNode.getChild(i)) + SBMLConstants.RBRACE;
            if (rootNode.getType() == libsbml.AST_FUNCTION_TAN)
               res += SBMLConstants.TAN + SBMLConstants.LBRACE + Eval(rootNode.getChild(i)) + SBMLConstants.RBRACE;
            //sinh, cosh, tanh
            if (rootNode.getType() == libsbml.AST_FUNCTION_SINH)
               res += SBMLConstants.SINH + SBMLConstants.LBRACE + Eval(rootNode.getChild(i)) + SBMLConstants.RBRACE;
            if (rootNode.getType() == libsbml.AST_FUNCTION_COSH)
               res += SBMLConstants.COSH + SBMLConstants.LBRACE + Eval(rootNode.getChild(i)) + SBMLConstants.RBRACE;
            if (rootNode.getType() == libsbml.AST_FUNCTION_TANH)
               res += SBMLConstants.TANH + SBMLConstants.LBRACE + Eval(rootNode.getChild(i)) + SBMLConstants.RBRACE;
            //Asin, Acos, Atan
            if (rootNode.getType() == libsbml.AST_FUNCTION_ARCSIN)
               res += SBMLConstants.ASIN + SBMLConstants.LBRACE + Eval(rootNode.getChild(i)) + SBMLConstants.RBRACE;
            if (rootNode.getType() == libsbml.AST_FUNCTION_ARCCOS)
               res += SBMLConstants.ACOS + SBMLConstants.LBRACE + Eval(rootNode.getChild(i)) + SBMLConstants.RBRACE;
            if (rootNode.getType() == libsbml.AST_FUNCTION_ARCTAN)
               res += SBMLConstants.ATAN + SBMLConstants.LBRACE + Eval(rootNode.getChild(i)) + SBMLConstants.RBRACE;
         }
         return res;
      }

      private string parseLn(ASTNode rootNode)
      {
         var res = "";
         for (long i = 0; i < rootNode.getNumChildren(); i++)
         {
            res += SBMLConstants.LN + SBMLConstants.LBRACE + Eval(rootNode.getChild(i)) + SBMLConstants.RBRACE;
         }
         return res;
      }

      private string parseLog(ASTNode rootNode)
      {
         if (rootNode.getNumChildren() == 1) return parseLn(rootNode);
         return SBMLConstants.LOG + SBMLConstants.LBRACE + Eval(rootNode.getLeftChild()) + SBMLConstants.DELIMITER +
                Eval(rootNode.getRightChild()) + SBMLConstants.RBRACE;
      }

      /// <summary>
      ///     Checks if a object path for the given objectName is already existant and creates a new one if not.
      /// </summary>
      private string getObjectPathName(string parentContainer, string objectName)
      {
         if (objectPathExistent(objectName))
            return objectName;

         var alias = createAliasFrom(objectName);
         _objectPaths.Add(_objectPathFactory.CreateFormulaUsablePathFrom(parentContainer, objectName).WithAlias(alias));
         return alias;
      }

      /// <summary>
      ///     Checks if a object path for the given objectName is already existant and creates a new one if not.
      /// </summary>
      private string getObjectPathName(string topContainer, string parentContainer, string objectName)
      {
         if (objectPathExistent(objectName))
            return objectName;

         var alias = createAliasFrom(objectName);
         var path =
            (_objectPathFactory.CreateFormulaUsablePathFrom(topContainer, parentContainer, objectName).WithAlias(alias));
         _objectPaths.Add(path);
         return alias;
      }

      private string createAliasFrom(string objectName)
      {
         if (_forbiddenNames.Contains(objectName.ToUpperInvariant()))
            objectName = $"{objectName}_";
         return _aliasCreator.CreateAliasFrom(objectName, _objectPaths.Select(x => x.Alias));
      }

      /// <summary>
      ///     Checks if the ObjectPath to a given Object is already existant. 
      /// </summary>
      /// <returns> True, if the ObjectPath exists, else false. </returns>
      private bool objectPathExistent(string name)
      {
         var existant = _objectPaths.Any(obj => obj.Alias == name);
         return existant;
      }

      /// <summary>
      ///     Gets the MoBi Object Path to the given variable
      ///     or creates a new one if not  existant.
      /// </summary>
      private IFormulaUsablePath getObjectPathForAssignment(string assignmentVariable)
      {
         if (_sbmlInformation.MoleculeInformation.Any(info => info.SpeciesIds.Exists(s => s == assignmentVariable)))
         {
            var molinfo =
               _sbmlInformation.MoleculeInformation.FirstOrDefault(info => info.SpeciesIds.Contains(assignmentVariable));
            var molecule = molinfo?.GetMoleculeBuilder();
            if (molecule != null)
            {
               var pathName = getObjectPathNameOfMolecule(molecule, molinfo.GetSpeciesIfOne());
               return _objectPaths.Find(x => x.Alias == pathName);
            }
         }

         var tc = GetMainTopContainer();
         if (tc == null) return null;

         //Container size?
         var container = tc.GetAllChildren<IContainer>();
         foreach (var compartment in from compartment in container
                                     where compartment.Name == assignmentVariable
                                     from compartmentParameter in compartment.Children
                                     where compartmentParameter.Name == SBMLConstants.SIZE
                                     select compartment)
         {
            var pathName = getObjectPathName(tc.Name, compartment.Name, SBMLConstants.SIZE);
            return _objectPaths.Find(x => x.Alias == pathName);
         }

         //Global Parameter?
         var parameter = tc.GetAllChildren<IParameter>();
         foreach (var param in parameter.Where(param => param.Name == assignmentVariable))
         {
            var pathName = getObjectPathName(tc.Name, param.Name);
            return _objectPaths.Find(x => x.Alias == pathName);
         }

         //Species Reference?
         isSpeciesReference(assignmentVariable);

         return null;
      }

      /// <summary>
      ///     Gets the object path name of a Molecule.
      /// </summary>
      private string getObjectPathNameOfMolecule(IMoleculeBuilder molecule, Species species)
      {
         //reaction 
         if (NeedAbsolutePath == false)
         {
            return getObjectPathName(ObjectPath.PARENT_CONTAINER, molecule.Name);
         }

         if (_sbmlInformation.MoleculeInformation.All(info => info.GetMoleculeBuilderName() != molecule.Name))
            return string.Empty;
         var molInfo = _sbmlInformation.MoleculeInformation.Find(info => info.GetMoleculeBuilderName() == molecule.Name);

         var parentContainer = GetContainerFromCompartment(molInfo.GetCompartment(species));
         var alias = createAliasFrom(molecule.Name);
         if (parentContainer != null)
         {
            var path =
               (_objectPathFactory.CreateFormulaUsablePathFrom(GetMainTopContainer().Name, parentContainer.Name,
                  molecule.Name).WithAlias(alias));
            _objectPaths.Add(path);
         }
         return alias;
      }

      /// <summary>
      ///     Throws an error Method if the rule should set the stoichiometry of a Species. 
      ///     Formulas in Stoichiometry are not supported. 
      /// </summary>
      private void isSpeciesReference(string assignmentVariable)
      {
         foreach (var speciesReference in _sbmlInformation.SpeciesReferences)
         {
            if (speciesReference.getId() != assignmentVariable) continue;
            var msg =
               new NotificationMessage(_sbmlProject, MessageOrigin.All, null, NotificationType.Warning).WithName(
                  SBMLConstants.SBML_FEATURE_NOT_SUPPORTED);
            msg.Description = "Stoichiometry of " + speciesReference.getId() + " was set to default value: " +
                              SBMLConstants.SBML_STOICHIOMETRY_DEFAULT;
            _sbmlInformation.NotificationMessages.Add(msg);
         }
      }

      /// <summary>
      ///     Checks if the node is a Container size Parameter.
      /// </summary>
      /// <returns>
      ///     Id of the Container size Parameter, if existant.
      ///     String.Empty, if the node is no Container size Parameter.
      /// </returns>
      private string getContainerSizeParamIfExistant(ASTNode rootNode)
      {
         var container = GetMainTopContainer().GetAllChildren<IContainer>();
         foreach (var compartment in container)
         {
            foreach (var compartmentParameter in compartment.Children)
            {
               if ((compartment.Name == rootNode.getName()) && (compartmentParameter.Name == SBMLConstants.SIZE))
                  return getObjectPathName(GetMainTopContainer().Name, compartment.Name, SBMLConstants.SIZE);
            }
         }
         return string.Empty;
      }

      /// <summary>
      ///     Checks if the node is a global Parameter (in the TopContainer).
      /// </summary>
      /// <returns>        
      ///     Id of the global Parameter, if existant.
      ///     String.Empty, if the node is no global Parameter.
      /// </returns>
      private string getGlobalParamIfExistant(ASTNode rootNode)
      {
         var parameter = GetMainTopContainer().GetAllChildren<IParameter>();
         var name = rootNode.getId() != String.Empty ? rootNode.getId() : rootNode.getName();
         return parameter.ExistsByName(name) ? getObjectPathName(GetMainTopContainer().ToString(), name) : string.Empty;
      }

      /// <summary>
      ///     Checks if the node is a molecule.
      /// </summary>
      /// <returns>
      ///     Id of the molecule, if existant.
      ///     String.Empty, if the node is no molecule.
      /// </returns>
      private string getMoleculeIfExistant(ASTNode rootNode)
      {
         //SpeciesReference
         var path = checkSpeciesReference(rootNode);
         if (!string.IsNullOrEmpty(path)) return path;

         var speciesId = rootNode.getName();
         if (_sbmlInformation.MoleculeInformation.Any(info => info.SpeciesIds.Exists(s => s == speciesId)))
         {
            var molInfo = _sbmlInformation.MoleculeInformation.Find(info => info.SpeciesIds.Contains(speciesId));
            return getObjectPathNameOfMolecule(molInfo.GetMoleculeBuilder(), molInfo.GetSpeciesIfOne());
         }

         return string.Empty;
      }


      /// <summary>
      ///     Checks if a rootNode is a SpeciesReference.
      /// </summary>
      private string checkSpeciesReference(ASTNode rootNode)
      {
         var isSpeciesReference = _sbmlInformation.SpeciesReferences.Exists(x => x.getId() == rootNode.getName());
         if (!isSpeciesReference) return string.Empty;
         var speciesRef = _sbmlInformation.SpeciesReferences.Find(x => x.getId() == rootNode.getName());
         var molinfo =
            _sbmlInformation.MoleculeInformation.FirstOrDefault(
               info => info.SpeciesIds.Contains(speciesRef.getSpecies()));

         return molinfo == null
            ? String.Empty
            : getObjectPathNameOfMolecule(molinfo.GetMoleculeBuilder(), molinfo.GetSpeciesIfOne());
      }

      /// <summary>
      ///     Checks if the node is a local reaction parameter.
      /// </summary>
      /// <returns> 
      ///     Id of the parameter, if existant.
      ///     String.Empty, if the node is no local reaction parameter.
      /// </returns>
      private string getLocalReactionParamIfExistant(ASTNode rootNode)
      {
         if (_reactionBuilder == null) return String.Empty;
         if (!_reactionBuilder.Parameters.ExistsByName(rootNode.getName())) return String.Empty;

         var localParameter = _reactionBuilder.Parameters.FindByName(rootNode.getName());
         //FindById(rootNode.getName());
         var path = _objectPathFactory.CreateFormulaUsablePathFrom(localParameter.Name);
         _objectPaths.Add(path);
         return localParameter.Name;
      }

      /// <summary>
      ///     Gets the MoBi Top Container generated for the SBML Import.
      /// </summary>
      public IContainer GetMainTopContainer()
      {
         return
            _sbmlProject.SpatialStructureCollection.Select(
               ss => ss.TopContainers.FindById(SBMLConstants.SBML_TOP_CONTAINER)).FirstOrDefault();
      }

      /// <summary>
      ///     Gets the Mobi Container by a given SBML Compartment.
      /// </summary>
      public IEntity GetContainerFromCompartment(string compartmentId)
      {
         var ssCollection = _sbmlProject.SpatialStructureCollection;
         return
            (from ss in ssCollection
             from topContainer in ss.TopContainers
             from container in topContainer.Children
             select container).FirstOrDefault(container => container.Name == compartmentId);
      }

      /// <summary>
      ///     Gets the MoBi Molecule Building Block generated for the SBML Import.
      /// </summary>
      public IMoleculeBuildingBlock GetMainMoleculeBuildingBlock()
      {
         return _sbmlProject.MoleculeBlockCollection.FirstOrDefault(mb => mb.Name == SBMLConstants.SBML_SPECIES_BB);
      }
   }
}