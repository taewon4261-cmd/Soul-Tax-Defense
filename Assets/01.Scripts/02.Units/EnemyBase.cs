using Unity.VisualScripting;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public EnemyDataSO data;
    public float speed;

    [SerializeField] private int hp;

    private float currentAttackCooldown = 0f;
    private bool isAttacking;

    private bool isDead;

    public LayerMask unitLayer;


    private void Awake()
    {
       if(data != null) hp = data.maxHp;
        speed = Random.Range(0.5f, 1.5f);
    }


    public void TakeDamage(int dmg)
    {
        if (isDead) return;

        hp -= dmg;
        if (hp <= 0)
        {
            Die(true);
        }
    }
    void Die(bool giveReward)
    {
        if (isDead) return;

        isDead = true;

        if (WaveManager.Instance != null)
        {
            WaveManager.Instance.OnEnemyKilled();
        }

        if (giveReward && GameManager.Instance != null)
        {
            GameManager.Instance.AddGold(data.goldReward);
        }
        Destroy(gameObject);
    }
            
        


    private void Update()
    {
        if (data == null) return;

        currentAttackCooldown -= Time.deltaTime;
        DetectAndAttackUnit();

        if (isAttacking == false)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
        if (transform.position.x < -10f) Destroy(gameObject);
    }

    void DetectAndAttackUnit()
    {
        float range = 0.6f;

       
        Debug.DrawRay(transform.position, Vector2.left * range, Color.red);

        Vector2 rayStart = (Vector2)transform.position + (Vector2.left * 0.3f);

        RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector2.left, range, unitLayer);

        if (hit.collider != null)
        {
            isAttacking = true;

            if (currentAttackCooldown <= 0)
            {
                UnitBase unit = hit.collider.GetComponent<UnitBase>();
                if (unit != null)
                {
                    Debug.Log($"적 -> 유닛({unit.name}) 공격!");
                    unit.TakeDamage(data.atk);
                    currentAttackCooldown = 2f;
                }

                Wall wall = hit.collider.GetComponent<Wall>();
                if (wall != null)
                {
                    wall.TakeDamage(data.atk);
                    currentAttackCooldown = 2f;
                    Debug.Log("성벽 공격 중...");
                }
            }
        }
        else
        {
            // 앞에 아무도 없으면 -> 다시 걷기
            isAttacking = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;
 
        Wall wall = collision.gameObject.GetComponent<Wall>();
       
        if (wall != null)
        {
            wall.TakeDamage(data.atk);
            Die(false);
        }
       
    }
}

