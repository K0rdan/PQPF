using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TunrIndicationManager : MonoBehaviour
{
    public Sprite[] spriteList;

    public void SetTurnValue(int val)
    {
        GetComponent<Image>().sprite = spriteList[val - 1];
    }
}
