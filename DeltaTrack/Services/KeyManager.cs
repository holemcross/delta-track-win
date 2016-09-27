using System;
using System.Linq;
using System.Xml.Linq;

namespace DeltaTrack.Services
{
    public static class KeyManager
    {
        private static string KEYS_FILE_PATH = "Assets/Keys/Keys.xml";

        public static string GetKeyByName(string keyName)
        {
            if (keyName == null) throw new ArgumentNullException(nameof(keyName));

            var document = XDocument.Load(KEYS_FILE_PATH);

            var key = document
                .Element("Keys")
                .Elements("Key")
                .Where(e => e.Attribute("name").Value == keyName)
                .Select(e => e.Attribute("value").Value);
                //.FirstOrDefault(e => e.Attribute("name")?.Value == keyName);

            return key?.First() ?? string.Empty;
        }
    }
}
