using System.Collections;


namespace Assets.Scripts.Utils
{
    public static class DictionaryExtensions
    {
        public static T GetValueType<T>(this IDictionary dictionary)
        {
            foreach (var item in dictionary.Values )
            {
                if (item is T)
                {
                    return (T)item;
                }
            }
            return default(T);
        }
    }
    
    
    
}