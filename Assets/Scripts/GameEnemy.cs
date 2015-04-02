using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameEnemy : MonoBehaviour
{
	public int Threat;
	public int Life;
	public int Speed;
	public int Loot;
	private int Range = 0;
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
	
	public static void Spawn(string name){
		
	}
}