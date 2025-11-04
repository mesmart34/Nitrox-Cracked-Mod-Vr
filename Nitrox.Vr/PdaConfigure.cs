using UnityEngine;
using static UnityEngine.Object;

namespace Nitrox.Vr;

public static class PdaConfigure
{
    private const string PAUSE_MENU_TEXT = "Pause Menu";
    private const string PAUSE_MENU_BUTTON_NAME = "PauseMenuButton";

    public static void ModifyPdaUi()
    { 
        uGUI_PDA pdaGUI = uGUI_PDA.main;
        
        Transform targetParent = pdaGUI.tabInventory.transform;
        
        uGUI_Dialog? dialog = pdaGUI.GetComponentInChildren<uGUI_Dialog>(true);
        uGUI_DialogButton? buttonPrefab = dialog.buttonPrefab;
        uGUI_DialogButton? button = Instantiate(buttonPrefab, targetParent).GetComponent<uGUI_DialogButton>();
        button.button.transform.parent = targetParent;
        button.button.gameObject.gameObject.name = PAUSE_MENU_BUTTON_NAME;
        button.text.text = PAUSE_MENU_TEXT;
        button.button.onClick.RemoveAllListeners();
        button.button.onClick.AddListener(() =>
        {
            IngameMenu.main.Open();
        });
        
        button.rectTransform.anchoredPosition = new Vector2(1100, 50);
        button.rectTransform.pivot = new Vector2(1, 0);
        button.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 300);
        button.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100);
        button.rectTransform.ForceUpdateRectTransforms();
        button.rectTransform.GetComponentsInChildren<RectTransform>().ForEach(rt => rt.ForceUpdateRectTransforms());
    }
}
