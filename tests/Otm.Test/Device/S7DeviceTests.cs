using System;
using Xunit;
using Otm.Config;
using Otm.Device;
using Snap7;
using Moq;
using Otm.Logger;
using NLog;

namespace Otm.Test.DataPoint
{
    public class S7DeviceTests
    {
        [Fact]
        public void Update_Device_Tags()
        {
            // prepare
            var dvConfig = new DeviceConfig {
                Name = "plc01",
                Driver = "s7",
                Config = "host=127.0.0.1;rack=0;slot=0",
                Tags = new DeviceTagConfig[] {
                    new DeviceTagConfig {  
                        Name = "tag1",
                        Type = "int",
                        Mode = "in",
                        Address = "DB800.DW10",
                        Rate = 50                            
                    },
                    new DeviceTagConfig{
                        Name = "tag2",
                        Type = "int",
                        Mode = "out",
                        Address = "DB800.DW12",
                        Rate = 50
                    }
                }
            };

            var connected = false;
            var buffer = new byte[13];
            byte dw10 = 0;

            var clientMock = new Mock<IS7Client>();
            // on the first call conected will be False
            clientMock.SetupGet(x => x.Connected).Returns(true);

            clientMock.Setup(x => x.ConnectTo("127.0.01", 0, 0))
                       .Returns(0);

            clientMock.Setup(x => x.ErrorText(0)).Returns("No error");
            clientMock.Setup(x => x.DBRead(800, 0, 13, It.IsAny<byte[]>()))
                      .Callback<int, int, int, byte[]>((dbNumber, start, lenght, buf) => 
                      {
                          // change second byte of dw10
                           buf[11] = dw10;
                      });

            var factoryMock = new Mock<IS7ClientFactory>();
            factoryMock.Setup(x => x.CreateClient()).Returns(clientMock.Object);

            var loggerFactoryMock = new Mock<ILoggerFactory>();
            loggerFactoryMock.Setup(x => x.GetCurrentClassLogger()).Returns(new Mock<ILogger>().Object);
            var devPlc01 = new S7Device(dvConfig, new S7ClientFactory(), loggerFactoryMock.Object);

            devPlc01.UpdateTags();
            
            var tag2 = 0;
            // assign a function to be executed when tag value is changed
            devPlc01.OnTagChange("tag2", (str, value) => { tag2 = (int)value; });

            devPlc01.UpdateTags();

            Assert.Equal(0, tag2);

            // update tag value to 1
            dw10 = 1;

            devPlc01.UpdateTags();

            Assert.Equal(1, tag2);
        }
    }
}

