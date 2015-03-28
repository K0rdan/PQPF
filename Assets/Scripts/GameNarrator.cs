using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;

//using NSFSM;

namespace NSGameNarrator{

	#region "GameNarrator"
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
				//Context.CurrentLine++;
				Interpret(line);
			}
			
			Context.Interpret (-1, ""); // end
		}
		
		void Update () {
			
		}
		
		void Interpret(string line)
		{
			//Context.

			// Trim Left
			string cmd = IndentationReplaceRE.Replace(line, "");

			// Count indentation
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
	#endregion

	#region "GameNarratorContext"
	public class GameNarratorContext
	{
		public Stack<GameNarratorNonTerminalExpression> ObjectConstructionStack = new Stack<GameNarratorNonTerminalExpression> ();
		//public Stack<GameNarratorAbstractExpression> ExpressionConstructionStack = new Stack<GameNarratorAbstractExpression> ();
		public Queue<GameNarratorAbstractExpression> ExpressionConstructionQueue = new Queue<GameNarratorAbstractExpression> ();
		//TODO ObjectConstructionStack.Peek()

		private int CurrentIndentation = 0;
		public int CurrentLine = 0;

		//public Dictionary<string, GameNarratorAbstractExpression> NarrationObjects = new Dictionary<string, GameNarratorAbstractExpression> ();
		// A sub list of NarrationObjects
		//public Dictionary<string, GameNarratorAbstractExpression> TaggedNarrationObjects = new Dictionary<string, GameNarratorAbstractExpression> ();

		public List<GameNarratorAbstractExpression> Expressions = new List<GameNarratorAbstractExpression> ();
		public List<GameNarratorAbstractExpression> VariableExpressions = new List<GameNarratorAbstractExpression> ();
		
		private Regex NarrationCommentRE = new Regex(@"#(?!""(?:(?:[^""#]*""){2})*[^""]*)");
		//private Regex NarrationObjectRE = new Regex(@"^[:word:]+ +"".+"" *:$");
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
		
		public void Interpret (int indentationCount, string cmd)
		{
			CurrentLine ++;

			if (indentationCount < 0)
			{
				// end
				//TODO Resolve all non terminal expression in stacks (Object construction)
				/*while(ObjectConstructionStack.Count > 0) {
					ObjectConstructionStack.Pop().Resolve(this);
				}*/

				//IndentationResolve(0);
				while (ObjectConstructionStack.Count > 0)
				{
					GameNarratorNonTerminalExpression gnnte = ObjectConstructionStack.Pop();
					Debug.Log("Popping " + gnnte.ToString());
					gnnte.Resolve(this);
				}

				//
				Debug.Log ("End of Script");
				return;// null;
			}

			if (cmd == "") {
				return;
			}

			// Detect comment
			Match m = NarrationCommentRE.Match (cmd);
			if (m.Success)
			{
				cmd = cmd.Remove(m.Index).Trim();
				cmd = NarrationCommentRE.Replace(cmd, "");

				if(cmd == ""){
					Debug.Log("Line " + CurrentLine.ToString() + " Full comment line");
					return;// null;
				}
			}

			IndentationResolve (indentationCount);


			Debug.Log ("Interpreting line " + CurrentLine.ToString()+ " - " + cmd);

			GameNarratorAbstractExpression gnabex  = null;
			while(cmd != "") {
				gnabex = InterpretWords (ref cmd);
				if (gnabex != null)
				{
					Debug.Log ("Enqueuing " + gnabex.ToString());
					ExpressionConstructionQueue.Enqueue (gnabex);
				}
			}

			ResolveExpression();
			return;// gnabex;
		}

		public GameNarratorAbstractExpression InterpretWords(ref string cmd){
			// No command to be interpreted
			cmd = cmd.Trim ();

			if (cmd == ""){
				Debug.LogError("cmd is void");

				return null;
			}

			// No keywords were detected, try detecting a variable expression
			GameNarratorAbstractExpression gnabex = DetectVariableExpression (ref cmd);
			if (gnabex != null){
				Debug.Log("Variable Expression Detected");
				return gnabex;
			}

			// Detect expression cmd using the keyword
			gnabex = DetectKeyword(ref cmd);
			if (gnabex != null){
				Debug.Log("Keyword Detected");
				return gnabex;
			}

			// TODO
			// No variable expressions were detected, try detecting variable
			gnabex = DetectVariable (ref cmd);
			if (gnabex != null){
				Debug.Log("Variable Detected");
				return gnabex;
			}

			return null;
		}

		public GameNarratorAbstractExpression DetectKeyword(ref string cmd)
		{
			foreach (GameNarratorAbstractExpression gnabex in Expressions) {
				// Match keyword
				if (gnabex.MatchKeyword(cmd)) {
					//Debug.Log ("Line " + CurrentLine.ToString () + ", " + gnabex.GetName () + " KeyWord detected : " + cmd);

					Regex re = new Regex ("^" + gnabex.GetKeyword () + " *");
					cmd = re.Replace (cmd, "", 1);
					
					// TODO return this line below
					return gnabex.Interpret2(this, ref cmd);
				}
			}
			return null;
		}
			
		public GameNarratorAbstractExpression DetectVariableExpression(ref string cmd)
		{
			foreach (GameNarratorAbstractExpression gnabex in VariableExpressions) {
				// Match keyword
				if (gnabex.MatchPattern(ref cmd)) {					
					//Debug.Log ("Line " + CurrentLine.ToString () + ", " + gnabex.GetName () + " Expression detected : " + cmd);

					return gnabex.Interpret2(this, ref cmd);	
				}
			}
			return null;
		}

		public GameNarratorAbstractExpression DetectVariable(ref string cmd)
		{
			GameNarratorObject gno = null;

			Regex cmdCutter = new Regex (@"^[^\s]+"); // TODO CmdCutterRE
			Match m = cmdCutter.Match (cmd);

			if (!m.Success) {
				Debug.LogError("No word detected (l." + CurrentLine.ToString () + ") - " + cmd);
				return null;
			}
			Debug.Log ("Detected word : " + m.Value);

			string word = m.Value.Trim ();
			cmd = cmdCutter.Replace (cmd, "", 1);
			//Debug.Log ("Replaced cmd : " + cmd);

			if(GameNarratorObject.Vars.TryGetValue(word, out gno))
			{
				return gno.ParentExpression;
			}

			Debug.LogError ("Variable \"" + word+ "\" doesn't exist");

			return null;
		}

		private void IndentationResolve(int indentationCount)
		{
			int deltaIndentation = indentationCount - CurrentIndentation;
			if (deltaIndentation < 0) {
				// Unindentation, resolve deltaIndentation objects
				Debug.Log ("deltaIndentation = " + deltaIndentation.ToString());
				for(int i = deltaIndentation; i < 0; i++)
				{
					GameNarratorNonTerminalExpression gnnte = ObjectConstructionStack.Pop();
					Debug.Log("Popping " + gnnte.ToString());
					gnnte.Resolve(this);
				}
				
			} else if (deltaIndentation > 0) {
				// New indentation, begin object construction
				// Check deltaIndentation == 1
				if(deltaIndentation == 1){
					// TODO
					
				}else{
					Debug.LogError ("Multiple indentations error");
				}
				//
			} else {
				// Same indentation, same object in construction
				
			}
			CurrentIndentation = indentationCount;
		}

		private void ResolveExpression()
		{
			//TODO evaluate subexpression currently in stack
			Debug.Log("cmd == \"\", resolving expression");
			while (ExpressionConstructionQueue.Count > 0) {
				GameNarratorAbstractExpression gnabex = ExpressionConstructionQueue.Dequeue ();
				Debug.Log ("Dequeueing " + gnabex.ToString());
				if (typeof (GameNarratorNonTerminalExpression).IsInstanceOfType (gnabex)){
					GameNarratorNonTerminalExpression gnnte = gnabex as GameNarratorNonTerminalExpression;
					gnnte.Resolve (this);
				}
			}
			return;
		}
	}
	#endregion

}