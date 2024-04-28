**System requirements:**
- 2.4 GHz CPU or faster (multi-core recommended)
- Windows 7 or above
- Graphics card with OpenGL 3.2 support

**Required software on Windows:**
- [Microsoft .Net Framework 4.7.2](https://dotnet.microsoft.com/download/dotnet-framework/net472)

**Building on Linux:**

These instructions are for Debian-based distros and were tested with Debian 10 and Ubuntu 18.04. For others it should be similar.

__Note:__ this is experimental. You're pretty much on your own if you encounter any problems with running the application.

- Install Mono. The `mono-complete` package from the Debian repo doesn't include `msbuild`, so you have to install `mono-complete` by following the instructions on the Mono project's website: https://www.mono-project.com/download/stable/#download-lin
- Install additional required packages: `sudo apt install make g++ git libx11-dev mesa-common-dev`
- Go to a directory of your choice and clone the repository (it'll automatically create a `high-voltage-ring` directory in the current directory): `git clone https://git.do.srb2.org/KartKrew/high-voltage-ring.git`
- Compile HVR: `cd high-voltage-ring && make`
- Run HVR: `cd Build && ./builder`

**Links:**
- [HVR repository](https://git.do.srb2.org/KartKrew/high-voltage-ring)
- [HVR issues](https://git.do.srb2.org/KartKrew/high-voltage-ring/-/issues)

Ultimate Zone Builder:
- [SRB2MB thread](https://mb.srb2.org/addons/ultimate-zone-builder.6126/)

Ultimate Doom Builder:
- [UDB repository](https://github.com/UltimateDoomBuilder/UltimateDoomBuilder)
- [Original forum.zdoom.org thread](https://forum.zdoom.org/viewtopic.php?f=232&t=66745)

More detailed info can be found in the **editor documentation** (Refmanual.chm)
