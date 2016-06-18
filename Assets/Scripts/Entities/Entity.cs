using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {
    [SerializeField]
    private int BaseHealth = 1;

    [SerializeField]
    private int BaseDamage = 1;

    public int Health { get; protected set; }

    protected virtual void Start()
    {
        Health = BaseHealth;
    }

    public float HealthMultiplier = 1;
    public int HealthAdded = 0;

    public float DamageDealtMultiplier = 1;
    public int DamageDealtAdded = 0;

    public float DamageRecievedMultiplier = 1;
    public int DamageRecievedAdded = 0;

    public float HealMultiplier = 1;
    public int HealAdded = 0;

    public float HealthPercent { get { return (float)Health / MaxHealth; } }
    public int Damage { get { return Mathf.Clamp((int)(BaseDamage * DamageDealtMultiplier) + DamageDealtAdded, 0, int.MaxValue); } }
    public int MaxHealth { get { return (int)(BaseHealth * HealthMultiplier + HealthAdded); } }

    public void Hurt(int damage)
    {
        damage = (int)(damage * DamageRecievedMultiplier + DamageRecievedAdded);
        if ((Health -= damage) <= 0)
            Destroy(gameObject);
    }

    public void Heal(int amount)
    {
        amount = (int)(amount * HealMultiplier + HealAdded);
        if ((Health += amount) > MaxHealth)
            Health = MaxHealth;
    }
}
