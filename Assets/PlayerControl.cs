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
    bool landed = false;

    void Start()
    {
        impact = GetComponent<AudioSource>();
        rbody = GetComponent<Rigidbody>();
        impactParticles = GetComponent<ParticleSystem>();
    }
    void Update()
    {
        if (!landed) {
            moveForward();
            control();
        }
    }
    void OnCollisionEnter(Collision other) {
        if (!landed) {
            if (other.gameObject.CompareTag("Goal")) {
                StartCoroutine(delayedWinSound(1f, other.gameObject.GetComponent<AudioSource>()));
                successfulImpact();
                //TODO Signal public goal success
                Debug.Log("END");
            } else {
                failedImpact();
                //TODO Signal public goal failed
            }
        }
    }
    private void control()
    {
        float h = yawSpeed * Input.GetAxis("Mouse X");
        float v = pitchSpeed * Input.GetAxis("Mouse Y");
        transform.Rotate(-v, h, 0);
    }
    private void moveForward()
    {   
        transform.Translate(Vector3.forward * Time.deltaTime * forwardSpeed);
    }
    private void successfulImpact()
    {
        landed = true;
        impact.Play();
        rbody.freezeRotation = true;
        rbody.isKinematic = true;
        impactParticles.Play();
        //TODO pan camera to hit object
    }
    private void failedImpact()
    {
        //TODO make look more failed-like (bounce off that crap)
        landed = true;
        impact.Play();
        rbody.useGravity = true;
        impactParticles.Play();
        //TODO pan camera to hit object
    }
    IEnumerator delayedWinSound(float x, AudioSource audio)
    {
        yield return new WaitForSeconds(x);
        audio.Play();
    }
}
