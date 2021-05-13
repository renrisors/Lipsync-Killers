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

    //booleanas usadas pela Behavior Tree do oponente
    [HideInInspector] public bool idle;
    [HideInInspector] public bool special = false;
    [HideInInspector] public bool frenesiHappening = false;
    [HideInInspector] public int comboAtual;



    private void Start()
    {
        
        if(this.transform.gameObject.name == "Character Liquor")
        {
            handleActivated = Resources.Load<Sprite>("BOTAO_ATIVADO_LIQUOR");
            handleDesactivated = Resources.Load<Sprite>("BOTAO_DESATIVADO_LIQUOR-12");
        }

        else if (this.transform.gameObject.name == "Character Nonami")
        {
            handleActivated = Resources.Load<Sprite>("BOTAO_ATIVADO_NONAMI");
            handleDesactivated = Resources.Load<Sprite>("BOTAO_DESATIVADO_NONAMI-12");
        }
            
        else
        {
            Debug.LogError("Nome não encontrado. Botão Default colcoado");
            handleActivated = Resources.Load<Sprite>("BOTAO_ATIVADO");
            handleDesactivated = Resources.Load<Sprite>("BOTAO_DESATIVADO");
        }
        
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
        animator.SetFloat("Up", joystick.Vertical);

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
        switch (energyCharge)
        {
            case 0:
                handle.GetComponent<Image>().sprite = handleDesactivated;
                energyBar.GetComponent<Image>().sprite = energyBarDesactivated;
                L3.SetActive(false);
                break;
            
            case 1:
                energyBar.GetComponent<Image>().sprite = Resources.Load<Sprite>("BARRA (1)");
                break;

            case 5:
                energyBar.GetComponent<Image>().sprite = Resources.Load<Sprite>("BARRA (2)");
                break;

            case 10:
                energyBar.GetComponent<Image>().sprite = Resources.Load<Sprite>("BARRA (3)");
                break;

            case 15:
                energyBar.GetComponent<Image>().sprite = Resources.Load<Sprite>("BARRA (4)");
                break;

            case 20:
                energyBar.GetComponent<Image>().sprite = Resources.Load<Sprite>("BARRA (5)");
                break;

            case 30:
                energyBar.GetComponent<Image>().sprite = Resources.Load<Sprite>("BARRA (6)");
                //handle.GetComponent<Image>().sprite = handleActivated;
                L3.SetActive(true);
                ChargedSFX.GetComponent<AudioSource>().Play();
                break;

            default:
                break;
        }
    }


    public void ChargeSpecial()
    {
        energyCharge++;
    }

    public void Special()
    {
        frenesi = true;
        special = true;


        animator.SetTrigger("Special");
        energyCharge = 0;
        SpecialSFX.GetComponent<AudioSource>().Play();
        special = false;
    }

    public void SetScore(int addscore)
    {
        score = addscore;
    }

    public int GetScore()
    {
        return score;
    }

    public void GetCombo(int combo)
    {
        comboAtual = combo;
    }



    public int Frenesi(int addscore)
    {
        int rand = UnityEngine.Random.Range(0, 100);
        if ((rand >= 1 && rand <= 12) && (frenesi == true || frenesiHappening == true))
        {
            frenesiHappening = true;
            /* Durante 5 segundos o especial é repetido e nesse tempo pontuação no trilho 
                é aumentada em 3x. Tap = 150, Hold 225 e Slide 105 */
            animator.SetBool("Frenesi", true);

            frenesiAnim.gameObject.SetActive(true);
            //frenesiAnim.SetBool("Frenesi", true);

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
        yield return new WaitForSeconds(8);
        Debug.Log("5 segundos depois");
        energyCharge = 0;
        animator.SetBool("Frenesi", false);
        frenesiAnim.gameObject.SetActive(false);
        frenesiAnim.SetBool("Frenesi", false);
        frenesi = false;
        frenesiHappening = false;
    }
}
