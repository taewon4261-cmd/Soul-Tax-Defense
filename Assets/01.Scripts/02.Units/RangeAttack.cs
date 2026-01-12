using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : MonoBehaviour
{
    public UnitDataSO data;
    private int damage;
    private float speed = 5f;

    public void Setup(Transform target)
    {
        if (data != null)
        {
            damage = data.attackPower;
        }
        Vector3 direction = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Ã·º¸´Â°Å...

        transform.rotation = Quaternion.Euler(0, 0, angle);

        Destroy(gameObject,3f);
    }

    private void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime, Space.Self);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyBase enemy = collision.gameObject.GetComponent<EnemyBase>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
