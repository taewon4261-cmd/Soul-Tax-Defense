using UnityEngine;
using UnityEngine.UI;

public class HealthBarView : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private Vector3 offset = new Vector3(0, 1.2f, 0);

    private Transform _target;
    private Camera _mainCam;

    public void Initialize(Transform target)
    {
        _target = target;
        _mainCam = Camera.main; // 실제로는 캐싱된 카메라를 쓰는 게 좋음
    }

    // Presenter가 호출할 업데이트 함수
    public void UpdateFill(float ratio)
    {
        if (fillImage != null) fillImage.fillAmount = ratio;
    }

    private void LateUpdate()
    {
        if (_target == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = _target.position + offset;
        transform.rotation = _mainCam.transform.rotation;
    }
}
