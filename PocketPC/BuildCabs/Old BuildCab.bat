REM Created by: Robert Werner of PocketPollster.com
REM Created on: September 30, 2005
REM Last Updated: July 10, 2006

REM Note: This file needs to be updated if the structure of 'Desktop\MobileCabs\..." is changed.



"C:\Program Files\Microsoft Visual Studio .NET 2003\CompactFrameworkSDK\v1.0.5000\Windows CE\..\bin\..\bin\cabwiz.exe" "C:\Documents and Settings\Robert Werner\My Documents\Visual Studio Projects\PocketPollster\PocketPC\BuildCabs\PP_PPC.inf" /dest "C:\Documents and Settings\Robert Werner\My Documents\Visual Studio Projects\PocketPollster\PocketPC\cab\Debug" /err CabWiz.PPC.log /cpu ARMV4 ARM SH3 MIPS X86 WCE420X86

xcopy "C:\Documents and Settings\Robert Werner\My Documents\Visual Studio Projects\PocketPollster\PocketPC\cab\Debug\PP_PPC.ARM.CAB" "C:\Documents and Settings\Robert Werner\My Documents\Visual Studio Projects\PocketPollster\Desktop\MobileCabs\wce300\arm" /y

xcopy "C:\Documents and Settings\Robert Werner\My Documents\Visual Studio Projects\PocketPollster\PocketPC\cab\Debug\PP_PPC.ARMV4.CAB" "C:\Documents and Settings\Robert Werner\My Documents\Visual Studio Projects\PocketPollster\Desktop\MobileCabs\wce400\armv4" /y

xcopy "C:\Documents and Settings\Robert Werner\My Documents\Visual Studio Projects\PocketPollster\PocketPC\cab\Debug\PP_PPC.SH3.CAB" "C:\Documents and Settings\Robert Werner\My Documents\Visual Studio Projects\PocketPollster\Desktop\MobileCabs\wce300\sh3" /y

xcopy "C:\Documents and Settings\Robert Werner\My Documents\Visual Studio Projects\PocketPollster\PocketPC\cab\Debug\PP_PPC.X86.CAB" "C:\Documents and Settings\Robert Werner\My Documents\Visual Studio Projects\PocketPollster\Desktop\MobileCabs\wce300\x86" /y

xcopy "C:\Documents and Settings\Robert Werner\My Documents\Visual Studio Projects\PocketPollster\PocketPC\cab\Debug\PP_PPC.X86.CAB" "C:\Documents and Settings\Robert Werner\My Documents\Visual Studio Projects\PocketPollster\Desktop\MobileCabs\wce400\x86" /y
