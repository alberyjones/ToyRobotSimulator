# Toy Robot Simulator #

See *.\Toy Robot Simulator.pdf* for details of the coding exercise this is based on.

The main robot simulator library is in the *.\ToyRobotSimulator* folder, with the unit test project located in *.\ToyRobotSimulatorTests*

The *.\ToyRobotSimulator\ToyRobotSimulator.sln* solution includes both projects.

A brief guide to the key classes:

## ToyRobot ##

This is the implementation of the main robot class, including the key methods: *Place(), Move(), Left(), Right(), Report()*

## TableTop ##

This class encapsulates properties of table top on which the toy robot simulator runs.

## ConsoleController ##

This class models the input and output for use directly in the console application, implemented as a discrete controller class for ease of unit testing.

## CommandDefinitions ##

This class encapsulates the definition of commands that can be invoked and allows lookup based on supplied command text. 

This is arguably overkill for the limited number of commands we currently need to support, but is put in place with future extensibility in mind. 

As the number of commands increases, a developer should only need to add code to the *PopulateCommandDefinitions* method (and any supporting methods in the robot class itself) and this will be reflected in the usage message and command line / file support.

## ConsoleControllerTests ##

Runs unit tests on the *ConsoleController* class, loading data from *testData.xml*.

## ToyRobotTests ##

Runs unit tests directly on the *ToyRobot* class, loading data from *testData.xml*.
