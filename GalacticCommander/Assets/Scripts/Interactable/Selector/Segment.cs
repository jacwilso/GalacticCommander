using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIWheel
{
    public abstract class Segment : MonoBehaviour
    {
        protected void Start()
        {
            GetComponent<Button>().onClick.AddListener(Select);
        }

        public abstract void Select();
    }
}
