using System;
using System.Collections.Generic;
using System.Reflection;
using Nitrox.Vr;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NitroxPatcher.Patches.Persistent
{
    public sealed partial class FPSInputModule_UpdateMouseState_Patch : NitroxPatch, IPersistentPatch
    {
        private static readonly MethodInfo TARGET_METHOD = Reflect.Method((FPSInputModule t) => t.UpdateMouseState(default(PointerEventData)));

        // Cached reflection
        private static readonly Type mouseStateType = typeof(PointerInputModule).GetNestedType("MouseState", BindingFlags.NonPublic);
        private static readonly MethodInfo getButtonState = mouseStateType.GetMethod("GetButtonState", BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly MethodInfo setButtonState = mouseStateType.GetMethod("SetButtonState", BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly FieldInfo m_MouseStateField = typeof(PointerInputModule).GetField("m_MouseState", BindingFlags.Instance | BindingFlags.NonPublic);

        public static void Prefix(FPSInputModule __instance, PointerEventData leftData)
        {
            if (leftData != null)
            {
                leftData.position = __instance.lastRaycastResult.worldPosition;
            }
        }

        private static bool GetPointerData(FPSInputModule instance, int id, out PointerEventData data, bool create)
        {
            data = null;
            if (instance == null)
            {
                return false;
            }

            FieldInfo pointerDataField = typeof(FPSInputModule).GetField("m_PointerData", BindingFlags.Instance | BindingFlags.NonPublic);
            Dictionary<int, PointerEventData> pointerData = pointerDataField?.GetValue(instance) as System.Collections.Generic.Dictionary<int, PointerEventData>;
            if (pointerData == null)
            {
                return false;
            }

            if (pointerData.TryGetValue(id, out data) || !create)
            {
                return data != null;
            }
            
            // Access protected eventSystem via reflection
            PropertyInfo eventSystemProp = typeof(FPSInputModule).GetProperty("eventSystem", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy);
            EventSystem eventSystem = eventSystemProp?.GetValue(instance) as EventSystem;
            if (eventSystem == null)
            {
                return false;
            }

            data = new PointerEventData(eventSystem);
            pointerData[id] = data;

            return data != null;
        }

        public static void Postfix(FPSInputModule __instance, PointerEventData leftData)
        {
            if (!GetPointerData(__instance, -3, out PointerEventData data2, create: true))
            {
                return;
            }
            if (leftData == null || data2 == null)
            {
                return;
            }

            __instance.CopyFromTo(leftData, data2);
            data2.button = PointerEventData.InputButton.Middle;

            if (GameInput.PrimaryDevice != GameInput.Device.Controller)
            {
                return;
            }

            bool buttonDown = GameInput.GetButtonDown(GameInput.button2);
            bool buttonUp = GameInput.GetButtonUp(GameInput.button2);

            // Reflection for MouseState
            object mouseState = m_MouseStateField?.GetValue(__instance);
            if (mouseState == null)
            {
                return;
            }

            object buttonState = getButtonState.Invoke(mouseState, [PointerEventData.InputButton.Middle]);
            if (buttonState == null)
            {
                return;
            }

            FieldInfo buttonStateField = buttonState.GetType().GetField("buttonState", BindingFlags.Instance | BindingFlags.Public);
            if (buttonStateField != null)
            {
                PointerEventData.FramePressState framePressState = (PointerEventData.FramePressState)buttonStateField.GetValue(buttonState)!;

                if (framePressState != PointerEventData.FramePressState.NotChanged)
                {
                    return;
                }
            }

            setButtonState.Invoke(mouseState, [
                PointerEventData.InputButton.Middle,
                FPSInputModule.ConstructPressState(buttonDown, buttonUp),
                data2
            ]);
        }
    }
}
