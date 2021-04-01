using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarHandler : MonoBehaviour
{
    public GameObject[] stars;
    public GameObject[] text;
    [HideInInspector] public int score;

    public void starsAchieved()
    {
        if (score >= 0 && score < 10000)
        {
            //Zero Stars
            text[0].SetActive(true);
            Debug.Log(score);
        }

        else if (score >= 10000 && score < 20000)
        {
            //One Stars
            stars[0].SetActive(true);
            text[1].SetActive(true);
            Debug.Log(score);
        }

        else if (score >= 20000 && score < 30000)
        {
            //Two Stars
            stars[0].SetActive(true);
            stars[1].SetActive(true);
            text[2].SetActive(true);
            Debug.Log(score);
        }

        else if (score >= 30000 && score < 40000)
        {
            //Three Stars
            stars[0].SetActive(true);
            stars[1].SetActive(true);
            stars[2].SetActive(true);
            text[3].SetActive(true);
        }

        else if (score >= 40000 && score < 50000)
        {
            //Four Stars
            stars[0].SetActive(true);
            stars[1].SetActive(true);
            stars[2].SetActive(true);
            stars[3].SetActive(true);
            text[4].SetActive(true);
        }

        else if (score >= 50000)
        {
            //Four Stars
            stars[0].SetActive(true);
            stars[1].SetActive(true);
            stars[2].SetActive(true);
            stars[3].SetActive(true);
            stars[4].SetActive(true);
            text[5].SetActive(true);
        }
    }
}
