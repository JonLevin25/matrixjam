using UnityEditor;
using System.IO;
using System;
using System.Text;

namespace MatrixJam.TeamMeta
{
    public class ReplaceNameTest : UnityEditor.AssetModificationProcessor
    {
        #region Constants

        private const int MetaPrefixLength = 5;
        private const string Tab = "\t";
        private const string Spaces = "    ";
        private static readonly string LineFeed = Environment.NewLine;
        private static readonly string SingleSpaceBlock = $"{LineFeed}{Spaces}";
        private static readonly string DoubleSpaceBlock = $"{LineFeed}{Spaces}{Spaces}";
        private static readonly string SpacedClosingScope = $"{Spaces}}}{LineFeed}";
        private static readonly string NamespacePreFormat =
            $"namespace MatrixJam.{{0}}{LineFeed}{{{{{LineFeed}{Spaces}public class";
        #endregion
        
        public static void OnWillCreateAsset(string path)
        {
            if (Path.GetFileNameWithoutExtension(path).EndsWith(".cs"))
            {
                string scriptFilePath = path.Remove(path.Length - MetaPrefixLength);
                if (File.Exists(scriptFilePath))
                {
                    StringBuilder builder = new StringBuilder(File.ReadAllText(scriptFilePath));
                    builder.Replace(Tab, Spaces).Replace(SingleSpaceBlock, DoubleSpaceBlock);
                    int lastCurly = builder.ToString().LastIndexOf('}');
                    builder.Insert(lastCurly, SpacedClosingScope);
                    int firstCurly = builder.ToString().IndexOf('{');
                    builder.Insert(firstCurly, Spaces);
                    builder.Replace("public class", string.Format(NamespacePreFormat, GameJamSetup.TeamName));
                    //builder.Replace(Environment.NewLine, LineFeed);
                    File.WriteAllText(scriptFilePath, builder.ToString());
                    AssetDatabase.Refresh();
                }
            }
        }
    }
}