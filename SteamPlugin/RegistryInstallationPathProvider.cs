using Microsoft.Win32;

namespace SteamPlugin;

public class RegistryInstallationPathProvider : IInstallationPathProvider
{
    private const string RegistryKeyX64 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Valve\Steam";
    private const string RegistryKeyX32 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Valve\Steam";
    
    public string? FindInstallationPath()
    {
        return FindInstallationPath(RegistryKeyX64)
               ?? FindInstallationPath(RegistryKeyX32);
    }

    private static string? FindInstallationPath(string subKey)
    {
        try
        {
            using var steamKey = Registry.LocalMachine.OpenSubKey(subKey);
            return steamKey?.GetValue("InstallPath") as string;
        }
        catch
        {
            return null;
        }
    }
}