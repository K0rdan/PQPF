using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JzzSoundManager : MonoBehaviour
{
	/* Channels are
	 * 0 : BG Music
	 * 1 : On screen Sound
	 * 2 : Narration
	 * 3 : FX
	*/
	public const int bgChannel = 0;
	public const int screenSound = 1;
	public const int narrationChannel = 2;
	public const int fxChannel = 3;

	public static JzzSoundManager instance = null;
	public AudioClip[] ClipList;
	AudioSource[] sources;
	float[] oldVolume;
	float[] newVolume;
	float[] transitionStart;
	float[] transitionTime;
	Transform cam;
	
	// Make sure all channel are stopped when changing levels (unless looped)
	void OnLevelWasLoaded(int level)
	{
		cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
		for (int i = 0; i < sources.Length; i++)
		{
			if (!sources[i].loop)
			{
				sources[i].Stop();
			}
		}
	}
	
	void Awake()
	{
		// so we can play music between scenes
		if (instance == null)
		{
			DontDestroyOnLoad(gameObject);
			instance = this;
		}
		else
		{
			if (instance != this)
				Destroy(gameObject);
		}
		
		// Audio Channels
		sources = new AudioSource[4];
		
		for (int i = 0; i < sources.Length; i++)
		{
			sources[i] = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
		}
		
		cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
		transform.position = cam.position;
		
		oldVolume = new float[sources.Length];
		newVolume = new float[sources.Length];
		transitionStart = new float[sources.Length];
		transitionTime = new float[sources.Length];
		
		for (int i = 0; i < sources.Length; i++)
		{
			oldVolume[i] = 1.0f;
			newVolume[i] = 1.0f;
			transitionStart[i] = 0.0f;
			transitionTime[i] = 0.00001f;
		}
	}
	
	void Update()
	{
		transform.position = cam.position;
		for (int i = 0; i < sources.Length; i++)
		{
			// interpolates for a smooth volume change
			sources[i].volume = Mathf.Lerp(oldVolume[i], newVolume[i], Mathf.Min(1.0f, (Time.time - transitionStart[i]) / transitionTime[i]));
			if (newVolume[i] <= 0 && sources[i].volume <= 0 && sources[i].isPlaying)
			{
				sources[i].Stop();
			}
		}
	}
	
	// Sets Volume of specified channel
	public void SetVolume(int channel, float newVol)
	{
		oldVolume[channel] = newVol;
		newVolume[channel] = newVol;
		sources[channel].volume = newVol;
	}
	
	// Sets Volume of specified channel over a certain time
	public void SetVolume(int channel, float newVol, float time)
	{
		oldVolume[channel] = sources[channel].volume;
		newVolume[channel] = newVol;
		transitionStart[channel] = Time.time;
		transitionTime[channel] = time;
	}
	
	// Plays an AudioClip with specified Index, on any free channel, returns channel number
	public int PlayClip(int clipIndex, bool loop)
	{
		Debug.Log(ClipList.Length);
		AudioClip thisClip = ClipList[clipIndex];
		if (thisClip != null)
		{
			for (int i = 0; i < sources.Length; i++)
			{
				AudioSource s = sources[i];
				if (!s.isPlaying)
				{
					s.clip = thisClip;
					s.loop = loop;
					s.Play();
					SetVolume(i, 1.0f);
					return i;
				}
			}
		}
		return -1;
	}
	
	// Plays an AudioClip with specified Index on specified channel, can be looped, returns channel number
	public int PlayClip(int clipIndex, int channel, bool loop)
	{
		AudioClip thisClip = ClipList[clipIndex];
		if (thisClip != null)
		{
			sources[channel].clip = thisClip;
			sources[channel].loop = loop;
			sources[channel].Play();
			SetVolume(channel, 1.0f);
		}
		return channel;
	}
	
	// Stops an AudioClip with specified Index on any channel
	public void StopClip(int clipIndex)
	{
		AudioClip thisClip = ClipList[clipIndex];
		if (thisClip != null)
		{
			foreach (AudioSource s in sources)
			{
				if (s.clip == thisClip && s.isPlaying)
				{
					s.Stop();
				}
			}
		}
	}
	
	// Stops the given channel
	public void StopChannel(int i)
	{
		sources[i].Stop();
	}
	
}
