using System.Diagnostics;

namespace StyleCopExtend
{
    /// <summary>
    /// StyleCopのビルド
    /// </summary>
    public static class StyleCopBuilder
    {
        /// <summary>
        /// StyleCop をビルドする 
        /// </summary>
        public static void Build(string xBuildCommand, string styleCopDirectory, string xBuildArgs)
        {
            var info = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Normal,
                UseShellExecute = false,
                FileName = xBuildCommand,
                Arguments = styleCopDirectory + "StyleCop.sln" + xBuildArgs,
                CreateNoWindow = true,
                RedirectStandardError = true
            };

            var process = new Process
            {
                StartInfo = info
            };

            process.Start();

            var error = process.StandardError.ReadToEnd();
            if (!string.IsNullOrEmpty(error))
            {
                UnityEngine.Debug.Log(error);
            }

            process.WaitForExit();
        }
    }
}