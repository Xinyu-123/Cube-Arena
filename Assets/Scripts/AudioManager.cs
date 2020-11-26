using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class AudioManager: MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource gunShot;
    public AudioSource hitMarker;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }

    }

    public void PlayerHit()
    {
        hitMarker.Play();
    }

    public void GunShot()
    {
        gunShot.Play();
    }


}

