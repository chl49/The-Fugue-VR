using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

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
    private bool theatreClear = false;
    private bool playerLock = false;
    private float gravitySpeed;
    private float distToGround;
    private float headToBody;
    void Start() {
        headToBody = headCollision.transform.position.y-bodyCollision.transform.position.y;
        distToGround = bodyCollision.bounds.extents.y;
        audioSource = GetComponent<AudioSource>();
        gravitySpeed = Player.instance.transform.position.y;
    }

    bool IsGrounded() {
        //return head.SweepTest(Player.instance.hmdTransform.TransformDirection(Vector3.down), out hit, 0.2f);
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }
    
    bool IsSinking() {
        Vector3 headHeight = Player.instance.hmdTransform.TransformDirection(new Vector3(0,1,0));
        return Physics.Raycast(transform.position+Vector3.ProjectOnPlane(headHeight,Vector3.forward), -Vector3.up, 1f+distToGround);
    }
    void pianoCollision() {
        if(first==false&&!audioSource.isPlaying){
            //TODO: move hit.collider.tag"piano" inside stateDown
            /*
            if(interactionBoolean.stateDown && !interBool){
                interBool = true;
                print(interactionBoolean);
            }
            if(interactionBoolean.stateUp && interBool){
                interBool = false;
            }
            */
            PlayAudioClip( audioSource, performace );
            first=true;
            playerLock=true;
            }
        if(first==true&&!audioSource.isPlaying){
            theatreClear=true;
            playerLock=false;
        }
    }
    void theatreCollision(RaycastHit hit) {
        if(theatreClear==true){
            Destroy(hit.collider.gameObject);
            boundaryCollision=false;
        }
        else {
            boundaryCollision=true;
        }
        //TEMP
        //Destroy(hit.collider.gameObject);
        //theatreClear=true;
    }

    void DetectCollider() {
        RaycastHit hit;
        distToGround = bodyCollision.bounds.extents.y;
        if (head.SweepTest(Player.instance.hmdTransform.TransformDirection(Vector3.forward), out hit, 0.4f)){
            switch (hit.collider.tag) {
                case "piano": pianoCollision();
                    break;
                case "theatre": theatreCollision(hit);
                    break;
                case "boundary": boundaryCollision=true;
                    break;
                default: boundaryCollision=false;
                    break;
            }
        }
        else{
            boundaryCollision=false;
        }
        if(first==true&&!audioSource.isPlaying){
            theatreClear=true;
            playerLock=false;
        }
    }
    void MoveForward(){
        if(!IsGrounded()){
            if(theatreClear==true){
                if(IsSinking()){
                    print("Sinking");
                    gravitySpeed=0;
                    gravitySpeed=gravitySpeed+2f;
                }
                else{
                    //print("please work"+transform.position.y);
                    gravitySpeed=gravitySpeed-0.1f;
                }
            }
        }
        else{
            gravitySpeed=0;
        }
        Vector3 gravity = Player.instance.hmdTransform.TransformDirection(new Vector3(0,gravitySpeed,0));
        transform.position += Time.deltaTime * Vector3.ProjectOnPlane(gravity,Vector3.forward);
        if(moveValue.axis.y>0){
            Vector3 direction = Player.instance.hmdTransform.TransformDirection(new Vector3(0,0,moveValue.axis.y));
            speed = moveValue.axis.y * sensitivity;
            speed = Mathf.Clamp(speed, 0, maxSpeed);
            transform.position += speed*Time.deltaTime * Vector3.ProjectOnPlane(direction,Vector3.up);
        }
        /////////
    }

    // Update is called once per frame
    void Update()
    {
        //collisionDetection
        DetectCollider();
        //movement
        if(!boundaryCollision&&!playerLock){
            MoveForward();
        }
    }
    private void PlayAudioClip( AudioSource source, AudioClip clip )
	{
		source.clip = clip;
		source.Play();
    }	
    private void onTriggerEnter(Collider other){
        if (other.gameObject.tag=="piano"){
            audioSource.Play();
        }
    }
}