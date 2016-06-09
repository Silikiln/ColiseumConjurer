using UnityEngine;
using System.Collections;
using System;

using Random = UnityEngine.Random;

public class Dodgeball : Trial
{
    static float timeBetweenShots = .33f;
    static float projectileSpeed = 300;
    static float aimError = .5f;

    public Dodgeball()
    {
        Name = "Dodgeball";
        Description = "Avoid the flying objects";

        BaseObjectCount = 10;

        projectilePrefab = LoadResource<GameObject>("DodgeballProjectile");
    }

    int projectilesLaunched = 0;
    GameObject projectilePrefab;
    IEnumerator spawning;

    public void ProjectileHit(bool hitPlayer)
    {
        if (hitPlayer)
        {
            TrialHandler.Instance.EventFailed();
            return;
        }

        projectilesLaunched++;
        if (RequirementsMet)
            TrialHandler.Instance.EventFinished();
    }

    IEnumerator SpawnProjectiles()
    {
        for (int i = 0; i < ObjectCount; i++)
        {
            GameObject projectile = Instantiate(projectilePrefab);
            projectile.transform.parent = transform;

            Vector2 position = (Vector2)PlayerController.Instance.transform.position 
                + PlayerController.Instance.GetComponent<Rigidbody2D>().velocity / 2; ;
            float angle = Mathf.Atan2(position.y, position.x) + aimError * Random.Range(-1f, 1f);
            position = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            projectile.GetComponent<Rigidbody2D>().AddForce(position.normalized * projectileSpeed);
            
            //Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), GetComponent<Collider2D>());

            yield return new WaitForSeconds(timeBetweenShots);
        }
    }

    public override void Setup()
    {
        spawning = SpawnProjectiles();
        StartCoroutine(spawning);
    }

    public override void Cleanup()
    {
        StopCoroutine(spawning);
        foreach (Transform t in transform)
            Destroy(t.gameObject);
    }

    public override bool RequirementsMet
    {
        get { return projectilesLaunched == ObjectCount; }
    }
}
