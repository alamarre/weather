# Weather API

This is a simple .NET Framework Web API that exposes two endpoints:

- `/ping` returns `pong`.
- `/api/weather/{location}` returns a brief weather summary for the specified location by querying [wttr.in](https://wttr.in).

All code is synchronous and the project is built and tested on Windows.
