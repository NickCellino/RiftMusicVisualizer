using UnityEngine;
using System.Collections;

public class GenerateCubes : MonoBehaviour {

	public int numberOfCubes;
	public float radius;
	public GameObject cubePrefab;
	public GameObject[] musicCubes;
	public float spectrumScaleMultiplier = 30.0f;
	public float mathLerpMultiplier = 20.0f;
	public Vector3 initialCubeSize = new Vector3(1,1,1);

	// Use this for initialization
	void Start () {
		for (int i = 0; i < numberOfCubes; i++) {
			float angle = i * Mathf.PI * 2 / numberOfCubes;
			Vector3 pos = new Vector3(Mathf.Cos (angle), 0 , Mathf.Sin (angle)) * radius;
			GameObject newCube = Instantiate (cubePrefab, pos, Quaternion.identity) as GameObject;
			newCube.transform.parent = gameObject.transform;
			newCube.transform.localScale = initialCubeSize;
		}
		musicCubes = GameObject.FindGameObjectsWithTag ("music_cube");

	}
	
	// Update is called once per frame
	void Update () {
		float[] spectrum = AudioListener.GetSpectrumData (1024, 0, FFTWindow.Hamming);
		for (int i = 0; i < numberOfCubes; i++) {
			Vector3 previousScale = musicCubes [i].transform.localScale;
			previousScale.y = Mathf.Lerp (previousScale.y, spectrum[i] * spectrumScaleMultiplier, Time.deltaTime * mathLerpMultiplier);
			musicCubes [i].transform.localScale = previousScale;
		}
	}
}
