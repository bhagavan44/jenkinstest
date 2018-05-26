#load "build/args.cake"

Task("Clean")
    .Does(() =>
{
    Information("Clean directories");
    CleanDirectories("./LogTool/**/bin");
    CleanDirectories("./LogTool/**/obj");
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
    MSBuild(
        Paths.SolutionFile,
        settings => settings.SetConfiguration(configuration)
        .WithTarget("Build"));    
});

Task("Test")
    .IsDependentOn("Restore")
    .Does(() =>
{
    Information("Testing the code");
});

RunTarget(target);