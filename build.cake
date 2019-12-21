#tool nuget:?package=NUnit.ConsoleRunner&version=3.10.0
#addin nuget:?package=Cake.ExtendedNuGet&version=2.1.1

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var solution = "Service.Template.sln";

var binaries = "./Sources/Service.Template.Instance/bin";
var objects = "./Sources/Service.Template.Instance/obj";
var packages = "./artefacts/packages";
var templateNuspec = "./template.package";
var serviceNuspec = "./Sources/Service.Template.Instance/Service.Template.Instance.package";

Task("Clean")
    .Does(() =>
{
    CleanDirectories(new[] {packages, binaries, objects});
});

Task("Restore")
    .Does(() =>
{
    DotNetCoreRestore();
});

Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .Does(() =>
{
		DotNetCoreBuild (solution, new DotNetCoreBuildSettings {
			Configuration = configuration
		});
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
    NuGetPack(serviceNuspec, new NuGetPackSettings{ OutputDirectory = packages });
});

Task("Template").IsDependentOn("Build").Does(() =>
{
    EnsureDirectoryExists(packages);
    NuGetPack(templateNuspec, new NuGetPackSettings{ OutputDirectory = packages });
});

Task("Default")
    .IsDependentOn("Tests")
    .IsDependentOn("Package");

RunTarget(target);
