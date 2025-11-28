using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // 기존 변수 (순간 이동 관련 변수들은 사용하지 않음)
    // private Vector2 targetPos; // 사용하지 않음
    // public float Yincrement;   // 사용하지 않음
    // public float speed;        // 더 이상 MoveTowards에 사용되지 않음

    // === 물리 기반 이동을 위한 변수 추가/수정 ===
    private Rigidbody2D rb;          // Rigidbody 컴포넌트 레퍼런스
    public float thrustForce = 50f;  // 위로 밀어 올리는 힘의 크기 (유니티 인스펙터에서 조정)

    public float minheight;
    public float maxheight;
    public float health = 3;
    public GameObject effect;
    public Animator camAnim;
    public Text healthDisplay;
    public GameObject gameOver;
    private ScoreManager scoreManager;


    void Start()
    {
        // Rigidbody2D 컴포넌트 가져오기
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component missing on Player object. Physics movement will not work!");
        }

        scoreManager = FindObjectOfType<ScoreManager>();
    }


    void Update()
    {
        // === 1. 게임 로직 및 UI 업데이트 ===

        healthDisplay.text = "HP: " + health.ToString();

        // ⬇️ 새로 추가된 추락 사망 로직 ⬇️
        // minheight가 이제 추락사 데드 존의 경계선 역할을 합니다.
        if (transform.position.y < minheight)
        {
            Debug.Log("Player fell below the minimum safe height. Game Over.");
            health = 0; // 체력을 0으로 설정하여 게임 오버 로직을 실행
        }
        // ⬆️ 새로 추가된 추락 사망 로직 ⬆️


        if (health <= 0)
        {
            if (scoreManager != null)
            {
                scoreManager.EndGameAndDisplayScore();
            }
            gameOver.SetActive(true);
            Destroy(gameObject);
        }

        // ... (키 입력에 따른 시각적 효과 코드는 그대로 유지)
    }

    // FixedUpdate는 물리 계산을 위한 시간 간격으로 실행됩니다.
    void FixedUpdate()
    {
        if (rb == null) return;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            rb.AddForce(Vector2.up * thrustForce, ForceMode2D.Force);
        }

        Vector3 currentPos = transform.position;

        // ⬇️ 기존 경계 제한 로직 수정 ⬇️
        // minheight 경계 제한 로직을 제거하고, maxheight 상한선만 유지합니다.
        if (currentPos.y > maxheight)
        {
            // 속도를 0으로 만들어 상한선을 넘어가지 못하게 막습니다.
            rb.velocity = new Vector2(rb.velocity.x, 0);

            // 위치를 강제로 상한선에 맞춥니다.
            transform.position = new Vector3(
                currentPos.x,
                maxheight, // minheight는 이제 추락사 판정에만 사용
                currentPos.z
            );
        }
        // ⬆️ 기존 경계 제한 로직 수정 ⬆️
    }
    // Player 스크립트의 맨 아래에 추가합니다.

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌한 상대방의 태그가 "Enemy"인지 확인
        if (other.CompareTag("Enemy"))
        {
           
            Instantiate(effect, transform.position, Quaternion.identity);
            if (camAnim != null)
            {
                camAnim.SetTrigger("Shake");
            }

            // 2. 체력 감소
            health--;

            // 3. 충돌한 장애물 오브젝트 파괴
            Destroy(other.gameObject);
        }
    }
}