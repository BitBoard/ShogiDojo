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

    /// <summary>
    /// CICDのワークフローからリリースビルドを実行する時はこちらを呼び出す
    /// </summary>
    private static void ReleaseBuildForCICD()
    {
        Build(BuildOptions.None, BuildTarget.WebGL, true);
    }

    /// <summary>
    /// CICDのワークフローから開発ビルドを実行する時はこちらを呼び出す
    /// </summary>
    private static void DevelopBuildForCICD()
    {
        Build(BuildOptions.Development, BuildTarget.WebGL, true);
    }

    private static void Build(BuildOptions buildOptions, BuildTarget buildTarget, bool isCICD = false)
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        
        // ビルド対象のシーン
        buildPlayerOptions.scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes);
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
        
        if (isCICD)
        {
            //成否に応じてUnityEditorの終了プロセスを決定する
            EditorApplication.Exit(summary.result == BuildResult.Succeeded ? 0 : 1);
        }
    }
}