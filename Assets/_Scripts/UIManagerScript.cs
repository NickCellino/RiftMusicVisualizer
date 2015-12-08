using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using NAudio;
using NAudio.Wave;

public class UIManagerScript : MonoBehaviour {

    protected FileBrowser fileBrowser;
	public GUISkin guiSkin;

	protected bool popup;
	private GameObject sceneSelection;
	private GameObject scrollbar;
	private GameObject sceneText;

	public string[] sceneList;
	public GameObject sceneButtonPrefab;

    [SerializeField]
    protected Texture2D m_directoryImage, m_fileImage;

    protected void Awake()
    {
        GameObject sourceObject = GameObject.FindGameObjectWithTag("music_source");
        AudioSource audio = sourceObject.GetComponent<AudioSource>();
        DontDestroyOnLoad(audio);
		InputField file_input = GameObject.FindGameObjectWithTag ("file_path").GetComponent<InputField> ();
		//file_input.Select ();
		//file_input.ActivateInputField ();

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
        //fileBrowser = new FileBrowser(the_canvas.pixelRect, "Choose Song", FileSelected);
		// fileBrowser = new FileBrowser(new Rect(0, 0, 1024, 1024), "Choose Song", FileSelected);
        //fileBrowser.SelectionPattern = "*.mp3";
		//FileSelected (@"D:\Media\Music\Lil Dicky\07_Molly_feat_Brendon_Urie_of_Panic_at_the_Disco.mp3");
		string file_path = GameObject.FindGameObjectWithTag ("file_path").GetComponent<InputField> ().text;
		FileSelected (@file_path);
    }

    //Called for every GUI event. The instance variable fileBrowser will only not be null if ChooseSong was just selected
    protected void OnGUI()
    {	
		GUI.skin = guiSkin;
        if (fileBrowser != null)
        {
		//	RenderTexture temp = RenderTexture.active;
		//	RenderTexture.active = GameObject.FindGameObjectWithTag ("FileBrowPlane").GetComponent<RenderTexture>();
            fileBrowser.OnGUI();
		//	RenderTexture.active = temp;
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


	public string translate_to_scene_name(string display_name) {
	switch (display_name) {
		case "Colorful":
			return "main_scene";
		case "Strobe":
			return "ethan_scene";
		case "Swirly":
			return "main_scene_dup";
		default:
			return "";
		}
	}

	void ChangeScene(string scene)
	{
		string scene_name = translate_to_scene_name (scene);
		SceneManager.getInstance.setScene(scene_name);
		SetSceneSelection(false);
	}
	
	public void SetSceneSelection(bool state)
	{
		sceneSelection.SetActive(state);
		//scrollbar.SetActive(state);
		sceneText.SetActive(state);
	}
}
