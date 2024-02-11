using UnityEngine;
using System.Collections;

namespace Player
{
    public class PlayerController : MonoBehaviour, IPolarityObject
    {
        public float jumpForce;
        public float moveSpeed = 5f;
        private Rigidbody2D rb;
        public bool isGrounded;
        private bool polarity = true; // Default polarity

        public bool Polarity
        {
            get { return polarity; }
            set { polarity = value; }
        }

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void Start()
        {
            StartCoroutine(TogglePolarityCoroutine());
        }

        void Update()
        {
            float moveInput = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if(other.gameObject.CompareTag("SuperGround"))
            {
                isGrounded = true;
            }
            if ((polarity && other.gameObject.CompareTag("Ground")) || (!polarity && other.gameObject.CompareTag("Hazard")))
            {
                isGrounded = true;
            }
            else if ((polarity && other.gameObject.CompareTag("Hazard")) || (!polarity && other.gameObject.CompareTag("Ground")))
            {
                GameManager.Instance.GameOver();
            }

            // Semi-solid platform logic
            if (other.gameObject.CompareTag("SemiSolid"))
            {
                if (rb.velocity.y < 0 && transform.position.y > other.transform.position.y)
                {
                    isGrounded = false;
                }
                else
                {
                    isGrounded = true;
                }
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if ((polarity && other.gameObject.CompareTag("Ground")) || (!polarity && other.gameObject.CompareTag("Hazard")))
            {
                isGrounded = false;
            }
            if(other.gameObject.CompareTag("SuperGround"))
            {
                isGrounded = false;
            }
            if (other.gameObject.CompareTag("SemiSolid"))
            {
                isGrounded = false;
            }
        }

        IEnumerator TogglePolarityCoroutine()
        {
            while (true)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    polarity = !polarity;
                    Debug.Log("Polarity switched: " + (polarity ? "Hazard -> Ground" : "Ground -> Hazard"));
                }

                yield return null;
            }
        }
    }
}
