using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameManager gameManager;
    private Rigidbody rb;

    public float moveSpeed = 5f; 

    void Start()
    {

        gameManager = FindObjectOfType<GameManager>();

        if (gameManager == null)
        {
            Debug.LogError("GameManager tidak ditemukan di scene!");
        }


        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody tidak ditemukan! Player harus memilikinya.");
        }
    }



    void FixedUpdate()
    {

        if (gameManager.IsGameOver())
        {

            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            return;
        }


        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {

            gameManager.CollectCoin();

            // Hapus Koin
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Obstacle"))
        {

            gameManager.HitObstacle();


        }
    }
}