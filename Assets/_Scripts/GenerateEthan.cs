using UnityEngine;
using System.Collections;

public class GenerateEthan : MonoBehaviour
{

    public int numberOfCubes = 400;
    public int numBalls = 5;
    public int numTs = 1;
    public float radius;
    public GameObject cubePrefab;
    public GameObject cubebPrefab;
    public GameObject ballPrefab;
    public GameObject tPrefab;
    public GameObject[] musicCubes;
    public float gridX = 30f;
    public float gridY = 30f;
    public float spacing = 10f;

    public float spectrumScaleMultiplier = 30.0f;
    public float mathLerpMultiplier = 20.0f;
    public float spectrumScaleMultiplier2 = 100.0f;
    public float mathLerpMultiplier2 = 100.0f;
    public Vector3 initialCubeSize = new Vector3(100, 100, 100);
    public Vector3 initialBallSize = new Vector3(1, 1, 1);
    public Vector3 initialTSize = new Vector3(1, 1, 1);
    public float[] spectrum;
    public GameObject bulb;
    public Light lightComp;

    public GameObject[] musicBalls;
    public Material BlackBallMaterial;
    public GameObject[] musicTs;
    public float ballSpeed;

    void Awake()
    {
        SceneManager.getInstance.Play();
    }

	void OnGUI()
	{
		Event e = Event.current;
		
		if(e.type.Equals(EventType.KeyDown))
		{
			SceneManager.getInstance.HandleKeyEvent(e.keyCode.ToString());
		}
	}

    // Use this for initialization
    void Start()
    {
        for (int y = -0; y < gridY; y++)
        {
            for (int x = 0; x < gridX; x++)
            {
                Vector3 pos = new Vector3((x - 10) * spacing, -8 - ((y + x) / (float)2.5), (y - 10) * spacing);
                GameObject newCube = Instantiate(cubePrefab, pos, Quaternion.identity) as GameObject;
                newCube.GetComponent<MeshRenderer>().material = BlackBallMaterial;
                newCube.GetComponent<MeshRenderer>().material.color = Color.black;
            }
        }
        for (int y = -0; y < gridY; y++)
        {
            for (int x = 0; x < gridX; x++)
            {
                Vector3 pos = new Vector3((x - 10) * spacing, 8 - ((y + x) / (float)2.5), (y - 10) * spacing);
                GameObject newCube = Instantiate(cubePrefab, pos, Quaternion.identity) as GameObject;
                newCube.GetComponent<MeshRenderer>().material = BlackBallMaterial;
                newCube.GetComponent<MeshRenderer>().material.color = Color.black;
            }
        }

        musicCubes = GameObject.FindGameObjectsWithTag("music_cube");
        musicBalls = GameObject.FindGameObjectsWithTag("music_ball");
        musicTs = GameObject.FindGameObjectsWithTag("music_cube");
    }

    void FixedUpdate()
    {
        for (int i = 0; i < musicBalls.Length; i++)
        {
            // Apply a random force once in a while because why not
            if (Random.Range(0, 60) == 0)
            {
                Rigidbody rb = musicBalls[i].GetComponent<Rigidbody>();
                Vector3 movement = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
                rb.AddForce(movement * ballSpeed);
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        spectrum = AudioListener.GetSpectrumData(4096, 0, FFTWindow.Hamming);


        for (int i = 200; i < 400; i++)
        {
            Vector3 previousScale = musicCubes[i].transform.localScale;
            previousScale.y = Mathf.Lerp(previousScale.y, spectrum[i - 200] * spectrumScaleMultiplier, Time.deltaTime * mathLerpMultiplier);
            musicCubes[i].transform.localScale = previousScale;
            if (i == 200)
            {
                GameObject dir = GameObject.Find("strobe");
                float new_intensity = Mathf.Lerp(dir.GetComponent<Light>().intensity / 4, previousScale.y, Time.deltaTime * mathLerpMultiplier2);
                dir.GetComponent<Light>().intensity = new_intensity * 4;
            }
            if (i == 205)
            {
                GameObject dir2 = GameObject.Find("strobe2");
                float new_intensity = Mathf.Lerp(dir2.GetComponent<Light>().intensity / 4, previousScale.y, Time.deltaTime * mathLerpMultiplier2);
                dir2.GetComponent<Light>().intensity = new_intensity * 4;
            }
            if (i == 208)
            {
                GameObject dir3 = GameObject.Find("strobe3");
                float new_intensity = Mathf.Lerp(dir3.GetComponent<Light>().intensity / 2, previousScale.y, Time.deltaTime * mathLerpMultiplier2);
                dir3.GetComponent<Light>().intensity = new_intensity * 2;
            }
        }
        for (int i = 199; i >= 0; i--)
        {
            Vector3 previousScale = musicCubes[i].transform.localScale;
            previousScale.y = Mathf.Lerp(previousScale.y, spectrum[199 - i] * spectrumScaleMultiplier, Time.deltaTime * mathLerpMultiplier);
            musicCubes[i].transform.localScale = previousScale;
        }
        for (int i = 600; i < 800; i++)
        {
            Vector3 previousScale = musicCubes[i].transform.localScale;
            previousScale.y = Mathf.Lerp(previousScale.y, spectrum[i - 600] * spectrumScaleMultiplier, Time.deltaTime * mathLerpMultiplier);
            musicCubes[i].transform.localScale = previousScale;

        }
        for (int i = 599; i >= 400; i--)
        {
            Vector3 previousScale = musicCubes[i].transform.localScale;
            previousScale.y = Mathf.Lerp(previousScale.y, spectrum[599 - i] * spectrumScaleMultiplier, Time.deltaTime * mathLerpMultiplier);
            musicCubes[i].transform.localScale = previousScale;
        }
    }

    public float[] getLatestSpectrumData()
    {
        return spectrum;
    }
}
