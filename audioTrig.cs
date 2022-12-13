using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioTrig : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject source;
    public AudioSource audioSource;
    public AudioClip sound;
    public string tag;
    private bool first  =false; 
    void Start()
    {
        audioSource = source.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.tag!=tag && !first){
            audioSource.clip = sound;
            audioSource.Play();
            first=true;
        }
    }
}