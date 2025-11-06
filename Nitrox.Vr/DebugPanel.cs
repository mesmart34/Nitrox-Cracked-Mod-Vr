using TMPro;
using UnityEngine;

namespace Nitrox.Vr;

// This behaves like ErrorMesage.Show(), but is designed to not spam with changing values
// I used this to debug/view certain positions in the UI
public class DebugPanel : MonoBehaviour
{
    private TextMeshProUGUI entry = null!;
    
    public static DebugPanel Instance { get; set; } = null!;

    private void Start()
    {
        GameObject? prefabMessage = ErrorMessage.main.prefabMessage;
        GameObject obj = Instantiate(prefabMessage);
        entry = obj.GetComponent<TextMeshProUGUI>();
        entry.rectTransform.SetParent(ErrorMessage.main.messageCanvas, false);
        obj.SetActive(true);
        entry.text = "";

        Instance = this;
        Settings.IsDebugChanged += (isOn) =>
        {
            enabled = isOn;
        };
    }

    private void OnDisable()
    {
        if (!entry)
        {
            return;
        }
        
        entry.text = "";
        entry.enabled = false;
    }

    private void OnEnable()
    {
        if (entry)
        {
            entry.enabled = true;
        }
    }

    public static void Show(string message)
    {
        if (Instance == null)
        {
            return;
        }
        Instance.entry.text = message;
    }
}
