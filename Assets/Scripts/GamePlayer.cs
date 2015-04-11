using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GamePlayer : MonoBehaviour
{
	private GameManager GM;

	public string Name;
	public int Craftiness;
	public int MaxLiveliness;

	public int Liveliness;
	public int Damage = 0;
	public int Range = 0;
	private int HealRest = 0;

	private int DigUpCount = 0;

	public List<object> Skills = new List<object> ();
	
	public List<object> Inventory = new List<object> ();

	public BoardSquare CurrentSquare;
	public List<BoardSquare> Reach;
	public List<BoardSquare> Path;

	public bool Fleeing = false;
	public bool HasFailedToFlee = false;
	public int FleeIterator = 0;

	public List<BoardSquare> FightReach;
	public List<BoardSquare> TargetableSquares = new List<BoardSquare>();
	public List<GameEnemy> TargetableEnemies = new List<GameEnemy>();
	public GameEnemy TargetEnemy;

	private Vector3 defaultSpritePosition;
	public Vector3 DefaultSpritePosition {
		get{
			return defaultSpritePosition;
		}
		set{
			defaultSpritePosition = value;
		}
	}

	public void Awake()
	{
		GM = GameObject.Find("GameManager").GetComponent<GameManager> ();
	}
	
	public void Refresh()
	{

		Damage -= HealRest;
		Damage = (Damage < 0) ? 0 : Damage;
		HealRest = 0;
		Liveliness = MaxLiveliness - Damage;
		DigUpCount = 0;
		Fleeing = false;
		FleeIterator = 0;
		HasFailedToFlee = false;

		if (!IsActive ()) {
			GM.DoNext();
		}
	}

	public bool IsActive(){
		return Liveliness > 0;
	}

	public int GetUsedLiveliness()
	{
		return MaxLiveliness - Damage - Liveliness;
	}

	public void GetActions(out List<string> ls) // List<GameCharacterAction>
	{
		ls = new List<string> ();

		if(!(Liveliness < DigUpCost()))
		{
			ls.Add("Fouille");
		}

		/*if (CurrentSquare.IsThreatened (this)) {
			if(!HasFailedToFlee)
			{
				ls.Add ("Fuite");
			}
		} else */
		if (Liveliness > 0 && !HasFailedToFlee) {
			ls.Add ("Déplacement");
		}

		if (Liveliness >= 2){
			FightReach = GM.GameBoard.Reach(CurrentSquare, Range);

			TargetableSquares.Clear();
			TargetableEnemies.Clear();
			for(int i = 0; i < FightReach.Count; ++i)
			{
				if(FightReach[i].IsThreatened(this)){
					TargetableSquares.Add (FightReach[i]);
					TargetableEnemies.AddRange(FightReach[i].ThreatList(this));
				}
			}
			if(TargetableSquares.Count > 0)
			{ 
				ls.Add ("Combat");
			}
		}

		// TODO, not ready yet
		if (HasActivableSkill()) {
			ls.Add ("Capacité");
		}

		// TODO, not ready yet
		ls.Add ("Inventaire");

		// Always available
		ls.Add ("Repos");

	}
	
	public void GetActiveSkills(){
		// search skill
	}
	
	public int DigUpCost()
	{
		return 1 + DigUpCount;
	}
	public void	DigUp(object modifier)
	{
		GamePlayer p = GM.EM.Scenario.GetCurrentPlayer ();
		Resource rsr = p.CurrentSquare.digUpResource ();
		Debug.Log ("Dug up : " + rsr.Name);
		// TODO launch animation


		p.Inventory.Add(rsr);

		Liveliness -= DigUpCost();
		++DigUpCount;
	}

	public void Move(){
		// UI

		// TODO Resolve Path
		int i = 1;
		while (i < Path.Count)
		{
			// TODO Extension : square state, ablaze
			if (!CurrentSquare.IsThreatened (this) || Fleeing)
			{
				Fleeing = false; // Fleeing once per square
				FleeIterator = 0;
				MoveTo(Path[i]);
				--Liveliness;
				DigUpCount = 0;

				// TODO Extension Square passing (eg. through an ablaze square, lose 1 life)
				// ...
				///
			}/* else {
				// The current square is threatened
				GM.GameBoard.Phase = Board.BoardPhase.PlayerFleeing;
				Path.RemoveRange(0, i - 1); // The remaining path will be remembered 
				Debug.Log("Remaining path count : " + Path.Count);

				return;
			}*/
			++i;
		}

		GM.DM.ReloadActionPrompter ();
	}

	public void MoveTo(BoardSquare s){
		if (s != null) {
			if(CurrentSquare != null){
				CurrentSquare.Players.Remove(this);
			}
			CurrentSquare = s;
			s.Players.Add (this);
		} else {
			
		}
	}
		
	public bool CanFight(){
		return true;
	}
	// TODO fight is when the player tries to hurt his target... not before selecting
	// create a 
	public void GetFightTarget(){
		GM.DM.HideActionPrompter();
		
		for (int i = 0; i < TargetableEnemies.Count; ++i){
			GameEnemy enemy = TargetableEnemies[i];
			
			// Instantiate a clickable button 
			GameObject buttonInstance = GameObject.Instantiate (Resources.Load<GameObject> ("Prefabs/GUI/EnemySelectionButton")) as GameObject;
			buttonInstance.GetComponentInChildren<Text>().text = TargetableEnemies[i].Name + " - Menace : " + enemy.Threat + " - Vie : " + enemy.Life + " - Case : " + (enemy.CurrentSquare.Id + 1);
			GameObject enemyListPanel = GM.PlayerProfileScreen.transform.Find("EnemyList").gameObject;
			
			RectTransform rect = buttonInstance.GetComponent<RectTransform>();
			rect.position = new Vector3(rect.position.x, rect.position.y + i * 100, 0);
			
			buttonInstance.transform.SetParent(enemyListPanel.transform);
			
			// onclick, select this, remove all buttons in enemylist and go to attack phase
			Button button = buttonInstance.GetComponent<Button>();
			button.interactable = true;
			button.onClick.AddListener(() => {
				TargetEnemy = enemy;
				Liveliness -= 2;

				GM.GameBoard.Phase = Board.BoardPhase.PlayerAttacking;

				GM.DM.ClearEnemyList();
			});
		}
	}
	public void Fight(){
		Debug.Log ("Target is : " + TargetEnemy);
		if (TargetEnemy == null) {
			return;
		}
		
		int dice = 0;
		if (GM.GameBoard.RandomSlider.GetComponent<GenerateNumbers> ().isAnimationEnded) {
			dice = GM.GameBoard.RandomSlider.GetComponent<GenerateNumbers> ().Value();
		} else {
			Debug.LogError ("Player Fight has been called while Slider animation was running");
		}
		
		if (TargetEnemy.Threat <= Craftiness + dice) { // + BONUS Craftiness
			int dmg = Craftiness + dice - TargetEnemy.Threat;
			Debug.Log (Name + "hurts " + TargetEnemy.Name + " [" + dmg + "damage(s)]");
			TargetEnemy.Hurt (dmg); // TODO + BONUS Damage
		} else {
			Debug.Log (Name + " misses its attack");
		}
	}

	public void Hurt(){
		Debug.Log (Name + " is hurt");

		if (GetUsedLiveliness () == 0) {
			--Liveliness;
		}
		++Damage;

		if (Damage >= MaxLiveliness) {
			Die ();
		}
	}

	public void Die(){
		CurrentSquare.Players.Remove (this);
		GM.EM.Scenario.Players.Remove (this); // TODO be healable instead?
	}
	public void Heal(int life){
		//TODO Respawn on this square

	}


	//TODO
	public bool HasActivableSkill(){
		return false;
	}
	public void Skill() // TODO Skills?
	{
		Liveliness -= 2;
	}

	public void LookInventory()
	{

	}

	public void Rest()
	{
		HealRest = Liveliness / 2;
	}
	
	
	public static void Spawn(string name)
	{

	}
}