#tool nuget:?package=vswhere&version=2.4.1

var target = Argument("target", "Build");
var configuration = Argument("configuration", "Release");
var coverageReportPath = Argument<FilePath>("coverageReportPath", "coverage.zip");
var sonarProject = Argument("sonarProject", "jenkinstest");
var sonarKey = Argument("sonarKey", "");
var sonarUrl = Argument("sonarUrl", "https://sonarcloud.io");
var branch = Argument("branch", "master");
var buildNumber = Argument("buildNumber", "1.0");
var sonarOrganization = Argument("sonarOrganization", "bhagavan44-github");


public static class Paths
{
    public static FilePath SolutionFile => "LogTool/LogTool.sln";
    public static DirectoryPath TestResultFolder => "TestResults";
    public static FilePath TestResultFile => "TestResult.trx";
    public static FilePath CoverageFile => "TestResults/Coverage.xml";
    public static DirectoryPath CoverageReportDirectory => "coverage";
}

public FilePath VSTestToolsPath() {
    DirectoryPath vsTestInstallationPath  = VSWhereProducts("*", new VSWhereProductSettings 
    {
        Requires = "Microsoft.VisualStudio.PackageGroup.TestTools.Core"
    }).FirstOrDefault();
    FilePath vsTestPath = (vsTestInstallationPath==null)
    ? null
    : vsTestInstallationPath.CombineWithFilePath("./Common7/IDE/CommonExtensions/Microsoft/TestWindow/vstest.console.exe");
    return vsTestPath;
}