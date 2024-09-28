using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator; // Reference to the Animator
    public float speed = 5.0f; // Movement speed

    void Update()
    {
        float move = Input.GetAxis("Vertical");  

        animator.SetFloat("Speed", Mathf.Abs(move));

        transform.Translate(Vector3.forward * move * speed * Time.deltaTime);
    }
}
