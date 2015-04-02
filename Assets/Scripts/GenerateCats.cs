using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MyExtensions;

public class GenerateCats : MonoBehaviour {
	private List<GameObject> cats;
	
	private List<Cat> tempCats;
	private List<Vector2> tempCases;
	
	// Use this for initialization
	void Start () {
		cats = new List<GameObject>();
		
		// TEMP
		tempCats = new List<Cat>();
		tempCats.Add(new Cat() { Name = "Fidel", HP = 1000, caseNumber = 1 });
		tempCats.Add(new Cat() { Name = "Test2", HP = 3, caseNumber = 2 });
		tempCats.Add(new Cat() { Name = "Test3", HP = 3, caseNumber = 3 });
		tempCats.Add(new Cat() { Name = "Test4", HP = 3, caseNumber = 4 });
		tempCats.Add(new Cat() { Name = "Test5", HP = 3, caseNumber = 5 });
		tempCats.Add(new Cat() { Name = "Test6", HP = 3, caseNumber = 6 });
		tempCats.Add(new Cat() { Name = "Test7", HP = 3, caseNumber = 7 });
		tempCats.Add(new Cat() { Name = "Test8", HP = 3, caseNumber = 8 });
		tempCats.Add(new Cat() { Name = "Test9", HP = 3, caseNumber = 9 });
		tempCats.Add(new Cat() { Name = "Test10", HP = 3, caseNumber = 10 });
		tempCats.Add(new Cat() { Name = "Test11", HP = 3, caseNumber = 11 });
		tempCats.Add(new Cat() { Name = "Test12", HP = 3, caseNumber = 12 });
		tempCats.Add(new Cat() { Name = "Test13", HP = 3, caseNumber = 13 });
		tempCats.Add(new Cat() { Name = "Test14", HP = 3, caseNumber = 14 });
		tempCats.Add(new Cat() { Name = "Test15", HP = 3, caseNumber = 15 });
		tempCats.Add(new Cat() { Name = "Test16", HP = 3, caseNumber = 16 });
		tempCats.Add(new Cat() { Name = "Test17", HP = 3, caseNumber = 17 });
		tempCats.Add(new Cat() { Name = "Test18", HP = 3, caseNumber = 18 });
		tempCats.Add(new Cat() { Name = "Test19", HP = 3, caseNumber = 19 });
		tempCats.Add(new Cat() { Name = "Test20", HP = 3, caseNumber = 20 });
		tempCats.Add(new Cat() { Name = "Test21", HP = 3, caseNumber = 21 });
		tempCats.Add(new Cat() { Name = "Test22", HP = 3, caseNumber = 22 });
		tempCats.Add(new Cat() { Name = "Test23", HP = 3, caseNumber = 23 });
		tempCats.Add(new Cat() { Name = "Test24", HP = 3, caseNumber = 24 });
		tempCats.Add(new Cat() { Name = "Test25", HP = 3, caseNumber = 25 });
		tempCats.Add(new Cat() { Name = "Test26", HP = 3, caseNumber = 26 });
		tempCats.Add(new Cat() { Name = "Test27", HP = 3, caseNumber = 27 });
		tempCats.Add(new Cat() { Name = "Test28", HP = 3, caseNumber = 28 });
		tempCats.Add(new Cat() { Name = "Test29", HP = 3, caseNumber = 29 });
		tempCats.Add(new Cat() { Name = "Test30", HP = 3, caseNumber = 30 });
		tempCats.Add(new Cat() { Name = "Test31", HP = 3, caseNumber = 31 });
		tempCats.Add(new Cat() { Name = "Test32", HP = 3, caseNumber = 32 });
		tempCats.Add(new Cat() { Name = "Test33", HP = 3, caseNumber = 33 });
		tempCats.Add(new Cat() { Name = "Test34", HP = 3, caseNumber = 34 });
		tempCats.Add(new Cat() { Name = "Test35", HP = 3, caseNumber = 35 });
		
		tempCases = new List<Vector2>();
		tempCases.Add(new Vector2(400, 370));
		tempCases.Add(new Vector2(570, 370));
		tempCases.Add(new Vector2(650, 250));
		tempCases.Add(new Vector2(590, 40));
		tempCases.Add(new Vector2(700, -10));
		tempCases.Add(new Vector2(610, -290));
		tempCases.Add(new Vector2(710, -330));
		tempCases.Add(new Vector2(330, -390));
		tempCases.Add(new Vector2(40, -390));
		tempCases.Add(new Vector2(190, -470));
		tempCases.Add(new Vector2(-270, -365));
		tempCases.Add(new Vector2(-320, -250));
		tempCases.Add(new Vector2(-470, -170));
		tempCases.Add(new Vector2(-570, -270));
		tempCases.Add(new Vector2(-670, -370));
		tempCases.Add(new Vector2(-670, -170));
		tempCases.Add(new Vector2(-710, -65));
		tempCases.Add(new Vector2(-340, -120));
		tempCases.Add(new Vector2(-300, 0));
		tempCases.Add(new Vector2(-480, 35));
		tempCases.Add(new Vector2(-300, 110));
		tempCases.Add(new Vector2(-420, 170));
		tempCases.Add(new Vector2(-520, 205));
		tempCases.Add(new Vector2(-300, 255));
		tempCases.Add(new Vector2(-170, 315));
		tempCases.Add(new Vector2(0, 315));
		tempCases.Add(new Vector2(155, 315));
		tempCases.Add(new Vector2(340, 235));
		tempCases.Add(new Vector2(510, 100));
		tempCases.Add(new Vector2(512, -165));
		tempCases.Add(new Vector2(335, -310));
		tempCases.Add(new Vector2(-20, -310));
		tempCases.Add(new Vector2(-195, -160));
		tempCases.Add(new Vector2(-195, 105));
		tempCases.Add(new Vector2(-20, 235));
		//
		
		for (var i = 0; i < 35; i++) 
		{
			GameObject go = GameObject.Instantiate(Resources.Load("Prefabs/Pieces/Chat")) as GameObject;
			
			EventHandler catEventHandler = go.AddComponent<EventHandler>();
			catEventHandler.catName = tempCats[i].Name;
			catEventHandler.catHP = tempCats[i].HP;
			
			
			go.transform.SetParent(GameObject.Find("Panel_PopCats").transform, false);
			RectTransform rect = go.GetComponent<RectTransform>();
			rect.SetDefaultScale();
			rect.position = new Vector3(tempCases[i].x, tempCases[i].y, 0);
			cats.Add(go);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

public class Cat
{
	public string Name { get; set; }
	public int  HP { get; set; }
	public int caseNumber { get; set; }
}
