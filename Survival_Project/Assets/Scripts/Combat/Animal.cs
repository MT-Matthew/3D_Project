using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    public string animalName;
    public bool playerInRange;
    public float detectionRange = 10f;

    [SerializeField] int currentHealth;
    [SerializeField] int maxHealth;

    [Header("Sounds")]
    [SerializeField] AudioSource soundChannel;
    [SerializeField] AudioClip mimicHitAndCream;
    [SerializeField] AudioClip mimicHitAndDie;

    [SerializeField] AudioClip mimicHit1;
    [SerializeField] AudioClip mimicHit2;
    [SerializeField] AudioClip mimicHit3;

    Animator animator;
    public bool isDead;

    [SerializeField] ParticleSystem bloodSplashParticles;
    public GameObject bloodPuddle;

    enum AnimalType
    {
        Rabbit,
        Mimic
    }

    [SerializeField] AnimalType thisAnimalType;




    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        float distance = Vector3.Distance(PlayerState.Instance.playerBody.transform.position, transform.position);

        if (distance < detectionRange)
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }
    }


    public void TakeDamage(int damage)
    {
        if (isDead == false)
        {
            currentHealth -= damage;

            bloodSplashParticles.Play();

            if (currentHealth <= 0)
            {
                PlayDyingSound();
                
                animator.SetBool("isDead", true);
                animator.SetTrigger("Die");
                GetComponent<AI_Movement>().enabled = false;
                GetComponent<NavMeshAgent>().enabled = false;
                StartCoroutine(PuddleDelay());
                // StartCoroutine(Remove());
                isDead = true;
            }
            else
            {
                animator.SetTrigger("GetHit");
                PlayHitSound();
            }
        }
        
    }

    public IEnumerator Remove()
    {
        yield return new WaitForSeconds(3.2f);
        Destroy(gameObject);
    }

    IEnumerator PuddleDelay()
    {
        yield return new WaitForSeconds(1f);
        bloodPuddle.SetActive(true);
    }

    void PlayHitSound()
    {
        switch (thisAnimalType)
        {
            case AnimalType.Mimic:
                // soundChannel.PlayOneShot(mimicHitAndCream);
                GetRandomMimic();
                break;
            case AnimalType.Rabbit:
                // soundChannel.PlayOneShot(mimicHitAndCream);
                GetRandomMimic();
                break;
            default:
                break;
        }
    }

    void PlayDyingSound()
    {
        switch (thisAnimalType)
        {
            case AnimalType.Mimic:
                soundChannel.PlayOneShot(mimicHitAndDie);
                break;
            case AnimalType.Rabbit:
                soundChannel.PlayOneShot(mimicHitAndDie);
                break;
            default:
                break;
        }
    }

    void GetRandomMimic()
    {
        int randomNum = Random.Range(0, 3);
        if (randomNum == 0)
        {
            soundChannel.PlayOneShot(mimicHit1);
        }
        else if(randomNum == 1)
        {
            soundChannel.PlayOneShot(mimicHit2);
        }
        else if(randomNum == 2)
        {
            soundChannel.PlayOneShot(mimicHit3);
        }
    }


}
