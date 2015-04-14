using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using MyExtensions;
using UnityEngine.EventSystems;

public class GenerateNumbers : MonoBehaviour, IDragHandler
{
    private List<GameObject> listNumberGO;
    private List<int> listNumber;
    public bool isAnimationStarted = false, isAnimationEnded = false;

    private float animationStartTime;

    public float defilementSpeed = 6f, animationDuration = 1.2f, sigma = 1.2f;
    public float Height, Width;

    private float randDuration;
    private float nbHeight, nbWidth;

    private System.Random r = new System.Random();
    // Use this for initialization
    void Start()
    {
        listNumberGO = new List<GameObject>();
        listNumber = new List<int>();

        DisplayNumbers();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAnimationStarted)
        {
            if ((Time.time - animationStartTime) < randDuration)
            {
                // Défilement des chiffres
                foreach (GameObject go in listNumberGO)
                {
                    RectTransform rect = go.GetComponent<RectTransform>() as RectTransform;

                    //if (defilementSpeed > 0)
                    //{
                    //    if (rect.position.y < Height)
                    //        rect.position = new Vector3(rect.position.x, rect.position.y + defilementSpeed, rect.position.z);
                    //    else
                    //        rect.position = new Vector3(rect.position.x, (rect.position.y % Height) - Height * 2f, rect.position.z);
                    //}
                    //else
                    //{
                    //    if (rect.position.y > -Height * 2f)
                    //        rect.position = new Vector3(rect.position.x, rect.position.y + defilementSpeed, rect.position.z);
                    //    else
                    //        rect.position = new Vector3(rect.position.x, (rect.position.y + Height * 2f) + Height, rect.position.z);
                    //}

                    if (defilementSpeed > 0)
                    {
                        if (rect.position.x < Width)
                            rect.position = new Vector3(rect.position.x + defilementSpeed, rect.position.y, rect.position.z);
                        else
                            rect.position = new Vector3((rect.position.x % Width) - Width * 2f, rect.position.y, rect.position.z);
                    }
                    else
                    {
                        if (rect.position.x > -Width * 2f)
                            rect.position = new Vector3(rect.position.x + defilementSpeed, rect.position.y, rect.position.z);
                        else
                            rect.position = new Vector3((rect.position.x + Width * 2f) + Width, rect.position.y, rect.position.z);
                    }
                }
                //Debug.Log(listNumberGO[0].GetComponent<RectTransform>().position.x < Width ? listNumberGO[0].GetComponent<RectTransform>().position.x.ToString() : "False");
            }
            else
            {
                // Fin animation et affichage du rand
                isAnimationEnded = true;
                isAnimationStarted = false;
                DisplayNumbers();
                //Debug.Log(Value());
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isAnimationStarted)
        {
            //defilementSpeed = Math.Sign(eventData.delta.y) * Math.Abs(defilementSpeed);
            defilementSpeed = Math.Sign(eventData.delta.x) * Math.Abs(defilementSpeed);
            randDuration = animationDuration + (sigma * (float)r.NextDouble() - 0.5f);

            DisplayNumbers();
            isAnimationStarted = true;
            animationStartTime = Time.time;
        }
    }

    public int Value()
    {
        if (isAnimationEnded)
            return int.Parse(listNumberGO[3].GetComponent<Text>().text);
        else
            return -1;
    }

    private void DisplayNumbers()
    {
        Clear();

        nbHeight = GetComponent<RectTransform>().rect.height;
        nbWidth = GetComponent<RectTransform>().rect.width;

        for (int i = 0; i < 6; i++)
        {
            GameObject go = GameObject.Instantiate(Resources.Load("Prefabs/GUI/Number")) as GameObject;
            go.transform.SetParent(this.transform, false);

            RectTransform textRect = go.GetComponent<RectTransform>() as RectTransform;
            textRect.SetHeight(Height);
            textRect.SetWidth(Width);
            textRect.anchorMin = new Vector2(0, 0);
            textRect.anchorMax = new Vector2(1, 0);
            //textRect.anchoredPosition = new Vector2(0, (Height / 2) * (i-3) + (nbHeight / 2f));
            textRect.anchoredPosition = new Vector2((Width / 2) * (i - 3) + (nbWidth / 2f), 40);

            bool valid = false;
            while (!valid)
            {
                var val = r.Next(1, 7);

                bool found = false;
                foreach (int savedValue in listNumber)
                {
                    if (val == savedValue)
                        found = true;
                }

                if (!found)
                {
                    Text t = go.GetComponent<Text>() as Text;
                    t.text = val.ToString();

                    listNumberGO.Add(go);
                    listNumber.Add(val);

                    valid = true;
                }
            }
        }
    }

    private void Clear()
    {
        foreach (GameObject go in listNumberGO)
        {
            GameObject.Destroy(go);
        }
        listNumberGO.Clear();
        listNumber.Clear();
    }

    public void Activate(bool b)
    {
        if (b)
        {
            isAnimationEnded = false;
        }
        gameObject.SetActive(b);
    }

}