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
            imagenActiva = trackedImage;
            CrearSiguienteFlecha();
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

        Vector3 offset = flechasOffsets[indiceActual];
        Vector3 posicionMundo = imagenActiva.transform.TransformPoint(offset);
        GameObject nuevaFlecha = Instantiate(flechaPrefab, posicionMundo, Quaternion.LookRotation(imagenActiva.transform.forward));

        FlechaTrigger trigger = nuevaFlecha.AddComponent<FlechaTrigger>();
        trigger.manager = this;

        indiceActual++;
    }
}

