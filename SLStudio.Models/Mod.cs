﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLStudio.Models
{
    public class Mod
    {
        public string modName;
        public string modDescription;

        public List<Rpk> modRpks;

        public Mod() { }
    }
}