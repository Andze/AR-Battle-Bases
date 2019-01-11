using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_Health : MonoBehaviour {

    public int Health = 0;
    public GameObject HealthBar;
    public float BarSpacing = 0.10f;


    private GameObject[] CurrentHealth;

	void Awake ()
    {
        CurrentHealth = new GameObject[Health];        
        for (int i = 0; i < Health; i++)
        {
            CurrentHealth[i] = Instantiate(HealthBar, new Vector3(((this.transform.position.x - (BarSpacing * (Health-1) /2)) + (BarSpacing*i)),0.75f,this.transform.position.z), Quaternion.identity, this.transform);
        }
	}

    public void TakeDamage (int amount)
    {
        Health -= amount;

        if (Health < 0)
            PlayerDead();
        
        for (int i = 0; i < CurrentHealth.Length; i++)
        {
            if (i > Health)
                CurrentHealth[i].gameObject.SetActive(false);
            else
                CurrentHealth[i].gameObject.SetActive(true);
        } 
    }

    void PlayerDead()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
    

    }
}
