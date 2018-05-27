var target = Argument("target", "Build");
var configuration = Argument("configuration", "Release");
var coverageReportPath = Argument<FilePath>("coverageReportPath", "coverage.zip");


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
