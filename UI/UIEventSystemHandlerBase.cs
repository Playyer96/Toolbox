using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Toolbox.UI
{
    public abstract class UIEventSystemHandlerBase : MonoBehaviour
    {
        [Header("Controls")] [SerializeField] protected InputActionReference navigateReference;

        [Header("Animation")] [SerializeField] protected float selectedAnimationScale = 1.1f;
        [SerializeField] protected float scaleDuration = 0.25f;

        [Header("Sounds")] [SerializeField] protected UnityEvent soundEvent;

        public void Awake()
        {
            InitializeUIElements();
        }

        public virtual void OnEnable()
        {
            navigateReference.action.performed += OnNavigate;
        }

        public virtual void OnDisable()
        {
            navigateReference.action.performed -= OnNavigate;
        }

        protected abstract void InitializeUIElements();
        protected abstract void OnFocusElement(object element);
        protected abstract void OnBlurElement(object element);

        protected virtual void OnNavigate(InputAction.CallbackContext context)
        {
        }

        protected void PlayFocusAnimation(Vector3 scale, Transform target)
        {
            if (target != null)
            {
                DOTween.Kill(target);
                DOTween.To(() => target.localScale, x => target.localScale = x, scale, scaleDuration);
            }
        }
    }
}