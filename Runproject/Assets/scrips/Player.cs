using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Vector2 targetPos;
    public float Yincrement;
    public float speed;
    public float minheight;
    public float maxheight;
    public float health = 3;
    public GameObject effect;
    public Animator camAnim;
    public Text healthDisplay;

    public GameObject gameOver;

    // ?? ScoreManager 참조를 위한 변수 추가 (옵션이지만 권장됨)
    private ScoreManager scoreManager;


    void Start()
    {
        // ?? Start 함수에서 ScoreManager 컴포넌트를 찾아 참조합니다.
        // 게임 씬에 ScoreManager 스크립트가 붙은 오브젝트가 하나만 있다고 가정합니다.
        scoreManager = FindObjectOfType<ScoreManager>();
    }


    void Update()
    {
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

        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.UpArrow) && transform.position.y < maxheight)
        {
            Instantiate(effect, transform.position, Quaternion.identity);
            camAnim.SetTrigger("Shake");
            targetPos = new Vector2(transform.position.x, transform.position.y + Yincrement);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && transform.position.y > minheight)
        {
            Instantiate(effect, transform.position, Quaternion.identity);
            camAnim.SetTrigger("Shake");
            targetPos = new Vector2(transform.position.x, transform.position.y - Yincrement);
        }
    }
}