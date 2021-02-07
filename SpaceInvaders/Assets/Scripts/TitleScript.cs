using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScript : MonoBehaviour
{
	private GUIStyle buttonStyle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGUI()
    {
    	GUILayout.BeginArea(new Rect(10, Screen.height / 2 + 100, Screen.width - 10, 200));

        // Load the main scene
        // The scene needs to be added into build setting to be loaded!

        if (GUILayout.Button("New Game"))
        {
           SceneManager.LoadScene("GameScene");
        }
        if (GUILayout.Button("High Score"))
        {
            Debug.Log("This is not implemented yet!");
        }
        if (GUILayout.Button("Exit"))
        {
            Application.Quit();
        }

        GUILayout.EndArea();
    }
}
