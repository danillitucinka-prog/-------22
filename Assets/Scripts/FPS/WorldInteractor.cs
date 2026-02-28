using LoxQuest3D.Interactables;
using LoxQuest3D.Scenes;
using UnityEngine;
using UnityEngine.UI;

namespace LoxQuest3D.FPS
{
    public sealed class WorldInteractor : MonoBehaviour
    {
        public Camera cam;
        public float maxDistance = 3f;
        public KeyCode interactKey = KeyCode.E;

        [Header("UI")]
        public Text hintText;

        private void Awake()
        {
            if (cam == null) cam = Camera.main;
        }

        private void Update()
        {
            var target = RaycastTarget(out var hit);
            if (hintText != null)
                hintText.text = target != null ? $"E: {target.displayName}" : "";

            if (target != null && Input.GetKeyDown(interactKey))
                target.Interact();
        }

        private InteractableWorldTarget RaycastTarget(out RaycastHit hit)
        {
            hit = default;
            if (cam == null) return null;
            var ray = new Ray(cam.transform.position, cam.transform.forward);
            if (!Physics.Raycast(ray, out hit, maxDistance)) return null;
            return hit.collider.GetComponentInParent<InteractableWorldTarget>();
        }
    }
}

