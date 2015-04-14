using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ObjectifsCountManager : MonoBehaviour
{

    public Text metalCount;
    public Text foodCount;
    public Text glassCount;
    public Text plasticCount;

    public void UpdateObjectifCounts(int nbr_food, int nbr_metal, int nbr_glass, int nbr_plastic)
    {
        foodCount.text = nbr_food.ToString();
        metalCount.text = nbr_metal.ToString();
        glassCount.text = nbr_glass.ToString();
        plasticCount.text = nbr_plastic.ToString();
    }

}
