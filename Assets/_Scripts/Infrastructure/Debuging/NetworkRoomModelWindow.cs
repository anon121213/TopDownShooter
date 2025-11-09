using System.Collections.Generic;
using System.Reflection;
using UniRx;
using UnityEditor;
using UnityEngine;

namespace _Scripts.Infrastructure.Debuging
{
    public class NetworkRoomModelWindow : EditorWindow
    {
        [MenuItem("Debug/Network Room Model")]
        public static void Open()
        {
            GetWindow<NetworkRoomModelWindow>("NetworkRoomModel");
        }

        private Vector2 _scroll;
        private readonly Dictionary<string, bool> _foldouts = new();

        private void OnGUI()
        {
            var model = NetworkRoomModelRegistry.Model;

            if (model == null)
            {
                EditorGUILayout.HelpBox("NetworkRoomModel not registered yet.", MessageType.Warning);
                return;
            }

            _scroll = EditorGUILayout.BeginScrollView(_scroll);
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("ROOM STATE", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Server:", model.IsServer.Value.ToString());
            EditorGUILayout.LabelField("ConnectionState:", model.ConnectionState.Value.ToString());
            EditorGUILayout.Space();

            DrawDict("Players DTO", model.PlayersDto);
            DrawDict("Players (Root Models)", model.PlayersRoot);

            DrawDict("Mobs DTO", model.MobsDto);
            DrawDict("Mobs (Root Models)", model.MobsRoot);

            DrawDict("Projectiles DTO", model.ProjectilesDto);

            EditorGUILayout.EndScrollView();
        }

        /* =============================
           UNIVERSAL DRAW FUNCTION
        ============================== */

        private void DrawDict<TKey, TValue>(string label, IReadOnlyReactiveDictionary<TKey, TValue> dict)
        {
            EnsureFoldout(label);

            _foldouts[label] = EditorGUILayout.Foldout(
                _foldouts[label],
                $"{label} ({dict.Count})",
                true,
                EditorStyles.foldoutHeader
            );

            if (!_foldouts[label]) return;

            EditorGUI.indentLevel++;

            foreach (var kvp in dict)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.LabelField($"Key: {kvp.Key}", EditorStyles.boldLabel);

                DrawValue(kvp.Value);

                EditorGUILayout.EndVertical();
                EditorGUILayout.Space(3);
            }

            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
        }


        /* =============================
           VALUE RENDERING LOGIC
        ============================== */

        private void DrawValue(object value)
        {
            if (value == null)
            {
                EditorGUILayout.LabelField("null");
                return;
            }

            // Unity Object reference
            if (value is Object unityObj)
            {
                EditorGUILayout.LabelField("Object: " + unityObj.name);

                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Ping", GUILayout.Width(60)))
                    EditorGUIUtility.PingObject(unityObj);
                if (GUILayout.Button("Select", GUILayout.Width(60)))
                {
                    Selection.activeObject = unityObj;
                    EditorGUIUtility.PingObject(unityObj);
                }
                EditorGUILayout.EndHorizontal();
                return;
            }

            // Primitive / basic types → single line
            var type = value.GetType();
            if (type.IsPrimitive || value is string || value is Vector3 || value is Quaternion)
            {
                EditorGUILayout.LabelField(value.ToString());
                return;
            }

            // DTO / struct / class → Display fields & readable properties
            EditorGUI.indentLevel++;

            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var f in fields)
            {
                var val = f.GetValue(value);
                EditorGUILayout.LabelField($"{f.Name}: {val}");
            }

            PropertyInfo[] props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var p in props)
            {
                if (!p.CanRead || p.GetIndexParameters().Length > 0) 
                    continue;

                var val = p.GetValue(value);
                EditorGUILayout.LabelField($"{p.Name}: {val}");
            }

            EditorGUI.indentLevel--;
        }


        private void EnsureFoldout(string key)
        {
            if (!_foldouts.ContainsKey(key))
                _foldouts[key] = false;
        }
    }
}
