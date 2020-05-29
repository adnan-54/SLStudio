using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace SlrrLib.Model
{
  public class DynamicScxV3
  {
    public static readonly string FileSignature = "INVO";
    public static readonly int Version = 3;

    public bool EndingZeroIntPresent
    {
      get;
      private set;
    }
    public List<DynamicMaterialV3> Materials
    {
      get;
      set;
    } = new List<DynamicMaterialV3>();
    public List<DynamicMeshV3> Meshes
    {
      get;
      set;
    } = new List<DynamicMeshV3>();

    public DynamicScxV3(BinaryScxV3 constructFrom = null)
    {
      if (constructFrom == null)
        return;
      Dictionary<BinaryMaterialV3, DynamicMaterialV3> matToMat = new Dictionary<BinaryMaterialV3, DynamicMaterialV3>();
      foreach(var mat in constructFrom.Materials)
      {
        DynamicMaterialV3 toad = new DynamicMaterialV3(mat);
        Materials.Add(toad);
        matToMat[mat] = toad;
      }
      foreach(var model in constructFrom.Models)
      {
        Meshes.Add(new DynamicMeshV3(matToMat[model.GetCorrespondingMaterial()],model));
      }
      EndingZeroIntPresent = constructFrom.EndingZeroIntPresent;
    }

    public bool HasAnyReflection()
    {
      foreach (var mat in Materials)
      {
        if (mat.HasReflection())
          return true;
      }
      return false;
    }
    public void SetAllreflectionPercent(float percent)
    {
      foreach (var mat in Materials)
      {
        mat.SetReflectionStrength(percent);
      }
    }
    public void SaveAs(string fnam, bool backUp = true, string bakSuffix = "_BAK_ScxConst_")
    {
      if (backUp && File.Exists(fnam))
      {
        int bakInd = 0;
        while (File.Exists(fnam + bakSuffix + bakInd.ToString()))
          bakInd++;
        File.Copy(fnam, fnam + bakSuffix + bakInd.ToString());
      }
      BinaryWriter bw = new BinaryWriter(File.Create(fnam));
      bw.Write(ASCIIEncoding.ASCII.GetBytes(FileSignature));
      bw.Write(Version);
      foreach(var model in Meshes)
      {
        model.VerticesSizeBound();
        model.MakeSameDefinedFixedDefVertices();
        int oneVertexSize = model.GetOneVertexSize();
        if (!((!model.CorrespondingMaterial.IsOneVertexSizeDefined && oneVertexSize == 44) || (!model.CorrespondingMaterial.IsOneVertexSizeDefined && model.VertexDatas.Count == 0)))
          model.CorrespondingMaterial.OneVertexSize = model.GetOneVertexSize();
        model.CorrespondingMaterial.FixIsDefines();
        model.CorrespondingMaterial.Save(bw);
        model.Save(bw);
      }
      if(EndingZeroIntPresent)
        bw.Write((int)0);
      bw.Close();
    }
  }
}
