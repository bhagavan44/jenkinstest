var target = Argument("target", "Build");
var configuration = Argument("configuration", "Release");

public static class Paths
{
    public static FilePath SolutionFile => "LogTool/LogTool.sln";
    public static DirectoryPath TestResultFolder => "TestResults";
    public static FilePath TestResultFile => "TestResult.trx";
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
