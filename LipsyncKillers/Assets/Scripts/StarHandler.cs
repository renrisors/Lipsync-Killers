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
        if (score >= 0 && score < 3200)
        {
            //Zero Stars
            text[0].SetActive(true);
        }

        else if (score >= 3200 && score < 5150)
        {
            //One Stars
            stars[0].SetActive(true);
            text[1].SetActive(true);
        }

        else if (score >= 5150 && score < 9450)
        {
            //Two Stars
            stars[0].SetActive(true);
            stars[1].SetActive(true);
            text[2].SetActive(true);
        }

        else if (score >= 9450 && score < 13750)
        {
            //Three Stars
            stars[0].SetActive(true);
            stars[1].SetActive(true);
            stars[2].SetActive(true);
            text[3].SetActive(true);
        }

        else if (score >= 13750 && score < 16250)
        {
            //Four Stars
            stars[0].SetActive(true);
            stars[1].SetActive(true);
            stars[2].SetActive(true);
            stars[3].SetActive(true);
            text[4].SetActive(true);
        }

        else if (score >= 16250)
        {
            //Five Stars
            stars[0].SetActive(true);
            stars[1].SetActive(true);
            stars[2].SetActive(true);
            stars[3].SetActive(true);
            stars[4].SetActive(true);
            text[5].SetActive(true);
        }
    }
}
