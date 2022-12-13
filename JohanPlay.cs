using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JohanPlay : MonoBehaviour
{
    private Animator animator;
    public AudioSource audioSource;

    void Awake()
    {
        animator = GetComponent<Animator>();

    }

    void Start()
    {
        StartCoroutine("PlaySound");

    }

    void Update()
    { }


    IEnumerator PlaySound()
    {
        yield return new WaitForSeconds(5f);
        audioSource.Play();
    }
}
