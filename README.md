# Unreal-Unity Communication Package
A bundle of Unreal Project and a Unity Project pre-configured to facilitate inter-communication between both engines.

# Working
The Unity project acts as the server and starts listening at whatever port we specify at, and our IP [By default, I have used localhost but interchanging to any other ip also works.]


The Unreal project acts as the client and connects to our Unity socket. We specify the IP address and the port we want to connect to.
