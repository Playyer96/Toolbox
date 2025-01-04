using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Toolbox.UI
{
    public class UnityUIEventSystemHandler : MonoBehaviour
    {
        [Header("References")] public List<Selectable> selectables = new List<Selectable>();
        [SerializeField] protected Selectable firstSelected;

        [Header("Controls")] [SerializeField] protected InputActionReference navigateReference;

        [Header("Animation")] [SerializeField] protected float selectedAnimationScale = 1.1f;
        [SerializeField] protected float scaleDuration = 0.25f;
        [SerializeField] protected List<GameObject> animationExclusions = new List<GameObject>();

        [Header("Sounds")] [SerializeField] protected UnityEvent soundEvent;

        protected Dictionary<Selectable, Vector3> Scales = new Dictionary<Selectable, Vector3>();

        protected Selectable LastSelected;

        protected Tween ScaleUpTween;
        protected Tween ScaleDownTween;

        public virtual void Awake()
        {
            foreach (var selectable in selectables)
            {
                AddSelectionListeners(selectable);
                Scales.Add(selectable, selectable.transform.localScale);
            }
        }

        public virtual void OnEnable()
        {
            navigateReference.action.performed += OnNavigate;

            // Set selectables back to the original size
            for (int i = 0; i < selectables.Count; i++)
            {
                selectables[i].transform.localScale = Scales[selectables[i]];
            }

            StartCoroutine(SelectAfterDelay());
        }

        public virtual void OnDisable()
        {
            navigateReference.action.performed -= OnNavigate;

            ScaleUpTween.Kill(true);
            ScaleDownTween.Kill(true);
        }

        protected virtual IEnumerator SelectAfterDelay()
        {
            yield return null;
            EventSystem.current.SetSelectedGameObject(firstSelected.gameObject);
        }

        protected virtual void AddSelectionListeners(Selectable selectable)
        {
            // Add Listeners
            EventTrigger eventTrigger = selectable.GetComponent<EventTrigger>();
            if (eventTrigger == null)
            {
                eventTrigger = selectable.gameObject.AddComponent<EventTrigger>();
            }

            // Add Events
            EventTrigger.Entry selectEntry = new EventTrigger.Entry()
            {
                eventID = EventTriggerType.Select
            };
            selectEntry.callback.AddListener(OnSelect);
            eventTrigger.triggers.Add(selectEntry);

            EventTrigger.Entry deselectEntry = new EventTrigger.Entry()
            {
                eventID = EventTriggerType.Deselect
            };
            deselectEntry.callback.AddListener(OnDeselect);
            eventTrigger.triggers.Add(deselectEntry);

            EventTrigger.Entry pointerEnter = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerEnter
            };
            pointerEnter.callback.AddListener(OnPointerEnter);
            eventTrigger.triggers.Add(pointerEnter);

            EventTrigger.Entry pointerExit = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerExit
            };
            pointerExit.callback.AddListener(OnPointerExit);
            eventTrigger.triggers.Add(pointerExit);
        }

        private void OnSelect(BaseEventData eventData)
        {
            soundEvent?.Invoke();
            LastSelected = eventData.selectedObject.GetComponent<Selectable>();

            if (animationExclusions.Contains(eventData.selectedObject))
                return;

            Vector3 newScale = eventData.selectedObject.transform.localScale * selectedAnimationScale;
            ScaleUpTween = eventData.selectedObject.transform.DOScale(newScale, scaleDuration);
        }

        private void OnDeselect(BaseEventData eventData)
        {
            if (animationExclusions.Contains(eventData.selectedObject))
                return;

            Selectable selectable = eventData.selectedObject.GetComponent<Selectable>();
            ScaleDownTween = eventData.selectedObject.transform.DOScale(Vector3.one, scaleDuration);
        }

        public void OnPointerEnter(BaseEventData eventData)
        {
            PointerEventData pointerEventData = eventData as PointerEventData;
            if (pointerEventData != null)
            {
                Selectable selectable = pointerEventData.pointerEnter.GetComponentInParent<Selectable>();
                if (selectable == null)
                {
                    selectable = pointerEventData.pointerEnter.GetComponentInChildren<Selectable>();
                }

                pointerEventData.selectedObject = selectable.gameObject;
            }
        }

        public void OnPointerExit(BaseEventData eventData)
        {
            PointerEventData pointerEventData = eventData as PointerEventData;
            if (pointerEventData != null)
            {
                pointerEventData.selectedObject = null;
            }
        }

        protected virtual void OnNavigate(InputAction.CallbackContext context)
        {
            EventSystem.current.SetSelectedGameObject(LastSelected.gameObject);
        }
    }
}