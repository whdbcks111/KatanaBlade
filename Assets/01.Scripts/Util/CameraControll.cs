using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour
{
    [SerializeField] private Transform _followTarget;
    public bool IsMoveRestricted = false;

    private Vector3 _vel;
    private Vector3 _targetPosition, _shakeOffset;

    private void Awake()
    {
        _targetPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (IsMoveRestricted) RestrictMove();
        else Follow();

        transform.position = _targetPosition + _shakeOffset;
    }

    private void Update()
    {


        if (Input.GetKeyDown(KeyCode.S))
        {
            Shake(0.5f, 0.5f);
        }
    }

    public void Shake(float time, float force)
    {
        StartCoroutine(ShakeRoutine(time, force));
    }

    private IEnumerator ShakeRoutine(float time, float force)
    {
        for(float i = 0; i < time; i += Time.deltaTime)
        {
            yield return null;
            _shakeOffset = new(Random.Range(-force, force), Random.Range(-force, force));
        }
        _shakeOffset = Vector3.zero;
    }

    public void Follow()
    {
        // _followTarget 따라가는 코드 구현, transform.position을 수정하지 말고 _targetPosition을 수정할 것
        _targetPosition = Vector3.SmoothDamp((Vector2)_targetPosition, _followTarget.position, 
            ref _vel, 0.2f) + Vector3.forward * transform.position.z;
    }

    public void RestrictMove()
    {
        // 이동 제한 코드
    }
}
