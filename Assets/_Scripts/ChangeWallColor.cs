using UnityEngine;
using System.Collections;

public class ChangeWallColor : MonoBehaviour {

	private GameObject music_cubes;
	private Color current_wall_color;
	public float colorLerpMultiplier = 20;

	// Use this for initialization
	void Start () {
		music_cubes = GameObject.Find ("MusicCubeContainer");
		current_wall_color = determineNewWallColor (Color.red);

	}
	
	// Update is called once per frame
	void Update () {

		Color newWallColor = determineNewWallColor (current_wall_color);
		// Do color update
		foreach (Transform child in transform) {
			Renderer renderer = child.gameObject.GetComponent<Renderer>();
			Material material = renderer.material;
			material.SetColor ("_Color", newWallColor);
		}
	}

	Color determineNewWallColor(Color previousColor) {
		float[] spectrum_data = music_cubes.GetComponent<GenerateCubes>().getLatestSpectrumData ();

		// Get lowest spectrum data point
		if (spectrum_data.Length  > 0) {

			float bass = spectrum_data [0];
	
			Color newColor = Color.Lerp (Color.blue, Color.red, bass * 2500);
			Color ret = Color.Lerp (previousColor, newColor, Time.deltaTime * colorLerpMultiplier);

			return ret;
		} else {
			return Color.white;
		}
	}
}
