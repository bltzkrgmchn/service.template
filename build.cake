#tool paket:?package=NUnit.ConsoleRunner&group=main
#addin paket:?package=Cake.Paket&version=4.0.0
#addin paket:?package=Cake.ExtendedNuGet&version=2.1.1

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var solution = "Service.Template.sln";

var binaries = "./Sources/Service.Template.Instance/bin";
var objects = "./Sources/Service.Template.Instance/obj";
var packages = "./artefacts/packages";
var templateNuspec = "service.template.nuspec";

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

Task("PackageTemplate").IsDependentOn("Build").Does(() =>
{
    EnsureDirectoryExists(packages);
    NuGetPack(templateNuspec, new NuGetPackSettings{ OutputDirectory = packages });
});

Task("Default")
    .IsDependentOn("Tests")
    .IsDependentOn("Package");

Task("Template")
    .IsDependentOn("PackageTemplate");

RunTarget(target);
