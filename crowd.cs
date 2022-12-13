using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crowd : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject voices;
    public AudioSource audioSource;
    public AudioClip gossip;
    public AudioClip gossip2;
    private bool first  =false; 
    void Start()
    {
        audioSource = voices.GetComponent<AudioSource>();
        audioSource.clip = gossip;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(gameObject.tag=="triggered" && !first){
            audioSource.clip = gossip2;
            audioSource.Play();
            first=true;
            gameObject.tag="Untagged";
            print("TRIGGER WARNING");
        }
        
    }
}
