using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NSGameNarrator
{	
	#region "GnabexAbstractClasses"
	public abstract class GameNarratorAbstractExpression
	{ 
		public string[] args;
		public virtual string GetKeyword()
		{
			return "";
		}
		public virtual string GetExpressionPattern()
		{
			return "";
		}
		public abstract string GetName ();
		public virtual int GetPriority () {
			return 10;
		}
		
		public bool MatchKeyword(string cmd){
			string keyword = GetKeyword ();
			
			return Regex.Match (cmd, "^"+keyword+" *").Success;
		}
		
		public bool MatchPattern(string cmd){
			string pattern = GetExpressionPattern ();
			if (pattern == "") {
				return false;
			}
			
			Match m = Regex.Match (cmd, pattern);
			
			if (m.Success) {
				// Capture
				args = new string[m.Groups.Count];
				for (int i = 1; i < m.Groups.Count; i++) {
					args [i] = "";
					for (int j = 0; j < m.Groups[i].Captures.Count; j++) {
						args [i] += m.Groups [i].Captures [j].Value;
					}
					//TEST
					//Debug.Log (args [i]);
				}
				return true;
			} else {
				return false;
			}
		}
		
		public abstract GameNarratorAbstractExpression Interpret(GameNarratorContext context, ref string cmd);
	}
	
	// "TerminalExpression" 
	public abstract class GameNarratorTerminalExpression : GameNarratorAbstractExpression
	{
		
	}
	
	// "NonterminalExpression" 
	public abstract class GameNarratorNonTerminalExpression : GameNarratorAbstractExpression
	{
		public GameNarratorObject GNO = null;
		
		public override sealed GameNarratorAbstractExpression Interpret(GameNarratorContext context, ref string cmd){
			GameNarratorNonTerminalExpression gnnte = this.MemberwiseClone() as GameNarratorNonTerminalExpression;
			context.ObjectConstructionStack.Push (gnnte);
			return gnnte.Interpret2 (context, ref cmd);
		}
		public abstract GameNarratorAbstractExpression Interpret2 (GameNarratorContext context, ref string cmd);
		
		public abstract void Resolve (GameNarratorContext context);
	}
	#endregion


	#region "Pure Expressions"
	public class NarrationObjectExpression : GameNarratorNonTerminalExpression
	{
		/* No Keyword */
		/**************/
		public override string GetExpressionPattern()
		{
			return @"^([a-zA-Z\u00c0-\u017F][a-zA-Z0-9\u00c0-\u017F]*) +""([^""]+)"" *: *$";
		}
		public override string GetName()
		{
			return "NarrationObject";
		}
		public override int GetPriority () {
			return 1;
		}
		public override GameNarratorAbstractExpression Interpret2(GameNarratorContext context, ref string cmd)
		{
			// TODO

			cmd = "";
			return null;
		}
		public override void Resolve (GameNarratorContext context) {}
	}

	public class TaggedNarrationObjectExpression : GameNarratorNonTerminalExpression
	{
		/* No Keyword */
		/**************/
		public override string GetExpressionPattern()
		{
			return @"^([a-zA-Z\u00c0-\u017F][a-zA-Z0-9\u00c0-\u017F]*) +""([^""]+)"" *, *([a-zA-Z\u00c0-\u017F][a-zA-Z0-9\u00c0-\u017F]*) *: *$";
		}
		public override string GetName()
		{
			return "TaggedNarration Object";
		}
		public override int GetPriority () {
			return 1;
		}
		public override GameNarratorAbstractExpression Interpret2(GameNarratorContext context, ref string cmd)
		{
			Debug.Log ("Creating TaggedNarrationObject : " + cmd);
			
			GNO = new GameNarratorObject (args[1], args[2], args[3]);
			GNO.ParentExpression = this;
			//
			
			//
			cmd = "";
			return this;
		}
		public override void Resolve (GameNarratorContext context) {}
	}

	public class StringTypeExpression : GameNarratorTerminalExpression
	{
		string s = "";
		/* No Keyword */
		/**************/
		public override string GetExpressionPattern()
		{
			return @"^\s*""([^""]*)""\s";
		}
		public override string GetName()
		{
			return "String PrimitiveType";
		}
		public override int GetPriority () {
			return 1;
		}
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, ref string cmd)
		{
			// TODO
			s = args[1];
			Debug.Log ("Found string : " + s);
			return this;
		}

	}
	#endregion


	#region "Keyword Scenarios"
	public class ScenarioExpression : GameNarratorNonTerminalExpression
	{
		public GameScenario GS = null;
		
		public override string GetKeyword()
		{
			return "Scenario";
		}
		public override string GetExpressionPattern()
		{
			return @"^([0-9])+ *, *""(.+)"" *:$";
		}
		public override string GetName()
		{
			return "Scenario Region";
		}
		public override int GetPriority()
		{
			return 1;
		}
		public override GameNarratorAbstractExpression Interpret2(GameNarratorContext context, ref string cmd)
		{
			// TODO
			if (MatchPattern (cmd)) {
				// The main expression
				GS = new GameScenario(int.Parse(args[1]), args[2]);
			} else {
				// Just the keyword
				
				//GameScenario.getScenario(args[1]);
			}
			cmd = "";
			return this;
		}
		public override void Resolve (GameNarratorContext context) {}
	}
	
	public class ExpositionExpression : GameNarratorNonTerminalExpression
	{
		public override string GetKeyword()
		{
			return "Exposition";
		}
		public override string GetExpressionPattern()
		{
			return @"^\s*:\s*$";
		}
		public override int GetPriority()
		{
			return 10;
		}
		
		//
		public override string GetName()
		{
			return "Exposition Region";
		}
		public override GameNarratorAbstractExpression Interpret2(GameNarratorContext context, ref string cmd)
		{
			//TODO
			if (MatchPattern (cmd)) {
				cmd = "";
			} else {
				// Not creating a region
				
			}
			
			return this;
		}
		public override void Resolve (GameNarratorContext context) {}
	}
	
	public class StarringExpression : GameNarratorNonTerminalExpression
	{
		public override string GetKeyword()
		{
			return "Starring";
		}
		public override string GetExpressionPattern()
		{
			return @"^\s*:\s*$";
		}
		public override int GetPriority()
		{
			return 10;
		}
		
		//
		public override string GetName()
		{
			return "Starring Region";
		}
		public override GameNarratorAbstractExpression Interpret2(GameNarratorContext context, ref string cmd)
		{
			//TODO
			if (MatchPattern (cmd)) {
				
				cmd = "";
			} else {
				// Not creating a region
				
			}

			return this;
		}
		public override void Resolve (GameNarratorContext context) {}
	}

	public class AsExpression : GameNarratorTerminalExpression
	{
		public override string GetKeyword()
		{
			return "as";
		}
		public override string GetExpressionPattern()
		{
			return @"^as +";
		}
		public override int GetPriority()
		{
			return 10;
		}
		
		//
		public override string GetName()
		{
			return "As Constructor";
		}
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, ref string cmd)
		{
			GameNarratorNonTerminalExpression gnnte = context.ObjectConstructionStack.Peek ();
			//TODO make something with gnnte
			//gnnte.GNO 
			Debug.Log (gnnte.GNO.VarName + " As ...");
			//gnabex.
			
			return this;//gnnte;
		}
	}

	public class WithExpression : GameNarratorTerminalExpression
	{
		public override string GetKeyword()
		{
			return "with";
		}
		public override string GetExpressionPattern()
		{
			return @"^with +$";
		}
		public override int GetPriority()
		{
			return 10;
		}
		
		//
		public override string GetName()
		{
			return "With Constructor";
		}
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, ref string cmd)
		{
			//TODO
			return this;
		}
	}

	public class OfExpression : GameNarratorTerminalExpression
	{
		public override string GetKeyword()
		{
			return "of";
		}
		public override string GetExpressionPattern()
		{
			return @"^ +of +$";
		}
		public override int GetPriority()
		{
			return 10;
		}
		
		//
		public override string GetName()
		{
			return "Of Allocator";
		}
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, ref string cmd)
		{
			//TODO
			return this;
		}
	}

	public class IsExpression : GameNarratorTerminalExpression
	{
		public override string GetKeyword()
		{
			return "is";
		}
		public override string GetExpressionPattern()
		{
			return @"^ +is +$";
		}
		public override int GetPriority()
		{
			return 10;
		}
		
		//
		public override string GetName()
		{
			return "Is Comparator";
		}
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, ref string cmd)
		{
			//TODO
			return this;
		}
	}

	public class ActiveExpression : GameNarratorTerminalExpression
	{
		public override string GetKeyword()
		{
			return "active";
		}
		public override string GetExpressionPattern()
		{
			return @"^ +active +$";
		}
		public override int GetPriority()
		{
			return 10;
		}
		
		//
		public override string GetName()
		{
			return "Active Modifier";
		}
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, ref string cmd)
		{
			//TODO
			//context.ObjectConstructionStack.
			return this;
		}
	}

	public class PassiveExpression : GameNarratorTerminalExpression
	{
		public override string GetKeyword()
		{
			return "passive";
		}
		public override string GetExpressionPattern()
		{
			return @"^ +passive +$";
		}
		public override int GetPriority()
		{
			return 10;
		}
		
		//
		public override string GetName()
		{
			return "Passive Modifier";
		}
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, ref string cmd)
		{
			//TODO
			return this;
		}
	}

	public class ConsumableExpression : GameNarratorTerminalExpression
	{
		public override string GetKeyword()
		{
			return "consumable";
		}
		public override string GetExpressionPattern()
		{
			return @"^ +consumable +$";
		}
		public override int GetPriority()
		{
			return 10;
		}
		
		//
		public override string GetName()
		{
			return "Consumable Modifier";
		}
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, ref string cmd)
		{
			//TODO
			return this;
		}
	}

	public class AbilityExpression : GameNarratorNonTerminalExpression
	{
		public override string GetKeyword()
		{
			return "ability";
		}
		public override string GetExpressionPattern()
		{
			return @"^ +ability +""\[a-zA-Z0-9\\u00c0-\\u017F-\\s\]+"" *:$";
		}
		public override int GetPriority()
		{
			return 10;
		}
		
		//
		public override string GetName()
		{
			return "Ability Object";
		}
		public override GameNarratorAbstractExpression Interpret2(GameNarratorContext context, ref string cmd)
		{
			//TODO
			return this;
		}
		public override void Resolve (GameNarratorContext context) {}
	}

	public class CostExpression : GameNarratorTerminalExpression
	{
		public override string GetKeyword()
		{
			return "cost";
		}
		public override string GetExpressionPattern()
		{
			return @"^ +cost +$";
		}
		public override int GetPriority()
		{
			return 10;
		}
		
		//
		public override string GetName()
		{
			return "Cost Object";
		}
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, ref string cmd)
		{
			//TODO
			return this;
		}
	}

	public class EquipmentExpression : GameNarratorNonTerminalExpression
	{
		public override string GetKeyword()
		{
			return "equipment";
		}
		public override string GetExpressionPattern()
		{
			return @"^ +equipment +"".+"" +$";
		}
		public override int GetPriority()
		{
			return 10;
		}
		
		//
		public override string GetName()
		{
			return "Equipment Object";
		}
		public override GameNarratorAbstractExpression Interpret2(GameNarratorContext context, ref string cmd)
		{
			//TODO
			Debug.Log ("Creating Equipment");
			return this;
		}
		public override void Resolve (GameNarratorContext context) {}
	}

	public class SetExpression : GameNarratorTerminalExpression
	{
		public override string GetKeyword()
		{
			return "set";
		}
		public override string GetExpressionPattern()
		{
			return @"^set +([a-zA-Z][\\w]+) +(.+)$";// @"^set +[a-zA-Z][\\w]* +in.+:$"; <-- In Modifier
		}
		public override string GetName()
		{
			return "Set Accessor";
		}
		public override int GetPriority () {
			return 10;
		}
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, ref string cmd)
		{
			// TODO
			return this;
		}  
	}

	public class InExpression : GameNarratorTerminalExpression
	{
		public override string GetKeyword()
		{
			return "in";
		}
		public override string GetExpressionPattern()
		{
			return @"^in +.$";
		}
		public override string GetName()
		{
			return "In Modifier";
		}
		public override int GetPriority () {
			return 10;
		}
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, ref string cmd)
		{
			// TODO
			return this;
		}  
	}

	public class SelfExpression : GameNarratorTerminalExpression
	{
		public override string GetKeyword()
		{
			return "self";
		}
		public override string GetExpressionPattern()
		{
			return @"^self$";
		}
		public override string GetName()
		{
			return "Self Object";
		}
		public override int GetPriority () {
			return 10;
		}
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, ref string cmd)
		{
			// TODO
			return this;
		}
	}

	public class StateExpression : GameNarratorTerminalExpression
	{
		public override string GetKeyword()
		{
			return "State";
		}
		public override string GetExpressionPattern()
		{
			return @"^State +of +""(.+)""$";
		}
		public override string GetName()
		{
			return "State Object";
		}
		public override int GetPriority () {
			return 10;
		}
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, ref string cmd)
		{
			// TODO
			return this;
		}  
	}

	public class NowExpression : GameNarratorNonTerminalExpression
	{
		public override string GetKeyword()
		{
			return "now";
		}
		public override string GetExpressionPattern()
		{
			return @"^now *:$";
		}
		public override string GetName()
		{
			return "Now TimeEvent";
		}
		public override int GetPriority () {
			return 10;
		}
		public override GameNarratorAbstractExpression Interpret2(GameNarratorContext context, ref string cmd)
		{
			// TODO
			return this;
		}
		public override void Resolve (GameNarratorContext context) {}
	}

	public class AtExpression : GameNarratorNonTerminalExpression
	{
		public override string GetKeyword()
		{
			return "at";
		}
		public override string GetExpressionPattern()
		{
			return @"^at +.+:$";
		}
		public override string GetName()
		{
			return "At TimeEvent";
		}
		public override int GetPriority () {
			return 10;
		}
		public override GameNarratorAbstractExpression Interpret2(GameNarratorContext context, ref string cmd)
		{
			// TODO
			return this;
		}
		public override void Resolve (GameNarratorContext context) {}
	}

	public class WhenExpression : GameNarratorNonTerminalExpression
	{
		public override string GetKeyword()
		{
			return "when";
		}
		public override string GetExpressionPattern()
		{
			return @"^when +.+:$";
		}
		public override string GetName()
		{
			return "When TimeEvent";
		}
		public override int GetPriority () {
			return 10;
		}
		public override GameNarratorAbstractExpression Interpret2(GameNarratorContext context, ref string cmd)
		{
			// TODO
			return this;
		}
		public override void Resolve (GameNarratorContext context) {}
	}

	public class PhaseExpression : GameNarratorTerminalExpression
	{
		public override string GetKeyword()
		{
			return "Phase";
		}
		public override string GetExpressionPattern()
		{
			return @"^Phase +of +""(.+)""$";
		}
		public override string GetName()
		{
			return "Phase Object";
		}
		public override int GetPriority () {
			return 10;
		}
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, ref string cmd)
		{
			// TODO
			return this;
		}
	}

	public class ChooseExpression : GameNarratorTerminalExpression
	{
		public override string GetKeyword()
		{
			return "Choose";
		}
		public override string GetExpressionPattern()
		{
			return @"^Choose +[\w] +from +.+$";
		}
		public override string GetName()
		{
			return "Choose UI";
		}
		public override int GetPriority () {
			return 10;
		}
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, ref string cmd)
		{
			// TODO
			return this;
		}
	}

	public class ChosenExpression : GameNarratorTerminalExpression
	{
		public override string GetKeyword()
		{
			return "Chosen";
		}
		public override string GetExpressionPattern()
		{
			return @"^Chosen +[\w] +.+$";
		}
		public override string GetName()
		{
			return "Chosen UI";
		}
		public override int GetPriority () {
			return 10;
		}
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, ref string cmd)
		{
			// TODO
			return this;
		}
	}
	#endregion

	#region "Keyword Game Expressions"
	public class CharacterExpression : GameNarratorTerminalExpression
	{
		public override string GetKeyword()
		{
			return "Character";
		}
		public override string GetExpressionPattern()
		{
			return @"^Character +:\s*$";
		}
		public override string GetName()
		{
			return "Character Object";
		}
		public override int GetPriority () {
			return 10;
		}
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, ref string cmd)
		{
			// TODO
			Debug.Log ("Character");
			return this;
		}
	}

	public class PlayerExpression : GameNarratorTerminalExpression
	{
		public override string GetKeyword()
		{
			return "Player";
		}
		public override string GetExpressionPattern()
		{
			return @"^Player +:\s*$";
		}
		public override string GetName()
		{
			return "Player Object";
		}
		public override int GetPriority () {
			return 10;
		}
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, ref string cmd)
		{
			// TODO
			return this;
		}
	}
	#endregion
}