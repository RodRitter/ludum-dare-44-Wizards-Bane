using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wizard : MonoBehaviour
{
    public int health = 10;
    public int maxHealth = 10;

    public Spell[] spells = new Spell[3];
    public int currentSpell = 0;

    public int score = 0;

    [Header("Spells")]
    public Text forceSpellControl;
    public Text burnSpellControl;
    public Text shockSpellControl;
    public GameObject forceParticles;
    public GameObject burnParticles;
    public GameObject shockParticles;

    [Header("Health")]
    public GameObject healthChunkPrefab;
    public GameObject healthBar;
    public GameObject floatingSpellCost;

    [Header("Score")]
    public Text scoreText;

    [Header("Particles")]
    public ParticleSystem lifeDrain;

    private void Start()
    {
        spells[0] = new Spell("Force", DamageType.Force);
        spells[1] = new Spell("Burn", DamageType.Burn);
        spells[2] = new Spell("Shock", DamageType.Shock);

        forceSpellControl.color = new Color(255, 215, 0);
        burnSpellControl.color = new Color(255, 255, 255);
        shockSpellControl.color = new Color(255, 255, 255);
        currentSpell = 0;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            forceSpellControl.color = new Color(255, 215, 0);
            burnSpellControl.color = new Color(255,255,255);
            shockSpellControl.color = new Color(255,255,255);
            currentSpell = 0;
        } else if (Input.GetKeyDown(KeyCode.W))
        {
            forceSpellControl.color = new Color(255,255,255);
            burnSpellControl.color = new Color(255, 215, 0);
            shockSpellControl.color = new Color(255,255,255);
            currentSpell = 1;
        } else if (Input.GetKeyDown(KeyCode.E))
        {
            forceSpellControl.color = new Color(255,255,255);
            burnSpellControl.color = new Color(255,255,255);
            shockSpellControl.color = new Color(255, 215, 0);
            currentSpell = 2;
        }

        if(Input.GetMouseButtonDown(0))
        {
            bool gamePaused = FindObjectOfType<GameController>().isPaused;

            if(!gamePaused)
            {
                GetComponent<Animator>().SetTrigger("Cast");
                GameObject currentParticle = forceParticles;

                switch (currentSpell)
                {
                    case 0:
                        currentParticle = forceParticles;
                        break;
                    case 1:
                        currentParticle = burnParticles;
                        break;
                    case 2:
                        currentParticle = shockParticles;
                        break;
                }

                Instantiate(currentParticle, Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 0.5f), Quaternion.identity);
                TakeHardDamage(spells[currentSpell].spellCost);

                CreateFloatingSpellCost(spells[currentSpell].spellCost, transform.position + new Vector3(1f, 0, 0));
            }


            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if(hit.collider != null)
            {
                if(hit.collider.tag == "Monster")
                {
                    Monster monster = hit.collider.GetComponent<Monster>();
                    DamageType currentDmg = spells[currentSpell].damageType;
                    DamageType monsterWeakness = monster.weakness.type;

                    if (currentDmg == monsterWeakness)
                    {
                        // +1 damage
                        int damage = spells[currentSpell].damage + 1;
                        monster.TakeDamage(damage);
                    }
                    else
                    {
                        int damage = spells[currentSpell].damage;
                        monster.TakeDamage(damage);
                    }

                    if(monster.health <= 0)
                    {
                        lifeDrain.Clear();
                        lifeDrain.Play();
                    }
                }
                
            }
        }
    }

    void CreateFloatingSpellCost(int damage, Vector3 position)
    {
        GameObject clone = Instantiate(floatingSpellCost, position, Quaternion.identity);
        clone.GetComponent<FloatingHealthDownText>().Setup(damage);
    }

    public void TakeHardDamage(int amount)
    {
        health -= amount;
        HealthCheck();
    }



    void HealthCheck()
    {
        if(healthBar.transform.childCount != health)
        {
            foreach(Transform child in healthBar.transform)
            {
                Destroy(child.gameObject);
            }
            for (int i = 0; i < health; i++)
            {
                Instantiate(healthChunkPrefab, healthBar.transform);
            }

            if(healthBar.transform.childCount < health)
            {
                // Add Health anim
            }

            if (healthBar.transform.childCount > health)
            {
                // REmove Health anim
            }
        }

        if (health <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        FindObjectOfType<GameController>().EndGame();
    }
    
}

public class Spell
{
    public string name;
    public DamageType damageType;
    public int spellCost = 1;
    public int damage = 1;

    public Spell(string name, DamageType damageType)
    {
        this.name = name;
        this.damageType = damageType;
    }
}

