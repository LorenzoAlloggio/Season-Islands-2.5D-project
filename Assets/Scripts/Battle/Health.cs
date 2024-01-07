using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Health : MonoBehaviour
{
    [SerializeField] private int health = 100;

    public GameObject FloatingTextPrefab;

    private int MAX_HEALTH = 100;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            //Damage(10);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            //Heal(10);
        }
    }

    public void Damage(int amount)
    {
        if(amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative Damage");
        }

        this.health -= amount;

        if(FloatingTextPrefab)
        {
            ShowFloatingText();
        }


        if(health <= 0)
        {
            Die();
        }
    }

    void ShowFloatingText()
    {
        var go = Instantiate(FloatingTextPrefab, transform.position, Quaternion.identity, transform);
        go.GetComponent<TMP_Text>().text = health.ToString();
    }

    public void Heal(int amount)
    {
        if (amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative healing");
        }

        bool wouldBeOverMaxHealth = health + amount > MAX_HEALTH;

        if(wouldBeOverMaxHealth)
        {
            this.health = MAX_HEALTH;
        }
        else
        {
            this.health += amount;
        }
    }

    private void Die()
    {
        Debug.Log("I am dead!");
        Destroy(gameObject);
    }
}
