using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Surfer
{

    /// <summary>
    /// Data to store reaction name/path and runtime logic
    /// </summary>
    public struct PathAction
    {
        public string Path {get;private set;}

        public Action<FuncInput> Action {get;private set;}

        public List<PathField> Fields { get; private set; }

        public PathAction(string path, Action<FuncInput> act)
        {
            Path = path;
            Action = act;
            Fields = new List<PathField>();
        }

        public PathAction(string path, List<PathField> fields, Action<FuncInput> act)
        {
            Path = path;
            Action = act;
            Fields = fields;
        }
    }
}