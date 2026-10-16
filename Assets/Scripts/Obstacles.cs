using System.Collections;
using UnityEngine;

namespace SnowballAdventure
{
    // --------------------------------------------------------
    // 1. HAZARD SPIKE
    // --------------------------------------------------------
    public class HazardSpike : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            var ball = collision.GetComponentInParent<SnowballController>() ?? collision.GetComponent<SnowballController>();
            if (ball != null)
            {
                ball.HitHazard();
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var ball = collision.gameObject.GetComponentInParent<SnowballController>() ?? collision.gameObject.GetComponent<SnowballController>();
            if (ball != null)
            {
                ball.HitHazard();
            }
        }
    }

    // --------------------------------------------------------
    // 2. BOUNCE SPRING / MUSHROOM
    // --------------------------------------------------------
    public class BounceSpring : MonoBehaviour
    {
        public float bounceForce = 12.5f;
        public Transform springVisual;

        private Vector3 initialScale = Vector3.one;
        private bool isBouncing = false;

        private void Awake()
        {
            if (springVisual == null) springVisual = transform;
            initialScale = springVisual.localScale;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<SnowballController>(out var ball))
            {
                ball.Bounce(bounceForce);
                if (!isBouncing) StartCoroutine(SpringAnimation());
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent<SnowballController>(out var ball))
            {
                ball.Bounce(bounceForce);
                if (!isBouncing) StartCoroutine(SpringAnimation());
            }
        }

        private IEnumerator SpringAnimation()
        {
            isBouncing = true;
            // Compress down
            springVisual.localScale = new Vector3(initialScale.x * 1.35f, initialScale.y * 0.45f, initialScale.z);
            yield return new WaitForSeconds(0.06f);
            // Overshoot upward
            springVisual.localScale = new Vector3(initialScale.x * 0.85f, initialScale.y * 1.4f, initialScale.z);
            yield return new WaitForSeconds(0.12f);
            // Return to normal
            springVisual.localScale = initialScale;
            isBouncing = false;
        }
    }

    // --------------------------------------------------------
    // 3. CRUMBLING ICE PLATFORM
    // --------------------------------------------------------
    public class CrumblingPlatform : MonoBehaviour
    {
        public float crumbleDelay = 0.65f;
        public float respawnDelay = 3.5f;

        private Vector3 originalPos;
        private SpriteRenderer rend;
        private Collider2D col;
        private bool isCrumbling = false;

        private void Awake()
        {
            originalPos = transform.position;
            rend = GetComponent<SpriteRenderer>();
            col = GetComponent<Collider2D>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (isCrumbling) return;
            if (collision.gameObject.CompareTag("Player") || collision.gameObject.GetComponent<SnowballController>() != null)
            {
                StartCoroutine(CrumbleSequence());
            }
        }

        private IEnumerator CrumbleSequence()
        {
            isCrumbling = true;
            SoundManager.EnsureInstance().PlayCrumble();

            // Shake violently
            float elapsed = 0f;
            while (elapsed < crumbleDelay)
            {
                elapsed += Time.deltaTime;
                float shakeOffset = Mathf.Sin(elapsed * 60f) * 0.08f;
                transform.position = originalPos + new Vector3(shakeOffset, 0f, 0f);
                yield return null;
            }

            // Drop and fade out
            if (col != null) col.enabled = false;
            elapsed = 0f;
            Vector3 dropStart = transform.position;
            Color startColor = rend != null ? rend.color : Color.white;

            while (elapsed < 0.4f)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / 0.4f;
                transform.position = dropStart + Vector3.down * (t * 2.5f);
                if (rend != null) rend.color = new Color(startColor.r, startColor.g, startColor.b, 1f - t);
                yield return null;
            }

            if (rend != null) rend.enabled = false;

            // Wait for respawn
            yield return new WaitForSeconds(respawnDelay);

            // Reset
            transform.position = originalPos;
            if (rend != null)
            {
                rend.color = startColor;
                rend.enabled = true;
            }
            if (col != null) col.enabled = true;
            isCrumbling = false;
        }
    }

    // --------------------------------------------------------
    // 4. MOVING FROST HAZARD (Rolling spiked wheel / sentry)
    // --------------------------------------------------------
    public class MovingFrostHazard : MonoBehaviour
    {
        public float moveDistance = 5f;
        public float moveSpeed = 3.5f;
        public float spinSpeed = 360f;

        private Vector3 startPos;
        private int direction = 1;

        private void Start()
        {
            startPos = transform.position;
        }

        private void Update()
        {
            transform.position += Vector3.right * (direction * moveSpeed * Time.deltaTime);
            transform.Rotate(0f, 0f, -direction * spinSpeed * Time.deltaTime);

            if (Mathf.Abs(transform.position.x - startPos.x) >= moveDistance)
            {
                direction = -direction;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var ball = collision.GetComponentInParent<SnowballController>() ?? collision.GetComponent<SnowballController>();
            if (ball != null)
            {
                ball.HitHazard();
            }
        }
    }

    // --------------------------------------------------------
    // 5. FALLING ICICLE
    // --------------------------------------------------------
    public class FallingIcicle : MonoBehaviour
    {
        public float triggerRadius = 3f;
        public float fallGravity = 2.5f;

        private Rigidbody2D rb;
        private Vector3 startPos;
        private bool isFalling = false;
        private bool hasFallen = false;

        private void Awake()
        {
            startPos = transform.position;
            rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Kinematic;
            }
        }

        private void Update()
        {
            if (isFalling || hasFallen) return;

            if (SnowballController.Instance != null)
            {
                Vector3 playerPos = SnowballController.Instance.transform.position;
                if (Mathf.Abs(playerPos.x - transform.position.x) < triggerRadius && playerPos.y < transform.position.y)
                {
                    StartCoroutine(DropSequence());
                }
            }
        }

        private IEnumerator DropSequence()
        {
            isFalling = true;

            // Shake slightly before dropping
            float elapsed = 0f;
            while (elapsed < 0.25f)
            {
                elapsed += Time.deltaTime;
                float shake = Mathf.Sin(elapsed * 50f) * 0.05f;
                transform.position = startPos + new Vector3(shake, 0f, 0f);
                yield return null;
            }

            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.gravityScale = fallGravity;
            }

            hasFallen = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var ball = collision.GetComponentInParent<SnowballController>() ?? collision.GetComponent<SnowballController>();
            if (ball != null)
            {
                ball.HitHazard();
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var ball = collision.gameObject.GetComponentInParent<SnowballController>() ?? collision.gameObject.GetComponent<SnowballController>();
            if (ball != null)
            {
                ball.HitHazard();
            }
        }
    }

    // --------------------------------------------------------
    // 6. ICY CHASM / ABYSS TRIGGER
    // --------------------------------------------------------
    public class IcyChasmTrigger : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            var ball = collision.GetComponentInParent<SnowballController>() ?? collision.GetComponent<SnowballController>();
            if (ball != null)
            {
                ball.HitHazard();
            }
        }
    }

    // --------------------------------------------------------
    // 7. COLLECTIBLE COIN
    // --------------------------------------------------------
    public class CoinItem : MonoBehaviour
    {
        public float bobSpeed = 3.5f;
        public float bobHeight = 0.15f;
        public float rotateSpeed = 60f;

        private Vector3 startPos;
        private bool isCollected = false;

        private void Start()
        {
            startPos = transform.position;
        }

        private void Update()
        {
            if (isCollected) return;

            // Gentle floating bob
            float newY = startPos.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
            transform.position = new Vector3(startPos.x, newY, startPos.z);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (isCollected) return;

            if (collision.GetComponentInParent<SnowballController>() != null || collision.CompareTag("Player"))
            {
                isCollected = true;
                SoundManager.EnsureInstance().PlayCoin();

                GameSceneManager gsm = GameSceneManager.Instance;
                if (gsm != null)
                {
                    gsm.AddCoin();
                }

                StartCoroutine(CollectEffect());
            }
        }

        private IEnumerator CollectEffect()
        {
            float elapsed = 0f;
            Vector3 initial = transform.position;
            Vector3 initScale = transform.localScale;

            while (elapsed < 0.22f)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / 0.22f;
                transform.position = initial + Vector3.up * (t * 0.8f);
                transform.localScale = Vector3.Lerp(initScale, initScale * 1.4f, t);
                yield return null;
            }

            gameObject.SetActive(false);
        }
    }

    // --------------------------------------------------------
    // 8. CHECKPOINT FLAG
    // --------------------------------------------------------
    public class Checkpoint : MonoBehaviour
    {
        public Sprite idleSprite;
        public Sprite activeSprite;
        public SpriteRenderer flagRenderer { get => rend; set => rend = value; }
        public Sprite flagActiveSprite { get => activeSprite; set => activeSprite = value; }

        private SpriteRenderer rend;
        private bool isActivated = false;

        private void Awake()
        {
            if (rend == null) rend = GetComponent<SpriteRenderer>();
            if (idleSprite == null) idleSprite = Resources.Load<Sprite>("Sprites/checkpoint_flag_idle");
            if (activeSprite == null) activeSprite = Resources.Load<Sprite>("Sprites/checkpoint_flag_active");
            if (rend != null && idleSprite != null) rend.sprite = idleSprite;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (isActivated) return;

            if (collision.TryGetComponent<SnowballController>(out var ball))
            {
                isActivated = true;
                if (rend != null && activeSprite != null)
                {
                    rend.sprite = activeSprite;
                }
                ball.SetCheckpoint(transform.position + Vector3.up * 0.5f);
                SoundManager.EnsureInstance().PlayCheckpoint();
            }
        }
    }

    // --------------------------------------------------------
    // 9. LEVEL GOAL (Victory Arch / Igloo)
    // --------------------------------------------------------
    public class LevelGoal : MonoBehaviour
    {
        private bool isFinished = false;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (isFinished) return;

            if (collision.GetComponentInParent<SnowballController>() != null || collision.CompareTag("Player"))
            {
                isFinished = true;
                SoundManager.EnsureInstance().PlayVictory();

                GameSceneManager gsm = GameSceneManager.Instance;
                if (gsm != null)
                {
                    gsm.TriggerLevelComplete();
                }
            }
        }
    }

    public class GoalArch : LevelGoal
    {
    }

    // --------------------------------------------------------
    // 10. WIND DRAFT ZONE (Upward air geyser)
    // --------------------------------------------------------
    public class WindDraftZone : MonoBehaviour
    {
        public float upwardLiftSpeed = 12f;
        public float forwardPushSpeed = 0f;

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Rigidbody2D>(out var rb))
            {
                float newY = Mathf.Max(rb.linearVelocity.y, upwardLiftSpeed);
                float newX = rb.linearVelocity.x;
                if (Mathf.Abs(forwardPushSpeed) > 0.1f)
                {
                    newX = Mathf.MoveTowards(newX, forwardPushSpeed, Time.fixedDeltaTime * 20f);
                }
                rb.linearVelocity = new Vector2(newX, newY);
            }
        }
    }

    // --------------------------------------------------------
    // 11. BLINKING PHASE ICE PLATFORM
    // --------------------------------------------------------
    public class BlinkingIcePlatform : MonoBehaviour
    {
        public float activeDuration = 2.2f;
        public float warningDuration = 0.8f;
        public float inactiveDuration = 1.4f;
        public float initialOffset = 0f;

        private Collider2D col;
        private SpriteRenderer rend;

        private void Awake()
        {
            col = GetComponent<Collider2D>();
            rend = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            StartCoroutine(PhaseCycle());
        }

        private IEnumerator PhaseCycle()
        {
            if (initialOffset > 0f)
            {
                yield return new WaitForSeconds(initialOffset);
            }

            while (true)
            {
                // Active solid phase
                if (col != null) col.enabled = true;
                if (rend != null) rend.color = Color.white;
                yield return new WaitForSeconds(activeDuration);

                // Warning flicker phase
                float timer = 0f;
                while (timer < warningDuration)
                {
                    if (rend != null)
                    {
                        float a = Mathf.PingPong(timer * 8f, 1f) > 0.5f ? 1f : 0.25f;
                        rend.color = new Color(1f, 1f, 1f, a);
                    }
                    timer += Time.deltaTime;
                    yield return null;
                }

                // Inactive vanished phase
                if (col != null) col.enabled = false;
                if (rend != null) rend.color = new Color(1f, 1f, 1f, 0.15f);
                yield return new WaitForSeconds(inactiveDuration);
            }
        }
    }

    // --------------------------------------------------------
    // 12. SPEED BOOST RAMP (Golden Chevron Booster)
    // --------------------------------------------------------
    public class SpeedBoostRamp : MonoBehaviour
    {
        public float boostSpeed = 14f;
        public float upwardKick = 4.5f;

        private bool onCooldown = false;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (onCooldown) return;

            if (collision.TryGetComponent<SnowballController>(out var ball) &&
                collision.TryGetComponent<Rigidbody2D>(out var rb))
            {
                onCooldown = true;
                rb.linearVelocity = new Vector2(boostSpeed, Mathf.Max(rb.linearVelocity.y, upwardKick));
                SoundManager.EnsureInstance().PlaySpring();
                StartCoroutine(CooldownRoutine());
            }
        }

        private IEnumerator CooldownRoutine()
        {
            yield return new WaitForSeconds(0.4f);
            onCooldown = false;
        }
    }

    // --------------------------------------------------------
    // 13. ROTATING FROST BAR (Spiked Obstacle Arm)
    // --------------------------------------------------------
    public class RotatingFrostBar : MonoBehaviour
    {
        public float rotationSpeed = 65f; // degrees per second
        public bool clockwise = true;

        private void Update()
        {
            float dir = clockwise ? -1f : 1f;
            transform.Rotate(0f, 0f, dir * rotationSpeed * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var ball = collision.GetComponentInParent<SnowballController>() ?? collision.GetComponent<SnowballController>();
            if (ball != null)
            {
                ball.HitHazard();
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var ball = collision.gameObject.GetComponentInParent<SnowballController>() ?? collision.gameObject.GetComponent<SnowballController>();
            if (ball != null)
            {
                ball.HitHazard();
            }
        }
    }

    // --------------------------------------------------------
    // 14. FROST TRAMPOLINE (Mega Bouncy Pad / Super Shroom)
    // --------------------------------------------------------
    public class FrostTrampoline : MonoBehaviour
    {
        public float bounceForce = 17.5f;
        public Transform trampolineVisual;

        private Vector3 initialScale = Vector3.one;
        private bool isBouncing = false;

        private void Awake()
        {
            if (trampolineVisual == null) trampolineVisual = transform;
            initialScale = trampolineVisual.localScale;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<SnowballController>(out var ball))
            {
                ball.Bounce(bounceForce);
                SoundManager.EnsureInstance().PlaySpring();
                if (!isBouncing) StartCoroutine(TrampolineSquash());
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent<SnowballController>(out var ball))
            {
                ball.Bounce(bounceForce);
                SoundManager.EnsureInstance().PlaySpring();
                if (!isBouncing) StartCoroutine(TrampolineSquash());
            }
        }

        private IEnumerator TrampolineSquash()
        {
            isBouncing = true;
            trampolineVisual.localScale = new Vector3(initialScale.x * 1.45f, initialScale.y * 0.35f, initialScale.z);
            yield return new WaitForSeconds(0.08f);
            trampolineVisual.localScale = new Vector3(initialScale.x * 0.8f, initialScale.y * 1.55f, initialScale.z);
            yield return new WaitForSeconds(0.14f);
            trampolineVisual.localScale = initialScale;
            isBouncing = false;
        }
    }

    // --------------------------------------------------------
    // 15. MOVING ICE PLATFORM (Oscillating Floating Raft)
    // --------------------------------------------------------
    public class MovingIcePlatform : MonoBehaviour
    {
        public Vector2 moveDelta = new Vector2(4f, 0f);
        public float speed = 2.2f;
        public float waitTime = 0.35f;

        private Vector3 startPos;
        private Vector3 targetPos;
        private bool movingToTarget = true;
        private float pauseTimer = 0f;
        private Rigidbody2D rb;

        private void Awake()
        {
            startPos = transform.position;
            targetPos = startPos + (Vector3)moveDelta;
            rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Kinematic;
            }
        }

        private void FixedUpdate()
        {
            if (pauseTimer > 0f)
            {
                pauseTimer -= Time.fixedDeltaTime;
                if (rb != null) rb.linearVelocity = Vector2.zero;
                return;
            }

            Vector3 dest = movingToTarget ? targetPos : startPos;
            Vector3 newPos = Vector3.MoveTowards(transform.position, dest, speed * Time.fixedDeltaTime);

            if (rb != null)
            {
                rb.MovePosition(newPos);
            }
            else
            {
                transform.position = newPos;
            }

            if (Vector3.Distance(newPos, dest) < 0.02f)
            {
                movingToTarget = !movingToTarget;
                pauseTimer = waitTime;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.GetComponent<SnowballController>() != null)
            {
                collision.transform.SetParent(transform);
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.GetComponent<SnowballController>() != null)
            {
                if (collision.transform.parent == transform)
                {
                    collision.transform.SetParent(null);
                }
            }
        }
    }

    // --------------------------------------------------------
    // 16. SWINGING FROST MACE (Pendulum Hazard)
    // --------------------------------------------------------
    public class SwingingFrostMace : MonoBehaviour
    {
        public float maxAngle = 60f;
        public float swingSpeed = 2.4f;
        public float phaseOffset = 0f;

        private float timer = 0f;

        private void Awake()
        {
            timer = phaseOffset;
        }

        private void Update()
        {
            timer += Time.deltaTime;
            float currentAngle = maxAngle * Mathf.Sin(timer * swingSpeed);
            transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);
        }
    }

    // --------------------------------------------------------
    // 17. FROST PORTAL (Warp Vortex)
    // --------------------------------------------------------
    public class FrostPortal : MonoBehaviour
    {
        public FrostPortal targetPortal;
        public Vector2 exitVelocity = new Vector2(8f, 6f);
        public float cooldownTime = 0.8f;

        private static float globalLastTeleportTime = 0f;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (targetPortal == null) return;
            if (Time.time - globalLastTeleportTime < cooldownTime) return;

            if (collision.TryGetComponent<SnowballController>(out var ball) &&
                collision.TryGetComponent<Rigidbody2D>(out var rb))
            {
                globalLastTeleportTime = Time.time;
                Vector3 exitPos = targetPortal.transform.position;
                ball.transform.position = exitPos;
                rb.linearVelocity = exitVelocity;
                SoundManager.EnsureInstance().PlaySpring();
            }
        }
    }

    // --------------------------------------------------------
    // 18. BOUNCE GRAVITY ORB (Mid-Air Kinetic Jump Restorer)
    // --------------------------------------------------------
    public class BounceGravityOrb : MonoBehaviour
    {
        public float upwardBounce = 13.5f;
        public float forwardBoost = 4.5f;
        public float cooldown = 0.5f;

        private float lastBounceTime = -1f;
        private Vector3 initialScale;
        private bool isAnimating = false;

        private void Awake()
        {
            initialScale = transform.localScale;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (Time.time - lastBounceTime < cooldown) return;

            if (collision.TryGetComponent<SnowballController>(out var ball) &&
                collision.TryGetComponent<Rigidbody2D>(out var rb))
            {
                lastBounceTime = Time.time;
                float dir = Mathf.Sign(rb.linearVelocity.x);
                if (Mathf.Abs(dir) < 0.1f) dir = 1f;

                rb.linearVelocity = new Vector2(dir * forwardBoost, upwardBounce);
                SoundManager.EnsureInstance().PlaySpring();
                if (!isAnimating) StartCoroutine(PulseAnimation());
            }
        }

        private IEnumerator PulseAnimation()
        {
            isAnimating = true;
            transform.localScale = initialScale * 1.4f;
            yield return new WaitForSeconds(0.1f);
            transform.localScale = initialScale * 0.85f;
            yield return new WaitForSeconds(0.12f);
            transform.localScale = initialScale;
            isAnimating = false;
        }
    }

    // --------------------------------------------------------
    // 19. FROST LASER BEAM (Pulsing Cryo-Laser Hazard)
    // --------------------------------------------------------
    public class FrostLaserBeam : MonoBehaviour
    {
        public float inactiveDuration = 2.0f;
        public float warningDuration = 0.7f;
        public float activeDuration = 1.4f;
        public float initialDelay = 0f;

        public SpriteRenderer beamRenderer;
        public Collider2D beamCollider;

        private void Start()
        {
            StartCoroutine(LaserCycle());
        }

        private IEnumerator LaserCycle()
        {
            if (initialDelay > 0f) yield return new WaitForSeconds(initialDelay);

            while (true)
            {
                // Inactive state
                if (beamCollider != null) beamCollider.enabled = false;
                if (beamRenderer != null) beamRenderer.color = new Color(1f, 1f, 1f, 0.05f);
                yield return new WaitForSeconds(inactiveDuration);

                // Warning flickering state
                float warnTimer = 0f;
                while (warnTimer < warningDuration)
                {
                    if (beamRenderer != null)
                    {
                        float a = Mathf.PingPong(warnTimer * 12f, 1f) > 0.4f ? 0.4f : 0.1f;
                        beamRenderer.color = new Color(1f, 1f, 1f, a);
                    }
                    warnTimer += Time.deltaTime;
                    yield return null;
                }

                // Active laser blast state
                if (beamCollider != null) beamCollider.enabled = true;
                if (beamRenderer != null) beamRenderer.color = Color.white;
                yield return new WaitForSeconds(activeDuration);
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (beamCollider != null && beamCollider.enabled)
            {
                var ball = collision.GetComponentInParent<SnowballController>() ?? collision.GetComponent<SnowballController>();
                if (ball != null)
                {
                    ball.HitHazard();
                }
            }
        }
    }

    // --------------------------------------------------------
    // 20. CRUSHING ICE PILLAR (Ceiling Smasher / Thwomp)
    // --------------------------------------------------------
    public class CrushingIcePillar : MonoBehaviour
    {
        public float slamSpeed = 16f;
        public float retractSpeed = 3.5f;
        public float groundWait = 0.6f;
        public float triggerDistanceX = 2.2f;
        public float dropDistance = 4.5f;

        private Vector3 startPos;
        private Vector3 targetPos;
        private bool isSlamming = false;
        private bool isRetracting = false;

        private void Awake()
        {
            startPos = transform.position;
            targetPos = startPos + Vector3.down * dropDistance;
        }

        private void Update()
        {
            if (!isSlamming && !isRetracting)
            {
                // Check if player is below
                GameObject player = GameObject.FindWithTag("Player");
                if (player != null)
                {
                    float dx = Mathf.Abs(player.transform.position.x - transform.position.x);
                    float dy = transform.position.y - player.transform.position.y;
                    if (dx < triggerDistanceX && dy > 0f && dy < dropDistance + 2f)
                    {
                        StartCoroutine(SlamRoutine());
                    }
                }
            }
        }

        private IEnumerator SlamRoutine()
        {
            isSlamming = true;
            while (Vector3.Distance(transform.position, targetPos) > 0.05f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, slamSpeed * Time.deltaTime);
                yield return null;
            }
            transform.position = targetPos;
            yield return new WaitForSeconds(groundWait);

            isSlamming = false;
            isRetracting = true;
            while (Vector3.Distance(transform.position, startPos) > 0.05f)
            {
                transform.position = Vector3.MoveTowards(transform.position, startPos, retractSpeed * Time.deltaTime);
                yield return null;
            }
            transform.position = startPos;
            isRetracting = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var ball = collision.GetComponentInParent<SnowballController>() ?? collision.GetComponent<SnowballController>();
            if (ball != null)
            {
                ball.HitHazard();
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var ball = collision.gameObject.GetComponentInParent<SnowballController>() ?? collision.gameObject.GetComponent<SnowballController>();
            if (ball != null)
            {
                ball.HitHazard();
            }
        }
    }

    // --------------------------------------------------------
    // 21. FROST CONVEYOR (Accelerating Ice Belt)
    // --------------------------------------------------------
    public class FrostConveyor : MonoBehaviour
    {
        public float conveyorSpeed = 5.5f;

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent<Rigidbody2D>(out var rb))
            {
                rb.linearVelocity = new Vector2(conveyorSpeed, rb.linearVelocity.y);
            }
        }
    }

    // --------------------------------------------------------
    // 22. GRAVITY FLIP PAD (Antigravity Well)
    // --------------------------------------------------------
    public class GravityFlipPad : MonoBehaviour
    {
        public float antigravityLift = 13.5f;
        public float pushForward = 3.5f;

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Rigidbody2D>(out var rb))
            {
                rb.linearVelocity = new Vector2(Mathf.Max(rb.linearVelocity.x, pushForward), antigravityLift);
            }
        }
    }
}
