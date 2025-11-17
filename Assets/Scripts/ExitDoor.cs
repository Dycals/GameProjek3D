using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour, IInteractable
{
    public void Interact(PlayerInteract player)
    {
        if (GameManager.Instance.ObjectivesCollected >= GameManager.Instance.totalObjectives)
        {
            Debug.Log("Pintu keluar terbuka! Level Selesai.");
            GameManager.Instance.EndGame(true);
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log($"Belum semua objektif terkumpul! Sisa: {GameManager.Instance.totalObjectives - GameManager.Instance.objectivesCollected}");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
