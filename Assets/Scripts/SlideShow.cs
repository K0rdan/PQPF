using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SlideShow : MonoBehaviour {
	public string NextScene;
	public Slide[] Slides;

	protected Slide CurrentSlide;
	protected int CurrentSlideIndex;

	public Button NextButton;
	public Button PreviousButton;

	private int nbScreenshots = 0;

	// Use this for initialization
	void Start () {
		if (Slides.Length > 0)
		{
			CurrentSlide = Slides[0];
			CurrentSlideIndex = 0;

			PreviousButton.enabled = false;
			PreviousButton.gameObject.SetActive(false);

			Slides[0].Enable();
			for (int i = 1; i < Slides.Length; i++)
			{
				Slides[i].Disable();
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		if (Event.current.Equals (Event.KeyboardEvent ("return"))) {
			Debug.Log ("Screenshot!");
			Application.CaptureScreenshot ("RatBall - screenshot" + nbScreenshots.ToString () + ".png");
			nbScreenshots++;
		}
	}

	public void NextSlide(){
		// If last Slide
		if (CurrentSlideIndex == Slides.Length - 1) {
			// Next Scene
			Debug.Log ("Next Scene");
			Application.LoadLevel(NextScene);

			return;
		}

		if (CurrentSlideIndex < Slides.Length - 1) {
			Debug.Log ("Next");
			CurrentSlideIndex ++;
			
			CurrentSlide.Disable();
			CurrentSlide = Slides [CurrentSlideIndex];
			Debug.Log ("CurrentSlide : " + CurrentSlideIndex.ToString());
			CurrentSlide.Enable();

			if(CurrentSlideIndex > 0){
				PreviousButton.enabled = true;
				PreviousButton.gameObject.SetActive(true);
			}
		}
	}

	public void PreviousSlide(){
		if (CurrentSlideIndex > 0) {
			Debug.Log ("Previous");
			CurrentSlideIndex --;

			CurrentSlide.Disable();
			CurrentSlide = Slides [CurrentSlideIndex];
			Debug.Log ("CurrentSlide : " + CurrentSlideIndex.ToString());
			CurrentSlide.Enable();

			if(CurrentSlideIndex == 0){
				PreviousButton.enabled = false;
			}

		}
	}
}

[System.Serializable]
public class Slide
{
	public Text[] txts;
	public Image[] imgs;
	
	public Slide()
	{
		
	}

	public void Enable(){
		Debug.Log ("Enable");
		for(int i = 0; i < txts.Length; ++i){
			txts[i].enabled = true;
		}
		for(int i = 0; i < imgs.Length; ++i){
			imgs[i].enabled = true;
		}
	}

	public void Disable(){
		Debug.Log ("Disable");
		for(int i = 0; i < txts.Length; ++i){
			txts[i].enabled = false;
		}
		for(int i = 0; i < imgs.Length; ++i){
			imgs[i].enabled = false;
		}
	}
}
