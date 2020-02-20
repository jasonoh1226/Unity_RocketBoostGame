using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]

public class Rocket : MonoBehaviour
{
    // Member variables
    [SerializeField] float controlRotate = 100f;
    [SerializeField] float controlThrust = 100f;
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] AudioClip mainEngineSound;
    [SerializeField] AudioClip successSound;
    [SerializeField] AudioClip deathSound;

    [SerializeField] ParticleSystem mainEngineParticle;
    [SerializeField] ParticleSystem successParticle;
    [SerializeField] ParticleSystem deathParticle;




    Rigidbody rigidbody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending }
    State state = State.Alive;

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
        // Player only can control when the rocket is alive
        if(state == State.Alive)
        {
            Thrust();
            Rotate();
        }
        
    }

    private void Thrust()
    {
        float thrustThisFrame = controlRotate * Time.deltaTime;

        // Can thrust while rotating
        if (Input.GetKey(KeyCode.Space))
        {
            mainEngineParticle.Play();

            rigidbody.AddRelativeForce(Vector3.up * thrustThisFrame);

            // So it doesn't layer each other
            if (!audioSource.isPlaying)
                audioSource.PlayOneShot(mainEngineSound);
           
        }
        else
        {
            audioSource.Stop();
            mainEngineParticle.Stop();
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
        if (state != State.Alive) { return; } // Ignore the collision

        switch(collision.gameObject.tag)
        {
            case "SafetyZone":
                // Do nothing
                break;
            case "Finish":
                ProcessFinish();
                break;
            default:
                ProcessDeath();
                break;
        }
    }

    private void ProcessFinish()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(successSound);
        state = State.Transcending;
        successParticle.Play();
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    private void ProcessDeath()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(deathSound);
        state = State.Dying;
        deathParticle.Play();
        Invoke("LoadFirstLevel", levelLoadDelay);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        int currentSceneInx = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIdx = currentSceneInx + 1;
        if (nextSceneIdx == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIdx = 0;
        }
        SceneManager.LoadScene(nextSceneIdx);
    }
}
