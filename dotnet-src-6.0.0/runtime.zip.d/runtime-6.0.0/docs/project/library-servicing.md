# How to service a library

This document provides the steps necessary after modifying a library in a servicing branch (where "servicing branch" refers to any branch whose name begins with `release/`).

## Check if a package is generated

If a library is packable (check for the `<IsPackable>true</IsPackable>` property) you'll need to set `<GeneratePackageOnBuild>true</GeneratePackageOnBuild>` in the source project. That is necessary as packages aren't generated by default in servicing releases.

## Determine ServiceVersion

When you make a change to a library & ship it during the servicing release, the `ServicingVersion` must be bumped. This property is found in the library's source project. It's also possible that the property is not in that file, in which case you'll need to add it to the library's source project and set it to 1. If the property is already present in your library's source project, just increment the servicing version by 1.

## Test your changes

All that's left is to ensure that your changes have worked as expected. To do so, execute the following steps:

1. From a clean copy of your branch, run `build.cmd/sh libs -allconfigurations`

2. Check in `bin\packages\Debug` for the existence of your package, with the appropriate package version.

3. Try installing the built package in a test application, testing that your changes to the library are present & working as expected.
   To install your package add your local packages folder as a feed source in VS or your nuget.config and then add a PackageReference to the specific version of the package you built then try using the APIs.

## Approval Process

All the servicing change must go through an approval process. Please create your PR using [this template](https://raw.githubusercontent.com/dotnet/runtime/main/.github/PULL_REQUEST_TEMPLATE/servicing_pull_request_template.md). You should also add `servicing-consider` label to the pull request and bring it to the attention of the engineering lead responsible for the area.
