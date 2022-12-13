using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openDoor : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject greenRoomDoor;
    public Animator doorOpen_Anim1;
    public AudioSource audioSource;
    public AudioClip creaking;
    void Start()
    {
        doorOpen_Anim1 = greenRoomDoor.GetComponent<Animator>();
        audioSource = greenRoomDoor.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.tag=="triggered"){
            doorOpen_Anim1.SetBool("doorCon", true);
            audioSource.clip = creaking;
            audioSource.Play();
            gameObject.tag="Untagged";
        }
    }
}
