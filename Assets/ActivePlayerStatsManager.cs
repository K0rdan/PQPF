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
        for (int i = 0; i < gm.EM.Scenario.GetCurrentPlayer().Craftiness; i++)
        {
            astuce_list[i].SetActive(true);
        }
        if (astuce_list.Length > gm.EM.Scenario.GetCurrentPlayer().Craftiness)
        {
            for (int i = gm.EM.Scenario.GetCurrentPlayer().Craftiness; i < astuce_list.Length; i++)
            {
                astuce_list[i].SetActive(false);
            }
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
        if (vivacity_list.Length > gm.EM.Scenario.GetCurrentPlayer().MaxLiveliness)
        {
            for (int i = gm.EM.Scenario.GetCurrentPlayer().MaxLiveliness; i < vivacity_list.Length; i++)
            {
                vivacity_list[i].SetActive(false);
            }
        }
    }

}
