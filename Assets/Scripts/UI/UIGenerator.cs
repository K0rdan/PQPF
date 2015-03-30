using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Collections.Generic;

//using MyExtensions;
using NSBoard;
//using NSActionManager;

public class UIGenerator {// : MonoBehaviour {
    //public const float CENTER_X = 62;
    //public const float CENTER_Y = -15;
	private const float MARGIN = 2;

	//container = GameObject.Find ("ActionPrompter");
	public static void ActionButtonsFromList(GameObject container, Dictionary<string, GameAction> al) {
	    //for(int i = 0; i < al.Count; i++)
        //{

		int i = 0;
		foreach(KeyValuePair<string, GameAction> entry in al)
		{
			GameObject go = GameObject.Instantiate(Resources.Load("Prefabs/ActionButton")) as GameObject;
            
            go.transform.SetParent(container.transform);

            RectTransform rect = go.GetComponent<RectTransform>();

			Vector3 scale = rect.localScale;
			scale.x = 1;
			scale.y = 1;
			scale.z = 1;
			rect.localScale = scale;

			Vector3 pos = rect.position;
			pos.y -= i * (rect.rect.height + MARGIN);
			++i;
			rect.position = rect.TransformVector(pos);

			go.GetComponentInChildren<Text>().text = entry.Key;


			Button b = go.GetComponent<Button>();
			b.interactable = true;

			string s = entry.Key; // Closure issue : store external value (entry.Key) in internal variable (s)
			b.onClick.AddListener(() => {ActionManager.DoAction(s);});
        }

		ActionManager.LoadActionList(al);
	}

	public static void DestroyChildren(GameObject container) {
		Component[] comps = container.GetComponentsInChildren(typeof(Component));

		List<GameObject> lgo = new List<GameObject>();

		for (int i = 0; i < container.transform.childCount; ++i) {
			lgo.Add (container.transform.GetChild(i).gameObject);
		}

		for (int i = 0; i < lgo.Count; ++i) {
			GameObject.Destroy (lgo[i]);
		}
	}

	public static void UpdateActionButtonsFromList(GameObject container, Dictionary<string, GameAction> al) {
		DestroyChildren (container);
		ActionButtonsFromList(container, al);
	}

}
