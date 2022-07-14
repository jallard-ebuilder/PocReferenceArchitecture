# Reference Architecture Template

See https://github.com/dotnet/templating/wiki for configuration.

## Installation

To insall the template locally for dotnet new to use execute the following command from the folder that contains the ReferenceArchitecuture.sln file:

- Linux/MacOS: ```$ dotnet new --install ./```
- Windows: ```$ dotnet new --install .\```

NOTE: Make sure all extra folders are removed before installing. This includes all bin and obj folders and also folders like .vscode, .ideas, .git, etc. Anything in this folder will get bundled in to the project template.

TODO: Create script that copies the projects to a clean folder before installing.

## Uninstallation

To remove the template from dotnet new, execute the following command from the folder that contains the ReferenceArchitecuture.sln file:

- Linux/MacOS: ```$ dotnet new --uninstall ./```
- Windows: ```$ dotnet new --uninstall .\```

## Use

From the folder that you want to create the new service, run the following command:

- ```$ dotnet new refservice -o <PROJECT_NAME>```