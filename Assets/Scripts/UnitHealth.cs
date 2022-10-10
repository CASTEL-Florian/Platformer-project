using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitHealth : MonoBehaviour
{
    [SerializeField] private UnityEvent damageEvent;
    [SerializeField] private UnityEvent deathEvent;
    [SerializeField] private int maxHealth;
    [SerializeField] private bool screenShake = false;
    [SerializeField] private GameObject deathParticles;
    private int currentHealth;
    private bool alive = true;
    private MyCamera cam;

    private void Start()
    {
        cam = FindObjectOfType<MyCamera>();
    }
    private void OnEnable()
    {
        alive = true;
        currentHealth = maxHealth;
    }

    public void TakeDamage(int value)
    {
        if (!alive)
            return;
        currentHealth -= value;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        damageEvent.Invoke();
        if (currentHealth <= 0)
        {
            alive = false;
            deathEvent.Invoke();
            if (deathParticles)
                Instantiate(deathParticles, transform.position, transform.rotation);
        }
        if (screenShake)
            StartCoroutine(cam.Shake());
    }


    public bool isAlive()
    {
        return alive;
    }



    public void SetMaxHealth(int value)
    {
        maxHealth = value;
        currentHealth = value;
    }
}