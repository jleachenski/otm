﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otm.Shared.ContextConfig
{
    public class ContextInput
    {
        public string Name { get; set; }
        public string Mode { get; set; }
        public bool Enabled { get; set; }
        public string Logger { get; set; }
    }
}
