﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Helpers;
using Gibe.FileSystem;

namespace LiveLink.CacheBusting
{
	public class ManifestFile
	{
		private readonly IFileService _fileService;
		private readonly string _path;
		private readonly IPaths _paths;
		private readonly string _sourceFile;

		public ManifestFile(IFileService fileService, string path, string sourceFile)
		{
			_path = path;
			_sourceFile = sourceFile;
			_fileService = fileService;

			WatchManifestForChanges(sourceFile);
		}

		public event EventHandler Changed;

		private void WatchManifestForChanges(string file)
		{
			var fsw = new FileSystemWatcher(Path.GetDirectoryName(file), Path.GetFileName(file));
			fsw.NotifyFilter = NotifyFilters.LastWrite;
			fsw.Changed += (sender, args) => OnChanged(args);
			fsw.Created += (sender, args) => OnChanged(args);
			fsw.EnableRaisingEvents = true;
		}

		public Dictionary<string, string> GetManifest()
		{
			var json = _fileService.ReadAllText(_sourceFile);
			var lookups = Json.Decode<Dictionary<string, string>>(json);

			var output = new Dictionary<string, string>();
			foreach (var kvp in lookups)
				output.Add($"{_path}{kvp.Key}", $"{_path}{kvp.Value}");
			return output;
		}

		public void OnChanged(EventArgs e)
		{
			Changed?.Invoke(this, e);
		}
	}
}