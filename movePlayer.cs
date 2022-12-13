using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.SceneManagement;

public class movePlayer : MonoBehaviour
{
    public SteamVR_Action_Vector2 moveValue;
    public SteamVR_Action_Boolean interactionBoolean;
    public bool interBool;
    public float maxSpeed;
    public float sensitivity;
    public Rigidbody head;
    public Rigidbody body;
    public Collider headCollision;
    public Collider bodyCollision;
    private float speed = 0.0f;
    public AudioSource audioSource;
    public AudioClip performace;
    public Player player;
    private bool first = false;
    private bool boundaryCollision = false;
    //private bool IsElevation = false;
    private bool falling = false;  //V11
    public bool IsElevation = false;
    private bool playerLock = false;
    private float gravitySpeed;
    private float distToGround;
    private float headToBody;
    public GameObject keys;
    public Animator m_animator;
    public GameObject greenRoomDoor;
    public Animator doorOpen_Anim1;

    void Start()
    {
        headToBody = headCollision.transform.position.y - bodyCollision.transform.position.y;
        distToGround = bodyCollision.bounds.extents.y;
        audioSource = GetComponent<AudioSource>();
        gravitySpeed = Player.instance.transform.position.y;
        m_animator = keys.GetComponent<Animator>();
        doorOpen_Anim1 = greenRoomDoor.GetComponent<Animator>();
        falling = false;
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
        //RaycastHit hit;
        //return head.SweepTest(Player.instance.hmdTransform.TransformDirection(-Vector3.up), out hit,distToGround + 0.5f);
    }

    bool IsSinking()
    {
        Vector3 headHeight = Player.instance.hmdTransform.TransformDirection(new Vector3(0, 1, 0));
        return Physics.Raycast(transform.position + Vector3.ProjectOnPlane(headHeight, Vector3.forward), -Vector3.up, 0.75f + distToGround);
    }

    bool leftBounds()
    {
        //return head.SweepTest(Player.instance.hmdTransform.TransformDirection(Vector3.down), out hit, 0.2f);
        RaycastHit hit;
        return head.SweepTest(Player.instance.hmdTransform.TransformDirection(Vector3.left), out hit, 0.4f);
    }
    bool rightBounds()
    {
        //return head.SweepTest(Player.instance.hmdTransform.TransformDirection(Vector3.down), out hit, 0.2f);
        RaycastHit hit;
        return head.SweepTest(Player.instance.hmdTransform.TransformDirection(Vector3.right), out hit, 0.4f);
    }
    bool backBounds()
    {
        //return head.SweepTest(Player.instance.hmdTransform.TransformDirection(Vector3.down), out hit, 0.2f);
        RaycastHit hit;
        return head.SweepTest(Player.instance.hmdTransform.TransformDirection(-Vector3.forward), out hit, 0.4f);
        //return Physics.Raycast(transform.position, -Vector3.forward, 0.4f);
    }

    bool moveY()
    {
        //return head.SweepTest(Player.instance.hmdTransform.TransformDirection(Vector3.down), out hit, 0.2f);
        return Mathf.Abs(moveValue.axis.y) >= Mathf.Abs(moveValue.axis.x);
    }

    void pianoCollision(RaycastHit hit)
    {
        // if(interactionBoolean.stateDown){
        hit.collider.tag = "triggered";
        if (first == false && !audioSource.isPlaying)
        {
            PlayAudioClip(audioSource, performace);
            first = true;
            playerLock = true;
        }
        if (first == true && !audioSource.isPlaying)
        {
            playerLock = false;
            IsElevation = true;
        }
        // }
    }
    void theatreCollision(RaycastHit hit)
    {
        if (IsElevation == true)
        {
            Destroy(hit.collider.gameObject);
            boundaryCollision = false;
        }
        else
        {
            boundaryCollision = true;
        }
        //TEMP
        //Destroy(hit.collider.gameObject);
        //theatreClear=true;
    }
    void changeTag(RaycastHit hit)
    {    //V10
        if (interactionBoolean.stateDown)
        {
            hit.collider.tag = "triggered";
        }
    }

    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("Dream2");
    }

    void DetectCollider()
    {
        RaycastHit hit;
        distToGround = bodyCollision.bounds.extents.y;
        if (head.SweepTest(Player.instance.hmdTransform.TransformDirection(Vector3.forward), out hit, 0.4f))
        {
            switch (hit.collider.tag)
            {
                case "piano":
                    pianoCollision(hit);
                    break;
                case "window":
                    hit.collider.tag = "triggered";
                    break;
                case "criticTrigger":
                    hit.collider.tag = "triggered";
                    break;
                case "theatre":
                    theatreCollision(hit);
                    break;
                case "boundary":
                    boundaryCollision = true;
                    break;
                case "fallTrigger":
                    m_animator.SetBool("fall", true); falling = true; //V11
                    hit.collider.tag = "triggered";
                    StartCoroutine("ChangeScene");
                    break;
                case "unlockDoor":
                    doorCollision();
                    break;
                case "changeMaterial":
                    changeTag(hit);   //V10 
                    break;
                default:
                    boundaryCollision = false;
                    break;
            }
        }
        else
        {
            boundaryCollision = false;
        }
        if (first == true && !audioSource.isPlaying)
        {
            IsElevation = true;
            playerLock = false;
        }
    }
    void Elevation()
    {
        if (!IsGrounded())
        {
            if (IsElevation == true)
            {
                if (IsSinking() && !falling)
                {  //V11
                    print("Sinking");
                    gravitySpeed = 0;
                    gravitySpeed = gravitySpeed + 2f;
                }
                else
                {
                    //print("please work"+transform.position.y);
                    gravitySpeed = gravitySpeed - 0.1f;
                }
            }
        }
        else
        {
            if (falling)
            {
                gravitySpeed = gravitySpeed - 0.1f;
            }
            else
            {
                gravitySpeed = 0;
            }
        }
        Vector3 gravity = Player.instance.hmdTransform.TransformDirection(new Vector3(0, gravitySpeed, 0));
        //transform.position += Time.deltaTime * Vector3.ProjectOnPlane(Quaternion.Inverse(camera.transform.rotation) * gravity, Vector3.forward);
        transform.position += Time.deltaTime * Vector3.ProjectOnPlane(Quaternion.Inverse(Player.instance.hmdTransform.rotation) * gravity, Vector3.forward);
        //transform.position += Time.deltaTime * Vector3.ProjectOnPlane( gravity, Vector3.forward);
        /////////
    }
    void MoveForward()
    {
        if (moveValue.axis.y > 0 && !boundaryCollision)
        {
            Vector3 direction = Player.instance.hmdTransform.TransformDirection(new Vector3(0, 0, moveValue.axis.y));
            speed = moveValue.axis.y * sensitivity;
            speed = Mathf.Clamp(speed, 0, maxSpeed);
            transform.position += speed * Time.deltaTime * Vector3.ProjectOnPlane(direction, Vector3.up);
        }
        else if (moveValue.axis.y < 0 && !backBounds())
        {
            Vector3 direction = Player.instance.hmdTransform.TransformDirection(new Vector3(0, 0, moveValue.axis.y));
            speed = moveValue.axis.y * sensitivity;
            speed = Mathf.Clamp(-speed, 0, maxSpeed);
            transform.position += speed * Time.deltaTime * Vector3.ProjectOnPlane(direction, Vector3.up);
        }
        /////////
    }
    void MoveSideways()
    {
        if (moveValue.axis.x > 0 && !rightBounds())
        {//right
            Vector3 direction = Player.instance.hmdTransform.TransformDirection(new Vector3(moveValue.axis.x, 0, 0));
            speed = moveValue.axis.x * sensitivity;
            speed = Mathf.Clamp(speed, 0, maxSpeed);
            transform.position += speed * Time.deltaTime * Vector3.ProjectOnPlane(direction, Vector3.up);
        }

        if (moveValue.axis.x < 0 && !leftBounds())
        {//left
            Vector3 direction = Player.instance.hmdTransform.TransformDirection(new Vector3(moveValue.axis.x, 0, 0));
            speed = moveValue.axis.x * sensitivity;
            speed = Mathf.Clamp(-speed, 0, maxSpeed);
            transform.position += speed * Time.deltaTime * Vector3.ProjectOnPlane(direction, Vector3.up);
        }
        /////////
    }

    // Update is called once per frame
    void Update()
    {
        //collisionDetection
        DetectCollider();
        //movement
        Elevation();
        if (moveY() && !playerLock)
        {
            MoveForward();
        }
        else if (!moveY() && !playerLock)
        {
            MoveSideways();
        }
    }

    private void doorCollision()
    {
        // DetectCollider();
        // if (interactionBoolean.stateDown)
        // {
        //     doorOpen_Anim1.SetBool("doorCon", true);
        // }
        boundaryCollision = true;
        // DetectCollider();
    }
    private void PlayAudioClip(AudioSource source, AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }
    private void onTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "piano")
        {
            audioSource.Play();
        }
    }
}
