# Examinos Garden Hub

----
**Automatic irrigation of garden built on [BigClown](https://www.bigclown.com) technology.** For now it is composed of two modules (ExaGardenRouter and ExaGadrenBrain). These modules are written as "Console App (.Net Core)" and are designed to be deployed to the [HUB](https://www.bigclown.com/#system-concept) (for example Raspberry Pi). Modules are currently being developed and tested on PC with Windows 10.

----
##ExaGardenRouter
ExaGardenRouter is an router for translating MQTT messages received from particular sensors and sent to controls. It changes only Topic, Payload is preserved. Modul is logging messages and it will also send telemetric data to Cloud (Microsoft Azure).

* In current version the router receives messages from temperature sensor (Temperature Tag) and relative humidity sensor (Humidity Tag) situated on [NODE](https://www.bigclown.com/#system-concept) (Core Module - Remote). System Topics of messages from sensors *"nodes/remote/thermometer/…"* and *"nodes/remote/humidity-sensor/…"* - changes to domain  names *"garden/environment/greenhouse-big/temperature"* and *"garden/environment/greenhouse-big/humidity"*
* Messages sent from ExaGadrenBrain *"garden/monitor/greenhouse-big/alert"* transforms to *"nodes/base/light/-/set"* and these messages then control LED on [GATEWAY](https://www.bigclown.com/#system-concept) (Core Module - Base)
* All relevant messages displays on console

##ExaGadrenBrain
ExaGadrenBrain is the controling core of the system, which is monitoring the state of the environment (weather, garden, greenhouse, water tank, …) and it is handling the irrigation and shading techniques (water pump, valves, ventilation, …).

* Receives messages *"garden/environment/greenhouse-big/temperature"* and *"garden/environment/greenhouse-big/humidity"* and if the difference is greater than +-5%, warns the system by sending a message *"garden/monitor/greenhouse-big/alert"*
