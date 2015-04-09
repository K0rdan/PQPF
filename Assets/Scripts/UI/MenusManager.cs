using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenusManager : MonoBehaviour
{
    public Image background_img;
    public GameObject acceuil_btn;
    public GameObject main_menu;
    public GameObject data_PlayScreen;
    public Sprite[] bgSprites;


    int currentScreen;
    bool allowedChange;

    void Awake()
    {
        currentScreen = 0;
        allowedChange = false;
    }

    void Start()
    {
        EnableCurrentScreen();
    }

    public void InputScreenChange()
    {
        currentScreen++;
        EnableCurrentScreen();
    }

    void EnableCurrentScreen()
    {
        switch (currentScreen)
        {
            case 0:
                acceuil_btn.SetActive(true);
                main_menu.SetActive(false);
                data_PlayScreen.SetActive(false);
                background_img.sprite = bgSprites[currentScreen];
                break;
            case 1:
                background_img.sprite = bgSprites[currentScreen];
                acceuil_btn.SetActive(false);
                main_menu.SetActive(true);
                data_PlayScreen.SetActive(false);
                break;
            case 2:
                background_img.sprite = bgSprites[currentScreen];
                acceuil_btn.SetActive(false);
                main_menu.SetActive(false);
                data_PlayScreen.SetActive(true);
                break;
            default:
                Debug.LogError("Wrong screen index");
                break;
        }

    }

    public void ChangeScene(string scn)
    {
        if (allowedChange)
            Application.LoadLevel(scn);
    }

    public void ScenarioChosen()
    {
        allowedChange = true;
    }
}
