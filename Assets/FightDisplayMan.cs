using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FightDisplayMan : MonoBehaviour
{

    public Text combatText;
    public Text ref_atq_vivacity;
    public Text ref_atq_astuce;
    public Text ref_cib_vivacity;
    public Text ref_cib_astuce;
    public GameObject imgHolder_atq;
    public GameObject imgHolder_cib;
    public GameObject nextButton;

    bool showFightResult;

    void Start()
    {
        //combatText.text = GenerateCombatText(5, 5, 4, 4, "ATK", true, "Tirette", "Le Chat", true);
        //combatText.text = GenerateCombatText(5, 3, 1, 0, "FLEE", false, "Le Chat", "Tirette");
        showFightResult = false;
    }

    public void GenerateCombatText(int astuce_atq, int astuce_cible, int val_de, int damage,
        string mode, bool result,
        string atq_name, string cible_name,
        bool death = false)
    {


        string txt = "";

        #region Attaque
        if (mode == "ATK")
        {
            if (atq_name != "Le Chat" && result)
            {
                txt += atq_name + " (Astuce <color=green>" + astuce_atq + "</color> ) obtient <color=blue>" + val_de + "</color> et inflige <color=red><b>" +
                damage + "</b></color> point(s) de blessures au Chat (Menace <color=green>" + astuce_cible + "</color> ) ! (" + astuce_atq + " + " + val_de + " - " +
                astuce_cible + " = " + damage + ")";
                if (death)
                    txt += "\nLe chat abandonne le combat et fuit la queue entre les pattes !";
            }
            if (atq_name != "Le Chat" && !result)
            {
                txt += atq_name + " (Astuce <color=green>" + astuce_atq + "</color> ) obtient <color=blue>" + val_de + "</color> et ne parvient pas à toucher " +
                  "Le Chat (Menace <color=green>" + astuce_cible + "</color> ) !\n(" + astuce_atq + " + " + val_de + " < " +
                  astuce_cible + " )";
            }
            if (atq_name == "Le Chat" && result)
            {
                txt += cible_name + " (Astuce <color=green>" + astuce_cible + "</color> ) obtient <color=blue>" + val_de + "</color> mais ne parvient pas à échapper aux griffes du chat" +
                " (Menace <color=green>" + astuce_atq + "</color> ):\n" + cible_name + " perd <color=red>" + damage + "</color> point(s) de vivacité ! (" + astuce_cible + " + " + val_de + " - " +
                astuce_atq + " = " + damage + ")";
                if (death)
                    txt += "\n" + cible_name + " s’éffondre en appelant à l’aide";
            }
            if (atq_name == "Le Chat" && !result)
            {
                txt += cible_name + " (Astuce <color=green>" + astuce_cible + "</color> ) obtient <color=blue>" + val_de + "</color> et parvient à éviter les morsures du chat" +
                " (Menace <color=green>" + astuce_atq + "</color> ) ! (" + astuce_cible + " + " + val_de + " > " +
                astuce_atq + " )";
            }
        } 
        #endregion

        #region Escape
        if (mode == "FLEE")
        {
            if (result)
            {
                txt += cible_name + " (Astuce <color=green>" + astuce_cible + "</color> ) obtient <color=blue>" + val_de + "</color> et réussit à échapper à la vigilance du Chat"+
                " (Menace <color=green>" + astuce_atq + "</color> ) !\n (" + astuce_cible + " + " + val_de + " > " + astuce_atq +" )";
            }
            else
            {
                txt += cible_name + " (Astuce <color=green>" + astuce_cible + "</color> ) obtient <color=blue>" + val_de + "</color> et attire l’attention du Chat" +
               " (Menace <color=green>" + astuce_atq + "</color> ) :\nla fuite est impossible ! (" + astuce_cible + " + " + val_de + " <= " + astuce_atq + " )";
            }
        }
        #endregion


        combatText.text = txt;
        nextButton.SetActive(true);
    }


    public void UpdateStatus(int val_atq_astuce, int val_atq_vivacity, int val_cib_astuce, int val_cib_vivacity, string id_atk, string id_cib)
    {
        if (!showFightResult)
        {
            nextButton.SetActive(false);
            combatText.text = "";
        }
        showFightResult = !showFightResult;
        
        ref_atq_astuce.text = val_atq_astuce.ToString();
        ref_atq_vivacity.text = val_atq_vivacity.ToString();
        ref_cib_astuce.text = val_cib_astuce.ToString();
        ref_cib_vivacity.text = val_cib_vivacity.ToString();

        imgHolder_atq.SendMessage("UpdateSate", id_atk); // ids are : volotom  -  niles  - souris  -  filou  -  cat 
        imgHolder_cib.SendMessage("UpdateSate", id_cib); // ids are : volotom  -  niles  - souris  -  filou  -  cat 
    }
}
