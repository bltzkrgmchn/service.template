#tool nuget:?package=NUnit.ConsoleRunner&version=3.10.0
#addin nuget:?package=Cake.ExtendedNuGet&version=2.1.1
#addin nuget:?package=Cake.FileHelpers&version=3.2.1

var target = Argument ("target", "Default");
var configuration = Argument ("configuration", "Release");

var solution = "Service.Template.sln";
var releaseNotesPath = "./RELEASE_NOTES.md";
var binaries = "./Sources/Service.Template.Instance/bin";
var objects = "./Sources/Service.Template.Instance/obj";
var packages = "./artifacts/packages";
var nuspec = "./package.template";

string version;

Task ("Clean")
    .Does (() => {
        CleanDirectories (new [] { packages, binaries, objects });
        Information ($"Clean complete.");
    });

Task ("Restore")
    .Does (() => {
        var restoreSettings = new DotNetCoreRestoreSettings {
        PackagesDirectory = "./packages"
        };

        DotNetCoreRestore (restoreSettings);
        Information ($"Restore complete.");
    });

Task ("Version")
    .Does (() => {
        version = FileReadLines (releaseNotesPath).FirstOrDefault();
        Information ($"Estimated version is '{version}'.");

        var file = "./Sources/Service.Template.Instance/AssemlyInfo.cs";
        CreateAssemblyInfo(file, new AssemblyInfoSettings {
            Title = "Service.Template",
            Version = version,
            FileVersion = version,
            InformationalVersion = version,
            Copyright = $"Copyright (c) Aleksey Balandin 2019 - {DateTime.Now.Year}"
        });
    });

Task ("Build")
    .IsDependentOn ("Clean")
    .IsDependentOn ("Restore")
    .IsDependentOn ("Version")
    .Does (() => {
        DotNetCoreBuild (solution, new DotNetCoreBuildSettings {
            Configuration = configuration,
                NoRestore = true
        });
        Information ($"Build complete.");
    });

Task ("Publish")
    .IsDependentOn ("Build")
    .IsDependentOn ("Version")
    .Does (() => {
        var settings = new DotNetCorePublishSettings
        {
            Framework = "netcoreapp3.1",
            Configuration = "Release",
            OutputDirectory = "./artifacts/publish",
            NoBuild = true,
            NoRestore = true,
            Runtime = "win-x64"
        };

        DotNetCorePublish("./Sources/Service.Template.Instance/Service.Template.Instance.csproj", settings);
        Information ($"Publish complete.");
    });

Task ("Tests")
    .IsDependentOn ("Publish")
    .Does (() => {
        var testProjects = GetFiles("./Tests/**/*.csproj");
        Information ($"Found '{testProjects.Count}' test projects.");

        var settings = new DotNetCoreTestSettings
        {
            NoBuild = true,
            NoRestore = true,
            Verbosity = DotNetCoreVerbosity.Normal,
            Configuration = "Release"
        };

        foreach(var testProject in testProjects)
        {
            Information ($"Running test on '{testProject.FullPath}' project.");
            DotNetCoreTest(testProject.FullPath, settings);
        }

        Information ($"Tests complete.");
    });

Task ("Package")
    .IsDependentOn ("Publish")
    .IsDependentOn ("Version")
    .Does (() => {
        EnsureDirectoryExists (packages);
        NuGetPack (nuspec, new NuGetPackSettings {
            OutputDirectory = packages,
            Version = version,
            ArgumentCustomization = args=>args.Append("-NoDefaultExcludes")
            });
        Information ($"Package complete.");
    });

Task ("Default")
    .IsDependentOn ("Tests")
    .IsDependentOn ("Package");

RunTarget (target);