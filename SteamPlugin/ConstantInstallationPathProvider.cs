namespace SteamPlugin;

// Fallback method of finding the installation path, if registry cannot be accessed.
public class ConstantInstallationPathProvider : IInstallationPathProvider
{
    private readonly string _installationPath;

    public ConstantInstallationPathProvider(string installationPath)
    {
        _installationPath = installationPath;
    }
    
    public string FindInstallationPath()
    {
        return _installationPath;
    }
}