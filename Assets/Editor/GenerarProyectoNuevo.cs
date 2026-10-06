using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GenerarProyectoNuevo
{
    const string Scenes="Assets/Scenes";
    static readonly string[] Nombres={"Menu","Pradera","Final"};
    [MenuItem("Entrega/Preparar juego nuevo")]
    public static void Preparar()
    {
        ConfigurarSpritesPixelArt();
        Directory.CreateDirectory(Scenes);
        foreach(string nombre in Nombres)CrearEscena(nombre);
        EditorBuildSettings.scenes=new[]{new EditorBuildSettingsScene(Scenes+"/Menu.unity",true),new EditorBuildSettingsScene(Scenes+"/Pradera.unity",true),new EditorBuildSettingsScene(Scenes+"/Final.unity",true)};
        PlayerSettings.productName="El Prado Dorado";PlayerSettings.companyName="Crisshubb";PlayerSettings.defaultScreenWidth=1280;PlayerSettings.defaultScreenHeight=720;PlayerSettings.fullScreenMode=FullScreenMode.Windowed;
        EditorSceneManager.OpenScene(Scenes+"/Menu.unity");
        Debug.Log("Proyecto preparado: escenas limpias Menu, Pradera y Final. El contenido se construye en runtime.");
    }
    static void ConfigurarSpritesPixelArt()
    {
        foreach(string guid in AssetDatabase.FindAssets("t:Texture2D",new[]{"Assets/Resources/Art"}))
        {
            string ruta=AssetDatabase.GUIDToAssetPath(guid);var importer=AssetImporter.GetAtPath(ruta) as TextureImporter;
            if(importer==null)continue;
            bool cambio=importer.textureType!=TextureImporterType.Sprite||importer.spritePixelsPerUnit!=64||importer.filterMode!=FilterMode.Point||importer.mipmapEnabled||importer.textureCompression!=TextureImporterCompression.Uncompressed||!importer.alphaIsTransparency;
            if(!cambio)continue;
            importer.textureType=TextureImporterType.Sprite;importer.spriteImportMode=SpriteImportMode.Single;importer.spritePixelsPerUnit=64;
            importer.filterMode=FilterMode.Point;importer.mipmapEnabled=false;importer.textureCompression=TextureImporterCompression.Uncompressed;
            importer.alphaIsTransparency=true;importer.wrapMode=TextureWrapMode.Clamp;importer.SaveAndReimport();
        }
    }
    static void CrearEscena(string nombre)
    {EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);EditorSceneManager.SaveScene(SceneManager.GetActiveScene(),Scenes+"/"+nombre+".unity");}
    [MenuItem("Entrega/Validar escenas y build")]
    public static void Validar()
    {
        if(EditorBuildSettings.scenes.Length!=Nombres.Length)throw new BuildFailedException("El build debe tener las tres escenas del juego.");
        for(int i=0;i<Nombres.Length;i++)
        {
            string ruta=Scenes+"/"+Nombres[i]+".unity";
            if(!File.Exists(ruta)||!EditorBuildSettings.scenes[i].enabled||EditorBuildSettings.scenes[i].path!=ruta)throw new BuildFailedException("Escena ausente o fuera de orden: "+ruta);
            Scene escena=EditorSceneManager.OpenScene(ruta,OpenSceneMode.Single);
            foreach(var root in escena.GetRootGameObjects())foreach(var comp in root.GetComponentsInChildren<Component>(true))if(comp==null)throw new BuildFailedException("Hay un componente faltante en "+ruta+" / "+root.name);
        }
        Debug.Log("VALIDACION_OK: escenas Menu, Pradera y Final listas en el orden correcto.");
    }
    [MenuItem("Entrega/Compilar ejecutable Windows")]
    public static void CompilarWindows()
    {
        Preparar();Validar();string project=Directory.GetParent(Application.dataPath).FullName;string output=Path.Combine(project,"Entregables","ElPradoDorado_Windows");Directory.CreateDirectory(output);
        var report=BuildPipeline.BuildPlayer(new BuildPlayerOptions{scenes=new[]{Scenes+"/Menu.unity",Scenes+"/Pradera.unity",Scenes+"/Final.unity"},locationPathName=Path.Combine(output,"ElPradoDorado.exe"),target=BuildTarget.StandaloneWindows64,options=BuildOptions.None});
        if(report.summary.result!=BuildResult.Succeeded)throw new BuildFailedException("Build Windows falló: "+report.summary.result);
        Debug.Log("BUILD_WINDOWS_OK "+report.summary.totalSize+" bytes en "+output);
    }
}
