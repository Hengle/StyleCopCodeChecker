using System.Collections.Generic;
using UnityEngine;

namespace CodeChecker
{
    /// <summary>
    /// コードチェックするパス
    /// </summary>
    public class CodeCheckPath
    {
        /// <summary>
        /// StyleCop のチェック対象にするディレクトリを指定
        /// </summary>
        public readonly List<string> TargetPathList = new List<string>
        {
            Application.dataPath + "/Test/",
        };
    }
}