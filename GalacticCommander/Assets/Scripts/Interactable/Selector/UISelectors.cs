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

    [SerializeField, MinMaxValue(0.01f, 10f)]
    private Vector2 scaleBounds;


    private static UISelectors instance;

    private Camera cam;
    private Segmenter segmenter;

    private void Awake()
    {
        if (instance != null)
            Debug.LogError("Multiple action selectors");
        instance = this;
    }

    private void Start()
    {
        //gameObject.SetActive(false);
        cam = Camera.main;
        segmenter = GetComponentInChildren<Segmenter>();
    }

    private void Update()
    {
        float scale = Vector3.SqrMagnitude(cam.transform.position - transform.position);
        scale = Mathf.Min(Mathf.Max(scaleBounds.x, scale), scaleBounds.y);
        transform.localScale = scale * Vector3.one;
    }

    public void Activate(ShipProperties properties)
    {
        int segments = properties.network.Length +
            properties.weapons.Length +
            properties.engines.Length +
            properties.structure.Length +
            properties.energy.Length +
            properties.personnel.Length;
        segmenter.Segments = segments;
    }
}