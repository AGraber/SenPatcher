﻿using HyoutaUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenLib {
	public abstract class FileFixBase : FileFix {
		public bool TryApply(string basepath, string backuppath) {
			string subpath = GetSubPath();
			string subtargetpath = GetSubTargetPath();
			string bkpsubpath = GetBackupSubPath();
			string sha1 = GetSha1();
			string path = Path.Combine(basepath, subpath);
			string targetpath = Path.Combine(basepath, subtargetpath);
			string bkppath = Path.Combine(backuppath, bkpsubpath);

			if (TargetFileExists() && subpath != subtargetpath) {
				throw new Exception("Revert not implemented when source != target.");
			}

			if (File.Exists(path)) {
				using (var ms = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read).CopyToMemoryAndDispose()) {
					ms.Position = 0;
					if (SenUtils.CalcSha1(ms) == sha1) {
						// original file is unpatched, copy to backup
						SenUtils.CreateBackupIfRequired(bkppath, ms);

						// and apply
						return DoApplyAndWrite(targetpath, ms);
					}
				}
			}

			// check if backup path has a clean copy
			if (File.Exists(bkppath)) {
				using (var ms = new FileStream(bkppath, FileMode.Open, FileAccess.Read, FileShare.Read).CopyToMemoryAndDispose()) {
					if (SenUtils.CalcSha1(ms) == sha1) {
						return DoApplyAndWrite(targetpath, ms);
					}
				}
			}

			// we have no clean copy
			return false;
		}

		public bool TryRevert(string basepath, string backuppath) {
			string subpath = GetSubPath();
			string subtargetpath = GetSubTargetPath();
			string bkpsubpath = GetBackupSubPath();
			string sha1 = GetSha1();
			string path = Path.Combine(basepath, subpath);
			string targetpath = Path.Combine(basepath, subtargetpath);
			string bkppath = Path.Combine(backuppath, bkpsubpath);

			if (!TargetFileExists()) {
				// target file was newly created, just delete on revert
				return SenUtils.TryDeleteFile(targetpath);
			}

			if (subpath != subtargetpath) {
				throw new Exception("Revert not implemented when source != target.");
			}

			// check if file needs to be reverted in the first place
			if (File.Exists(path)) {
				using (var ms = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read).CopyToMemoryAndDispose()) {
					ms.Position = 0;
					if (SenUtils.CalcSha1(ms) == sha1) {
						// original file is unpatched, we're good
						return true;
					}
				}
			}

			// original file is modified in some way
			// check if backup path has a clean copy
			if (File.Exists(bkppath)) {
				using (var ms = new FileStream(bkppath, FileMode.Open, FileAccess.Read, FileShare.Read).CopyToMemoryAndDispose()) {
					if (SenUtils.CalcSha1(ms) == sha1) {
						// yup, backup has clean copy, just copy this back to the default path
						return SenUtils.TryWriteFile(ms, path);
					}
				}
			}

			// we have no clean copy
			return false;
		}

		public abstract string GetSubPath();

		public virtual string GetSubTargetPath() {
			return GetSubPath();
		}

		public virtual bool TargetFileExists() {
			return true;
		}

		public virtual string GetBackupSubPath() {
			return GetSubPath().Replace('/', '_').Replace('\\', '_');
		}

		public abstract string GetSha1();

		private bool DoApplyAndWrite(string path, Stream bin) {
			DoApply(bin);
			return SenUtils.TryWriteFileIfDifferent(bin, path);
		}

		protected abstract void DoApply(Stream bin);

		public abstract string GetDescription();
	}
}