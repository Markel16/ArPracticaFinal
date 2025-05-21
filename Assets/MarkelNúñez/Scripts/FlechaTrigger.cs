using UnityEngine;

public class FlechaTrigger : MonoBehaviour
{
    public FlechasSecuenciales manager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            manager.SiguienteFlecha(this.gameObject);
        }
    }
}

