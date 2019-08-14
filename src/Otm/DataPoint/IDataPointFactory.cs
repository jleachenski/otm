using System;
using System.Collections.Generic;
using NLog;
using Otm.Config;
using Otm.Logger;

namespace Otm.DataPoint
{
    public interface IDataPointFactory
    {
        IDictionary<string, IDataPoint> CreateDataPoints(IEnumerable<DataPointConfig> dataPointsConfig, ILoggerFactory loggerFactory);
    }
}