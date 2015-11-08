using UnityEngine;
using System.Collections;
using NAudio;
using NAudio.Wave;

public class UIManagerScript : MonoBehaviour {

    protected FileBrowser fileBrowser;

    [SerializeField]
    protected Texture2D m_directoryImage, m_fileImage;

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
        
        //path = "file://" + Application.dataPath + "/_Audio/06_Macklemore_Ryan_Lewis_-_Irish_Celebration.wav";
     
        if (path != null)
        {
            path = "file://" + path;
            Debug.Log(path);
            if(path.Contains(".mp3"))
            {
                using (Mp3FileReader reader = new Mp3FileReader(path))
                {
                    Debug.Log("Reached");
                    path.Replace(".mp3", ".wav");
                    WaveFileWriter.CreateWaveFile(path , reader);
                }
            }
            Debug.Log(path);
            WWW wtf = new WWW(path);

            //Wait for wtf to finish
            while (!wtf.isDone)
            {
            }
            AudioSourceSingleton.getInstance.setClip(wtf.GetAudioClip(false));
        }
    }

    public void ChooseGenre()
    {
        Debug.Log("Put selection of genre here");
    }
}
