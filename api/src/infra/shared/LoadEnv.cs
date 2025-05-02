namespace Infra.Env;

public static class LoadEnv {

    private static void DefineEnvFromFile (string filePath) {

        if (!File.Exists(filePath)) {
            throw new FileLoadException("Arquivo .env não encontrado!");
        }

        foreach ( var line in File.ReadAllLines(filePath) ) {
            var parts = line.Split( '=', StringSplitOptions.RemoveEmptyEntries);
            
            if (parts.Length != 2) continue;

            Environment.SetEnvironmentVariable(parts[0], parts[1]); 
        }

    }

    public static void Load () {
        var appRoot = Directory.GetCurrentDirectory();
        var dotEnv = Path.Combine(appRoot, ".env");

        DefineEnvFromFile(dotEnv);
        
    }

}