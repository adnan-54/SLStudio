using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SlrrLib.Model
{
    public class RpkExchange
    {
        private static readonly string floatFormat = "F10";
        private static readonly bool writeAllLines = false;

        private StringBuilder sb;
        private int tabDepth = 0;
        private DynamicRpk dat;
        private int li = 0;
        private string[] lns = new string[0];

        public void SaveRpkToRDB2(string rpkFnam)
        {
            if (!File.Exists(rpkFnam))
            {
                MessageLog.AddError("File not found: " + rpkFnam);
                return;
            }
            //try
            {
                DynamicRpk rpkDat = new DynamicRpk(new BinaryRpk(rpkFnam, true));
                sb = new StringBuilder();
                saveRpk(rpkDat);
                File.WriteAllText(rpkFnam + ".RDB2", sb.ToString());
            }
            //catch (Exception e)
            {
                //MessageLog.AddError("FATAL ERROR:\n" + e.Message + "\nAt:\n" + e.StackTrace);
            }
        }

        public string GetLastReadLineAndIndex()
        {
            if (li >= lns.Length)
                return "Line: " + li.ToString() + " there is not enough lines in file";
            return "Line: " + li.ToString() + " :\n" + currentLine();
        }

        public DynamicRpk LoadRPKFromRDB2(string rdbFnam)
        {
            MessageLog.AddMessage("Reading...");
            lns = File.ReadAllLines(rdbFnam);
            dat = new DynamicRpk();
            DynamicResEntry lastRes = new DynamicResEntry();
            while (anyLinesLeft())
            {
                string curLine = stepLine();
                if (curLine.Trim() == "<RPK>")
                {
                }
                else if (curLine.Trim() == "</RPK>")
                {
                    break;
                }
                else if (curLine.Trim() == "<EXTERNAL_REFS>")
                {
                    while (stepLine().Trim() != "</EXTERNAL_REFS>")
                    {
                        dat.ExternalReferences.Add(currentLine().Trim());
                    }
                }
                else if (curLine.Trim() == "<RES>")
                {
                    lastRes = new DynamicResEntry();
                    while (stepLine().Trim() != "</RES>")
                    {
                        if (currentLineKey() == "typeid")
                            lastRes.TypeID = currentLineValueHex();
                        else if (currentLineKey() == "superid")
                            lastRes.SuperID = currentLineValueHex();
                        else if (currentLineKey() == "additionaltype")
                            lastRes.AdditionalType = (byte)currentLineValueInt();
                        else if (currentLineKey() == "alias")
                            lastRes.Alias = currentLineValue();
                        else if (currentLineKey() == "isparentcompatible")
                            lastRes.IsParentCompatible = currentLineValueInt();
                        else if (currentLineKey() == "typeofentry")
                            lastRes.TypeOfEntry = (byte)currentLineValueInt();
                        else if (currentLineKey() == "<FLOAT_BOUNDS>")
                        {
                            lastRes.AdditionalFloatList = new List<float>();
                            while (stepLine().Trim() != "</FLOAT_BOUNDS>")
                            {
                                lastRes.AdditionalFloatList.Add(currentLineFloat());
                            }
                        }
                        else if (currentLineKey().Trim() == "<RSD>")
                        {
                            lastRes.RSD = loadRsd();
                        }
                    }
                    dat.Entries.Add(lastRes);
                }
            }
            return dat;
        }

        private string stepLine()
        {
            li++;
            if (lns.Length <= li)
                return "";
            if (writeAllLines)
                MessageLog.AddMessage(currentLine());
            return lns[li].TrimStart('\t');
        }

        private bool anyLinesLeft()
        {
            return (li < lns.Length);
        }

        private string currentLine()
        {
            if (anyLinesLeft())
                return lns[li].TrimStart('\t');
            return "";
        }

        private float currentLineFloat()
        {
            if (anyLinesLeft())
                return float.Parse(lns[li].Trim('\t', '\0', ' '));
            return 0;
        }

        private int currentLineInt()
        {
            if (anyLinesLeft())
                return int.Parse(lns[li].Trim('\t', '\0', ' '));
            return 0;
        }

        private int currentLineHexInt()
        {
            if (anyLinesLeft())
                return int.Parse(lns[li].Trim('\t', '\0', ' ').Substring(2), System.Globalization.NumberStyles.HexNumber);
            return 0;
        }

        private string currentLineValue()
        {
            string curL = currentLine();
            if (!curL.Contains('='))
                return curL;
            return (curL.Substring(curL.IndexOf('=') + 1).Trim('\t', ' ', '\r', '\n'));
        }

        private string currentLineKey()
        {
            string curL = currentLine();
            if (!curL.Contains('='))
                return curL;
            return (curL.Remove(curL.IndexOf('=')).Trim('\t', ' ', '\r', '\n')).ToLower();
        }

        private int currentLineValueHex()
        {
            return int.Parse(currentLineValue().Substring(2), System.Globalization.NumberStyles.HexNumber);
        }

        private int currentLineValueInt()
        {
            return int.Parse(currentLineValue());
        }

        private float currentLineValueFloat()
        {
            return float.Parse(currentLineValue());
        }

        private void appendWithTabDepth(string line, int tabMod = 0)
        {
            if (tabMod < 0)
                tabDepth += tabMod;
            string tab = "";
            for (int i = 0; i != tabDepth; ++i)
                tab += "\t";
            sb.Append(tab);
            sb.Append(line);
            if (tabMod > 0)
                tabDepth += tabMod;
        }

        private void appendLineWithTabDepth(string str, int tabMod = 0)
        {
            var spl = str.Trim('\0').Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in spl)
            {
                appendWithTabDepth(line, tabMod);
                sb.AppendLine();
            }
        }

        private void saveRpk(DynamicRpk rpkDat)
        {
            appendLineWithTabDepth("<RPK>", 1);
            appendLineWithTabDepth("<EXTERNAL_REFS>", 1);
            foreach (var extRef in rpkDat.ExternalReferences)
                appendLineWithTabDepth(extRef);
            appendLineWithTabDepth("</EXTERNAL_REFS>", -1);
            foreach (var res in rpkDat.Entries)
            {
                MessageLog.AddMessage("0x" + res.TypeID.ToString("X8"));
                saveRes(res);
            }
            appendLineWithTabDepth("</RPK>", -1);
        }

        private void saveRes(DynamicResEntry res)
        {
            appendLineWithTabDepth("<RES>", 1);
            appendLineWithTabDepth("TypeID=0x" + res.TypeID.ToString("X8"));
            appendLineWithTabDepth("SuperID=0x" + res.SuperID.ToString("X8"));
            appendLineWithTabDepth("AdditionalType=" + res.AdditionalType.ToString());
            appendLineWithTabDepth("Alias=" + res.Alias.Trim('\0'));
            appendLineWithTabDepth("IsParentCompatible=" + ((int)res.IsParentCompatible).ToString());
            appendLineWithTabDepth("TypeOfEntry=" + res.TypeOfEntry.ToString());
            if (res.AdditionalFloatList != null && res.AdditionalFloatList.Any())
            {
                appendLineWithTabDepth("<FLOAT_BOUNDS>", 1);
                foreach (var b in res.AdditionalFloatList)
                    appendLineWithTabDepth(b.ToString("F8"));
                appendLineWithTabDepth("</FLOAT_BOUNDS>", -1);
            }
            saveRSD(res.RSD);
            appendLineWithTabDepth("</RES>", -1);
        }

        private void saveRSD(DynamicRsdEntry rsd)
        {
            appendLineWithTabDepth("<RSD>", 1);
            foreach (var rsdEntry in rsd.InnerEntries)
            {
                if (rsdEntry is DynamicEXTPInnerEntry)
                    saveEXTP(rsdEntry as DynamicEXTPInnerEntry);
                else if (rsdEntry is DynamicICFGInnerEntry)
                    saveICFG(rsdEntry as DynamicICFGInnerEntry);
                else if (rsdEntry is DynamicInnerPhysEntry)
                    savePHYS(rsdEntry as DynamicInnerPhysEntry);
                else if (rsdEntry is DynamicInnerPolyEntry)
                    savePOLY(rsdEntry as DynamicInnerPolyEntry);
                else if (rsdEntry is DynamicRSDInnerEntry)
                    saveRSDInner(rsdEntry as DynamicRSDInnerEntry);
                else if (rsdEntry is DynamicStringInnerEntry)
                    saveStr(rsdEntry as DynamicStringInnerEntry);
                else if (rsdEntry is DynamicSpatialNode)
                    saveNamedArray(rsdEntry as DynamicSpatialNode);
                else if (rsdEntry is DynamicXCFGInnerEntry)
                    saveXCFG(rsdEntry as DynamicXCFGInnerEntry);
                else
                {
                    throw new Exception("Unkown data in rsd array");
                }
            }
            appendLineWithTabDepth("</RSD>", -1);
        }

        private void saveEXTP(DynamicEXTPInnerEntry extp)
        {
            appendLineWithTabDepth("<EXTP>", 1);
            appendLineWithTabDepth("EXTPint=" + extp.EXTPint.ToString());
            appendLineWithTabDepth("</EXTP>", -1);
        }

        private void saveICFG(DynamicICFGInnerEntry icfg)
        {
            appendLineWithTabDepth("<ICFG>", 1);
            foreach (var line in icfg.DataList)
                appendLineWithTabDepth(line);
            appendLineWithTabDepth("</ICFG>", -1);
        }

        private void saveRSDInner(DynamicRSDInnerEntry rsdInner)
        {
            appendLineWithTabDepth("<RSDInner>", 1);
            appendLineWithTabDepth(rsdInner.StringData);
            appendLineWithTabDepth("</RSDInner>", -1);
        }

        private void saveXCFG(DynamicXCFGInnerEntry xcfg)
        {
            appendLineWithTabDepth("<XCFG>", 1);
            appendLineWithTabDepth("Key=" + xcfg.CfgRefrenceKey);
            appendLineWithTabDepth("Value=" + xcfg.CfgReferenceValue);
            appendLineWithTabDepth("</XCFG>", -1);
        }

        private void saveStr(DynamicStringInnerEntry str)
        {
            appendLineWithTabDepth("<STR>", 1);
            appendLineWithTabDepth(str.StringData);
            appendLineWithTabDepth("</STR>", -1);
        }

        private void savePHYS(DynamicInnerPhysEntry phys)
        {
            appendLineWithTabDepth("<PHYS>", 1);
            appendLineWithTabDepth("<INDEX>", 1);
            foreach (var frst in phys.Indices)
            {
                appendLineWithTabDepth(frst.NormalX.ToString(floatFormat) + ";" + frst.NormalY.ToString(floatFormat) + ";" + frst.NormalZ.ToString(floatFormat) + ";" +
                                       frst.TriIndex0.ToString() + ";" + frst.TriIndex1.ToString() + ";" + frst.TriIndex2.ToString() + ";" + frst.UnkownInt1.ToString());
            }
            appendLineWithTabDepth("</INDEX>", -1);
            appendLineWithTabDepth("<VERTEX>", 1);
            foreach (var scnd in phys.VertexData)
            {
                appendLineWithTabDepth(scnd.VertexX.ToString(floatFormat) + ";" + scnd.VertexY.ToString(floatFormat) + ";" + scnd.VertexZ.ToString(floatFormat));
            }
            appendLineWithTabDepth("</VERTEX>", -1);
            appendLineWithTabDepth("</PHYS>", -1);
        }

        private void savePOLY(DynamicInnerPolyEntry poly)
        {
            appendLineWithTabDepth("<POLY>", 1);
            appendLineWithTabDepth("SomeCount=" + poly.UnkownCount1.ToString());
            foreach (var mesh in poly.Meshes)
            {
                appendLineWithTabDepth("<MESH>", 1);
                appendLineWithTabDepth("TextureIndex=" + mesh.TextureIndex.ToString());
                appendLineWithTabDepth("Indices=" + mesh.Indices.Select(x => x.ToString()).Aggregate((x, y) => x + ";" + y));
                appendLineWithTabDepth("<VERTEX>", 1);
                foreach (var vertex in mesh.Vertices)
                {
                    appendLineWithTabDepth(vertex.UVChannel1X.ToString(floatFormat) + ";" + vertex.UVChannel1Y.ToString(floatFormat) + ";" +
                                           vertex.UVChannel2X.ToString(floatFormat) + ";" + vertex.UVChannel2Y.ToString(floatFormat) + ";" +
                                           vertex.UVChannel3X.ToString(floatFormat) + ";" + vertex.UVChannel3Y.ToString(floatFormat) + ";" +
                                           vertex.VertexCoordX.ToString(floatFormat) + ";" + vertex.VertexCoordY.ToString(floatFormat) + ";" + vertex.VertexCoordZ.ToString(floatFormat) + ";" +
                                           vertex.VertexNormalX.ToString(floatFormat) + ";" + vertex.VertexNormalY.ToString(floatFormat) + ";" + vertex.VertexNormalZ.ToString(floatFormat) + ";" +
                                           vertex.VertexColorA.ToString() + ";" + vertex.VertexColorR.ToString() + ";" + vertex.VertexColorG.ToString() + ";" + vertex.VertexColorB.ToString() + ";" +
                                           vertex.VertexIlluminationA.ToString() + ";" + vertex.VertexIlluminationR.ToString() + ";" + vertex.VertexIlluminationG.ToString() + ";" + vertex.VertexIlluminationB.ToString());
                }
                appendLineWithTabDepth("</VERTEX>", -1);
                appendLineWithTabDepth("</MESH>", -1);
            }
            appendLineWithTabDepth("</POLY>", -1);
        }

        private void saveNamedArray(DynamicSpatialNode arr)
        {
            appendLineWithTabDepth("<NAMEDDATA_ARRAY>", 1);
            foreach (var nam in arr.DataArray)
            {
                saveNamedData(nam);
            }
            appendLineWithTabDepth("</NAMEDDATA_ARRAY>", -1);
        }

        private void saveNamedData(DynamicNamedSpatialData namedData)
        {
            appendLineWithTabDepth("<NAMEDDATA>", 1);
            appendLineWithTabDepth("BBoxHalfWidthX=" + namedData.BoundingBoxHalfWidthX.ToString(floatFormat));
            appendLineWithTabDepth("BBoxHalfWidthY=" + namedData.BoundingBoxHalfWidthY.ToString(floatFormat));
            appendLineWithTabDepth("BBoxHalfWidthZ=" + namedData.BoundingBoxHalfWidthZ.ToString(floatFormat));
            appendLineWithTabDepth("BBoxX=" + namedData.BoundingBoxX.ToString(floatFormat));
            appendLineWithTabDepth("BBoxY=" + namedData.BoundingBoxY.ToString(floatFormat));
            appendLineWithTabDepth("BBoxZ=" + namedData.BoundingBoxZ.ToString(floatFormat));
            appendLineWithTabDepth("Name=" + namedData.Name.Trim('\0'));
            appendLineWithTabDepth("SuperID=0x" + namedData.SuperID.ToString("X8"));
            appendLineWithTabDepth("TypeID=0x" + namedData.TypeID.ToString("X8"));
            appendLineWithTabDepth("ShortType=0x" + namedData.UnkownShort1.ToString("X8"));
            appendLineWithTabDepth("IsParentCompatible=" + ((int)namedData.UnkownIsParentCompatible).ToString());
            appendLineWithTabDepth("UNK7=" + namedData.UnkownData7.ToString("F1"));
            appendLineWithTabDepth("UNK8=" + namedData.UnkownData8.ToString("F1"));
            appendLineWithTabDepth("UNK9=" + namedData.UnkownData9.ToString("F1"));
            appendLineWithTabDepth("UNK10=" + namedData.UnkownData10.ToString("F1"));
            appendLineWithTabDepth("UNK11=" + namedData.UnkownData11.ToString("F1"));
            appendLineWithTabDepth("UNK12=" + namedData.UnkownData12.ToString("F1"));
            foreach (var owned in namedData.OwnedEntries)
            {
                if (owned is DynamicSpatialNode)
                    saveNamedArray(owned as DynamicSpatialNode);
                else if (owned is DynamicInnerPhysEntry)
                    savePHYS(owned as DynamicInnerPhysEntry);
                else if (owned is DynamicInnerPolyEntry)
                    savePOLY(owned as DynamicInnerPolyEntry);
                else if (owned is DynamicRSDInnerEntry)
                    saveRSDInner(owned as DynamicRSDInnerEntry);
                else
                {
                    throw new Exception("Unkown data in rpk array");
                }
            }
            appendLineWithTabDepth("</NAMEDDATA>", -1);
        }

        private DynamicRsdEntry loadRsd()
        {
            DynamicRsdEntry ret = new DynamicRsdEntry();
            while (stepLine().Trim() != "</RSD>")
            {
                if (currentLine().Trim() == "<EXTP>")
                    ret.InnerEntries.Add(loadEXTP());
                if (currentLine().Trim() == "<ICFG>")
                    ret.InnerEntries.Add(loadICFG());
                if (currentLine().Trim() == "<RSDInner>")
                    ret.InnerEntries.Add(loadRSDInner());
                if (currentLine().Trim() == "<XCFG>")
                    ret.InnerEntries.Add(loadXCFG());
                if (currentLine().Trim() == "<STR>")
                    ret.InnerEntries.Add(loadSTR());
                if (currentLine().Trim() == "<PHYS>")
                    ret.InnerEntries.Add(loadPHYS());
                if (currentLine().Trim() == "<POLY>")
                    ret.InnerEntries.Add(loadPOLY());
                if (currentLine().Trim() == "<NAMEDDATA_ARRAY>")
                    ret.InnerEntries.Add(loadNAMEDARRAY());
                if (currentLine().Trim() == "<NAMEDDATA>")
                    ret.InnerEntries.Add(loadNAMEDDATA());
            }
            return ret;
        }

        private DynamicEXTPInnerEntry loadEXTP()
        {
            DynamicEXTPInnerEntry ret = new DynamicEXTPInnerEntry();
            while (stepLine().Trim() != "</EXTP>")
                ret.EXTPint = currentLineValueInt();
            return ret;
        }

        private DynamicICFGInnerEntry loadICFG()
        {
            DynamicICFGInnerEntry ret = new DynamicICFGInnerEntry();
            stepLine();
            if (currentLine().Trim() == "</ICFG>")
                return ret;
            ret.DataList.Add(currentLine());
            string cfgData = "";
            while (stepLine().Trim() != "</ICFG>")
                cfgData += currentLine() + "\r\n";
            ret.DataList.Add(cfgData);
            return ret;
        }

        private DynamicRSDInnerEntry loadRSDInner()
        {
            DynamicRSDInnerEntry ret = new DynamicRSDInnerEntry();
            ret.Signature = "RSD\0";
            while (stepLine().Trim() != "</RSDInner>")
                ret.StringData += currentLine() + "\r\n";
            return ret;
        }

        private DynamicXCFGInnerEntry loadXCFG()
        {
            DynamicXCFGInnerEntry ret = new DynamicXCFGInnerEntry();
            while (stepLine().Trim() != "</XCFG>")
            {
                if (currentLineKey() == "key")
                    ret.CfgRefrenceKey = currentLineValue();
                if (currentLineKey() == "value")
                    ret.CfgReferenceValue = currentLineValue();
            }
            return ret;
        }

        private DynamicStringInnerEntry loadSTR()
        {
            DynamicStringInnerEntry ret = new DynamicStringInnerEntry();
            while (stepLine().Trim() != "</STR>")
            {
                ret.StringData += currentLine() + "\r\n";
            }
            return ret;
        }

        private DynamicInnerPhysEntry loadPHYS()
        {
            DynamicInnerPhysEntry ret = new DynamicInnerPhysEntry();
            string[] spl;
            while (stepLine().Trim() != "</PHYS>")
            {
                if (currentLine().Trim() == "<INDEX>")
                {
                    while (stepLine().Trim() != "</INDEX>")
                    {
                        spl = currentLine().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        DynamicPhysFacingDescriptor toad = new DynamicPhysFacingDescriptor();
                        toad.NormalX = float.Parse(spl[0]);
                        toad.NormalY = float.Parse(spl[1]);
                        toad.NormalZ = float.Parse(spl[2]);
                        toad.TriIndex0 = int.Parse(spl[3]);
                        toad.TriIndex1 = int.Parse(spl[4]);
                        toad.TriIndex2 = int.Parse(spl[5]);
                        toad.UnkownInt1 = int.Parse(spl[6]);
                        ret.Indices.Add(toad);
                    }
                }
                if (currentLine().Trim() == "<VERTEX>")
                {
                    while (stepLine().Trim() != "</VERTEX>")
                    {
                        spl = currentLine().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        DynamicPhysVertex toad = new DynamicPhysVertex();
                        toad.VertexX = float.Parse(spl[0]);
                        toad.VertexY = float.Parse(spl[1]);
                        toad.VertexZ = float.Parse(spl[2]);
                        ret.VertexData.Add(toad);
                    }
                }
            }
            return ret;
        }

        private DynamicInnerPolyEntry loadPOLY()
        {
            DynamicInnerPolyEntry ret = new DynamicInnerPolyEntry();
            while (stepLine().Trim() != "</POLY>")
            {
                if (currentLineKey() == "somecount")
                    ret.UnkownCount1 = currentLineValueInt();
                if (currentLineKey().Trim() == "<MESH>")
                {
                    DynamicInnerPolyMesh toad = new DynamicInnerPolyMesh();
                    while (stepLine().Trim() != "</MESH>")
                    {
                        if (currentLineKey() == "textureindex")
                            toad.TextureIndex = currentLineValueInt();
                        if (currentLineKey() == "indices")
                        {
                            var spl = currentLineValue().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                            toad.Indices = spl.Select(x => int.Parse(x)).ToList();
                        }
                        if (currentLineKey().Trim() == "<VERTEX>")
                        {
                            while (stepLine().Trim() != "</VERTEX>")
                            {
                                DynamicPolyVertexData toadVert = new DynamicPolyVertexData();
                                var spl = currentLineValue().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                                toadVert.UVChannel1X = float.Parse(spl[0]);
                                toadVert.UVChannel1Y = float.Parse(spl[1]);
                                toadVert.UVChannel2X = float.Parse(spl[2]);
                                toadVert.UVChannel2Y = float.Parse(spl[3]);
                                toadVert.UVChannel3X = float.Parse(spl[4]);
                                toadVert.UVChannel3Y = float.Parse(spl[5]);
                                toadVert.VertexCoordX = float.Parse(spl[6]);
                                toadVert.VertexCoordY = float.Parse(spl[7]);
                                toadVert.VertexCoordZ = float.Parse(spl[8]);
                                toadVert.VertexNormalX = float.Parse(spl[9]);
                                toadVert.VertexNormalY = float.Parse(spl[10]);
                                toadVert.VertexNormalZ = float.Parse(spl[11]);
                                toadVert.VertexColorA = byte.Parse(spl[12]);
                                toadVert.VertexColorR = byte.Parse(spl[13]);
                                toadVert.VertexColorG = byte.Parse(spl[14]);
                                toadVert.VertexColorB = byte.Parse(spl[15]);
                                toadVert.VertexIlluminationA = byte.Parse(spl[16]);
                                toadVert.VertexIlluminationR = byte.Parse(spl[17]);
                                toadVert.VertexIlluminationG = byte.Parse(spl[18]);
                                toadVert.VertexIlluminationB = byte.Parse(spl[19]);
                                toad.Vertices.Add(toadVert);
                            }
                        }
                    }
                    ret.Meshes.Add(toad);
                }
            }
            return ret;
        }

        private DynamicSpatialNode loadNAMEDARRAY()
        {
            DynamicSpatialNode ret = new DynamicSpatialNode();
            ret.Signature = 0;
            while (stepLine().Trim() != "</NAMEDDATA_ARRAY>")
            {
                ret.DataArray.Add(loadNAMEDDATA());
            }
            return ret;
        }

        private DynamicNamedSpatialData loadNAMEDDATA()
        {
            DynamicNamedSpatialData ret = new DynamicNamedSpatialData();
            while (stepLine().Trim() != "</NAMEDDATA>")
            {
                if (currentLineKey() == "BBoxHalfWidthX".ToLower())
                    ret.BoundingBoxHalfWidthX = currentLineValueFloat();
                if (currentLineKey() == "BBoxHalfWidthY".ToLower())
                    ret.BoundingBoxHalfWidthY = currentLineValueFloat();
                if (currentLineKey() == "BBoxHalfWidthZ".ToLower())
                    ret.BoundingBoxHalfWidthZ = currentLineValueFloat();
                if (currentLineKey() == "BBoxX".ToLower())
                    ret.BoundingBoxX = currentLineValueFloat();
                if (currentLineKey() == "BBoxY".ToLower())
                    ret.BoundingBoxY = currentLineValueFloat();
                if (currentLineKey() == "BBoxZ".ToLower())
                    ret.BoundingBoxZ = currentLineValueFloat();
                if (currentLineKey() == "UNK7".ToLower())
                    ret.UnkownData7 = currentLineValueFloat();
                if (currentLineKey() == "UNK8".ToLower())
                    ret.UnkownData8 = currentLineValueFloat();
                if (currentLineKey() == "UNK9".ToLower())
                    ret.UnkownData9 = currentLineValueFloat();
                if (currentLineKey() == "UNK10".ToLower())
                    ret.UnkownData10 = currentLineValueFloat();
                if (currentLineKey() == "UNK11".ToLower())
                    ret.UnkownData11 = currentLineValueFloat();
                if (currentLineKey() == "UNK12".ToLower())
                    ret.UnkownData12 = currentLineValueFloat();
                if (currentLineKey() == "Name".ToLower())
                    ret.Name = currentLineValue();
                if (currentLineKey() == "SuperID".ToLower())
                    ret.SuperID = currentLineValueHex();
                if (currentLineKey() == "TypeID".ToLower())
                    ret.TypeID = currentLineValueHex();
                if (currentLineKey() == "ShortType".ToLower())
                    ret.UnkownShort1 = (short)currentLineValueHex();
                if (currentLineKey() == "IsParentCompatible".ToLower())
                    ret.UnkownIsParentCompatible = currentLineValueInt();
                if (currentLineKey() == "<RSDInner>")
                    ret.OwnedEntries.Add(loadRSDInner());
                if (currentLineKey() == "<PHYS>")
                    ret.OwnedEntries.Add(loadPHYS());
                if (currentLineKey() == "<POLY>")
                    ret.OwnedEntries.Add(loadPOLY());
                if (currentLineKey() == "<NAMEDDATA_ARRAY>")
                    ret.OwnedEntries.Add(loadNAMEDARRAY());
            }
            return ret;
        }
    }
}