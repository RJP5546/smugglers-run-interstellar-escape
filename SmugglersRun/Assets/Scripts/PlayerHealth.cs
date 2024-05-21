using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private GameManager gm;

    [SerializeField] private int health;
    [SerializeField] private int maxHealth;
    [SerializeField] private GameObject explosionFX;
    [SerializeField] private GameObject[] HealthUIHearts;
    [SerializeField] private FMODUnity.EventReference crashEvent;

    private ParticleSystem explosionParticle;
    [SerializeField] private bool invincible;
    public bool IsInPlanetTransition = false;
    private int obstacleLayer;
    private int triggerLayer;

    // Start is called before the first frame update
    void Start()
    {
        //retrieve explosionFX main module and set callback for use when player dies
        explosionParticle = explosionFX.GetComponent<ParticleSystem>();
        var main = explosionParticle.main;
        main.stopAction = ParticleSystemStopAction.Callback;

        //set layer to check collisions on
        obstacleLayer = LayerMask.NameToLayer("Obstacle");
        triggerLayer = LayerMask.NameToLayer("Trigger");

        //fetch gamemanager and initialize health
        gm = GameManager.instance;
        invincible = false;

        health = maxHealth;
        Time.timeScale = 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        var otherLayer = other.gameObject.layer;

        Debug.Log(other.gameObject.layer);

        if (otherLayer == obstacleLayer && !invincible)
        {
            print("Obstacle collision detected");
            health -= 1;
            HealthUIHearts[health].SetActive(false);
            FMODUnity.RuntimeManager.PlayOneShot(crashEvent, transform.position);

            StartCoroutine(DamageCooldown());

            if (health <= 0)
            {
                explosionParticle.Play();
                GetComponent<MeshRenderer>().enabled = false;
                gm.gameOver();
            }

        }
        else if (otherLayer == triggerLayer)
        {
            if (gm.GetIsInSpace()) { gm.SetIsInSpace(false); }
        }
    }

    private void OnParticleSystemStopped()
    {
        gameObject.SetActive(false);
    }

    public IEnumerator DamageCooldown()
    {
        SetInvincible();
        yield return new WaitForSeconds(3);
        if(!IsInPlanetTransition)
        {
            SetVulnerable();
            print("damage cooldown ended");
        }
    }

    public void SetInvincible()
    {
        invincible = true;
    }

    public void SetVulnerable()
    {
        invincible = false;
    }

    public void SetInTransition(bool val)
    {
        IsInPlanetTransition = val;
    }

}
