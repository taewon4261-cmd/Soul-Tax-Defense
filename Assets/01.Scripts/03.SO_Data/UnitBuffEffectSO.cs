using UnityEngine;

[CreateAssetMenu(menuName = "SoulTax/Effects/Unit Buff")]
public class UnitBuffEffectSO : ContractEffectSO
{
    public enum TargetUnit { Skeleton, Imp, Wall, All }
    public enum StatType { Attack, HP, AtkSpeed, DamageReflect }

    public TargetUnit targetUnit;
    public StatType statType;
    public float value; // 예: 0.5 (50%), 10 (고정수치) 등

    public override void ApplyEffect()
    {
        // GameManager 혹은 UpgradeManager를 통해 수치 반영
        // 예: GameManager가 전역 버프 배율을 관리한다고 가정
        GameManager.Instance.ApplyUnitBuff(targetUnit, statType, value);
        Debug.Log($"[계약] {targetUnit}의 {statType}가 {value}만큼 증가했습니다.");
    }
}