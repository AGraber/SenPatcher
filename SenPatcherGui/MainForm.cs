﻿using HyoutaUtils;
using SenLib;
using SenLib.Sen1;
using SenLib.Sen2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SenPatcherGui {
	public partial class MainForm : Form {
		public MainForm() {
			InitializeComponent();
		}

		private void buttonCs1SteamEn_Click(object sender, EventArgs e) {
			OpenFileGui(Path.Combine(SenCommonPaths.Sen1SteamDir, SenCommonPaths.Sen1EnExePath), new List<SenVersion>() { SenVersion.Sen1_v1_6_En });
		}

		private void buttonCs1SteamJp_Click(object sender, EventArgs e) {
			OpenFileGui(Path.Combine(SenCommonPaths.Sen1SteamDir, SenCommonPaths.Sen1JpExePath), new List<SenVersion>() { SenVersion.Sen1_v1_6_Jp });
		}

		private void buttonCs1GalaxyEn_Click(object sender, EventArgs e) {
			OpenFileGui(Path.Combine(SenCommonPaths.Sen1GalaxyDir, SenCommonPaths.Sen1EnExePath), new List<SenVersion>() { SenVersion.Sen1_v1_6_En });
		}

		private void buttonCs1GalaxyJp_Click(object sender, EventArgs e) {
			OpenFileGui(Path.Combine(SenCommonPaths.Sen1GalaxyDir, SenCommonPaths.Sen1JpExePath), new List<SenVersion>() { SenVersion.Sen1_v1_6_Jp });
		}

		private void buttonCs2SteamEn_Click(object sender, EventArgs e) {
			OpenFileGui(Path.Combine(SenCommonPaths.Sen2SteamDir, SenCommonPaths.Sen2EnExePath), new List<SenVersion>() { SenVersion.Sen2_v1_4_1_En, SenVersion.Sen2_v1_4_2_En });
		}

		private void buttonCs2SteamJp_Click(object sender, EventArgs e) {
			OpenFileGui(Path.Combine(SenCommonPaths.Sen2SteamDir, SenCommonPaths.Sen2JpExePath), new List<SenVersion>() { SenVersion.Sen2_v1_4_1_Jp, SenVersion.Sen2_v1_4_2_Jp });
		}

		private void buttonCs2GalaxyEn_Click(object sender, EventArgs e) {
			OpenFileGui(Path.Combine(SenCommonPaths.Sen2GalaxyDir, SenCommonPaths.Sen2EnExePath), new List<SenVersion>() { SenVersion.Sen2_v1_4_1_En, SenVersion.Sen2_v1_4_2_En });
		}

		private void buttonCs2GalaxyJp_Click(object sender, EventArgs e) {
			OpenFileGui(Path.Combine(SenCommonPaths.Sen2GalaxyDir, SenCommonPaths.Sen2JpExePath), new List<SenVersion>() { SenVersion.Sen2_v1_4_1_Jp, SenVersion.Sen2_v1_4_2_Jp });
		}

		private void buttonManuallySelect_Click(object sender, EventArgs e) {
			using (OpenFileDialog d = new OpenFileDialog()) {
				d.Filter = "Cold Steel executables (ed8*.exe)|ed8*.exe|All files (*.*)|*.*";
				if (d.ShowDialog() == DialogResult.OK) {
					OpenFileGui(d.FileName, null);
				}
			}
		}

		private void OpenFileGui(string path, List<SenVersion> expectedVersions) {
			if (!File.Exists(path)) {
				MessageBox.Show("No file found at " + path + ".");
				return;
			}

			Stream binary = null;
			SenVersion? actualVersion = null;
			try {
				(binary, actualVersion) = SenVersionIdentifier.OpenAndIdentifyGame(path);
			} catch (Exception ex) {
				MessageBox.Show("Error while identifying " + path + ": " + ex.ToString());
				return;
			}

			if (binary == null || actualVersion == null) {
				MessageBox.Show("Could not identify file at " + path + " as any supported Cold Steel executable.");
				return;
			}

			if (actualVersion == SenVersion.Sen1_v1_6_En || actualVersion == SenVersion.Sen1_v1_6_Jp) {
				new Sen1Form(path, binary, actualVersion.Value).ShowDialog();
				return;
			}

			if (actualVersion == SenVersion.Sen2_v1_4_1_En || actualVersion == SenVersion.Sen2_v1_4_1_Jp || actualVersion == SenVersion.Sen2_v1_4_2_En || actualVersion == SenVersion.Sen2_v1_4_2_Jp) {
				new Sen2Form(path, binary, actualVersion.Value).ShowDialog();
				return;
			}

			MessageBox.Show("Internal error?");
		}

		private void buttonCs1SystemDataAuto_Click(object sender, EventArgs e) {
			OpenCs1SystemData(SenCommonPaths.Sen1SystemDataFile);
		}

		private void buttonCs1SystemDataManual_Click(object sender, EventArgs e) {
			using (OpenFileDialog d = new OpenFileDialog()) {
				d.Filter = "CS1 System Data File (" + SenCommonPaths.Sen1SystemDataFilename + ")|" + SenCommonPaths.Sen1SystemDataFilename + "|All files (*.*)|*.*";
				if (d.ShowDialog() == DialogResult.OK) {
					OpenCs1SystemData(d.FileName);
				}
			}
		}

		private void OpenCs1SystemData(string path) {
			if (!File.Exists(path)) {
				MessageBox.Show("No file found at " + path + ".");
				return;
			}

			Sen1SystemData data = null;
			using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read)) {
				if (fs.Length != Sen1SystemData.FileLength) {
					MessageBox.Show("Incorrect filesize for CS1 system data at " + path + ".");
					return;
				}
				data = new Sen1SystemData(fs.CopyToMemory(), HyoutaUtils.EndianUtils.Endianness.LittleEndian);
			}

			new Sen1SystemDataForm(data, path, HyoutaUtils.EndianUtils.Endianness.LittleEndian).ShowDialog();
		}

		private void buttonCs2SystemDataAuto_Click(object sender, EventArgs e) {
			OpenCs2SystemData(SenCommonPaths.Sen2SystemDataFile);
		}

		private void buttonCs2SystemDataManual_Click(object sender, EventArgs e) {
			using (OpenFileDialog d = new OpenFileDialog()) {
				d.Filter = "CS2 System Data File (" + SenCommonPaths.Sen2SystemDataFilename + ")|" + SenCommonPaths.Sen2SystemDataFilename + "|All files (*.*)|*.*";
				if (d.ShowDialog() == DialogResult.OK) {
					OpenCs2SystemData(d.FileName);
				}
			}
		}

		private void OpenCs2SystemData(string path) {
			if (!File.Exists(path)) {
				MessageBox.Show("No file found at " + path + ".");
				return;
			}

			Sen2SystemData data = null;
			using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read)) {
				if (fs.Length != Sen2SystemData.FileLength) {
					MessageBox.Show("Incorrect filesize for CS2 system data at " + path + ".");
					return;
				}
				data = new Sen2SystemData(fs.CopyToMemory(), HyoutaUtils.EndianUtils.Endianness.LittleEndian);
			}

			new Sen2SystemDataForm(data, path, HyoutaUtils.EndianUtils.Endianness.LittleEndian).ShowDialog();
		}
	}
}