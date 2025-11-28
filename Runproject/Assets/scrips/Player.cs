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
        // === 게임 로직 및 UI 업데이트 (물리 계산 외의 것) ===

        healthDisplay.text = "HP: " + health.ToString();

        if (health <= 0)
        {
            if (scoreManager != null)
            {
                scoreManager.EndGameAndDisplayScore();
            }
            gameOver.SetActive(true);
            Destroy(gameObject);
        }

        // === 키 입력에 따른 시각적 효과 (GetKeyDown을 사용해 한 번만 실행) ===
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            Instantiate(effect, transform.position, Quaternion.identity);
            camAnim.SetTrigger("Shake");
        }

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

        if (currentPos.y > maxheight || currentPos.y < minheight)
        {
            // 속도를 0으로 만들어 경계를 넘어가려는 움직임을 막습니다.
            rb.velocity = new Vector2(rb.velocity.x, 0);

            // 위치를 강제로 경계선에 맞춥니다.
            transform.position = new Vector3(
                currentPos.x,
                Mathf.Clamp(currentPos.y, minheight, maxheight),
                currentPos.z
            );
        }
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