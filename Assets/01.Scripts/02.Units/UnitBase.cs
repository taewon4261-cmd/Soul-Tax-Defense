using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.CanvasScaler;



public class UnitBase : MonoBehaviour
{
    public GameObject rangeAttack;
    public Transform firePoint;

    public UnitDataSO data;

    public LayerMask enemyLayer;

    public float hp;

    protected float attackCool;


    public void TakeDamage(int dmg)
    {
        hp -= dmg;

        if (hp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
        
    }


    void Start()
    {
        if (data != null)
        {
            hp = data.maxHp;
            GetComponent<SpriteRenderer>().sprite = data.unitSprite;
            gameObject.name = data.unitName;
        }
    }

    void Attack(EnemyBase target)
    {
        
        if (rangeAttack != null)
        {
            
            GameObject bullet = Instantiate(rangeAttack, transform.position, Quaternion.identity);

            RangeAttack ra = bullet.GetComponent<RangeAttack>();
            if (ra != null)
            {
                ra.Setup(target.transform);
            }
        }
       
        else
        {
            target.TakeDamage(data.attackPower);
        }
    }

    protected virtual void Update()
    {
        if (data == null) return;

        // 1. 쿨타임 감소
        attackCool -= Time.deltaTime;

        // 2. 레이더 감지 (변수 이름: targetCollider)
        Collider2D targetCollider = Physics2D.OverlapCircle(transform.position, data.attackRange, enemyLayer);

        // 3. 적을 찾았고(targetCollider가 있고) + 쿨타임이 끝났다면
        if (targetCollider != null && attackCool <= 0)
        {
            // hit.GetComponent가 아니라 targetCollider.GetComponent로 변경!
            EnemyBase enemy = targetCollider.GetComponent<EnemyBase>();

            if (enemy != null)
            {
                // 공격 실행
                Attack(enemy);

                // 쿨타임 리셋
                attackCool = data.attackSpeed;
            }
        }


    }
    private void OnDrawGizmosSelected()
    {
        if (data != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, data.attackRange);
        }
    }

}




//public Tile CurrentTile { get; set; }

//[SerializeField] protected int hp = 50;

//public void TakeDamage(int dmg)
//{
//    hp -= dmg;
//    if (hp <= 0) Die();
//}

//protected virtual void Die()
//{
//    if (CurrentTile != null)
//    {
//        CurrentTile.Clear();
//        CurrentTile = null;
//    }

//    Destroy(gameObject);
//}