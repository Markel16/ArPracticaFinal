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

    void CrearSiguienteFlecha()
    {
        if (indiceActual >= flechasOffsets.Count || imagenActiva == null)
            return;

        if (flechaPrefab == null)
        {
            Debug.LogError(" ERROR: El prefab de la flecha no está asignado en el inspector.");
            return;
        }

        Vector3 offset = flechasOffsets[indiceActual];
        Vector3 posicionMundo = imagenActiva.transform.TransformPoint(offset);

        GameObject nuevaFlecha = Instantiate(flechaPrefab, posicionMundo, Quaternion.LookRotation(imagenActiva.transform.forward));

        if (nuevaFlecha == null)
        {
            Debug.LogError(" ERROR: No se pudo instanciar la flecha.");
            return;
        }

        FlechaTrigger trigger = nuevaFlecha.AddComponent<FlechaTrigger>();
        trigger.manager = this;

        Debug.Log("Flecha instanciada correctamente en el paso " + (indiceActual + 1));
        indiceActual++;
    }
}
