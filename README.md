# microservice-consul-csharp
Example of self monitoring micro service using Consul.io and Microphone.  Written in C# using Web Api and OWIN

Instructions:
-install and run consul.io (https://consul.io)
-Open the solution in VS2017 and build it.
-From a command prompt Run the service: test1.exe [port] [version]  eg: test1.exe 4001 v1
-Multiple instances of the service can be run by specifying a different port.
-The consul.io web UI should show the services running. Stop the service to show failure in the consul web UI.  Stop the consul.io service to 
show the service detect the failure and try to re-register itself.
