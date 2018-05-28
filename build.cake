#load "build/args.cake"
#load "build/test.cake"
#tool nuget:?package=GitVersion&version=3.6.5
#tool nuget:?package=GitVersion.CommandLine&version=3.6.5

Task("Clean")
    .Does(() =>
{
    Information("Clean directories and files");
    CleanDirectories("./LogTool/**/bin");
    CleanDirectories("./LogTool/**/obj");
    CleanDirectories(Paths.TestResultFolder.ToString());
    CleanDirectories(Paths.CoverageReportDirectory.ToString());
    if (FileExists(coverageReportPath))
    {
        DeleteFile(coverageReportPath);
    }
});

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() =>
{
    Information("Restoring the nuget packages");
    NuGetRestore(Paths.SolutionFile);
});

Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>
{
    Information("Building the solution");
    MSBuild(
        Paths.SolutionFile,
        settings => settings.SetConfiguration(configuration)
        .WithTarget("ReBuild"));    
});

Task("Version")
    .Does(() =>
{
   var version = GitVersion();
   Information($"Semantic version {version.SemVer}");

   packageVersion = version.NuGetVersion;
   Information($"Nuget version {packageVersion}");

   if(!BuildSystem.IsLocalBuild)
   {
       GitVersion(new GitVersionSettings
       {
           OutputType = GitVersionOutput.BuildServer,
           UpdateAssemblyInfo = true
        });
   }
});

RunTarget(target);