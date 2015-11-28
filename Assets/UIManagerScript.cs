using UnityEngine;
using System.Collections;
using NAudio;
using NAudio.Wave;
using UnityEngine.UI;

public class UIManagerScript : MonoBehaviour {

    protected FileBrowser fileBrowser;
    protected bool popup;
    private GameObject sceneSelection;
    private GameObject scrollbar;
    private GameObject sceneText;

    public string[] sceneList;
    public GameObject sceneButtonPrefab;

    void Awake()
    {
        sceneSelection = GameObject.FindGameObjectWithTag("Scene_Selection");
        scrollbar = GameObject.FindGameObjectWithTag("Scrollbar");
        sceneText = GameObject.FindGameObjectWithTag("Scene_Text");
        GameObject scrollList = GameObject.FindGameObjectWithTag("Scroll_List");

        foreach (string s in sceneList)
        {
            GameObject sceneButton = (GameObject)Instantiate(sceneButtonPrefab);
            sceneButton.transform.SetParent(scrollList.transform, false);

            string copy = s;

            sceneButton.GetComponentInChildren<Text>().text = copy;

            
            sceneButton.GetComponent<Button>().onClick.AddListener(() => ChangeScene(copy));
        }

        SetSceneSelection(false);
    }

    public void StartVisualizer()
    {
        Application.LoadLevel(SceneManager.getInstance.getScene());
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

    void ChangeScene(string scene)
    {
        SceneManager.getInstance.setScene(scene);
        SetSceneSelection(false);
    }

    public void SetSceneSelection(bool state)
    {
        sceneSelection.SetActive(state);
        scrollbar.SetActive(state);
        sceneText.SetActive(state);
    }
}
