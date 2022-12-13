using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDreamDoor : MonoBehaviour
{
    private Animator animator;
    public GameObject door;


    void Start()
    {
        animator = door.GetComponent<Animator>();
    }

    void Update()
    {

    }


    void OnTriggerEnter(Collider col)
    {
        animator.SetTrigger("openDreamDoor");
    }


}
