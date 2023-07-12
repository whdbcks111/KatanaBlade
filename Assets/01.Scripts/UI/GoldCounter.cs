using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldCounter : MonoBehaviour
{
    public static GoldCounter Instance;
    public int Gold { get; private set; }
    public TextMeshProUGUI GoldText;


    private void Awake()
    {
        if (Instance != null)
            return;

        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        GoldText.text = "Gold : " + Gold.ToString();
    }

    public void IncreaseGolds(int value) => Gold += value;
}
