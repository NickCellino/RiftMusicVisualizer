using UnityEngine;
using System.Collections;

public class GenerateCubes : MonoBehaviour {

	public int numberOfCubes;
	public int numBalls = 5;
	public float radius;
	public GameObject cubePrefab;
	public GameObject ballPrefab;
	public GameObject[] musicCubes;

	public float spectrumScaleMultiplier = 30.0f;
	public float mathLerpMultiplier = 20.0f;
	public Vector3 initialCubeSize = new Vector3(1,1,1);
	public Vector3 initialBallSize = new Vector3(1,1,1);
	public float[] spectrum;

	public GameObject[] musicBalls;
	public Material[] ballMaterials;
	public float ballSpeed;

    void Awake()
    {
        AudioSourceSingleton.getInstance.Play();
    }

	// Use this for initialization
	void Start () {
        for (int i = 0; i < numberOfCubes; i++) {
			// Generate bar
			float angle = i * Mathf.PI * 2 / numberOfCubes;
			Vector3 pos = new Vector3(Mathf.Cos (angle), 0 , Mathf.Sin (angle)) * radius;
			GameObject newCube = Instantiate (cubePrefab, pos, Quaternion.identity) as GameObject;
			newCube.transform.parent = gameObject.transform;
			newCube.transform.localScale = initialCubeSize;
			newCube.GetComponent<MeshRenderer>().material = ballMaterials[i%(ballMaterials.Length)];
		}
		for (int i = 0; i < numBalls; i++) {
			// Generate ball
			float xCoord = Random.Range(-10f, 10.0f);
			float yCoord = Random.Range(-10f, 10.0f);
			float zCoord = Random.Range(-10f, 10.0f);
			Vector3 ballPos = new Vector3(xCoord, yCoord, zCoord);
			GameObject newBall = Instantiate (ballPrefab, ballPos, Quaternion.identity) as GameObject;
			newBall.transform.localScale = initialBallSize;
			newBall.transform.position = ballPos;
			newBall.GetComponent<MeshRenderer>().material = ballMaterials[i%(ballMaterials.Length)];
		}
		musicCubes = GameObject.FindGameObjectsWithTag ("music_cube");
		musicBalls = GameObject.FindGameObjectsWithTag ("music_ball");
    }

	void FixedUpdate() {
		for (int i = 0; i < musicBalls.Length; i++) {
			// Apply a random force once in a while because why not
			if (Random.Range (0, 60) == 0) {
				Rigidbody rb = musicBalls[i].GetComponent<Rigidbody>();
				Vector3 movement = new Vector3 (Random.Range (-1.0f, 1.0f), Random.Range (-1.0f, 1.0f), Random.Range (-1.0f, 1.0f));
				rb.AddForce(movement * ballSpeed);
			}

		}
	}
	
	// Update is called once per frame
	void Update () {
		spectrum = AudioListener.GetSpectrumData (1024, 0, FFTWindow.Hamming);
		for (int i = 0; i < numberOfCubes; i++) {
			Vector3 previousScale = musicCubes [i].transform.localScale;
			previousScale.y = Mathf.Lerp (previousScale.y, spectrum[i] * spectrumScaleMultiplier, Time.deltaTime * mathLerpMultiplier);
			musicCubes [i].transform.localScale = previousScale;
		}
	}

	public float[] getLatestSpectrumData() {
		return spectrum;
	}
}
