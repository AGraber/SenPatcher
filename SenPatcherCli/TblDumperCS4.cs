﻿using HyoutaPluginBase;
using HyoutaUtils;
using HyoutaUtils.Streams;
using SenLib;
using SenLib.Sen4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenPatcherCli.Sen4 {
	public enum TblType {
		item,
		item_q,
	}

	public class TblDumper {
		public Tbl BaseTbl;

		public TblDumper(DuplicatableStream stream, EndianUtils.Endianness e = EndianUtils.Endianness.LittleEndian) {
			BaseTbl = new Tbl(stream, e);
		}

		public TblType? IdentifyEntry(int index) {
			try {
				return (TblType)Enum.Parse(typeof(TblType), BaseTbl.Entries[index].Name);
			} catch (Exception) {
				Console.WriteLine("no entry for {0}", BaseTbl.Entries[index].Name);
				return null;
			}
		}

		public static void Dump(string filenametxt, string filenametbl) {
			var tbl = new TblDumper(new HyoutaUtils.Streams.DuplicatableFileStream(filenametbl), EndianUtils.Endianness.LittleEndian);
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < tbl.BaseTbl.Entries.Count; ++i) {
				DuplicatableByteArrayStream stream;
				TblType? tblType = tbl.IdentifyEntry(i);
				switch (tblType) {
					case TblType.item:
					case TblType.item_q: {
							sb.Append("[").Append(i).Append("] ");
							sb.Append(tbl.BaseTbl.Entries[i].Name).Append(":");
							stream = new DuplicatableByteArrayStream(tbl.BaseTbl.Entries[i].Data);
							List<string> postprint = new List<string>();
							sb.AppendFormat(" Idx {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16()); // usable by who
							postprint.Add(stream.ReadUTF8Nullterm().Replace("\n", "{n}"));
							sb.Append("\n");
							// effects when used/equipped, in groups?
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.Append("\n");
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.Append("\n");
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.Append("\n");
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.Append("\n");
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.Append("\n");
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.Append("\n");
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.Append("\n");
							// stats when equipped?
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.Append("\n");
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.Append("\n");
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.Append("\n");
							postprint.Add(stream.ReadUTF8Nullterm().Replace("\n", "{n}"));
							postprint.Add(stream.ReadUTF8Nullterm().Replace("\n", "{n}"));
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.AppendFormat(" {0:x4}", stream.ReadUInt16());
							sb.Append("\n");
							while (true) {
								int b = stream.ReadByte();
								if (b == -1) break;
								sb.AppendFormat(" {0:x2}", b);
							}
							foreach (string s in postprint) {
								sb.AppendFormat("\n{0}", s);
							}
							sb.Append("\n\n");
							break;
						}
					default:
						sb.Append("[").Append(i).Append("] ");
						sb.Append(tbl.BaseTbl.Entries[i].Name).Append(":");
						foreach (byte b in tbl.BaseTbl.Entries[i].Data) {
							sb.AppendFormat(" {0:x2}", b);
						}
						sb.Append("\n");
						break;
				}
			}
			System.IO.File.WriteAllText(filenametxt, sb.ToString());
		}

		public static void InjectItemsIntoSaveFile(string savefilename, string itemtblfilename) {
			var tbl = new TblDumper(new HyoutaUtils.Streams.DuplicatableFileStream(itemtblfilename), EndianUtils.Endianness.LittleEndian);
			List<ushort> itemIds = new List<ushort>();
			for (int i = 0; i < tbl.BaseTbl.Entries.Count; ++i) {
				TblType? tblType = tbl.IdentifyEntry(i);
				if (tblType == TblType.item || tblType == TblType.item_q) {
					var stream = new DuplicatableByteArrayStream(tbl.BaseTbl.Entries[i].Data);
					itemIds.Add(stream.ReadUInt16());
				}
			}

			using (var fs = new FileStream(savefilename, FileMode.Open, FileAccess.ReadWrite)) {
				fs.Position = 0x36f1c;
				for (int i = 0; i < 0x1800; ++i) {
					if (i < itemIds.Count) {
						fs.WriteUInt16(itemIds[i]);
						fs.WriteUInt16(95);
						fs.Position += 0x1c;
					} else {
						fs.WriteUInt16(9999);
						fs.WriteUInt16(0);
						fs.Position += 0x1c;
					}
				}

				// there's a save checksum in this game, it's checked at 0x1403e7077 in the english executable, just break there and make the cmp succeed to load...
			}

			return;
		}
	}
}
