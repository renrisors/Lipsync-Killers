/*  
    Jogador             Reação
    Sem ação/Idle       Walk
    Walk                Up/Walk
    20 acertos          Golpe carregado (com chances de Frenesi)
    Golpe carregado     Idle
    Frenesi             Walk/Up
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OponentBehavior : MonoBehaviour
{
    private Actions _player;
    private Animator _anim;
    private float direction = -1;
    private float moveSpeed = 5;

    private GameObject boundariLeft;
    private GameObject boundariRight;

    private GameObject FrenesiSFX;
    private GameObject SpecialSFX;

    private int rng;
    private bool canWalk = true;
    private bool canChangeAction = true;
    private bool frenesi = false;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Actions>();
        _anim = this.transform.gameObject.GetComponent<Animator>();

        boundariLeft = GameObject.Find("Boudary Left");
        boundariRight = GameObject.Find("Boudary Right");

        FrenesiSFX = GameObject.Find("FrenesiSFX");
        SpecialSFX = GameObject.Find("SpecialSFX");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_player.startMoving == true)
        {
            HandleMovement();
        }

        if(_player.comboAtual == 20 && canWalk == true)
        {
            DecideFrenesi();
        }
    }

    public void HandleMovement()
    {
        if ((_player.idle == true  || _player.frenesiHappening == false || _player.special == false)  && canWalk == true)
        {
            transform.position = transform.position + new Vector3(direction * moveSpeed * Time.deltaTime, 0, 0);
            _anim.SetFloat("Walking", direction);
            //_anim.SetFloat("Up", 0);

            if (transform.position.x < boundariLeft.transform.position.x)
            {
                direction = 1;
            }
            else if (transform.position.x > boundariRight.transform.position.x)
            {
                direction = -1;
            }
        }
        if((_player.idle == false || _player.frenesiHappening == true) && _player.special == false)
        {
            DecideWalkUp();
        }

    }
    
    public void DecideWalkUp() //Decide if the oponent will walk or perform Up based on player actions
    {
        rng = Random.Range(0, 101);
        
        if(canChangeAction == true)
        {
            if (rng >= 0 && rng <= 50)
            {
                Debug.Log("DECIDIU WALK");
                _anim.SetFloat("Walking", direction);
                //_anim.SetFloat("Up", 0);
                canChangeAction = false;
                StartCoroutine(CanChangeAction());

            }
            else if (rng > 50 && rng <= 100)
            {
                Debug.Log("DECIDIU UP");
                _anim.SetFloat("Up", 1);
                //_anim.SetFloat("Walking", 0);
                _anim.SetTrigger("UpTrigger");
                //canWalk = false;
                canChangeAction = false;
                StartCoroutine(CanChangeAction());
            }
        }
    }

    public void DecideFrenesi()
    {
        rng = UnityEngine.Random.Range(0, 100);
        
        canWalk = false;
        if (frenesi == false)
        {            
            if (rng >= 1 && rng <= 12)
            {
                frenesi = true;
                _anim.SetFloat("Up", 0);
                _anim.SetFloat("Walking", 0);
                _anim.SetBool("Frenesi", true);
                _player.frenesiAnim.gameObject.SetActive(true);
                _player.frenesiAnim.SetBool("Frenesi", true);
                FrenesiSFX.GetComponent<AudioSource>().Play();

                StartCoroutine(CountdownFrenesi());
            }
            else if (frenesi == false)
            {
                Debug.Log("Entrou no else");
                _anim.SetFloat("Up", 0);
                _anim.SetFloat("Walking", 0);
                _anim.SetTrigger("Special");
                SpecialSFX.GetComponent<AudioSource>().Play();
                StartCoroutine(CanChangeAction());
            }
        }
    }

    IEnumerator CountdownFrenesi()
    {
        yield return new WaitForSeconds(6);
        _anim.SetBool("Frenesi", false);
        _player.frenesiAnim.gameObject.SetActive(false);
        _player.frenesiAnim.SetBool("Frenesi", false);
        yield return new WaitForSeconds(2);
        canWalk = true;
        frenesi = false;
    }

    IEnumerator CanChangeAction()
    {
        yield return new WaitForSeconds(3);
        //Debug.Log("3 seg depois");
        canWalk = true;
        canChangeAction = true;
    }
}
