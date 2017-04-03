# microservice-consul-csharp
Example of self monitoring micro service using Consul.io and Microphone.  Written in C# using Web Api and OWIN

This sample application shows how to build a self hosted web api that uses Microphone to register itself with consul.io.  It implements a web api controller (status) for health checking by 
the server an provides a very basic mechanism for the service to ensure the consul.io server is running and recover from server failures.

Instructions:
-install and run consul.io (https://consul.io).  
-Open the solution in VS2017 and build it (ensure you do a nuget restore!).  
-From a command prompt Run the service: Microservice-example.exe [port] [version]  eg: Microservice-example.exe 4001 v1  
-Multiple instances of the service can be run by specifying a different port.  
-The consul.io web UI should show the services running. Stop the service to show failure in the consul web UI.  
  
Once everything is running stop the consul.io service to show the service detect the failure and try to re-register itself.  
