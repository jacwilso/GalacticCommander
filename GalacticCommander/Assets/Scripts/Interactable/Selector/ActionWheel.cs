using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UIWheel
{
    public class ActionWheel : MonoBehaviour, IInteractable
    {
        public static ActionWheel Instance
        {
            get { return instance; }
        }

        //[SerializeField, MinMaxValue(0.01f, 10f)]
        //private Vector2 scaleBounds;
        [NonSerialized]
        public Vector3 worldPos;

        private static ActionWheel instance;

        // Positioning
        private Camera cam;
        private PlayerShip ship;
        private RectTransform uiElement;
        private WorldToCanvas w2c;

        // Segments
        private Segmenter segmenter;
        private List<bool> availableActions;

        private void Awake()
        {
            if (instance != null)
                Debug.LogError("Multiple action selectors");
            instance = this;
        }

        private void Start()
        {
            gameObject.SetActive(false);
            cam = Camera.main;
            segmenter = GetComponentInChildren<Segmenter>();

            uiElement = GetComponent<RectTransform>();
            w2c = new WorldToCanvas(GetComponentInParent<Canvas>());
        }

        private void Update()
        {
            //float scale = Vector3.SqrMagnitude(cam.transform.position - transform.position);
            //scale = Mathf.Min(Mathf.Max(scaleBounds.x, scale), scaleBounds.y);
            //transform.localScale = scale * Vector3.one;
            uiElement.anchoredPosition = w2c.ConvertWorldToCanvas(worldPos);
        }

        public void Activate(PlayerShip ship, ShipProperties properties)
        {
            this.ship = ship;
            gameObject.SetActive(true);
            availableActions = properties.AvailableActions(ship.TurnAP);
            segmenter.SetSegments(properties.GetIcons(), availableActions);
            uiElement.anchoredPosition = w2c.ConvertWorldToCanvas(worldPos);
        }

        public void Select()
        {
            float angle = 2f * Mathf.PI / segmenter.Segments;
            Vector2 inputPos = ARCursor.Instance.InputPosition;
            inputPos -= 0.5f * new Vector2(Screen.width, Screen.height);
            float inputAngle = Mathf.Atan2(inputPos.y, inputPos.x) - Mathf.PI;
            int segment = (int)(-inputAngle / angle);
            if (availableActions[segment])
            {
                gameObject.SetActive(false);
                ship.Action(segment);
            }
        }

        public void Deselect()
        {
            gameObject.SetActive(false);
        }
    }
}
