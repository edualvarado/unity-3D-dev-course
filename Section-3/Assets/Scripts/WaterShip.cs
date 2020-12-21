using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaterShip : MonoBehaviour
{
    [Header("Motion Settings")]
    [Range(0, 100000f)] [SerializeField] float mainThrust = 50000f;
    [Range(0, 200f)] [SerializeField] float rcsThrust = 100f;
    [SerializeField] float levelLoadDelay = 1f;

    [Header("Audio Settings")]
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip rcsEngine;
    [SerializeField] AudioClip destroy;
    [SerializeField] AudioClip success;

    [Header("Particles Effect")]
    [SerializeField] ParticleSystem destroyParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem mainEngineParticles;

    [SerializeField] bool debugMode = false;
    public bool collisionsEnabled = true;

    enum State { Alive, Dying, Transcending }
    State state = State.Alive;

    Rigidbody rigidBody;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = this.GetComponent<Rigidbody>();
        audioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }

        RespondToDebugMode();
    }

    private void RespondToDebugMode()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            LoadNextScene();
        }
        else if(Input.GetKeyDown(KeyCode.C))
        {
            collisionsEnabled = !collisionsEnabled;
        }
    }

    private void RespondToThrustInput()
    {
        float Speed = mainThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.Space))
            ApplyThrust(Speed);
        else
            audioSource.Stop();
            mainEngineParticles.Stop();
    }

    private void RespondToRotateInput()
    {
        rigidBody.angularVelocity = Vector3.zero;

        float rotationSpeed = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            Debug.Log("A");
            transform.Rotate(Vector3.forward * rotationSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationSpeed);
        }
    }


    private void ApplyThrust(float Speed)
    {
        rigidBody.AddRelativeForce(Vector3.up * Speed * Time.deltaTime);
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(mainEngine);

        if (!mainEngineParticles.isPlaying)
            mainEngineParticles.Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive || !collisionsEnabled)
            return;

        switch(collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDestroySequence();
                break;
        }
    }

    private void StartDestroySequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(destroy);
        destroyParticles.Play();
        Invoke("LoadFirstScene", levelLoadDelay);
    }

    private void StartSuccessSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        successParticles.Play();
        Invoke("LoadNextScene", levelLoadDelay);
    }

    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }

        SceneManager.LoadScene(nextSceneIndex);
    }

    private void LoadFirstScene()
    {
        SceneManager.LoadScene(0);
    }
}
