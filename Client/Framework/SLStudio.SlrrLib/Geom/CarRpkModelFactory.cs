using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Media3D;

namespace SlrrLib.Geom
{
    public class CarRpkModelFactory : IModelFactory
    {
        private bool onlyLoadSelectedVehicle = false;
        private bool doNotLoadStranglerParts = false;
        private bool onlyLoadStockParts = false;
        private bool putRimsOnWheelSlot = false;
        private bool loadForScriptChange = false;
        private IEnumerable<string> modelGroupsCache = null;

        public int LastSelectedIndex
        {
            get;
            private set;
        } = -1;

        public string LastSelectedKey
        {
            get;
            private set;
        } = "";

        public SlrrLib.Model.HighLevel.RpkManager LastRPKMng
        {
            get;
            private set;
        } = null;

        public SlrrLib.Model.Cfg SelectedVehicleCfg
        {
            get;
            private set;
        } = null;

        public CarRpkModelFactory(bool OnlyLoadSelectedCfg = false, bool PutRimsOnWheelSlot = false,
                                  bool DoNotLoadStranglerParts = false, bool OnlyLoadStockParts = false, bool ForScriptLoad = false)
        {
            onlyLoadSelectedVehicle = OnlyLoadSelectedCfg;
            putRimsOnWheelSlot = PutRimsOnWheelSlot;
            doNotLoadStranglerParts = DoNotLoadStranglerParts;
            onlyLoadStockParts = OnlyLoadStockParts;
            loadForScriptChange = ForScriptLoad;
        }

        public CarRpkModelFactory(string rpkToLoad, bool OnlyLoadSelectedCfg = false, bool PutRimsOnWheelSlot = false,
                                  bool DoNotLoadStranglerParts = false, bool OnlyLoadStockParts = false, bool ForScriptLoad = false)
        {
            modelGroupsCache = new List<string> { rpkToLoad };
            onlyLoadSelectedVehicle = OnlyLoadSelectedCfg;
            putRimsOnWheelSlot = PutRimsOnWheelSlot;
            doNotLoadStranglerParts = DoNotLoadStranglerParts;
            onlyLoadStockParts = OnlyLoadStockParts;
            loadForScriptChange = ForScriptLoad;
        }

        public IEnumerable<NamedModel> GenModels(string modelGroup)
        {
            List<NamedCfgInducedModel> ret = new List<NamedCfgInducedModel>();
            string slrrRoot = SlrrLib.Model.HighLevel.GameFileManager.GetSLRRRoot(modelGroup);
            SlrrLib.Model.HighLevel.RpkManager rpkMng = new SlrrLib.Model.HighLevel.RpkManager(modelGroup, slrrRoot);
            SlrrLib.Model.HighLevel.GameFileManager minGManag = new SlrrLib.Model.HighLevel.GameFileManager(slrrRoot);
            minGManag.ManualAddRPK(rpkMng, modelGroup);
            LastRPKMng = rpkMng;
            int curInd = 0;
            rpkMng.BuildSlotCache();
            HashSet<int> typeIDsOfSlotOwnersProcessed = new HashSet<int>();
            Queue<SlrrLib.Model.HighLevel.CfgSlotCacheEntry> slots = new Queue<SlrrLib.Model.HighLevel.CfgSlotCacheEntry>();
            List<int> IDCheckOrder = rpkMng.CarDefTypeIDs().ToList();
            List<int> stockPartsTypeIDS = new List<int>();
            if (onlyLoadSelectedVehicle)
            {
                List<string> IDNames = IDCheckOrder.Select(x => Path.GetFileNameWithoutExtension(rpkMng.GetCfgFromTypeID(x).CfgFileName)).ToList();
                Dictionary<int, string> dict = new Dictionary<int, string>();
                for (int dict_i = 0; dict_i != IDCheckOrder.Count; ++dict_i)
                {
                    dict.Add(IDCheckOrder[dict_i], IDNames[dict_i]);
                }
                View.IndexSelector selector = new View.IndexSelector();
                selector.SetSelectionList(dict);
                selector.ShowDialog();
                foreach (var id in IDCheckOrder)
                    typeIDsOfSlotOwnersProcessed.Add(id);
                IDCheckOrder.Clear();
                IDCheckOrder.Add(selector.SelectedInt);
                LastSelectedIndex = selector.SelectedInt;
                LastSelectedKey = selector.SelectedString;
                typeIDsOfSlotOwnersProcessed.Remove(selector.SelectedInt);
                SelectedVehicleCfg = rpkMng.GetCfgFromTypeID(selector.SelectedInt);
                if (onlyLoadStockParts)
                {
                    stockPartsTypeIDS = new List<int>();
                    foreach (var ln in SelectedVehicleCfg.LinesWithName("stockpart"))
                    {
                        stockPartsTypeIDS.Add(ln.Tokens[1].ValueAsHexInt);
                    }
                    stockPartsTypeIDS.Add(selector.SelectedInt);
                }
            }
            if (!doNotLoadStranglerParts)
                IDCheckOrder.AddRange(rpkMng.PartDefTypeIDs());

            foreach (var carID in IDCheckOrder)
            {
                if (typeIDsOfSlotOwnersProcessed.Contains(carID))
                    continue;
                var res = rpkMng.GetResEntry(carID);
                if (rpkMng.typeIDTOSlotToSlotCacheEntry.ContainsKey(carID))
                {
                    typeIDsOfSlotOwnersProcessed.Add(carID);
                    foreach (var startSlot in rpkMng.typeIDTOSlotToSlotCacheEntry[carID])
                    {
                        slots.Enqueue(startSlot.Value);
                    }
                    while (slots.Any())
                    {
                        var curSlot = slots.Dequeue();
                        if (onlyLoadStockParts && !stockPartsTypeIDS.Contains(curSlot.TypeIDOfSlotOwner))
                            continue;
                        var cfgOwner = rpkMng.GetCfgFromTypeID(curSlot.TypeIDOfSlotOwner);
                        var modelOwner = ret.FirstOrDefault(x => x.Cfg == cfgOwner);
                        if (modelOwner == null)
                        {
                            var nulFnam = rpkMng.GetHeuristicScxFnamFromLocalPartTypeID(curSlot.TypeIDOfSlotOwner);
                            if (nulFnam != null && rpkMng.IsFullFnamProbableForLocalPath(slrrRoot + "\\" + nulFnam))
                            {
                                nulFnam = slrrRoot + "\\" + nulFnam;
                                if (!ret.Any(x => x.ScxFnam.ToLower() == nulFnam.ToLower()) && File.Exists(nulFnam))
                                {
                                    SCXModelFactor scxFact = new SCXModelFactor();
                                    var allNulModels = scxFact.GetModels(nulFnam, new Vector3D(0, 0, 0), new Vector3D(0, 0, 0)).OfType<NamedScxModel>().Select(x => new NamedCfgInducedModel(x));
                                    var paintableInd = rpkMng.GetPartScxPaintableTextureIndex(curSlot.TypeIDOfSlotOwner);
                                    foreach (var model in allNulModels)
                                    {
                                        model.Cfg = cfgOwner;
                                        model.Translate = new Vector3D(0, 0, 0);
                                        model.Ypr = new Vector3D(0, 0, 0);
                                        model.SourceTypeID = curSlot.TypeIDOfSlotOwner;
                                        if (onlyLoadSelectedVehicle)
                                            model.Name += " 0x" + curSlot.TypeIDOfSlotOwner.ToString("X8") + " ";
                                        if (loadForScriptChange)
                                        {
                                            try
                                            {
                                                model.Name = Path.GetFileNameWithoutExtension(rpkMng.GetClassPairFromTypeID(model.SourceTypeID).GetClassFileName()) + " | " + model.Name;
                                            }
                                            catch (Exception) { }
                                        }
                                        if (model.Scxv4Source != null)
                                        {
                                            foreach (var mat in model.Scxv4Source.Meshes[model.MeshIndex].Material.Entries)
                                            {
                                                if (mat is Model.DynamicMaterialV4MapDefinition mapDef)
                                                {
                                                    if (mapDef.TextureIndex == paintableInd)
                                                    {
                                                        if (mapDef.MapChannel >= 0 && mapDef.MapChannel <= 2)
                                                        {
                                                            model.PaintableTextureUVIndex = mapDef.MapChannel;
                                                            model.HasPaintableTexture = true;
                                                            model.Name += " UV: " + model.PaintableTextureUVIndex.ToString();
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (model.Scxv3Source != null)
                                        {
                                            if (model.Scxv3Source.Materials[model.MeshIndex].DiffuseMapIndex == paintableInd)
                                            {
                                                model.PaintableTextureUVIndex = model.Scxv3Source.Materials[model.MeshIndex].DiffuseMixFirstMapChannel;
                                                model.HasPaintableTexture = true;
                                                model.Name += " UV: " + model.PaintableTextureUVIndex.ToString();
                                            }
                                        }
                                    }
                                    ret.AddRange(allNulModels);
                                }
                            }
                        }
                        var localCompatible = minGManag.GetLocalAttachableReferencesForSlot(rpkMng, curSlot.TypeIDOfSlotOwner, curSlot.SlotID);
                        foreach (var potPart in localCompatible)
                        {
                            if (onlyLoadStockParts && !stockPartsTypeIDS.Contains(potPart.SlotRef.TypeID))
                                continue;
                            if (!typeIDsOfSlotOwnersProcessed.Contains(potPart.SlotRef.TypeID))
                            {
                                typeIDsOfSlotOwnersProcessed.Add(potPart.SlotRef.TypeID);
                                if (rpkMng.typeIDTOSlotToSlotCacheEntry.ContainsKey(potPart.SlotRef.TypeID))
                                {
                                    foreach (var nextSlot in rpkMng.typeIDTOSlotToSlotCacheEntry[potPart.SlotRef.TypeID])
                                    {
                                        slots.Enqueue(nextSlot.Value);
                                    }
                                }
                            }
                            else //if (OnlyLoadSelectedVehicle)
                                continue;
                            try
                            {
                                var scxFnam = rpkMng.GetHeuristicScxFnamFromLocalPartTypeID(potPart.SlotRef.TypeID);
                                var cfg = rpkMng.GetCfgFromTypeID(potPart.SlotRef.TypeID);
                                if (cfg == null || scxFnam == null)
                                {
                                    continue;
                                }
                                scxFnam = slrrRoot + "\\" + scxFnam;
                                if (!ret.Any(x => x.ScxFnam.ToLower() == scxFnam.ToLower()) && rpkMng.IsFullFnamProbableForLocalPath(scxFnam))
                                {
                                    var toad = new NamedCfgInducedModel();
                                    toad.Cfg = cfg;
                                    toad.SourceTypeID = potPart.SlotRef.TypeID;
                                    if (onlyLoadSelectedVehicle)
                                        toad.Name += " 0x" + potPart.SlotRef.TypeID.ToString("X8") + " ";
                                    toad.Name = Path.GetFileNameWithoutExtension(cfg.CfgFileName);
                                    if (loadForScriptChange)
                                    {
                                        try
                                        {
                                            toad.Name = Path.GetFileNameWithoutExtension(rpkMng.GetClassPairFromTypeID(toad.SourceTypeID).GetClassFileName()) + " | " + toad.Name;
                                        }
                                        catch (Exception) { }
                                    }
                                    toad.SourceOfModel = NamedModelSource.Mesh;
                                    var sourceSlot = rpkMng.GetSlotFromTypeIDAndSlotID(curSlot.TypeIDOfSlotOwner, curSlot.SlotID);
                                    var targetSlot = rpkMng.GetSlotFromTypeIDAndSlotID(potPart.SlotRef.TypeID, potPart.SlotRef.SlotID);
                                    toad.Translate = (modelOwner.Translate / 100.0) + new Vector3D(sourceSlot.LineX, sourceSlot.LineY, sourceSlot.LineZ) -
                                                     new Vector3D(targetSlot.LineX, targetSlot.LineY, targetSlot.LineZ);
                                    toad.Translate *= 100.0;
                                    toad.Ypr = modelOwner.Ypr + new Vector3D(sourceSlot.LineRotX, sourceSlot.LineRotY, sourceSlot.LineRotZ) +
                                               new Vector3D(targetSlot.LineRotX, targetSlot.LineRotY, targetSlot.LineRotZ);
                                    SCXModelFactor scxFact = new SCXModelFactor();
                                    var allModels = scxFact.GetModels(scxFnam, toad.Translate, toad.Ypr).OfType<NamedScxModel>().Select(x => new NamedCfgInducedModel(x));
                                    var paintableInd = rpkMng.GetPartScxPaintableTextureIndex(potPart.SlotRef.TypeID);
                                    foreach (var model in allModels)
                                    {
                                        model.Cfg = cfg;
                                        model.Translate = toad.Translate;
                                        model.Ypr = toad.Ypr;
                                        model.SourceTypeID = potPart.SlotRef.TypeID;
                                        if (onlyLoadSelectedVehicle)
                                            model.Name += " 0x" + potPart.SlotRef.TypeID.ToString("X8") + " ";
                                        if (loadForScriptChange)
                                        {
                                            try
                                            {
                                                model.Name = Path.GetFileNameWithoutExtension(rpkMng.GetClassPairFromTypeID(model.SourceTypeID).GetClassFileName()) + " | " + model.Name;
                                            }
                                            catch (Exception) { }
                                        }
                                        if (model.Scxv4Source != null)
                                        {
                                            foreach (var mat in model.Scxv4Source.Meshes[model.MeshIndex].Material.Entries)
                                            {
                                                if (mat is SlrrLib.Model.DynamicMaterialV4MapDefinition mapDef)
                                                {
                                                    if (mapDef.TextureIndex == paintableInd)
                                                    {
                                                        if (mapDef.MapChannel >= 0 && mapDef.MapChannel <= 2)
                                                        {
                                                            model.PaintableTextureUVIndex = mapDef.MapChannel;
                                                            model.HasPaintableTexture = true;
                                                            model.Name += " UV: " + model.PaintableTextureUVIndex.ToString();
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (model.Scxv3Source != null)
                                        {
                                            if (model.Scxv3Source.Materials[model.MeshIndex].DiffuseMapIndex == paintableInd)
                                            {
                                                model.PaintableTextureUVIndex = model.Scxv3Source.Materials[model.MeshIndex].DiffuseMixFirstMapChannel;
                                                model.HasPaintableTexture = true;
                                                model.Name += " UV: " + model.PaintableTextureUVIndex.ToString();
                                            }
                                        }
                                    }
                                    ret.AddRange(allModels);
                                }
                            }
                            catch (Exception)
                            {
                                SlrrLib.Model.MessageLog.AddError("Error loading PartWith ID: 0x" + potPart.SlotRef.TypeID.ToString("X8"));
                            }
                        }
                    }
                    if (putRimsOnWheelSlot)
                    {
                        var mainCfg = rpkMng.GetCfgFromTypeID(carID);
                        foreach (var line in mainCfg.LinesWithName("wheel"))
                        {
                            SlrrLib.Model.CfgPartPositionLine ln = new SlrrLib.Model.CfgPartPositionLine(line);
                            var toad = new NamedModel();
                            toad.Name = "Wheel";
                            toad.SourceOfModel = NamedModelSource.Mesh;
                            toad.Translate = new Vector3D(ln.LineX, ln.LineY + 0.15f, ln.LineZ);
                            toad.Translate *= 100.0;
                            SCXModelFactor scxFact = new SCXModelFactor();
                            var allModels = scxFact.GetModels("tyre.scx", toad.Translate, toad.Ypr).OfType<NamedScxModel>();
                            foreach (var model in allModels)
                            {
                                model.Translate = toad.Translate;
                                model.Name = "Wheel";
                            }
                            ret.AddRange(allModels.Select(x => new NamedCfgInducedModel(x)));
                        }
                    }
                }
                else
                {
                    SlrrLib.Model.MessageLog.AddError("TypeID: 0x" + carID.ToString("X8") + " does not have slot cache entry.");
                }
                curInd++;
            }
            if (onlyLoadStockParts)
            {
                return ret.Where(x => stockPartsTypeIDS.Any(y => y == x.SourceTypeID)).OrderBy(x => x.SourceTypeID);
            }
            if (loadForScriptChange)
                return ret.Where(x => rpkMng.GetClassPairFromTypeID(x.SourceTypeID) != null &&
                                 rpkMng.GetClassPairFromTypeID(x.SourceTypeID).GetClassFileName()
                                 .Contains("parts\\scripts\\bodypart\\"))
                       .OrderBy(x => x.Name);
            return ret.OrderBy(x => x.HasPaintableTexture);
        }

        public IEnumerable<string> GetModelGroups()
        {
            return modelGroupsCache;
        }
    }
}