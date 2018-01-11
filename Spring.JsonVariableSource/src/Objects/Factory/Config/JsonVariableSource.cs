using System;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Spring.Core.IO;
using Spring.Properties;

namespace Spring.Objects.Factory.Config {
    /// <summary>
    ///     Variable-Placeholder for spring.net, that loads variables for configuration from a validate-able json-file.
    /// </summary>
    public class JsonVariableSource : IVariableSource {
        private readonly JObject _variables;

        /// <summary>
        ///     Ctor.
        /// </summary>
        /// <param name="location">location of the json-file.</param>
        public JsonVariableSource(IResource location) {
            if (location == null) {
                throw new ArgumentNullException(nameof(location));
            }

            Location = location;
            _variables = LoadJson(Location);

            if (CanResolveVariable("$schema")) {
                string schemaLocation = ResolveVariable("$schema");
                IResource schemaResource = new FileSystemResource(Location.File.Directory.FullName + "\\" + schemaLocation);
                ValidateResource(_variables, schemaResource);
            }
        }

        /// <summary>
        ///     Convenience property. Gets a single location
        ///     to read properties from.
        /// </summary>
        /// <value>A location to read properties from.</value>
        public IResource Location {
            get;
        }

        /// <summary>
        ///     Before requesting a variable resolution, a client should
        ///     ask, whether the source can resolve a particular variable name.
        /// </summary>
        /// <param name="name">the name of the variable to resolve</param>
        /// <returns><c>true</c> if the variable can be resolved, <c>false</c> otherwise</returns>
        public bool CanResolveVariable(string name) {
            JToken selectToken = _variables.SelectToken(name);
            return selectToken != null;
        }

        /// <summary>
        ///     Resolves variable value for the specified variable name.
        /// </summary>
        /// <param name="name">The name of the variable to resolve.</param>
        /// <returns>
        ///     The variable value if able to resolve, <c>null</c> otherwise.
        /// </returns>
        public string ResolveVariable(string name) {
            JToken selectToken = _variables.SelectToken(name);
            if (selectToken != null) {
                return selectToken.Value<string>();
            } else {
                return null;
            }
        }

        private static JObject LoadJson(IResource location) {
            JObject variables;
            using (Stream stream = location.InputStream) {
                using (StreamReader sr = new StreamReader(stream)) {
                    string json = sr.ReadToEnd();
                    variables = JsonConvert.DeserializeObject<JObject>(json);
                }
            }

            return variables;
        }

        private void ValidateResource(JObject variables, IResource schemaLocation) {
            JObject schemaObject;
            try {
                schemaObject = LoadJson(schemaLocation);
            } catch (Exception e) {
                Debug.WriteLine(Spring.Properties.Resources.Debug_Shema_Not_Found + Environment.NewLine + e);
                return;
            }

            JSchema schema = JSchema.Parse(schemaObject.ToString());
            if (!variables.IsValid(schema)) {
                throw new FormatException(string.Format(Resources.Exception_Invalid_Json, schemaLocation.File.FullName));
            }
        }
    }
}