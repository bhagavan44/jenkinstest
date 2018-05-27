#tool nuget:?package=MSBuild.SonarQube.Runner.Tool
#addin nuget:?package=Cake.Sonar
#tool nuget:?package=OpenCover
#tool nuget:?package=ReportGenerator
 
Task("SonarBegin")
  .IsDependentOn("Clean")
  .Does(() => {

    Information($"Sonar Key: {sonarKey}");    
    SonarBegin(new SonarBeginSettings{
        Url = sonarUrl,
        Login = sonarKey,
        Verbose = true,
        Branch = branch,
        OpenCoverReportsPath = Paths.CoverageFile.ToString(),
        Version = buildNumber,
        Key = sonarProject
     });
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
                Register = "path32",
                SkipAutoProps = true
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

Task("SonarEnd")
  .Does(() => {
     SonarEnd(new SonarEndSettings{
        Login = sonarKey,
     });
});

Task("Sonar")
  .IsDependentOn("SonarBegin")
  .IsDependentOn("Coverage")
  .IsDependentOn("SonarEnd");