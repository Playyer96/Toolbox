using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Toolbox.UI
{
    public class UIEventSystemHandler : MonoBehaviour
    {
        [Header("References")] public List<Selectable> selectables = new List<Selectable>();

        [Header("Animation")] [SerializeField] private float selectedAnimationScale = 1.1f;
        [SerializeField] private float scaleDuration = 0.25f;

        protected Dictionary<Selectable, Vector3> Scales = new Dictionary<Selectable, Vector3>();

        protected virtual void AddSelectionListeners(Selectable selectable)
        {
            // Add Listeners
            EventTrigger eventTrigger = selectable.GetComponent<EventTrigger>();
            if (eventTrigger == null)
            {
                eventTrigger = selectable.gameObject.AddComponent<EventTrigger>();
            }

            // Add Events
            EventTrigger.Entry SelectEntry = new EventTrigger.Entry()
            {
                eventID = EventTriggerType.Select
            };
            SelectEntry.callback.AddListener(OnSelect);
            eventTrigger.triggers.Add(SelectEntry);

            EventTrigger.Entry DeselectEntry = new EventTrigger.Entry()
            {
                eventID = EventTriggerType.Deselect
            };
            DeselectEntry.callback.AddListener(OnDeselect);
            eventTrigger.triggers.Add(DeselectEntry);
        }

        private void OnDeselect(BaseEventData eventData)
        {
     
        }

        private void OnSelect(BaseEventData eventData)
        {
            throw new System.NotImplementedException();
        }
    }
}