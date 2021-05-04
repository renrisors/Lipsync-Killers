using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Actions : MonoBehaviour
{
    public float energyCharge = 0.0f;
    public bool canSpecial;
    public Animator animator;
    public Animator frenesiAnim;

    public Joystick joystick;
    public float moveSpeed;

    public GameObject handle;
    public GameObject energyBar;
    public GameObject L3;

    Sprite handleDesactivated;
    Sprite handleActivated;
    Sprite energyBarDesactivated;
    Sprite energyBarActivated;

    private GameObject boundariLeft;
    private GameObject boundariRight;
    private GameObject FrenesiSFX;
    private GameObject SpecialSFX;
    private GameObject ChargedSFX;

    [HideInInspector]
    public bool frenesi = false;
    int score;
    [HideInInspector]
    public bool idle;

    private void Start()
    {
        handleDesactivated = Resources.Load<Sprite>("BOTAO_DESATIVADO");
        handleActivated = Resources.Load<Sprite>("BOTAO_ATIVADO");
        energyBarDesactivated = Resources.Load<Sprite>("BARRA ENERGIA_DESATIVADA");
        energyBarActivated = Resources.Load<Sprite>("BOTAO_BARRA_CHEIA_ATIVADA");

        handle.GetComponent<Image>().sprite = handleDesactivated;
        energyBar.GetComponent<Image>().sprite = energyBarDesactivated;
        //transform.position = SpawnPoint.transform.position;

        L3.SetActive(false);
        frenesiAnim.GetComponent<Animator>().SetBool("Frenesi", false);
        frenesiAnim.gameObject.SetActive(false);

        FrenesiSFX = GameObject.Find("FrenesiSFX");
        SpecialSFX = GameObject.Find("SpecialSFX");
        ChargedSFX = GameObject.Find("ChargedSFX");

        boundariLeft = GameObject.Find("Boudary Left");
        boundariRight = GameObject.Find("Boudary Right");
    }

    void FixedUpdate()
    {
        transform.position = transform.position + new Vector3(joystick.Horizontal * moveSpeed * Time.deltaTime, 0, 0);
        animator.SetFloat("Walking", joystick.Horizontal);
        animator.SetFloat("Up", joystick.Vertical);;

        if (transform.position.x < boundariLeft.transform.position.x)
        {
            this.transform.position = new Vector3(boundariLeft.transform.position.x, this.transform.position.y, 0);
        }
        else if (transform.position.x > boundariRight.transform.position.x)
        {
            this.transform.position = new Vector3(boundariRight.transform.position.x, this.transform.position.y, 0);
        }

        if (joystick.Horizontal < 0.2 && joystick.Horizontal > -0.2)
            idle = true;
        else
            idle = false;

        // Check if special is ready and change de joystick layout;
        if (energyCharge == 30)
        {
            handle.GetComponent<Image>().sprite = handleActivated;
            energyBar.GetComponent<Image>().sprite = energyBarActivated;
            L3.SetActive(true);
            ChargedSFX.GetComponent<AudioSource>().Play();

        }
        else if(energyCharge == 0)
        {
            handle.GetComponent<Image>().sprite = handleDesactivated;
            energyBar.GetComponent<Image>().sprite = energyBarDesactivated;
            L3.SetActive(false);
        }
    }


    public void ChargeSpecial()
    {
        energyCharge++;
    }

    public void Special()
    {
        frenesi = true;
        
        animator.SetTrigger("Special");
        energyCharge = 0;
        SpecialSFX.GetComponent<AudioSource>().Play();
    }

    public void SetScore(int addscore)
    {
        score = addscore;
    }

    public int GetScore()
    {
        return score;
    }

    public int Frenesi(int addscore)
    {
        int rand = UnityEngine.Random.Range(0, 100);
        if ((rand >= 1 && rand <= 7) && frenesi == true)
        {
            /* Durante 5 segundos o especial é repetido e nesse tempo pontuação no trilho 
                é aumentada em 3x. Tap = 150, Hold 225 e Slide 105 */
            animator.SetTrigger("Frenesi");

            frenesiAnim.gameObject.SetActive(true);
            frenesiAnim.SetBool("Frenesi", true);

            FrenesiSFX.GetComponent<AudioSource>().Play();

            StartCoroutine(CountdownFrenesi());


            return addscore *= 3;
        }
        frenesi = false;

        return addscore;
    }

    IEnumerator CountdownFrenesi()
    {
        Debug.Log("entrou");
        yield return new WaitForSeconds(5);
        Debug.Log("5 segundos depois");
        energyCharge = 0;
        animator.SetTrigger("Frenesi");
        frenesiAnim.gameObject.SetActive(false);
        frenesiAnim.SetBool("Frenesi", false);
        frenesi = false;
    }
}
