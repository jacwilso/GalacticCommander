using System;
using UnityEngine;

public class ConfirmationUI : MonoBehaviour
{
    public event Action ConfirmEvent, CancelEvent;

    public static ConfirmationUI Instance => instance;

    [SerializeField]
    GameObject confirnBtn, cancelBtn, turnBtn;

    static ConfirmationUI instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Multiple ConfirmationUI");
        }
        instance = this;
    }

    void Start()
    {
        Activate(false);
    }

    public void Activate(bool isActive, bool cancelActive = false)
    {
        confirnBtn.gameObject.SetActive(isActive);
        cancelBtn.gameObject.SetActive(isActive || cancelActive);
    }

    public void Confirm()
    {
        ConfirmEvent?.Invoke();
        Activate(false);
        turnBtn.gameObject.SetActive(true);
    }

    public void Cancel()
    {
        CancelEvent?.Invoke();
        Activate(false);
        turnBtn.gameObject.SetActive(true);
    }

    public void TurnAction()
    {
        turnBtn.gameObject.SetActive(false);
    }
}
