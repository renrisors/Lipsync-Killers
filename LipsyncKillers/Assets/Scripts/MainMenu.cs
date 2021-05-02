using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private string scene = "scene";
    
    public void ChangeScene(string SceneName)
    {
        scene = SceneName;
        StartCoroutine(SceneDelay());
    }


    public void ExitGame()
    {
        StartCoroutine(SceneDelay());
        Application.Quit();
    }

    public IEnumerator SceneDelay()
    {
        yield return new WaitForSeconds(0.07f);
        SceneManager.LoadScene(scene);
    }
}
    
