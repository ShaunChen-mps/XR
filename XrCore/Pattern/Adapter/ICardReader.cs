﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XrCore.Pattern.Adapter
{
    public interface ICardReader
    {
        string ReadCard(string filePath);
    }
}
