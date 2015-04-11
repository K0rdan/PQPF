using UnityEngine;
using System.Collections;

public class CombatSpiteHandler : MonoBehaviour
{

    public GameObject niles;
    public GameObject volotom;
    public GameObject cat;
    public GameObject souris;
    public GameObject filou;

    public void UpdateSate(string id)
    {
        Clear();
        switch (id)
        {
            case "volotom":
                volotom.SetActive(true);
                break;
            case "niles":
                niles.SetActive(true);
                break;
            case "souris":
                souris.SetActive(true);
                break;
            case "filou":
                filou.SetActive(true);
                break;
            case "cat":
                cat.SetActive(true);
                break;
            default:
                Debug.LogError("Unknown ID for Target Sprite Manager");
                break;
        }
    }

    void Clear()
    {
        niles.SetActive(false);
        volotom.SetActive(false);
        cat.SetActive(false);
        souris.SetActive(false);
        filou.SetActive(false);
    }


}
