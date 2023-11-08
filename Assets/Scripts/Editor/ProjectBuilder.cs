using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;

/// <summary>
/// コマンドラインビルドを行う際に実行するためのエディタ拡張(GitHub Actionsでのビルド時など)
/// </summary>

public class ProjectBuilder
{

    [MenuItem("CustomBuild/ReleaseBuild")]
    public static void ReleaseBuild()
    {
        Build(BuildOptions.None, BuildTarget.WebGL);
    }

    [MenuItem("CustomBuild/DevelopBuild")]
    public static void DevelopBuild()
    {
        Build(BuildOptions.Development, BuildTarget.WebGL);
    }

    private static void Build(BuildOptions buildOptions, BuildTarget buildTarget)
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        // ビルド出力先
        buildPlayerOptions.locationPathName = "Build";
        // ビルドの種類
        buildPlayerOptions.options = buildOptions;
        // ビルドするプラットフォームの指定
        buildPlayerOptions.target = buildTarget;

        // ビルドの実行と結果通知
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded");
        }
        else if (summary.result == BuildResult.Failed)
        {
            Debug.LogError("Build Failed");
        }
    }
}