using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actions : MonoBehaviour
{
    public float energyCharge = 0.0f;
    public bool canSpecial;
    public Animator animator;

    public Joystick joystick;
    public float moveSpeed;
    private float horizontalMove = 0f;
    private bool canMove = true; 

    void Update()
    {
        if(canMove == true)
        {
            if (joystick.Horizontal <= .2f)
            {
                horizontalMove = moveSpeed;
            }
            else if (joystick.Horizontal > .2f)
            {
                horizontalMove = moveSpeed;
            }
            else
            {
                horizontalMove = 0f;
            }
        }

        // Check if special is ready and play the animation
        if (energyCharge == 5)
        {
            animator.SetBool("Special", true);
        }

    }

    void FixedUpdate()
    {
        animator.SetFloat("Walking", Mathf.Abs(joystick.Horizontal));
        var moviment = joystick.Horizontal;
        transform.position += new Vector3(moviment, 0, 0) * Time.time * moveSpeed;
    }

    public void chargeSpecial()
    {
        energyCharge++;
    }

    public void AddCombo(int addCombo, float deltaDiff, int addScore)
    {
        //statsSystem.AddCombo(addCombo, deltaDiff, addScore);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.otherCollider is BoxCollider2D)
        {
            if (collision.collider.tag == "Boudary")
            {
                canMove = false;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.otherCollider is BoxCollider2D)
        {
            if (collision.collider.tag == "Boudary")
            {
                canMove = true;
            }
        }
    }
}
