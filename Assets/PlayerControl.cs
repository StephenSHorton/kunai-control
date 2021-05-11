using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] float yawSpeed = 2.0f;
    [SerializeField] float pitchSpeed = 2.0f;
    [SerializeField] float forwardSpeed = 2.0f;

    AudioSource impact;
    Rigidbody rbody;
    ParticleSystem impactParticles;
    bool frozen = false;

    void Start()
    {
        impact = GetComponent<AudioSource>();
        rbody = GetComponent<Rigidbody>();
        impactParticles = GetComponent<ParticleSystem>();
    }
    void Update()
    {
        if (!frozen){
            moveForward();
            control();
        }
    }

    void OnCollisionEnter(Collision hit) {
        frozen = true;
        impact.Play();
        rbody.freezeRotation = true;
        rbody.isKinematic = true;
        impactParticles.Play();
    }

    private void control()
    {
        // Get the mouse delta. This is not in the range -1...1
        float h = yawSpeed * Input.GetAxis("Mouse X");
        float v = pitchSpeed * Input.GetAxis("Mouse Y");
        transform.Rotate(-v, h, 0);
    }
    private void moveForward()
    {   
        transform.Translate(Vector3.forward * Time.deltaTime * forwardSpeed);
    }
}
