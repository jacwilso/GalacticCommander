using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISelectors : MonoBehaviour
{
    public static UISelectors instance;

    [SerializeField]
    private GameObject actionWheel,
        attackWheel;

    private void Awake()
    {
        if (instance != null)
            Debug.LogError("Multiple action selectors");
        instance = this;
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        actionWheel.SetActive(true);
        attackWheel.SetActive(false);
    }

    public void AttackWheel()
    {
        actionWheel.SetActive(false);
        attackWheel.SetActive(true);
    }
}