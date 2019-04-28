using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int damage;
    public float attackSpeed = 5f;
    public float speed = 5f;
    public float attackRange = 1f;

    [Header("Health")]
    public int health;
    public int healthOnDeath = 1;
    public GameObject healthContainer;
    public GameObject healthBarPrefab;
    public GameObject floatingTextPrefab;
    public GameObject floatingHealthTextPrefab;

    public Sprite forceIcon;
    public Sprite burnIcon;
    public Sprite shockIcon;

    public Weakness weakness;

    float lastAttack;
    bool isAttacking;

    Wizard wizard;
    GameController gameController;

    private void Start()
    {
        wizard = FindObjectOfType<Wizard>();
        gameController = FindObjectOfType<GameController>();

        // Give random weakness
        int randWeakness = Random.Range(0, 3);
        SpriteRenderer weaknessIcon = transform.Find("weakness").GetComponent<SpriteRenderer>();
        switch(randWeakness)
        {
            case 0:
                weakness = new Weakness(DamageType.Force);
                weaknessIcon.sprite = forceIcon;
                break;
            case 1:
                weakness = new Weakness(DamageType.Burn);
                weaknessIcon.sprite = burnIcon;
                break;
            case 2:
                weakness = new Weakness(DamageType.Shock);
                weaknessIcon.sprite = shockIcon;
                break;
        }
    }

    void Update()
    {
        Move();
        HealthCheck();
    }

    void Move()
    {
        
        if (!isAttacking && InAttackRange())
        {
            isAttacking = true;
            StartCoroutine(ApplyDamage());

        } else if(!InAttackRange())
        {
            transform.position = Vector3.MoveTowards(transform.position, wizard.transform.position, speed * Time.deltaTime);
        }
        
    }

    void HealthCheck()
    {
        if (health <= 0)
        {
            wizard.health += healthOnDeath;
            CreateFloatingHealth(healthOnDeath, transform.position);
            Debug.Log("DEAD: +"+ healthOnDeath.ToString() + " | " + wizard.health.ToString() + "/" + wizard.maxHealth.ToString());
            if (wizard.health > wizard.maxHealth) wizard.health = wizard.maxHealth;
            wizard.score += 1;
            wizard.scoreText.text = wizard.score.ToString();
            Camera.main.gameObject.GetComponent<CameraShake>().shakeDuration = 0.2f;
            // Play death anim?
            Destroy(gameObject);
        } else
        {
            // Update health bar
            foreach (Transform child in healthContainer.transform)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < health; i++)
            {
                Instantiate(healthBarPrefab, healthContainer.transform);
            }
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        CreateFloatingDamage(amount, transform.position);
        HealthCheck();
    }

    void CreateFloatingDamage(int damage, Vector3 position)
    {
        GameObject clone = Instantiate(floatingTextPrefab, position, Quaternion.identity);
        clone.GetComponent<FloatingText>().Setup(damage);
    }

    void CreateFloatingHealth(int damage, Vector3 position)
    {
        GameObject clone = Instantiate(floatingHealthTextPrefab, position, Quaternion.identity);
        clone.GetComponent<FloatingHealthText>().Setup(damage);
    }

    bool InAttackRange()
    {
        return Vector3.Distance(transform.position, wizard.transform.position) < attackRange;
    }

    IEnumerator ApplyDamage()
    {
        

        while (Time.time > lastAttack)
        {
            lastAttack = Time.time + attackSpeed;
            // Play attack animation
            wizard.TakeHardDamage(damage);
            yield return null;
        }

        isAttacking = false;


    }
}

public class Weakness
{
    public DamageType type;
    public Weakness(DamageType type)
    {
        this.type = type;
    }
}

public class Resistance
{
    public DamageType type;
    public Resistance(DamageType type)
    {
        this.type = type;
    }
}

public enum DamageType
{
    None,
    Force,
    Shock,
    Burn
}
