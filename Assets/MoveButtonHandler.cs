using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MoveButtonHandler : MonoBehaviour
{
    public Sprite deplacement;
    public Sprite deplacement_h;
    public Sprite fuite;
    public Sprite fuite_h;
    public Button btnStates;

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
            SpriteState st = new SpriteState();
            st.highlightedSprite = fuite_h;
            st.pressedSprite = fuite_h;
            btnStates.spriteState = st;
            isAllowedToMove = false;
        }
        else
        {
            isAllowedToMove = true;
            GetComponent<Image>().sprite = deplacement;
            SpriteState st = new SpriteState();
            st.highlightedSprite = deplacement_h;
            st.pressedSprite = deplacement_h;
            btnStates.spriteState = st;
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
