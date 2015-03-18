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
		private Regex TaggedNarrationObjectRE = new Regex("^[a-zA-Z][a-zA-Z0-9]* +\"[\\w\\s]+\" *, *[a-zA-Z][a-zA-Z0-9]* *:$");
		// ^[a-zA-Z][a-zA-Z0-9\u00C0-\u017F]* +"[a-zA-Z0-9\u00c0-\u017F\s]+" *, *[a-zA-Z][a-zA-Z0-9\u00C0-\u017F]* *:$ // Regex avec caractères accentués

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

	public class ExpressionFactory
	{

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

	public class ScenarioExpression : GameNarratorNonTerminalExpression
	{
		public override string GetKeyword()
		{
			return "Scenario";
		}
		public override string GetExpressionPattern() 
		{
			return @"^Scenario +[0-9]+ *, ""[\w\s]"" *:$";
			// return @"^Scenario +[0-9]+ *, *"[a-zA-Z0-9\u00c0-\u017F\s]+" *:$";
		}
		public override string GetName()
		{
			return "Scenario";
		}
		public override int GetPriority () {
			return 1;
		}
		public override void Interpret(GameNarratorContext context)  
		{
			// TODO
			//context.Expressions.Find (ex => ex.GetName () == "Scenario").MatchKeyword(cmd);
		}
	}

	public class NarrationObjectExpression : GameNarratorNonTerminalExpression
	{
		/* No Keyword */
		/**************/
		public override string GetExpressionPattern()
		{
			return @"^[a-zA-Z][a-zA-Z0-9]* +""[\w\s]+"" *:$";
			//return @"^Scenario +[0-9]+ *, *"[a-zA-Z0-9\u00c0-\u017F\s]+" *:";
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
			return @"^[a-zA-Z][a-zA-Z0-9]* +""[\w\s]+"" *, *[a-zA-Z][a-zA-Z0-9]* *:$";
		}
		public override string GetName()
		{
			return "TaggedNarrationObject";
		}
		public override int GetPriority () {
			return 1;
		}
		public override void Interpret(GameNarratorContext context)
		{
			// TODO
			
		}  
	}

}