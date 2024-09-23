using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAtk : MonoBehaviour
{


    /// <summary>
    /// 튀어오르는 공격 자체 처리
    /// </summary>
    /// <param name="argWaitTime">공격 후 기다리는 시간</param>
    /// <param name="argSpeed">공격 속도</param>
    /// <param name="argMaxHeight">최대 공격 높이</param>
    public void PopAtk(float argWaitTime, float argSpeed, float argMaxHeight)
    {
        StartCoroutine(IEPopAtk(argWaitTime, argSpeed, argMaxHeight));
    }

    /// <summary>
    /// 튀어오르는 공격 IE 자체 처리
    /// </summary>
    /// <param name="argWaitTime">공격 후 기다리는 시간</param>
    /// <param name="argSpeed">공격 속도</param>
    /// <param name="argMaxHeight">최대 공격 높이</param>
    /// <returns>IE</returns>
    IEnumerator IEPopAtk(float argWaitTime, float argSpeed, float argMaxHeight)
    {
        // 올라가기
        while (true)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                new Vector3(transform.position.x, argMaxHeight, transform.position.z),
                argSpeed * Time.deltaTime);
            if (Mathf.Abs(argMaxHeight - transform.position.y) < 0.01f)
            {
                break;
            }

            yield return null;
        }
        yield return new WaitForSeconds(argWaitTime - 1.0f);

        // 내려가기
        while (true)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                new Vector3(transform.position.x, -10.0f, transform.position.z),
                argSpeed * Time.deltaTime);
            if (Mathf.Abs(-10.0f - transform.position.y) < 0.01f)
            {
                break;
            }

            yield return null;
        }
    }
}
