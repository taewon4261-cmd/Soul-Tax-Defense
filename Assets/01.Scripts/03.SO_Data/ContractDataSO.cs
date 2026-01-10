
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ContractType
{
    GoldGain,       // 즉시 골드 획득
    UnitAttackBuff, // 아군 공격력 증가
    TaxReduction    // 세금 감소 
}

[CreateAssetMenu(menuName = ("Contract/SO"))]
public class ContractDataSO : ScriptableObject
{
    public string contractName;

    [TextArea]
    public string description;     // 예: 골드를 100 획득합니다.
    public Sprite icon;            // 카드에 들어갈 이미지 (없으면 비워둬도 됨)

    [Header("효과 설정")]
    public ContractType type;      // 효과 종류
    public float value;            // 효과 수치 (예: 100이면 100골드, 5면 공격력 5)
}
