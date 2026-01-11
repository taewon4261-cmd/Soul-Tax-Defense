using UnityEngine;

[CreateAssetMenu(fileName = "NewContract", menuName = "SoulTax/Contract Data")]
public class ContractDataSO : ScriptableObject
{
    public string contractName;
    public Sprite icon;
    [TextArea] public string uiDescription; // UI에 표시될 텍스트

    // 실제 발동될 효과 (Inspector에서 드래그 앤 드롭)
    public ContractEffectSO effect;
}