using UnityEngine;
using System.Collections;

public class AudioSourceSingleton : MonoBehaviour {

    private static AudioSourceSingleton instance;

    private static AudioSource audioSource;

    public static AudioSourceSingleton getInstance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<AudioSourceSingleton>();
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
            //DontDestroyOnLoad(audioSource);
        }
        else
        {
            //If a Singleton already exists and you find
            //another reference in scene, destroy it!
            if (this != instance)
                Destroy(this.gameObject);
        }
    }

    public void play()
    {
        audioSource.Play();
    }

    public void setClip(AudioClip songClip)
    {
        audioSource.clip = songClip;
    }
}
