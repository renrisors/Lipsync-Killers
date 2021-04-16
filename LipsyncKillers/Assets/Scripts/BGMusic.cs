using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMusic : MonoBehaviour
{

    //Play Global
    private static BGMusic instance = null;
    private Scene scene;
    public static BGMusic Instance
    {
        get { return instance; }
    }

    void Awake()
    {

        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        scene = SceneManager.GetActiveScene();

        if (scene.name == "NewMainStage")
        {
            Destroy(this.gameObject);
            return;
        }
    }
}