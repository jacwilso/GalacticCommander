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


        [SerializeField]
        Sprite movementIcon;

        //[SerializeField, MinMaxValue(0.01f, 10f)]
        // Vector2 scaleBounds;
        [NonSerialized]
        public Vector3 worldPos;

        static ActionWheel instance;

        // Positioning
        Camera cam;
        PlayerShip ship;
        RectTransform uiElement;
        WorldToCanvas w2c;

        // Segments
        Segmenter segmenter;
        List<bool> availableActions;

        void Awake()
        {
            if (instance != null)
                Debug.LogError("Multiple action selectors");
            instance = this;
        }

        void Start()
        {
            gameObject.SetActive(false);
            cam = Camera.main;
            segmenter = GetComponentInChildren<Segmenter>();

            uiElement = GetComponent<RectTransform>();
            w2c = new WorldToCanvas(GetComponentInParent<Canvas>());
        }

        void Update()
        {
            //float scale = Vector3.SqrMagnitude(cam.transform.position - transform.position);
            //scale = Mathf.Min(Mathf.Max(scaleBounds.x, scale), scaleBounds.y);
            //transform.localScale = scale * Vector3.one;
            uiElement.anchoredPosition = w2c.ConvertWorldToCanvas(worldPos);
        }

        public void Activate(PlayerShip ship)
        {
            this.ship = ship;
            gameObject.SetActive(true);
            availableActions = ship.AvailableActions(ship.CurrentAP);
            var icons = ship.GetIcons();
            icons.Insert(0, movementIcon);
            segmenter.SetSegments(icons, availableActions);
            uiElement.anchoredPosition = w2c.ConvertWorldToCanvas(worldPos);
        }

        public void Select()
        {
            float angle = 2f * Mathf.PI / segmenter.Segments;
            Vector2 inputPos = ARCursor.Instance.InputPosition;
            inputPos -= 0.5f * new Vector2(Screen.width, Screen.height);
            inputPos -= uiElement.anchoredPosition;
            float offset = 0.5f * angle * (segmenter.Segments % 2);
            float inputAngle = Mathf.Repeat(Mathf.PI - Mathf.Atan2(inputPos.y, -inputPos.x) + offset, 2 * Mathf.PI);
            int segment = (int)(inputAngle / angle);
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
