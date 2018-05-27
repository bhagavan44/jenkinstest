#load "build/args.cake"
#load "build/test.cake"

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

RunTarget(target);