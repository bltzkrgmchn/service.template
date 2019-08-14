#tool paket:?package=NUnit.ConsoleRunner&group=main
#addin paket:?package=Cake.Paket
#addin paket:?package=Cake.Docker

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var solution = "Facade.Template.sln";

var binaries = "./Sources/Facade.Template.Instance/bin";
var objects = "./Sources/Facade.Template.Instance/obj";
var packages = "./artefacts/packages";
var containers = "./artefacts/containers";

Task("Clean")
    .Does(() =>
{
    CleanDirectories(new[] {packages, binaries, containers, objects});
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

Task("Containerize").IsDependentOn("Build").Does(() =>
    EnsureDirectoryExists(containers);
    var settings = new DockerImageBuildSettings { Tag = new[] {"dockerapp:latest" }};
    DockerBuild(settings, containers);
});

Task("Default")
    .IsDependentOn("Tests")
    .IsDependentOn("Package")
    .IsDependentOn("Containerize");

RunTarget(target);
