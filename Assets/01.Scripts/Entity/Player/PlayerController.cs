using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool _canParry;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //투사체의 후딜을 짧게
    //근접 몬스터의 공격은 후딜을 길게, 넉백 존재
    private void Parry()
    {
        //마우스 포인터 방향에 따라 부채꼴로 패링되게
        //원 콜라이더 소환하여 닿는 오브젝트 판별
        //RaycastHit2D hit = ;
        //마우스 포인터와 플레이어 간의 방향 확인

        //해당 각도의 보정, 닿았을 경우에 패링 판정 넣기
        //foreach문으로 확인

        ParryCool(.5f);
    }

    private void ParryEffect()
    {

    }

    IEnumerator ParryCool(float time)
    {
        _canParry = false;
        yield return new WaitForSeconds(time);
        _canParry = true;
    }
}
