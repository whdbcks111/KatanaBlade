using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour
{
    [SerializeField] private Transform _followTarget;
    public bool IsMoveRestricted = false;

    private Vector3 _targetPosition, _shakeOffset;

    private void Update()
    {
        if (IsMoveRestricted) RestrictMove();
        else Follow();

        transform.position = _targetPosition + _shakeOffset;
    }

    public void Shake(float time, float force)
    {
        // 카메라 쉐이크 로직 구현 _shakeOffset 변수에 쉐이크 이동값 넣기
    }

    public void Follow()
    {
        // _followTarget 따라가는 코드 구현, transform.position을 수정하지 말고 _targetPosition을 수정할 것
    }

    public void RestrictMove()
    {
        // 이동 제한 코드
    }
}
