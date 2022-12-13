using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playPiano : MonoBehaviour
{
    public GameObject keys;
    public Animator m_animator;
    public string anim;
    public AudioSource audioSource;
    public AudioClip performance;
    public GameObject crowd;
    public AudioSource audioSource2;
    public AudioClip audience;
    public GameObject light;
    public AudioSource audioSource3;
    public AudioClip flicker;
    [Header("Invisible Boxes")]
    public GameObject invisibleBox1;
    public GameObject invisibleBox2;
    public GameObject invisibleBox3;
    public GameObject invisibleBox4;

    public GameObject light1;
    public GameObject light2;
    public GameObject light3;
    public GameObject light4;
    private bool first = false;
    //public MeshRenderer my_renderer; // V10
    //public Material normalPaper; // V10
    //public Material glow; // V10
    public GameObject g_piano;

    public GameObject stagelight_pointlight;
    public Animator sp_animator;
    public GameObject red_light;
    public Animator rl_animator;
    // Start is called before the first frame update
    void Start()
    {

        m_animator = keys.GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        audioSource2 = crowd.GetComponent<AudioSource>();
        audioSource3 = light.GetComponent<AudioSource>();
        invisibleBox1.SetActive(false);
        invisibleBox2.SetActive(false);
        invisibleBox3.SetActive(false);
        invisibleBox4.SetActive(false);
        light1.SetActive(false);
        light2.SetActive(false);
        light3.SetActive(false);
        light4.SetActive(false);

        //my_renderer = GetComponent<MeshRenderer>(); // V10
        //my_renderer.material = glow;
        //p_renderer = p_renderer.GetComponent<MeshRenderer>(); // V10
        //p_renderer.material = material;

        sp_animator = stagelight_pointlight.GetComponent<Animator>();
        rl_animator = red_light.GetComponent<Animator>();


    }

    // Update is called once per frame
    void Update()
    {

        //m_animator.SetBool(anim, true);
        if (gameObject.tag == "triggered" && !first)
        {
            //my_renderer.material = normalPaper;
            m_animator.SetBool(anim, true);
            invisibleBox1.SetActive(true);
            invisibleBox2.SetActive(true);
            invisibleBox3.SetActive(true);
            invisibleBox4.SetActive(true);

            g_piano.SetActive(false);
            audioSource.clip = performance;
            audioSource.Play();
            audioSource2.clip = audience;
            audioSource2.Play();
            sp_animator.SetBool(anim, true);
            rl_animator.SetBool(anim, true);

            first = true;
            keys.gameObject.tag = "Untagged";
            print("TRIGGER WARNING");
        }
        if (first == true && !audioSource.isPlaying)
        {
            audioSource3.clip = flicker;
            audioSource3.Play();
            invisibleBox1.SetActive(false);
            invisibleBox2.SetActive(false);
            invisibleBox3.SetActive(false);
            invisibleBox4.SetActive(false);
            m_animator.SetBool(anim, false);
            light1.SetActive(true);
            light2.SetActive(true);
            light3.SetActive(true);
            light4.SetActive(true);
            //keys.gameObject.tag=="Untagged"
            //keys.gameObject.tag=="triggered"
        }

    }
}
