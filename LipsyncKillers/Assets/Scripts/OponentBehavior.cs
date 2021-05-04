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

        if (_player.idle == true)
        {
            transform.position = transform.position + new Vector3(direction * moveSpeed * Time.deltaTime, 0, 0);
            _anim.SetFloat("Walking", direction);

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
            _anim.SetFloat("Walking", 0);
        }

    }
}
