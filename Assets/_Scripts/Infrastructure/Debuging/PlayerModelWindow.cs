#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace _Scripts.Infrastructure.Debuging
{
  public class PlayerModelWindow : EditorWindow
  {
    [MenuItem("Debug/Gameplay/Player Model Debugger")]
    public static void Open()
    {
      GetWindow<PlayerModelWindow>("Player Model Debugger");
    }

    private Vector2 _scroll;

    private void OnGUI()
    {
      GUILayout.Label("Active PlayerModels", EditorStyles.boldLabel);
      EditorGUILayout.Space();

      _scroll = EditorGUILayout.BeginScrollView(_scroll);

      if (PlayerModelRegistry.Models.Count == 0)
      {
        EditorGUILayout.HelpBox("Have no active players.", MessageType.Info);
      }

      foreach (var model in PlayerModelRegistry.Models.Where(model => model))
      {
        EditorGUILayout.BeginVertical("box");

        EditorGUILayout.LabelField("Object", model.gameObject.name);
        EditorGUILayout.LabelField("Actor", model.ActorNumber.Value.ToString());
        EditorGUILayout.LabelField("Local", model.IsLocal.Value.ToString());
        EditorGUILayout.LabelField("Dead", model.IsDead.Value.ToString());
        EditorGUILayout.LabelField("Health", model.Health.Value.ToString());

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