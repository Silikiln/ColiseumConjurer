using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {
    [SerializeField]
    private int BaseHealth = 1;

    [SerializeField]
    private int BaseDamage = 1;

    public int Health { get; private set; }

    protected virtual void Start()
    {
        Health = BaseHealth;
        HealthModifier = _healthModifier;
    }

    [SerializeField]
    private float _healthModifier = 1;
    public float HealthModifier
    {
        set
        {
            Health = (int)(Health * (value / _healthModifier));
            _healthModifier = value;
        }

        get { return _healthModifier; }
    }
    public float DamageReceivedPercent = 1;
    public float DamageDealtPercent = 1;
    public float HealModifier = 1;

    public int Damage { get { return (int)(BaseDamage * DamageDealtPercent); } }
    public int MaxHealth { get { return (int)(BaseHealth * HealthModifier); } }

    public void Hurt(int damage)
    {
        damage = (int)(damage * DamageReceivedPercent);
        if ((Health -= damage) <= 0)
            Destroy(gameObject);
    }

    public void Heal(int amount)
    {
        amount = (int)(amount * HealModifier);
        if ((Health += amount) > MaxHealth)
            Health = MaxHealth;
    }
}
