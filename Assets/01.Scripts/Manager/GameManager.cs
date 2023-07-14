using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

// ���� ���� ���¸� ǥ���ϰ�, ���� ������ UI�� �����ϴ� ���� �Ŵ���
// ������ �� �ϳ��� ���� �Ŵ����� ������ �� �ִ�.
public class GameManager : MonoBehaviour
{
    public static GameManager instance; // �̱����� �Ҵ��� ���� ����

    public bool IsGameover = false; // ���� ���� ����
    public TextMeshProUGUI ScoreText; // ������ ����� UI �ؽ�Ʈ
    public GameObject GameoverUI; // ���� ������ Ȱ��ȭ �� UI ���� ������Ʈ
    public float LimitTime;
    public TextMeshProUGUI TimerText;
    public GameObject InventoryUI;
    public Canvas Canvas;
    public GameObject PauseUI;
    public GameObject EffectPrefab;
    public Transform EffectIconContainer;

    [Header("ItemPopup")]
    public GameObject ItemPopup;
    public Image ItemPopupIcon;
    public TextMeshProUGUI ItemPopupName, ItemPopupDesc;

    [Header("ShopPopup")]
    public GameObject ShopPopup;
    public Image ShopPopupIcon;
    public TextMeshProUGUI ShopPopupName, ShopPopupDesc;
    public GameObject ShopPanel;
    public GameObject[] ItemListUI = new GameObject[4];

    private MonoBehaviour _currentShowingUI;
    private GameObject _openedPopup;

    private int _score = 0; // ���� ����
    private float _timer = 0; // �ð� ����

    [HideInInspector] public int Gold = 0;

    [Header("Related to Boss")]
    public Slider BossHPBar;
    public TextMeshProUGUI BossName;
    private Boss _curBoss;
    

    // ���� ���۰� ���ÿ� �̱����� ����
    void Awake()
    {
        // �̱��� ���� instance�� ����ִ°��
        if (instance == null)
        {
            // instance�� ����ִٸ�(null) �װ��� �ڱ� �ڽ��� �Ҵ�
            instance = this;
        }
        else
        {
            // instance�� �̹� �ٸ� GameManager ������Ʈ�� �Ҵ�Ǿ� �ִ� ���
            // ���� �ΰ� �̻��� GameManager ������Ʈ�� �����Ѵٴ� �ǹ�.
            // �̱��� ������Ʈ�� �ϳ��� �����ؾ� �ϹǷ� �ڽ��� ���� ������Ʈ�� �ı�
            Debug.LogWarning("���� �ΰ� �̻��� ���� �Ŵ����� �����մϴ�!");
            Destroy(gameObject);
        }

        Screen.SetResolution(1920, 1080, true);
        //SceneManager.sceneLoaded += OnSceneLoadedEvent;
    }

    private void OnSceneLoadedEvent(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "BossScene")
        {
            _curBoss = FindObjectOfType<Boss>();
        }
    }





    //�ð� ������Ű�� �޼��� 
    void Start()
    {
        StartCoroutine(StartTimer());

        ItemPopup.SetActive(false);
    }

    public void ShowItemPopup(MonoBehaviour ui, Sprite icon, string name, string desc)
    {
        ItemPopup.SetActive(true);
        ItemPopupIcon.sprite = icon;
        ItemPopupDesc.SetText(desc);
        ItemPopupName.SetText(name);
        _currentShowingUI = ui;
    }
    public void HideItemPopup(MonoBehaviour itemIconUI)
    {
        if (_currentShowingUI == itemIconUI)
            ItemPopup.SetActive(false);
    }

    public void ShopItemPopup(GameObject ui, Sprite icon, string name, string desc)
    {
        ShopPopup.SetActive(true);
        ShopPopupIcon.sprite = icon;
        ShopPopupDesc.SetText(desc);
        ShopPopupName.SetText(name);
        //_currentShowingUI = ui;
    }

    public void HideShopPopup(GameObject shopIconUI)
    {
        //if (_currentShowingUI == shopIconUI)
            ShopPopup.SetActive(false);
    }

    public EquipPopup CreateEquipPopup()
    {
        if (_openedPopup != null) Destroy(_openedPopup);
        var position = Input.mousePosition;
        var popup = Instantiate(Resources.Load<EquipPopup>("UI/EquipPopup"), Canvas.transform);
        popup.transform.position = position;
        _openedPopup = popup.gameObject;

        return popup;
    }

    public void ClosePopup()
    {
        Destroy(_openedPopup);
        _openedPopup = null;
    }

    public UnequipPopup CreateUnequipPopup()
    {
        if (_openedPopup != null) Destroy(_openedPopup);
        var position = Input.mousePosition;
        var popup = Instantiate(Resources.Load<UnequipPopup>("UI/UnequipPopup"), Canvas.transform);
        popup.transform.position = position;
        _openedPopup = popup.gameObject;

        return popup;
    }

    private IEnumerator StartTimer()
    {
        _timer = 0;
        while (true)
        {
            _timer += Time.deltaTime;
            TimerText.text = "�ð� : " + Mathf.Round(_timer);
            yield return null;
        }
    }

    void Update()
    {
        // ���� ���� ���¿��� ������ ������� �� �ְ� �ϴ� ó��
        if (IsGameover && Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        // �κ��丮 Ű�ٿ� ���
        if (Input.GetKeyDown(KeyCode.I))
        {
            print("���");
            InventoryUI.SetActive(!InventoryUI.activeSelf);
        }

        var effectUICount = EffectIconContainer.childCount;
        var effects = Player.Instance.Effects;

        if(effectUICount > effects.Length)
        {
            DestroyImmediate(EffectIconContainer.GetChild(effectUICount - 1).gameObject);
        }
        else if(effectUICount < effects.Length)
        {
            Instantiate(EffectPrefab, EffectIconContainer);
        }

        int index = 0;
        foreach(Transform child in EffectIconContainer)
        {
            if(child.GetChild(0).TryGetComponent(out Image image))
            {
                print(index + "dadsd");
                image.sprite = effects[index].Icon;
                image.fillAmount = effects[index].Duration / effects[index].MaxDuration;
            }
            ++index;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale > 0f)
            {
                Time.timeScale = 0f;
                PauseUI.SetActive(true);
            }

            else
            {
                Time.timeScale = 1f;
                PauseUI.SetActive(false);
            }
        }



        if (_curBoss is not null)
        {
            BossHPBar.gameObject.SetActive(true);
            BossName.gameObject.SetActive(true);
            BossHPBar.value = _curBoss.HP / _curBoss.MaxHP;
            BossName.text = _curBoss.gameObject.name;
            
        }
        else
        {
            BossHPBar.gameObject.SetActive(false);
            BossName.gameObject.SetActive(false);
        }
    }
    // �÷��̾� ĳ���Ͱ� ����� ���� ������ �����ϴ� �޼���
    public void OnPlayerDead()
    {

        IsGameover = true;
        GameoverUI.SetActive(true);
    }
    public void GameExit()
    {
        Application.Quit();
    }


    /*������ ������Ű�� �޼���
    public void AddScore(int newScore)
    {
        if (isGameover)
        {
            _score += newScore;
            ScoreText.text = "Score" + _score;  
        }
    }*/
}

