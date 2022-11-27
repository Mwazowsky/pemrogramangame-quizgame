using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnStart : MonoBehaviour
{
    //public GameObject targetGameObject;
    public bool isDisabled = true;
    public AudioSource Audio = null;
    
    void Start()
    {
        this.gameObject.SetActive(true);
        if (Audio != null)
        {
            Audio.Play();
        }

        if (isDisabled == true)
        {
            this.gameObject.SetActive(false);
        }
    }
}

