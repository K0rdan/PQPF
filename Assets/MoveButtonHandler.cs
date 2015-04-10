using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MoveButtonHandler : MonoBehaviour
{
    public Sprite deplacement;
    public Sprite fuite;

    GameManager gm;
    bool isAllowedToMove;

    void Start()
    {
        isAllowedToMove = true;
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void UpdateState()
    {
        if (gm.EM.Scenario.GetCurrentPlayer().CurrentSquare.IsThreatened(gm.EM.Scenario.GetCurrentPlayer()))
        {
            GetComponent<Image>().sprite = fuite;
            isAllowedToMove = false;
        }
        else
        {
            isAllowedToMove = true;
            GetComponent<Image>().sprite = deplacement;
        }
    }

    public void EnableButtonAction()
    {
        if (isAllowedToMove)
        {
            gm.Move();
        }
        // TODO call flee function of GM
        else
        {
            gm.Flee();
        }
    }

    
}
