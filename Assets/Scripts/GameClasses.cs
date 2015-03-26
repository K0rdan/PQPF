using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace NSGameNarrator{
	#region "GameClasses"
	public class GameNarratorObject
	{
		public string VarName;
		public string DisplayedName;
		public string Tag;
		public GameNarratorAbstractExpression ParentExpression;
		
		public Dictionary<string, GameNarratorObject> RequiredAttributes;
		//TODO
		//public GameNarratorType GNType;
		
		// All instances
		public static Dictionary<string, GameNarratorObject> Vars = new Dictionary<string, GameNarratorObject> ();
		// A subset of vars
		public static Dictionary<string, List<GameNarratorObject>> Tags = new Dictionary<string, List<GameNarratorObject>> ();
		
		
		public GameNarratorObject(string varName, string displayedName, string tag) {
			VarName = varName;
			DisplayedName = displayedName;
			Tag = tag;
			ParentExpression = null;

			Debug.Log ("vn = " + varName + ","); 
			Debug.Log ("dn = " + displayedName + ",");
			Debug.Log ("t  = " + tag);
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
			}
		}
	}

	public class GameScenario
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

	public class GameCharacter
	{
		public static List<GameCharacter> Characters = new List<GameCharacter>();
		
		public GameNarratorObject GNO;
		
		public GameCharacter(GameNarratorObject gno)
		{
			GNO = gno;
			Characters.Add (this);
			
			GNO.RequiredAttributes = new Dictionary<string, GameNarratorObject>();
			//GNO.RequiredAttributes.add();
		}
		
		public void Action()
		{
			
		}
	}

	public class GamePlayer : GameCharacter
	{
		public static List<GamePlayer> Players = new List<GamePlayer>();
		
		public GamePlayer (GameNarratorObject gno) : base(gno)
		{
			Players.Add (this);
			
			GNO.RequiredAttributes = new Dictionary<string, GameNarratorObject>();
			//GNO.RequiredAttributes.add (); 
		}
		
		public void Action()
		{
			
		}
		
		
	}

	public class GameEnemy : GameCharacter
	{
		public static List<GameEnemy> Enemies = new List<GameEnemy>();
		
		public GameEnemy (GameNarratorObject gno) : base(gno)
		{
			Enemies.Add (this);
			
			GNO.RequiredAttributes = new Dictionary<string, GameNarratorObject>();
			//GNO.RequiredAttributes.add ();
		}
		
		public void Action()
		{
			
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
		
		public void LootBonus()
		{
			Debug.Log ("DeusExMachina : ");
			for (int i = 0; i < Lgnabex.Count; ++i) {
				Debug.Log(Lgnabex[i].ToString());
			}
		}
		
		public void LootBonus2()
		{
			Debug.Log ("DeusExMachina2 : ");
			for (int i = 0; i < Lgnabex.Count; ++i) {
				Debug.Log(Lgnabex[i].ToString());
			}
		}
		
	}
	#endregion
}