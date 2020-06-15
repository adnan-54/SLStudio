using System;
using System.IO;

namespace SlrrLib.Model
{
    public enum DynamicMaterialEntryPrettyType
    {
        DiffuseColor,
        IlluminationColor,
        SpecularColor,
        Glossiness,
        ReflectionPercent,
        BumpPercent,
        DiffuseMap,
        DiffuseMixSecond,
        BumpMap,
        ReflectionMap,
        IlluminationMap,
        MaterialName,
        Unkown
    }

    public abstract class DynamicMaterialV4Entry
    {
        protected ushort type
        {
            get;
            set;
        }

        protected ushort typeSwapped
        {
            get
            {
                return (ushort)((type >> 8) | (type << 8));
            }
            set
            {
                type = (ushort)((value >> 8) | (value << 8));
            }
        }

        public ushort UnknownFlag
        {
            get;
            set;
        }

        public DynamicMaterialEntryPrettyType TypeEnumed
        {
            get
            {
                if (UnknownFlag == 0 && typeSwapped == 0)
                    return DynamicMaterialEntryPrettyType.DiffuseColor;
                if (UnknownFlag == 2 && typeSwapped == 0)
                    return DynamicMaterialEntryPrettyType.IlluminationColor;
                if (UnknownFlag == 1 && typeSwapped == 0)
                    return DynamicMaterialEntryPrettyType.SpecularColor;
                if (UnknownFlag == 0 && typeSwapped == 1)
                    return DynamicMaterialEntryPrettyType.Glossiness;
                if (UnknownFlag == 1 && typeSwapped == 1)
                    return DynamicMaterialEntryPrettyType.ReflectionPercent;
                if (UnknownFlag == 2 && typeSwapped == 1)
                    return DynamicMaterialEntryPrettyType.BumpPercent;
                if (UnknownFlag == 0 && typeSwapped == 6)
                    return DynamicMaterialEntryPrettyType.DiffuseMap;
                if (UnknownFlag == 1 && typeSwapped == 6)
                    return DynamicMaterialEntryPrettyType.DiffuseMixSecond;
                if (UnknownFlag == 2 && typeSwapped == 6)
                    return DynamicMaterialEntryPrettyType.BumpMap;
                if (UnknownFlag == 3 && typeSwapped == 6)
                    return DynamicMaterialEntryPrettyType.ReflectionMap;
                if (UnknownFlag == 4 && typeSwapped == 6)
                    return DynamicMaterialEntryPrettyType.IlluminationMap;
                if (UnknownFlag == 0 && typeSwapped == 8)
                    return DynamicMaterialEntryPrettyType.MaterialName;
                return DynamicMaterialEntryPrettyType.Unkown;
            }
            set
            {
                switch (value)
                {
                    case DynamicMaterialEntryPrettyType.DiffuseColor:
                        if (typeSwapped == 0)
                            UnknownFlag = 0;
                        break;

                    case DynamicMaterialEntryPrettyType.IlluminationColor:
                        if (typeSwapped == 0)
                            UnknownFlag = 2;
                        break;

                    case DynamicMaterialEntryPrettyType.SpecularColor:
                        if (typeSwapped == 0)
                            UnknownFlag = 1;
                        break;

                    case DynamicMaterialEntryPrettyType.Glossiness:
                        if (typeSwapped == 1)
                            UnknownFlag = 0;
                        break;

                    case DynamicMaterialEntryPrettyType.ReflectionPercent:
                        if (typeSwapped == 1)
                            UnknownFlag = 1;
                        break;

                    case DynamicMaterialEntryPrettyType.BumpPercent:
                        if (typeSwapped == 1)
                            UnknownFlag = 2;
                        break;

                    case DynamicMaterialEntryPrettyType.DiffuseMap:
                        if (typeSwapped == 6)
                            UnknownFlag = 0;
                        break;

                    case DynamicMaterialEntryPrettyType.DiffuseMixSecond:
                        if (typeSwapped == 6)
                            UnknownFlag = 1;
                        break;

                    case DynamicMaterialEntryPrettyType.BumpMap:
                        if (typeSwapped == 6)
                            UnknownFlag = 2;
                        break;

                    case DynamicMaterialEntryPrettyType.ReflectionMap:
                        if (typeSwapped == 6)
                            UnknownFlag = 3;
                        break;

                    case DynamicMaterialEntryPrettyType.IlluminationMap:
                        if (typeSwapped == 6)
                            UnknownFlag = 4;
                        break;

                    case DynamicMaterialEntryPrettyType.MaterialName:
                        if (typeSwapped == 8)
                            UnknownFlag = 0;
                        break;

                    case DynamicMaterialEntryPrettyType.Unkown:
                    default:
                        break;
                }
            }
        }

        public virtual int Size
        {
            get
            {
                return BinaryMaterialV4Entry.TypeToSize[type] + 4;//4 header;
            }
        }

        public string PrettyName
        {
            get
            {
                if (UnknownFlag == 0 && typeSwapped == 0)
                    return "Diffuse Color";
                if (UnknownFlag == 2 && typeSwapped == 0)
                    return "Illumination Color";
                if (UnknownFlag == 1 && typeSwapped == 0)
                    return "Specular Color";
                if (UnknownFlag == 0 && typeSwapped == 1)
                    return "Glossiness";
                if (UnknownFlag == 1 && typeSwapped == 1)
                    return "Reflection Percent";
                if (UnknownFlag == 2 && typeSwapped == 1)
                    return "Bump Percent";
                if (UnknownFlag == 0 && typeSwapped == 6)
                    return "Diffuse Map";
                if (UnknownFlag == 1 && typeSwapped == 6)
                    return "Diffuse Mix Second";
                if (UnknownFlag == 2 && typeSwapped == 6)
                    return "Bump Map";
                if (UnknownFlag == 3 && typeSwapped == 6)
                    return "Reflection Map";
                if (UnknownFlag == 4 && typeSwapped == 6)
                    return "Illumination Map";
                if (UnknownFlag == 0 && typeSwapped == 8)
                    return "Material Name";
                return "Unkown";
            }
        }

        protected DynamicMaterialV4Entry(BinaryMaterialV4Entry constructFrom = null)
        {
            if (constructFrom == null)
                return;
        }

        public abstract void Save(BinaryWriter bw);

        public override string ToString()
        {
            return Enum.GetName(typeof(DynamicMaterialEntryPrettyType), TypeEnumed);
        }
    }
}