using System.Collections;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace SnowballAdventure
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class SnowballController : MonoBehaviour
    {
        public static SnowballController Instance { get; private set; }

        [Header("Movement Physics")]
        public float maxRollSpeed = 6.6f;
        public float accelerationSpeed = 18f;
        public float decelerationSpeed = 22f;
        public float jumpSpeed = 7.6f;
        public float airControlMultiplier = 0.75f;

        [Header("Ground Check")]
        public LayerMask groundLayer;
        public float groundCheckDistance = 0.12f;
        public Vector2 groundBoxSize = new Vector2(0.7f, 0.15f);

        [Header("Visual & Effects")]
        public Transform squashTransform;
        public Transform visualTransform;
        public SpriteRenderer visualRenderer;
        public ParticleSystem snowDustParticles;

        [Header("Runtime State")]
        public bool isGrounded { get; private set; }
        public bool canControl = true;
        public Vector3 respawnPosition;

        // Private Physics & Input
        private Rigidbody2D rb;
        private CircleCollider2D circleCol;
        private float radius;
        private float currentHorizontalInput = 0f;
        private bool jumpRequested = false;
        private bool isJumpCooldown = false;
        private Vector2 lastPhysicsPos;

        // UI Touch input flags
        public static bool uiLeftDown = false;
        public static bool uiRightDown = false;
        public static bool uiJumpDown = false;

        [Header("Physical Rolling Animation")]
        [Tooltip("Multiplier for rolling rotation speed (1.0 = exact mathematical 1:1 no-slip rolling).")]
        public float rollSpeedMultiplier = 0.8f;

        // Visual Rolling Angle & Squash
        private float currentRollAngle = 0f;
        private float targetVisualSpeed = 0f;
        private float currentVisualSpeed = 0f;
        private Vector3 targetScale = Vector3.one;
        private Vector3 currentScale = Vector3.one;
        private bool wasGroundedLastFrame = true;

        private void Awake()
        {
            Instance = this;
            rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.freezeRotation = true;
                rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
                rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            }

            circleCol = GetComponent<CircleCollider2D>();
            if (circleCol != null)
            {
                circleCol.radius = 0.5f;
                circleCol.offset = Vector2.zero;
                PhysicsMaterial2D zeroFriction = new PhysicsMaterial2D("SnowballPhysicsMat")
                {
                    friction = 0f,
                    bounciness = 0f
                };
                circleCol.sharedMaterial = zeroFriction;
            }
            radius = circleCol != null ? circleCol.radius * transform.localScale.x : 0.5f;

            // 1. Ensure child "Squash" exists (handles world-aligned vertical squash & stretch)
            if (squashTransform == null)
            {
                Transform existingSquash = transform.Find("Squash");
                if (existingSquash != null)
                {
                    squashTransform = existingSquash;
                }
                else
                {
                    GameObject sqObj = new GameObject("Squash");
                    sqObj.transform.SetParent(transform, false);
                    sqObj.transform.localPosition = Vector3.zero;
                    sqObj.transform.localRotation = Quaternion.identity;
                    sqObj.transform.localScale = Vector3.one;
                    squashTransform = sqObj.transform;
                }
            }

            // 2. Ensure child "Visual" exists under squashTransform (handles 360° rolling rotation)
            if (visualTransform == null)
            {
                Transform existingVisual = squashTransform.Find("Visual") ?? transform.Find("Visual");
                if (existingVisual != null)
                {
                    visualTransform = existingVisual;
                    if (visualTransform.parent != squashTransform)
                    {
                        visualTransform.SetParent(squashTransform, false);
                    }
                }
                else
                {
                    GameObject vObj = new GameObject("Visual");
                    vObj.transform.SetParent(squashTransform, false);
                    vObj.transform.localPosition = Vector3.zero;
                    vObj.transform.localRotation = Quaternion.identity;
                    vObj.transform.localScale = Vector3.one;
                    visualTransform = vObj.transform;
                }
            }

            // 3. Ensure SpriteRenderer exists on the Visual child
            if (visualRenderer == null && visualTransform != null)
            {
                visualRenderer = visualTransform.GetComponent<SpriteRenderer>();
                if (visualRenderer == null)
                {
                    visualRenderer = visualTransform.gameObject.AddComponent<SpriteRenderer>();
                }
            }

            if (visualRenderer != null)
            {
                visualRenderer.sortingOrder = 10; // In front of terrain & obstacles
            }

            respawnPosition = transform.position;
            lastPhysicsPos = rb != null ? rb.position : (Vector2)transform.position;
        }

        private void Start()
        {
            ApplyCharacterSkin();
            canControl = true;
        }

        public void ApplyCharacterSkin()
        {
            if (visualRenderer == null && visualTransform != null)
            {
                visualRenderer = visualTransform.GetComponent<SpriteRenderer>();
                if (visualRenderer == null)
                {
                    visualRenderer = visualTransform.gameObject.AddComponent<SpriteRenderer>();
                }
            }

            if (visualRenderer != null)
            {
                Sprite s = GameDataManager.GetActiveCharacterSprite();
                if (s == null)
                {
                    s = Resources.Load<Sprite>("Sprites/char_0_frosty");
                }
                visualRenderer.sprite = s;
                visualRenderer.sortingOrder = 10;
            }
        }

        private void Update()
        {
            if (!canControl) return;

            float h = 0f;
            bool jumpKey = false;

            // 1. UI Touch controls (on-screen buttons)
            if (uiLeftDown) h -= 1f;
            if (uiRightDown) h += 1f;
            if (uiJumpDown)
            {
                jumpKey = true;
                uiJumpDown = false;
            }

            // 2. New Input System (Keyboard + Gamepad)
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null)
            {
                if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) h -= 1f;
                if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) h += 1f;
                if (Keyboard.current.spaceKey.wasPressedThisFrame || Keyboard.current.wKey.wasPressedThisFrame || Keyboard.current.upArrowKey.wasPressedThisFrame) jumpKey = true;
            }
            if (Gamepad.current != null)
            {
                float stickX = Gamepad.current.leftStick.x.ReadValue();
                if (Mathf.Abs(stickX) > 0.15f) h += stickX;
                if (Gamepad.current.dpad.left.isPressed) h -= 1f;
                if (Gamepad.current.dpad.right.isPressed) h += 1f;
                if (Gamepad.current.buttonSouth.wasPressedThisFrame) jumpKey = true;
            }
#endif

            // 3. Legacy Input fallback (wrapped in try-catch so it never crashes if disabled)
#if ENABLE_LEGACY_INPUT_MANAGER || !ENABLE_INPUT_SYSTEM
            try
            {
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) h -= 1f;
                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) h += 1f;
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) jumpKey = true;
                if (h == 0f) h = Input.GetAxisRaw("Horizontal");
            }
            catch { }
#endif

            currentHorizontalInput = Mathf.Clamp(h, -1f, 1f);

            // Jump trigger
            if (jumpKey && isGrounded && !isJumpCooldown)
            {
                jumpRequested = true;
            }

            // 4. Smooth render-rate rolling rotation in exact sync with linear motion
            currentVisualSpeed = Mathf.Lerp(currentVisualSpeed, targetVisualSpeed, Time.deltaTime * 35f);
            if (Mathf.Abs(currentVisualSpeed) < 0.01f && targetVisualSpeed == 0f)
            {
                currentVisualSpeed = 0f;
            }

            if (radius > 0.001f && Mathf.Abs(currentVisualSpeed) > 0.001f)
            {
                float angularSpeedDeg = -(currentVisualSpeed / radius) * Mathf.Rad2Deg * rollSpeedMultiplier;
                currentRollAngle = (currentRollAngle + angularSpeedDeg * Time.deltaTime) % 360f;
            }

            if (visualTransform != null)
            {
                visualTransform.localRotation = Quaternion.Euler(0f, 0f, currentRollAngle);
            }

            // Animate Squash and Stretch
            AnimateSquashAndStretch();
        }

        private void FixedUpdate()
        {
            CheckGround();

            if (!canControl) return;

            // 1. Measure actual physical displacement moved since last physics frame
            Vector2 currentPos = rb.position;
            float actualDeltaX = currentPos.x - lastPhysicsPos.x;
            lastPhysicsPos = currentPos;

            // Target visual speed is active only when ball is actually moving horizontally
            // When blocked against a wall or stationary, actualDeltaX is ~0, halting rotation immediately
            if (Mathf.Abs(actualDeltaX) > 0.0005f)
            {
                targetVisualSpeed = rb.linearVelocity.x;
            }
            else
            {
                targetVisualSpeed = 0f;
            }

            // 2. Apply horizontal acceleration / deceleration
            float accel = isGrounded ? accelerationSpeed : (accelerationSpeed * airControlMultiplier);
            if (currentHorizontalInput != 0f)
            {
                // Crisp turn-around if pressing opposite to current movement
                if (Mathf.Abs(rb.linearVelocity.x) > 0.1f && Mathf.Sign(currentHorizontalInput) != Mathf.Sign(rb.linearVelocity.x))
                {
                    accel += decelerationSpeed;
                }

                rb.linearVelocity += Vector2.right * (currentHorizontalInput * accel * Time.fixedDeltaTime);
                if (Mathf.Abs(rb.linearVelocity.x) > maxRollSpeed)
                {
                    rb.linearVelocity = new Vector2(Mathf.Sign(rb.linearVelocity.x) * maxRollSpeed, rb.linearVelocity.y);
                }
            }
            else
            {
                // Crisp linear deceleration to halt firmly without slushy sliding
                float decel = isGrounded ? decelerationSpeed : (decelerationSpeed * 0.4f);
                float newX = Mathf.MoveTowards(rb.linearVelocity.x, 0f, decel * Time.fixedDeltaTime);
                rb.linearVelocity = new Vector2(newX, rb.linearVelocity.y);
            }

            // 3. Execute Jump
            if (jumpRequested)
            {
                ExecuteJump(jumpSpeed);
                jumpRequested = false;
            }
        }

        private void CheckGround()
        {
            Vector2 checkPos = (Vector2)transform.position + Vector2.down * (radius + groundCheckDistance * 0.5f);
            bool grounded = false;

            if (groundLayer.value != 0)
            {
                RaycastHit2D hit = Physics2D.BoxCast(checkPos, groundBoxSize, 0f, Vector2.down, groundCheckDistance, groundLayer);
                grounded = hit.collider != null;
            }
            else
            {
                Collider2D[] cols = Physics2D.OverlapBoxAll(checkPos, groundBoxSize, 0f);
                foreach (Collider2D c in cols)
                {
                    if (c.gameObject != gameObject && !c.isTrigger)
                    {
                        grounded = true;
                        break;
                    }
                }
            }

            isGrounded = grounded;

            // Just landed detection for juicy bounce squash
            if (!wasGroundedLastFrame && isGrounded)
            {
                // Land squash (proportional to falling speed)
                float fallSpeed = Mathf.Abs(rb.linearVelocity.y);
                float squashAmount = Mathf.Clamp(fallSpeed * 0.035f, 0.15f, 0.45f);
                targetScale = new Vector3(1f + squashAmount, 1f - squashAmount, 1f);

                if (snowDustParticles != null)
                {
                    snowDustParticles.Play();
                }
            }

            wasGroundedLastFrame = isGrounded;
        }

        private void ExecuteJump(float speed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, speed);
            isJumpCooldown = true;
            StartCoroutine(ResetJumpCooldown());

            // Jump stretch
            targetScale = new Vector3(0.75f, 1.35f, 1f);

            SoundManager.EnsureInstance().PlayJump();
        }

        public void Bounce(float bounceForce)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounceForce);
            targetScale = new Vector3(0.7f, 1.45f, 1f);
            SoundManager.EnsureInstance().PlaySpring();
        }

        private IEnumerator ResetJumpCooldown()
        {
            yield return new WaitForSeconds(0.2f);
            isJumpCooldown = false;
        }

        private void AnimateSquashAndStretch()
        {
            Transform target = squashTransform != null ? squashTransform : visualTransform;
            if (target == null) return;

            // Smoothly return target scale back to normal (1, 1, 1)
            targetScale = Vector3.Lerp(targetScale, Vector3.one, Time.deltaTime * 8f);
            currentScale = Vector3.Lerp(currentScale, targetScale, Time.deltaTime * 18f);

            target.localScale = currentScale;
        }

        public void HitHazard()
        {
            if (!canControl) return;

            GameSceneManager gsm = GameSceneManager.Instance;
            if (gsm != null)
            {
                gsm.OnPlayerDied(this);
            }
            else
            {
                StartCoroutine(HazardDeathSequence());
            }
        }

        public void TriggerDeathShrink(System.Action onShrinkComplete)
        {
            StartCoroutine(DeathShrinkRoutine(onShrinkComplete));
        }

        private IEnumerator DeathShrinkRoutine(System.Action onShrinkComplete)
        {
            canControl = false;
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;

            SoundManager.EnsureInstance().PlayPop();

            // Pop shrink animation
            float elapsed = 0f;
            Vector3 startScale = visualTransform != null ? visualTransform.localScale : Vector3.one;
            while (elapsed < 0.25f)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / 0.25f;
                if (visualTransform != null)
                {
                    visualTransform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);
                }
                yield return null;
            }

            if (visualTransform != null)
            {
                visualTransform.localScale = Vector3.zero;
            }

            yield return new WaitForSeconds(0.2f);
            onShrinkComplete?.Invoke();
        }

        public void RespawnAtCheckpoint()
        {
            StartCoroutine(RespawnRoutine());
        }

        private IEnumerator RespawnRoutine()
        {
            transform.position = respawnPosition;
            rb.linearVelocity = Vector2.zero;
            rb.simulated = true;
            lastPhysicsPos = respawnPosition;
            currentRollAngle = 0f;
            targetVisualSpeed = 0f;
            currentVisualSpeed = 0f;
            currentScale = Vector3.one;
            targetScale = Vector3.one;
            if (squashTransform != null)
            {
                squashTransform.localScale = Vector3.one;
            }
            if (visualTransform != null)
            {
                visualTransform.localScale = Vector3.zero;
                visualTransform.localRotation = Quaternion.identity;
            }

            // Pop back to full size
            float elapsed = 0f;
            while (elapsed < 0.22f)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / 0.22f;
                if (visualTransform != null)
                {
                    visualTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
                }
                yield return null;
            }

            if (visualTransform != null)
            {
                visualTransform.localScale = Vector3.one;
            }

            canControl = true;
        }

        private IEnumerator HazardDeathSequence()
        {
            TriggerDeathShrink(() => RespawnAtCheckpoint());
            yield return null;
        }

        public void SetCheckpoint(Vector3 position)
        {
            respawnPosition = position;
        }

        // Mobile UI input hooks (instance & static)
        public void SetMobileHorizontal(float val)
        {
            if (val < -0.1f) { uiLeftDown = true; uiRightDown = false; }
            else if (val > 0.1f) { uiRightDown = true; uiLeftDown = false; }
            else { uiLeftDown = false; uiRightDown = false; }
        }

        public void PressMobileJump()
        {
            uiJumpDown = true;
        }

        // Static UI button hooks
        public static void SetMoveLeft(bool down) => uiLeftDown = down;
        public static void SetMoveRight(bool down) => uiRightDown = down;
        public static void RequestJump() => uiJumpDown = true;
    }
}
