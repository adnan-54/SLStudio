using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SlrrLib.Model
{
    public class DynamicRpk
    {
        public string Signature
        {
            get;
            set;
        }

        public int Version512
        {
            get;
            set;
        }

        public int ExternalReferencesUnkownZero
        {
            get;
            set;
        }

        public List<string> ExternalReferences
        {
            get;
            set;
        } = new List<string>();

        public List<DynamicResEntry> Entries
        {
            get;
            set;
        } = new List<DynamicResEntry>();

        public DynamicRpk()
        : this(null)
        {
        }

        public DynamicRpk(BinaryRpk constructFrom = null)
        {
            if (constructFrom == null)
            {
                Signature = "RPAK";
                Version512 = 512;
                ExternalReferencesUnkownZero = 0;
                return;
            }
            Signature = constructFrom.FileHeader.Siganture;
            Version512 = constructFrom.FileHeader.Version512;
            ExternalReferencesUnkownZero = constructFrom.FileHeader.ExternalReferencesUnkownZero;
            foreach (var ext in constructFrom.ExternalReferences)
            {
                ExternalReferences.Add(ext.ReferenceString);
            }
            foreach (var res in constructFrom.RESEntries)
            {
                Entries.Add(new DynamicResEntry(res));
            }
        }

        public void SaveAs(Stream stream)
        {
            foreach (var res in Entries)
            {
                foreach (var strEntr in res.RSD.InnerEntries)
                {
                    if (strEntr is DynamicStringInnerEntry strObj)
                    {
                        if (strObj.StringData != "" && !strObj.StringData.EndsWith("\r\n"))
                            strObj.StringData += "\r\n";
                    }
                }
            }

            using BinaryWriter bw = new BinaryWriter(stream, Encoding.UTF8, true);
            byte[] signatureBuff = new byte[4];
            string signPropLength = Signature;

            while (signPropLength.Length < 4)
                signPropLength += " ";

            Array.Copy(Encoding.ASCII.GetBytes(signPropLength), signatureBuff, 4);

            bw.Write(signatureBuff);
            bw.Write(Version512);
            bw.Write(ExternalReferences.Count);
            bw.Write(ExternalReferencesUnkownZero);

            short ext_i = 1;
            foreach (var extRef in ExternalReferences)
            {
                bw.Write((short)0);
                bw.Write(ext_i);
                ext_i++;
                byte[] extRefBuff = new byte[60];
                Array.Copy(Encoding.ASCII.GetBytes(extRef), extRefBuff, extRef.Length >= extRefBuff.Length ? extRefBuff.Length : extRef.Length);
                bw.Write(extRefBuff);
            }

            bw.Write(entriesSize());
            bw.Write((int)Entries.Count);
            bw.Write((int)Entries.Count(x => x.TypeOfEntry == 1));
            bw.Write((int)Entries.Count(x => x.TypeOfEntry != 1));

            int rsdOffset = (int)bw.BaseStream.Position + entriesSize();
            foreach (var res in Entries)
            {
                res.Save(bw, rsdOffset);
                if (res.RSD != null)
                    rsdOffset += res.RSD.GetSizeIncludingHiddenEntries();
            }

            foreach (var res in Entries)
                res.RSD.Save(bw);

            bw.Flush();
        }

        public void SaveAs(string fnam, bool bak = true, string BakSignature = "_BAK_Rpk_")
        {
            if (bak && File.Exists(fnam))
            {
                int bakInd = 0;
                while (File.Exists(fnam + BakSignature + bakInd.ToString()))
                    bakInd++;
                File.Copy(fnam, fnam + BakSignature + bakInd.ToString());
            }
            foreach (var res in Entries)
            {
                foreach (var strEntr in res.RSD.InnerEntries)
                {
                    var strObj = strEntr as DynamicStringInnerEntry;
                    if (strObj != null)
                    {
                        if (strObj.StringData != "" && !strObj.StringData.EndsWith("\r\n"))
                            strObj.StringData += "\r\n";
                    }
                }
            }
            BinaryWriter bw = new BinaryWriter(File.Create(fnam));
            byte[] signatureBuff = new byte[4];
            string signPropLength = Signature;
            while (signPropLength.Length < 4)
                signPropLength += " ";
            Array.Copy(ASCIIEncoding.ASCII.GetBytes(signPropLength), signatureBuff, 4);
            bw.Write(signatureBuff);
            bw.Write(Version512);
            bw.Write((int)ExternalReferences.Count);
            bw.Write(ExternalReferencesUnkownZero);
            short ext_i = 1;
            foreach (var extRef in ExternalReferences)
            {
                bw.Write((short)0);
                bw.Write(ext_i);
                ext_i++;
                byte[] extRefBuff = new byte[60];
                Array.Copy(ASCIIEncoding.ASCII.GetBytes(extRef), extRefBuff, extRef.Length >= extRefBuff.Length ? extRefBuff.Length : extRef.Length);
                bw.Write(extRefBuff);
            }
            bw.Write(entriesSize());
            bw.Write((int)Entries.Count);
            bw.Write((int)Entries.Count(x => x.TypeOfEntry == 1));
            bw.Write((int)Entries.Count(x => x.TypeOfEntry != 1));
            int rsdOffset = (int)bw.BaseStream.Position + entriesSize();
            foreach (var res in Entries)
            {
                res.Save(bw, rsdOffset);
                if (res.RSD != null)
                    rsdOffset += res.RSD.GetSizeIncludingHiddenEntries();
            }
            foreach (var res in Entries)
            {
                res.RSD.Save(bw);
            }
            bw.Close();
        }

        public List<DynamicSpatialNode> SpatialStructs()
        {
            List<DynamicSpatialNode> ret = new List<DynamicSpatialNode>();
            foreach (var entr in Entries)
            {
                foreach (var rsd in entr.RSD.InnerEntries)
                {
                    if (rsd is DynamicSpatialNode)
                        ret.Add(rsd as DynamicSpatialNode);
                }
            }
            return ret;
        }

        public int GetFirstFreeTypeIDIncludingHiddenEntries()
        {
            int ret = -1;
            if (!Entries.Any())
                return 15;
            List<int> listOfTypeIDs = new List<int>();
            listOfTypeIDs.AddRange(Entries.Select(x => x.TypeID));
            foreach (var spatialStructs in SpatialStructs())
            {
                spatialStructs.VisitAllDecendantNamedDatas((DynamicNamedSpatialData parent, DynamicNamedSpatialData child) =>
                {
                    listOfTypeIDs.Add(child.TypeID);
                });
            }
            var ordered = listOfTypeIDs.OrderBy(x => x).ToList();
            for (int i = 0; i < ordered.Count - 1; ++i)
            {
                if (ordered[i] + 1 != ordered[i + 1])
                {
                    ret = ordered[i] + 1;
                    break;
                }
            }
            if (ret != -1 && ret < 0xFFFF)
                return ret;
            ret = ordered.Last() + 1;
            if (ret >= 0xFFFF)
                return -1;
            return ret;
        }

        public List<int> GetFreeTypeIDsIncludingHiddenEntries(int numofTypeIDs)
        {
            List<int> ret = new List<int>();
            List<int> listOfTypeIDs = new List<int>();
            listOfTypeIDs.AddRange(Entries.Select(x => x.TypeID));
            foreach (var spatialStructs in SpatialStructs())
            {
                spatialStructs.VisitAllDecendantNamedDatas((DynamicNamedSpatialData parent, DynamicNamedSpatialData child) =>
                {
                    listOfTypeIDs.Add(child.TypeID);
                });
            }
            var ordered = listOfTypeIDs.OrderBy(x => x).ToList();
            for (int i = 0; i != ordered.Count - 1; ++i)
            {
                if (ordered[i] + 1 != ordered[i + 1])
                {
                    if (ordered[i] + 1 != -1 && ordered[i] + 1 < 0xFFFF)
                    {
                        ret.Add(ordered[i] + 1);
                        ordered.Insert(i + 1, ordered[i] + 1);
                    }
                    if (ret.Count == numofTypeIDs)
                        break;
                }
            }
            int lastMax = ordered.Last();
            while (ret.Count < numofTypeIDs)
            {
                if (lastMax >= 0xFFFF)
                    return ret;
                lastMax++;
                ret.Add(lastMax);
            }
            return ret;
        }

        public int GetOrAddExternalRefIndexOfRPK(string SlrrRootRelativeRPKFnam)
        {
            string model = SlrrRootRelativeRPKFnam.ToLower().Replace('/', '\\');
            for (int i = 0; i != ExternalReferences.Count; ++i)
            {
                if (ExternalReferences[i].ToLower().Replace('/', '\\') == SlrrRootRelativeRPKFnam)
                    return i + 1;
            }
            ExternalReferences.Add(model);
            return ExternalReferences.Count;
        }

        public void FixBounds()
        {
            foreach (var res in Entries)
            {
                foreach (var rsd in res.RSD.InnerEntries)
                {
                    if (rsd is DynamicSpatialNode)
                    {
                        var arr = rsd as DynamicSpatialNode;
                        foreach (var named in arr.DataArray)
                        {
                            fixChildBounds(named);
                            System.Windows.Media.Media3D.Rect3D parentBound = new System.Windows.Media.Media3D.Rect3D();
                            parentBound.X = res.AdditionalFloatList[0];
                            parentBound.Y = res.AdditionalFloatList[1];
                            parentBound.Z = res.AdditionalFloatList[2];
                            parentBound.SizeX = res.AdditionalFloatList[3] * 2.0;
                            parentBound.SizeY = res.AdditionalFloatList[4] * 2.0;
                            parentBound.SizeZ = res.AdditionalFloatList[5] * 2.0;
                            parentBound.X -= res.AdditionalFloatList[3];
                            parentBound.Y -= res.AdditionalFloatList[4];
                            parentBound.Z -= res.AdditionalFloatList[5];
                            System.Windows.Media.Media3D.Rect3D childBound = new System.Windows.Media.Media3D.Rect3D();
                            childBound.X = named.BoundingBoxX;
                            childBound.Y = named.BoundingBoxY;
                            childBound.Z = named.BoundingBoxZ;
                            childBound.SizeX = named.BoundingBoxHalfWidthX * 2.0;
                            childBound.SizeY = named.BoundingBoxHalfWidthY * 2.0;
                            childBound.SizeZ = named.BoundingBoxHalfWidthZ * 2.0;
                            childBound.X -= named.BoundingBoxHalfWidthX;
                            childBound.Y -= named.BoundingBoxHalfWidthY;
                            childBound.Z -= named.BoundingBoxHalfWidthZ;
                            if (!parentBound.Contains(childBound))
                            {
                                parentBound.Union(childBound);
                                res.AdditionalFloatList[0] = (float)parentBound.X;
                                res.AdditionalFloatList[1] = (float)parentBound.Y;
                                res.AdditionalFloatList[2] = (float)parentBound.Z;
                                res.AdditionalFloatList[3] = (float)parentBound.SizeX / 2.0f;
                                res.AdditionalFloatList[4] = (float)parentBound.SizeY / 2.0f;
                                res.AdditionalFloatList[5] = (float)parentBound.SizeZ / 2.0f;
                                res.AdditionalFloatList[0] += res.AdditionalFloatList[3];
                                res.AdditionalFloatList[1] += res.AdditionalFloatList[4];
                                res.AdditionalFloatList[2] += res.AdditionalFloatList[5];
                            }
                        }
                    }
                }
            }
        }

        private int entriesSize()
        {
            int ret = 0;
            foreach (var entry in Entries)
            {
                ret += entry.Size;
            }
            return ret;
        }

        private void fixChildBounds(DynamicNamedSpatialData parent)
        {
            foreach (var own in parent.OwnedEntries)
            {
                if (own is DynamicSpatialNode)
                {
                    var arr = own as DynamicSpatialNode;
                    foreach (var child in arr.DataArray)
                    {
                        fixChildBounds(child);
                        if (!parent.IsOtherContainedWithinBounds(child))
                        {
                            parent.IncludeOtherWithinBounds(child);
                        }
                    }
                }
            }
        }
    }
}