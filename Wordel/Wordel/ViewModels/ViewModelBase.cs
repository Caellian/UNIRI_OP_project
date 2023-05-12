using ReactiveUI;

namespace Wordel.ViewModels;

public class ViewModelBase : ReactiveObject
{
    private static System.Resources.ResourceManager resourceMan;
    private static System.Globalization.CultureInfo resourceCulture;

    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    public static System.Resources.ResourceManager ResourceManager {
        get {
            if (resourceMan == null) {
                resourceMan = new System.Resources.ResourceManager("Wordel.Properties.Resources", typeof(ViewModelBase).Assembly);
            }
            return resourceMan;
        }
    }
    
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    public static System.Globalization.CultureInfo Culture {
        get {
            return resourceCulture;
        }
        set {
            resourceCulture = value;
        }
    }

    public static string GetString(string key)
    {
        var localized = ResourceManager.GetString(key, resourceCulture);
        return localized ?? key;
    }
}
