using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;

//using NSFSM;

namespace NSGameNarrator{
	
	enum InterpretationPhase
	{
		WaitingForScriptTypePhase,
		DeclarationPhase,
		ObjectPhase,
		SetupPhase,
		EventPhase
	}
	
	public class GameNarrator : MonoBehaviour {
		public string Scenario;
		private GameNarratorContext Context;// = new GameNarratorContext();
		
		private Regex IndentationRE = new Regex(@"\t|[ ]{3}");
		private Regex IndentationReplaceRE = new Regex(@"^\s+");
		
		void Start () {
			Context = new GameNarratorContext();
			
			string[] lines = File.ReadAllLines(Scenario);
			
			foreach (string line in lines)
			{
				Interpret(line);
			}
			
			Context.Interpret (0, ""); // end
		}
		
		void Update () {
			
		}
		
		void Interpret(string line)
		{
			//Context.
			
			string cmd = IndentationReplaceRE.Replace(line, "");
			string indentation = IndentationReplaceRE.Match(line).ToString();
			
			Match m = IndentationRE.Match (indentation);
			int indentationCount = 0;
			while (m.Success) {
				m = m.NextMatch ();
				indentationCount++;
			}

			//TEST log each line
			//Debug.Log (indentationCount.ToString () + cmd);//line);
			Context.Interpret (indentationCount, cmd);
		}
	}
	
	public class GameNarratorContext{
		public Stack<GameNarratorAbstractExpression> ObjectConstructionStack = new Stack<GameNarratorAbstractExpression> ();
		public Stack<GameNarratorAbstractExpression> ExpressionConstructionStack = new Stack<GameNarratorAbstractExpression> ();
		//TODO ObjectConstructionStack.Peek()

		private int CurrentIndentation = 0;
		private int CurrentLine = 0;

		public Dictionary<string, GameNarratorAbstractExpression> NarrationObjects = new Dictionary<string, GameNarratorAbstractExpression> ();
		// A sub list of NarrationObjects
		public Dictionary<string, GameNarratorAbstractExpression> TaggedNarrationObjects = new Dictionary<string, GameNarratorAbstractExpression> ();

		public List<GameNarratorAbstractExpression> Expressions = new List<GameNarratorAbstractExpression> ();
		public List<GameNarratorAbstractExpression> VariableExpressions = new List<GameNarratorAbstractExpression> ();
		
		private Regex NarrationCommentRE = new Regex(@"#(?!""(?:(?:[^""#]*""){2})*[^""]*)");
		//"^(?:((""[^""]*"")?[^""]*)*)#.*");
		//private Regex NarrationCommentRE = new Regex(@"^#.*$");
		private Regex NarrationObjectRE = new Regex(@"^[:word:]+ +"".+"" *:$");
		private Regex TaggedNarrationObjectRE = new Regex(@"^[:word:]+ +"".+"" *, *[:word:]* *:$");

		public GameNarratorContext(){
			Type baseType = typeof(GameNarratorAbstractExpression); 
			// Reflection to register all expressions
			foreach (Type t in typeof(GameNarratorAbstractExpression).Assembly.GetTypes ()) {
				if(!t.IsClass || t.IsAbstract || !t.IsSubclassOf(baseType))
				{
					continue;
				}

				// TEST Reflection Detection
				//Debug.Log ("ExpressionClass found " + t.ToString());
				
				// Fake AbstractFactory by Reflection
				RegisterExpression(Activator.CreateInstance(Type.GetType(t.ToString())) as  GameNarratorAbstractExpression);
			}
			
			// TEST Insertion sort (in RegisterExpression)
			//	Debug.Log ("Expressions List");
			//foreach (GameNarratorAbstractExpression gnabex in Expressions) {
			//	Debug.Log ("Sorted : " + gnabex.ToString());
			//}
			//	Debug.Log ("Variable Expressions List");
			//foreach (GameNarratorAbstractExpression gnabex in VariableExpressions) {
			//	Debug.Log ("Sorted : " + gnabex.ToString());
			//}
		}
		
		private void RegisterExpression(GameNarratorAbstractExpression gnabex)
		{
			List<GameNarratorAbstractExpression> expressionList;
			int c;
			// Store expressions dealing with variables in another list
			if (gnabex.GetKeyword () == "") {
				expressionList = VariableExpressions;
			} else {
				expressionList = Expressions;
			}

			c = expressionList.Count;

			if (c == 0)
			{
				expressionList.Add(gnabex);
			} else {
				int priority = gnabex.GetPriority();
				// Insertion sort
				for (int i = 0; i < c; ++i)
				{
					int p = expressionList[i].GetPriority();
					if (priority < p)
					{
						expressionList.Insert(i, gnabex);
						return;
					}
				}
				expressionList.Insert(c, gnabex);
			}
		}
		
		public GameNarratorAbstractExpression Interpret (int indentationCount, string cmd)
		{
			CurrentLine ++;

			if (indentationCount < 0)
			{
				// end
				//TODO Resolve all expression in stacks

				//
				Debug.Log ("End of Script");
				return null;
			}

			// Detect comment
			Match m = NarrationCommentRE.Match (cmd);
			if (m.Success)
			{
				cmd = cmd.Remove(m.Index).Trim();
				cmd = NarrationCommentRE.Replace(cmd, "");

				if(cmd == ""){
					Debug.Log("Line " + CurrentLine.ToString() + " Full comment line");
					return null;
				}
			}

			// Indentation resolve
			// TODO
			int deltaIndentation = indentationCount - CurrentIndentation;
			if (deltaIndentation < 0) {
				// Unindentation, resolve deltaIndentation objects

			} else if (deltaIndentation > 0) {
				// New indentation, begin object contruction
				// Check deltaIndentation == 1

				//

			} else {
				// Same indentation, same object in construction

			}
			CurrentIndentation = indentationCount;

			// TODO Factorize this
			// Detect expression cmd using the keyword
			GameNarratorAbstractExpression gnabex = DetectKeyword(cmd);

			if (gnabex != null || cmd == "") {
				return gnabex;
			}

			// No keywords were detected, try detecting variable
			return DetectVariableExpression (cmd);
		}

		public GameNarratorAbstractExpression DetectKeyword(string cmd)
		{
			foreach (GameNarratorAbstractExpression gnabex in Expressions) {
				// Match keyword
				if (gnabex.MatchKeyword(cmd)) {
					//Debug.Log ("Line " + CurrentLine.ToString () + ", " + gnabex.GetName () + " KeyWord detected : " + cmd);
					
					Regex re = new Regex ("^" + gnabex.GetKeyword () + " +");
					cmd = re.Replace (cmd, "", 1);
					
					// TODO return this line below
					return gnabex.Interpret(this, cmd);
				}
			}
			return null;
		}
			
		public GameNarratorAbstractExpression DetectVariableExpression(string cmd)
		{
			foreach (GameNarratorAbstractExpression gnabex in VariableExpressions) {
				// Match keyword
				if (gnabex.MatchPattern(cmd)) {					
					//Debug.Log ("Line " + CurrentLine.ToString () + ", " + gnabex.GetName () + " Expression detected : " + cmd);

					return gnabex.Interpret(this, cmd);	
				}
			}
			return null;
		}

	}
	
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
			//if (keyword == "") {
			//	return false;
			//}
			
			return Regex.Match (cmd, "^"+keyword+" +").Success;
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
					Debug.Log (args [i]);
				}
				return true;
			} else {
				return false;
			}
		}
		
		public abstract GameNarratorAbstractExpression Interpret(GameNarratorContext context, string cmd);
	}
	
	// "TerminalExpression" 
	public abstract class GameNarratorTerminalExpression : GameNarratorAbstractExpression
	{

	}
	
	// "NonterminalExpression" 
	public abstract class GameNarratorNonTerminalExpression : GameNarratorAbstractExpression
	{
		public override sealed GameNarratorAbstractExpression Interpret(GameNarratorContext context, string cmd){
			GameNarratorNonTerminalExpression gnnte = this.MemberwiseClone() as GameNarratorNonTerminalExpression;
			context.ExpressionConstructionStack.Push (gnnte);
			return gnnte.Interpret2 (context, cmd);
		}
		public abstract GameNarratorAbstractExpression Interpret2 (GameNarratorContext context, string cmd);

		public abstract void Resolve (GameNarratorContext context);
	}

	//TODO remove...
	public class CommentExpression : GameNarratorTerminalExpression
	{
		public override string GetKeyword()
		{
			return "#";
		}
		public override string GetExpressionPattern()
		{
			return @"^#.*$";
		}
		public override string GetName()
		{
			return "OneLineComment Comment";
		}
		public override int GetPriority () {
			return 1;
		}
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, string cmd)
		{
			// TODO
			return null;
		}  
	}


	public class NarrationObjectExpression : GameNarratorNonTerminalExpression
	{
		/* No Keyword */
		/**************/
		public override string GetExpressionPattern()
		{
			return @"^[a-zA-Z\u00c0-\u017F][a-zA-Z0-9\u00c0-\u017F]* +"".+"" *: *$";
		}
		public override string GetName()
		{
			return "NarrationObject";
		}
		public override int GetPriority () {
			return 1;
		}
		public override GameNarratorAbstractExpression Interpret2(GameNarratorContext context, string cmd)
		{
			// TODO
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
			return @"^([a-zA-Z\u00c0-\u017F][a-zA-Z0-9\u00c0-\u017F]*) +""(.+)"" *, *([a-zA-Z\u00c0-\u017F][a-zA-Z0-9\u00c0-\u017F]*) *: *$";
		}
		public override string GetName()
		{
			return "TaggedNarration Object";
		}
		public override int GetPriority () {
			return 1;
		}
		public override GameNarratorAbstractExpression Interpret2(GameNarratorContext context, string cmd)
		{
			Debug.Log ("Creating TaggedNarrationObject : " + cmd);
			// TODO
			Regex re = new Regex (GetExpressionPattern ());
			Match m = re.Match (cmd);

			//


			//
			return null;
		}
		public override void Resolve (GameNarratorContext context) {}
	}
	
	#region "Scenario"
	public class ScenarioExpression : GameNarratorNonTerminalExpression
	{
		public override string GetKeyword()
		{
			return "Scenario";
		}
		public override string GetExpressionPattern()
		{
			return @"^Scenario +[0-9]+ *, *"".+"" *:$";
		}
		public override string GetName()
		{
			return "Scenario Region";
		}
		public override int GetPriority()
		{
			return 1;
		}
		public override GameNarratorAbstractExpression Interpret2(GameNarratorContext context, string cmd)
		{
			// TODO
			return null;
		}
		public override void Resolve (GameNarratorContext context) {}
	}
	#endregion
	
	#region "Exposition"
	public class ExpositionExpression : GameNarratorNonTerminalExpression
	{
		public override string GetKeyword()
		{
			return "Exposition";
		}
		public override string GetExpressionPattern()
		{
			return @"^Exposition *:$";
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
		public override GameNarratorAbstractExpression Interpret2(GameNarratorContext context, string cmd)
		{
			//TODO
			return null;
		}
		public override void Resolve (GameNarratorContext context) {}
	}
	#endregion
	
	#region "Starring"
	public class StarringExpression : GameNarratorNonTerminalExpression
	{
		public override string GetKeyword()
		{
			return "Starring";
		}
		public override string GetExpressionPattern()
		{
			return @"^Starring *:$";
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
		public override GameNarratorAbstractExpression Interpret2(GameNarratorContext context, string cmd)
		{
			//TODO
			return null;
		}
		public override void Resolve (GameNarratorContext context) {}
	}
	#endregion
	
	#region "As"
	public class AsExpression : GameNarratorTerminalExpression
	{
		public override string GetKeyword()
		{
			return "as";
		}
		public override string GetExpressionPattern()
		{
			return @"^as +$";
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
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, string cmd)
		{
			//TODO
			return null;
		}
	}
	#endregion
	
	#region "With"
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
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, string cmd)
		{
			//TODO
			return null;
		}
	}
	#endregion
	
	#region "Of"
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
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, string cmd)
		{
			//TODO
			return null;
		}
	}
	#endregion
	
	#region "Is"
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
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, string cmd)
		{
			//TODO
			return null;
		}
	}
	#endregion
	
	#region "Active"
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
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, string cmd)
		{
			//TODO
			return null;
		}
	}
	#endregion
	
	#region "Passive"
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
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, string cmd)
		{
			//TODO
			return null;
		}
	}
	#endregion
	
	#region "Consumable"
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
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, string cmd)
		{
			//TODO
			return null;
		}
	}
	#endregion
	
	#region "Ability"
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
		public override GameNarratorAbstractExpression Interpret2(GameNarratorContext context, string cmd)
		{
			//TODO
			return null;
		}
		public override void Resolve (GameNarratorContext context) {}
	}
	#endregion
	
	#region "Cost"
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
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, string cmd)
		{
			//TODO
			return null;
		}
	}
	#endregion
	
	#region "Equipment"
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
		public override GameNarratorAbstractExpression Interpret2(GameNarratorContext context, string cmd)
		{
			//TODO
			Debug.Log ("Creating Equipment");
			return null;
		}
		public override void Resolve (GameNarratorContext context) {}
	}
	#endregion
	
	#region "Set"
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
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, string cmd)
		{
			// TODO
			return null;
		}  
	}
	#endregion
	
	#region "In"
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
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, string cmd)
		{
			// TODO
			return null;
		}  
	}
	#endregion
	
	#region "Self"
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
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, string cmd)
		{
			// TODO
			return null;
		}
	}
	#endregion
	
	#region "State"
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
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, string cmd)
		{
			// TODO
			return null;
		}  
	}
	#endregion
	
	#region "Now"
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
		public override GameNarratorAbstractExpression Interpret2(GameNarratorContext context, string cmd)
		{
			// TODO
			return null;
		}
		public override void Resolve (GameNarratorContext context) {}
	}
	#endregion
	
	#region "At"
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
		public override GameNarratorAbstractExpression Interpret2(GameNarratorContext context, string cmd)
		{
			// TODO
			return null;
		}
		public override void Resolve (GameNarratorContext context) {}
	}
	#endregion
	
	#region "When"
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
		public override GameNarratorAbstractExpression Interpret2(GameNarratorContext context, string cmd)
		{
			// TODO
			return null;
		}
		public override void Resolve (GameNarratorContext context) {}
	}
	#endregion
	
	#region "Phase"
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
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, string cmd)
		{
			// TODO
			return null;
		}
	}
	#endregion
}