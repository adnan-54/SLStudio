using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SlrrLib.Model
{
    public class BinaryScxV3 : Scx
    {
        public bool EndingZeroIntPresent
        {
            get;
            private set;
        } = false;

        public List<BinaryMaterialV3> Materials
        {
            get;
            private set;
        } = new List<BinaryMaterialV3>();

        public List<BinaryMeshV3> Models
        {
            get;
            private set;
        } = new List<BinaryMeshV3>();

        public BinaryScxV3(string fnam, bool readVertexData = false)
        : base(fnam)
        {
            ReadData(readVertexData);
        }

        public override void ReadData(bool readVertexData = false)
        {
            ProperlyReadData = false;
            var bytes = fileCache.GetFileData();
            int currentInd = 0;
            FileSignature = ASCIIEncoding.ASCII.GetString(bytes, currentInd, 4);
            currentInd += 4;
            readVersion = BitConverter.ToInt32(bytes, currentInd);
            currentInd += 4;
            while (currentInd < bytes.Length)
            {
                BinaryMaterialV3 toadMat = new BinaryMaterialV3(fileCache, currentInd);
                if (toadMat.Size == 0)
                {
                    if (currentInd + 4 == bytes.Length)
                    {
                        EndingZeroIntPresent = true;
                        ProperlyReadData = true;
                    }
                    break;
                }
                Materials.Add(toadMat);
                currentInd += toadMat.Size;

                BinaryMeshV3 toadModel = new BinaryMeshV3(toadMat, fileCache, currentInd, readVertexData);
                currentInd += toadModel.Size;

                Models.Add(toadModel);
            }
            if (currentInd == bytes.Length)
                ProperlyReadData = true;
        }

        public override bool HasAnyReflection()
        {
            foreach (var mat in Materials)
            {
                if (mat.HasReflection())
                    return true;
            }
            return false;
        }

        public override void SetAllreflectionPercent(float percent)
        {
            foreach (var mat in Materials)
            {
                mat.SetReflectionStrength(percent);
            }
        }

        public override string ToString()
        {
            return Path.GetFileNameWithoutExtension(fileCache.FileName);
        }
    }
}