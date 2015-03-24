using UnityEngine;
using System.Collections;

public class Defilement : MonoBehaviour {
    private Animator animator;

	// Use this for initialization
	void Start () {
        this.animator = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            this.animator.SetBool("Unpause", true);
        }
	}

    public bool IsDone()
    {
        return this.animator.GetBool("Unpause");
    }
}
