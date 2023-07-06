using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

// 게임 오버 상태를 표현하고, 게임 점수와 UI를 관리하는 게임 매니저
// 씬에는 단 하나의 게임 매니저만 존재할 수 있다.
public class GameManager : MonoBehaviour
{
    public static GameManager instance; // 싱글톤을 할당할 전역 변수

    public bool IsGameover = false; // 게임 오버 상태
    public TextMeshProUGUI ScoreText; // 점수를 출력할 UI 텍스트
    public GameObject GameoverUI; // 게임 오버시 활성화 할 UI 게임 오브젝트
    public float LimitTime;
    public TextMeshProUGUI TimerText;

    private int _score = 0; // 게임 점수
    private float _timer = 0; // 시간 변수

    // 게임 시작과 동시에 싱글톤을 구성
    void Awake()
    {
        // 싱글톤 변수 instance가 비어있는경우
        if (instance == null)
        {
            // instance가 비어있다면(null) 그곳에 자기 자신을 할당
            instance = this;
        }
        else
        {
            // instance에 이미 다른 GameManager 오브젝트가 할당되어 있는 경우
            // 씬에 두개 이상의 GameManager 오브젝트가 존재한다는 의미.
            // 싱글톤 오브젝트는 하나만 존재해야 하므로 자신의 게임 오브젝트를 파괴
            Debug.LogWarning("씬에 두개 이상의 게임 매니저가 존재합니다!");
            Destroy(gameObject);
        }

        Screen.SetResolution(1920, 1080, true);
    }

    //시간 증가시키는 메서드 
    void Start()
    {
        TimerText = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();
        StartCoroutine(StartTimer());
    }

    private IEnumerator StartTimer()
    {
        _timer = 0;
        while (true)
        {
            _timer += Time.deltaTime;
            TimerText.text = "시간 : " + Mathf.Round(_timer);
            yield return null;
        }
    }

    void Update()
    {
        // 게임 오버 상태에서 게임을 재시작할 수 있게 하는 처리
        if (IsGameover && Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
    }
    // 플레이어 캐릭터가 사망시 게임 오버를 실행하는 메서드
    public void OnPlayerDead()
    {
        IsGameover = true;
        GameoverUI.SetActive(true);
    }



    /*점수를 증가시키는 메서드
    public void AddScore(int newScore)
    {
        if (isGameover)
        {
            _score += newScore;
            ScoreText.text = "Score" + _score;  
        }
    }*/
}

