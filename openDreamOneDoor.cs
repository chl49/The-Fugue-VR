using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openDreamOneDoor : MonoBehaviour
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
        animator.SetBool("doorCon", true);
    }


}
