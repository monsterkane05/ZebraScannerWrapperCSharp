using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZebraScannerWrapper.Enums
{
    public enum WeightStatus
    {
        SCALE_NOT_ENABLED = 0,
        SCALE_NOT_READY = 1,
        STABLE_WEIGHT_OVER_LIMIT = 2,
        STABLE_WEIGHT_UNDER_ZERO = 3,
        NON_STABLE_WEIGHT = 4,
        STABLE_ZERO_WEIGHT = 5,
        STABLE_NON_ZERO_WEIGHT = 6
    }
}
