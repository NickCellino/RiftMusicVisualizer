# RiftMusicVisualizer
A VR music visualizer for the Oculus Rift.

Notes for adding scene:

All visualization scenes should interface with the menu_scene using SceneManager.
For basic functionality, the audio should be started in the Awake() function of a 
script that is attached to an Asset that is created with the scene. Furthermore,
key handling should be delegated in the OnGUI ffunction of the same script. Look
in the script GenerateCubes for an example.
