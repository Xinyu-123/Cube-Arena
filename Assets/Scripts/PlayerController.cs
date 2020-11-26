using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform camTransform;
    public PlayerManager player;
    
    private bool allowFire = true;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (allowFire)
            {
                ClientSend.PlayerShoot(camTransform.forward);
                StartCoroutine(FireRate());
            }
        }
        

    }
    private void FixedUpdate()
    {
        SendInputToServer();
    }

    private void SendInputToServer()
    {
        bool[] _inputs = new bool[]
        {
            Input.GetKey(KeyCode.W),
            Input.GetKey(KeyCode.S),
            Input.GetKey(KeyCode.D),
            Input.GetKey(KeyCode.A),
            Input.GetKey(KeyCode.Space)
        };



        ClientSend.PlayerMovement(_inputs);
    }

    IEnumerator FireRate()
    {
        allowFire = false;
        yield return new WaitForSeconds(0.5f);
        allowFire = true;
    }
}
