#tool nuget:?package=MSBuild.SonarQube.Runner.Tool&version=4.2.0
#addin nuget:?package=Cake.Sonar&version=1.1.0
#tool nuget:?package=OpenCover&version=4.6.519
#tool nuget:?package=ReportGenerator&version=3.1.2
 
Task("SonarBegin")
  .IsDependentOn("Clean")
  .Does(() => {

    SonarBegin(new SonarBeginSettings{
        Url = sonarUrl,
        Login = sonarKey,
        Verbose = true,
        Branch = branch,
        OpenCoverReportsPath = Paths.CoverageFile.ToString(),
        Version = packageVersion,
        Key = sonarProject,
        ArgumentCustomization = args => args
            .Append($"/o:{sonarOrganization}"),
     });
});

Task("Test")
    .WithCriteria(()=> BuildSystem.IsRunningOnJenkins)
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
                Register = "path32",
                SkipAutoProps = true
            }
            .WithFilter("+[*]*")
            .WithFilter("-[*.Tests*]*")
            );
});

Task("VSTest")
    .WithCriteria(()=> BuildSystem.IsRunningOnVSTS)
    .IsDependentOn("Build")
    .Does(() =>
{
    Information("Testing the code");
    EnsureDirectoryExists(Paths.TestResultFolder);

    VSTest("./**/bin/**/*.Tests.dll", 
        new VSTestSettings() 
        { 
            InIsolation = true,
            ToolPath = VSTestToolsPath(),
            EnableCodeCoverage = true,
            ArgumentCustomization = args => args.Append("/logger:trx;LogFileName=" + Paths.TestResultFile)
        });

});

Task("Coverage")
    .WithCriteria(()=> BuildSystem.IsRunningOnJenkins)
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

Task("SonarEnd")
  .Does(() => {
     SonarEnd(new SonarEndSettings{
        Login = sonarKey,
     });
});

Task("Sonar")
    .IsDependentOn("Version")
    .IsDependentOn("SonarBegin")
    .IsDependentOn("Coverage")
    .IsDependentOn("SonarEnd");