#load "build/args.cake"
#tool nuget:?package=vswhere&version=2.4.1

Task("Clean")
    .Does(() =>
{
    Information("Clean directories");
    CleanDirectories("./LogTool/**/bin");
    CleanDirectories("./LogTool/**/obj");
    CleanDirectories(Paths.TestResultFolder.ToString());
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
        .WithTarget("Build"));    
});

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
{
    Information("Testing the code");

    VSTest("./**/bin/**/*.Tests.dll", 
        new VSTestSettings() 
        { 
            InIsolation = true,
            ToolPath = VSTestToolsPath(),
            ArgumentCustomization = args => args.Append("/logger:trx;LogFileName=" + Paths.TestResultFile)
        });
});

RunTarget(target);