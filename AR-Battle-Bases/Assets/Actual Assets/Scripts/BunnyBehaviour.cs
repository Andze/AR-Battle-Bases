using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyBehaviour : MonoBehaviour
{
    public Texture2D eggTexture;
    public HealthBarBehaviour healthBar;

    private GameObject preSpawn;
    private GameObject postSpawn;
    private Animator bunnyAnimator;

    private void Awake()
    {
        preSpawn = transform.GetChild(0).gameObject;
        postSpawn = transform.GetChild(1).gameObject;
        bunnyAnimator = postSpawn.transform.GetComponentInChildren<Animator>();

        if (eggTexture != null) {
            for (int i = 0; i < preSpawn.transform.childCount; i++) {
                if (preSpawn.transform.GetChild(i).name.Contains("egg")) {
                    preSpawn.transform.GetChild(i).GetComponent<Renderer>().material.mainTexture = eggTexture;
                }
            }

            for (int i = 0; i < postSpawn.transform.childCount; i++) {
                if (postSpawn.transform.GetChild(i).name.Contains("egg")) {
                    postSpawn.transform.GetChild(i).GetComponent<Renderer>().material.mainTexture = eggTexture;
                }
            }
        }

        postSpawn.SetActive(false);
    }
    
    public void PlayAnimation(int animationIndex, float delay = 0.0f)
    {
        if (bunnyAnimator != null) {
            switch (animationIndex) {
                case 0:
                    preSpawn.SetActive(false);
                    postSpawn.SetActive(true);

                    if (delay == 0.0f) {
                        bunnyAnimator.SetTrigger("bunny_spawn");
                    } else {
                        StartCoroutine(DelayAnimation(delay, "bunny_spawn"));
                    }
                    break;

                case 1:
                    if (delay == 0.0f) {
                        bunnyAnimator.SetTrigger("bunny_attack");
                    } else {
                        StartCoroutine(DelayAnimation(delay, "bunny_attack"));
                    }
                    break;

                case 2:
                    if (delay == 0.0f) {
                        bunnyAnimator.SetTrigger("bunny_flinch");
                    } else {
                        StartCoroutine(DelayAnimation(delay, "bunny_flinch"));
                    }
                    break;
            }
        }
    }

    private IEnumerator DelayAnimation(float delay, string animationName)
    {
        yield return new WaitForSeconds(delay);
        bunnyAnimator.SetTrigger(animationName);
        yield return null;
    }

    public IEnumerator Die(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
        yield return null;
    }
}
