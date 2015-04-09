using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;

public class GameEnemy : MonoBehaviour
{
	private GameManager GM;

	// TODO name it in prefabs...
	public string Name;

	public int Threat;
	public int Life;
	public int Speed;
	public int Loot;
	private int Range = 0;

	public enum BehaviourParadigm{
		NearestWeakest,
		NearestStrongest,
		Strongest,
		Weakest,
		FixedTarget,
		PlayerTarget,
		EnemyTarget
	};
	public BehaviourParadigm Behaviour;
	public string ObjectiveArgument;

	public BoardSquare Target;
	public List<BoardSquare> Path;

	public BoardSquare CurrentSquare;

	public GamePlayer TargetPlayer;
	public GameEnemy TargetEnemy; // TODO extension
	
	public void Awake(){
		GM = GameObject.Find("GameManager").GetComponent<GameManager> ();
	}

	/*public GameEnemy (string displayedName, string tag, int threat, int speed, int loot) : base(displayedName, tag)
	{
		Threat = threat;
		Life = threat;
		Speed = speed;
		Loot = loot;
		Range =
	}*/
	
	public void GetActions(out Dictionary<string, GameAction> al) // ?
	{
		al = new Dictionary<string, GameAction> ();
		
	}

	public void ComputePath(){
		switch (Behaviour) {
		case BehaviourParadigm.NearestWeakest:
			Path = null;
			List<List<BoardSquare>> pathList = new List<List<BoardSquare>>();

			//Nearest then with the most damage ans then the less maxLiveliness (ie the last player)
			int maxDamage = 0;
			int minDistance = 10000;
			List<GamePlayer> players = GM.EM.Scenario.Players;
			int i = players.Count - 1;
			while(i >= 0) // Reverse so we can get the weakest in terms of MaxLiveliness
			{
				List<BoardSquare> lbs;
				GM.GameBoard.AStar(CurrentSquare, players[i].CurrentSquare, out lbs);

				if(lbs != null)
				{
					if(lbs.Count < minDistance)
					{
						minDistance = lbs.Count;
						Path = lbs;
						maxDamage = players[i].Damage;
					}else if(lbs.Count == minDistance)
					{
						if(players[i].Damage > maxDamage){
							maxDamage = players[i].Damage;
							Path = lbs;
						}
					}
				}

				--i;
			}
			break;
		case BehaviourParadigm.FixedTarget:
			GM.GameBoard.AStar(CurrentSquare, Target, out Path);
			break;
		case BehaviourParadigm.PlayerTarget:
			//TODO
			//GM.GameBoard.AStar(CurrentSquare, (ObjectiveArgument), Path);
			break;
		case BehaviourParadigm.EnemyTarget:
			//TODO
			//GM.GameBoard.AStar(CurrentSquare, (ObjectiveArgument), Path);
			break;

			//TODO other paradigms
		}
	}

	public void Move(){
		// UI
		/*if (CurrentSquare.IsThreatened (this)) {
			// Fight
			//selectTarget
			SeekAndDestroy ();
		} else {
		*/	
		ComputePath();
		if(Path != null)
		{
			Color green = new Color(0, 1, 0, 0.25f);
			Color red = new Color(1, 0.5f, 0, 0.3f);

			Path[0].SetMaterial(Path[0].matEnter, red);
			int i = 1;
			while(i < Speed && !CurrentSquare.IsThreatened (this))
			{
				Debug.Log ("Enemy moving to #" + (Path[i].Id+1));
				MoveTo(Path[i]);

				// Colorize movement path
				if (i < Path.Count - 1 && !CurrentSquare.IsThreatened (this))
				{
					Path[i].SetMaterial(Path[i].matEnter, green);
				} else {
					Path[i].SetMaterial(Path[i].matEnter, red);
				}

				++i;
			}


			/*if(CurrentSquare.IsThreatened (this))
			{
				// Fight
				//selectTarget
				SeekAndDestroy ();
			}*/
		}
		//}
	}

	public void MoveTo(BoardSquare s){
		// TODO extend this
		//... eg trigger SquareEvents
		//
		if (s != null) {
			if(CurrentSquare != null){
				CurrentSquare.Enemies.Remove(this);
			}
			CurrentSquare = s;
			s.Enemies.Add (this);

			RectTransform rect = gameObject.GetComponent<RectTransform>();
			rect.position = new Vector3(s.Center.x, s.Center.y, 0);
		} else {
			
		}
	}
	
	public bool CanFight(){
		return true;
	}

	// TODO not a bool?
	public bool Fight(){
		Debug.Log ("Target is : " + TargetPlayer);
		if (TargetPlayer == null) {
			return true; // Done
		}

		int dice = 0;
		if (GM.GameBoard.RandomSlider.GetComponent<GenerateNumbers> ().isAnimationEnded) {
			dice = GM.GameBoard.RandomSlider.GetComponent<GenerateNumbers> ().Value();
		} else {
			Debug.LogError ("Enemy Fight has been called while Slider animation was running");
		}

		if (Threat > TargetPlayer.Craftiness + dice) {
			Debug.Log ("Cat hurts " + TargetPlayer.Name);
			TargetPlayer.Hurt ();
		} else {
			Debug.Log (TargetPlayer.Name + " evades Cat's attack");
		}
		return false;
	}
	// Only on this square... TODO handle Range
	public void GetFightTarget() {
		Debug.Log ("FightTarget");
		switch (Behaviour) {
		case BehaviourParadigm.NearestWeakest:
			Debug.Log ("NearestWeakest");
			int maxDamage = int.MinValue;
			for (int i = CurrentSquare.Players.Count - 1; i > -1; --i) {
				if (maxDamage < CurrentSquare.Players [i].Damage) {
					maxDamage = CurrentSquare.Players [i].Damage;
					TargetPlayer = CurrentSquare.Players [i];
				}
			}

			break;
		case BehaviourParadigm.NearestStrongest:
			Debug.Log ("NearestStrongest");
			int minDamage = int.MaxValue;
			for (int i = 0; i < CurrentSquare.Players.Count; ++i) {
				if (minDamage > CurrentSquare.Players [i].Damage) {
					minDamage = CurrentSquare.Players [i].Damage;
					TargetPlayer = CurrentSquare.Players [i];
				}
			}

			break;
		default:
			TargetPlayer = CurrentSquare.Players [0];
			break;
		}
	}
	
	public void Hurt(int damage){
		Debug.Log ("Cat is hurt");
		Life -= damage;
	}

	public void Die(){
		Debug.Log ("Cat Dies");
		CurrentSquare.Enemies.Remove (this);
		GM.GameBoard.Enemies.Remove (this);
	}
}