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
        Application.LoadLevel("ethan_scene");
    } 

    //TODO: Change look of file browser
    public void ChooseSong()
    {
        fileBrowser = new FileBrowser(new Rect(100, 100, 500, 600), "Choose Song", FileSelected);
        //fileBrowser.SelectionPattern = "*.mp3";
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
     
        if (path != null)
        {
            Debug.Log(path);
            if(path.Contains(".mp3"))
            {
                Debug.Log(path);
                using (Mp3FileReader reader = new Mp3FileReader(path))
                {
                    Debug.Log("Reached");
                    path = path.Replace(".mp3", ".wav");
                    WaveFileWriter.CreateWaveFile(path , reader);
                }
            }
            path = "file://" + path;
            WWW wtf = new WWW(path);

            //Wait for wtf to finish
            while (!wtf.isDone)
            {
            }
            SceneManager.getInstance.setClip(wtf.GetAudioClip(false));
        }
    }

    public void ChooseGenre()
    {
        Debug.Log("Put selection of genre here");
    }
}
