using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActivePlayerStatsManager : MonoBehaviour
{

    public GameObject[] vivacity_list;
    public Sprite[] vivacity_states;
    public GameObject[] astuce_list;

    GameManager gm;

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void UpdateState()
    {
        Debug.Log("PLAYER CRAFTINESS :: " + gm.EM.Scenario.GetCurrentPlayer().Craftiness);
        Debug.Log("PLAYER Liveliness :: " + gm.EM.Scenario.GetCurrentPlayer().Liveliness);
        Debug.Log("PLAYER UsedLiveliness :: " + gm.EM.Scenario.GetCurrentPlayer().GetUsedLiveliness());
        for (int i = 0; i < gm.EM.Scenario.GetCurrentPlayer().Craftiness; i++)
        {
            astuce_list[i].SetActive(true);
        }
        for (int i = 0; i < gm.EM.Scenario.GetCurrentPlayer().MaxLiveliness; i++)
        {
            vivacity_list[i].SetActive(true);
            if (i < gm.EM.Scenario.GetCurrentPlayer().Liveliness)
                vivacity_list[i].GetComponent<Image>().sprite = vivacity_states[0];
            else
                if (i >= gm.EM.Scenario.GetCurrentPlayer().Liveliness && i < (gm.EM.Scenario.GetCurrentPlayer().Liveliness + gm.EM.Scenario.GetCurrentPlayer().GetUsedLiveliness()))
                {
                    vivacity_list[i].GetComponent<Image>().sprite = vivacity_states[1];
                }
                else
                {
                    vivacity_list[i].GetComponent<Image>().sprite = vivacity_states[2];
                }
        }
    }

}
