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
    public Animation frenesiAnim;

    public Joystick joystick;
    public float moveSpeed;

    public GameObject handle;
    public GameObject energyBar;
    public GameObject L3;

    Sprite handleDesactivated;
    Sprite handleActivated;
    Sprite energyBarDesactivated;
    Sprite energyBarActivated;

    public GameObject SpawnPoint;
    public GameObject boundariLeft;
    public GameObject boundariRight;

    bool frenesi = false;
    private float countdown = 5f;

    private void Start()
    {
        handleDesactivated = Resources.Load<Sprite>("BOTAO_DESATIVADO");
        handleActivated = Resources.Load<Sprite>("BOTAO_ATIVADO");
        energyBarDesactivated = Resources.Load<Sprite>("BARRA ENERGIA_DESATIVADA");
        energyBarActivated = Resources.Load<Sprite>("BOTAO_BARRA_CHEIA_ATIVADA");

        handle.GetComponent<Image>().sprite = handleDesactivated;
        energyBar.GetComponent<Image>().sprite = energyBarDesactivated;
        transform.position = SpawnPoint.transform.position;

        L3.SetActive(false);
        frenesiAnim.Stop();
    }

    void Update()
    {
        transform.position = transform.position + new Vector3(joystick.Horizontal * moveSpeed * Time.deltaTime, 0, 0);
        animator.SetFloat("Walking", joystick.Horizontal /*Mathf.Abs(joystick.Horizontal)*/);

        if (transform.position.x < boundariLeft.transform.position.x)
        {
            this.transform.position = new Vector3(boundariLeft.transform.position.x, this.transform.position.y, 0);
        }
        else if (transform.position.x > boundariRight.transform.position.x)
        {
            this.transform.position = new Vector3(boundariRight.transform.position.x, this.transform.position.y, 0);
        }
        

        // Check if special is ready and change de joystick layout;
        if (energyCharge == 30)
        {
            handle.GetComponent<Image>().sprite = handleActivated;
            energyBar.GetComponent<Image>().sprite = energyBarActivated;
            L3.SetActive(true);
            frenesi = true;
        }
        else if(energyCharge == 0)
        {
            handle.GetComponent<Image>().sprite = handleDesactivated;
            energyBar.GetComponent<Image>().sprite = energyBarDesactivated;
            L3.SetActive(false);
            frenesi = false;
        }

    }


    public void ChargeSpecial()
    {
        energyCharge++;
    }

    public void Special()
    {
        frenesi = true;
        animator.SetBool("Special", true);
        energyCharge = 0;
        
    }

    public int Frenesi(int addscore)
    {
        int rand = UnityEngine.Random.Range(0, 100);
        if((rand >=1 || rand <=7) && frenesi == true)
        {
            /* Durante 5 segundos o especial é repetido e nesse tempo pontuação no trilho 
                é aumentada em 3x. Tap = 150, Hold 225 e Slide 105 */
            Debug.Log(rand);
            frenesiAnim.Play();

            countdown -= Time.deltaTime;
            if(countdown == 0.0f)
            {
                energyCharge = 0;
                animator.SetBool("Special", false);
                countdown = 5f;
            }
            return addscore *= 3;
        }
        return addscore;
    }
}
