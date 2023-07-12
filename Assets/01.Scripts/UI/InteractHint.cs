using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class InteractHint : MonoBehaviour
{
    public static readonly Color[] Colors =
    {
        new Color(1f, 0f, 0.2f),
        new Color(1f, 1f, 0f),
        new Color(0.2f, 1f, 0f),
        new Color(1f, 0f, 1f),
        new Color(0.3f, 0f, 1f),
        new Color(0f, 1f, 1f),
        new Color(0f, 1f, 0.7f),
    };
    [HideInInspector] public float InteractDistance; 

    private TextMeshProUGUI _text;
    private float _timer = 0f;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if(Player.Instance != null)
        {
            var dist = (Player.Instance.transform.position - transform.position).magnitude;
            var alpha = 1 - Mathf.Clamp01((dist - InteractDistance) / 10);

            if (dist < InteractDistance)
            {
                if((_timer -= Time.deltaTime) < 0f)
                {
                    _text.color = Colors[Random.Range(0, Colors.Length)];
                    _timer += 0.1f;
                }
            }
            else
            {
                _text.color = Color.white;
            }
            _text.alpha = alpha;
        }
    }
}
