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
    private int rng;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Actions>();
        _anim = this.transform.gameObject.GetComponent<Animator>();

        boundariLeft = GameObject.Find("Boudary Left");
        boundariRight = GameObject.Find("Boudary Right");
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (_player.idle == true || _player.frenesiHappening == true)
        {
            transform.position = transform.position + new Vector3(direction * moveSpeed * Time.deltaTime, 0, 0);
            _anim.SetFloat("Walking", direction);
            _anim.SetFloat("Up", 0);

            if (transform.position.x < boundariLeft.transform.position.x)
            {
                direction = 1;
            }
            else if (transform.position.x > boundariRight.transform.position.x)
            {
                direction = -1;
            }
        }
        else
        {
            rng = Random.Range(0, 101);
            //Debug.Log(rng);
            if (rng >= 0 && rng <= 50)
                _anim.SetFloat("Walking", 0);
            else if(rng > 50 && rng <= 100)
                _anim.SetFloat("Up", 0.6f);
        }

        /*if(_player.special == true)
            _anim.SetFloat("Walking", 0);*/

    }
}
