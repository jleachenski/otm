using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Otm.Server.ContextConfig;
using Otm.Server.DataPoint;
using Otm.Server.Device;
using Otm.Shared.ContextConfig;

namespace Otm.Server.Transaction
{
    public static class TransactionFactory
    {
        public static IDictionary<string, ITransaction> CreateTransactions(
            IEnumerable<TransactionConfig> transactionsConfig,
            IDictionary<string, IDataPoint> dataPoints,
            IDictionary<string, IDevice> devices,
            ILogger logger)
        {
            var transactions = new Dictionary<string, ITransaction>();

            if (transactionsConfig != null)
                foreach (var trConfig in transactionsConfig)
                {
                    // verify transaction name
                    if (string.IsNullOrWhiteSpace(trConfig.Name))
                    {
                        var ex = new Exception($"Invalid Transaction name in config. Name: {trConfig.Name}");
                        ex.Data.Add("field", "Name");
                        throw ex;
                    }

                    // verify the datapoint name
                    if (!dataPoints.ContainsKey(trConfig.DataPointName) && dataPoints[trConfig.DataPointName] == null)
                    {
                        var ex = new Exception($"Invalid DataPointName name in Transaction config. DataPointName ({trConfig.DataPointName}) Transaction ({trConfig.Name})");
                        ex.Data.Add("field", "DataPointName");
                        throw ex;
                    }

                    // verify the trigger OnTagChange
                    if (trConfig.TriggerType == TriggerTypes.OnTagChange && string.IsNullOrWhiteSpace(trConfig.TriggerSourceName))
                    {
                        var ex = new Exception($"Invalid TriggerSourceName, can't be empty. TriggerType ({trConfig.TriggerType }) Transaction ({trConfig.Name})");
                        ex.Data.Add("field", "TriggerSourceName");
                        throw ex;
                    }

                    // verify the trigger tag name
<<<<<<< HEAD
                    if (trConfig.TriggerType == TriggerTypes.OnTagChange && !devices[trConfig.SourceDeviceName].ContainsTag(trConfig.TriggerSourceName))
                    {
                        var ex = new Exception($"Invalid TriggerSourceName name in Transaction config. TriggerSourceName ({trConfig.TriggerSourceName}) Device ({trConfig.SourceDeviceName}) Transaction ({trConfig.Name})");
=======
                    if (trConfig.TriggerType == TriggerTypes.OnTagChange && !devices[trConfig.DeviceName].ContainsTag(trConfig.TriggerTagName))
                    {
                        var ex = new Exception($"Invalid TriggerTagName name in Transaction config. TriggerTagName ({trConfig.TriggerTagName}) Device ({trConfig.DeviceName}) Transaction ({trConfig.Name})");
>>>>>>> b9eb390e7415b6851e61969baecd0f404858b5ce
                        ex.Data.Add("field", "Name");
                        throw ex;
                    }

                    // verify the trigger OnCycle
                    if (trConfig.TriggerType == TriggerTypes.OnCycle && trConfig.TriggerTime == 0)
                    {
                        var ex = new Exception($"Invalid TriggerTime, can't be zero. TriggerType ({trConfig.TriggerType }) Transaction ({trConfig.Name})");
                        ex.Data.Add("field", "TriggerTime");
                        throw ex;
                    }

                    // verify the trigger type
                    if    (trConfig.TriggerType != TriggerTypes.OnTagChange 
                        && trConfig.TriggerType != TriggerTypes.OnCycle 
                        && trConfig.TriggerType != TriggerTypes.OnMessageReceived)
                    {
                        var ex = new Exception($"Invalid TriggerType. TriggerType ({trConfig.TriggerType }) Transaction ({trConfig.Name})");
                        ex.Data.Add("field", "TriggerType");
                        throw ex;
                    }


                    // verify the device name
                    if (!devices.ContainsKey(trConfig.DeviceName) && devices[trConfig.DeviceName] == null)
                    {
                        var ex = new Exception($"Invalid DeviceName name in Transaction config. DeviceName ({trConfig.DeviceName}) Transaction ({trConfig.Name})");
                        ex.Data.Add("field", "DeviceName");
                        throw ex;
                    }

                    foreach (var bind in trConfig.Binds)
                    {
                        // verify each bind
                        var dpParam = dataPoints[trConfig.DataPointName].GetParamConfig(bind.DataPointParam);
                        var dvTag = devices[trConfig.DeviceName].GetTagConfig(bind.DeviceTag);

                        // verify the DataPointParam of bind
                        if (dpParam == null)
                        {
                            var ex = new Exception($"Invalid DataPointParam name in Transaction config. DataPointParam ({bind.DataPointParam}) Transaction ({trConfig.Name})");
                            ex.Data.Add("field", "Name");
                            throw ex;
                        }

                        // verify the DeviceTag of bind if not have static value
                        if (dvTag == null && string.IsNullOrWhiteSpace(bind.Value))
                        {
                            var ex = new Exception($"Invalid DeviceTag name in Transaction config. DeviceTag ({bind.DeviceTag}) Transaction ({trConfig.Name})");
                            ex.Data.Add("field", "Name");
                            throw ex;
                        }

                        if ((dvTag == null && string.IsNullOrWhiteSpace(bind.Value)) )
                        {
                            var ex = new Exception($"DeviceTag or Value must me set in Transaction config. DataPointParam ({bind.DataPointParam}) Transaction ({trConfig.Name})");
                            ex.Data.Add("field", "Name");
                            throw ex;
                        }

                        if ((dvTag != null && !string.IsNullOrWhiteSpace(bind.Value)))
                        {
                            var ex = new Exception($"DeviceTag and Value can`t be set at same time in Transaction config. DataPointParam ({bind.DataPointParam}) Transaction ({trConfig.Name})");
                            ex.Data.Add("field", "Name");
                            throw ex;
                        }

                        if ((dvTag != null && string.IsNullOrWhiteSpace(bind.Value))) 
                        {
                            var compatModes = (dvTag.Mode == Modes.ToOTM && dpParam.Mode == Modes.FromOTM) ||
                                            (dvTag.Mode == Modes.FromOTM && dpParam.Mode == Modes.ToOTM);
                            if (!compatModes)
                            {
                                var ex = new Exception($"Invalid modes between DataPointParam and DeviceTag in Transaction config (valid is in => out or out => in). DataPointParam ({bind.DataPointParam}) DeviceTag ({bind.DeviceTag}) Transaction ({trConfig.Name})");
                                ex.Data.Add("field", "Name");
                                throw ex;
                            }

                            if (dvTag.TypeCode != dpParam.TypeCode)
                            {
                                var ex = new Exception($"Invalid data type between DataPointParam and DeviceTag in Transaction config. DataPointParam ({bind.DataPointParam}) DeviceTag ({bind.DeviceTag}) Transaction ({trConfig.Name})");
                                ex.Data.Add("field", "Name");
                                throw ex;
                            }
                        }
                    }

<<<<<<< HEAD
                    var transaction = new Transaction(trConfig, devices[trConfig.SourceDeviceName], devices[trConfig.TargetDeviceName], dataPoints[trConfig.DataPointName], logger);
=======
                    var transaction = new Transaction(trConfig, devices[trConfig.DeviceName], dataPoints[trConfig.DataPointName], logger);
>>>>>>> b9eb390e7415b6851e61969baecd0f404858b5ce
                    transactions[trConfig.Name] = transaction;

                    logger.LogInformation($"Transaction {trConfig.Name}: Created");
                }

            return transactions;
        }
    }
}