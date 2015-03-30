using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//using NSActionManager;

namespace NSGameNarrator{
	#region "GameClasses"

	// All game objects shall inherit from this class
	public class GameNarratorObject
	{
		// All instances
		public static Dictionary<string, GameNarratorObject> Vars = new Dictionary<string, GameNarratorObject> ();
		// A subset of vars
		public static Dictionary<string, List<GameNarratorObject>> Tags = new Dictionary<string, List<GameNarratorObject>> ();		


		public string VarName;
		public string DisplayedName;
		public string Tag;
		public GameNarratorAbstractExpression ParentExpression;
		public Dictionary<string, GameNarratorObject> Properties = new Dictionary<string, GameNarratorObject>();
		public List<string> RequiredProperties;
		public GameNarratorCommand Command = null;

		public List<GameNarratorCommand> ExpressionCommands = null;
		// Call AddCommand only when resolving expression line, after the GNC is fully constructed
		public void AddCommand(GameNarratorCommand gnc)
		{
			if (ExpressionCommands == null)
			{
				ExpressionCommands = new List<GameNarratorCommand>();
			}
			ExpressionCommands.Add(gnc);
		}
		public void ExecuteCommands(){
			if(ExpressionCommands != null) 
			{
				for (int i = 0; i < ExpressionCommands.Count; ++i) {
					ExpressionCommands[i]();
				}
			}
		}


		public GameNarratorObject(string varName, string displayedName, string tag) {
			VarName = varName;
			DisplayedName = displayedName;
			Tag = tag;
			ParentExpression = null;

			/*
			Debug.Log ("vn = " + varName + ","); 
			Debug.Log ("dn = " + displayedName + ",");
			Debug.Log ("t  = " + tag);
			 */
			if (VarName != "") {
				// Register instances
				if (!Vars.ContainsKey (VarName)) {
					Vars.Add (VarName, this);
				} else {
					Debug.LogError ("Variable \"" + VarName + "\" already exists");
					//TDDO Error crash
				}
			}

			// Register Tagged instances
			if (Tag != "") {
				List<GameNarratorObject> l;
				if (Tags.TryGetValue (Tag, out l)) {
					l.Add (this);
				} else {
					l = new List<GameNarratorObject> ();
					l.Add (this);
					Tags.Add (Tag, new List<GameNarratorObject> ());
				}

				// Properties initialization
				Properties ["Set"] = new GameNarratorObject ("", "", "");
				Properties ["Set"].Command = () => {return this;};
			} // else, GNO is a Command
		}
	}

	public class GameScenario // : GameNarratorObject
	{
		public static List<GameScenario> Scenarios = new List<GameScenario>();
		public static GameScenario GetScenarioById(int id)
		{
			//TODO optimize if possible? low priority...
			for(int i = 0; i < Scenarios.Count; ++i){
				if(Scenarios[i].Id == id)
				{
					return Scenarios[i];
				}
			}
			return null;
		}
		
		public int Id;
		public string Name;
		
		public GameScenario(int id, string name)
		{
			Id = id;
			Name = name;
			
			// Insertion sort
			if (Scenarios.Count == 0) {
				Scenarios.Add(this);
			} else {
				for(int i = 0; i < Scenarios.Count; ++i)
				{
					if (id < Scenarios[i].Id)
					{
						Scenarios.Add (this);
						return;
					}else if (id == Scenarios[i].Id)
					{
						Debug.LogError("Scenario #" + id.ToString() + " already exists");
						return;
					}
				}
				Scenarios.Add (this);
			}
			
		}
	}


	#endregion

	#region "GameNarratorObject extensions"
	public class GameCharacter
	{
		public static List<GameCharacter> Characters = new List<GameCharacter>();

		public GameNarratorObject GNO;
		
		public GameCharacter(GameNarratorObject gno)
		{
			GNO = gno;
			Characters.Add (this);

			GNO.RequiredProperties = new List<string>();
			//None are required;
		}
		
		public virtual void GetActions(out Dictionary<string, GameAction> al) // ?
		{
			al = new Dictionary<string, GameAction>();

			List<string> sls = new List<string>();
			sls.Add ("Text1");
			sls.Add ("Hey!");
			ActionManager.Context ["SayListString"] = sls;
			al ["Say"] = GameCharacter.Say;
		}
	
		public static void Say()//IEnumerator Say()
		{
			ActionManager.Busy = true; // Synchronous

			object o;
			if (ActionManager.Context.TryGetValue ("SayListString", out o)) {
				List<string> ls = o as List<string>;
				
				for (int i = 0; i < ls.Count; ++i) {
					Debug.Log (ls [i]);
					ls [i] = "hoho - " + i.ToString ();

					//yield
					new WaitForSeconds (2.5f);
					return;
				}
			}

			ActionManager.Busy = false; // Synchronous
		}
	}

	public class GamePlayer : GameCharacter
	{
		public static List<GamePlayer> Players = new List<GamePlayer>();
		public static int CurrentPlayerIndex = 0;
		public static GamePlayer CurrentPlayer;


		public GamePlayer (GameNarratorObject gno) : base(gno)
		{
			Players.Add (this);
			// TODO where is a GNO BoardSquare, it is updated when moving

			GNO.Properties ["where"] = new GameNarratorObject ("", "case 27", "where BoardSquare");
			//GNO.RequiredProperties.Add ("");
		}
		
		public override void GetActions(out Dictionary<string, GameAction> al) // List<GameCharacterAction>
		{
			base.GetActions(out al);

		}

	}

	public class GameEnemy : GameCharacter
	{
		public static List<GameEnemy> Enemies = new List<GameEnemy>();
		
		public GameEnemy (GameNarratorObject gno) : base(gno)
		{
			Enemies.Add (this);
			
			//GNO.RequiredProperties.Add ("");
		}
		
		public override void GetActions(out Dictionary<string, GameAction> al) // ?
		{
			base.GetActions(out al);


		}

	}
	#endregion


	#region "ExtendedGameClasses"
	//GameNarratorProperty

	public class DeusExMachina
	{
		public List<GameNarratorAbstractExpression> Lgnabex;
		
		public DeusExMachina(List<GameNarratorAbstractExpression> lgnabex)
		{
			Lgnabex = lgnabex;
		}
		
		public void LootBonus(GameNarratorObject gno)
		{
			Debug.Log ("DeusExMachina : ");
			Debug.Log (gno);
			for (int i = 0; i < Lgnabex.Count; ++i) {
				Debug.Log(Lgnabex[i].ToString());
			}
		}
		
		public void LootBonus2(GameNarratorObject gno)
		{
			Debug.Log ("DeusExMachina2 : ");
			Debug.Log (gno);
			for (int i = 0; i < Lgnabex.Count; ++i) {
				Debug.Log(Lgnabex[i].ToString());
			}
		}
		
	}
	#endregion
}