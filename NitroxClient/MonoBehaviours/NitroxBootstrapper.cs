extern alias SteamVRRef;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Nitrox.Vr;
using NitroxClient.MonoBehaviours.Discord;
using NitroxClient.MonoBehaviours.Gui.MainMenu;
using SteamVRRef::Valve.VR;
using UnityEngine;

namespace NitroxClient.MonoBehaviours;

public class NitroxBootstrapper : MonoBehaviour
{
    internal static NitroxBootstrapper Instance;

    // Awake is too early in Subnautica's lifecycle to access PlatformUtils
    // so we pick Start which will always happen after it's initialized
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
        gameObject.AddComponent<SceneCleanerPreserve>();
        gameObject.AddComponent<NitroxMainMenuModifications>();
        gameObject.AddComponent<DiscordClient>();
        
        string subnauticaManagedPath = Path.Combine(Application.dataPath, "Managed");
        
        string[] dllsToLoad = [
            Path.Combine(subnauticaManagedPath, "SteamVR.dll"),
            Path.Combine(subnauticaManagedPath, "SteamVR_Actions.dll")
        ];
        foreach (string dll in dllsToLoad)
        {
            string pathToDll = dll.Replace("\\", "/");
            Assembly.Load(pathToDll);
        }
        

#if DEBUG
        EnableDeveloperFeatures();
        CreateDebugger();
#endif

        // This is very important, see Application_runInBackground_Patch.cs
        Application.runInBackground = true;
        Log.Info($"Unity run in background set to \"{Application.runInBackground}\"");
        // Also very important for similar reasons
        MiscSettings.pdaPause = false;
    }

#if DEBUG
    private static void EnableDeveloperFeatures()
    {
        Log.Info("Enabling Subnautica developer console");
        PlatformUtils.SetDevToolsEnabled(true);
    }

    private void CreateDebugger()
    {
        Log.Info("Enabling Nitrox debugger");
        GameObject debugger = new();
        debugger.name = "Debug manager";
        debugger.AddComponent<NitroxDebugManager>();
        debugger.transform.SetParent(transform);
    }
#endif
}
