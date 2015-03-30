using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Collections.Generic;

//namespace NSActionManager {
public delegate void GameAction();
//public delegate IEnumerator GameAction();

public class ActionManager : MonoBehaviour {
	public static Dictionary<string, object> Context = new Dictionary<string, object>();
	public static Dictionary<string, GameAction> AL;

	private static ActionManager instance;
	public static ActionManager Instance {
		get
		{
			return instance;
		}
	}

	public static bool Asynchronous = false;
	public static bool Busy = false;
	//public static object Lock = new object();

	// Use this for initialization
	void Start () {
		instance = this;
	}

	// Update is called once per frame
	void Update () {}

	public static bool LoadActionList(Dictionary<string, GameAction> al)
	{
		if(!Busy)
		{
			AL = al;
			return true;
		}
		return false;
	}

	public static void DoAction (string s)
	{
		if (!Busy) {
			GameAction a;
			if (AL.TryGetValue (s, out a)) {
				a();
				/*if(a != null){
					Instance.StartCoroutine (a ());
				}*/
			}
		}
	}

}
//}