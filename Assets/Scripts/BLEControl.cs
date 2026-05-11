using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BLEControl : MonoBehaviour
{
    public float speed;
    public CharacterController character;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        character = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 forward = transform.right * x + transform.forward * z;
        character.SimpleMove(forward*speed);
    }
}
