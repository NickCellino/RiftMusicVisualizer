using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour
{

    public int numberOfCubes;
    public int numBalls = 5;
    public float radius;
    public float radius2;
    public float radius3;
    public float radius4;
    public float radius5;

    public GameObject cubePrefab;
    public GameObject ballPrefab;
    public GameObject[] musicCubes;

    public float spectrumScaleMultiplier = 30.0f;
    public float mathLerpMultiplier = 20.0f;
    public Vector3 initialCubeSize = new Vector3(1, 1, 1);
    public Vector3 initialBallSize = new Vector3(1, 1, 1);
    public Vector3 r1 = new Vector3(1, 1, 1);
    public Vector3 r2 = new Vector3(1, -1, 1);
    public Vector3 r3 = new Vector3(-1, 1, -1);
    public Vector3 r4 = new Vector3(1, 2, -3);
    public float[] spectrum;

    public GameObject[] musicBalls;
    public Material[] ballMaterials;
    public float ballSpeed;

    float initial_lightBrightness;

    public Vector3 RotateAmount;
    public Vector3 RotateAmount2;

    

    void Awake()
    {
        SceneManager.getInstance.Play();
    }

    void OnGUI()
    {
        Event e = Event.current;

        if (e.type.Equals(EventType.KeyDown))
        {
            SceneManager.getInstance.HandleKeyEvent(e.keyCode.ToString());
        }
    }

    // Use this for initialization
    void Start()
    {
        GameObject lantern_fab = GameObject.FindGameObjectWithTag("lantern_fab");
        int b1 = numberOfCubes / 5;
        int b2 = (numberOfCubes / 5)*2;
        int b3 = (numberOfCubes / 5) * 3;
        int b4 = (numberOfCubes / 5) * 4;

        for (int i = 0; i < b1; i++)
        {
            // Generate bar
            float angle = i * Mathf.PI * 2 / b1;
            Vector3 pos = new Vector3(Mathf.Cos(angle), Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            GameObject lantern_cube = Instantiate(lantern_fab, pos, Quaternion.identity) as GameObject;
            GameObject lantern = lantern_cube.transform.Find("lantern_light").gameObject;
            lantern_cube.transform.parent = gameObject.transform;
            lantern_cube.transform.localScale = initialCubeSize;
            lantern_cube.GetComponent<MeshRenderer>().material = ballMaterials[0];
            Light lantern_light = lantern.GetComponent<Light>();
            lantern_light.color = ballMaterials[0].color;

            Debug.Log(lantern != null);
            Debug.Log(lantern_light);
        }
        for (int i = b1; i < b2; i++)
        {
            // Generate bar
            float angle = i * Mathf.PI * 2 / b1;
            Vector3 pos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), Mathf.Sin(angle)) * radius2;
            GameObject lantern_cube = Instantiate(lantern_fab, pos, Quaternion.identity) as GameObject;
            GameObject lantern = lantern_cube.transform.Find("lantern_light").gameObject;
            lantern_cube.transform.parent = gameObject.transform;
            lantern_cube.transform.localScale = initialCubeSize;
            lantern_cube.GetComponent<MeshRenderer>().material = ballMaterials[1];
            Light lantern_light = lantern.GetComponent<Light>();
            lantern_light.color = ballMaterials[1].color;

            Debug.Log(lantern != null);
            Debug.Log(lantern_light);
        }
        for (int i = b2; i < b3; i++)
        {
            // Generate bar
            float angle = i * Mathf.PI * 2 / b1;
            Vector3 pos = new Vector3(Mathf.Cos(angle*(float)1.4), Mathf.Sin(angle)*(float)1.4, Mathf.Sin(angle*2)) * radius3;
            GameObject lantern_cube = Instantiate(lantern_fab, pos, Quaternion.identity) as GameObject;
            GameObject lantern = lantern_cube.transform.Find("lantern_light").gameObject;
            lantern_cube.transform.parent = gameObject.transform;
            lantern_cube.transform.localScale = initialCubeSize;
            lantern_cube.GetComponent<MeshRenderer>().material = ballMaterials[2];
            Light lantern_light = lantern.GetComponent<Light>();
            lantern_light.color = ballMaterials[2].color;

            Debug.Log(lantern != null);
            Debug.Log(lantern_light);
        }
        for (int i = b3; i < b4; i++)
        {
            // Generate bar
            float angle = i * Mathf.PI * 2 / b1;
            Vector3 pos = new Vector3(Mathf.Cos(angle*(-2)), Mathf.Cos(angle) * (float)1.4, Mathf.Sin(angle+12)) * radius4;
            GameObject lantern_cube = Instantiate(lantern_fab, pos, Quaternion.identity) as GameObject;
            GameObject lantern = lantern_cube.transform.Find("lantern_light").gameObject;
            lantern_cube.transform.parent = gameObject.transform;
            lantern_cube.transform.localScale = initialCubeSize;
            lantern_cube.GetComponent<MeshRenderer>().material = ballMaterials[3];
            Light lantern_light = lantern.GetComponent<Light>();
            lantern_light.color = ballMaterials[3].color;

            Debug.Log(lantern != null);
            Debug.Log(lantern_light);
        }
        for (int i = b4; i < numberOfCubes; i++)
        {
            // Generate bar
            float angle = i * Mathf.PI * 2 / b1;
            Vector3 pos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle) * (float)1.8, Mathf.Sin(angle)) * radius5;
            GameObject lantern_cube = Instantiate(lantern_fab, pos, Quaternion.identity) as GameObject;
            GameObject lantern = lantern_cube.transform.Find("lantern_light").gameObject;
            lantern_cube.transform.parent = gameObject.transform;
            lantern_cube.transform.localScale = initialCubeSize;
            lantern_cube.GetComponent<MeshRenderer>().material = ballMaterials[4];
            Light lantern_light = lantern.GetComponent<Light>();
            lantern_light.color = ballMaterials[4].color;

            Debug.Log(lantern != null);
            Debug.Log(lantern_light);
        }
        musicCubes = GameObject.FindGameObjectsWithTag("lantern_fab");

    }


    // Update is called once per frame
    void Update()
    {
        int b1 = numberOfCubes / 5;
    int b2 = (numberOfCubes / 5) * 2;
    int b3 = (numberOfCubes / 5) * 3;
    int b4 = (numberOfCubes / 5) * 4;
        spectrum = AudioListener.GetSpectrumData(4096, 0, FFTWindow.Hamming);
        for (int i = 1; i < numberOfCubes + 1; i++)
        {
            Vector3 previousScale = musicCubes[i].transform.localScale;

            previousScale.y = Mathf.Lerp(previousScale.y, spectrum[i] * spectrumScaleMultiplier, Time.deltaTime * mathLerpMultiplier);
            previousScale.x = Mathf.Lerp(previousScale.x, spectrum[i] * spectrumScaleMultiplier, Time.deltaTime * mathLerpMultiplier);
            previousScale.z = Mathf.Lerp(previousScale.z, spectrum[i] * spectrumScaleMultiplier, Time.deltaTime * mathLerpMultiplier);
            musicCubes[i].transform.localScale = previousScale;

            GameObject lantern_cube = musicCubes[i];
            GameObject lantern = lantern_cube.transform.Find("lantern_light").gameObject;
            Light lantern_light = lantern.GetComponent<Light>();

            float new_intensity = Mathf.Lerp(lantern_light.intensity, previousScale.y, Time.deltaTime * mathLerpMultiplier);
            lantern_light.intensity = new_intensity;
            if (i < b1) { transform.Rotate(RotateAmount * Time.deltaTime); }
            else if (i < b2 && i >= b1) { transform.RotateAround(Vector3.zero, r1, 2 * (float)(-1.5) * Time.deltaTime); }
            else if (i < b3 && i >= b2) { transform.RotateAround(Vector3.zero, r2, 4 * (float)(-1.5) * Time.deltaTime); }
            else if (i < b4 && i >= b3) { transform.RotateAround(Vector3.zero, r3, 3 * (float)(-1.5) * Time.deltaTime); }
            else { transform.RotateAround(Vector3.zero, r4, 2 * (float)(-1.5) * Time.deltaTime); }
            musicCubes[i].transform.Rotate(RotateAmount2 * Time.deltaTime);

            //Debug.Log(lantern_light.intensity);
        }
    }

    public float[] getLatestSpectrumData()
    {
        return spectrum;
    }
}

