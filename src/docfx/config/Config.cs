// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.Docs.Build
{
    internal sealed class Config
    {
        public static readonly string[] DefaultExclude = new[]
        {
            "_site/**",             // Default output location
            "_localization/**",     // Localization file when using folder convention
            "_themes/**",           // Default template location
        };

        private static readonly string[] s_defaultInclude = new[] { "**/*.{md,yml,json}" };

        /// <summary>
        /// Gets the default site name
        /// </summary>
        public readonly string SiteName = "Docs";

        /// <summary>
        /// Gets the default product name
        /// </summary>
        public readonly string Product = string.Empty;

        /// <summary>
        /// Gets the default docset name
        /// </summary>
        public readonly string Name = string.Empty;

        /// <summary>
        /// Gets the file glob patterns included by the docset.
        /// </summary>
        [JsonConverter(typeof(OneOrManyConverter))]
        public readonly string[] Files = s_defaultInclude;

        /// <summary>
        /// Gets the file glob patterns excluded from this docset.
        /// </summary>
        [JsonConverter(typeof(OneOrManyConverter))]
        public readonly string[] Exclude = Array.Empty<string>();

        /// <summary>
        /// Gets the file groups config.
        /// </summary>
        [JsonConverter(typeof(OneOrManyConverter))]
        public readonly FileGroupConfig[] FileGroups = Array.Empty<FileGroupConfig>();

        /// <summary>
        /// Gets the output config.
        /// </summary>
        public readonly OutputConfig Output = new OutputConfig();

        /// <summary>
        /// Gets the global metadata added to each document.
        /// </summary>
        public readonly GlobalMetadata GlobalMetadata = new GlobalMetadata();

        /// <summary>
        /// {Schema}://{Hostname}/{SiteBasePath}: https://docs.microsoft.com/dotnet
        /// </summary>
        public string BaseUrl { get; private set; } = string.Empty;

        /// <summary>
        /// host name used for generating .xrefmap.json
        /// </summary>
        public string XrefBaseUrl { get; private set; } = string.Empty;

        /// <summary>
        /// The extend file addresses
        /// The addresses can be absolute url or relative path
        /// </summary>
        [JsonConverter(typeof(OneOrManyConverter))]
        public readonly string[] Extend = Array.Empty<string>();

        /// <summary>
        /// Gets whether we are running in legacy mode
        /// </summary>
        public readonly bool Legacy;

        /// <summary>
        /// Gets the file metadata added to each document.
        /// It is a map of `{metadata-name} -> {glob} -> {metadata-value}`
        /// </summary>
        public readonly Dictionary<string, SourceInfo<Dictionary<string, JToken>>> FileMetadata = new Dictionary<string, SourceInfo<Dictionary<string, JToken>>>();

        /// <summary>
        /// Gets a map from source folder path and output URL path.
        /// We rely on a Dictionary behavior that the enumeration order is the same as insertion order if there is no other mutations.
        /// </summary>
        public readonly Dictionary<PathString, PathString> Routes = new Dictionary<PathString, PathString>();

        /// <summary>
        /// Gets the configuration about contribution scenario.
        /// </summary>
        public readonly ContributionConfig Contribution = new ContributionConfig();

        /// <summary>
        /// Gets the map from dependency name to git url
        /// All dependencies need to be restored locally before build
        /// The default value is empty mappings
        /// </summary>
        public readonly Dictionary<PathString, DependencyConfig> Dependencies = new Dictionary<PathString, DependencyConfig>();

        /// <summary>
        /// Gets the document id configuration section
        /// </summary>
        public readonly DocumentIdConfig DocumentId = new DocumentIdConfig();

        /// <summary>
        /// Gets allow custom error code, severity and message.
        /// </summary>
        public readonly Dictionary<string, CustomError> CustomErrors = new Dictionary<string, CustomError>();

        /// <summary>
        /// Gets the authorization keys for required resources access
        /// </summary>
        public readonly Dictionary<string, HttpConfig> Http = new Dictionary<string, HttpConfig>();

        /// <summary>
        /// Gets the configurations related to GitHub APIs, usually related to resolve contributors.
        /// </summary>
        public readonly GitHubConfig GitHub = new GitHubConfig();

        /// <summary>
        /// Gets the configurations related to Microsoft Graph.
        /// </summary>
        public readonly MicrosoftGraphConfig MicrosoftGraph = new MicrosoftGraphConfig();

        /// <summary>
        /// Gets whether warnings should be treated as errors.
        /// </summary>
        public readonly bool WarningsAsErrors;

        /// <summary>
        /// The addresses of xref map files, used for resolving xref.
        /// They should be absolute url or relative path
        /// </summary>
        [JsonConverter(typeof(OneOrManyConverter))]
        public readonly SourceInfo<string>[] Xref = Array.Empty<SourceInfo<string>>();

        /// <summary>
        /// The configurations for localization build
        /// </summary>
        public readonly LocalizationConfig Localization = new LocalizationConfig();

        /// <summary>
        /// Gets the moniker range mapping
        /// </summary>
        public readonly Dictionary<string, SourceInfo<string>> MonikerRange = new Dictionary<string, SourceInfo<string>>();

        /// <summary>
        /// Get the definition of monikers
        /// It should be absolute url or relative path
        /// </summary>
        public readonly SourceInfo<string> MonikerDefinition = new SourceInfo<string>(string.Empty);

        /// <summary>
        /// Get the file path of content validation rules
        /// </summary>
        public readonly SourceInfo<string> MarkdownValidationRules = new SourceInfo<string>(string.Empty);

        /// <summary>
        /// Get the metadata JSON schema file path.
        /// </summary>
        [JsonConverter(typeof(OneOrManyConverter))]
        public readonly SourceInfo<string>[] MetadataSchema = Array.Empty<SourceInfo<string>>();

        /// <summary>
        /// Get the template folder or git repository url (like https://github.com/docs/theme#master)
        /// </summary>
        public readonly PackagePath Template = new PackagePath();

        /// <summary>
        /// Gets the dependency lock file path.
        /// It is an absolute file path or relative file path relative to docset folder.
        /// </summary>
        public readonly SourceInfo<string> DependencyLock = new SourceInfo<string>(string.Empty);

        /// <summary>
        /// When enabled, updated_at for each document will be the last build time
        /// for the latest commit that touches that document.
        /// </summary>
        public readonly bool UpdateTimeAsCommitBuildTime = false;

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            if (Output.LowerCaseUrl)
            {
                BaseUrl = BaseUrl.ToLowerInvariant();
            }
        }
    }
}
