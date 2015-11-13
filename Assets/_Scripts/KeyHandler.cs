using UnityEngine;
using System.Collections;

public class KeyHandler : MonoBehaviour {

	void OnGUI()
    {
        Event e = Event.current;
        if (e.keyCode.ToString() == "Escape")
        {
          AudioSourceSingleton.getInstance.Stop();
          Application.LoadLevel("menu_scene");
        }
    }        
}
