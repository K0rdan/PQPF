using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MyExtensions;

public class GenerateCats : MonoBehaviour {
    private List<GameObject> cats; 

	// Use this for initialization
	void Start () {
        cats = new List<GameObject>();

        for (var i = 0; i < 3; i++) 
        {
            GameObject go = GameObject.Instantiate(Resources.Load("Prefabs/Image_Cat")) as GameObject;
            go.AddComponent<EventHandler>();

            go.transform.SetParent(GameObject.Find("Panel_PopCats").transform, false);
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.SetDefaultScale();
            rect.localPosition = new Vector3(rect.position.x, rect.position.y, 0);

            cats.Add(go);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
