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

    public Joystick joystick;
    public float moveSpeed;

    public GameObject handle;
    public GameObject energyBar;

    Sprite handleDesactivated;
    Sprite handleActivated;
    Sprite energyBarDesactivated;
    Sprite energyBarActivated;

    public GameObject SpawnPoint;
    public GameObject boundariLeft;
    public GameObject boundariRight;

    private void Start()
    {
        handleDesactivated = Resources.Load<Sprite>("BOTAO_DESATIVADO");
        handleActivated = Resources.Load<Sprite>("BOTAO_ATIVADO");
        energyBarDesactivated = Resources.Load<Sprite>("BARRA ENERGIA_DESATIVADA");
        energyBarActivated = Resources.Load<Sprite>("BOTAO_BARRA_CHEIA_ATIVADA");

        handle.GetComponent<Image>().sprite = handleDesactivated;
        energyBar.GetComponent<Image>().sprite = energyBarDesactivated;
        transform.position = SpawnPoint.transform.position;
    }

    void Update()
    {
        transform.position = transform.position + new Vector3(joystick.Horizontal * moveSpeed * Time.deltaTime, 0, 0);
        animator.SetFloat("Walking", Mathf.Abs(joystick.Horizontal));

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
            //animator.SetBool("Special", true);
        }

    }


    public void chargeSpecial()
    {
        energyCharge++;
    }

    public void AddCombo(int addCombo, float deltaDiff, int addScore)
    {
        //statsSystem.AddCombo(addCombo, deltaDiff, addScore);
    }

    public void Frenesi()
    {
        int rand = UnityEngine.Random.Range(0, 100);
        if(rand == UnityEngine.Random.Range(1, 7))
        {
            /* Durante 5 segundos o especial é repetido e nesse tempo pontuação no trilho 
                é aumentada em 3x. Tap = 150, Hold 225 e Slide 105 */
        }
    }
}
