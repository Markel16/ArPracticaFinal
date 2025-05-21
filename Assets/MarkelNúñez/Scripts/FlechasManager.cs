using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class FlechasManager : MonoBehaviour
{
    private ARTrackedImageManager trackedImageManager;

    [Header("Prefab de la flecha")]
    public GameObject flechaPrefab;

    [Header("Offset de cada flecha respecto al marcador")]
    public List<Vector3> flechasOffsets;

    private Dictionary<string, List<GameObject>> flechasInstanciadas = new Dictionary<string, List<GameObject>>();

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
            CrearRuta(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            ActualizarRuta(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            string nombre = trackedImage.referenceImage.name ?? "DefaultImage";

            if (flechasInstanciadas.ContainsKey(nombre))
            {
                foreach (GameObject flecha in flechasInstanciadas[nombre])
                {
                    Destroy(flecha);
                }
                flechasInstanciadas.Remove(nombre);
            }
        }
    }

    void CrearRuta(ARTrackedImage trackedImage)
    {
        if (flechaPrefab == null || flechasOffsets.Count == 0)
            return;

        string nombre = trackedImage.referenceImage.name ?? "DefaultImage";

        List<GameObject> flechas = new List<GameObject>();

        for (int i = 0; i < flechasOffsets.Count; i++)
        {
            Vector3 offsetLocal = flechasOffsets[i];
            Vector3 posicionMundo = trackedImage.transform.TransformPoint(offsetLocal);
            GameObject flecha = Instantiate(flechaPrefab, posicionMundo, Quaternion.LookRotation(trackedImage.transform.forward));
            flechas.Add(flecha);
        }

        flechasInstanciadas[nombre] = flechas;
    }

    void ActualizarRuta(ARTrackedImage trackedImage)
    {
        string nombre = trackedImage.referenceImage.name ?? "DefaultImage";

        if (!flechasInstanciadas.ContainsKey(nombre))
            return;

        List<GameObject> flechas = flechasInstanciadas[nombre];

        for (int i = 0; i < flechas.Count; i++)
        {
            Vector3 nuevaPos = trackedImage.transform.TransformPoint(flechasOffsets[i]);
            flechas[i].transform.position = nuevaPos;
        }
    }
}


