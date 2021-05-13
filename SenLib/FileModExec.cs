﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SenLib {
	public class FileModExec {
		private static HyoutaUtils.HyoutaArchive.HyoutaArchiveContainer TryLoadBackupArchive(string backupArchivePath) {
			try {
				if (File.Exists(backupArchivePath)) {
					Console.WriteLine("Loading backup archive at {0}...", backupArchivePath);
					using (var fs = new HyoutaUtils.Streams.DuplicatableFileStream(backupArchivePath)) {
						return new HyoutaUtils.HyoutaArchive.HyoutaArchiveContainer(fs);
					}
				}
			} catch (Exception ex) {
				Console.WriteLine("Failed to load backup file archive: {0}", ex.Message);
			}
			return null;
		}

		public static FileStorage InitializeAndPersistFileStorage(string baseDir, KnownFile[] knownFiles) {
			if (!Directory.Exists(baseDir)) {
				Console.WriteLine("No directory found at {0}.", baseDir);
				return null;
			}

			string backupArchivePath = Path.Combine(baseDir, "senpatcher_rerun_revert_data.bin");
			FileStorage.InitReturnValue fileStoreReturnValue = null;
			using (HyoutaUtils.HyoutaArchive.HyoutaArchiveContainer backupArchive = TryLoadBackupArchive(backupArchivePath)) {
				fileStoreReturnValue = FileStorage.InitializeFromKnownFiles(baseDir, knownFiles, backupArchive);
			}

			if (fileStoreReturnValue.ShouldWriteBackupArchive) {
				Console.WriteLine("New file(s) found, writing new backup archive to {0}...", backupArchivePath);
				Stream ms = new MemoryStream();
				fileStoreReturnValue.Storage.WriteToHyoutaArchive(ms);
				using (var fs = new FileStream(backupArchivePath, FileMode.Create)) {
					ms.Position = 0;
					HyoutaUtils.StreamUtils.CopyStream(ms, fs);
				}
			}

			return fileStoreReturnValue.Storage;
		}

		public static PatchResult ExecuteMods(string gamedir, FileStorage storage, List<FileMod> mods, ProgressReporter progress) {
			return ExecuteModsInternal(gamedir, storage, mods, progress, false);
		}

		private static PatchResult ExecuteModsInternal(string gamedir, FileStorage storage, List<FileMod> mods, ProgressReporter progress, bool revert) {
			int total = mods.Count;
			int success = 0;

			for (int i = 0; i < mods.Count; i++) {
				FileMod mod = mods[i];
				progress.Message(string.Format("{0}: {1}...", revert ? "Restoring" : "Patching", mod.GetDescription()), i, total);

				var results = mod.TryApply(storage);
				if (results == null) {
					progress.Error(string.Format("Failed to generate data: {0}", mod.GetDescription()));
					continue;
				}

				bool diskWriteSuccess = true;
				foreach (FileModResult result in results) {
					string path = Path.Combine(gamedir, result.TargetPath);
					if (result.ResultData != null) {
						if (!SenUtils.TryWriteFileIfDifferent(result.ResultData, path)) {
							progress.Error(string.Format("Failed to write to disk: {0}", path));
							diskWriteSuccess = false;
						}
					} else {
						if (!SenUtils.TryDeleteFile(path)) {
							progress.Error(string.Format("Failed to delete from disk: {0}", path));
							diskWriteSuccess = false;
						}
					}
				}

				if (diskWriteSuccess) {
					++success;
				}
			}

			progress.Message(string.Format("Finished {0}.", revert ? "restoring files" : "applying patches"), total, total);
			return new PatchResult(total, success);
		}

		public static PatchResult RevertMods(string gamedir, FileStorage storage, IEnumerable<FileMod> mods, ProgressReporter progress) {
			return ExecuteModsInternal(gamedir, storage, InvertMods(mods).ToList(), progress, true);
		}

		public static IEnumerable<FileMod> InvertMods(IEnumerable<FileMod> mods) {
			foreach (FileMod mod in mods) {
				yield return new FileModInvert(mod);
			}
		}
	}
}
