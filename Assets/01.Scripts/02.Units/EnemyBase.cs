using Unity.VisualScripting;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public EnemyDataSO data;
    public GameObject healthBarPrefab;

    private HealthModel _healthModel;
    private HealthPresenter _healthPresenter;
    public float speed;
    private float currentAttackCooldown = 0f;
    private bool isAttacking;
    private bool isDead;

    public LayerMask unitLayer;

    private GameObject _healthBarObject;

    private void Awake()
    {
        speed = Random.Range(0.5f, 1.5f);
    }
    protected virtual void Start()
    {
        if (data == null) return;
        // 1. 모델 생성
        _healthModel = new HealthModel(data.maxHp);

        // 2. 뷰 생성 (UI 프리팹 생성)
        _healthBarObject = Instantiate(healthBarPrefab);
        HealthBarView healthView = _healthBarObject.GetComponent<HealthBarView>();

        healthView.Initialize(this.transform);
        _healthPresenter = new HealthPresenter(_healthModel, healthView);
    }


    public void TakeDamage(float dmg)
    {
        if (isDead || _healthModel == null) return;

        // 핵심: 이제 hp -= dmg 대신 모델을 수정합니다.
        _healthModel.ChangeHealth(-dmg);

        // 체력 확인도 모델에게 물어봅니다.
        if (_healthModel.CurrentHp <= 0)
        {
            Die(true);
        }
    }
    void Die(bool giveReward)
    {
        if (isDead) return;
        isDead = true;

        // 이벤트 구독 해제 (메모리 누수 방지)
        _healthPresenter?.Dispose();

        if (_healthBarObject != null)
        {
            Destroy(_healthBarObject);
        }

        if (WaveManager.Instance != null) WaveManager.Instance.OnEnemyKilled();
        if (giveReward && GameManager.Instance != null)
            GameManager.Instance.AddGold(Random.Range(5, 10));

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

