#tool paket:?package=NUnit.ConsoleRunner&group=main
#addin paket:?package=Cake.Paket

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var packages = "./artefacts/packages";

Task("Clean")
    .Does(() =>
{
    CleanDirectories(new[] {packages});
});

Task("Restore")
    .Does(() =>
{
    PaketRestore();
});

Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .Does(() =>
{
    if(IsRunningOnWindows())
    {
      MSBuild("./Facade.Template.sln", settings =>
        settings.SetConfiguration(configuration));
    }
    else
    {
      XBuild("./Facade.Template.sln", settings =>
        settings.SetConfiguration(configuration));
    }
});

Task("Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    NUnit3("./Tests/**/bin/" + configuration + "/*Tests.dll", new NUnit3Settings {
        NoResults = true
        });
});

Task("Package").IsDependentOn("Build").Does(() =>
{
    EnsureDirectoryExists(packages);
    PaketPack(packages, new PaketPackSettings { Version = "1.0.0" });
});


Task("Default")
    .IsDependentOn("Tests")
    .IsDependentOn("Package");

RunTarget(target);
