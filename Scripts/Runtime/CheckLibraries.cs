using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace Visuals
{
    [Serializable]
    public class CheckLibraries : ScriptableObject
    {
        [SerializeField] private bool Check = false;
        private CheckLibraries() { }

        private static CheckLibraries _instance = null;
        public static CheckLibraries GetInstance()
        {
            if (_instance == null)
            {
                _instance = Resources.Load<CheckLibraries>("CheckLibraries");
            }

            return _instance;
        }

        public static bool Instance
        {
            get
            {
                return GetInstance().Check;
            }
            set
            {
                GetInstance().Check = value;
#if UNITY_EDITOR
                EditorUtility.SetDirty(GetInstance());
#endif
            }
        }
    }
}