# Nuget Deploy Validator

## Intro

Takes a directory with nupkgs, and perfom checks if weither or not they exists in a remote v3 feed.

## Usage

```
$ dotnet run /nugets https://api.nuget.org
Found local package AwesomeLib.Common 2.0.13
WARNING: Package AwesomeLib.Common already deployed at version 2.0.13
$ echo $?
128
```
