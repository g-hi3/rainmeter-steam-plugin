namespace SteamPlugin;

// TODO: Try to use Steam SDK instead of the steam file reader.
public interface IInstallationPathProvider
{
    public string? FindInstallationPath();
}