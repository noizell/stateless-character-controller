using System.Collections;
using UnityEditor;
using UnityEngine;


namespace Surfer
{
    /// <summary>
    /// Helper class of Surfer for editor and runtime code.
    /// </summary>
    public static class SurferHelper 
    {

        const string OpeningBracket = "(";
        const string ClosingBracket = ")";
        const string SettingsPath = "Assets/Resources/SurferSettings.asset";
        public const string Unset = "-----";
        public const int kWhateverVersion = 0;
        public const int kPlayerIDFallback = 0;
        public const int kNestedPlayerID = -1;
        public const int kDefaultPlayerID = kNestedPlayerID;
        public const float lineHeight = 20;
        public static Vector3 OutPos = new Vector3(0,-10_000,0);
        public static string kOverallVolume = "_kSUVolume";
        public static string kNoFoldout = "NO_FOLDOUT";
        public const string OldInputSymbol = "SUOld", NewInputSymbol = "SUNew", RewiredInputSymbol = "SURew",
                CharTweenerSymbol = "SUCharT", BoltSymbol = "SUBolt";

        public static string BuildVPrefix
        {
            get
            {
                return "v" + Application.version;
            }
        }

        static SurferSO _so = default;
        public static SurferSO SO
        {
            get
            {
                if(_so != null)
                    return _so;

        #if UNITY_EDITOR
                _so = AssetDatabase.LoadAssetAtPath<SurferSO>(SettingsPath);

                if (_so == null)
                {
                    _so = ScriptableObject.CreateInstance<SurferSO>();
                    _so.SetUp();

                    System.IO.Directory.CreateDirectory("Assets/Resources");

                    AssetDatabase.CreateAsset(_so, SettingsPath);
                    AssetDatabase.SaveAssets();

                    //if not in scene, add it
                    SurferManager sm = GameObject.FindObjectOfType<SurferManager>();
                    if(sm!=null)
                    {
                        GameObject.DestroyImmediate(sm.gameObject);
                        new GameObject("Surfer").AddComponent<SurferManager>();
                    }
                }
        #else
                _so = SurferManager.I.SO;
        #endif
                return _so;
            }
            
        }


        public static RectTransform GetParentRect(Transform transf)
        {

            RectTransform cp = null;

            if(transf.parent != null)
            {
                if(transf.parent.GetComponent<RectTransform>()!=null)
                    cp = transf.parent.GetComponent<RectTransform>();
                else
                    cp = GetParentRect(transf.parent);
            }

            return cp;

        }

        


        /// <summary>
        /// Play a sound if an audioClip is set. It checks and adds (if not available) an AudioSource  
        /// </summary>
        /// <param name="go">GameObject where to check or add for an Audiosource to play the sound on</param>
        public static void PlaySound(AudioClip clip,GameObject go, float delay = 0)
        {
            if(go == null)
            return;
            
            if(clip!=null)
            {
                AudioSource _audioSource = go.GetComponent<AudioSource>();
                if(_audioSource == null)
                {
                    _audioSource = go.AddComponent<AudioSource>();
                }

                _audioSource.clip = clip;

                if(delay < Mathf.Epsilon)
                    _audioSource.Play();
                else
                    go.GetComponent<MonoBehaviour>().StartCoroutine(DelaySound(_audioSource,delay));
                
            }
        }


        /// <summary>
        /// Coroutine for delayed sound based on action delay
        /// </summary>
        /// <param name="delay"></param>
        static IEnumerator DelaySound(AudioSource source,float delay)
        {
            yield return new WaitForSeconds(delay);
            source.Play();
        }

#if UNITY_EDITOR

        public static void DrawLine(ref Rect pos)
        {

            Handles.color = Color.gray;
            Handles.DrawLine(new Vector2(pos.x - 15, pos.y-1), new Vector2(pos.width + 15, pos.y-1));
            Handles.DrawLine(new Vector2(pos.x - 15, pos.y), new Vector2(pos.width + 15, pos.y));

        }


        public static void DrawLine()
        {
            var rect = EditorGUILayout.BeginHorizontal();
            Handles.color = Color.gray;
            Handles.DrawLine(new Vector2(rect.x - 15, rect.y), new Vector2(rect.width + 15, rect.y));
            EditorGUILayout.EndHorizontal();
        }





        public static void UpdateBuildVersionStrings()
        {


            AddOrUpdate(RuntimePlatform.Android, GetVersionFor(RuntimePlatform.Android));
            AddOrUpdate(RuntimePlatform.IPhonePlayer, GetVersionFor(RuntimePlatform.IPhonePlayer));
            AddOrUpdate(RuntimePlatform.WindowsPlayer, GetVersionFor(RuntimePlatform.WindowsPlayer));
            AddOrUpdate(RuntimePlatform.OSXPlayer, GetVersionFor(RuntimePlatform.OSXPlayer));
            AddOrUpdate(RuntimePlatform.PS4, GetVersionFor(RuntimePlatform.PS4));
            AddOrUpdate(RuntimePlatform.XboxOne, GetVersionFor(RuntimePlatform.XboxOne));
            AddOrUpdate(RuntimePlatform.Switch, GetVersionFor(RuntimePlatform.Switch));



        }



        static void AddOrUpdate(RuntimePlatform platf, string versionTxt)
        {

            if (!SO.BuildVersions.ContainsKey(platf))
                SO.BuildVersions.Add(platf, versionTxt);
            else
                SO.BuildVersions[platf] = versionTxt;

        }




        static string GetVersionFor(RuntimePlatform platf)
        {

            switch (platf)
            {

                case RuntimePlatform.Android:

                    return BuildVPrefix + "(" + PlayerSettings.Android.bundleVersionCode + ")";

                case RuntimePlatform.IPhonePlayer:

                    return BuildVPrefix + "(" + PlayerSettings.iOS.buildNumber + ")";

                case RuntimePlatform.OSXPlayer:

                    return BuildVPrefix + "(" + PlayerSettings.macOS.buildNumber + ")";


                case RuntimePlatform.PS4:

                    return BuildVPrefix + "(" + PlayerSettings.PS4.appVersion + "/" + PlayerSettings.PS4.masterVersion + ")";


                case RuntimePlatform.XboxOne:

                    return BuildVPrefix + "(" + PlayerSettings.XboxOne.Version + ")";

                case RuntimePlatform.Switch:

                    return BuildVPrefix + "(" + PlayerSettings.Switch.displayVersion + "(" + PlayerSettings.Switch.releaseVersion + ")";

                default:

                    return BuildVPrefix;


            }


        }



        public static bool HasIntegration(string symbol)
        {
            return PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup).Contains(symbol);
        }


#endif


        public static string GetVersion()
        {

            switch (Application.platform)
            {

                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.LinuxEditor:
                case RuntimePlatform.WindowsEditor:

#if UNITY_EDITOR

                    BuildTarget bt = EditorUserBuildSettings.activeBuildTarget;

                    switch(bt)
                    {

                        case BuildTarget.Android:
                            return GetVersionFor(RuntimePlatform.Android);
                        case BuildTarget.iOS:
                            return GetVersionFor(RuntimePlatform.IPhonePlayer);
                        case BuildTarget.StandaloneWindows:
                        case BuildTarget.StandaloneWindows64:
                            return GetVersionFor(RuntimePlatform.WindowsPlayer);
                        case BuildTarget.StandaloneOSX:
                            return GetVersionFor(RuntimePlatform.OSXPlayer);
                        case BuildTarget.PS4:
                            return GetVersionFor(RuntimePlatform.PS4);
                        case BuildTarget.XboxOne:
                            return GetVersionFor(RuntimePlatform.XboxOne);
                        case BuildTarget.Switch:
                            return GetVersionFor(RuntimePlatform.Switch);
                        default:

                            return BuildVPrefix;

                    }

#endif


                case RuntimePlatform.Android:
                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.OSXPlayer:
                case RuntimePlatform.PS4:
                case RuntimePlatform.XboxOne:
                case RuntimePlatform.Switch:


                    if (SO.BuildVersions.TryGetValue(Application.platform, out string value))
                        return value;


                    return BuildVPrefix;


                default:

                    return BuildVPrefix;


            }


        }













    }



}
