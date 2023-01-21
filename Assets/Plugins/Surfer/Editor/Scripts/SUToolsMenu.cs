using System.IO;
using UnityEditor;
using UnityEngine;

namespace Surfer
{
    public class SUToolsMenu : EditorWindow
    {

        static string _conditionsScriptPath = "Assets/Surfer/Runtime/Scripts/Condition/ConditionChecks.cs";
        static string _conditionsFieldsScriptPath = "Assets/Surfer/Runtime/Scripts/Condition/CustomSerializedConditionFields.cs";
        static string _reactionsScriptPath = "Assets/Surfer/Runtime/Scripts/Element/CustomReactions.cs";
        static string _reactionsFieldsScriptPath = "Assets/Surfer/Runtime/Scripts/Element/CustomSerializedReactionFields.cs";


        static string _contentPackagePath = "Assets/Surfer/SurferContent.unitypackage";

        static string _conditionsPackPath = "Assets/Surfer/Conditions.unitypackage";
        static string _conditionsFieldsPackPath = "Assets/Surfer/ConditionsFields.unitypackage";
        static string _reactionsPackPath = "Assets/Surfer/Reactions.unitypackage";
        static string _reactionsFieldsPackPath = "Assets/Surfer/ReactionsFields.unitypackage";


        static string _playmakerPackPath = "Assets/Surfer/PlayMaker.unitypackage";
        static string _demoPackPath = "Assets/Surfer/Demo.unitypackage";

        static string _kInstallCompleted = "_SUInstallCompleted";



        [MenuItem("Tools/Surfer/Install or Update", priority = -10)]
        static void Init()
        {

            CheckToDelete(_conditionsScriptPath,_conditionsPackPath);
            CheckToDelete(_reactionsScriptPath,_reactionsPackPath);
            CheckToDelete(_conditionsFieldsScriptPath,_conditionsFieldsPackPath);
            CheckToDelete(_reactionsFieldsScriptPath,_reactionsFieldsPackPath);


            EditorPrefs.SetBool(_kInstallCompleted, false);

            CheckToUnpack(_conditionsPackPath);
            CheckToUnpack(_reactionsPackPath);
            CheckToUnpack(_conditionsFieldsPackPath);
            CheckToUnpack(_reactionsFieldsPackPath);
            CheckToUnpack(_contentPackagePath);

        }

        [UnityEditor.Callbacks.DidReloadScripts]
        static void FinishedCompile()
        {
            if (!EditorPrefs.GetBool(_kInstallCompleted))
                return;

            EditorPrefs.SetBool(_kInstallCompleted,false);
            EditorUtility.DisplayDialog("Surfer", "\nCompleted!", "Ok!");

        }



        [MenuItem("Tools/Surfer/Email", priority = -10)]
        static void Email()
        {

            Application.OpenURL("mailto:atstudiosupp@gmail.com");

        }


        [MenuItem("Tools/Surfer/Docs API", priority = -10)]
        static void Docs()
        {
            Application.OpenURL("https://atstudios.github.io/Surfer/index.html");

        }



        [MenuItem("Tools/Surfer/PlayMaker/Unpack")]
        static void PlayMaker()
        {

            if (System.IO.File.Exists(_playmakerPackPath))
            {
                EditorPrefs.SetBool(_kInstallCompleted, true);
                AssetDatabase.ImportPackage(_playmakerPackPath, false);
                AssetDatabase.DeleteAsset(_playmakerPackPath);

            }

        }

        [MenuItem("Tools/Surfer/Demo/Unpack")]
        static void Demo()
        {

            if (System.IO.File.Exists(_demoPackPath))
            {
                AssetDatabase.ImportPackage(_demoPackPath, false);
                AssetDatabase.DeleteAsset(_demoPackPath);

            }

        }

        private static void CheckToDelete(string script,string package)
        {
            if (System.IO.File.Exists(script))
                AssetDatabase.DeleteAsset(package);
        }

        private static void CheckToUnpack(string package)
        {
            if (System.IO.File.Exists(package))
            {
                EditorPrefs.SetBool(_kInstallCompleted, true);
                AssetDatabase.ImportPackage(package, false);
                AssetDatabase.DeleteAsset(package);
            }
        }

    }
}



