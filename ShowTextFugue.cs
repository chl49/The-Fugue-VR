using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTextFugue : MonoBehaviour
{

    public GameObject text;

    void Start()
    {
        StartCoroutine("ShowText");

    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator ShowText()
    {
        yield return new WaitForSeconds(40f);
        text.SetActive(true);
    }
}
