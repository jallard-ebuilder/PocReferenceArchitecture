# Reference Architecture

This solution demonstrates how to build event driven services, using:

- .Net6 Web API
- e-Builder Eventing Platform
- Clean Architecture

Included Features

- per e-Builder standards
  - structured logging using Serilog
  - health endpoint
  - Open API 3 (swagger) endpoints (json, yaml, ui)

Table of Contents

- [Getting Started](GettingStarted.md) - get the code, build it, run it, play with it
- [Docker Cheat Sheet](DockerCheatSheet.md)
- Open Items
  - [todo](Todo.md) - pending tweaks, updates, etc
  - [defects](Defects.md) - defects that must be addressed

## Creating a new Service

- create a github repo
- request access to confluent cloud
  - you need a client 
- create kafka topics, with ACLs
