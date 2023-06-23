using System;

namespace MoBi.Engine.Sbml
{
   static class SBMLConstants
   {
      public const string DIMENSION = "Dimension";
      public const string UNIT = "Unit";
      public static string AREA = "Area";
      public static string LENGTH = "Length";
      public static string DEFAULT_PROJECT_NAME = "SBML project";
      public static string FORMULA = "Formula";
      public const string SBML_EVENT_ASSIGNMENT = "SBML Event Assignment ";
      public const string SBML_EVENTBUILDER = "EVENTBUILDER";
      public const string SBML_DEFAULTEVENTNAME = "Default Event";

      //Building Block Names
      public const string SBML_REACTION_BB = "SBML Reactions";
      public const string SBML_SPECIES_BB = "SBML Species";
      public const string SBML_EVENT_BB = "SBML Events";
      public const string SBML_PARAMETER_VALUES_BB = "SBML Parameter Values";
      public const string SBML_INITIAL_CONDITIONS_BB = "SBML Initial Conditions";
      public const string SBML_PASSIVETRANSPORTS_BB = "SBML Passive Transports";

      public const string SBML_SPECIES = "Species: ";
      public const string SBML_EVENTS = "Events: ";
      public const string SBML_TOP_CONTAINER = "SBML TopContainer: ";
      public const string SBML_EVENTS_TOP_CONTAINER = "SBML Events TopContainer";
      public const string SBML_MODEL = "SBML Model: ";
      public const string SBML_NOTES = " SBML Notes: ";
      public const string SBML_METAID = " SBML MetaID: ";
      public const string SBML_SBO = " SBML SBOTerm: ";
      public const string SBML_INITIAL_ASSIGNMENT = "Initial Assignment ";
      public const string SBML_ASSIGNMENT = "Assignment Rule";
      public const string SBML_KINETIC_LAW = "Kinetic Law ";
      public const string SBML = "SBML ";
      public const string SBML_ = "SBML_";

      public const string TAGS = "Tags";
      public const string TAG = "Tag";
      public const string SIZE = "size";
      public const string SPACE = " ";
      public const string VOLUME = "Volume";
      public const string SBML_BASE_UNIT = "SBML Base Unit ";
      public const string MSV = "MSV ";
      public const string RATE_RULE = "Rate Rule ";
      public const string PSDEUDO_FORMULA = "Pseudo Formula ";
      public const string EVENTS = "Events";
      public const string DEFAULT_FORMULA_NAME = "Default Formula Name";
      public const string SBML_DUMMYSPECIES = "DummySpecies_";

      //Constant values
      public const int SBML_MAX_SPATIAL_STRUCTURES = 1;
      public const int SBML_MAX_TOP_CONTAINERS = 1;
      public const int SBML_STOICHIOMETRY_DEFAULT = 1;
      public const int SBML_CONTAINER_3D = 3;
      public const int SBML_CONTAINER_2D =2;
      public const int SBML_CONTAINER_1D = 1;
      public const int SBML_CONTAINER_NO = 0;

      //Notifications
      public const string SBML_FEATURE_NOT_SUPPORTED = "SBML Feature not supported.";

      //Descriptions
      public const string SBML_INITIAL_CONDITIONS_DESCRIPTION = "The initial amount of the species in the SBML Model.";
      public const string SBML_SIZE_DESCRIPTION = "This Parameter represents the size of the SBML compartment.";

      //Formula/Math
      public const string MINUS = " - ";
      public const string PLUS = " + ";
      public const string DIVIDE = " / ";
      public const string TIMES = " * ";
      public const string AND = " & ";
      public const string OR = " | ";
      public const string NOT = " ¬ ";
      public const string EQUAL = " == ";
      public const string GEQ = " >= ";
      public const string LEQ = " <= ";
      public const string GT = " > ";
      public const string LT = " < ";
      public const string UNEQUAL = " <> ";
      public const string UNEQUAL2 = " != ";
      public const string POW = " ^ ";
      public const string POW2 = "pow";
      public const string LBRACE = "(";
      public const string RBRACE = ")";
      public const string SQRT = "sqrt";

      public const string E = "E";
      public const string EXP = "e";

      public const string LN = "ln";
      public const string LOG = "log";

      public const string DELIMITER = ";";

      public const string COS = "cos";
      public const string SIN = "sin";
      public const string TAN = "tan";

      public const string COSH = "cosh";
      public const string SINH = "sinh";
      public const string TANH = "tanh";

      public const string ACOS = "acos";
      public const string ASIN = "asin";
      public const string ATAN = "atan";

      public static string NoValidConversionToLevel3Version2 = "The file cannot be converted to Level 3 Version 2. Please, use an older version of MoBi to load this file";

      public static string ModelNotRead(string errorLog)
      {
         return String.Format("'ReadSBML' contains errors. Import not possible. Errorlog: {0}", errorLog);
      }

      public static string CouldNotConvertToActualLevel(long latestLevel, long latestVersion, long actualLevel, long actualVersion)
      {
         return string.Format("Could not convert from SBML Level: {0} Version: {1} to Level: {2} Version {3}", actualLevel, actualVersion, latestLevel, latestVersion);
      }

      public const string Model = "SBML Model";
   }
}
