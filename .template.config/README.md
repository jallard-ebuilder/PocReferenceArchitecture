# Reference Architecture Template

## Installation

To insall the template locally for dotnet new to use execute the following command from the folder that contains the ReferenceArchitecuture.sln file:

- Linux/MacOS: ```$ dotnet new --install ./```
- Windows: ```$ dotnet new --install .\```

## Uninstallation

To remove the template from dotnet new, execute the following command from the folder that contains the ReferenceArchitecuture.sln file:

- Linux/MacOS: ```$ dotnet new --uninstall ./```
- Windows: ```$ dotnet new --uninstall .\```

## Use

From the folder that you want to create the new service, run the following command:

- ```$ dotnet new refservice -o <PROJECT_NAME>```