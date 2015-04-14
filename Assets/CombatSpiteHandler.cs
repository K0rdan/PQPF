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
            case "Pyromancien":
                volotom.SetActive(true);
                break;
            case "Chasseur de Trésors":
                niles.SetActive(true);
                break;
            case "Medic":
                souris.SetActive(true);
                break;
            case "Filou":
                filou.SetActive(true);
                break;
            case "cat":
                cat.SetActive(true);
                break;
            default:
                Debug.LogError("Unknown ID for Target Sprite Manager : " + id);
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
