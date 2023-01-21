using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Surfer
{

    public struct SUStateInfo
    {
        public string Name { get; private set; }
        public int PlayerID { get; private set; }
        public int Version { get; private set; }
        public object[] CustomData { get; private set; }

        public SUStateInfo(string name,int v,int playerID,params object[] customData)
        {
            Name = name;
            Version = v;
            CustomData = customData;
            PlayerID = playerID;
        }

        public SUStateInfo(string name,int playerID)
        {
            Name = name;
            Version = 0;
            CustomData = null;
            PlayerID = playerID;
        }


        public SUStateInfo(SUElementData data)
        {
            Name = data.StateData.Name;
            Version = 0;
            CustomData = null;
            PlayerID = data.PlayerID;
        }

    }


}


