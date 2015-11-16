using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {

    private static SceneManager instance;

    private static AudioSource audioSource;

    public static SceneManager getInstance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<SceneManager>();
                //Tell unity not to destroy this object when loading a new scene!
                DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            //If I am the first instance, make me the Singleton
            instance = this;
            audioSource = GameObject.FindGameObjectWithTag("music_source").GetComponent<AudioSource>();
            DontDestroyOnLoad(this);
        }
        else
        {
            //If a Singleton already exists and you find
            //another reference in scene, destroy it!
            if (this != instance)
                Destroy(this.gameObject);
        }
    }

    public void HandleKeyEvent(string command)
    {
        Debug.Log(command);
        if (command == "Escape")
        {
            Stop();
            Application.LoadLevel("menu_scene");
        }
        else if(command == "Space")
        {
            if(audioSource.isPlaying)
            {
                audioSource.Pause();
            }
            else 
            {
                Play();
            }
        }
    }

    public void Play()
    {
        audioSource.Play();
    }

    public void Stop()
    {
        audioSource.Stop();
    }

    public void setClip(AudioClip songClip)
    {
        audioSource.clip = songClip;
    }
}
