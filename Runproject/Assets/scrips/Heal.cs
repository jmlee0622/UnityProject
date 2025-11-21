using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Obstacle : MonoBehaviour
{

    public float healAmount = 2;
    public float speed;

    public GameObject effect; // 이펙트 프리팹 (할당 선택 사항)

    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        if (transform.position.x <= -25)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            // ?? Null 체크 추가: effect 변수에 무언가 할당되어 있을 때만 Instantiate 실행
            if (effect != null)
            {
                Instantiate(effect, transform.position, Quaternion.identity);
            }

            // ?? 체력 회복 로직 (이펙트 생성 여부와 관계없이 실행됨)
            other.GetComponent<Player>().health += healAmount;

            Destroy(gameObject);
        }
    }
}