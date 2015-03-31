using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MyExtensions;

public class GenerateResources : MonoBehaviour
{
    private List<GameObject> resources;

    // Use this for initialization
    void Start()
    {
		/*
        resources = new List<GameObject>();

        GameObject go1 = GameObject.Instantiate(Resources.Load("Prefabs/Image_Ferraille")) as GameObject;
        go1.AddComponent<EventHandler>();
        resources.Add(go1);

        GameObject go2 = GameObject.Instantiate(Resources.Load("Prefabs/Image_Verre")) as GameObject;
        go2.AddComponent<EventHandler>();
        resources.Add(go2);

        GameObject go3 = GameObject.Instantiate(Resources.Load("Prefabs/Image_Nourriture")) as GameObject;
        go3.AddComponent<EventHandler>();
        resources.Add(go3);

        GameObject go4 = GameObject.Instantiate(Resources.Load("Prefabs/Image_Plastique")) as GameObject;
        go4.AddComponent<EventHandler>();
        resources.Add(go4);

        foreach(GameObject go in resources)
        {
            go.transform.SetParent(GameObject.Find("Panel_PopResources").transform, false);
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.SetDefaultScale();
            rect.localPosition = new Vector3(rect.position.x, rect.position.y, 0);
        }*/
    }

    // Update is called once per frame
    void Update()
    {

    }

	public static void PopResource(string rtype, float x, float y)
	{
		GameObject go = GameObject.Instantiate(Resources.Load("Prefabs/Image_"+rtype), new Vector3(x - Screen.width/2, y - Screen.height/2, -2.5f) / 1.24f, Quaternion.Euler(0, 0, 0)) as GameObject;
		go.AddComponent<EventHandler>();
	
		go.transform.SetParent(GameObject.Find("PopResources").transform, false);
		//RectTransform rect = go.GetComponent<RectTransform>();
		//rect.SetDefaultScale();
		//rect.localPosition = rect.TransformVector(new Vector3(x - Screen.width/2, y - Screen.height/2, -2.5f));


	}
}