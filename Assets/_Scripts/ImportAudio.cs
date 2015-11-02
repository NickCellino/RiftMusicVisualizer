using UnityEngine;
using System.Collections;

public class ImportAudio : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		AudioSource audio = GetComponent<AudioSource> ();
		audio.Play ();

		string audio_path = "file://" + Application.dataPath + "/_Audio/06_Macklemore_Ryan_Lewis_-_Irish_Celebration.wav";
		Debug.Log (audio_path);
		WWW wtf = new WWW(audio_path);
		while (!wtf.isDone) {
			// Wait until download finishes
		}

		audio.clip = wtf.GetAudioClip (false);
		audio.Play ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
