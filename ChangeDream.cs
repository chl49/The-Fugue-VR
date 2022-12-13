using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeDream : MonoBehaviour
{
    public Scene scene;

    void OnTriggerEnter(Collider col)
    {
        SceneManager.LoadScene("Dream2");
    }



}
