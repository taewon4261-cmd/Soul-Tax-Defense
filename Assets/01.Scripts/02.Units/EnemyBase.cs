using Unity.VisualScripting;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public EnemyDataSO data;
    public float speed;

    [SerializeField] private int hp;

    private float currentAttackCooldown = 0f;
    private bool isAttacking;

    public LayerMask unitLayer;


    private void Awake()
    {
       if(data != null) hp = data.maxHp;
        speed = Random.Range(0.5f, 1.5f);
    }


    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        if (hp <= 0)
        {
            Die(true);
        }
    }
    void Die(bool giveReward)
    {
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
        float range = 0.5f;

       
        Debug.DrawRay(transform.position, Vector2.left * range, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, range, unitLayer);

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
        Wall wall = collision.gameObject.GetComponent<Wall>();
       
        if (wall != null)
        {
            wall.TakeDamage(data.atk);
        }
        Die(false);
       
    }
}




//[Header("Stats (나중에 SO로 연결 가능)")]
//public float moveSpeed = 2f;
//public int damage = 5;
//public float attackInterval = 1f;
//public float attackRange = 0.2f;

//[Header("Grid")]
//public Vector2Int currentGridPos; // 지금 적이 있는 칸(시작할 때 세팅)
//private Tile currentTile;

//private float nextAttackTime;
//private UnitBase target;

//private void Start()
//{
//    currentTile = GridManager.Instance.GetTile(currentGridPos);
//    if (currentTile != null)
//        transform.position = currentTile.transform.position;
//}

//private void Update()
//{
//    Tile nextTile = GetNextTile();

//    // 끝(성 도착) 처리
//    if (nextTile == null)
//    {
//        // TODO: 성 데미지 주기/클리어 등
//        return;
//    }

//    // 막혔으면 공격
//    if (nextTile.IsBlocked)
//    {
//        target = nextTile.occupiedUnit;
//        AttackIfPossible();
//        return;
//    }

//    // 안 막혔으면 이동
//    target = null;
//    MoveTo(nextTile);
//}

//Tile GetNextTile()
//{
//    // 오른쪽으로 전진
//    Vector2Int nextPos = currentGridPos + Vector2Int.right;
//    return GridManager.Instance.GetTile(nextPos);
//}

//void MoveTo(Tile nextTile)
//{
//    Vector3 dest = nextTile.transform.position;
//    transform.position = Vector3.MoveTowards(transform.position, dest, moveSpeed * Time.deltaTime);

//    if (Vector3.Distance(transform.position, dest) < 0.01f)
//    {
//        currentTile = nextTile;
//        currentGridPos = currentTile.gridPos;
//    }
//}

//void AttackIfPossible()
//{
//    if (target == null) return;

//    float dist = Vector3.Distance(transform.position, target.transform.position);
//    if (dist > attackRange) return;

//    if (Time.time >= nextAttackTime)
//    {
//        nextAttackTime = Time.time + attackInterval;
//        target.TakeDamage(damage);

//        // 타겟이 죽어서 타일이 비워졌으면 다음 프레임부터 다시 전진함
//    }
//}