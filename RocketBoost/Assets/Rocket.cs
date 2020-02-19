using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    // Member variables
    [SerializeField] float controlRotate = 100f;
    [SerializeField] float controlThrust = 100f;
    Rigidbody rigidbody;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

        // Fetch the AudioSource from the GameObject
        audioSource = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();
    }

    private void Thrust()
    {
        float thrustThisFrame = controlRotate * Time.deltaTime;

        // Can thrust while rotating
        if (Input.GetKey(KeyCode.Space))
        {
            rigidbody.AddRelativeForce(Vector3.up * thrustThisFrame);

            // So it doesn't layer each other
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void Rotate()
    {
        // Take manual control of rotation
        rigidbody.freezeRotation = true;

        float rotationThisFrame = controlRotate * Time.deltaTime;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        // Resume physics control of rotation
        rigidbody.freezeRotation = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch(collision.gameObject.tag)
        {
            case "SafetyZone":
                print("Ok");
                break;
            default:
                print("Dead");
                break;
        }
    }

}
