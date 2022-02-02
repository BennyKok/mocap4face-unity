// Facemoji Mocap4Face API for Unity
// Fork of @BennyKok's Android wrapper.
// iOS port by Thomas Suarez (@tomthecarrot) from Teleportal (@0xTELEPORTAL)

#if UNITY_EDITOR && UNITY_IOS

using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

namespace Facemoji
{
    public class FacemojiNativePluginBuildIOS
    {
        private const string headerFilePathComponent = "/UnityFramework/UnityFramework.h";
        private static readonly string[] headerFileLineAddenda = new string[] {
            "// BEGIN Facemoji Plugin for Unity",
            "#import \"FacemojiAPIUnityIOS.h\"",
            "// END Facemoji Plugin for Unity"
        };

        private const string projectFilePathComponent = "/Unity-iPhone.xcodeproj/project.pbxproj";
        private const string projectFileTextSearch = "\\/\\* FacemojiAPIUnityIOS\\.h \\*\\/; };";
        private const string projectFileTextReplacement = "/* FacemojiAPIUnityIOS.h */; settings = {ATTRIBUTES = (Public, ); }; };";

        [PostProcessBuild]
        static void OnPostprocessBuild(BuildTarget target, string buildDirectoryPath)
        {
            // Two corrections to the Xcode project must be done for compilation:
            // 1) Import the Facemoji Unity API header in the UnityFramework's implied bridging header
            //    so that it is accessible from Swift.
            // 2) Set the Facemoji Unity API header's target membership visibility from `Project` to `Public`
            //    so that it can be imported by the framework.

            // TODO: this appends to the header file on each build. Subsequent appends are redundant.
            string headerFilePath = buildDirectoryPath + headerFilePathComponent;
            AppendToHeaderFile(headerFilePath);

            string projectFilePath = buildDirectoryPath + projectFilePathComponent;
            ReplaceInFile(projectFilePath, projectFileTextSearch, projectFileTextReplacement);
        }

        private static void AppendToHeaderFile(string filePath)
        {
            StreamWriter writer = new StreamWriter(filePath, true);

            writer.WriteLine();
            foreach (string line in headerFileLineAddenda)
            {
                writer.WriteLine(line);
            }
            
            writer.Close();
        }

        // Derived from https://stackoverflow.com/a/58377834/2617124
        private static void ReplaceInFile(string filePath, string searchText, string replaceText)
        {
            StreamReader reader = new StreamReader(filePath);
            string content = reader.ReadToEnd();
            reader.Close();

            content = Regex.Replace(content, searchText, replaceText);

            StreamWriter writer = new StreamWriter(filePath);
            writer.Write(content);
            writer.Close();
        }
    }
}

#endif