using UnityEngine;
using System.Collections;

public class GoToGameSceneController : MonoBehaviour {

    public void GoToScene()
    {
        Application.LoadLevel("InGame");
    }
}
