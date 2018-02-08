using System.Collections.Generic;
using StyleCopExtend;
using UnityEditor;
using UnityEngine;

namespace CodeChecker
{
    /// <summary>
    /// コードチェッカー
    /// </summary>
    public class CodeChecker : EditorWindow
    {
        /// <summary>
        /// StyleCop の結果のリスト
        /// </summary>
        private static List<StyleCopData> _resultDataList;

        /// <summary>
        /// エディタウィンドウ用の ScrollPosition
        /// </summary>
        private Vector2 _scrollPosition = Vector2.zero;

        /// <summary>
        /// Menu の処理
        /// </summary>
        [MenuItem("Tools/CodeChecker")]
        private static void Open()
        {
            GetWindow<CodeChecker>(typeof(SceneView));
        }

        /// <summary>
        /// GUI
        /// </summary>
        private void OnGUI()
        {
            SetEvent();

            var targetPathList = new CodeCheckPath().TargetPathList;

            if (GUILayout.Button("Code Check"))
            {
                StyleCopRunner.ExcuteStyleCop(targetPathList);
            }

            GUILayout.Space(10);

            if (_resultDataList == null)
            {
                return;
            }

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            foreach (var data in _resultDataList)
            {
                EditorGUILayout.BeginHorizontal(GUI.skin.box);
                if (GUILayout.Button("Open", GUILayout.Width(40)))
                {
                    // Visual Studio for Mac では Line の対応を確認
                    // Mac 版の Rider は指定ファイルを開くのみ
                    UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(data.AssetDatabasePath,
                        data.Line);
                }

                EditorGUILayout.BeginVertical();
                GUILayout.Label(data.WarningType + data.AssetDatabasePath);
                GUILayout.Label(data.WarningMessage);
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();
        }

        /// <summary>
        /// SetEvent
        /// </summary>
        private static void SetEvent()
        {
            StyleCopRunner.OnFinishedStyleCopAction = OnFinishedStyleCop;
        }

        /// <summary>
        /// StyleCop が終了したときに呼ばれる
        /// </summary>
        private static void OnFinishedStyleCop(List<StyleCopData> dataList)
        {
            _resultDataList = dataList;
        }
    }
}