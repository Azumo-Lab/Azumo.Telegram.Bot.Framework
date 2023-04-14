using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace _1Password.TokenGetter
{
    internal class SystemOSVersion
    {
        public static SystemEnum GetSystemEnum()
        {
            SystemEnum SystemEnum;
            string notSupportedSystem = "不支持的系统";
            string notSupportedProcessArchitecture = "不支持的系统架构";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                SystemEnum = SystemEnum.Apple_Universal;
                return SystemEnum;
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                SystemEnum = RuntimeInformation.ProcessArchitecture switch
                {
                    Architecture.X64 => SystemEnum.Windows_AMD64,
                    Architecture.X86 => SystemEnum.Windows_386,
                    _ => throw new NotSupportedException(notSupportedProcessArchitecture),
                };
                return SystemEnum;
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                string OSVersion = RuntimeInformation.OSDescription;
                if (OSVersion.ToLower().IndexOf("FreeBSD".ToLower()) > -1)
                {
                    SystemEnum = RuntimeInformation.ProcessArchitecture switch
                    {
                        Architecture.Arm => SystemEnum.FreeBSD_ARM,
                        Architecture.Arm64 => SystemEnum.FreeBSD_ARM64,
                        Architecture.X64 => SystemEnum.FreeBSD_AMD64,
                        Architecture.X86 => SystemEnum.FreeBSD_386,
                        _ => throw new NotSupportedException(notSupportedProcessArchitecture),
                    };
                }
                else if (OSVersion.ToLower().IndexOf("OpenBSD".ToLower()) > -1)
                {
                    SystemEnum = RuntimeInformation.ProcessArchitecture switch
                    {
                        Architecture.Arm64 => SystemEnum.OpenBSD_ARM64,
                        Architecture.X64 => SystemEnum.OpenBSD_AMD64,
                        Architecture.X86 => SystemEnum.OpenBSD_386,
                        _ => throw new NotSupportedException(notSupportedProcessArchitecture),
                    };
                }
                else
                {
                    SystemEnum = RuntimeInformation.ProcessArchitecture switch
                    {
                        Architecture.Arm => SystemEnum.Linux_ARM,
                        Architecture.Arm64 => SystemEnum.Linux_ARM64,
                        Architecture.X64 => SystemEnum.Linux_AMD64,
                        Architecture.X86 => SystemEnum.Linux_386,
                        _ => throw new NotSupportedException(notSupportedProcessArchitecture),
                    };
                }
                return SystemEnum;
            }
            throw new NotSupportedException(notSupportedSystem);
        }
    }
}
