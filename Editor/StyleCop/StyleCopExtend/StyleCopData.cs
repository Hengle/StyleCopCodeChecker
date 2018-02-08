using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace StyleCopExtend
{
    /// <summary>
    /// StyleCop の結果
    /// </summary>
    public class StyleCopData
    {
        /// <summary>
        /// 対象ファイルのフルパス
        /// </summary>
        public readonly string FileFullPath;

        /// <summary>
        /// アセットデータベース上でのパス
        /// </summary>
        public readonly string AssetDatabasePath;

        /// <summary>
        /// StyleCop の警告メッセージ
        /// </summary>
        public readonly string WarningMessage;

        /// <summary>
        /// StyleCop の警告タイプ(SA1600 など)
        /// </summary>
        public readonly string WarningType;

        /// <summary>
        /// 警告エラー
        /// </summary>
        public readonly int Line;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public StyleCopData(string fileFullPath, string warningMessage, string warningType, int line)
        {
            FileFullPath = fileFullPath;
            AssetDatabasePath = new Regex("Assets/.+").Match(FileFullPath).ToString();
            WarningMessage = warningMessage;
            WarningType = warningType;
            Line = line;
        }
    }
}