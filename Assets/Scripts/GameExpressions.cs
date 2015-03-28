using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Reflection;

namespace NSGameNarrator
{	
	#region "GnabexAbstractClasses"
	public abstract class GameNarratorAbstractExpression
	{ 
		public string[] args;
		public Match argsMatch;

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
			
			return Regex.Match (cmd, "^"+keyword+@"(?!\w) *").Success;
		}
		public bool MatchPattern(ref string cmd){
			string pattern = GetExpressionPattern ();
			if (pattern == "") {
				return false;
			}
			
			Match m = Regex.Match (cmd, pattern);
			
			if (m.Success) {
				// Capture
				args = new string[m.Groups.Count];
				argsMatch = m;
				for (int i = 0; i < m.Groups.Count; i++) {
					args [i] = "";
					for (int j = 0; j < m.Groups[i].Captures.Count; j++) {
						args [i] += m.Groups [i].Captures [j].Value;
					}
					//TEST
					Debug.Log (i.ToString() + " - " + args [i]);
				}
				// Consume matched expression
				int index = cmd.IndexOf(args[0]);
				cmd = (index < 0) ? cmd : cmd.Remove(index, args[0].Length);
				Debug.Log ("Consumed cmd : " + cmd);

				return true;
			} else {
				return false;
			}
		}

		public GameNarratorAbstractExpression Interpret2(GameNarratorContext context, ref string cmd){
			//GameNarratorAbstractExpression gnabex = Activator.CreateInstance(this.GetType()) as GameNarratorAbstractExpression;
			GameNarratorAbstractExpression gnabex = this.MemberwiseClone() as GameNarratorAbstractExpression;
			return gnabex.Interpret(context, ref cmd);
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
		public virtual Type GetObjectType(){
			return null;
		}

		public GameNarratorNonTerminalExpression AsConstructor(GameNarratorNonTerminalExpression gnnte) {
			Debug.Log ("ASCONSTRUCTOR");
			if (gnnte.GetObjectType() == null) {
				Debug.Log ("NULL");
				return null;
			}

			Type[] t = { typeof(GameNarratorObject) };

			object[] o = { GNO };

			Debug.Log ("INVOKE - " + gnnte.GetObjectType().GetConstructor (t).ToString ());

			return  gnnte.GetObjectType().GetConstructor (t).Invoke (o) as GameNarratorNonTerminalExpression;
		}

		public abstract void Resolve (GameNarratorContext context);
	}
	#endregion


	#region "Pure Expressions"
	/*public class NarrationObjectExpression : GameNarratorNonTerminalExpression
	{
		// No Keyword
		
		public override string GetExpressionPattern()
		{
			return @"^([a-zA-Z\u00c0-\u017F][a-zA-Z0-9\u00c0-\u017F]*) +""([^""]+)"" *";
		}
		public override string GetName()
		{
			return "NarrationObject";
		}
		public override int GetPriority () {
			return 2; // Shall be less than TaggedNarrationObject
		}
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, ref string cmd)
		{
			Debug.Log ("Creating TaggedNarrationObject : " + cmd);
			
			GNO = new GameNarratorObject (args[1], args[2], "");
			GNO.ParentExpression = this;

			return this;
		}
		public override void Resolve (GameNarratorContext context) {}
	}*/

	public class TaggedNarrationObjectExpression : GameNarratorNonTerminalExpression
	{
		public virtual Type GetType(){
			return typeof (GameNarratorObject);
		}

		/* No Keyword */
		/**************/
		public override string GetExpressionPattern()
		{
			return @"^([a-zA-Z\u00c0-\u017F][a-zA-Z0-9\u00c0-\u017F]*) +""([^""]+)"" *, *([a-zA-Z\u00c0-\u017F][a-zA-Z0-9\u00c0-\u017F]*) *";
		}
		public override string GetName()
		{
			return "TaggedNarration Object";
		}
		public override int GetPriority () {
			return 1;
		}
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, ref string cmd)
		{
			Debug.Log ("Creating TaggedNarrationObject : " + cmd);
			
			GNO = new GameNarratorObject (args[1], args[2], args[3]);
			GNO.ParentExpression = this;
			//
			
			//
			return this;
		}
		public override void Resolve (GameNarratorContext context) {}
	}

	public class StringTypeExpression : GameNarratorTerminalExpression
	{
		public string S = "";

		/* No Keyword */
		/**************/

		public override string GetExpressionPattern()
		{
			return @"^\s*""([^""]*)""\s*";
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
			S = args[1];
			Debug.Log ("Found string : " + S);
			return this;
		}

	}

	public class IntegerTypeExpression : GameNarratorTerminalExpression
	{
		public int Val = 0;
		
		/* No Keyword */
		/**************/
		
		public override string GetExpressionPattern()
		{
			return @"^\s*([0-9]+)\s*";
		}
		public override string GetName()
		{
			return "Integer PrimitiveType";
		}
		public override int GetPriority () {
			return 1;
		}
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, ref string cmd)
		{
			// TODO
			Val = int.Parse(args[1]);
			Debug.Log ("Found Integer : " + Val);
			return this;
		}
		
	}

	//List
	public class ListTypeExpression : GameNarratorTerminalExpression
	{
		public List<GameNarratorTerminalExpression> Lgnabex = new List<GameNarratorTerminalExpression> ();
		
		/* No Keyword */
		/**************/
		
		public override string GetExpressionPattern()
		{
			// TODO lists in list, void list

			return @"^\s*{\s*(?:([^{},]*)\s*,\s*)*([^{},]*)\s*}\s*";
		}
		public override string GetName()
		{
			return "Integer PrimitiveType";
		}
		public override int GetPriority () {
			return 1;
		}
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, ref string cmd)
		{
			// TODO
			List<string> ls = new List<string> ();

			for (int i = 0; i < argsMatch.Groups[1].Captures.Count; i++) {
				ls.Add(argsMatch.Groups[1].Captures[i].Value);
			}

			string s = "";
			for (int i = 0; i < argsMatch.Groups[2].Captures.Count; i++) {
				s += argsMatch.Groups[2].Captures[i].Value;
			}
			ls.Add (s);

			//Lgnabex;
			//Val = int.Parse(args[1]);
			//Debug.Log ("Found Integer : " + val);
			Debug.Log ("List args");
			for (int i = 0; i < ls.Count; i++) {
				Debug.Log (ls[i]);
			}

			return this;
		}
		
	}

	//Filter
	public class ChooseExpression : GameNarratorTerminalExpression
	{
		/*public override string GetKeyword()
		{
			return "Choose";
		}*/
		public override string GetExpressionPattern()
		{
			return @"^\s*Choose\[\s*([a-zA-Z][a-zA-Z0-9]*)\s+from\s+([a-zA-Z][a-zA-Z0-9]*)\s+with\s+([^\[\]]*)\s*\]\s*";
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
		public override string GetExpressionPattern()
		{
			return @"^\s*Chosen\[\s*([a-zA-Z][a-zA-Z0-9]*)\s*\]\s*";
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

	//DotMethod
	public class DotMethodExpression : GameNarratorTerminalExpression
	{
		//public GamePlayer CO;
		
		public override string GetExpressionPattern()
		{
			return @"^\.([a-zA-Z][a-zA-Z0-9]*)\s*";
		}
		public override string GetName()
		{
			return "Dot Function";
		}
		public override int GetPriority () {
			return 10;
		}
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, ref string cmd)
		{
			//TODO
			
			return this;
		}
	}

	public class DeusExMachinaExpression : GameNarratorTerminalExpression
	{
		public string Str;
		public List<GameNarratorAbstractExpression> Lgnabex = new List<GameNarratorAbstractExpression> ();

		/* No Keyword */
		/**************/
		
		public override string GetExpressionPattern()
		{
			// TODO lists in list, void list
			
			return @"^\s*Deus\s+Ex\s+Machina\s+\$([a-zA-Z][a-zA-Z0-9]*)\s*";
		}
		public override string GetName()
		{
			return "DeusExMachina Function";
		}
		public override int GetPriority () {
			return 1;
		}
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, ref string cmd)
		{
			// TODO
			Str = args [1];

			while (cmd != "") {
				GameNarratorAbstractExpression gnabex = context.InterpretWords(ref cmd);
				if (gnabex != null)
				{
					Lgnabex.Add(gnabex);
					Debug.Log ("Enqueuing " + gnabex.ToString());
					context.ExpressionConstructionQueue.Enqueue (gnabex);
				}
			}

			Type DEMType = typeof(DeusExMachina);
			MethodInfo dem = DEMType.GetMethod(Str);
			Debug.Log ("Will invoke method " + Str);
			Debug.Log ("methodinfo " + dem.ToString());

			dem.Invoke(new DeusExMachina(Lgnabex), null);

			return this;
		}
		
	}
	#endregion


	#region "Keyword Expression"
	public class StackExpression : GameNarratorNonTerminalExpression
	{
		//public GameScenario GS = null;
		
		public override string GetKeyword()
		{
			return ":";
		}
		public override string GetExpressionPattern()
		{
			return "";
		}
		public override string GetName()
		{
			return "Stack Stacker";
		}
		public override int GetPriority()
		{
			return 1;
		}
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, ref string cmd)
		{
			// TODO put in resolve?
			Debug.Log ("Stacking");
			GameNarratorAbstractExpression gnabex = context.ExpressionConstructionQueue.Peek ();
			if (typeof (GameNarratorNonTerminalExpression).IsInstanceOfType (gnabex)) {
				context.ObjectConstructionStack.Push(gnabex as GameNarratorNonTerminalExpression);
				Debug.Log ("Stacked " + gnabex.ToString ());
			} else {
				Debug.LogError("Misuse of Stacker Command");
			}

			cmd = "";
			return this;
		}
		public override void Resolve (GameNarratorContext context) {

		}
	}


	public class ScenarioExpression : GameNarratorNonTerminalExpression
	{
		public GameScenario GS = null;
		
		public override string GetKeyword()
		{
			return "Scenario";
		}
		public override string GetExpressionPattern()
		{
			return @"^([0-9]+) *, *""(.+)"" *";
		}
		public override string GetName()
		{
			return "Scenario Region";
		}
		public override int GetPriority()
		{
			return 1;
		}
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, ref string cmd)
		{
			// TODO
			Debug.Log("Scenario keyword, cmd left : " + cmd);
			if (MatchPattern (ref cmd)) {
				// The main expression
				GS = new GameScenario(int.Parse(args[1]), args[2]);
			} else {
				// Just the keyword

				//checkpattern(regex)

				//GameScenario.getScenario(args[1]);
				GS = null;
				//TODO //
				//GS = GameScenario.GetScenarioById(int.Parse(args[1]));
			}
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
			return @"";
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
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, ref string cmd)
		{
			/*//TODO
			if (MatchPattern (ref cmd)) {
				cmd = "";
			} else {
				// Not creating a region
				
			}*/
			
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
			return @"";
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
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, ref string cmd)
		{
			Debug.Log("Starring keyword, cmd left : " + cmd);
			//TODO
			/*if (MatchPattern (ref cmd)) {
				
				cmd = "";
			} else {
				// Not creating a region
				
			}*/

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
			GameNarratorAbstractExpression gnabex =  context.InterpretWords (ref cmd);

			//TODO make something with gnnte and gnabex parsed to gnnte
			//gnnte.GNO
			gnnte.AsConstructor (gnabex as GameNarratorNonTerminalExpression);

			Type t = typeof(GamePlayer);
			//Debug.Log (gnnte);
			//Debug.Log (gnnte.GNO);
			Debug.Log (gnnte.GNO.VarName + " As ...");
			//gnabex.

			return gnabex;
			//this;//gnnte;
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
			GameNarratorNonTerminalExpression gnnte = context.ObjectConstructionStack.Peek ();
			GameNarratorAbstractExpression gnabex = context.InterpretWords (ref cmd);
			//TODO Add gnabex (object/property) to gnnte

			//

			return gnabex;
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
			//TODO gnabex on end of queue of next gnabex (which shall be a primitive type or a variable conatining a primitive type )
			//GameNarratorAbstractExpression gnabexQ = context.ExpressionConstructionQueue.

			GameNarratorAbstractExpression gnabex = context.InterpretWords (ref cmd);

			return gnabex;
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
			GameNarratorAbstractExpression gnabex = context.InterpretWords (ref cmd);
			//TODO Add active modifier to gnabex

			//context.ObjectConstructionStack.
			return gnabex;
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
			GameNarratorAbstractExpression gnabex = context.InterpretWords (ref cmd);
			//TODO Add passive modifier to gnabex
			
			//context.ObjectConstructionStack.
			return gnabex;
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
			GameNarratorAbstractExpression gnabex = context.InterpretWords (ref cmd);
			//TODO Add consumable modifier to gnabex
			
			//context.ObjectConstructionStack.
			return gnabex;
		}
	}

	public class TimesExpression : GameNarratorTerminalExpression
	{
		public override string GetKeyword()
		{
			return "times";
		}
		public override string GetExpressionPattern()
		{
			return @"^ +times +[0-9]+$";
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
			return context.InterpretWords (ref cmd);
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
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, ref string cmd)
		{
			//TODO
			return this;
		}
		public override void Resolve (GameNarratorContext context) {}
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
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, ref string cmd)
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

			return context.InterpretWords (ref cmd);
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
			return context.InterpretWords (ref cmd);
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
			// TODO if the expression in the beginning og the queue has a set accessor
			// modify
			// with the gnabex to come
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
			return @"";
		}
		public override string GetName()
		{
			return "Now TimeEvent";
		}
		public override int GetPriority () {
			return 10;
		}
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, ref string cmd)
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
			return @"";
		}
		public override string GetName()
		{
			return "At TimeEvent";
		}
		public override int GetPriority () {
			return 10;
		}
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, ref string cmd)
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
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, ref string cmd)
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


	#endregion

	#region "Keyword Game Expressions"
	public class CharacterExpression : GameNarratorNonTerminalExpression
	{
		public override Type GetObjectType(){
			return typeof(GameCharacter);
		}

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
		public override void Resolve(GameNarratorContext context){
			//TODO
			
		}
	}

	public class PlayerExpression : GameNarratorNonTerminalExpression
	{
		public GamePlayer GP;
		public override Type GetObjectType(){
			return typeof(GamePlayer);
		}
		public override string GetKeyword()
		{
			return "Player";
		}
		public override string GetExpressionPattern()
		{
			return @"";
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
			//Debug.Log ("creating Player from GNO");
			//GP = new GamePlayer (context.ObjectConstructionStack.Peek ().GNO);
			//TODO put it in AsExpression interpret2, as is followed by a GameItem derived

			return this;
		}
		public override void Resolve(GameNarratorContext context){
			//TODO

		}
	}

	public class EnemyExpression : GameNarratorNonTerminalExpression
	{
		//public GameEnemy GE;
		public override Type GetObjectType(){
			return typeof(GameEnemy);
		}
		public override string GetKeyword()
		{
			return "Enemy";
		}
		public override string GetExpressionPattern()
		{
			return @"";
		}
		public override string GetName()
		{
			return "Enemy Object";
		}
		public override int GetPriority () {
			return 10;
		}
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, ref string cmd)
		{
			// TODO
			//Debug.Log ("creating Player from GNO");
			//GP = new GamePlayer (context.ObjectConstructionStack.Peek ().GNO);
			//TODO put it in AsExpression interpret2, as is followed by a GameItem derived
			
			return this;
		}
		public override void Resolve(GameNarratorContext context){
			//TODO
			
		}
	}

	public class CostExpression : GameNarratorNonTerminalExpression
	{
		//public GamePlayer CO;
		
		public override string GetKeyword()
		{
			return "cost";
		}
		public override string GetExpressionPattern()
		{
			return @"";
		}
		public override string GetName()
		{
			return "Cost Trigger";
		}
		public override int GetPriority () {
			return 10;
		}
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, ref string cmd)
		{
			//TODO

			return this;
		}
		public override void Resolve(GameNarratorContext context){
			//TODO
			
		}
	}

	//TODO replace with Board.activeSquare
	public class CurrentSquareExpression : GameNarratorTerminalExpression
	{
		//public GamePlayer CO;
		
		public override string GetKeyword()
		{
			return "currentSquare";
		}
		public override string GetExpressionPattern()
		{
			return @"";
		}
		public override string GetName()
		{
			return "CurrentSquare Object";
		}
		public override int GetPriority () {
			return 10;
		}
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, ref string cmd)
		{
			//TODO
			
			return this;
		}
	}

	public class CraftinessExpression : GameNarratorTerminalExpression
	{
		//public GamePlayer CO;
		
		public override string GetKeyword()
		{
			return "craftiness";
		}
		public override string GetExpressionPattern()
		{
			return @"";
		}
		public override string GetName()
		{
			return "Craftiness Object";
		}
		public override int GetPriority () {
			return 10;
		}
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, ref string cmd)
		{
			//TODO
			
			return this;
		}
	}

	public class LivelinessExpression : GameNarratorTerminalExpression
	{
		//public GamePlayer CO;
		
		public override string GetKeyword()
		{
			return "liveliness";
		}
		public override string GetExpressionPattern()
		{
			return @"";
		}
		public override string GetName()
		{
			return "Liveliness Object";
		}
		public override int GetPriority () {
			return 10;
		}
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, ref string cmd)
		{
			//TODO
			
			return this;
		}
	}

	public class ThreatExpression : GameNarratorTerminalExpression
	{
		//public GamePlayer CO;
		
		public override string GetKeyword()
		{
			return "threat";
		}
		public override string GetExpressionPattern()
		{
			return @"";
		}
		public override string GetName()
		{
			return "Threat Object";
		}
		public override int GetPriority () {
			return 10;
		}
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, ref string cmd)
		{
			//TODO
			
			return this;
		}
	}

	public class SpeedExpression : GameNarratorTerminalExpression
	{
		//public GamePlayer CO;
		
		public override string GetKeyword()
		{
			return "speed";
		}
		public override string GetExpressionPattern()
		{
			return @"";
		}
		public override string GetName()
		{
			return "Speed Object";
		}
		public override int GetPriority () {
			return 10;
		}
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, ref string cmd)
		{
			//TODO
			
			return this;
		}
	}

	public class RangeExpression : GameNarratorTerminalExpression
	{
		//public GamePlayer CO;
		
		public override string GetKeyword()
		{
			return "range";
		}
		public override string GetExpressionPattern()
		{
			return @"";
		}
		public override string GetName()
		{
			return "Range Object";
		}
		public override int GetPriority () {
			return 10;
		}
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, ref string cmd)
		{
			//TODO
			
			return this;
		}
	}

	public class BonusCraftinessExpression : GameNarratorTerminalExpression
	{
		//public GamePlayer CO;
		
		public override string GetKeyword()
		{
			return "bonusCraftiness";
		}
		public override string GetExpressionPattern()
		{
			return @"";
		}
		public override string GetName()
		{
			return "BonusCraftiness Object";
		}
		public override int GetPriority () {
			return 10;
		}
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, ref string cmd)
		{
			//TODO
			
			return this;
		}
	}

	public class BonusDamageExpression : GameNarratorTerminalExpression
	{
		//public GamePlayer CO;
		
		public override string GetKeyword()
		{
			return "bonusDamage";
		}
		public override string GetExpressionPattern()
		{
			return @"";
		}
		public override string GetName()
		{
			return "BonusDamage Object";
		}
		public override int GetPriority () {
			return 10;
		}
		public override GameNarratorAbstractExpression Interpret(GameNarratorContext context, ref string cmd)
		{
			//TODO
			
			return this;
		}
	}

	#endregion
}