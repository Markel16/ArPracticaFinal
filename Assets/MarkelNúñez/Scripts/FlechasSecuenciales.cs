using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class FlechasSecuenciales : MonoBehaviour
{
    private ARTrackedImageManager trackedImageManager;

    public GameObject flechaPrefab;
    public List<Vector3> flechasOffsets;

    private int indiceActual = 0;
    private ARTrackedImage imagenActiva;
    private GameObject ultimaFlecha = null;

    void Awake()
    {
        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        if (trackedImageManager != null)
            trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        if (trackedImageManager != null)
            trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            Debug.Log("¡Imagen detectada: " + trackedImage.referenceImage.name + "!");
            imagenActiva = trackedImage;
            CrearSiguienteFlecha();
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            if (imagenActiva == null)
            {
                imagenActiva = trackedImage;
                Debug.Log("Imagen actualizada: " + trackedImage.referenceImage.name);
            }
        }

        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            Debug.Log("Imagen eliminada: " + trackedImage.referenceImage.name);
        }
    }

    public void SiguienteFlecha(GameObject flechaAnterior)
    {
        Destroy(flechaAnterior);
        CrearSiguienteFlecha();
    }

    public void CrearSiguienteFlecha()
    {
        if (indiceActual >= flechasOffsets.Count) return;

        Vector3 offset = flechasOffsets[indiceActual];
        Vector3 posicion;

        if (indiceActual == 0 && imagenActiva != null)
        {
            // La primera flecha se coloca desde la imagen, sin rotación
            posicion = imagenActiva.transform.position + offset;
        }
        else if (ultimaFlecha != null)
        {
            // Las siguientes flechas se colocan en cadena desde la anterior, sin rotación
            posicion = ultimaFlecha.transform.position + offset;
        }
        else
        {
            Debug.LogWarning("No se puede colocar la flecha, no hay referencia.");
            return;
        }

        Quaternion rotacion = Quaternion.LookRotation(offset.normalized);
        GameObject nuevaFlecha = Instantiate(flechaPrefab, posicion, rotacion);

        FlechaTrigger trigger = nuevaFlecha.AddComponent<FlechaTrigger>();
        trigger.manager = this;

        ultimaFlecha = nuevaFlecha;
        Debug.Log("✅ Flecha instanciada correctamente en el paso " + (indiceActual + 1));

        indiceActual++;
    }


}
