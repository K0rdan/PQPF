using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FouilleResultManager : MonoBehaviour {

    public Sprite[] sprites;
    public Image imgHolder;

    public void UpdateResult(string id)
    {
        switch (id)
        {
            case "Boustifaille":
                imgHolder.sprite = sprites[0];
                break;
            case "Ferraille":
                imgHolder.sprite = sprites[1];
                break;
            case "Verre":
                imgHolder.sprite = sprites[2];
                break;
            case "Plastique":
                imgHolder.sprite = sprites[3];
                break;
            default:
                Debug.LogError("Update Result of FouilleResultManager received UNKOWNK ID");
                break;
        }
    }
}
