using UnityEngine;

namespace SnowballAdventure
{
    public class SmoothFollowCamera : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset = new Vector3(0f, 1.4f, -10f);
        public float smoothTime = 0.15f;
        public float lookAheadFactor = 0.25f;

        [Header("Level Bounds")]
        public float minX = 0f;
        public float maxX = 110f;
        public float minY = 0.8f;
        public float maxY = 8f;

        private Vector3 currentVelocity;
        private Vector3 targetPos;

        private void Start()
        {
            if (target == null && SnowballController.Instance != null)
            {
                target = SnowballController.Instance.transform;
            }
        }

        private void LateUpdate()
        {
            if (target == null)
            {
                if (SnowballController.Instance != null) target = SnowballController.Instance.transform;
                else return;
            }

            float lookAhead = 0f;
            if (target.TryGetComponent<Rigidbody2D>(out var rb))
            {
                lookAhead = rb.linearVelocity.x * lookAheadFactor;
            }

            float desiredX = Mathf.Clamp(target.position.x + offset.x + lookAhead, minX, maxX);
            float desiredY = Mathf.Clamp(target.position.y + offset.y, minY, maxY);

            targetPos = new Vector3(desiredX, desiredY, offset.z);
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref currentVelocity, smoothTime);
        }
    }

    /// <summary>
    /// Smooth 3-tile continuous parallax layer that moves at a fraction of camera speed
    /// and seamlessly wraps around tiles so there are never visible seams or empty edges.
    /// </summary>
    public class SmoothParallaxLayer : MonoBehaviour
    {
        [Tooltip("Speed multiplier relative to camera movement (0 = static, 1 = moves with foreground)")]
        public float parallaxFactor = 0.2f;
        public float tileWidth = 10.24f;
        public float verticalOffset = 0f;

        private Transform camTransform;
        private Transform[] tiles;
        private float lastCameraX;

        public void Initialize(Transform camera, Sprite sprite, int sortingOrder, float width, float scaleY, float yOffset)
        {
            camTransform = camera;
            tileWidth = width;
            verticalOffset = yOffset;
            lastCameraX = camera.position.x;

            tiles = new Transform[3];
            for (int i = 0; i < 3; i++)
            {
                GameObject tileObj = new GameObject($"Tile_{i}");
                tileObj.transform.SetParent(transform, false);

                SpriteRenderer rend = tileObj.AddComponent<SpriteRenderer>();
                rend.sprite = sprite;
                rend.sortingOrder = sortingOrder;

                tileObj.transform.localScale = new Vector3(width / (sprite.rect.width / sprite.pixelsPerUnit), scaleY, 1f);
                float xPos = (i - 1) * tileWidth;
                tileObj.transform.localPosition = new Vector3(xPos, verticalOffset, 0f);
                tiles[i] = tileObj.transform;
            }
        }

        private void LateUpdate()
        {
            if (camTransform == null || tiles == null || tiles.Length < 3) return;

            // Move this container smoothly based on camera movement
            float deltaX = camTransform.position.x - lastCameraX;
            transform.position += new Vector3(deltaX * parallaxFactor, 0f, 0f);
            lastCameraX = camTransform.position.x;

            // Keep vertical aligned with camera height loosely
            transform.position = new Vector3(transform.position.x, camTransform.position.y * (parallaxFactor * 0.25f), transform.position.z);

            // Wrap tiles around camera
            for (int i = 0; i < 3; i++)
            {
                float distFromCam = tiles[i].position.x - camTransform.position.x;

                // If tile fell too far behind camera on left, shift it to the right
                if (distFromCam < -tileWidth * 1.5f)
                {
                    tiles[i].position += new Vector3(tileWidth * 3f, 0f, 0f);
                }
                // If tile went too far ahead on right, shift it to the left
                else if (distFromCam > tileWidth * 1.5f)
                {
                    tiles[i].position -= new Vector3(tileWidth * 3f, 0f, 0f);
                }
            }
        }
    }
}
