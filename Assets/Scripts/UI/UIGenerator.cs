using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;

//using MyExtensions;
using NSBoard;

public class UIGenerator {// : MonoBehaviour {
    //public const float CENTER_X = 62;
    //public const float CENTER_Y = -15;
	public const float MARGIN = 2;

	//container = GameObject.Find ("ActionPrompter");
	public static void ActionButtonsFromList(GameObject container, List<string> ls) {
	    for(int i = 0; i < ls.Count; i++)
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
			rect.position = rect.TransformVector(pos);

			go.GetComponentInChildren<Text>().text = ls[i];


			Button b = go.GetComponent<Button>();
			b.interactable = true;
			Debug.Log (b);
			Debug.Log (b.onClick);
			b.onClick.AddListener(Board.ToggleBoard);
			/*b.onClick.AddListener(() => {
				Debug.Log ("Click click");	//handle click here
			});*/
        }
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

	public static void UpdateActionButtonsFromList(GameObject container, List<string> ls) {
		DestroyChildren (container);
		ActionButtonsFromList(container, ls);
	}



}
