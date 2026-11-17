using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using ETouch = UnityEngine.InputSystem.EnhancedTouch.Touch;

// Pasang di objek "XR Origin" (atau objek kosong baru).
// Mengatur: model muncul per marker, rotasi 1 jari, zoom 2 jari,
// tombol Info, tombol Play Sound.
public class MusikARManager : MonoBehaviour
{
    [Serializable]
    public class Entry
    {
        public string imageName;      // harus sama persis dengan nama di Reference Image Library
        public GameObject prefab;     // model 3D
        public AudioClip clip;        // suara alat musik
        public string title;          // nama alat musik
        [TextArea(3, 8)] public string info;
    }

    [Header("AR")]
    public ARTrackedImageManager imageManager;
    public Entry[] entries;

    [Header("UI")]
    public Text titleText;
    public GameObject infoPanel;
    public Text infoText;
    public AudioSource audioSource;

    readonly Dictionary<string, GameObject> spawned = new Dictionary<string, GameObject>();
    Entry current;

    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        imageManager.trackablesChanged.AddListener(OnChanged);
        if (infoPanel != null) infoPanel.SetActive(false);
        if (titleText != null) titleText.text = "Arahkan kamera ke gambar alat musik";
    }

    void OnDisable()
    {
        imageManager.trackablesChanged.RemoveListener(OnChanged);
        EnhancedTouchSupport.Disable();
    }

    void OnChanged(ARTrackablesChangedEventArgs<ARTrackedImage> args)
    {
        foreach (var img in args.added) Handle(img);
        foreach (var img in args.updated) Handle(img);
    }

    void Handle(ARTrackedImage img)
    {
        string key = img.referenceImage.name;
        Entry e = Find(key);
        if (e == null || e.prefab == null) return;

        if (!spawned.TryGetValue(key, out GameObject go) || go == null)
        {
            go = Instantiate(e.prefab, img.transform);
            go.transform.localPosition = Vector3.zero;
            spawned[key] = go;
        }

        bool tracking = img.trackingState == TrackingState.Tracking;
        go.SetActive(tracking);

        if (tracking)
        {
            current = e;
            if (titleText != null) titleText.text = e.title;
            if (infoPanel != null && infoPanel.activeSelf && infoText != null) infoText.text = e.info;
        }
        else if (current == e)
        {
            current = null;
            if (titleText != null) titleText.text = "Arahkan kamera ke gambar alat musik";
            if (infoPanel != null) infoPanel.SetActive(false);
        }
    }

    Entry Find(string key)
    {
        foreach (var e in entries) if (e.imageName == key) return e;
        return null;
    }

    void Update()
    {
        if (current == null) return;
        if (!spawned.TryGetValue(current.imageName, out GameObject go) || go == null) return;

        var touches = ETouch.activeTouches;
        if (touches.Count == 1)
        {
            // Abaikan sentuhan di atas tombol UI
            if (UnityEngine.EventSystems.EventSystem.current != null &&
                UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(touches[0].touchId))
                return;
            float dx = touches[0].delta.x;
            go.transform.Rotate(0f, -dx * 0.3f, 0f, Space.Self);
        }
        else if (touches.Count >= 2)
        {
            Vector2 a = touches[0].screenPosition, b = touches[1].screenPosition;
            Vector2 pa = a - touches[0].delta, pb = b - touches[1].delta;
            float prev = Vector2.Distance(pa, pb);
            float now = Vector2.Distance(a, b);
            if (prev > 1f)
            {
                float s = go.transform.localScale.x * (now / prev);
                s = Mathf.Clamp(s, 0.02f, 0.3f);
                go.transform.localScale = Vector3.one * s;
            }
        }
    }

    // Hubungkan ke tombol Info
    public void ToggleInfo()
    {
        if (current == null || infoPanel == null) return;
        bool show = !infoPanel.activeSelf;
        infoPanel.SetActive(show);
        if (show && infoText != null) infoText.text = current.info;
    }

    // Hubungkan ke tombol Play Sound
    public void PlaySound()
    {
        if (current == null || current.clip == null || audioSource == null) return;
        audioSource.Stop();
        audioSource.clip = current.clip;
        audioSource.Play();
    }
}
