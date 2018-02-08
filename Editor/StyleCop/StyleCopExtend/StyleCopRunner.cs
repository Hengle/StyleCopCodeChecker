using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace StyleCopExtend
{
    /// <summary>
    /// StyleCopを実行する
    /// </summary>
    public static class StyleCopRunner
    {
        /// <summary>
        /// StyleCop の結果を返すイベント
        /// </summary>
        public static Action<List<StyleCopData>> OnFinishedStyleCopAction;

        /// <summary>
        /// monoの場所
        /// "brew install mono" でmonoをインストールするか、パスを変更してください。
        /// </summary>
#if !UNITY_EDITOR_WIN
        private const string MonoCommand = "/usr/local/bin/mono";
#endif
#if UNITY_EDITOR_WIN
/// <summary>
/// XBuild の場所
/// </summary>
        private const string XBuildCommand = @"C:\Program Files\Unity\Editor\Data\MonoBleedingEdge\bin\xbuild.bat";

        /// <summary>
        /// StyleCop ビルド時の引数
        /// </summary>
        private const string XBuildArgs = " /p:Platform=\"Any CPU\" ";
#else
        /// <summary>
        /// XBuild の場所
        /// </summary>
        private const string XBuildCommand = "/Library/Frameworks/Mono.framework/Versions/Current/Commands/xbuild";

        /// <summary>
        /// StyleCop ビルド時の引数
        /// </summary>
        private const string XBuildArgs = "";
#endif
        /// <summary>
        /// StyleCopのディレクトリ
        /// https://github.com/Nylle/StyleCop.Console を利用しています。
        /// こちらのリポジトリをCloneしたフォルダを指定してください。
        /// </summary>
        private static readonly string StyleCopDirectory = Application.dataPath + "/Editor/StyleCop/StyleCop.Console/";

        /// <summary>
        /// StyleCop実行ファイルのパス
        /// </summary>
        private static readonly string StyleCopExePath =
            StyleCopDirectory + "StyleCop.Console/bin/Debug/StyleCop.Console.exe";

        /// <summary>
        /// 設定ファイルとオプションをArgsに詰める
        /// </summary>
#if UNITY_EDITOR_WIN
        private static readonly string Args = "-s Settings.StyleCop -p ";
#else
        private static readonly string Args = StyleCopExePath +
                                              " -s " + Application.dataPath +
                                              "/Editor/StyleCop/StyleCop.Console/StyleCop.Console/bin/Debug/Settings.StyleCop -p ";
#endif
        /// <summary>
        /// ログ収集用
        /// </summary>
        private static List<string> _outPutLogs;

        /// <summary>
        /// StyleCop の結果のリスト
        /// </summary>
        private static List<StyleCopData> _styleCopResultDataList;

        /// <summary>
        /// StyleCopを実行する
        /// </summary>
        public static void ExcuteStyleCop(List<string> targetPathList)
        {
            if (_outPutLogs == null)
            {
                _outPutLogs = new List<string>();
            }

            var pathes = string.Join(" ", targetPathList.ToArray());
            RunStyleCop(pathes, OnReceiveOutputData, (_, __) => OnFinished());
            OnFinishedStyleCopAction?.Invoke(_styleCopResultDataList);
        }


        /// <summary>
        /// 出力受け取り時のイベント
        /// </summary>
        private static void OnReceiveOutputData(object sender, DataReceivedEventArgs e)
        {
            _outPutLogs.Add(e.Data + '\n');
        }

        /// <summary>
        /// 終了時のイベント
        /// </summary>
        private static void OnFinished()
        {
            List<StyleCopData> styleCopDataList = new List<StyleCopData>();

            string filePath = "";

            for (int i = 0; i < _outPutLogs.Count; i++)
            {
                if (_outPutLogs[i].StartsWith("Pass") && _outPutLogs[i + 1].StartsWith("  Line"))
                {
                    filePath = "";
                    string[] separateLog = _outPutLogs[i].Split(' ');
                    // TODO:正規表現でマッチさせたい
                    filePath = separateLog[4] + separateLog[6];
                }

                if (_outPutLogs[i].StartsWith("  Line"))
                {
                    const string linePattern = "[0-9]+:";
                    var line = Convert.ToInt32(Regex.Match(_outPutLogs[i], linePattern).ToString().Replace(":", ""));
                    string content = _outPutLogs[i];

                    string warningContent = new Regex("(\\w|\\s|-|:)+\\.").Match(content).ToString();
                    string warningCode = new Regex("\\(\\w+\\)").Match(content).ToString();
                    var data = new StyleCopData(filePath, warningContent, warningCode, line);
                    styleCopDataList.Add(data);
                }
            }

            if (OnFinishedStyleCopAction != null)
            {
                _styleCopResultDataList = styleCopDataList;
            }

            _outPutLogs.Clear();
        }

        /// <summary>
        /// StyleCopを別プロセスで起動する
        /// </summary>
        private static void RunStyleCop(string targetPath, DataReceivedEventHandler dataReceivedEventHandler,
            EventHandler exitedHandler)
        {
            if (!File.Exists(StyleCopExePath))
            {
                StyleCopBuilder.Build(XBuildCommand, StyleCopDirectory, XBuildArgs);
            }

            var startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Normal,
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments = Args + targetPath,
                RedirectStandardOutput = true,
                FileName = StyleCopExePath
            };

#if UNITY_EDITOR_WIN
#else
            startInfo.FileName = MonoCommand;
#endif
            var process = new Process
            {
                StartInfo = startInfo,
                EnableRaisingEvents = true
            };
            process.OutputDataReceived += dataReceivedEventHandler;
            process.Exited += exitedHandler;

            process.Start();
            process.BeginOutputReadLine();
            process.WaitForExit();
        }
    }
}