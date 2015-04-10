using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BGOfTextScenario : MonoBehaviour
{

    public Sprite character_bg;
    public Sprite narration_bg;

    public void SetBgForScenario(string val)
    {
        if (val == "Character")
        {
            GetComponent<Image>().sprite = character_bg;
        }
        if (val == "Narration")
        {
            GetComponent<Image>().sprite = narration_bg;
        }
    }
}
