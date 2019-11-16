#tool paket:?package=NUnit.ConsoleRunner&group=main
#addin paket:?package=Cake.Paket

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var solution = "Service.Template.sln";

var binaries = "./Sources/Service.Template.Instance/bin";
var objects = "./Sources/Service.Template.Instance/obj";
var packages = "./artefacts/packages";

Task("Clean")
    .Does(() =>
{
    CleanDirectories(new[] {packages, binaries, objects});
});

Task("Restore")
    .Does(() =>
{
    PaketRestore();
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
    PaketPack(packages, new PaketPackSettings { Version = "1.0.0" });
});

Task("Default")
    .IsDependentOn("Tests")
    .IsDependentOn("Package");

RunTarget(target);
