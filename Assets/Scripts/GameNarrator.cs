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
			
			Debug.Log (indentationCount.ToString () + cmd);//line);
			Context.Interpret (indentationCount, cmd);
		}
	}
	
	public class GameNarratorContext{
		public Stack<GameNarratorAbstractExpression> ObjectConstructionStack = new Stack<GameNarratorAbstractExpression> ();
		public int CurrentIndentation = 0;
		
		public Dictionary<string, GameNarratorAbstractExpression> NarrationObjects = new Dictionary<string, GameNarratorAbstractExpression> ();
		public Dictionary<string, GameNarratorAbstractExpression> TaggedNarrationObjects = new Dictionary<string, GameNarratorAbstractExpression> ();
		
		
		public List<GameNarratorAbstractExpression> Expressions = new List<GameNarratorAbstractExpression> ();
		//public static List<string> Keywords = new List<string> ();
		//public static List<string> ExpressionPatterns = new List<string> ();
		
		private Regex NarrationCommentRE = new Regex("^#.*$");
		private Regex NarrationObjectRE = new Regex("^[a-zA-Z][a-zA-Z0-9]* +\"[\\w\\s]+\" *:$");
		private Regex TaggedNarrationObjectRE = new Regex("^[a-zA-Z][a-zA-Z0-9\\u00C0-\\u017F]* +\"[a-zA-Z0-9\\u00c0-\\u017F\\s]+\" *, *[a-zA-Z][a-zA-Z0-9\\u00C0-\\u017F]* *:$");
		
		public GameNarratorContext(){
			Debug.Log ("Reflection");
			Type baseType = typeof(GameNarratorAbstractExpression); 
			// Reflection to register all expressions
			foreach (Type t in typeof(GameNarratorAbstractExpression).Assembly.GetTypes ()) {
				if(!t.IsClass || t.IsAbstract || !t.IsSubclassOf(baseType))
				{
					continue;
				}
				
				//object[] o = new object[2]{t,this};
				//t.GetMethod("Register").Invoke(null, o);
				
				Debug.Log ("Expression found " + t.ToString());
				
				// Fake AbstractFactory by Reflection
				RegisterExpression(Activator.CreateInstance(Type.GetType(t.ToString())) as  GameNarratorAbstractExpression);
				
			}
			
			// Test Insertion sort (in RegisterExpression)
			//foreach (GameNarratorAbstractExpression gnabex in Expressions) {
			//	Debug.Log ("Sorted : " + gnabex.ToString());
			//}
			
			
			//TODO ObjectConstructionStack.Peek()
		}
		
		private void RegisterExpression(GameNarratorAbstractExpression gnabex)
		{
			//Keywords.Contains (
			int c = Expressions.Count;
			
			if (c == 0)
			{
				Expressions.Add(gnabex);
			} else {
				// Insertion sort
				int priority = gnabex.GetPriority();
				for (int i = 0; i < c; ++i)
				{
					int p = Expressions[i].GetPriority();
					if (priority < p)
					{
						Expressions.Insert(i, gnabex);
						return;
					}
				}
				Expressions.Insert(c, gnabex);
			}
		}
		
		public void Interpret (int indentationCount, string cmd)
		{
			Match m = NarrationCommentRE.Match (cmd);
			if (m.Success) {
				Debug.Log ("NarComment detected : " + m.ToString());
				return;
			}
			
			if (indentationCount < 0) {
				// end
				
				
				Debug.Log ("End of Script");
				return;
			}
			
			Match m1 = NarrationObjectRE.Match (cmd);
			if (m1.Success) {
				Debug.Log ("NarObj detected : " + m1.ToString());
			}
			
			Match m2 =TaggedNarrationObjectRE.Match (cmd);
			if (m2.Success) {
				Debug.Log ("TagdNarObj detected : " + m2.ToString());
			}
		}
	}
	
	public abstract class GameNarratorAbstractExpression
	{ 
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
			if (keyword == "") {
				return false;
			}
			
			return Regex.Match (cmd, "^"+keyword).Success;
		}
		
		public bool MatchPattern(string cmd){
			string pattern = GetExpressionPattern ();
			if (pattern == "") {
				return false;
			}
			
			Match m = Regex.Match (cmd, pattern);
			CaptureCollection c = m.Captures;
			
			return m.Success;
		}
		
		public abstract void Interpret(GameNarratorContext context);
	}
	
	// "TerminalExpression" 
	public abstract class GameNarratorTerminalExpression : GameNarratorAbstractExpression {}
	
	// "NonterminalExpression" 
	public abstract class GameNarratorNonTerminalExpression : GameNarratorAbstractExpression
	{
		public static int Priority = 10;
		public List<GameNarratorAbstractExpression> children;
	}
	
	public class NarrationObjectExpression : GameNarratorNonTerminalExpression
	{
		/* No Keyword */
		/**************/
		public override string GetExpressionPattern()
		{
			return @"^[a-zA-Z][a-zA-Z0-9]* +"".+"" *:$";
		}
		public override string GetName()
		{
			return "NarrationObject";
		}
		public override int GetPriority () {
			return 1;
		}
		public override void Interpret(GameNarratorContext context)
		{
			// TODO
			
		}  
	}
	
	public class TaggedNarrationObjectExpression : GameNarratorNonTerminalExpression
	{
		/* No Keyword */
		/**************/
		public override string GetExpressionPattern()
		{
			return @"^[a-zA-Z][a-zA-Z0-9]* +"".+"" *, *[a-zA-Z][a-zA-Z0-9\u00c0-\u017F]* *:$";
		}
		public override string GetName()
		{
			return "TaggedNarration Object";
		}
		public override int GetPriority () {
			return 1;
		}
		public override void Interpret(GameNarratorContext context)
		{
			// TODO
			
		}  
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
			return @"^Scenario +[0-9]+ *, *""[a-zA-Z0-9\\u00c0-\\u017F-\\s]+"" *:$";
		}
		public override string GetName()
		{
			return "Scenario Region";
		}
		public override int GetPriority()
		{
			return 1;
		}
		public override void Interpret(GameNarratorContext context)
		{
			// TODO
			//context.Expressions.Find (ex => ex.GetName () == "Scenario").MatchKeyword(cmd);
		}
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
		public override void Interpret(GameNarratorContext context)
		{
			//TODO
			
		}
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
		public override void Interpret(GameNarratorContext context)
		{
			//TODO
			
		}
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
		public override void Interpret(GameNarratorContext context)
		{
			//TODO
			
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
		public override void Interpret(GameNarratorContext context)
		{
			//TODO
			
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
		public override void Interpret(GameNarratorContext context)
		{
			//TODO
			
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
		public override void Interpret(GameNarratorContext context)
		{
			//TODO
			
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
		public override void Interpret(GameNarratorContext context)
		{
			//TODO
			
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
		public override void Interpret(GameNarratorContext context)
		{
			//TODO
			
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
		public override void Interpret(GameNarratorContext context)
		{
			//TODO
			
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
		public override void Interpret(GameNarratorContext context)
		{
			//TODO
			
		}
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
		public override void Interpret(GameNarratorContext context)
		{
			//TODO
			
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
		public override void Interpret(GameNarratorContext context)
		{
			//TODO
			
		}
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
			return @"^set +[a-zA-Z][\\w]+ +.+$";// @"^set +[a-zA-Z][\\w]* +in.+:$"; <-- In Modifier
		}
		public override string GetName()
		{
			return "Set Accessor";
		}
		public override int GetPriority () {
			return 10;
		}
		public override void Interpret(GameNarratorContext context)
		{
			// TODO
			
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
		public override void Interpret(GameNarratorContext context)
		{
			// TODO
			
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
		public override void Interpret(GameNarratorContext context)
		{
			// TODO
			
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
			return @"^State +of +"".+""$";
		}
		public override string GetName()
		{
			return "State Object";
		}
		public override int GetPriority () {
			return 10;
		}
		public override void Interpret(GameNarratorContext context)
		{
			// TODO
			
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
		public override void Interpret(GameNarratorContext context)
		{
			// TODO
			
		}  
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
		public override void Interpret(GameNarratorContext context)
		{
			// TODO
			
		}
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
		public override void Interpret(GameNarratorContext context)
		{
			// TODO
			
		}  
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
			return @"^Phase +of +"".+""$";
		}
		public override string GetName()
		{
			return "Phase Object";
		}
		public override int GetPriority () {
			return 10;
		}
		public override void Interpret(GameNarratorContext context)
		{
			// TODO
			
		}
	}
	#endregion
}