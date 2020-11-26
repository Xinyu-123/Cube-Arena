using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;
    public float health;
    public float maxHealth = 100f;
    public int score = 0;
    public SkinnedMeshRenderer model;
    public MeshRenderer[] gunModel;
    public Animator animator;
    public SimpleShoot gun;

    public GameObject nameTag;
    public CameraController camera;
    public HealthBar healthBar;

    public bool respawning;

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        health = maxHealth;
        respawning = false;

        healthBar.SetMaxHealth(maxHealth);
    }

    public void SetHealth(float _health)
    {
        health = _health;
        healthBar.SetHealth(health);
        if (health <= 0f)
            Die();
    }

    public void Die()
    {
        if(Client.instance.myId == id)
        {
            Cursor.lockState = CursorLockMode.None;
            respawning = true;
            camera.ResetRotation();
        }
        nameTag.SetActive(false);
        healthBar.RemoveHealthBar();
        model.enabled = false;
        
        foreach (MeshRenderer mesh in gunModel)
        {
            mesh.enabled = false;
        }
        if(id == Client.instance.myId)
            UIManager.instance.respawnText.SetActive(true);
    }

    public void Respawn()
    {

        if (Client.instance.myId == id)
        {
            camera.ResetRotation();
            respawning = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        nameTag.SetActive(true);
        healthBar.DisplayHealthBar();
        model.enabled = true;
        foreach (MeshRenderer mesh in gunModel)
        {
            mesh.enabled = true;
        }


        if (id == Client.instance.myId)
            UIManager.instance.respawnText.SetActive(false);
        SetHealth(maxHealth);
    }

    
}
