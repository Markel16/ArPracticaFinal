using UnityEngine;

public class FlechaTrigger : MonoBehaviour
{
    public FlechasSecuenciales manager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Colisi�n con flecha detectada, activando siguiente");
            manager.SiguienteFlecha(this.gameObject);
        }
    }
}
