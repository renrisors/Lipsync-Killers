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
        if (score >= 0 && score < 9450)
        {
            //Zero Stars
            text[0].SetActive(true);
        }

        else if (score >= 9450 && score < 13250)
        {
            //One Stars
            stars[0].SetActive(true);
            text[1].SetActive(true);
        }

        else if (score >= 13250 && score < 18550)
        {
            //Two Stars
            stars[0].SetActive(true);
            stars[1].SetActive(true);
            text[2].SetActive(true);
        }

        else if (score >= 18550 && score < 24350)
        {
            //Three Stars
            stars[0].SetActive(true);
            stars[1].SetActive(true);
            stars[2].SetActive(true);
            text[3].SetActive(true);
        }

        else if (score >= 24350 && score < 29750)
        {
            //Four Stars
            stars[0].SetActive(true);
            stars[1].SetActive(true);
            stars[2].SetActive(true);
            stars[3].SetActive(true);
            text[4].SetActive(true);
        }

        else if (score >= 29750)
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
