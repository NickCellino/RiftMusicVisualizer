using UnityEngine;
using System.Collections;

public class UIManagerScript : MonoBehaviour {

    protected FileBrowser fileBrowser;

    protected void awake()
    {
        GameObject sourceObject = GameObject.FindGameObjectWithTag("music_source");
        AudioSource audio = sourceObject.GetComponent<AudioSource>();
        DontDestroyOnLoad(audio);
    }

    public void StartVisualizer()
    {
        Application.LoadLevel("main_scene");
    } 

    //TODO: Change look of file browser
    public void ChooseSong()
    {
        fileBrowser = new FileBrowser(new Rect(100, 100, 500, 600), "Choose Song", FileSelected);
    }

    //Called for every GUI event. The instance variable fileBrowser will only not be null if ChooseSong was just selected
    protected void OnGUI()
    {
        if (fileBrowser != null)
        {
            fileBrowser.OnGUI();
        }
    }

    protected void FileSelected(string path)
    {
        fileBrowser = null;
        path = "_Audio/Watsky - Nothing Like the First Time - 06 A Hundred Words You Could Say Instead of Swag.mp3";
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        
        string audio_path = "file://" + Application.dataPath + "/_Audio/06_Macklemore_Ryan_Lewis_-_Irish_Celebration.wav";
        Debug.Log(audio_path);
        WWW wtf = new WWW(audio_path);
        while (!wtf.isDone)
        {
            // Wait until download finishes
        }
        
        audio.clip = wtf.GetAudioClip(false);
        audio.Play();
        if (path != null)
        {
            AudioSourceSingleton.getInstance.setClip((AudioClip)Resources.Load(path));
        }
    }



}
