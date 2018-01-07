2005-10-14 - Notes about this folder

At this time, this folder contains these subfolders:

  wce300
    arm
    x86
  wce400
    armv4
    x86

Each of these bottom-level folders contains 3 files:
 - The NETCF CAB file
 - The OpenNETCF CAB file (2006-06-18 This file was removed because of troublesome install issues on Pocket PC 2002 devices)
 - The Pocket Pollster PPC CAB file


The first two can be found here:
C:\Program Files\Microsoft Visual Studio .NET 2003\CompactFrameworkSDK\v1.0.5000\Windows CE

The third one is built by running this file:
C:\Documents and Settings\Robert Werner\My Documents\Visual Studio Projects\PocketPollster\PocketPC\BuildCabs\BuildCab.bat

*IMPORTANT NOTE*
"PP_PPC.inf", in the same folder as "BuildCab.bat", MUST be edited to reflect what files get compiled into "PP_PPC_xxxx.cab"

2006-05-22: Currently we're including the Debug versions of the DLLs, but eventually these must include the Release versions!

An indication that "BuildCab.bat" is working properly is that several separate command windows will briefly appear.  If they don't, then it isn't.




These CAB files are not backed up by the regular backup procedure because they change infrequently and/or can be regenerated on the fly.
