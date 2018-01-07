using System;
using System.Runtime.InteropServices;

namespace DataObjects
{
	/// <summary>
	/// Generate GUIDs on the .NET Compact Framework.
	/// </summary>
	public class PocketGuid
	{
		// guid variant types
		private enum GuidVariant
		{
			ReservedNCS = 0x00,
			Standard = 0x02,
			ReservedMicrosoft = 0x06,
			ReservedFuture = 0x07
		}

		// guid version types
		private enum GuidVersion
		{
			TimeBased = 0x01,
			Reserved = 0x02,
			NameBased = 0x03,
			Random = 0x04
		}
			
		// constants that are used in the class
		private class Const
		{
			// number of bytes in guid
			public const int ByteArraySize = 16;
			
			// multiplex variant info
			public const int VariantByte = 8;
			public const int VariantByteMask = 0x3f;
			public const int VariantByteShift = 6;

			// multiplex version info
			public const int VersionByte = 7;
			public const int VersionByteMask = 0x0f;
			public const int VersionByteShift = 4;
		}

		// imports for the crypto api functions
		private class WinApi
		{
			public const uint PROV_RSA_FULL = 1;
			public const uint CRYPT_VERIFYCONTEXT = 0xf0000000;

			[DllImport("coredll.dll")] 
			public static extern bool CryptAcquireContext(
				ref IntPtr phProv, string pszContainer, string pszProvider,
				uint dwProvType, uint dwFlags);

			[DllImport("coredll.dll")] 
			public static extern bool CryptReleaseContext( 
				IntPtr hProv, uint dwFlags);

			[DllImport("coredll.dll")] 
			public static extern bool CryptGenRandom(
				IntPtr hProv, int dwLen, byte[] pbBuffer);
		}
		
		// all static methods
		private PocketGuid()
		{
		}
		
		/// <summary>
		/// Return a new System.Guid object.
		/// </summary>
		public static Guid NewGuid()
		{
			IntPtr hCryptProv = IntPtr.Zero;
			Guid guid = Guid.Empty;
			
			try
			{
				// holds random bits for guid
				byte[] bits = new byte[Const.ByteArraySize];

				// get crypto provider handle
				if (!WinApi.CryptAcquireContext(ref hCryptProv, null, null, 
					WinApi.PROV_RSA_FULL, WinApi.CRYPT_VERIFYCONTEXT))
				{
					throw new SystemException(
						"Failed to acquire cryptography handle.");
				}
		
				// generate a 128 bit (16 byte) cryptographically random number
				if (!WinApi.CryptGenRandom(hCryptProv, bits.Length, bits))
				{
					throw new SystemException(
						"Failed to generate cryptography random bytes.");
				}
				
				// set the variant
				bits[Const.VariantByte] &= Const.VariantByteMask;
				bits[Const.VariantByte] |= 
					((int)GuidVariant.Standard << Const.VariantByteShift);

				// set the version
				bits[Const.VersionByte] &= Const.VersionByteMask;
				bits[Const.VersionByte] |= 
					((int)GuidVersion.Random << Const.VersionByteShift);
			
				// create the new System.Guid object
				guid = new Guid(bits);
			}
			finally
			{
				// release the crypto provider handle
				if (hCryptProv != IntPtr.Zero)
					WinApi.CryptReleaseContext(hCryptProv, 0);
			}
			
			return guid;
		}
	}
}
