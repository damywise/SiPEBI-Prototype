using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace SipebiPrototype.Core {
  /// <summary>
  /// Helpers for the <see cref="SecureString"/> class
  /// </summary>
  public static class SecureStringHelpers {
    /// <summary>
    /// Unsecures a <see cref="SecureString" to plain text/>
    /// </summary>
    /// <param name="secureString">the secure string</param>
    /// <returns></returns>
    public static string Unsecure(this SecureString secureString) {
      //make sure we have a secure string
      if (secureString == null)
        return string.Empty;

      //get a pointer for an unscure string in memory
      //It uses pointer so that the exact memory location can be controlled, it is like C/C++ code
      var unmanagedString = IntPtr.Zero;
      try {
        //Unsecure the password
        unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
        return Marshal.PtrToStringUni(unmanagedString);
      } finally {
        //Clean up any memory allocation
        Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
      }
    }
  }
}
