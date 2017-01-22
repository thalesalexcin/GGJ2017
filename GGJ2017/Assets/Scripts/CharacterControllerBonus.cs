using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Prime31;


public class CharacterControllerBonus : MonoBehaviour
{
    // movement config
    public float gravity = -25f;
    public float runSpeed = 8f;
    public float groundDamping = 20f; // how fast do we change direction? higher means faster
    public float inAirDamping = 5f;
    public float jumpHeight = 3f;

    public GameObject SignalsHolder;
    private GameObject _RespawnPoint;
    private GameObject endPoint;

    [HideInInspector]
    private float normalizedHorizontalSpeed = 0;

    private CharacterController2D _controller;
    private Animator _animator;
    private RaycastHit2D _lastControllerColliderHit;
    [HideInInspector]
    public Vector3 _velocity;
    public bool debugControls;
    public ParticleSystem robotParticles;
    private ParticleSystem spawnedParticles;
    private AudioManager _AudioManager;
    private Vector3 localLocalScale;

    void Awake()
    {
        _AudioManager = FindObjectOfType<AudioManager>();
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController2D>();
        _RespawnPoint = GameObject.FindGameObjectWithTag("Respawn");
        endPoint = GameObject.FindGameObjectWithTag("Finish");

        // listen to some events for illustration purposes
        _controller.onControllerCollidedEvent += onControllerCollider;
        _controller.onTriggerEnterEvent += onTriggerEnterEvent;
        _controller.onTriggerExitEvent += onTriggerExitEvent;

        transform.position = _RespawnPoint.transform.position;
        localLocalScale = transform.localScale;
        _velocity = Vector2.zero;
    }

    void Start()
    {
        SpawnParticles(robotParticles);
    }

    void SpawnParticles(ParticleSystem particlesToSpawn)
    {
        spawnedParticles = Instantiate(particlesToSpawn, transform);
        spawnedParticles.transform.position = transform.position;
        spawnedParticles.gameObject.SetActive(true);
        Destroy(spawnedParticles.gameObject, 2f);
    }


    #region Event Listeners

    void onControllerCollider(RaycastHit2D hit)
    {
        // bail out on plain old ground hits cause they arent very interesting
        if (hit.normal.y == 1f)
            return;

        // logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
        //Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
    }


    void onTriggerEnterEvent(Collider2D col)
    {
        //Debug.Log("onTriggerEnterEvent: " + col.gameObject.name);
        if (col.gameObject.tag == "BlocksTrigger")
        {
            _velocity.x += 20;
        }
        if (col.gameObject.tag == "Signal")
        {
            switch(col.gameObject.GetComponent<SignalWave>().InputType)
            {
                case EInputType.Left:
                    ReceiveMoveLeft(col.gameObject.GetComponent<SignalWave>().robotSpeed);
                    break;
                case EInputType.Right:
                    ReceiveMoveRight(col.gameObject.GetComponent<SignalWave>().robotSpeed);
                    break;
                case EInputType.Jump:
                    ReceiveMoveJump(col.gameObject.GetComponent<SignalWave>().robotSpeed);
                    break;
            }
            //Destroy(col.gameObject);
        }

        if (col.CompareTag("Hazard"))
        {
            Dead();
        }
        if (col.CompareTag("Finish"))
        {
            var scene = SceneManager.GetActiveScene();
            var nextSceneId = scene.buildIndex + 1;

            if (nextSceneId >= SceneManager.sceneCountInBuildSettings)
                nextSceneId = 1;

            _AudioManager.Play(EAudioType.EndLevel);
            SceneManager.LoadScene(nextSceneId, LoadSceneMode.Single);
        }
    }

    public void ReceiveMoveLeft(float velocityToAdd)
    {
        _velocity.x -= velocityToAdd;
        _AudioManager.PlayOneShot(EAudioType.Roll);
    }

    public void ReceiveMoveRight(float velocityToAdd)
    {
        _velocity.x += velocityToAdd;
        _AudioManager.PlayOneShot(EAudioType.Roll);
    }

    public void ReceiveMoveJump(float velocityToAdd)
    {
        if (_controller.isGrounded)
        {
            _velocity.y = Mathf.Sqrt(2f * (jumpHeight) * -gravity);
            _AudioManager.Play(EAudioType.Jump);
        }
        else
        {
            var addJump = velocityToAdd;
            _velocity.y += (jumpHeight * addJump) * 0.8f;
        }
    }

    void onTriggerExitEvent(Collider2D col)
    {
        Debug.Log("onTriggerExitEvent: " + col.gameObject.name);
    }

    #endregion

    void Dead()
    {
        //_EraseEnnemies();
        _AudioManager.Play(EAudioType.Die);

        transform.position = _RespawnPoint.transform.position;
        _velocity = Vector2.zero;
        SpawnParticles(robotParticles);
    }

    private void _EraseEnnemies()
    {
        foreach (var child in SignalsHolder.GetComponentsInChildren<SignalWave>())
            Destroy(child.gameObject);
    }

    // the Update loop contains a very simple example of moving the character around and controlling the animation
    void Update()
    {
        if (_controller.isGrounded)
        {
            //_velocity.y = 0;
        }

        if (_velocity.x > 0f)
        {
            transform.localScale = localLocalScale;
        }

        else if (_velocity.x < 0f)
        {
            transform.localScale = new Vector3(-localLocalScale.x, localLocalScale.y, localLocalScale.z);
        }

        if ((Input.GetKey(KeyCode.RightArrow))&&(debugControls))
        {
            normalizedHorizontalSpeed = 1;
            if (transform.localScale.x < 0f)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

            if (_controller.isGrounded)
            {
                //_animator.Play(Animator.StringToHash("Run"));
            }
        }
        else if ((Input.GetKey(KeyCode.LeftArrow))&&(debugControls))
        {
            normalizedHorizontalSpeed = -1;
            if (transform.localScale.x > 0f)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

            if (_controller.isGrounded)
            {
                //_animator.Play(Animator.StringToHash("Run"));
            }
        }
        else
        {
            normalizedHorizontalSpeed = 0;

            if (_controller.isGrounded)
            {
                //_animator.Play(Animator.StringToHash("Idle"));
            }
        }


        // we can only jump whilst grounded
        if ((_controller.isGrounded && Input.GetKeyDown(KeyCode.UpArrow))&&(debugControls))
        {
            _velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
            //_animator.Play(Animator.StringToHash("Jump"));
        }


        // apply horizontal speed smoothing it. dont really do this with Lerp. Use SmoothDamp or something that provides more control
        var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
        _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor);

        // apply gravity before moving
        _velocity.y += gravity * Time.deltaTime;

        // if holding down bump up our movement amount and turn off one way platform detection for a frame.
        // this lets us jump down through one way platforms
        if ((_controller.isGrounded && Input.GetKey(KeyCode.DownArrow))&&(debugControls))
        {
            _velocity.y *= 3f;
            _controller.ignoreOneWayPlatformsThisFrame = true;
        }

        _controller.move(_velocity * Time.deltaTime);

        // grab our current _velocity to use as a base for all calculations
        _velocity = _controller.velocity;

        //if (Mathf.Abs(_velocity.x) <= 0.1f)
        //    _AudioManager.Mute(EAudioType.Roll, true);
    }

}
