#if UNITY_EDITOR
using System.Globalization;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace _Scripts.Infrastructure.Debuging
{
  public class MobModelWindow : EditorWindow
  {
    [MenuItem("Debug/Gameplay/Mob Model Debugger")]
    public static void Open()
    {
      GetWindow<MobModelWindow>("Mob Model Debugger");
    }

    private Vector2 _scroll;

    private void OnGUI()
    {
      GUILayout.Label("Active MobModels", EditorStyles.boldLabel);
      EditorGUILayout.Space();

      _scroll = EditorGUILayout.BeginScrollView(_scroll);

      if (MobModelRegistry.Models.Count == 0)
      {
        EditorGUILayout.HelpBox("Have no active mobs.", MessageType.Info);
      }

      foreach (var model in MobModelRegistry.Models.Where(model => model))
      {
        EditorGUILayout.BeginVertical("box");

        EditorGUILayout.LabelField("Object", model.gameObject.name);
        EditorGUILayout.LabelField("Actor", model.ActorNumber.Value.ToString());
        EditorGUILayout.LabelField("BehaviourType", model.BehaviourType.Value.ToString());
        EditorGUILayout.LabelField("MobType", model.MobType.Value.ToString());
        EditorGUILayout.LabelField("SpawnPosition", model.SpawnPosition.Value.ToString());
        EditorGUILayout.LabelField("IsEnable", model.IsEnable.Value.ToString());
        EditorGUILayout.LabelField("Dead", model.IsDead.Value.ToString());
        EditorGUILayout.LabelField("Health", model.Health.Value.ToString(CultureInfo.InvariantCulture));

        if (GUILayout.Button("Select"))
        {
          Selection.activeObject = model.gameObject;
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();
      }

      EditorGUILayout.EndScrollView();

      Repaint();
    }
  }
}
#endif