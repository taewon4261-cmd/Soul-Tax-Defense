using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.CanvasScaler;



public class UnitBase : MonoBehaviour
{
    public UnitDataSO data;
    public GameObject healthBarPrefab;

    private HealthModel _healthModel;
    private HealthPresenter _healthPresenter;

    private Tile ownerTile;
    protected float attackCool;

    public GameObject rangeAttack;
    public Transform firePoint;


    public LayerMask enemyLayer;

    private GameObject _healthBarObject; 


    public void TakeDamage(float dmg)
    {
        if (_healthModel == null) return;

        // 모델의 체력을 깎으면, 프레젠터가 감지해서 UI(View)를 바꿉니다.
        _healthModel.ChangeHealth(-dmg);

        // 체력 확인도 모델에게 시킵니다.s
        if (_healthModel.CurrentHp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        _healthPresenter?.Dispose();

        if (_healthBarObject != null)
        {
            Destroy(_healthBarObject);
        }

        if (ownerTile != null) ownerTile.ClearUnit();
        Destroy(gameObject);
    }

    public void SetUp(Tile tile)
    {
        this.ownerTile = tile;
        if (ownerTile != null) ownerTile.SetUnit(this);

        // [MVP 조립] 배치될 때 HP바 생성
        if (data != null && healthBarPrefab != null)
        {
            _healthModel = new HealthModel(data.maxHp * GameManager.Instance.skeletonHpMult);

            _healthBarObject = Instantiate(healthBarPrefab);
            HealthBarView view = _healthBarObject.GetComponent<HealthBarView>();
            view.Initialize(this.transform);

            _healthPresenter = new HealthPresenter(_healthModel, view);
        }
    }


    void Start()
    {
        if (data != null)
        {
         
            GetComponent<SpriteRenderer>().sprite = data.unitSprite;
            gameObject.name = data.unitName;
        }
    }

    void Attack(EnemyBase target)
    {

        if (data != null && data.attackSound != null)
        {
            SoundManager.Instance.PlaySFX(data.attackSound);
        }
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