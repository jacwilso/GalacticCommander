using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISelectors : MonoBehaviour
{
    public static UISelectors Instance
    {
        get { return instance; }
    }

    [SerializeField]
    private GameObject actionWheel;

    private static UISelectors instance;

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
    }

    public void ToggleActionWheel()
    {
        actionWheel.SetActive(!actionWheel.activeSelf);
    }
}