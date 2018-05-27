#load "build/args.cake"
#tool nuget:?package=vswhere&version=2.4.1
#tool nuget:?package=OpenCover
#tool nuget:?package=ReportGenerator

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
        .WithTarget("Build"));    
});

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
{
    Information("Testing the code");
    EnsureDirectoryExists(Paths.TestResultFolder);

    OpenCover(tool=>
        tool.VSTest("./**/bin/**/*.Tests.dll", 
            new VSTestSettings() 
            { 
                InIsolation = false,
                ToolPath = VSTestToolsPath(),
                ArgumentCustomization = args => args.Append("/logger:trx;LogFileName=" + Paths.TestResultFile)
            }),
            Paths.CoverageFile,
            new OpenCoverSettings{
                Register = "Path64"
            }
            .WithFilter("+[*]*")
            .WithFilter("-[*.Tests*]*")
            );
});

Task("Coverage")
    .IsDependentOn("Test")
    .Does(() =>
{
    Information("Generating the report");
    
    ReportGenerator(
        Paths.CoverageFile,
        Paths.CoverageReportDirectory,
        new ReportGeneratorSettings
        {
            ReportTypes = new[] { ReportGeneratorReportType.Html }
        }
    );

    Zip(
        Paths.CoverageReportDirectory,
        MakeAbsolute(coverageReportPath)
    );
});

RunTarget(target);