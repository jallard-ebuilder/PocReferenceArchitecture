# Getting Started

- [Getting Started](#getting-started)
  - [Overview](#overview)
  - [Prerequisites](#prerequisites)
  - [Build and Run](#build-and-run)
    - [Get and Build the Source Code](#get-and-build-the-source-code)
    - [Run Local Kafka](#run-local-kafka)
    - [Run the Service](#run-the-service)
  - [Using the Service](#using-the-service)
    - [Root Endpoint](#root-endpoint)
    - [Using the Service](#using-the-service-1)
  - [Exercises](#exercises)
    - [Capture the Events](#capture-the-events)

## Overview

This document describes how to:

- get and build the source code
- start kafka
- run it

## Prerequisites

Installation and working knowledge of the following are required:

- .NET6 - `dotnet --version` must return `6.0.302`, or greater
- Docker - `docker --version` - tested with `20.10.14`
- Git - `git --version` - tested with `2.34.1`
- Kafka - instructions to run a local broker are included in this document

## Build and Run

### Get and Build the Source Code

- `git clone todo`
- `cd todo`
- `dotnet build`

### Run Local Kafka

The solution is configured to use a local instance of Kafka. You may reconfigure it to use any cluster you like. To get started, leave everything as is, and start Kafka docker containers.

The `docker-compose.yml` file defines a Kafka broker and Zookeeper instance. It is a subset of the [Confluent All-In-One Dockerfile](https://github.com/confluentinc/cp-all-in-one/blob/7.1.1-post/cp-all-in-one/docker-compose.yml).

```bash
# go to the folder that contains the docker-compose file
# alternatively, you can run from anywhere by using the `-f` parameter
# From the project root:
cd src/Documentation/Docker/Kafka

# OPTIONAL: If you have any issues starting the containers, it could be because previous
# instances terminated ungracefully.
# this will usually correct the issue:
docker-compose rm -f

# start the containers
docker-compose up -d

# view the containers
docker-compose ps

# create the topics - these are the topics used by the reference application
# note that it takes a few seconds for the broker to start. if you send
# these commands prior to start completion, it will throw an error. Try again.
docker-compose exec broker kafka-topics --bootstrap-server broker:29092 --create --topic demo-topic-1
docker-compose exec broker kafka-topics --bootstrap-server broker:29092 --create --topic demo-topic-2

# explanation:
# 'docker-compose exec broker` indicates that we are going to run a program in the `broker` container
# everything after broker is the literal command to run within the container
# thus, this executes the command `kafka-topics`, with parameters, in the broker container.
```

### Run the Service

Either load the solution into and IDE and click RUN, or RUN it from the command line:

```bash
# from the project root
cd src/ReferenceArchitecture.WebApi
dotnet run
```

## Using the Service

### Root Endpoint

The Root Endpoint provides a list of links.

- demo controller, included in the solution
- swagger - json, yaml, ui
- health endpoint

The endpoint is hosted locally with a self-signed certificate. You will receive certificate based errors and will need to push through them.

- browsers give you a warning with the option to proceed anyway
- postman will present a button to disable the cert check
- you can pass the `-k` parameter to curl commands

The root endpoint returns a json document containing links. If you browse to the endpoint, the browser won't, by default, format the json in a human friendly way.

- if using a browser, install a `json formatter` extension
- is using POSTMAN: all good. It will be properly formatted
- if using curl, you can pipe it to another program for formatting: `curl https://localhost:7108 -k -s | json_pp`

### Using the Service

Demonstrates

- publishing events
  - the controller publishes an event
  - one of the event handlers publishes an event
- consuming events
  - event handlers are assigned to receive events. See `EventHandlers.cs`
  - one of the event handlers generates a new event

Services will emit domain events. Some services will consume domain events. This service demonstrates both.

The `/api/v1/demo/events` demo endpoint allows you, the end user, to publish events. A real service would not expose an endpoint like this; the application code emits events, not the user. But, for demo purposes, this allows you to emit events and see that they are consumed.

The endpoint supports 2 types of events. These emulate File Management activities.

- `FileCreated` - the `FileCreatedLoggerEventHandler` will receive the event and print it to the console. It is just a logger.
- `FileUploaded` - the `PostProcessingEventHandler` is responsible for doing some work on the file once it is uploaded. When the work is complete, the handler publishes a new `FilePostProcessingCompleted` event. This is an actual domain event published by application code.
- `FilePostProcessingCompleted` - receives the event fired by the application code (previous bullet) and logs it.

Combined, these demonstrate that you can publish events, consume events, and create new events while consuming.

Use the Swagger UI, or tool of your choice, to publish messages. Watch the service's terminal output to see that the messages are received.

- if you publish a `FileCreated` event, its event handler will generate console output.
- if you publish a `FileUploaded` event, its event handler will generate console output. You will also see output for the `FilePostProcessingCompleted` event.

Example output after sending a `FileUploaded` event.

```bash
----------------------------------------------------------
           Event received: FileUploaded
           Time between PUBLISH and CONSUME: 7.1749ms
----------------------------------------------------------


----------------------------------------------------------
           Event received: FilePostProcessingCompleted
           Time between PUBLISH and CONSUME: 26.6879ms
----------------------------------------------------------
```

## Exercises

### Capture the Events

Demonstrates

- Dependency Injection

The demo service writes events to the console. Modify the service to:

- add the received events to a list
- return the list when `GET /api/v1/demo/events` is executed.

Steps

- create an object to contain the events - this can be a simple wrapper for a list
- register the new object in the di container (singleton)
- inject the new object into the event handler classes
  - when an event is received, add the event (or some information about it) to the new object
- inject the new object into the demo controller
  - when get is called, return the list of objects.
  - you can't return PpmCloudEvent directly; it isn't serializable.