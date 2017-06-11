using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Colorful.Model
{
    public interface IColorService
    {
        double ComputeColorIndex(string imageFileName);
    }
}
