using UnityEngine;

// 모든 계약 효과의 최상위 부모
public abstract class ContractEffectSO : ScriptableObject
{
    [TextArea] public string description; // 툴팁/설명용
    public abstract void ApplyEffect();   // 자식들이 구현할 실제 효과
}