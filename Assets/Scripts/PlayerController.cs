using UnityEngine;
using System.Collections;

namespace Player // Namespace for encapsulation and modularity
{
    public class PlayerController : MonoBehaviour
    {
        public float jumpForce;
        public float moveSpeed = 5f;
        private Rigidbody2D rb;
        public bool isGrounded;

        // private backing field for polarity
        private bool polarity = true; 
        // polarity is true = Ground is safe and Hazard is bad
        // polarity is false = Ground is bad and Hazard is safe 

        // Getter and Setter 
        public bool Polarity
        {
            get { return polarity; }
            set { polarity = value; }
        }

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        // Start is called before the first frame update
        void Start()
        {
            //Run polarity check as a coroutine
            StartCoroutine(TogglePolarityCoroutine());
        }

        // Update is called once per frame
        void Update()
        {
            // Horizontal movement
            float moveInput = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

            // Jumping
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            // SuperGround should always function normally, separate from polarity
            if(other.gameObject.CompareTag("SuperGround"))
            {
                isGrounded = true;
            }
            // Regular Collision with polarity switch
            if ((polarity && other.gameObject.CompareTag("Ground")) || (!polarity && other.gameObject.CompareTag("Hazard")))
            {
                isGrounded = true;
            }
            // Hazard Collision with polarity switch
            else if ((polarity && other.gameObject.CompareTag("Hazard")) || (!polarity && other.gameObject.CompareTag("Ground")))
            {
                GameManager.Instance.GameOver();
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            // Check collision exit with "Ground"
            if ((polarity && other.gameObject.CompareTag("Ground")) || (!polarity && other.gameObject.CompareTag("Hazard")))
            {
                isGrounded = false;
            }
            if(other.gameObject.CompareTag("SuperGround"))
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
                    // Toggle the polarity
                    Polarity = !Polarity;

                    // Debug log to indicate polarity switch
                    Debug.Log("Polarity switched: " + (polarity ? "Hazard -> Ground" : "Ground -> Hazard"));
                }

                yield return null;
            }
        }
    }
}
