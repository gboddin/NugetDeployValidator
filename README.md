# Nuget Deploy Validator

## Intro

Takes a directory with nupkgs, and checks if their version is already published to a remote Nuget feed.

Return code is :
 - 128 : conflicting versions
 - 1   : missing CLI arguments

## Usage

### Project

```
$ dotnet run --project /app/NugetDeployValidator/NugetDeployValidator /nugets https://api.nuget.org/v3/index.json
Found local package AwesomeLib.Common 2.0.13
WARNING: Package AwesomeLib.Common already deployed at version 2.0.13
$ echo $?
128
```

### Docker

Usable in CI systems

```
$ docker run -v /nugets:/nugets gboo/ndv /nugets https://api.nuget.org/v3/index.json
Found local package AwesomeLib.Common 2.0.13
WARNING: Package AwesomeLib.Common already deployed at version 2.0.13
$ echo $?
128
```