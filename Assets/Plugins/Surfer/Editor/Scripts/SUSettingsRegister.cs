using UnityEditor;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;


namespace Surfer
{

    public static class SUSettingsRegister
    {
        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {

            Texture _logo = default;

            //states
            string[] _states = default;
            string[] _events = default;
            string symbols = default;
            List<string> symbolsList = default;
            int _idx = default, _eventIdx = default;
            string _renamed = default, _newName = default, _eventRenamed = default, _newEventName = default;
            bool _justRenamed = default, _justAdded = default, _justRenamedEvent = default, _justAddedEvent = default,
                _oldInput = default, _newInput = default, _rewiredInput = default,
                _charTweener = default, _bolt = default;

            bool _justOpened = true;



            List<BuildTargetGroup> _bGroups = new List<BuildTargetGroup>() { BuildTargetGroup.Android , BuildTargetGroup.iOS , BuildTargetGroup.Standalone
            , BuildTargetGroup.PS4 , BuildTargetGroup.XboxOne, BuildTargetGroup.Switch};

            return new SettingsProvider("Preferences/Surfer", SettingsScope.User)
            {
                guiHandler = (searchContext) =>
                {

                    if (_justOpened)
                    {
                        GetSymbols();
                    }

                    using (var horiz = new GUILayout.HorizontalScope())
                    {
                        _logo = EditorGUIUtility.Load("Assets/Surfer/Editor/Images/logo.png") as Texture;
                        GUIStyle _style = new GUIStyle();
                        GUILayout.Box(_logo, _style, GUILayout.Width(100), GUILayout.Height(75));
                    }


                    if (GUILayout.Button("Email"))
                    {
                        Application.OpenURL("mailto:atstudiosupp@gmail.com");
                    }
                    if (GUILayout.Button("Documentation"))
                    {
                        Application.OpenURL("https://atstudios.github.io/Surfer/index.html");
                    }

                    EditorGUILayout.Space();
                    //horizontal line
                    DrawLine();

                    AddStatesSection();

                    EditorGUILayout.Space();
                    //input stuff
                    DrawLine();

                    AddIntegrationsSection();

                    EditorGUILayout.Space();

                    //custom events stuff
                    DrawLine();

                    AddEventsSection();


                    EditorGUILayout.Space();
                    DrawLine();

                    EditorGUILayout.Space();
                    if (GUILayout.Button("Generate All Constants"))
                    {
                        RebuildConstants();
                    }


                    EditorGUILayout.Space();
                    DrawLine();

                    EditorGUILayout.Space();
                    if (GUILayout.Button("Delete Slider Volume Key"))
                    {
                        PlayerPrefs.DeleteKey(SurferHelper.kOverallVolume);
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.Space();


                },

                keywords = new HashSet<string>(new[] { "Surfer", "Docs" })
            };

            void DrawLine()
            {
                var rect = EditorGUILayout.BeginHorizontal();
                Handles.color = Color.gray;
                Handles.DrawLine(new Vector2(rect.x - 15, rect.y), new Vector2(rect.width + 15, rect.y));
                EditorGUILayout.EndHorizontal();
            }


            void AddEventsSection()
            {
                EditorGUILayout.Space();

                //events stuff

                GUIStyle style = new GUIStyle();
                style.fontStyle = FontStyle.Bold;
                style.normal.textColor = Color.white;
                EditorGUILayout.LabelField("Custom Events", style);

                _events = SurferHelper.SO.GetEventsList();


                using (var horiz = new GUILayout.HorizontalScope())
                {

                    if (_justRenamedEvent)
                    {
                        _eventIdx = System.Array.IndexOf(_events, _eventRenamed);
                        _justRenamedEvent = false;
                    }
                    if (_justAddedEvent)
                    {
                        _eventIdx = System.Array.IndexOf(_events, _newEventName);
                        _justAddedEvent = false;
                    }

                    _eventIdx = EditorGUILayout.Popup("Modify :", _eventIdx, _events, GUILayout.Width(350));

                    if (_eventIdx != 0)
                    {
                        if (GUILayout.Button("Delete"))
                        {
                            if (EditorUtility.DisplayDialog("Delete", "Are you sure you want to delete event " + _events[_eventIdx] + " ?", "Yes", "No"))
                            {
                                SurferHelper.SO.RemoveEvent(_events[_eventIdx]);
                                EditorUtility.SetDirty(SurferHelper.SO);
                            }

                        }
                    }




                }

                using (var hor = new GUILayout.HorizontalScope())
                {

                    if (_eventIdx != 0)
                    {
                        _eventRenamed = EditorGUILayout.TextField("Rename to :", _eventRenamed, GUILayout.Width(350));


                        if (GUILayout.Button("Rename"))
                        {
                            SurferHelper.SO.RenameEvent(SurferHelper.SO.GetEventKey(_events[_eventIdx]), _eventRenamed);
                            EditorUtility.SetDirty(SurferHelper.SO);
                            _justRenamedEvent = true;
                        }
                    }

                }

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Add new");

                using (var vert = new GUILayout.HorizontalScope())
                {

                    _newEventName = EditorGUILayout.TextField("Event name: ", _newEventName, GUILayout.Width(350));


                    if (GUILayout.Button("Add"))
                    {
                        SurferHelper.SO.AddEvent(_newEventName);
                        EditorUtility.SetDirty(SurferHelper.SO);
                        _justAddedEvent = true;
                    }
                }


            }


            void GetSymbols()
            {

                symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
                symbolsList = symbols.Split(';').ToList();

                _oldInput = symbols.Contains(SurferHelper.OldInputSymbol);
                _newInput = symbols.Contains(SurferHelper.NewInputSymbol);
                _rewiredInput = symbols.Contains(SurferHelper.RewiredInputSymbol);
                _charTweener = symbols.Contains(SurferHelper.CharTweenerSymbol);
                _bolt = symbols.Contains(SurferHelper.BoltSymbol);


                _justOpened = false;

            }


            void AddIntegrationsSection()
            {
                EditorGUILayout.Space();


                GUIStyle style = new GUIStyle();
                style.fontStyle = FontStyle.Bold;
                style.normal.textColor = Color.white;
                EditorGUILayout.LabelField("Integrations", style);



                _oldInput = EditorGUILayout.Toggle("Old Input System", _oldInput);
                _newInput = EditorGUILayout.Toggle("New Input System", _newInput);
                _rewiredInput = EditorGUILayout.Toggle("Rewired", _rewiredInput);
                _charTweener = EditorGUILayout.Toggle("CharTweener", _charTweener);
                _bolt = EditorGUILayout.Toggle("Bolt", _bolt);



                if (GUILayout.Button("Save And Recompile"))
                {
                    //old input
                    if (_oldInput && !symbols.Contains(SurferHelper.OldInputSymbol))
                        symbols += ";" + SurferHelper.OldInputSymbol;
                    else if (!_oldInput && symbols.Contains(SurferHelper.OldInputSymbol))
                    {
                        symbols = symbols.Replace(";" + SurferHelper.OldInputSymbol, "");
                        symbols = symbols.Replace(SurferHelper.OldInputSymbol, "");
                    }

                    //new input
                    if (_newInput && !symbols.Contains(SurferHelper.NewInputSymbol))
                        symbols += ";" + SurferHelper.NewInputSymbol;
                    else if (!_newInput && symbols.Contains(SurferHelper.NewInputSymbol))
                    {
                        symbols = symbols.Replace(";" + SurferHelper.NewInputSymbol, "");
                        symbols = symbols.Replace(SurferHelper.NewInputSymbol, "");
                    }


                    //rewired
                    if (_rewiredInput && !symbols.Contains(SurferHelper.RewiredInputSymbol))
                        symbols += ";" + SurferHelper.RewiredInputSymbol;
                    else if (!_rewiredInput && symbols.Contains(SurferHelper.RewiredInputSymbol))
                    {
                        symbols = symbols.Replace(";" + SurferHelper.RewiredInputSymbol, "");
                        symbols = symbols.Replace(SurferHelper.RewiredInputSymbol, "");
                    }


                    //char tweener
                    if (_charTweener && !symbols.Contains(SurferHelper.CharTweenerSymbol))
                        symbols += ";" + SurferHelper.CharTweenerSymbol;
                    else if (!_charTweener && symbols.Contains(SurferHelper.CharTweenerSymbol))
                    {
                        symbols = symbols.Replace(";" + SurferHelper.CharTweenerSymbol, "");
                        symbols = symbols.Replace(SurferHelper.CharTweenerSymbol, "");
                    }

                    //bolt
                    if (_bolt && !symbols.Contains(SurferHelper.BoltSymbol))
                        symbols += ";" + SurferHelper.BoltSymbol;
                    else if (!_bolt && symbols.Contains(SurferHelper.BoltSymbol))
                    {
                        symbols = symbols.Replace(";" + SurferHelper.BoltSymbol, "");
                        symbols = symbols.Replace(SurferHelper.BoltSymbol, "");
                    }


                    //set all symbols

                    foreach (BuildTargetGroup value in _bGroups)
                        PlayerSettings.SetScriptingDefineSymbolsForGroup(value, symbols);
                }


            }


            void AddStatesSection()
            {

                EditorGUILayout.Space();


                //states stuff

                GUIStyle style = new GUIStyle();
                style.fontStyle = FontStyle.Bold;
                style.normal.textColor = Color.white;
                EditorGUILayout.LabelField("States", style);

                _states = SurferHelper.SO.GetStatesList();


                using (var horiz = new GUILayout.HorizontalScope())
                {

                    if (_justRenamed)
                    {
                        _idx = System.Array.IndexOf(_states, _renamed);
                        _justRenamed = false;
                    }
                    if (_justAdded)
                    {
                        _idx = System.Array.IndexOf(_states, _newName);
                        _justAdded = false;
                    }

                    _idx = EditorGUILayout.Popup("Modify :", _idx, _states, GUILayout.Width(350));

                    if (_idx != 0)
                    {
                        if (GUILayout.Button("Delete"))
                        {
                            if (EditorUtility.DisplayDialog("Delete", "Are you sure you want to delete state " + _states[_idx] + " ?", "Yes", "No"))
                            {
                                SurferHelper.SO.RemoveState(_states[_idx]);
                                EditorUtility.SetDirty(SurferHelper.SO);
                            }

                        }
                    }




                }

                using (var hor = new GUILayout.HorizontalScope())
                {

                    if (_idx != 0)
                    {
                        _renamed = EditorGUILayout.TextField("Rename to :", _renamed, GUILayout.Width(350));


                        if (GUILayout.Button("Rename"))
                        {
                            SurferHelper.SO.RenameState(SurferHelper.SO.GetStateKey(_states[_idx]), _renamed);
                            EditorUtility.SetDirty(SurferHelper.SO);
                            _justRenamed = true;
                        }
                    }

                }

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Add new");

                using (var vert = new GUILayout.HorizontalScope())
                {

                    _newName = EditorGUILayout.TextField("State name: ", _newName, GUILayout.Width(350));


                    if (GUILayout.Button("Add"))
                    {
                        SurferHelper.SO.AddState(_newName);
                        EditorUtility.SetDirty(SurferHelper.SO);
                        _justAdded = true;
                    }
                }

                EditorGUILayout.Space();

            }
        }





        static void RebuildConstants()
        {
            string location = "/Surfer/Runtime/Scripts/";
            string folderPath = Application.dataPath + location;

            string fileName = "SUConsts";

            File.WriteAllText(folderPath + fileName + ".cs", GetClassContent(fileName, SurferHelper.SO.GetStatesList()
                .Concat(SurferHelper.SO.GetScenesNameStrings(true))
                .Concat(SurferHelper.SO.GetEventsList(true)).ToArray()));
            AssetDatabase.ImportAsset("Assets/" + location + fileName + ".cs", ImportAssetOptions.ForceUpdate);

        }

        private static string GetClassContent(string className, string[] labelsArray)
        {
            string output = "";
            output += "//This class is auto-generated do not modify \n";
            output += "namespace Surfer \n";
            output += "{\n";
            output += "public class " + className + "\n";
            output += "{\n";
            foreach (string label in labelsArray)
            {
                if (label == SurferHelper.Unset)
                    continue;

                output += "\t" + BuildConstVariable(label) + "\n";
            }
            output += "}\n";
            output += "}";
            return output;
        }


        private static string BuildConstVariable(string varName)
        {
            if (varName.Contains("*Scene"))
                return "public const string SCENE_" + ToUpperCaseWithUnderscores(varName.Replace("*Scene", "")) + " = " + '"' + varName.Replace("*Scene", "") + '"' + ";";

            if (varName.Contains("*Event"))
                return "public const string EVENT_" + ToUpperCaseWithUnderscores(varName.Replace("*Event", "")) + " = " + '"' + varName.Replace("*Event", "") + '"' + ";";

            return "public const string " + ToUpperCaseWithUnderscores(varName) + " = " + '"' + varName + '"' + ";";
        }

        private static string ToUpperCaseWithUnderscores(string input)
        {
            string output = "" + input[0];

            for (int n = 1; n < input.Length; n++)
            {
                if ((char.IsUpper(input[n]) || input[n] == ' ') && !char.IsUpper(input[n - 1]) && input[n - 1] != '_' && input[n - 1] != ' ')
                {
                    output += "_";
                }

                if (input[n] == '-')
                    output += "";


                if (input[n] != ' ' && input[n] != '_' && input[n] != '-')
                {
                    output += input[n];
                }
            }

            output = output.ToUpper();
            return output;
        }


    }

}

